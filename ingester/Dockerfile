FROM microsoft/dotnet:2.2-aspnetcore-runtime

WORKDIR /app

COPY out/* ./

ENTRYPOINT [ "dotnet", "Ingester.Presentation.dll" ]
