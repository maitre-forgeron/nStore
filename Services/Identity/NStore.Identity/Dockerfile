#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy the certificate file into the container
COPY ["./certs/aspnetapp-root-cert.cer", "/https-root/aspnetapp-root-cert.cer"]

# Install OpenSSL
RUN apt-get update && apt-get install -y openssl

# Convert and install the certificate directly in the Dockerfile
RUN openssl x509 -inform DER -in /https-root/aspnetapp-root-cert.cer -out /https-root/aspnetapp-root-cert.crt \
    && cp /https-root/aspnetapp-root-cert.crt /usr/local/share/ca-certificates/ \
    && update-ca-certificates

USER app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Identity/NStore.Identity/NStore.Identity.csproj", "Services/Identity/NStore.Identity/"]
RUN dotnet restore "./Services/Identity/NStore.Identity/NStore.Identity.csproj"
COPY . .
WORKDIR "/src/Services/Identity/NStore.Identity"
RUN dotnet build "./NStore.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NStore.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NStore.Identity.dll"]