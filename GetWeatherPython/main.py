"""module docstring here"""
import os  # Miscellaneous operating system interfaces, https://docs.python.org/3/library/os.html
from datetime import datetime
# pprint = pretty print with indentation, good for JSON files so not everything is on the same line
from pprint import pprint  # so we can do pprint(stuff) instead of pprint.pprint(stuff)
from datetime import datetime, timezone
import pytz  # for timezones (get date and time from other countries)
import asyncio  # asynchronous input/output
import aiohttp  # fetch data from urls asynchronously (compared to import requests which is sync)
import json  # support for JSON files
import urllib  # converts strings to url format (needed in case of special characters)
import urllib.parse
import pymongo as pymongo  # for MongoDB support
import sys

import urllib.request
import ssl
ssl._create_default_https_context = ssl._create_unverified_context
response = urllib.request.urlopen('https://www.python.org')



"""Get environment variables"""
#dbUser = os.environ['DB_USER']
#dbPass = os.environ['DB_PASS']
#dbPassURL = urllib.parse.quote(dbPass)
#dbUser = 'hkteam1' #UK db
#dbPass = 'IUXsr2ZYKQuPu0Sj' #UKdb
dbUser = 'admin'
dbPass = 'admin'
dbPassURL = urllib.parse.quote(dbPass)
#weatherKey = os.environ['WEATHER_API_KEY']  # Fetch OpenWeatherMap API key from environment variables
weatherKey = 'b856d0ca9869a68dc5212d5cae9ced9b' 
"""End environment variables"""

"""Connect to MongoDB"""
#UKclient = pymongo.MongoClient(f'mongodb+srv://{dbUser}:{dbPassURL}'
#                            f'@issf2020hk-la5xb.gcp.mongodb.net/test?retryWrites=true&w=majority')

HKclient = pymongo.MongoClient(f'mongodb+srv://{dbUser}:{dbPassURL}'
                             f'@clustertest-kjhvv.azure.mongodb.net/test?retryWrites=true&w=majority')

#db = UKclient['mydb']
db = HKclient['FYP_1920']
weather_col = db['regional_weather']


