# Sistema de Monitoramento de Alagamentos

Sistema desenvolvido para monitorar zonas de risco de alagamento em São Paulo, permitindo o registro de níveis pluviométricos e geração de relatórios.

## Grupo

- João Antonio Vieira Rihan - rm99656
- Rodrigo Fernandes Serafim - rm550816


## Instalação

1. Clone o repositório
2. Navegue até a pasta do projeto
3. Execute o comando `dotnet build` para compilar o projeto
4. Execute o comando `dotnet run` para iniciar o sistema

## Funcionalidades

### Gerenciamento de Zonas de Risco
- Cadastro de novas zonas
- Edição de zonas existentes
- Remoção de zonas
- Listagem de todas as zonas

### Registro de Níveis Pluviométricos
- Registro de níveis por zona
- Alertas automáticos para níveis elevados
- Histórico de registros

### Relatórios
- Geração de relatórios de status
- Resumo por nível de risco
- Listagem de zonas críticas

### Histórico de Eventos
- Visualização de todos os eventos
- Filtragem por tipo de evento
- Filtragem por zona de risco

### Gerenciamento de Usuários
- Cadastro de novos usuários
- Edição de usuários existentes
- Remoção de usuários
- Diferentes níveis de acesso

### Configurações do Sistema
- Ajuste dos níveis pluviométricos para alertas
- Configuração do período de retenção de eventos
- Acesso restrito a administradores

## Níveis de Acesso

### Operador
- Visualizar zonas de risco
- Registrar níveis pluviométricos
- Visualizar relatórios
- Visualizar histórico de eventos

### Supervisor
- Todas as funcionalidades do Operador
- Gerenciar zonas de risco

### Administrador
- Todas as funcionalidades do Supervisor
- Gerenciar usuários
- Configurar parâmetros do sistema

## Credenciais Padrão

- Login: admin
- Senha: admin123

## Configurações Padrão

- Nível Crítico Pluviométrico: 50.0mm
- Nível Alto Pluviométrico: 30.0mm
- Nível Médio Pluviométrico: 15.0mm
- Retenção de Eventos: 30 dias

## Estrutura do Projeto

```
MonitoramentoAlagamentos/
├── Models/
│   ├── ZonaRisco.cs
│   ├── Evento.cs
│   ├── Usuario.cs
│   ├── Configuracao.cs
│   └── ConfiguracaoSistema.cs
├── Services/
│   ├── IZonaRiscoService.cs
│   ├── ZonaRiscoService.cs
│   ├── IEventoService.cs
│   ├── EventoService.cs
│   ├── IUsuarioService.cs
│   ├── UsuarioService.cs
│   ├── IConfiguracaoService.cs
│   └── ConfiguracaoService.cs
├── UI/
│   └── ConsoleUI.cs
├── Program.cs
├── appsettings.json
├── appsettings.Example.json
└── README.md
```
