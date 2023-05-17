﻿FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AccomodationGrading/AccomodationGrading.csproj", "AccomodationGrading/"]
COPY ["AccomodationGrading.Application/AccomodationGrading.Application.csproj", "AccomodationGrading.Application/"]
COPY ["AccomodationGrading.Domain/AccomodationGrading.Domain.csproj", "AccomodationGrading.Domain/"]
COPY ["AccomodationGrading.Infrastructure/AccomodationGrading.Infrastructure.csproj", "AccomodationGrading.Infrastructure/"]
COPY ["AccomodationGrading.Presentation/AccomodationGrading.Presentation.csproj", "AccomodationGrading.Presentation/"]

RUN dotnet restore "AccomodationGrading/AccomodationGrading.csproj"
COPY . .
WORKDIR "/src/AccomodationGrading"
RUN dotnet build "AccomodationGrading.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AccomodationGrading.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccomodationGrading.dll"]