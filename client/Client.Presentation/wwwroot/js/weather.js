"use strict";

function buildValueDiv(dataPointDto, startText, unit) {
    var valueDiv = document.createElement("h1");
    valueDiv.id = dataPointDto.sensorId + dataPointDto.type;
    valueDiv.textContent = startText + " " + dataPointDto.value + unit;
    return valueDiv;
}

var connection = new signalR.HubConnectionBuilder()
    .withUrl("http://weatherapi.mattiasberg.fi/dataPointHub")
    .build();

connection.on("ReceiveDataPoint", function (dataPointDto) {
    var dataPointDiv = document.getElementById(dataPointDto.sensorId);
    if (dataPointDiv === null || dataPointDiv === undefined) {
        var dataPointsHolder = document.getElementById("container");

        dataPointDiv = document.createElement("div");
        dataPointDiv.id = dataPointDto.sensorId;
        dataPointDiv.style = "padding: 20px;"

        var locationName = document.createElement("h2");
        locationName.textContent = dataPointDto.locationName;
        dataPointDiv.appendChild(locationName);

        var timestamp = document.createElement("span");
        timestamp.id = dataPointDto.sensorId + "timestamp";
        var datetime = new Date(dataPointDto.timestamp);
        var tzoffset = (new Date()).getTimezoneOffset();
        datetime.setMinutes(datetime.getMinutes() - tzoffset);
        timestamp.textContent = datetime.toLocaleString("en-GB");
        dataPointDiv.appendChild(timestamp);

        switch (dataPointDto.type) {
            case "Temperature":
                var temperature = buildValueDiv(dataPointDto, "Temperature", "°C");
                dataPointDiv.appendChild(temperature);
                break;
            case "RelativeHumidity":
                var humidity = buildValueDiv(dataPointDto, "Humidity", "%");
                dataPointDiv.appendChild(humidity);
                break;
            case "Pressure":
                var pressure = buildValueDiv(dataPointDto, "Pressure", "hPa");
                dataPointDiv.appendChild(pressure);
                break;
            case "WindSpeed":
                var windSpeed = buildValueDiv(dataPointDto, "Wind speed", "m/s");
                dataPointDiv.appendChild(windSpeed);
                break;
            case "WindDirection":
                var windDirection = buildValueDiv(dataPointDto, "Wind direction", "°");
                dataPointDiv.appendChild(windDirection);
                break;
            case "DewPoint":
                var dewPoint = buildValueDiv(dataPointDto, "Dew point", "°C");
                dataPointDiv.appendChild(dewPoint);
                break;
            case "Precipitation":
                var precipitation = buildValueDiv(dataPointDto, "Precipitation", "mm");
                dataPointDiv.appendChild(precipitation);
                break;
            case "Visibility":
                var visibility = buildValueDiv(dataPointDto, "Visibility", "m");
                dataPointDiv.appendChild(visibility);
                break;
        }

        dataPointsHolder.appendChild(dataPointDiv);
        return;
    }

    switch (dataPointDto.type) {
        case "Temperature":
            var temperature = document.getElementById(dataPointDto.sensorId + dataPointDto.type);
            if (temperature == undefined) {
                var temperature = buildValueDiv(dataPointDto, "Temperature", "°C");
                dataPointDiv.appendChild(temperature);
            }
            else {
                temperature.textContent = "Temperature " + dataPointDto.value + "°C";
            }
            break;
        case "RelativeHumidity":
            var humidity = document.getElementById(dataPointDto.sensorId + dataPointDto.type);
            if (humidity == undefined) {
                var humidity = buildValueDiv(dataPointDto, "Humidity", "%");
                dataPointDiv.appendChild(humidity);
            }
            else {
                humidity.textContent = "Humidity " + dataPointDto.value + "%";
            }
            break;
        case "Pressure":
            var pressure = document.getElementById(dataPointDto.sensorId + dataPointDto.type);
            if (pressure == undefined) {
                var pressure = buildValueDiv(dataPointDto, "Pressure", "hPa");
                dataPointDiv.appendChild(pressure);
            }
            else {
                pressure.textContent = "Pressure " + dataPointDto.value + "hPa";
            }
            break;
        case "WindSpeed":
            var windSpeed = document.getElementById(dataPointDto.sensorId + dataPointDto.type);
            if (windSpeed == undefined) {
                var windSpeed = buildValueDiv(dataPointDto, "Wind speed", "m/s");
                dataPointDiv.appendChild(windSpeed);
            }
            else {
                windSpeed.textContent = "Wind speed " + dataPointDto.value + "m/s";
            }
            break;
        case "WindDirection":
            var windDirection = document.getElementById(dataPointDto.sensorId + dataPointDto.type);
            if (windDirection == undefined) {
                var windDirection = buildValueDiv(dataPointDto, "Wind direction", "°");
                dataPointDiv.appendChild(windDirection);
            }
            else {
                windDirection.textContent = "Wind direction " + dataPointDto.value + "°";
            }
            break;
        case "DewPoint":
            var dewPoint = document.getElementById(dataPointDto.sensorId + dataPointDto.type);
            if (dewPoint == undefined) {
                var dewPoint = buildValueDiv(dataPointDto, "Dew point", "°C");
                dataPointDiv.appendChild(dewPoint);
            }
            else {
                dewPoint.textContent = "Dew point " + dataPointDto.value + "°C";
            }
            break;
        case "Precipitation":
            var precipitation = document.getElementById(dataPointDto.sensorId + dataPointDto.type);
            if (precipitation == undefined) {
                var precipitation = buildValueDiv(dataPointDto, "Precipitation", "mm");
                dataPointDiv.appendChild(precipitation);
            }
            else {
                precipitation.textContent = "Precipitation " + dataPointDto.value + "mm";
            }
            break;
        case "Visibility":
            var visibility = document.getElementById(dataPointDto.sensorId + dataPointDto.type);
            if (visibility == undefined) {
                var visibility = buildValueDiv(dataPointDto, "Visibility", "m");
                dataPointDiv.appendChild(visibility);
            }
            else {
                visibility.textContent = "Visibility " + dataPointDto.value + "m";
            }
            break;
    }

    var timestamp = document.getElementById(dataPointDto.sensorId + "timestamp");
    var datetime = new Date(dataPointDto.timestamp);
    timestamp.textContent = datetime.toLocaleString("en-GB");
});

connection.start().then(function () {
    connection.invoke("RetrieveDataPoints").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});
