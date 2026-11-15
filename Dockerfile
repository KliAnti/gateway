FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

ARG GITHUB_PACKAGES_USERNAME=viac5

COPY NuGet.config ./
COPY gateway/gateway.csproj gateway/
RUN --mount=type=secret,id=github_packages_token \
	export NuGetPackageSourceCredentials_github="Username=${GITHUB_PACKAGES_USERNAME};Password=$(cat /run/secrets/github_packages_token);ValidAuthenticationTypes=Basic" && \
	dotnet restore gateway/gateway.csproj --configfile NuGet.config

COPY gateway/ gateway/
RUN dotnet publish gateway/gateway.csproj --configuration Release --output /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "gateway.dll"]