# Coir

**Coir** is an OpenSimulator distribution: stock [OpenSimulator](http://opensimulator.org/) "Core" (0.9.3.x, .NET 8)
plus a small, reviewable set of feature branches, built for the
[Migrating Coconuts](https://world.migratingcoconuts.net) grid. Coir is the tough fibre of the coconut husk — strong,
plain and useful — and that is the idea: **Core, with the extra features people have been getting from other forks,
kept as thin patches on top so it can follow upstream.**

Coir is not affiliated with or endorsed by the OpenSimulator project. The upstream OpenSimulator README (build and run instructions) is kept as [`README-OpenSim.md`](README-OpenSim.md) and `BUILDING.md`.

## What is in it

Each item is its own topic branch in this repository, so you can read, cherry-pick or drop it:

| Feature | Branch | Notes |
|---|---|---|
| Display names (viewer cap + storage) | `feature/display-names` | Ported onto Core from OpenSim-NGC's implementation (#94) |
| Experiences | `feature/experiences` | Schema-neutral against existing databases |
| MySqlConnector data driver | `feature/mysqlconnector` | Verified against Oracle MySQL 8.0 |
| Gloebit money module | `feature/gloebit` | Add-on module, LGPL-3.0 (see its `LICENSE`) |
| OpenSimSearch | `feature/opensimsearch-clean` | Add-on module, taken from OpenSim-NGC/Tranquillity v1.0.49 (no licence file of its own; see Credit) |
| HTTP listener bind address + port range | `feature/ip-binding-port-range` | |
| Control-plane security hardening | `feature/security-pr210` | Port of the upstream Tranquillity hardening work |
| Extra OSSL/LSL functions | `feature/ossl-lsl-additions` | |
| SQLite/standalone compatibility fixes | `feature/standalone-sqlite-compat` | |
| Experience teleports bypass landing points | `feature/experience-landing-points` | Teleports under an allowed Experience use ViaLure |
| Optional password-less IAR load/save | `feature/passwordless-iar` | `[InventoryArchiver] RequirePassword=false`; default `true` = stock behaviour |
| Access control (ban list) | `feature/access-control` | Optional DB-backed IP / viewer MAC / id0 ban list for login and the Hypergrid gatekeeper; off unless `AccessControlService` is set |
| Keep existing map tile at region start | `feature/maptile-if-missing` | `[Map] OnlyGenerateIfMissing = true`; default `false` = stock behaviour |
| LSL / movement fixes | `feature/lsl-speed-tweaks` | `llGodLikeRezObject` requires a present owner in god mode; `SpeedModifier` scales X/Y only |

`mc-clean` is the integration branch that releases are built from.

## Credit

Coir exists because other people did the hard work first:

* **The OpenSimulator project and every one of its contributors** — Coir *is* Core with a few branches on top: the physics
  (ubODE), the libomv libraries, the region and grid services, everything not listed below is theirs. See `LICENSE.txt` and
  `CONTRIBUTORS.txt` for the full list. (Individual Core developers are deliberately not singled out: we credit all of
  them, not some of them.)
* **StolenRuby (Mobius Grid)** — the **HTTP listener IP binding** and **auto port-range** (Consortium fork, August 2020; Coir's `feature/ip-binding-port-range` is a direct re-base of that design and code), the **Experiences** work carried through OpenSim-NGC, and several small fixes that Core itself adopted (Top Scripts, EEP defaults, `llTargetedEmail`, attachment undo positions). StolenRuby also wrote an earlier, separate display-names implementation for Consortium (2020). (StolenRuby was with Mobius Grid at the time of this work.)
* **Mike Chase (Utopia Skye)** — leads OpenSim-NGC / Tranquillity; the **display-names** implementation (#94) and its migration fix are his, and Coir's `feature/display-names` is that code ported onto Core. His NGC control-plane security hardening (#210) is the basis of `feature/security-pr210`.
* **The OpenSim-NGC / Tranquillity project** — the control-plane hardening (PR #210) and the Gloebit / OpenSimSearch add-on modules these ports started from. (No Skia/CoreJ2K imaging work and no PR #252 texture fix is in Coir.)
* **OpenSimSearch** — taken from OpenSim-NGC/Tranquillity v1.0.49 (`Addons/OpenSimSearch`), which carries no licence file or header of its own; it sits in a repository under the OpenSimulator BSD licence, and its `query.php` carries the notice "Copyright (c) Melanie Thielker (http://opensimulator.org/)". Coir's changes to it are small: Mono.Addins registration, Core build wiring, and treating a search reply with no `data` member as "no results" (reported upstream as OpenSim-NGC/OpenSim-Tranquillity#209). If you are the author or a rights holder and would like it credited differently, please open an issue.
* **Gloebit** — the Gloebit OpenSim money module, Copyright (C) 2015 Gloebit LLC, LGPL-3.0, carried via OpenSim-NGC/Tranquillity v1.0.49 with its licence file unchanged.
* **MySqlConnector** (Bradley Grainger and contributors, MIT licence) — the MySQL driver in Coir, replacing Oracle's `MySql.Data` that Core ships with. It is their work and Coir only uses it unmodified.

### Whose commits are carried on top of Core

Generated from `git log origin/master..mc-clean` at each release (names are in-world names where known). Contributors to
Core itself are credited collectively above.

| Contributor | Carried in | What |
|---|---|---|
| KofiBeanery (Migrating Coconuts) | all branches | Porting, integration, build and release |
| Mike Chase (Utopia Skye) | `feature/display-names`, `feature/security-pr210`, `feature/experiences` (merge of StolenRuby's work, #86) | Display names (#94) + migration fix; control-plane hardening (#210); OpenSim-NGC / Tranquillity |
| StolenRuby (Mobius Grid) | `feature/ip-binding-port-range`, `feature/experiences` | IP binding and port range (Consortium, 2020); Experiences |
| Fly Man and Marty Mouse | `feature/experiences` | Experiences fixes (incl. a permission fix) |

If we have missed or mis-credited anyone, please open an issue and we will fix it.

## Releases

Each release is tagged `<upstream>-mc<n>` (for example `0.9.3.1-mc1`) and carries:

* **Source** — the tagged tree (GitHub's source archive).
* **Linux x64 binary package** — `coir-<upstream>-mc<n>-<sha>.tar.gz` with a `.sha256`. Framework-dependent: needs the .NET 8 runtime.
  No Windows or macOS binaries are published; they are untested.

## Status

Pre-release. Running in production on a handful of Migrating Coconuts regions; no warranty (see `LICENSE.txt`).
Back up your databases before running any new OpenSimulator build against them.

## Licence

BSD-3-Clause, inherited from OpenSimulator (`LICENSE.txt`); bundled add-on modules keep their own licences
(Gloebit: LGPL-3.0). Redistributions must keep those notices.
