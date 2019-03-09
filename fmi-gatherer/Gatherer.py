import os
import requests
import time
from owslib.wfs import WebFeatureService
import xml.etree.ElementTree as ET
import GracefulInterruptHandler as gih

ingester_endpoint = os.environ['INGESTER_ENDPOINT']
last_timestamp = ''

def SendDataToIngester(value, time, measurementType):
    result = requests.post(ingester_endpoint + "api/" + measurementType, json={'Value': value, 'Timestamp': time, 'SensorId': '0778f7bf-881f-4814-b939-ccd580aa5ddf', 'Location': 'Vaasa'})

def AddDataPoint():
    global last_timestamp
    wfs = WebFeatureService(url='http://opendata.fmi.fi/wfs', version='2.0.0')
    response = wfs.getfeature(storedQueryID='fmi::observations::weather::simple', storedQueryParams={'Place':'vaasa'})
    xml = response.read()
    root = ET.fromstring(xml)
    for latestData in root[-11:]:
        for member in latestData:
            memberKey = member[2].text
            memberValue = member[3].text
            time = member[1].text
            if memberKey == 't2m':
                temperature = memberValue
            elif memberKey == 'ws_10min':
                windSpeed = memberValue
            elif memberKey == 'wg_10min':
                windGust = memberValue
            elif memberKey == 'wd_10min':
                windDirection = memberValue
            elif memberKey == 'rh':
                relativeHumidity = memberValue
            elif memberKey == 'td':
                dewPoint = memberValue
            elif memberKey == 'r_1h':
                precipitationAmount = memberValue
            elif memberKey == 'snow_aws':
                snowDepth = memberValue
            elif memberKey == 'p_sea':
                pressure = memberValue
            elif memberKey == 'vis':
                visibility = memberValue
    
    if time != last_timestamp:
        last_timestamp = time
        SendDataToIngester(temperature, time, "Temperature")
        SendDataToIngester(windSpeed, time, "WindSpeed")
        SendDataToIngester(windGust, time, "WindGust")
        SendDataToIngester(windDirection, time, "WindDirection")
        SendDataToIngester(relativeHumidity, time, "RelativeHumidity")
        SendDataToIngester(dewPoint, time, "DewPoint")
        SendDataToIngester(precipitationAmount, time, "Precipitation")
        SendDataToIngester(pressure, time, "Pressure")
        SendDataToIngester(visibility, time, "Visibility")

with gih.GracefulInterruptHandler() as hander:
    while True:
        AddDataPoint()
        time.sleep(120)
