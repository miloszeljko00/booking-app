#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AccomodationGrading/AccomodationGrading.csproj", "AccomodationGrading/"]
COPY ["Accomodation.Infrastructure/AccomodationGrading.Infrastructure.csproj", "Accomodation.Infrastructure/"]
COPY ["AccomodationGrading.Application/AccomodationGrading.Application.csproj", "AccomodationGrading.Application/"]
COPY ["AccomodationGrading.Domain/AccomodationGrading.Domain.csproj", "AccomodationGrading.Domain/"]
RUN dotnet restore "AccomodationGrading/AccomodationGrading.csproj"
COPY . .
WORKDIR "/src/AccomodationGrading"
RUN dotnet build "AccomodationGrading.csproj" -c Release -o /app/build

FROM build AS publish	
RUN dotnet publish "AccomodationGrading.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccomodationGrading.dll"]