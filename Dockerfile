FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app
COPY ./PortalApp/*.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish ./PortalApp/PortalApp.csproj -c Release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "PortalApp.dll"]
