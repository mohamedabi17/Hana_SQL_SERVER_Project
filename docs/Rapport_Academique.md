# Rapport académique — Projet EVASION

## Page de garde

- **Établissement** : À compléter
- **Filière** : À compléter
- **Module** : Développement d'applications / Bases de données
- **Projet** : Application de gestion de location de véhicules
- **Entreprise simulée** : **EVASION**
- **Réalisé par** : À compléter
- **Encadré par** : À compléter
- **Date** : 16 mars 2026

## 1. Introduction

Le présent projet consiste à concevoir et développer une application de bureau Windows en **C# Windows Forms** reliée à une base de données **SQL Server** pour la gestion d'une société de location de véhicules nommée **EVASION**.

L'objectif principal est de digitaliser les opérations essentielles de l'entreprise : gestion des clients, gestion des véhicules, enregistrement des locations, retour des véhicules, calcul automatique des montants et production de rapports d'activité.

## 2. Contexte et problématique

Dans une société de location, plusieurs opérations doivent être suivies avec précision :
- l'identification des clients ;
- la disponibilité et la catégorisation des véhicules ;
- le calcul des frais de location ;
- le suivi des retours ;
- la sécurisation de l'accès via authentification.

Sans système informatisé, ces opérations deviennent difficiles à contrôler, augmentent le risque d'erreurs et ralentissent le travail administratif.

## 3. Objectifs du projet

### Objectif général

Développer une application de gestion simple, claire et fonctionnelle pour automatiser le processus de location de véhicules.

### Objectifs spécifiques

- gérer les clients, qu'ils soient particuliers ou organisations ;
- gérer les véhicules et leurs catégories ;
- gérer les tarifs selon la catégorie ;
- enregistrer les locations ;
- traiter le retour des véhicules ;
- calculer automatiquement le montant à payer ;
- produire des rapports d'exploitation ;
- contrôler l'accès par login et mot de passe.

## 4. Analyse des données

Le système repose sur cinq entités principales.

### 4.1 Client

Un client est identifié par un code unique et peut être :
- **IND** : individu ;
- **ORG** : organisation.

Attributs :
- `codeclient`
- `nom`
- `prenom`
- `adresse`
- `telephone`
- `typeclient`

### 4.2 Véhicule

Chaque véhicule est identifié par son immatriculation.

Attributs :
- `immatriculation`
- `categorie`
- `marque`
- `modele`
- `kilometrage`

### 4.3 Tarif

Chaque catégorie de véhicule possède un tarif journalier et un tarif kilométrique.

Attributs :
- `categorie`
- `prix_jour`
- `prix_km`

### 4.4 Location

Une location relie un client à un véhicule sur une période donnée.

Attributs :
- `id`
- `codeclient`
- `immatriculation`
- `datedebut`
- `datefin`
- `dateretour`
- `kmparcourus`
- `montant`

### 4.5 Utilisateur

Les utilisateurs de l'application sont stockés pour l'authentification.

Attributs :
- `id`
- `username`
- `password`
- `role`

## 5. Règles de gestion

Les principales règles de gestion sont les suivantes :
- un client possède un identifiant unique ;
- un véhicule appartient obligatoirement à une catégorie définie dans la table `TARIF` ;
- une location associe un client et un véhicule ;
- la date de fin doit être supérieure ou égale à la date de début ;
- la date de retour peut être nulle tant que le véhicule n'est pas rendu ;
- le kilométrage parcouru et le montant sont renseignés au retour ;
- le montant de location est calculé par la formule :

```text
montant = (nombre_de_jours * prix_jour) + (kmparcourus * prix_km)
```

## 6. Choix techniques

### Langages et outils

- **C#** pour l'application de bureau ;
- **Windows Forms** pour l'interface graphique ;
- **SQL Server** pour la base de données ;
- **Stored Procedures** pour sécuriser et structurer l'accès aux données ;
- **ADO.NET / SqlConnection** pour la communication entre l'application et la base de données.

