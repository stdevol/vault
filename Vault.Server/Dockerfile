FROM microsoft/dotnet:2.2-aspnetcore-runtime
COPY . /app
WORKDIR /app

ENV ASPNETCORE_URLS http://+:80
EXPOSE 80
CMD [ "dotnet", "Vault.Server.dll" ]
