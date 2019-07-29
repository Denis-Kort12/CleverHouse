//SDA  - 10
//SCK  - 13
//MOSI - 11
//MISO - 12
//IRO  - пусто
//GND  - GND
//RST  - 9
//3.3V - 3.3V

#include <SPI.h>
#include <MFRC522.h> // библиотека "RFID"
#include <Servo.h> //используем библиотеку для работы с сервоприводом
#include <Wire.h> //Датчик освещености
#include <BH1750FVI.h> //Датчик освещености
#include "DHT.h"

/*-------------------------------------------------------------------------------------*/
#define SS_PIN 10
#define RST_PIN 9

MFRC522 mfrc522(SS_PIN, RST_PIN);
unsigned long uidDec, uidDecTemp;  // для храниения номера метки в десятичном формате

/*-------------------------------------------------------------------------------------*/

Servo servo; //объявляем переменную servo типа Servo

/*-------------------------------------------------------------------------------------*/

BH1750FVI lightMeter; //Датчик освещености
int result;
int svetodiod = 3;
/*-------------------------------------------------------------------------------------*/

int analogMQ7 = A0; //Датчик дыма
int highLevel = 500;
int val = 0;

/*-------------------------------------------------------------------------------------*/

#define DHTPIN 7 // Датчик температуры
DHT dht(DHTPIN, DHT22);

/*-------------------------------------------------------------------------------------*/

void Card() //Ф-ция передает иникальный номер карты пользователя
{
    // Поиск новой метки
  if ( ! mfrc522.PICC_IsNewCardPresent())
  {
    return;
  }

  // Выбор метки
  if ( ! mfrc522.PICC_ReadCardSerial())
  {
    return;
  }

  uidDec = 0;

  // Выдача серийного номера метки.
  for (byte i = 0; i < mfrc522.uid.size; i++)
  {
    uidDecTemp = mfrc522.uid.uidByte[i];
    uidDec = uidDec * 256 + uidDecTemp;
  }

  /* Serial.println("Card UID: ");*/
  /*Serial.println(uidDec); // Выводим UID метки в консоль.*/
  if (uidDec == 2822276185 || uidDec == 1358052569) // Сравниваем Uid метки, если он равен заданому то реле вкл
  {       
    Serial.print(uidDec);
    Serial.print(" RFID\n");
    //delay(2000);    

    Open_door();
    delay(3000);
    Close_door();
  }

}

/*------------------------------------------------------------*/

void Open_door()
{
   servo.write(180); //ставим вал под 180     
}

void Close_door()
{
  servo.write(90); //ставим вал под 90
}

/*------------------------------------------------------------*/

void Osveshennost()
{
  uint16_t lux = lightMeter.readLightLevel();

  if(lux <= 2925)
  {
    result = lux/13;
    analogWrite(svetodiod, 225 - result); 
  }
  else
  {
    digitalWrite(svetodiod, LOW);
    delay(30);
  }
}

/*----------------------------------------------------------------*/

void Dim()
{
  val = analogRead(analogMQ7);
  
  if (val >= highLevel) // превышение уровня
  {
    Serial.print(val);
    Serial.print(" DIM\n");
  }
  else
  {
  }

delay(100);
}
/*----------------------------------------------------------------*/

void Temperature()
{
  //Считываем влажность
  float h = dht.readHumidity();
  
  // Считываем температуру
  float t = dht.readTemperature();
  
  // Проверка удачно прошло ли считывание.
  
  if (isnan(h) || isnan(t)) return;
  
  //Serial.println("Vlag: " + String(h) + " %\t"+"Temp: " + String(t) + " *C ");   
  Serial.println(String(h) + " " + String(t) + " Vlag");
  delay(100);
}

/*----------------------------------------------------------------*/

void Get_data()
{
  if (Serial.available() > 0)
  {
    char incomingChar = Serial.read();

    switch (incomingChar) 
    {
      case 't':
        Temperature();
        break;
      
    }   
  } 
}

/*----------------------------------------------------------------*/

void setup()
{ 
  Serial.begin(9600);
 
  SPI.begin();  //  инициализация SPI / Init SPI bus
  mfrc522.PCD_Init();     // инициализация MFRC522 / Init MFRC522 card.

  servo.attach(8); //привязываем привод к порту 8

  pinMode(A5, OUTPUT); //Датчик освещености
  pinMode(A4, OUTPUT); //Датчик освещености
  lightMeter.begin();
  pinMode(svetodiod, OUTPUT);
  
  dht.begin(); //Датчик температуры

}

void loop()
{
  Card(); //Передает иникальный номер карты пользователя
  Osveshennost();
  Dim();
  //Get_data();
  Temperature();
}