### Justification des choix

Windows Forms a été retenu car il permet de réaliser rapidement une interface de bureau claire et adaptée à un projet académique. SQL Server est bien adapté au stockage relationnel et à la mise en place de procédures stockées.

## 7. Architecture du projet

Le projet a été organisé de manière structurée :

- `Models/` : représente les entités métier ;
- `DataAccess/` : gère la connexion et les appels aux procédures stockées ;
- `Forms/` : contient toutes les interfaces utilisateur ;
- `Program.cs` : point d'entrée de l'application ;
- `database/` : scripts SQL ;
- `docs/` : documents du rendu académique.

## 8. Description fonctionnelle de l'application

### 8.1 Authentification

L'application démarre sur un formulaire de connexion. L'utilisateur saisit son nom d'utilisateur et son mot de passe. Si les identifiants sont corrects, l'accès au tableau de bord est autorisé.

### 8.2 Tableau de bord principal

Le tableau de bord utilise une navigation par onglets :
- **Clients** ;
- **Véhicules** ;
- **Locations** ;
- **Rapports**.

### 8.3 Gestion des clients

Cet onglet permet de :
- afficher la liste des clients ;
- ajouter un client ;
- modifier un client ;
- supprimer un client ;
- rechercher un client.

### 8.4 Gestion des véhicules

Cet onglet permet de :
- afficher la liste des véhicules ;
- ajouter un véhicule ;
- modifier un véhicule ;
- supprimer un véhicule ;
- rechercher un véhicule.

### 8.5 Gestion des locations

Cet onglet permet de :
- afficher les locations ;
- créer une location ;
- modifier une location ;
- supprimer une location ;
- rechercher une location ;
- enregistrer le retour du véhicule.

### 8.6 Rapports

L'application affiche plusieurs rapports simples :
- total des locations par véhicule ;
- revenu par catégorie ;
- véhicules les plus loués ;
- locations par client.

## 9. Rôle des procédures stockées

Les procédures stockées jouent un rôle central dans l'application. Elles permettent de :
- centraliser la logique SQL ;
- simplifier les appels côté C# ;
- mieux sécuriser les opérations ;
- faciliter la maintenance du projet.

Les procédures CRUD ont été réalisées pour :
- `CLIENT`
- `VEHICULE`
- `LOCATION`

Des procédures complémentaires ont aussi été ajoutées pour :
- l'authentification ;
- le retour d'un véhicule ;
- les rapports.

## 10. Exemple de calcul du montant

Considérons un véhicule de catégorie **COMPACTE** avec :
- `prix_jour = 180`
- `prix_km = 1.5`

Si la location dure **3 jours** et que le véhicule parcourt **200 km**, alors :

```text
montant = (3 × 180) + (200 × 1.5)
montant = 540 + 300
montant = 840
```

## 11. Sécurité et limites

### Points positifs

- authentification utilisateur ;
- contraintes SQL pour protéger l'intégrité des données ;
- calcul automatique du montant ;
- architecture claire.

### Limites académiques

- le mot de passe est stocké en clair dans cette version pédagogique ;
- la gestion fine de disponibilité des véhicules peut être améliorée ;
- les rapports sont simples et présentés dans des grilles.

## 12. Améliorations possibles

Dans une version plus avancée, on pourrait ajouter :
- le chiffrement des mots de passe ;
- une gestion de réservation ;
- un filtrage de disponibilité des véhicules ;
- l'impression ou l'export PDF des rapports ;
- des statistiques graphiques.

## 13. Conclusion

Ce projet a permis de mettre en pratique les notions de modélisation de données, de programmation Windows Forms, de communication avec SQL Server et d'organisation logicielle en couches.

La solution obtenue répond aux besoins essentiels de la société EVASION et constitue une base solide pour une démonstration universitaire claire, structurée et complète.
