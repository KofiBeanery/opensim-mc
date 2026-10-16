# Gloebit on OpenSim Core — porting notes

Source: OpenSim-NGC/OpenSim-Tranquillity tag `v1.0.49`, `Addons/Gloebit.GloebitMoneyModule`
(MPL-2.0 / see LICENSE). Changes versus NGC:

- Removed: PostgreSQL implementations + `*PGSQL.migrations` (not used here), NGC `PluginRegistration.cs`
  and `.csproj` (NGC's own plugin registry; Core uses Mono.Addins).
- Added: `prebuild.xml` (Core's build), `Properties/AssemblyInfo.cs` (Mono.Addins `[assembly: Addin]`),
  `[Extension]` on `GloebitMoneyModule`, `GlobalUsings.cs` (NGC enables ImplicitUsings, Core does not).
- `MainServer.Instance.DefaultServer` -> `MainServer.Instance` (Core has no `DefaultServer`).
- Needs the MySqlConnector driver (`feature/mysqlconnector`).

MC hardening (second commit): the Gloebit hunks of MC patch 40 (NGC PR #210) are applied — OAuth `state`
(XSRF) check on the authorization callback and a per-transaction callback key. They add migrations
`GloebitUsers` v4 (`PendingAuthState`) and `GloebitTransactions` v3 (`CallbackKey`), which is exactly what
the live MC databases are already stamped at (Users 4 / Transactions 3 / Subscriptions 1).

Tested: builds on .NET 8; module discovered; `GloebitUsers/Transactions/Subscriptions` migrations
create the tables on Oracle MySQL 8.0.46. Not tested: real Gloebit API calls, payments, land sales.
Copy `Gloebit.ini.example` to `bin/config-include/` yourself (the module's auto-copy looks in `bin/`).
