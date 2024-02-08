#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/sdk:6.0-windowsservercore-ltsc2019 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-windowsservercore-ltsc2019 AS build
WORKDIR /src
COPY ["HotelChief.API/HotelChief.API.csproj", "HotelChief.API/"]
COPY ["HotelChief.Application/HotelChief.Application.csproj", "HotelChief.Application/"]
COPY ["HotelChief.Core/HotelChief.Core.csproj", "HotelChief.Core/"]
COPY ["HotelChief.Infrastructure/HotelChief.Infrastructure.csproj", "HotelChief.Infrastructure/"]
RUN dotnet restore "HotelChief.API/HotelChief.API.csproj"
COPY . .
WORKDIR "/src/HotelChief.API"
RUN dotnet build "HotelChief.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HotelChief.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotelChief.API.dll"]