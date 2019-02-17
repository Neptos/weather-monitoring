"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5001/temperatureHub")
    .build();

connection.on("ReceiveTemperature", function (temperatureDto) {
    var temperatureDiv = document.getElementById(temperatureDto.locationId);
    if(temperatureDiv === null || temperatureDiv === undefined) {
        var temperatureHolder = document.getElementById("container");

        temperatureDiv = document.createElement("div");
        temperatureDiv.id = temperatureDto.locationId;
        temperatureDiv.style = "padding: 20px;"

        var locationName = document.createElement("h1");
        locationName.textContent = temperatureDto.locationName;
        temperatureDiv.appendChild(locationName);

        var timestamp = document.createElement("span");
        var datetime = new Date(temperatureDto.timestamp);
        var tzoffset = (new Date()).getTimezoneOffset();
        datetime.setMinutes(datetime.getMinutes() - tzoffset);
        timestamp.textContent = datetime.toLocaleString("en-GB");
        temperatureDiv.appendChild(timestamp);

        var temperature = document.createElement("h3");
        temperature.id = temperatureDto.locationId + "temperature";
        temperature.textContent = temperatureDto.value;
        temperatureDiv.appendChild(temperature);

        temperatureHolder.appendChild(temperatureDiv);
    }

    var temperature = document.getElementById(temperatureDto.locationId + "temperature");
    temperature.textContent = temperatureDto.value;
});

connection.start().then(function(){
    connection.invoke("RetrieveTemperatures").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});
