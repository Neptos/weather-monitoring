import requests
import os
import time
import datetime
import GracefulInterruptHandler as gih

sensor_endpoint = os.environ['SENSOR_ENDPOINT']
ingester_endpoint = os.environ['INGESTER_ENDPOINT']
data_points = []

def AggregateDataPoints():
    global data_points
    total_sum = sum(data_points)
    aggregate_average = total_sum/len(data_points)
    data_points = []
    return aggregate_average

def SendAggregateDataToIngester():
    average_temp = AggregateDataPoints()
    requests.post(ingester_endpoint + "api/Temperature", json={'Value': average_temp, 'Timestamp': datetime.datetime.now().strftime('%Y-%m-%dT%H:%M:%SZ'), 'SensorId': '64732521-eb10-4a23-8345-0817504ba627', 'Location': 'Vardagsrum'})

def AddDataPoint():
    temperature = requests.get(sensor_endpoint).text
    data_points.append(float(temperature))

with gih.GracefulInterruptHandler() as hander:
    while True:
        for x in range(30): # 5minutes
            AddDataPoint()
            time.sleep(10)
        SendAggregateDataToIngester()
