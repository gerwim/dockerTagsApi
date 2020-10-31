FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as builder

RUN mkdir /app
COPY /src /app
WORKDIR /app/Api
# Remove bin and obj folders if they exist
RUN find -type d -name bin -prune -exec rm -rf {} \; && find -type d -name obj -prune -exec rm -rf {} \;
RUN dotnet clean
RUN dotnet restore
# Run unit tests
RUN cd ../Api.Tests && dotnet test
RUN dotnet publish -c Release -o out

# Deployment image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
RUN mkdir /app
WORKDIR /app
COPY --from=builder /app/Api/out/ /app/
EXPOSE 80/tcp
CMD ["dotnet", "/app/Api.dll"]