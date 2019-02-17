build-release:
	docker stop client || true
	docker rm client || true
	rm -rf out
	dotnet publish -c Release -o out Client.Presentation
	mv Client.Presentation/out out
	docker build -t client .
	docker run -d -p 8080:80 --name client client
