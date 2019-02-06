From python

WORKDIR /app

RUN pip install requests && pip install OWSLib

COPY *.py ./

ENTRYPOINT [ "python", "./Gatherer.py" ]
