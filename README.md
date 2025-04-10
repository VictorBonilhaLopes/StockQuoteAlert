# 📈 StockQuoteAlert

Este projeto é uma aplicação desenvolvida em C# com .NET 8.0 que consome dados da API da **TwelveData** para gerar alertas de compra ou venda de ativos financeiros com base em cotações de mercado.

## ⚙️ Requisitos

Antes de executar ou debugar a aplicação, é necessário ter os seguintes requisitos instalados:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)

## 🔌 API utilizada

A aplicação utiliza a API da [TwelveData](https://twelvedata.com/) para obter informações de cotações e ativos financeiros.

## 🛠️ Configuração

Antes de rodar o projeto, é necessário configurar o arquivo `appsettings.example.json`, localizado na pasta `StockQuoteAlert\StockQuoteAlert` (mesma pasta onde está o `.csproj`), com seu token da TwelveData.

1. Renomeie o arquivo para `appsettings.json`.
2. Edite o valor da chave `Token`, substituindo `"MEUTOKEN"` pelo seu token válido:

```json
{
  "Token": "SEU_TOKEN_AQUI"
}
```

## 🚀 Como rodar

Após configurar o token, a aplicação pode ser executada de três maneiras:

---

🔹 **1. Pela linha de comando (sem compilar manualmente)**  
Acesse o diretório `StockQuoteAlert\StockQuoteAlert` no terminal e execute:

```bash
dotnet run -- PETR4/33.45/33.10
```

Substitua os valores pelo código da ação e os preços desejados.

---

🔹 **2. Através de uma IDE (como o Visual Studio)**  
Abra o projeto na IDE e pressione `F5` para executar em modo de depuração (debug).

---

🔹 **3. Compilando manualmente para gerar o executável** *(opcional)*  
Você também pode compilar o projeto e rodar o executável diretamente:

```bash
dotnet build
```

O executável será gerado em:

```
StockQuoteAlert\StockQuoteAlert\bin\Debug\net8.0\
```

Para executar, utilize o seguinte formato de comando dentro dessa pasta:

```bash
StockQuoteAlert.exe PETR4/33.45/33.10
```
