# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["src/MovieTicketBooking.WebHost/MovieTicketBooking.WebHost.csproj", "src/MovieTicketBooking.WebHost/"]
COPY ["src/Common/MovieTicketBooking.Common.Application/MovieTicketBooking.Common.Application.csproj", "src/Common/MovieTicketBooking.Common.Application/"]
COPY ["src/Common/MovieTicketBooking.Common.Domain/MovieTicketBooking.Common.Domain.csproj", "src/Common/MovieTicketBooking.Common.Domain/"]
COPY ["src/Common/MovieTicketBooking.Common.Infrastructure/MovieTicketBooking.Common.Infrastructure.csproj", "src/Common/MovieTicketBooking.Common.Infrastructure/"]
COPY ["src/Common/MovieTicketBooking.Common.Presentation/MovieTicketBooking.Common.Presentation.csproj", "src/Common/MovieTicketBooking.Common.Presentation/"]
COPY ["src/Modules/Users/MovieTicketBooking.Modules.Users.Infrastructure/MovieTicketBooking.Modules.Users.Infrastructure.csproj", "src/Modules/Users/MovieTicketBooking.Modules.Users.Infrastructure/"]
COPY ["src/Modules/Users/MovieTicketBooking.Modules.Users.Application/MovieTicketBooking.Modules.Users.Application.csproj", "src/Modules/Users/MovieTicketBooking.Modules.Users.Application/"]
COPY ["src/Modules/Users/MovieTicketBooking.Modules.Users.Domain/MovieTicketBooking.Modules.Users.Domain.csproj", "src/Modules/Users/MovieTicketBooking.Modules.Users.Domain/"]
COPY ["src/Modules/Users/MovieTicketBooking.Modules.Users.Presentation/MovieTicketBooking.Modules.Users.Presentation.csproj", "src/Modules/Users/MovieTicketBooking.Modules.Users.Presentation/"]
RUN dotnet restore "./src/MovieTicketBooking.WebHost/MovieTicketBooking.WebHost.csproj"
COPY . .
WORKDIR "/src/src/MovieTicketBooking.WebHost"
RUN dotnet build "./MovieTicketBooking.WebHost.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MovieTicketBooking.WebHost.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieTicketBooking.WebHost.dll"]