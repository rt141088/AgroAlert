# 🌾 AgroAlert API

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![xUnit](https://img.shields.io/badge/xUnit-13%20testes-green?style=for-the-badge)
![Swagger](https://img.shields.io/badge/Swagger-OpenAPI%203.0-85EA2D?style=for-the-badge&logo=swagger)
![JWT](https://img.shields.io/badge/JWT-Auth-000000?style=for-the-badge&logo=jsonwebtokens)
![EF Core](https://img.shields.io/badge/EF%20Core-8.0-512BD4?style=for-the-badge)

Sistema de alertas climáticos para agricultores — FIAP Advanced Business Development with .NET

---

## 📐 Arquitetura (Clean Architecture)

```
AgroAlert/
├── AgroAlert.Domain/          # Entidades, Interfaces, Enums (sem dependências)
├── AgroAlert.Application/     # Use Cases, DTOs, Services (SOLID)
├── AgroAlert.Infrastructure/  # EF Core, Oracle, Repositories
├── AgroAlert.API/             # Controllers, JWT, Swagger, Health Check
└── AgroAlert.Tests/           # xUnit (13 testes automatizados)
```

```
┌─────────────────────────────────────────┐
│              AgroAlert.API              │
│   Controllers │ JWT │ Swagger │ Health  │
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│          AgroAlert.Application          │
│        Services │ DTOs │ UseCases       │
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│            AgroAlert.Domain             │
│       Entities │ Interfaces │ Enums     │
└──────────────────▲──────────────────────┘
                   │
┌──────────────────┴──────────────────────┐
│         AgroAlert.Infrastructure        │
│      EF Core │ Oracle │ Repositories    │
└─────────────────────────────────────────┘
```

**Fluxo de dados:** API → Application → Domain ← Infrastructure

---

## 🚀 Como Rodar Localmente

### Pré-requisitos
- Visual Studio 2022
- .NET 8 SDK
- *(Opcional para produção)* Oracle Database

### Passo a Passo

**1. Abrir a solution**
```
Dê duplo clique em AgroAlert.sln
```

**2. Definir projeto de inicialização**
```
Clique com botão direito em AgroAlert.API → Set as Startup Project
```

**3. Rodar**
```
Pressione F5 ou clique no botão ▶️ verde
```

**4. Acessar o Swagger**
```
http://localhost:5000/swagger
```

> Em modo Development, usa **InMemory** automaticamente.
> Dados de demo são inseridos automaticamente:
> - Email: `joao@agroalert.com`
> - Senha: `senha123`

### Rodar via Terminal
```bash
cd AgroAlert/AgroAlert.API
dotnet restore
dotnet run
```

### Trocar para Oracle (Produção)
```bash
# 1. Edite appsettings.json com sua connection string
"OracleConnection": "Data Source=oracle.fiap.com.br/orcl;User Id=SEU_USER;Password=SUA_SENHA;"

# 2. Mude o ambiente para Production
$env:ASPNETCORE_ENVIRONMENT="Production"

# 3. Aplique as migrations
dotnet ef database update --project AgroAlert.Infrastructure --startup-project AgroAlert.API
```

---

## 🔐 Autenticação JWT

```bash
# 1. Registrar
POST /api/auth/register
{
  "nome": "João Silva",
  "email": "joao@agroalert.com",
  "senha": "senha123",
  "telefone": "(11) 99999-0001",
  "cpf": "123.456.789-00"
}

# 2. Login
POST /api/auth/login
{
  "email": "joao@agroalert.com",
  "senha": "senha123"
}

# 3. Copie o token e clique em "Authorize" no Swagger
# Informe: Bearer {token}
```

---

## 📡 Endpoints

| Método | Rota | Descrição | Auth |
|--------|------|-----------|------|
| POST | /api/auth/login | Login → Token JWT | ❌ |
| POST | /api/auth/register | Cadastro de agricultor | ❌ |
| GET | /api/agricultores | Lista agricultores | ✅ |
| POST | /api/agricultores | Cria agricultor | ✅ |
| GET | /api/agricultores/{id} | Busca por ID | ✅ |
| PUT | /api/agricultores/{id} | Atualiza agricultor | ✅ |
| DELETE | /api/agricultores/{id} | Soft delete | ✅ |
| GET | /api/propriedades | Lista propriedades | ✅ |
| POST | /api/propriedades | Cria propriedade | ✅ |
| GET | /api/propriedades/{id} | Busca por ID | ✅ |
| PUT | /api/propriedades/{id} | Atualiza propriedade | ✅ |
| DELETE | /api/propriedades/{id} | Remove propriedade | ✅ |
| GET | /api/alertas | Alertas com filtros | ✅ |
| GET | /api/alertas/{id} | Busca alerta por ID | ✅ |
| POST | /api/dados-climaticos | Envia dados do sensor → dispara alertas | ✅ |
| GET | /api/dados-climaticos | Lista dados climáticos | ✅ |
| GET | /api/regras | Lista regras | ✅ |
| POST | /api/regras | Cria regra de alerta | ✅ |
| DELETE | /api/regras/{id} | Remove regra | ✅ |
| GET | /health | Health Check | ❌ |

---

## 🧪 Exemplos de Request

### Criar Propriedade
```json
POST /api/propriedades
{
  "nome": "Fazenda Boa Vista",
  "localizacao": "Ribeirão Preto - SP",
  "areaHectares": 200,
  "latitude": -21.17,
  "longitude": -47.81,
  "tipoCultura": "Milho",
  "agricultorId": 1
}
```

### Enviar Dados Climáticos (dispara alertas automaticamente)
```json
POST /api/dados-climaticos
{
  "temperatura": 42,
  "umidade": 15,
  "precipitacao": 0,
  "velocidadeVento": 80,
  "propriedadeId": 1,
  "fonteDados": "Sensor IoT"
}
```

### Criar Regra de Alerta
```json
POST /api/regras
{
  "nome": "Temperatura Crítica",
  "tipoAlerta": 5,
  "parametro": "Temperatura",
  "operador": ">",
  "valorLimite": 38,
  "nivelRisco": 3,
  "propriedadeId": 1
}
```

---

## ✅ Testes Automatizados (xUnit)

**13 testes | 0 falhas**

| Teste | Descrição |
|-------|-----------|
| DeveCriarAgricultorComSucesso | Criação e persistência de agricultor |
| DeveRejeitarLoginComSenhaErrada | Validação de credenciais inválidas |
| DeveAceitarLoginComSenhaCorreta | Autenticação com credenciais válidas |
| DeveCriarAlertaComNivelAlto | Criação de alerta automático nível Alto |
| DeveDispararAlertaQuandoTemperaturaUltrapassaLimite | Validação de regra de negócio |

```bash
# Rodar todos os testes
dotnet test AgroAlert.Tests/

# Resultado esperado
# total: 13 | falhou: 0 | bem-sucedido: 13 ✅
```

---

## 🏗️ Decisões Técnicas

| Decisão | Motivo |
|---------|--------|
| **Clean Architecture** | Separação clara de responsabilidades por camada |
| **SOLID** | Interfaces nos repositórios (DIP), serviços com responsabilidade única (SRP) |
| **JWT stateless** | Tokens com expiração de 8h, sem estado no servidor |
| **ResponseWrapper** | Padronização de todas as respostas da API |
| **InMemory + Oracle** | InMemory para dev sem dependência de banco |
| **Soft Delete** | Agricultores nunca são deletados fisicamente (campo Ativo) |
| **Middleware global** | Tratamento centralizado de exceções |
| **Health Check** | Monitoramento em `/health` |

---

## 👥 Equipe

**FIAP — Advanced Business Development with .NET — Global Solution 2026**

| Nome | RM |
|------|----|
| Rafael Terra Teodoro | RM 560955 |
| Enzo Elia Tarraga | RM 560901 |
| Otoniel Arantes Barbado | RM 560112 |
| Ranaldo José da Silva | RM 559210 |
| Fabrício José da Silva | RM 560694 |
