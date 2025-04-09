# 📈 StockQuoteAlert

Este projeto é uma aplicação desenvolvida em C# com .NET 8.0 que consome dados da API da **TwelveData** para gerar alertas de compra ou venda de ativos financeiros com base em cotações de mercado.

## ⚙️ Requisitos

Antes de executar ou debugar a aplicação, é necessário ter os seguintes requisitos instalados:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)

## 🔌 API utilizada

A aplicação utiliza a API da [TwelveData](https://twelvedata.com/) para buscar as informações de cotações e ativos.

## 🛠️ Configuração

Antes de rodar o projeto, é necessário configurar o arquivo `appsettings.json`, localizado dentro da pasta `StockQuoteAlert\StockQuoteAlert` (mesma pasta onde está o `.csproj`), com seu token da TwelveData.  

Abra o arquivo e edite o valor da chave `Token`, substituindo `"MEUTOKEN"` pelo seu token válido:

```json
{
  "Token": "SEU_TOKEN_AQUI"
}
```

## 🚀 Como rodar

Após configurar o token, a aplicação pode ser executada de duas maneiras:

- **Executando diretamente o executável compilado**:

```bash
StockQuoteAlert\bin\Release\net8.0\StockQuoteAlert.exe
```
ou

- **Utilizando uma IDE como o Visual Studio**:  
   Basta pressionar `F5` para iniciar a aplicação em modo de depuração (debug).
