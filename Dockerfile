FROM microsoft/dotnet:latest

COPY bin/Debug/netcoreapp1.0/publish/ /app
WORKDIR /app
EXPOSE 5000/tcp
ENTRYPOINT dotnet dotnetcore-api-docker.dll
