# Weather monitor

This repository contains small subprojects that together gathers, stores and displays temperature data(and in the future hopefully more) using what ever technology I find interesting at the time.

## Running

Parts of this system uses RabbitMQ as a message broker. You can for example set one up using the public docker images available here https://hub.docker.com/_/rabbitmq.

The Ingester and the Distributor require some info in their `appsettings.json` files, see below.

Each subproject has a `Makefile`. Update it to suit your requirements and you should be good to go.

## Overview

### Ingester

This is where gatherers post their data. It in turn publishes the data on the message broker.

Include this in your `appsettings.json` in Ingester.Presentation:
```
  "ConnectionString": "Host=<host>;Port=5432;Database=<database>;Username=<username>;Password=<password>",
  "Rabbit":
  {
    "HostName": "<rabbitmq hostname>"
  }
```

### Distributor

Reads data from the Ingester using the RabbitMQ message broker and distributes it to clients using SignalR.

Include this in your `appsettings.json` in Distributor.Presentation:
```
  "Rabbit":
  {
    "HostName": "<rabbitmq hostname>"
  }
```

### Client

Serves a basic Razor pages based frontend that connects to the distributor using SignalR.

### Gatherers

They all basically function the same. Gather and aggregate data and then sends it to the Ingester.

#### Ruuvi gatherer

The `RUUVI_TAG_LIST` environment variable needs to follow the following syntax:

`RUUVI_TAG_LIST="<location name>;<mac address>,<location name>;<mac address>"`

`,` separated tags which have `;` separated name and mac address
