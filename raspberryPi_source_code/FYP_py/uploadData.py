#!/usr/bin/env python3
import serial
import time
from datetime import datetime
import http.client
import json
import sys
import requests
import Seeed_AMG8833
import os
import picamera
import base64
import subprocess
from datetime import datetime
import json
from board import SCL, SDA
import busio
from PIL import Image, ImageDraw, ImageFont
import adafruit_ssd1306

arduino=serial.Serial('/dev/ttyUSB0', baudrate = 9600, timeout= 1)
time.sleep(3)
body=""
numPoints = 1
dataList = [0] * numPoints
headers = {'Accept-Charset': 'utf-8','Content-Type': 'application/json'}

URL = "http://47.56.85.130:18888/uimg"
PARAMS = {'n': 'f348'}

os.putenv('SDL_FBDEV', '/dev/fb1')
sensor = Seeed_AMG8833.AMG8833()

logfile = open('updatedata.log', 'a')
timestamp = datetime.now()
print("[" + timestamp.strftime('%d-%b-%Y %H:%M:%S') + "] Sensing data start.", file = logfile)
logfile.close()
camera = picamera.PiCamera()

i2c = busio.I2C(SCL, SDA)
disp = adafruit_ssd1306.SSD1306_I2C(128, 64, i2c)
disp.fill(0)
disp.show()
width = disp.width
height = disp.height
image = Image.new("1", (width, height))
draw = ImageDraw.Draw(image)
draw.rectangle((0, 0, width, height), outline=0, fill=0)
padding = -2
top = padding
bottom = height - padding
x = 0
font = ImageFont.truetype('/usr/share/fonts/truetype/Pixellari.ttf', 16)



def getValue():
    
    #read serial
    arduinoData = arduino.readline().decode("ascii").split('\r\n')
    #print(arduinoData[0])
    return arduinoData[0]

def uploadData(thData):
    conn = http.client.HTTPConnection('13.75.55.240')
    try:
        conn.request('POST','/api/SensorUpdateCurrentValue', thData, headers)
        res = conn.getresponse()
        thData = res.read()
        print(res.status, res.reason)
        print(thData.decode('utf-8'))
        print()
        print(res.getheaders())
        logfile = open('updatedata.log', 'a')
        timestamp = datetime.now()
        print("[" + timestamp.strftime('%d-%b-%Y %H:%M:%S') + "] Data upload successfully", file = logfile)
        logfile.close()
        print("Data uploaded")
    except:
        logfile = open('updatedata.log', 'a')
        timestamp = datetime.now()
        print("[" + timestamp.strftime('%d-%b-%Y %H:%M:%S') + "] Data failed to upload", file = logfile)
        logfile.close()

def uploadIRThermalPhoto():
    pixels = sensor.read_temp()
    print(pixels)
    r=requests.post(url=URL, json=pixels, params=PARAMS)
    print(r)
    print("IR Cam Data uploaded")

def uploadPiCameraPhoto():
    
    time.sleep(2)    # Camera warm-up time
    camera.capture('piCamPic.jpg')
    
    with open("piCamPic.jpg", "rb") as img_file:
        my_string = base64.b64encode(img_file.read())
        print("base64: ")
        print(my_string)
    
    
    conn = http.client.HTTPConnection('13.75.55.240')
    camData = "{" + '"' + "deviceId" + '"' + ":" + '"CA1"' + ", " + '"' + "base_cam_img" + '"' + ":" + '"' + str(my_string) + '"' + ", " + '"' + "roomId" + '"' + ":" + '"' + "F348" + '"' + "}"
    print(camData)
    
    try:
        conn.request('POST','/api/Picam', camData, headers)
        res = conn.getresponse()
        camthData = res.read()
        print(res.status, res.reason)
        print(camthData.decode('utf-8'))
        print()
        print(res.getheaders())
        print("Pi Cam Data uploaded")
    except:
        print("failed!!!!!!!!")
        
        
        
def getACData():
    conn = http.client.HTTPConnection('13.75.55.240')
    try:
        conn.request('GET','/api/getDeviceStatus/AC011', "", headers)
        res = conn.getresponse()
        thedata = res.read()
        print("devices: ",thedata,res.status, res.reason)
        print(thedata.decode('utf-8'))
        print()
        print(res.getheaders())
        return thedata
    except:
        print()
        
    
    
while (1):
    
    data = getValue()
    print(data)
    time.sleep(1)
    uploadData(data)
    uploadIRThermalPhoto()
    #uploadPiCameraPhoto()
    
    jsonDevice = getACData()
    
    deviceData = json.loads(jsonDevice)
    getDeviceID = deviceData["deviceId"]
    getDeviceStatus = deviceData["status"]
    getDeviceSetValue = deviceData["set_value"]
    #print(getDeviceID, ", ", getDeviceStatus, ", ", getDeviceSetValue)
    # Draw a black filled box to clear the image.
    draw.rectangle((0, 0, width, height), outline=0, fill=0)
    
    ACID =  getDeviceID #get from API
    TempNumber = getDeviceSetValue #get from API
    C = "C"
    Temp = str(TempNumber) + C
    if getDeviceStatus == "True":
        onOff = "On"
    else:
        onOff = "Off"
        
    dateTime = datetime.now()
    dataStr = "Time: " + dateTime.strftime("%H:%M")
   
    draw.text((x, top + 12), "AC: " + ACID, font=font, fill=255)
    draw.text((x, top + 24), "Temp: " + Temp, font=font, fill=255)
    draw.text((x, top + 36), "Status: " + onOff, font=font, fill=255)
    draw.text((x, top + 48), dataStr, font=font, fill=255)

    # Display image.
    disp.image(image)
    disp.show()
    time.sleep(0.1)
    time.sleep(3)
    
    
    

time.sleep(3)
time.sleep(2)