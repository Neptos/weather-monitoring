import requests
import os
import time
import datetime
import GracefulInterruptHandler as gih
from ruuvitag_sensor.ruuvi import RuuviTagSensor

ruuvi_tag_list = os.environ['RUUVI_TAG_LIST'].split(",")
ruuvi_mac_list = []
ruuvi_name_list = {}
ingester_endpoint = os.environ['INGESTER_ENDPOINT']
data_points = {}

def GetRawData(data_points, mac, data_type):
    data = []
    for data_point in data_points[mac]:
        data.append(data_point[data_type])
    return data

def AggregateDataPoints():
    global data_points
    averages = {}

    for mac in ruuvi_mac_list:
        if mac in data_points:
            averages[mac] = {}
            averages[mac]["humidity"] = sum(GetRawData(data_points, mac, "humidity"))/len(data_points[mac])
            averages[mac]["temperature"] = sum(GetRawData(data_points, mac, "temperature"))/len(data_points[mac])
            averages[mac]["pressure"] = sum(GetRawData(data_points, mac, "pressure"))/len(data_points[mac])

    data_points = {}

    return averages

def SendAggregateDataToIngester():
    averages = AggregateDataPoints()
    for mac in ruuvi_mac_list:
        requests.post(ingester_endpoint + "api/Temperature", json={'Value': averages[mac]["temperature"], 'Timestamp': datetime.datetime.now().strftime('%Y-%m-%dT%H:%M:%SZ'), 'SensorId': mac, 'Location': ruuvi_name_list[mac]})
        requests.post(ingester_endpoint + "api/RelativeHumidity", json={'Value': averages[mac]["humidity"], 'Timestamp': datetime.datetime.now().strftime('%Y-%m-%dT%H:%M:%SZ'), 'SensorId': mac, 'Location': ruuvi_name_list[mac]})
        requests.post(ingester_endpoint + "api/Pressure", json={'Value': averages[mac]["pressure"], 'Timestamp': datetime.datetime.now().strftime('%Y-%m-%dT%H:%M:%SZ'), 'SensorId': mac, 'Location': ruuvi_name_list[mac]})

def AddDataPoints(data_received):
    global data_points
    for mac in ruuvi_mac_list:
        if mac not in data_points:
            data_points[mac] = []
        if mac in data_received:
            data_points[mac].append(data_received[mac])

for ruuvi_tag in ruuvi_tag_list:
    mac = ruuvi_tag.split(";")[1]
    name = ruuvi_tag.split(";")[0]
    ruuvi_mac_list.append(mac)
    ruuvi_name_list[mac] = name

with gih.GracefulInterruptHandler() as hander:
    while True:
        for x in range(30): # 5minutes
            data = RuuviTagSensor.get_data_for_sensors(ruuvi_tag_list, 4)
            AddDataPoints(data)
            time.sleep(10)
        SendAggregateDataToIngester()
