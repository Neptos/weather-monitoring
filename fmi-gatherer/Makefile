build-release:
	docker stop fmi-gatherer || true
	docker rm fmi-gatherer || true
	docker build -t fmi-gatherer .
	docker run -d --link ingester:ingester -e INGESTER_ENDPOINT=http://ingester:80/ --name fmi-gatherer fmi-gatherer
