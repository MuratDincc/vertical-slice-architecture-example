FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY . .
WORKDIR "/src/api/Commerce.Api/"
RUN dotnet restore "Commerce.Api.csproj"
RUN dotnet build "Commerce.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Commerce.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:80
EXPOSE 80
ENTRYPOINT ["dotnet", "Commerce.Api.dll"]
