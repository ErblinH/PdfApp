#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

RUN apt update
RUN apt install -y libgdiplus
RUN ln -s /usr/lib/libgdiplus.so /lib/x86_64-linux-gnu/libgdiplus.so
RUN apt-get install -y --no-install-recommends zlib1g fontconfig libfreetype6 libx11-6 libxext6 libxrender1 wget gdebi
RUN wget https://github.com/wkhtmltopdf/wkhtmltopdf/releases/download/0.12.5/wkhtmltox_0.12.5-1.stretch_amd64.deb
RUN gdebi --n wkhtmltox_0.12.5-1.stretch_amd64.deb
RUN apt install libssl1.1
RUN ln -s /usr/local/lib/libwkhtmltox.so /usr/lib/libwkhtmltox.so

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PdfApp.API/PdfApp.API.csproj", "PdfApp.API/"]
COPY ["PdfApp.Application/PdfApp.Application.csproj", "PdfApp.Application/"]
COPY ["PdfApp.Data/PdfApp.Data.csproj", "PdfApp.Data/"]
COPY ["PdfApp.Infrastructure/PdfApp.Infrastructure.csproj", "PdfApp.Infrastructure/"]
RUN dotnet restore "PdfApp.API/PdfApp.API.csproj"
COPY . .
WORKDIR "/src/PdfApp.API"
RUN dotnet build "PdfApp.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PdfApp.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "PdfApp.API.dll"]
