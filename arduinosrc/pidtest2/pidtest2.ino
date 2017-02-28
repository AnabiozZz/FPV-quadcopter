#include "freeram.h"
#include <PID_v1.h>
#include "mpu.h"
#include "I2Cdev.h"
#include <Servo.h> 

Servo MotorSE;
Servo MotorSW;
Servo MotorNE;
Servo MotorNW;
double Pitch = 0, Roll = 0, Power;

byte cmd,value,focus,run = 0;
 

double Setpoint = 0, Input = 0, Output = 0, Kp = 0, Ki = 0, Kd = 0;
PID ROTATION(&Input, &Output, &Setpoint, Kp, Ki, Kd, DIRECT);


float midvalueP[3] = {
  0,0,0};
float midvalueR[3] = {
  0,0,0};
int ret, ping = 0;


void setup() {
  MotorSE.attach(8);
  MotorSW.attach(9);
  MotorNE.attach(10);
  MotorNW.attach(11);

  MotorSE.writeMicroseconds(1000);
  MotorSW.writeMicroseconds(1000);
  MotorNE.writeMicroseconds(1000);
  MotorNW.writeMicroseconds(1000);

  Fastwire::setup(400,0);
  Serial1.begin(9600);
  ret = mympu_open(200);
  Setpoint = 0;
  ROTATION.SetMode(AUTOMATIC);

  while (!Serial1) {
    ;
  }

}

unsigned int c = 0; //cumulative number of successful MPU/DMP reads
unsigned int np = 0; //cumulative number of MPU/DMP reads that brought no packet back
unsigned int err_c = 0; //cumulative number of MPU/DMP reads that brought corrupted packet
unsigned int err_o = 0; //cumulative number of MPU/DMP reads that had overflow bit set
float truevalueP;
float truevalueR;

void loop() {

  if (Serial1.available() == 2)  
  {  

    cmd = Serial1.read();
    value = Serial1.read();
    ping = 0;
    //---------------------------------
    switch(cmd){

    case 49:
      run = 1;
      break;

    case 48:
      run = 0;
      MotorSE.writeMicroseconds(1000);
      MotorSW.writeMicroseconds(1000);
      MotorNE.writeMicroseconds(1000);
      MotorNW.writeMicroseconds(1000);
      break;

    case 50:
      MotorSE.writeMicroseconds(1200);
      MotorSW.writeMicroseconds(1200);
      MotorNE.writeMicroseconds(1200);
      MotorNW.writeMicroseconds(1200);
      break;
      //---------------------------------
    case 51:
      focus = 1;
      break;
    case 52:
      focus = 2;
      break;
    case 53:
      focus = 3;
      break;
      //---------------------------------
    case 54:
      switch (focus){
      case 1:
        Kp += 0.05;
        break;
      case 2:
        Ki += 0.005;
        break;        
      case 3:
        Kd += 0.05;
        break;
      }
      break;
      //---------------------------------
    case 55:
      Pitch = value;
      break;
    case 56:
      Roll = value;
      break;
    case 57:
      Power = 1200 + (value*2);
      break;
      //---------------------------------

    }
  }else 
  {
    ping = millis() - ping;
    if (ping > 500)
  {
    Pitch = 0;
    Roll = 0;
  }
  ping = millis();
  }

  ret = mympu_update();
  switch (ret) {
  case 0: 
    c++; 
    break;
  case 1: 
    np++; 
    return;
  case 2: 
    err_o++; 
    return;
  case 3: 
    err_c++; 
    return; 
  default: 
    return;
  }


  if (run == 1)
  {
    if (!(c%25)) {
      for (int i = 0; i < 2; i++)
      {
        midvalueP[i] = midvalueP[i+1];
        midvalueR[i] = midvalueR[i+1];

        truevalueP += midvalueP[i];
        truevalueR += midvalueR[i];
      }
      midvalueP[2] = mympu.ypr[1];
      midvalueR[2] = mympu.ypr[2];
      truevalueP = (truevalueP + midvalueP[2]) / 3.0;
      truevalueR = (truevalueR + midvalueR[2]) / 3.0;


      Input = truevalueP;
      Setpoint = Pitch;    
      ROTATION.Compute();


      MotorSE.writeMicroseconds(Power+Output);
      MotorSW.writeMicroseconds(Power+Output);
      MotorNE.writeMicroseconds(Power-Output);
      MotorNW.writeMicroseconds(Power-Output);



      Input = truevalueR;
      Setpoint = Roll;
      ROTATION.Compute();


      MotorSE.writeMicroseconds(Power+Output);
      MotorSW.writeMicroseconds(Power-Output);
      MotorNE.writeMicroseconds(Power+Output);
      MotorNW.writeMicroseconds(Power-Output);


      truevalueP = 0;
      truevalueR = 0;
    }
  }
}





