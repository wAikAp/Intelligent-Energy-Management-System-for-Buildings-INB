#include <dht.h>


//#include <ArduinoJson.h>

#include <DallasTemperature.h>

#include <OneWire.h>

#include <Wire.h>

#include <LiquidCrystal.h>

#define ONE_WIRE_BUS 7

OneWire oneWire(ONE_WIRE_BUS);
DallasTemperature sensors(&oneWire);
dht DHT;
const int DHT11_PIN = 8;
const int photocellPin = A0;
const int ledPin = 13;
const int relayPin = 9;
int outputValue = 0;
const int latchPin = 10;
const int clockPin = 12;
const int dataPin = 11;
int checkLight[3] = {
  0,0,0};
int num = 0;
int sensorValue;
const int rs = 12, en = 11, d4 = 5, d5 = 4, d6 = 3, d7 = 2;
LiquidCrystal lcd(rs,en,d4,d5,d6,d7);


//int dataArray[8] = {1,1,1,1,1,1,1,1};
////////////////////////////////////////////////////////////////////
void putOnOff(int number){
  digitalWrite(clockPin, LOW);
  digitalWrite(dataPin, number);
  digitalWrite(clockPin, HIGH);
}


double calPM25(int ppm){
  double mgm3 =0;
  double allUG[13];
  double allMolecular[13] = {
    17.03, 28.01, 44.01, 70.9, 30.026, 2.02, 16.04, 34.08, 46.01, 48, 165.822, 64.06, 78.9516      };
  String allMolecularFullName[13] = {
    "Ammonia[NH3]", "Carbon monoxide[CO]", "Carbon dioxide[CO2]", "Chlorine[CI2]", "Formaldehyde[CH2O]", "Hydrogen[H2]", "Methane[CH4]", "Hydrogen Sulfide[H2S]", "Nitrogen Dioxide[NO2]", "Ozone[O3]", "Perchoroethylene[C2CI4]", "Sulfur dioxide[SO2]", "VOC"      };
  String allMolecularName[13] = {
    "C_NH3:", "C_CO:", "C_CO2:", "C_CI2:", "C_CH2O:", "C_H2:", "C_CH4:", "C_H2S:", "C_NO2:", "C_O3:", "C_C2CI4:", "C_SO2:", "C_VOC:"      };

  for(int k=0; k<13;k++){
    mgm3 = ppm * 0.0409 * allMolecular[k];
    allUG[k] = mgm3 / 1000 ;
    //Serial.println(mgm3);
  }
  double sum = 0; 
  for(int j=0;j<13; j++){
    sum += allUG[j];
  }

  Serial.print("{");
  Serial.print('"');
  Serial.print("sensorId");
  Serial.print('"');
  Serial.print(":");
  Serial.print('"');
  Serial.print("AS001");
  Serial.print('"');
  Serial.print(",");
  Serial.print('"');
  Serial.print("values");
  Serial.print('"');
  Serial.print(":");
  Serial.print("[");

  for(int g=0;g<13;g++){
    Serial.print(allUG[g]);
    Serial.print(",");
  }
  Serial.print((double)sum/13);
  Serial.print("]},");

  return ((double)sum/13);
}

////////////////////////////////////////////////////////////////
void setup() {
  // put your setup code here, to run once:
  pinMode(relayPin, OUTPUT);
  pinMode(ledPin, OUTPUT);
  pinMode(latchPin, OUTPUT);
  pinMode(clockPin, OUTPUT);
  pinMode(dataPin, OUTPUT);

  Serial.begin(9600);
  while (!Serial) continue;
  sensors.begin();
}

