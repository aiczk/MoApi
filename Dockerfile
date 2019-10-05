FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ChatApp.Server/ChatApp.Server.csproj /ChatApp.Server/
RUN dotnet restore /ChatApp.Server/ChatApp.Server.csproj
COPY . .
WORKDIR /src/ChatApp.Server
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ChatApp.Server.dll"]

EXPOSE 12345