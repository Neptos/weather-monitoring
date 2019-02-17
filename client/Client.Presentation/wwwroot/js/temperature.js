"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5001/temperatureHub")
    .build();

connection.on("ReceiveTemperature", function (temperatureDto) {
    var temperatureDiv = document.getElementById(temperatureDto.sensorId);
    if(temperatureDiv === null || temperatureDiv === undefined) {
        var temperatureHolder = document.getElementById("container");

        temperatureDiv = document.createElement("div");
        temperatureDiv.id = temperatureDto.sensorId;
        temperatureDiv.style = "padding: 20px;"

        var locationName = document.createElement("h2");
        locationName.textContent = temperatureDto.locationName;
        temperatureDiv.appendChild(locationName);

        var timestamp = document.createElement("span");
        var datetime = new Date(temperatureDto.timestamp);
        var tzoffset = (new Date()).getTimezoneOffset();
        datetime.setMinutes(datetime.getMinutes() - tzoffset);
        timestamp.textContent = datetime.toLocaleString("en-GB");
        temperatureDiv.appendChild(timestamp);

        var temperature = document.createElement("h1");
        temperature.id = temperatureDto.sensorId + "temperature";
        temperatureDiv.appendChild(temperature);

        temperatureHolder.appendChild(temperatureDiv);
    }

    var temperature = document.getElementById(temperatureDto.sensorId + "temperature");
    var temperatureFloat = parseFloat(temperatureDto.value).toFixed(2);
    temperature.textContent = temperatureFloat.toString() + "°C";
});

connection.start().then(function(){
    connection.invoke("RetrieveTemperatures").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});
