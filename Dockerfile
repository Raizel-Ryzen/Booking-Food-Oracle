FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app 
#
# copy csproj and restore as distinct layers
COPY *.sln ..
COPY Application/*.csproj ./Application/
COPY Domain/*.csproj ./Domain/
COPY Infrastructure/*.csproj ./Infrastructure/
COPY WebUI/*.csproj ./WebUI/

#
RUN dotnet restore "WebUI/WebUI.csproj"
#
# copy everything else and build app
COPY . .

RUN dotnet build "WebUI/WebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebUI/WebUI.csproj" -c Release -o /app/publish

#
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app/WebUI 
#
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebUI.dll"]