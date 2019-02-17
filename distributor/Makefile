build-release:
	docker stop distributor || true
	docker rm distributor || true
	rm -rf out
	dotnet publish -c Release -o out Distributor.Presentation
	mv Distributor.Presentation/out out
	docker build -t distributor .
	docker run -d --link rabbit:my-rabbit -p 5001:80 --name distributor distributor
