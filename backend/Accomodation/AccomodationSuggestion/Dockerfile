﻿FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AccomodationSuggestion/AccomodationSuggestion.csproj", "AccomodationSuggestion/"]
COPY ["AccomodationSuggestion.Application/AccomodationSuggestion.Application.csproj", "AccomodationSuggestion.Application/"]
COPY ["AccomodationSuggestion.Domain/AccomodationSuggestion.Domain.csproj", "AccomodationSuggestion.Domain/"]
COPY ["AccomodationSuggestion.Infrastructure/AccomodationSuggestion.Infrastructure.csproj", "AccomodationSuggestion.Infrastructure/"]
COPY ["AccomodationSuggestion.Presentation/AccomodationSuggestion.Presentation.csproj", "AccomodationSuggestion.Presentation/"]

RUN dotnet restore "AccomodationSuggestion/AccomodationSuggestion.csproj"
COPY . .
WORKDIR "/src/AccomodationSuggestion"
RUN dotnet build "AccomodationSuggestion.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AccomodationSuggestion.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccomodationSuggestion.dll"]