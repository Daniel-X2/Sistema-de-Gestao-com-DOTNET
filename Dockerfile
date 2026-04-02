FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR ./Api

COPY . .
RUN dotnet publish ./Api/Api.csproj -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
RUN  useradd -m appuser
USER appuser

COPY --from=build /app .

ENTRYPOINT ["dotnet","Api.dll"]

EXPOSE 5153