void loop() {
    Serial.print("[");
    sensorValue = analogRead(5);
    //Serial.print("AirQua=");
    //Serial.print(sensorValue, DEC);
    //Serial.println(" PPM");
    double TotalPM = calPM25(sensorValue);


    int dataArray[8] = {
      1,1,1,1,1,1,1,1        };
    // put your main code here, to run repeatedly:
    outputValue = analogRead(photocellPin);
    //Serial.println("checkLS001");
    //Serial.println(outputValue);
    //Serial.println(num);


    checkLight[num] = outputValue; 
    num = num+1;
    num = num%3;
    //Serial.println(checkLight[0]);
    //Serial.println(checkLight[1]);
    //Serial.println(checkLight[2]);
    if((checkLight[0]-checkLight[1]) > 150 || (checkLight[0]-checkLight[1]) > 150 ||(checkLight[1]-checkLight[2]) > 150 ||(checkLight[1]-checkLight[2]) > 150 ){
      outputValue = -1;
    }  

    Serial.print("{");
    Serial.print('"');
    Serial.print("sensorId");
    Serial.print('"');
    Serial.print(":");
    Serial.print('"');
    Serial.print("LS001");
    Serial.print('"');
    Serial.print(",");
    Serial.print('"');
    Serial.print("values");
    Serial.print('"');
    Serial.print(":");
    Serial.print("[");  
    Serial.print(outputValue);
    Serial.print("]},");


    if(outputValue <= 0){
      dataArray[0] = 0;
      dataArray[1] = 0;
      dataArray[2] = 0;
      dataArray[3] = 0;
      dataArray[4] = 0;
      dataArray[5] = 0;
      dataArray[6] = 0;
      dataArray[7] = 0;
      //Serial.println("check 1"); 
    }
    else if(outputValue <= 200){ //not dark
      //dataArray = {0,0,1,0,0,1,0,1};
      dataArray[0] = 0;
      dataArray[1] = 0;
      dataArray[2] = 1;
      dataArray[3] = 0;
      dataArray[4] = 0;
      dataArray[5] = 1;
      dataArray[6] = 0;
      dataArray[7] = 1;
      //Serial.println("check 2"); 
    }
    else if (outputValue <= 400) { //little dark
      //dataArray = {1,0,1,1,0,1,0,1};
      dataArray[0] = 1;
      dataArray[1] = 1;
      dataArray[2] = 1;
      dataArray[3] = 1;
      dataArray[4] = 0;
      dataArray[5] = 0;
      dataArray[6] = 1;
      dataArray[7] = 1;
      //Serial.println("check 3"); 
    } 
    else if(outputValue <=600){//too dark
      //dataArray = {1,1,1,1,1,1,1,1};
      dataArray[0] = 1;
      dataArray[1] = 1;
      dataArray[2] = 1;
      dataArray[3] = 1;
      dataArray[4] = 1;
      dataArray[5] = 1;
      dataArray[6] = 1;
      dataArray[7] = 1;
      //Serial.println("check 4"); 
    }
    else{
      //dataArray = {0,0,0,0,0,0,0,0};
      dataArray[0] = 1;
      dataArray[1] = 1;
      dataArray[2] = 1;
      dataArray[3] = 1;
      dataArray[4] = 1;
      dataArray[5] = 1;
      dataArray[6] = 1;
      dataArray[7] = 1;
      //Serial.println("check 5"); 
    }


    //Humidity
    int chk = DHT.read11(DHT11_PIN);

    Serial.print("{");
    Serial.print('"');
    Serial.print("sensorId");
    Serial.print('"');
    Serial.print(":");
    Serial.print('"');
    Serial.print("HS001");
    Serial.print('"');
    Serial.print(",");
    Serial.print('"');
    Serial.print("values");
    Serial.print('"');
    Serial.print(":");
    Serial.print("[");  
    Serial.print(DHT.humidity, 1);
    Serial.print("]},");

    //Temperatures
    sensors.requestTemperatures();

    Serial.print("{");
    Serial.print('"');
    Serial.print("sensorId");
    Serial.print('"');
    Serial.print(":");
    Serial.print('"');
    Serial.print("TS001");
    Serial.print('"');
    Serial.print(",");
    Serial.print('"');
    Serial.print("values");
    Serial.print('"');
    Serial.print(":");
    Serial.print("[");  
    Serial.print(sensors.getTempCByIndex(0));
    Serial.print("]},");

    Serial.print("{");
    Serial.print('"');
    Serial.print("sensorId");
    Serial.print('"');
    Serial.print(":");
    Serial.print('"');
    Serial.print("TS002");
    Serial.print('"');
    Serial.print(",");
    Serial.print('"');
    Serial.print("values");
    Serial.print('"');
    Serial.print(":");
    Serial.print("[");  
    Serial.print(sensors.getTempCByIndex(1));
    Serial.print("]},");

    Serial.print("{");
    Serial.print('"');
    Serial.print("sensorId");
    Serial.print('"');
    Serial.print(":");
    Serial.print('"');
    Serial.print("TS003");
    Serial.print('"');
    Serial.print(",");
    Serial.print('"');
    Serial.print("values");
    Serial.print('"');
    Serial.print(":");
    Serial.print("[");  
    Serial.print(sensors.getTempCByIndex(2));
    Serial.print("]}");


    digitalWrite(latchPin, LOW);

    for(int i=0; i<8; i++){
      putOnOff(dataArray[i]);
      //Serial.println(dataArray[i]);
    }

    digitalWrite(latchPin, HIGH);
    Serial.println("]");



  
  delay(5000);
}















