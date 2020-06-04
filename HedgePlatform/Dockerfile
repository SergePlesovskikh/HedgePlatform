#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
# <wkhtmltopdf>
RUN apt-get update
RUN apt-get install wget libgdiplus -y
RUN wget -P /app https://github.com/rdvojmoc/DinkToPdf/raw/master/v0.12.4/64%20bit/libwkhtmltox.dll
RUN wget -P /app https://github.com/rdvojmoc/DinkToPdf/raw/master/v0.12.4/64%20bit/libwkhtmltox.dylib
RUN wget -P /app https://github.com/rdvojmoc/DinkToPdf/raw/master/v0.12.4/64%20bit/libwkhtmltox.so
# </wkhtmltopdf>

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["HedgePlatform/HedgePlatform.csproj", "HedgePlatform/"]
RUN dotnet restore "HedgePlatform/HedgePlatform.csproj"
COPY . .
WORKDIR "/src/HedgePlatform"
RUN dotnet build "HedgePlatform.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "HedgePlatform.csproj" -c Release -o /app/publish

FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .

ENV PATH=/app;$PATH
ENTRYPOINT ["dotnet", "HedgePlatform.dll"]
