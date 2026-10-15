<div align="center">

# 🚗 EVASION — Vehicle Rental Management System

**Desktop rental-agency software: clients, fleet, bookings, returns with automatic billing, and a 9-report analytics dashboard — business logic in SQL Server stored procedures.**

[![C#](https://img.shields.io/badge/C%23-.NET%208-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![WinForms](https://img.shields.io/badge/UI-WinForms-0078D4?logo=windows&logoColor=white)](https://learn.microsoft.com/dotnet/desktop/winforms/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-T--SQL-CC2927?logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![ADO.NET](https://img.shields.io/badge/Data-Microsoft.Data.SqlClient-5C2D91)](https://learn.microsoft.com/sql/connect/ado-net/)
[![Stored procedures](https://img.shields.io/badge/Stored%20procedures-33-success)](database/02_stored_procedures.sql)

*Academic project — Databases / SQL Server*

</div>

---

## 📌 Table of contents

- [Overview](#-overview)
- [Architecture](#-architecture)
- [Data model](#-data-model)
- [Rental lifecycle](#-rental-lifecycle)
- [Reports](#-reports)
- [Quick start](#-quick-start)
- [Project structure](#-project-structure)

---

## 🎯 Overview

| | Feature |
|---|---|
| 🔐 | **Role-based login** — `ADMIN` / `AGENT` |
| 👥 | **Clients** — individuals (`IND`) and organizations (`ORG`), search, CRUD |
| 🚙 | **Fleet** — vehicles by tariff category, mileage, availability |
| 📅 | **Bookings** — only vehicles *actually free* for the chosen dates are offered (overlap check in SQL) |
| ↩️ | **Returns** — amount computed automatically from days × daily rate + km × per-km rate; odometer updated |
| 📊 | **Dashboard** — clients, vehicles, active rentals, total revenue |
| 📈 | **9 business reports** (3 were required) |
| 🛡️ | **Typed SQL parameters everywhere** — no string-built SQL, no injection |

---

## 🏗️ Architecture

Three layers, with the business rules living close to the data:

```mermaid
flowchart TB
    subgraph UI["🖥️ Presentation — WinForms"]
        LF["LoginForm"]
        MF["MainForm<br/>tabs + dashboard"]
        DLG["Client · Vehicle · Rental<br/>Return dialogs"]
    end
    subgraph DAL["🧱 Data access — Repository pattern"]
        AR["AuthRepository"]
        CR["ClientRepository"]
        VR["VehicleRepository"]
        RR["RentalRepository"]
        RP["ReportRepository"]
        LR["LookupRepository"]
        DB["Database · DbHelper<br/>SqlClient, typed parameters"]
    end
    subgraph SQL["🗄️ SQL Server"]
        SP["33 stored procedures<br/>CRUD · business rules · reports"]
        T[("CLIENT · VEHICULE · TARIF<br/>LOCATION · UTILISATEUR")]
    end
    LF --> AR
    MF --> CR & VR & RR & RP
    DLG --> CR & VR & RR & LR
    AR & CR & VR & RR & RP & LR --> DB --> SP --> T
```

---

## 🗃️ Data model

```mermaid
erDiagram
    CLIENT ||--o{ LOCATION : rents
    VEHICULE ||--o{ LOCATION : "is rented in"
    TARIF ||--o{ VEHICULE : prices

    CLIENT {
        nvarchar codeclient PK
        nvarchar nom
        nvarchar prenom
        nvarchar adresse
        nvarchar telephone "len >= 8"
        char typeclient "IND | ORG"
    }
    TARIF {
        nvarchar categorie PK
        decimal prix_jour ">= 0"
        decimal prix_km ">= 0"
    }
    VEHICULE {
        nvarchar immatriculation PK
        nvarchar categorie FK
        nvarchar marque
        nvarchar modele
        int kilometrage ">= 0"
        bit disponible
    }
    LOCATION {
        int id PK
        nvarchar codeclient FK
        nvarchar immatriculation FK
        date datedebut
        date datefin ">= datedebut"
        date dateretour "nullable"
        int kmparcourus
        decimal montant
    }
    UTILISATEUR {
        int id PK
        nvarchar username
        nvarchar role "ADMIN | AGENT"
    }
```

Integrity is enforced in the schema with `CHECK` and `FOREIGN KEY` constraints, not only in the UI.

---

## 🔄 Rental lifecycle

```mermaid
sequenceDiagram
    autonumber
    actor A as Agent
    participant F as RentalDialog
    participant R as RentalRepository
    participant S as SQL Server

    A->>F: choose client + dates
    F->>R: GetAvailableVehicles(start, end)
    R->>S: EXEC dbo.GetAvailableVehicles
    S-->>F: vehicles with no overlapping open rental
    A->>F: pick vehicle, confirm
    F->>R: AddLocation(...)
    R->>S: EXEC dbo.AddLocation
    Note over A,S: … rental period …
    A->>F: Return (date, km driven)
    F->>R: ReturnVehicle(id, date, km)
    R->>S: EXEC dbo.ReturnVehicle
    S->>S: days = DATEDIFF + 1<br/>amount = days × prix_jour + km × prix_km
    S->>S: update odometer
    S-->>F: computed amount
```

**Overlap rule** used by `IsVehicleAvailable`: an open rental (`dateretour IS NULL`) blocks the vehicle when `datedebut ≤ requested_end AND datefin ≥ requested_start`.

---

## 📈 Reports

| Stored procedure | Insight |
|---|---|
| `ReportTotalRentalsPerVehicle` | Rentals per vehicle |
| `ReportMostRentedVehicles` | Top vehicles |
| `ReportRevenuePerCategory` | Revenue by tariff category |
| `ReportCategoryPerformance` | Category KPIs |
| `ReportRevenueByPeriod` | Revenue over a date range |
| `ReportRentalsByClient` | Client history |
| `ReportLoyalClients` | Most loyal clients |
| `ReportPendingRentals` | Vehicles not yet returned |
| `ReportVehicleUsageRate` | Fleet utilization |

---

## 🚀 Quick start

Requirements: Windows, .NET 8 SDK, SQL Server (Express or LocalDB) + SSMS.

```sql
-- in SSMS, run in order
:r database/01_schema.sql              -- tables, constraints, indexes, sample data
:r database/02_stored_procedures.sql   -- CRUD + business logic
:r database/03_reports.sql             -- reporting procedures
```

Set the connection string in `EVASION.WinForms/App.config`, then:

```bash
dotnet run --project EVASION.WinForms
```

Demo accounts: `admin / admin123` (ADMIN) · `agent1 / agent123` (AGENT). Full walkthrough in [`QUICKSTART.md`](QUICKSTART.md).

---

## 📁 Project structure

```
.
├── database/
│   ├── 01_schema.sql              # Schema, constraints, indexes, seed data
│   ├── 02_stored_procedures.sql   # CRUD, availability, return & billing, auth
│   └── 03_reports.sql             # 9 reporting procedures
├── EVASION.WinForms/
│   ├── Forms/                     # Login, Main, Client/Vehicle/Rental/Return dialogs
│   ├── Models/                    # Client, Vehicle, Rental, Tariff, User
│   └── DataAccess/                # Repositories + Database/DbHelper
├── docs/                          # Academic report (LaTeX/Markdown), demo guide
└── TEST_DASHBOARD.sql             # Dashboard verification queries
```

---

## 🗺️ Possible improvements

- Store password **hashes** (e.g. PBKDF2 / `HASHBYTES` + salt) instead of plain text
- Wrap `ReturnVehicle` updates in an explicit transaction
- Export reports to PDF / Excel

---

<div align="center">

**Mohamed Abi** · [GitHub @mohamedabi17](https://github.com/mohamedabi17)

</div>
