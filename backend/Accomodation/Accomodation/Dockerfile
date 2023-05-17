﻿
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Accomodation/Accomodation.csproj", "Accomodation/"]
COPY ["Accomodation.Application/Accomodation.Application.csproj", "Accomodation.Application/"]
COPY ["Accomodation.Domain/Accomodation.Domain.csproj", "Accomodation.Domain/"]
COPY ["Accomodation.Infrastructure/Accomodation.Infrastructure.csproj", "Accomodation.Infrastructure/"]
COPY ["Accomodation.Presentation/Accomodation.Presentation.csproj", "Accomodation.Presentation/"]

RUN dotnet restore "Accomodation/Accomodation.csproj"
COPY . .
WORKDIR "/src/Accomodation"
RUN dotnet build "Accomodation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Accomodation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Accomodation.dll"]