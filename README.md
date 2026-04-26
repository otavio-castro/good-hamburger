# Good Hamburger

Projeto full stack em .NET 10 com as seguintes separações:

- `GoodHamburgerApi`: API ASP.NET Core para cardapio e pedidos.
- `GoodHamburgerFront`: interface web em Razor Pages que consome a API.
- `GoodHamburgerTest`: testes unitarios da camada de servico da API.

## Visao geral

O sistema entrega um fluxo simples de lanchonete:

- o cardapio e carregado a partir de `Data/cardapio.json`;
- pedidos sao criados, consultados, atualizados e removidos via API;
- o front centraliza a experiencia em uma unica pagina com secoes de cardapio, formulario de pedido e lista de pedidos.

## Como executar

### Pre-requisitos

- .NET SDK 10
- EF (Entity Framework)
- HTTPS habilitado para executar os dois projetos localmente

### 1. Subir a API

```bash
cd codigos/back/GoodHamburgerApi/GoodHamburgerApi
dotnet run --launch-profile https
```

Padroes locais do projeto de API:

- Swagger: `https://localhost:7192/swagger`
- HTTP alternativo: `http://localhost:5082`

### 2. Subir o front

Antes de executar o front, confira se `Api:BaseUrl` em `codigos/front/GoodHamburgerFront/GoodHamburgerFront/appsettings.json` aponta para a URL da API em execucao.

```bash
cd codigos/front/GoodHamburgerFront/GoodHamburgerFront
dotnet run --launch-profile https
```

Padroes locais do projeto de front:

- HTTPS: `https://localhost:7245`
- HTTP alternativo: `http://localhost:5035`

### 3. Executar os testes

```bash
cd codigos/back/GoodHamburgerApi/GoodHamburgerTest
dotnet test
```

## Arquitetura adotada

### Separacao front e back

O repositorio foi dividido em dois aplicativos independentes:

- o back expoe a regra de negocio por HTTP;
- o front nao acessa banco diretamente, ele conversa com a API por `HttpClient`.

Essa separacao simplifica o front e deixa a regra de negocio concentrada em um unico lugar.

### Camadas do back

O backend segue um fluxo tradicional:

- `Controllers` recebem as requisicoes HTTP;
- `Services` concentram validacao e regras de negocio;
- `Repositories` tratam leitura e escrita de dados;
- `Context` guarda o `DbContext` do Entity Framework Core.

### Persistencia em memoria

Os pedidos sao armazenados com `UseInMemoryDatabase("GoodHamburgerDb")`. Isso reduz o custo de setup e deixa o projeto simples para avaliacao local, mas tambem significa que os pedidos sao perdidos ao reiniciar a aplicacao.

### Cardapio vindo de arquivo JSON

O cardapio nao esta em banco. Ele e carregado de `Data/cardapio.json` e copiado para a pasta de saida no build. A decisao facilita manter o menu fixo e previsivel, sem migracoes ou seed de banco.

### Mapeamento entre entidades e DTOs

O back usa AutoMapper para converter entidades em DTOs. O front trabalha com view models proprios, evitando expor as entidades internas da API diretamente.

### Regras de negocio principais

As regras de pedido estao centralizadas em `PedidoService`:

- precisa haver pelo menos um item;
- nao pode haver itens duplicados;
- e permitido no maximo 1 sanduiche, 1 acompanhamento e 1 bebida;
- descontos aplicados:
  - 10% para sanduiche + acompanhamento;
  - 15% para sanduiche + bebida;
  - 20% para combo completo.

### Front em uma pagina

O front foi montado como uma unica Razor Page com partes reutilizaveis em partials. A pagina principal suporta:

- visualizar cardapio;
- criar pedido;
- buscar pedido por id;
- carregar pedido para edicao;
- atualizar pedido;
- excluir pedido.

## O que ficou fora

Com base no codigo atual, estes pontos nao foram implementados:

- banco de dados real e migracoes;
- autenticacao e autorizacao;
- persistencia duravel dos pedidos;
- pagina de administracao separada;
- containerizacao e pipeline de deploy.

## Estrutura resumida

- `codigos/back/GoodHamburgerApi`: API, servicos, repositorios, DTOs e testes.
- `codigos/front/GoodHamburgerFront`: Razor Pages, view models e servico cliente da API.

## Observacoes finais

- O front depende da API estar rodando na URL configurada em `Api:BaseUrl`.
- O Swagger so aparece em ambiente de desenvolvimento.
- Os pedidos sao mantidos apenas enquanto a aplicacao estiver em execucao.
