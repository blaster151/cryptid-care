# CryptidCare

A pharmacy claims processing API for mythical beings (Werewolves, Hydras, Phoenixes, etc.).  

Built as a demonstration of clean architecture, TDD, and extensible rules design in .NET 10.

## Architecture Overview

```mermaid

flowchart TD
    Client -->|POST /claims| Controller

    subgraph Controller Layer
        Controller[ClaimsController]
    end

    subgraph Service Layer
        Controller -->|ProcessClaim| ClaimService
        ClaimService -->|Process| RulesEngine[ClaimRulesEngine]
        RulesEngine --> Rule1[SilverAllergyRule]
        RulesEngine --> Rule2[HydraHeadMultiplierRule]
        RulesEngine -->|...| RuleN[IClaimRule n]
    end

    subgraph Data Layer
        Controller -->|GetByIdAsync| PatientRepo[PatientRepository]
        Controller -->|GetByIdAsync| MedicineRepo[MedicineRepository]
        Controller -->|AddAsync| ClaimRepo[ClaimRepository]
        PatientRepo --> DB[(SQL Server)]
        MedicineRepo --> DB
        ClaimRepo --> DB
    end
```

## Patterns & Decisions

### Thin Controller
`ClaimsController` is responsible only for HTTP concerns: deserializing the request, delegating to the service layer, and returning an `ActionResult`. It has no business logic.

### Service Layer
`ClaimService` owns the business logic entry point. It accepts rich domain objects (`Patient`, `Medicine`, quantity), runs them through the rules engine, and returns a `ClaimResponse`. It has no knowledge of HTTP or persistence.

### Rules Engine (Chain of Responsibility)
Business rules are modeled as ordered, independently registered `IClaimRule` implementations. `ClaimRulesEngine` iterates them in registration order and short-circuits on the first rejection. Adding a new rule requires only:
1. Implementing `IClaimRule`
2. Registering it in `Program.cs`

No existing code changes. Rules are resolved via `IEnumerable<IClaimRule>` from the DI container, so order is controlled by registration order.

**Current rules:**
| Rule | Condition | Effect |
|---|---|---|
| `SilverAllergyRule` | Patient is Werewolf + medicine contains silver | Reject claim |
| `HydraHeadMultiplierRule` | Patient is Hydra | Multiply dispensed quantity by head count |
| `RefillCooldownRule` | An approved claim for the same patient + medicine exists within the last 30 days | Reject claim |

### Repository Pattern
Data access is abstracted behind `IPatientRepository`, `IMedicineRepository`, and `IClaimRepository`. The controller depends only on these interfaces — it has no knowledge of Entity Framework or SQL. This allows the ORM to be swapped or mocked independently of business logic.

### Mutable Pipeline Context
`ClaimContext` is an intentionally mutable object passed through the rules pipeline. Rules may set `DispensedQuantity` or call `Reject(reason)`. This avoids allocating new objects per rule and keeps rule implementations simple. If no rule sets `DispensedQuantity`, the service defaults it to `RequestedQuantity` (for non-rejected claims).

### Enum Serialization
`ClaimStatus` serializes as `"Approved"` / `"Rejected"` (strings), not integers. Configured globally via `JsonStringEnumConverter`. This makes API responses self-describing and resilient to enum reordering.

---

## Project Structure

```
CryptidCare/
├── Controllers/
│   └── ClaimsController.cs       # HTTP layer only
├── Data/
│   ├── CryptidCareDbContext.cs   # EF Core context
│   └── Repositories/             # IFoo + Foo implementations
├── Database/
│   ├── schema.sql                # Table definitions (for manual/local use)
│   ├── seed.sql                  # Sample mythical patients & medicines (manual/local)
│   └── init.sql                  # Combined schema + seed for Docker first-run
├── Models/                       # Domain types (Patient, Medicine, Claim, etc.)
├── Services/
│   ├── IClaimService.cs
│   ├── ClaimService.cs           # Owns rules engine; core logic entry point
│   └── Rules/
│       ├── IClaimRule.cs
│       ├── ClaimRulesEngine.cs
│       ├── SilverAllergyRule.cs
│       └── HydraHeadMultiplierRule.cs
├── Dockerfile
├── docker-compose.yml
└── CryptidCare.Tests/
    └── ClaimsControllerTests.cs  # xUnit; SQLite in-memory; DI-resolved controller
```

---

## Running with Docker (recommended)

**Prerequisites:** Docker Desktop (with WSL2 on Windows)

```bash
docker compose up --build
```

This starts three services:
1. **db** — SQL Server 2022 (Developer Edition in Docker; Express also works for local dev)
2. **db-init** — runs `Database/init.sql` once (creates database, tables, and seed data)
3. **api** — the CryptidCare API, available at `http://localhost:8080`

Browse the interactive API at: **http://localhost:8080/scalar**

---

## Sample API Request

Submit a claim for **Fawkes** (Phoenix) requesting **2 units** of **Regeneron**:

```bash
curl -s -X POST http://localhost:8080/claims \
  -H "Content-Type: application/json" \
  -d '{
    "patientId":  "a1a1a1a1-0000-0000-0000-000000000005",
    "medicineId": "b2b2b2b2-0000-0000-0000-000000000003",
    "quantity":   2
  }'
```

**Approved response:**
```json
{
  "status": "Approved",
  "dispensedQuantity": 2,
  "rejectionReason": null,
  "claimId": "<generated-uuid>"
}
```

**Rejected example** — Werewolf + silver medicine (Remus Lupin + Silver Sulfadiazine):

```bash
curl -s -X POST http://localhost:8080/claims \
  -H "Content-Type: application/json" \
  -d '{
    "patientId":  "a1a1a1a1-0000-0000-0000-000000000001",
    "medicineId": "b2b2b2b2-0000-0000-0000-000000000001",
    "quantity":   1
  }'
```

```json
{
  "status": "Rejected",
  "dispensedQuantity": 0,
  "rejectionReason": "Werewolves cannot be dispensed silver-based medicines.",
  "claimId": null
}
```

> See `Database/seed.sql` for the full list of pre-seeded patient and medicine IDs.

---

## Running Locally

**Prerequisites:** .NET 10 SDK, SQL Server Express (`.\SQLEXPRESS`)

```bash
# Apply schema and seed data
sqlcmd -S .\SQLEXPRESS -i Database/schema.sql
sqlcmd -S .\SQLEXPRESS -i Database/seed.sql

# Run API
dotnet run --project CryptidCare.csproj

# Run tests
dotnet test CryptidCare.Tests/CryptidCare.Tests.csproj
```

Browse the interactive API at: **http://localhost:5041/scalar**
