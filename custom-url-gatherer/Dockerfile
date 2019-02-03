From python:alpine

WORKDIR /app

RUN pip install requests

COPY *.py ./

ENTRYPOINT [ "python", "./Gatherer.py" ]
