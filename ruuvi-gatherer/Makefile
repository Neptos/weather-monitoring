build-release:
	docker stop ruuvi-gatherer || true
	docker rm ruuvi-gatherer || true
	docker build -t ruuvi-gatherer .
	docker run --net=host --privileged -d -e RUUVI_TAG_LIST="<location name>;<mac address>,<location name>;<mac address>" -e INGESTER_ENDPOINT=<your ingester endpoint> --name ruuvi-gatherer ruuvi-gatherer
