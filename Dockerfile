FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copia o arquivo .csproj e restaura as dependências do projeto
COPY *.csproj ./
RUN dotnet restore

# Copia o restante dos arquivos do projeto e compila o aplicativo
COPY . ./
RUN dotnet publish -c Release -o out

# Cria uma imagem final otimizada para publicação
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .

# Define a porta em que o contêiner expõe o aplicativo
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Inicia o aplicativo quando o contêiner for iniciado
ENTRYPOINT ["dotnet", "Paradigmas.dll"]