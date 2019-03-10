From python

WORKDIR /app

RUN apt-get update && \
    apt-get install -y sudo nano python3-dev python3-psutil bluez bluez-hcidump && \
    pip install requests ruuvitag_sensor

COPY *.py ./

ENTRYPOINT [ "python", "./Gatherer.py" ]
