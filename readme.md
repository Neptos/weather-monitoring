# Weather monitor

## Ingester

### Building the docker image
Create `appsettings.json` in Ingester.Presentation and edit it to something like this:
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionString": "Host=db;Port=5432;Database=<database>;Username=<username>;Password=<password>"
}
```

`docker build -t ingester .`

### Running the docker image
If you are running your database as a docker container on the same host just link the two containers
`docker run -d --link postgres:db -p 5000:5000 --name ingester ingester`

## Custom url gatherer

### Building the docker image
`docker build -t custom-url-gatherer .`

### Running the docker image
If it's running on the same host as the ingester you can just link the two containers:

`docker run -d --link ingester:ingester -e SENSOR_ENDPOINT=http://<Url to temperature info>/ -e INGESTER_ENDPOINT=http://ingester:5000/ --name <container name> custom-url-gatherer`

## Fmi gatherer

### Building the docker image
`docker build -t fmi-gatherer .`

###

`docker run -d --link ingester:ingester -e INGESTER_ENDPOINT=http://ingester:5000/ --name <container name> fmi-gatherer`
