FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["APIDemoUser/APIDemoUser.csproj", "APIDemoUser/"]
WORKDIR /src/APIDemoUser
COPY APIDemoUser/. .
RUN dotnet restore "APIDemoUser.csproj"
RUN dotnet build "APIDemoUser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "APIDemoUser.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "APIDemoUser.dll"]
