FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG TARGETARCH
WORKDIR /src
COPY ["src/Api/Api.csproj", "Api/"]
RUN dotnet restore "Api/Api.csproj"
COPY src .
WORKDIR "/src/Api"
RUN dotnet test ../Api.Tests
RUN ls -altrF
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish -a $TARGETARCH

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
