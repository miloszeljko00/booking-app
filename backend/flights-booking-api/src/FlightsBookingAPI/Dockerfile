#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Container we use for final publish
FROM mcr.microsoft.com/dotnet/core/aspnet:6.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build container
FROM mcr.microsoft.com/dotnet/core/sdk:6.0-buster AS build

# Copy the code into the container
WORKDIR /src
COPY ["src/FlightsBookingAPI/FlightsBookingAPI.csproj", "FlightsBookingAPI/"]

# NuGet restore
RUN dotnet restore "FlightsBookingAPI/FlightsBookingAPI.csproj"
COPY ["src/FlightsBookingAPI/", "FlightsBookingAPI/"]

# Build the API
WORKDIR "FlightsBookingAPI"
RUN dotnet build "FlightsBookingAPI.csproj" -c Release -o /app/build

# Publish it
FROM build AS publish
RUN dotnet publish "FlightsBookingAPI.csproj" -c Release -o /app/publish

# Make the final image for publishing
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlightsBookingAPI.dll"]
