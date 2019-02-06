import os
import requests
import time
from owslib.wfs import WebFeatureService
import xml.etree.ElementTree as ET
import GracefulInterruptHandler as gih

ingester_endpoint = os.environ['INGESTER_ENDPOINT']
last_timestamp = ''

def SendDataToIngester(temperature, time):
    result = requests.post(ingester_endpoint + "api/Temperature", json={'Value': temperature, 'Timestamp': time, 'SensorId': '0778f7bf-881f-4814-b939-ccd580aa5ddf', 'Location': 'Vasa'})

def AddDataPoint():
    global last_timestamp
    wfs = WebFeatureService(url='http://opendata.fmi.fi/wfs', version='2.0.0')
    response = wfs.getfeature(storedQueryID='fmi::observations::weather::simple', storedQueryParams={'Place':'vaasa'})
    xml = response.read()
    root = ET.fromstring(xml)
    for member in root[-11:]:
        if member[0][2].text == 't2m':
            temperature = member[0][3].text
            time = member[0][1].text
    
    if time != last_timestamp:
        last_timestamp = time
        SendDataToIngester(temperature, time)

with gih.GracefulInterruptHandler() as hander:
    while True:
        AddDataPoint()
        time.sleep(120)