async def regional_weather_data():
    """Fetches the data from the openweathermap.org api asynchronously in weather_resp"""
    while True:  # so the loop continues after one run; use this as long as async call is performed inside

        location = "Hong Kong"  # Used for the API call below
        wait_timer = 420  # Sets the amount of seconds between each loop, minimum 1 (or the API will refuse requests)
        loop_timer = 1  # Amount of seconds to wait after new data is stored

        async with aiohttp.ClientSession() as session:
            async with session.get(f'http://api.openweathermap.org/data/2.5/weather?q={location}&APPID={weatherKey}') \
                    as weather_req:

                weather_resp = json.loads(await weather_req.text())  # Converts the request data into JSON format
                celsius = round(weather_resp['main']['temp'] - 273.15, 2)  # converts the default Kelvin to Celsius °C

                """
                Saves the current time and date of Hong Kong using datetime and pytz library for timezone info.
                Then strftime is used to make the data more readable using the given properties,
                more info: https://docs.python.org/3/library/datetime.html#strftime-and-strptime-format-codes
                """
                location_datetime = datetime.now(pytz.timezone('Asia/Hong_Kong')).strftime('%X, %a %d %b %Y')
                location_date = datetime.now(pytz.timezone('Asia/Hong_Kong')).strftime('%Y-%m-%d, %A')
                location_time = datetime.now(pytz.timezone('Asia/Hong_Kong')).strftime('%X')

                """
                Dictionary in which we put the important stuff from weather_resp and filter out the static or
                unnecessary/redundant data (such as geolocation) - except location name
                """
                weather_data = {
                    'location': weather_resp['name'],
                    'date': location_date,
                    'time': location_time,
                    'temperature': celsius,
                    'description': weather_resp['weather'][0]['description'],
                    'wind': weather_resp['wind']['speed'],
                    'humidity': weather_resp['main']['humidity'],
                    'details': {
                        'temp_feels_like': round(weather_resp['main']['feels_like'] - 273.15, 2),
                        'temp_min': round(weather_resp['main']['temp_min'] - 273.15, 2),
                        'temp_max': round(weather_resp['main']['temp_max'] - 273.15, 2)
                    }
                }

                """
                Queries the most recent document within the same day
                Checks the latest temperature, if it's the same as the current one then do nothing and wait a bit.
                (It means the API data hasn't updated yet, and we want to avoid flooding the DB with redundant data.)
                If we get an IndexError, it probably means it couldn't find a document with today's date. 
                In that case, pass and let the code below create (and then update) one.
                Otherwise, continue to insert/update data (see below).
                """
                try:
                    last_doc_cursor = weather_col.find({'date': weather_data['date']}).sort([('_id', -1)]).limit(1)
                    last_doc = list(last_doc_cursor)  # Iterates through the PyMongo cursor and returns the data
                    last_temperature = last_doc[0]['temperature']

                    if last_temperature == celsius:
                        # print('Waiting for new API data...', end=" ")
                        # await asyncio.sleep(wait_timer)
                        """Print countdown of seconds left until next API call"""
                        for remaining in range(wait_timer, 0, -1):
                            sys.stdout.write("\r")  # print on same line (https://en.wikipedia.org/wiki/Carriage_return)
                            sys.stdout.write("Waiting for new API data: {:2d} second(s) until refresh."
                                             .format(remaining))
                            sys.stdout.flush()  # forces buffer "flush" so print statement is immediately executed
                            await asyncio.sleep(1)
                        sys.stdout.write("\r")
                        # print(f'Waited {wait_timer} second(s). {location} time: {location_time}')
                        continue
                except IndexError:
                    pass

                """
                Using Bucket Pattern, insert/update the database with the latest weather info.
                Creates a new document (record in SQL) and stores the data inside it.
                If a document with the same date already exists, updates its temperature and time of last update,
                then also adds the data in a history array (which contains all past weather information with details).
                If the maximum amount of records in a document has been reached, creates a new one (with the same date).
                
                """
                weather_col.update_one(
                    {  # filter: A query that matches the document to update.
                        'location': weather_data['location'],  # make sure the data is in the same location/city
                        'date': weather_data['date'],          # and the same day
                        'records': {'$lt': 1440}  # maximum record count (filter: records less than X)
                    },
                    {  # update: The modifications to apply.
                        '$set': {'temperature': celsius,  # updates the most important bits
                                 'last_update': location_time,
                                 'description': weather_resp['weather'][0]['description'],
                                 'icon': weather_resp['weather'][0]['icon'],
                                 'humidity': weather_resp['main']['humidity'],
                                 'wind':weather_resp['wind']['speed'],
                                 'details': {
                                     'temp_feels_like': round(
                                         weather_resp['main']['feels_like'] - 273.15, 2),
                                     'temp_min': round(weather_resp['main']['temp_min'] - 273.15, 2),
                                     'temp_max': round(weather_resp['main']['temp_max'] - 273.15, 2)
                                    }
                                 },
                        '$push': {  # pushes the info in the history array (with all the other data from the same day)
                            'history': {
                                'time': location_time,
                                'temperature': celsius,
                                'description': weather_resp['weather'][0]['description'],
                                'icon': weather_resp['weather'][0]['icon'],
                                'wind': weather_resp['wind']['speed'],
                                'humidity': weather_resp['main']['humidity'],
                                'details': {
                                    'temp_feels_like': round(
                                        weather_resp['main']['feels_like'] - 273.15, 2),
                                    'temp_min': round(weather_resp['main']['temp_min'] - 273.15, 2),
                                    'temp_max': round(weather_resp['main']['temp_max'] - 273.15, 2)
                                }
                            }},
                        "$inc": {"records": 1},  # increments the records by 1
                        "$setOnInsert": {"date": location_date}  # if a new document is inserted, set the date (once)
                    }, True)  # upsert (optional): If True, perform an insert if no documents match the filter.

                # print(datetime.now().strftime('%X'), end=" - ")  # print timestamp
                print('Stored regional data', end=": ")
                print(f'{weather_resp["name"]} @ {location_datetime} '
                      f'({celsius}°C, {weather_resp["weather"][0]["description"]})')
        await asyncio.sleep(loop_timer)


"""Start of asyncio event loop"""
loop = asyncio.get_event_loop()
try:
    asyncio.ensure_future(regional_weather_data())
    loop.run_forever()
except KeyboardInterrupt:
    pass
finally:
    print("Closing Loop")
    loop.close()
"""End of loop"""
