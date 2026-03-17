# EVASION — Gestion de location de véhicules

Projet académique réalisé avec **C# WinForms (.NET 8)** et **SQL Server** pour gérer les clients, véhicules et locations d'une agence de location.

## Vue d'ensemble

- **Architecture** : WinForms + Repository pattern + procédures stockées SQL Server.
- **Fonctions clés** : authentification, gestion clients/véhicules/locations, retour avec calcul automatique, rapports.
- **Amélioration récente** : tableau de bord statistique (clients, véhicules, locations actives, revenu total).

## Structure essentielle

- `database/01_schema.sql` : schéma, contraintes, données de test, index.
- `database/02_stored_procedures.sql` : procédures CRUD et logique métier.
- `database/03_reports.sql` : procédures de reporting.
- `EVASION.WinForms/` : application desktop.
- `docs/Rapport_Academique.tex` : rapport académique LaTeX.

## Exécution rapide

Pour lancer le projet, suivez les étapes de `QUICKSTART.md`.

## Comptes de démonstration

| Username | Password | Rôle |
|---|---|---|
| `admin` | `admin123` | `ADMIN` |
| `agent1` | `agent123` | `AGENT` |

## Documents de rendu

- `README.md`
- `QUICKSTART.md`
- `PRESENTATION_GUIDE.md`
- `ONE_PAGER.md`

## Remarque

Le projet cible **Windows** (WinForms) avec une base **SQL Server**.
