import Seeed_AMG8833
import os
import time
import requests

URL = "http://47.56.85.130:18888/uimg"
PARAMS = {'n': 'f348'}

os.putenv('SDL_FBDEV', '/dev/fb1')
sensor = Seeed_AMG8833.AMG8833()

time.sleep(.1)

    
while(1):

    #read the pixels
    pixels = sensor.read_temp()
    print(pixels)    
    r=requests.post(url=URL, json=pixels, params=PARAMS)
    print(r)
    time.sleep(2)

    
    

