#include <IRremote.h>
#include "Ultrasonic.h"

Ultrasonic ultrasonic(12, 13);

#define PIN_PIR 3 //датчик движения

#define PIN_LED1 A0 //светодиод в коридоре
#define PIN_LED2 A1 //светодиод в ванне
#define PIN_LED3 A2 //светодиод рядом с детской
#define PIN_LED4 A3 //светодиод в туалете
#define PIN_LED5 A4 //светодиод на кухне

#define DIG_PIN_LED1 5 //светодиод в ванне
#define DIG_PIN_LED2 6 //светодиод рядом с детской
#define DIG_PIN_LED3 7 //светодиод в туалете

IRrecv irrecv(8); //ИК-приемник
decode_results results;

bool Light_vanna = false;
bool Light_Near_Childroom = false;
bool Light_childroom = false;
bool Light_kitchen = false;

//---------------------------------------------------------------------------------

void Dvish()
{
  boolean pirVal = digitalRead(PIN_PIR);

  //Если обнаружили движение
  if(pirVal == HIGH)
  {
    digitalWrite(PIN_LED1, HIGH);
  }
  else
  {
    digitalWrite(PIN_LED1,LOW);
  }
}

//----------------------------------------------------------------------------------

void ik_pult() // Ик-приемник ф-ция
{
  if (irrecv.decode(&results))
  {
    Serial.println(results.value);
    
    if (results.value == 2538093563)
    {
      if (Light_vanna == false)
      {
        digitalWrite(PIN_LED2, HIGH);
        Light_vanna = true;
        delay(150);
      }
      else
      {
        digitalWrite(PIN_LED2, LOW);
        Light_vanna = false;
        delay(150);
      }
    }

    if (results.value == 4039382595)
    {
      if (Light_Near_Childroom == false)
      {
        digitalWrite(PIN_LED3, HIGH);
        Light_Near_Childroom = true;
        delay(150);
      }
      else
      {
        digitalWrite(PIN_LED3, LOW);
        Light_Near_Childroom = false;
        delay(150);
      }
    }

    if (results.value == 3238126971)
    {
      if (Light_childroom == false)
      {
        digitalWrite(PIN_LED4, HIGH);
        Light_childroom = true;
        delay(150);
      }
      else
      {
        digitalWrite(PIN_LED4, LOW);
        Light_childroom = false;
        delay(150);
      }
    }

    if (results.value == 2534850111)
    {
      if (Light_kitchen == false)
      {
        digitalWrite(PIN_LED5, HIGH);
        Light_kitchen = true;
        delay(150);
      }
      else
      {
        digitalWrite(PIN_LED5, LOW);
        Light_kitchen = false;
        delay(150);
      }
    }
         
    irrecv.resume(); // Receive the next value
  }
  
}

/*------------------------------------------------------------*/

void Posled_vkl()
{
  float dist_cm = ultrasonic.Ranging(CM);       // get distance
  //Serial.println(dist_cm);                      // print the distance
  
  delay(100);                                   // arbitary wait time.  

  if(dist_cm >= 29)
  {
    delay(100);
    digitalWrite(DIG_PIN_LED1, LOW);
    digitalWrite(DIG_PIN_LED2, LOW);
    digitalWrite(DIG_PIN_LED3, LOW);
  }

  if(dist_cm >= 20 && dist_cm < 29)
  {
    delay(100);
    digitalWrite(DIG_PIN_LED3, HIGH);
    digitalWrite(DIG_PIN_LED1, LOW);
    digitalWrite(DIG_PIN_LED2, LOW);
  }
  
  if(dist_cm >= 9 && dist_cm <= 19)
  {
    delay(100);
    digitalWrite(DIG_PIN_LED2, HIGH);
    digitalWrite(DIG_PIN_LED1, LOW);
    digitalWrite(DIG_PIN_LED3, LOW);
  }

  if(dist_cm < 9)
  {
    delay(100);
    digitalWrite(DIG_PIN_LED1, HIGH);
    digitalWrite(DIG_PIN_LED2, LOW);
    digitalWrite(DIG_PIN_LED3, LOW);
  }

}

/*------------------------------------------------------------*/

void Get_data()
{
  if (Serial.available() > 0)
  {
    char incomingChar = Serial.read();

    switch (incomingChar) 
    {
      case 'q':
        digitalWrite(PIN_LED2, HIGH); 
        break;
      case 'w':
        digitalWrite(PIN_LED3, HIGH);
        break;
      case 'e':
        digitalWrite(PIN_LED4, HIGH); 
        break;
      case 'r':
        digitalWrite(PIN_LED5, HIGH); 
        break;    
      
      case 'a':
        digitalWrite(PIN_LED2, LOW); 
        break;
      case 's':
        digitalWrite(PIN_LED3, LOW);
        break;
      case 'd':
        digitalWrite(PIN_LED4, LOW); 
        break;
      case 'f':
        digitalWrite(PIN_LED5, LOW); 
        break; 
      case 'x':      
        digitalWrite(PIN_LED2, LOW); 
        digitalWrite(PIN_LED3, LOW); 
        digitalWrite(PIN_LED4, LOW); 
        digitalWrite(PIN_LED5, LOW);
        break;
      case 'z':
        digitalWrite(PIN_LED2, HIGH);
        digitalWrite(PIN_LED3, HIGH); 
        digitalWrite(PIN_LED4, HIGH); 
        digitalWrite(PIN_LED5, HIGH);        
        break;
   
    }   
  } 
}

/*------------------------------------------------------------*/

void setup()
{
  irrecv.enableIRIn();  // запускаем прием инфракрасного сигнала
  
  Serial.begin(9600);
  
  pinMode(PIN_PIR, INPUT); // Датчик движения
  
  pinMode(PIN_LED1, OUTPUT); // светодиод в коридоре
  pinMode(PIN_LED2, OUTPUT); // светодиод в ванне
  pinMode(PIN_LED3, OUTPUT); // светодиод рядом с детской
  pinMode(PIN_LED4, OUTPUT); // светодиод в детской
  pinMode(PIN_LED5, OUTPUT); // светодиод на кухне

  pinMode(DIG_PIN_LED1, OUTPUT); // светодиод за дверью
  pinMode(DIG_PIN_LED2, OUTPUT); // светодиод за дверью
  pinMode(DIG_PIN_LED3, OUTPUT); // светодиод за дверью

  pinMode(8, INPUT); //ИК-приемник
  
}

void loop()
{
  Dvish();
  ik_pult();
  Posled_vkl();
  Get_data();
  
}




