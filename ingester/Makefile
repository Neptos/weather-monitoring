build-release:
	docker stop ingester || true
	docker rm ingester || true
	rm -rf out
	dotnet publish -c Release -o out Ingester.Presentation
	mv Ingester.Presentation/out out
	docker build -t ingester .
	docker run -d --link postgres:db --link rabbit:my-rabbit -p 5000:80 --name ingester ingester
