# Blog API


## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for PostgreSQL)

## How to run

### 1. Start the database

```bash
docker compose up -d
```

This starts PostgreSQL 16 on port `5432` with database `BlogDb`. On first start, scripts in `Blog.Infrastructure/Persistence/Scripts/` create tables and seed a sample author.

To reset the database (removes volumes):

```bash
docker compose down -v
```

### 2. Configure the API

Set the environment variable `ASPNETCORE_ENVIRONMENT=Development` to use the development configuration.

The default connection string for local development is in `Blog.API/appsettings.Development.json`.


### 3. Run the API

```bash
dotnet run --project Blog.API
```

### 4. Swagger (Development)

When running in the Development environment, open Swagger UI at `https://localhost:<port>/swagger` (the port is printed in the console).

### 5. Run tests

```bash
dotnet test
```

Test projects: `Blog.Domain.UnitTests`, `Blog.Application.IntegrationTests`, `Blog.API.UnitTests`.

### Sample requests

**Create a post** — `POST /posts` with `Content-Type: application/json`:

```json
{
  "authorId": "c478c52c-0159-408f-9c09-edb5b489b979",
  "title": "My post",
  "description": "Short summary",
  "content": "Full content"
}
```

The seeded author id comes from `Blog.Infrastructure/Persistence/Scripts/999_seed.sql`. The author must exist before creating a post.

**Get a post** — `GET /posts/{id}?includeAuthor=false` or `includeAuthor=true`.

## Solution layout

| Project | Role |
|---------|------|
| `Blog.Domain` | Aggregates, domain events, repository interfaces |
| `Blog.Application` | Commands, queries, DTOs, handlers |
| `Blog.Infrastructure` | Dapper/PostgreSQL, unit of work, domain event persistence |
| `Blog.API` | Minimal API endpoints, HTTP serialization |
| `*.UnitTests` / `*.IntegrationTests` | Unit and integration tests |

## Implemented features

- **Layered architecture** — Domain, Application, Infrastructure, and API projects with clear dependencies.
- **DDD** — `Post` and `Author` aggregates with value objects (`PostId`, `PostTitle`, `AuthorId`, etc.).
- **CQRS** — `CreatePostCommand` and `GetPostByIdQuery` with separate `ICommandHandler` / `IQueryHandler` implementations.
- **Persistence** — PostgreSQL via Dapper; `IUnitOfWork` wraps transactional writes.
- **Domain events / outbox** — `PostCreatedEvent` is raised when a post is created and stored in the `domain_events` table in the same transaction as the `post` row (`processed_at` is available for future projection).
- **API endpoints** — `POST /posts`, `GET /posts/{id}` with optional `includeAuthor` query parameter.
- **Custom serialization** — Pluggable `IApiContentSerializer` in `Blog.API/Serialization/`; JSON via `Content-Type` and `Accept`; HTTP 415 for unsupported request media types and 406 when `Accept` lists only unsupported types.
- **Developer experience** — Swagger and CORS in Development; Docker Compose for PostgreSQL.
- **Tests** — Domain unit tests, application integration tests (Testcontainers PostgreSQL), API serialization unit tests.

## Planned / future

| Area | Planned work |
|------|----------------|
| **Serialization** | `XmlApiContentSerializer` and `application/xml` content negotiation. |
| **Security** | JWT authorization; derive author id from the token on create instead of the request body. |
| **DI** | Auto-register command and query handlers by assembly scan. |
| **Documentation** | XML comments on public types and members. |
| **CQRS read model** | Redis cache and an outbox projector worker; query handlers reading from a read repository (infrastructure scaffolding exists under `Caching/`, `Outbox/`, `Projections/` and `Blog.Projector/` but is not wired in the API yet). |

