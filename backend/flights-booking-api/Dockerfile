# Build container
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy the code into the container
COPY . .

# Build the API project
WORKDIR /app/src/FlightsBookingAPI
RUN dotnet build -c Release -o /app/build

# Build the library project
WORKDIR /app/FlightsBooking
RUN dotnet build -c Release -o /app/build

# Publish the API project
WORKDIR /app/src/FlightsBookingAPI
RUN dotnet publish -c Release -o /app/publish

# Final container
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FlightsBookingAPI.dll"]