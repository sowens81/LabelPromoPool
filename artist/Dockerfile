#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["PromoPool.ArtistAPI/PromoPool.ArtistAPI.csproj", "PromoPool.ArtistAPI/"]
RUN dotnet restore "PromoPool.ArtistAPI/PromoPool.ArtistAPI.csproj"
COPY . .
WORKDIR "/src/PromoPool.ArtistAPI"
RUN dotnet build "PromoPool.ArtistAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PromoPool.ArtistAPI.csproj" -c Release -o /app/publish

FROM base AS final
ENV DatabaseSettings:CollectionName=VALUE
ENV DatabaseSettings:ConnectionString=VALUE
ENV DatabaseSettings:DatabaseName=VALUE
ENV Auth0Settings:AppDomain=VALUE
ENV Auth0Settings:Identifier=VALUE
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PromoPool.ArtistAPI.dll"]