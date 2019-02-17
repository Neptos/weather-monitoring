build-release:
	docker stop custom-url-gatherer || true
	docker rm custom-url-gatherer || true
	docker build -t custom-url-gatherer .
	docker run -d --link ingester:ingester -e SENSOR_ENDPOINT=http://192.168.1.47/ -e INGESTER_ENDPOINT=http://ingester:80/ --name custom-url-gatherer custom-url-gatherer
