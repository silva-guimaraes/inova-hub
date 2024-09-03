# Inova Hub

Plataforma de feedback de ideais.

## Quick Start
```sh
$ git clone 'link do repositorio'
$ cd 'repositorio'
$ ./docker.sh
$ ./create_db.sh
$ ./run.sh
```

 '1234' é a senha padrão do banco. Por favor mudar antes de fazer um deploy para produção.

### Requerimentos

  - Linguagem de programação Go versão >= 1.23.0

  - Linguagem de templating [Templ](https://templ.guide/) >= v0.2.707

  - Docker, versão mais recente.

  - Disposição mental para absorver toda essa lambança de código.

## Estrutura do projeto

### Arquivos

Breve explicação dos arquivos mais importantes e seus papeis.


`docker.sh`: Cria uma instancia do postgres onde todo o servidor opera em cima.

`create_db.sh`: Roda o script que gera a estrutura do banco de dados. Requer que o docker esteja rodando.

`run.sh`: Inicia uma instancia do postgres com o docker caso não haja uma e roda o projeto.

<hr>

`cmd/`: Define componentes estáticos e templating para o front-end.

`internal/`: Funcionamento interno do servidor, inclui rotas e banco de dados.

<hr>

`cdm/api/main.go`: entry-point. Contem a função main e nada alem mais.

`cdm/web/base.templ`: Templating para o body e navbar.

`cdm/web/hello.templ`: Templating para a maioria das rotas.

<hr>

`internal/database/database.go`: Define funcionamento interno do banco de dados. Apenas funções para interagir com o banco estão disponíveis.

`internal/database/schema.sql`: Script que gera o banco.

`internal/server/routes.go`: Todas as rotas são registradas aqui.

`internal/server/server.go`: Configurações do servidor HTTP.

`internal/server/logging.go`: Utilidade para logar acessos ao servidor no terminal.


