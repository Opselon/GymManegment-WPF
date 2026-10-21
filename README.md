# GymPro

A gym management system for Iranian gyms, rebuilt from the ground up on .NET 10.

This repository began life as a 2021 university project: a Windows Forms-era WPF
app with an EF6 EDMX model, hard-coded absolute paths to a Telerik installation
on the author's old desktop, and a three-DES password scheme keyed from a config
file. It no longer compiled on any machine that still exists.

It is now a layered .NET 10 application with a tested domain model, and the old
code is kept below for reference.

![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4)
![Platform](https://img.shields.io/badge/platform-Windows-0078D4)
![Tests](https://img.shields.io/badge/tests-78%20passing-2ea44f)
![Warnings](https://img.shields.io/badge/warnings-0-2ea44f)
![License](https://img.shields.io/badge/license-MIT-blue)

---

## Why it was rewritten

The original solution could not be upgraded in place. Every one of these was a
hard blocker, not a style preference:

| Problem | Consequence |
| --- | --- |
| DLL references to `C:\Users\Shadow\Desktop\…` and `J:\Binaries\WPF45\Telerik…` | Restore fails on any machine but the author's |
| .NET Framework 4.5, WPF + EF6 `.edmx` | Unsupported toolchain; no .NET 10 path |
| Passwords encrypted with 3-DES-ECB, key derived from an MD5 of a config value | Recoverable password store, ECB mode leaks structure |
| Business logic in `Form_Load` and click handlers | Nothing testable outside the UI |
| Vendored copies of MaterialDesign and MahApps source | 25 MB of third-party code to maintain |
| No tests, no CI, no README | No way to know whether a change broke anything |

The rewrite keeps the domain knowledge — the Jalali calendar rules, the
membership and payment model, the Persian UI — and discards the structure.

---

## Architecture

Four layers, dependencies pointing inward. The domain knows nothing about the
database, the UI, or the clock.

```
GymPro.Domain          entities, value objects, invariants — pure C#
   ▲
GymPro.Application     use cases (commands/queries), validation, ports
   ▲
GymPro.Persistence     EF Core 10: DbContext, mappings, repositories
GymPro.Infrastructure  PBKDF2 hashing, QR codes, Excel, backups
   ▲
GymPro.Presentation    WPF, MVVM via CommunityToolkit.Mvvm
```

```
src/
  GymPro.Domain/          Aggregate roots, value objects, domain events
  GymPro.Application/     CQRS handlers, FluentValidation validators, ports
  GymPro.Persistence/     EF Core mappings, repositories, unit of work
  GymPro.Infrastructure/  Cross-cutting adapters with no domain knowledge
  GymPro.Presentation/    WPF shell, views, view models, DI composition
tests/
  GymPro.Domain.Tests/        46 tests — calendar, money, membership rules
  GymPro.Application.Tests/   24 tests — handlers, validators, dispatcher
  GymPro.Persistence.Tests/    8 tests — real SQLite round-trips
```

### Design decisions

**The domain has no framework references.** `GymPro.Domain.csproj` references no
NuGet package at all. That is what makes the invariants testable in milliseconds
and what stops EF Core's habits from leaking into the model.

**Value objects instead of primitives.** `JalaliDate` wraps
`System.Globalization.PersianCalendar` rather than hand-rolled leap-year
arithmetic. `Money` stores rial as an integer amount, because `decimal` currency
rounding in a payment ledger is a bug waiting to happen.

**CQRS without a mediation library.** Commands and queries are plain records;
handlers are resolved from DI by their closed generic type. `RequestDispatcher`
is 46 lines and has no reflection-based assembly scanning. Skipping MediatR
removes a licensing question and a version-compatibility risk for no loss.

**Aggregates enforce their own rules.** `Membership.RegisterNew` rejects a
national code that is not ten digits, a duration outside 1–36 months, and an
empty name. The validator duplicates some of this on purpose: validation gives a
field-level message before the transaction starts, and the aggregate remains the
final authority.

**Domain events are raised inside the aggregate, not by callers.** A payment adds
a `PaymentRecorded` event. Identity is assigned by the store, so events carry
data, not a pre-insert id that would be zero.

**Failures are values, not exceptions.** `Result<T>` carries a `DomainError` with
a `ErrorKind` (Validation, NotFound, Conflict). Only programmer errors throw.

---

## The Pro modules

The original app covered registration and payment. "Pro" here means the
operational features a gym actually runs on day to day. They are in the
repository, not behind a licence check — there is no paywall and no tier flag.

Status is stated honestly: "domain" means the model, rules and persistence are
implemented and tested; "UI" means there is a screen for it.

| Module | What it does | Domain | UI |
| --- | --- | --- | --- |
| Members | Registration, search, national-code uniqueness | Implemented | Implemented |
| Memberships | Term, renewal, lapse detection (`ExpireIfLapsed`) | Implemented | Partial — register and list |
| Payments | Ledger per member, outstanding balance | Implemented | Partial — via charge command |
| Dashboard | Active members, expiring this month, total debt | Implemented | Implemented |
| Staff accounts | Roles (Admin, Coach, Receptionist), PBKDF2 sign-in | Implemented | Sign-in only |
| Attendance | Immutable check-in log, expired-member guard, QR payload rendering | Implemented | Implemented — check in, see today's log |
| Body measurements | Per-member progress, nullable metrics | Implemented | Partial — handlers and storage; no screen |
| Reporting | Excel export of the member list | Implemented | Implemented — save dialog |
| Backup | `BACKUP DATABASE … WITH COPY_ONLY`, retention pruning | Implemented | Not yet |

First run creates the database and seeds a single administrator account
(`admin` / `admin`) so a fresh clone can be signed into; change that password
before the app is used for real.

What remains for the modules still marked partial or "Not yet": a measurement
entry screen over `RecordMeasurementCommand`, and a backup screen over
`SqlServerBackupService`. Both are screens over handlers that already exist and
are tested; no new business rules are needed.

Check-in and check-out is recorded as check-in only: the sheet has no exit time
column, and inventing one would change the domain model rather than add a
screen.

What is deliberately *not* built: no SMS gateway, no online payment processor,
no multi-branch tenancy, and no automatic migration of the old database. Each
needs a provider account, credentials, or a data dump this repository does not
have — guessing at them would be worse than a documented gap.

---

## Security

The original scheme is gone, not ported.

- **Passwords**: PBKDF2-HMAC-SHA256, 210 000 iterations, per-user 128-bit salt,
  256-bit subkey, compared in fixed time. Iteration count and salt travel with
  each hash, so it can be raised later without invalidating existing accounts.
- **Removed**: 3-DES-ECB with an MD5-derived key. ECB is deterministic —
  identical passwords produced identical ciphertext — and the key was
  recoverable from the config file.
- **Secrets**: never committed. Connection strings come from configuration or
  environment variables.

---

## Getting started

Requires Windows and the .NET 10 SDK. The exact SDK version is pinned in
`global.json`.

```bash
git clone https://github.com/Opselon/GymManegment-WPF.git
cd GymManegment-WPF

dotnet restore GymPro.slnx
dotnet build GymPro.slnx -c Release
dotnet test  GymPro.slnx -c Release
dotnet run   --project src/GymPro.Presentation
```

The connection string is read from `appsettings.json` under the `ConnectionStrings:GymPro`
key, or from the `GYMPRO__CONNECTIONSTRING` environment variable.

Schema management uses `dotnet ef`:

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add Initial --project src/GymPro.Persistence --startup-project src/GymPro.Presentation
dotnet ef database update      --project src/GymPro.Persistence --startup-project src/GymPro.Presentation
```

The persistence tests build the schema with `EnsureCreated` against SQLite, so the
mappings are exercised in CI without needing a SQL Server instance.

### Quality gates

The build is configured to fail rather than warn — `TreatWarningsAsErrors`,
`AnalysisLevel=latest-recommended`, and `GenerateDocumentationFile` are on for
every project. The command below must exit 0:

```bash
dotnet build GymPro.slnx -c Release
```

CI runs build, test, `dotnet format --verify-no-changes`, and a NuGet
vulnerability audit where known-vulnerable packages fail the build.

---

## Migrating from the original

The old tree is preserved at `GymManegment/` and `DataModelLayer/` and is not
part of `GymPro.slnx`.

| Original | Replacement |
| --- | --- |
| `Members` (EDMX entity) | `Membership` aggregate + `Payments` ledger |
| `Users` with 3-DES password column | `User` with PBKDF2 hash |
| `UsersTimeOut` (login session rows) | ASP.NET-style session, not modelled yet |
| `LockTable` (manual row locks) | EF Core transaction + optimistic concurrency |
| `VW_Member`, `VW_Customers` SQL views | Read models projected in query handlers |
| Gregorian `DateTime` columns | `JalaliDate` value object |

Existing data is not migrated automatically: the old schema stored dates as
Gregorian `DateTime` and names as unterminated `nvarchar`, so a dump needs a
one-off script that this repository does not include.

---

## License

MIT. See [LICENSE](LICENSE).
