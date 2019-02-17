"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5001/temperatureHub")
    .build();

connection.on("ReceiveTemperature", function (temperatureDto) {
    var li = document.createElement("li");
    li.textContent = temperatureDto.locationName + " " + temperatureDto.value + " " + temperatureDto.timestamp;

    var locationList = document.getElementById(temperatureDto.locationId);
    if(locationList === null || locationList === undefined) {
        var listholder = document.getElementById("lists");
        locationList = document.createElement("ul");
        locationList.id = temperatureDto.locationId;
        listholder.appendChild(locationList);
    }
    if (locationList.children.length > 0) {
        locationList.removeChild(locationList.children[0]);
    }
    locationList.appendChild(li);
});

connection.start().then(function(){
}).catch(function (err) {
    return console.error(err.toString());
});
