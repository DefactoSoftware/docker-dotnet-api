# Publish image
FROM microsoft/dotnet:latest

# Publish app
COPY bin/Debug/netcoreapp1.0/publish/ /app
WORKDIR /app
EXPOSE 5000/tcp

# Run app
ENTRYPOINT dotnet dotnetcore-api-docker.dll
