FROM  mcr.microsoft.com/dotnet/sdk:6.0 AS builder
WORKDIR /app
COPY . .
RUN dotnet publish -c release -o /app/bin

FROM  mcr.microsoft.com/dotnet/aspnet:6.0 
WORKDIR /app
COPY --from=builder /app/bin .
EXPOSE 80
ENTRYPOINT ["dotnet", "dotnetapp.dll"]



# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# WORKDIR /src
# COPY MyMicroservice.csproj .
# RUN dotnet restore
# COPY . .
# RUN dotnet publish -c release -o /app

# FROM mcr.microsoft.com/dotnet/aspnet:6.0
# WORKDIR /app
# COPY --from=build /app .
# ENTRYPOINT ["dotnet", "MyMicroservice.dll"]
