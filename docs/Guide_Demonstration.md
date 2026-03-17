# Guide de démonstration — EVASION

## 1. Préparation

### Installer et préparer la base de données

Exécuter les scripts dans cet ordre :
1. `database/01_schema.sql`
2. `database/02_stored_procedures.sql`
3. `database/03_reports.sql`

### Configurer la connexion

Vérifier `EVASION.WinForms/App.config` :
- nom de serveur SQL Server ;
- type d'authentification ;
- nom de la base `EVASIONDB`.

## 2. Lancement de l'application

- Ouvrir `EVASION.sln` dans Visual Studio.
- Restaurer les packages NuGet.
- Définir `EVASION.WinForms` comme projet de démarrage.
- Lancer l'application.

## 3. Scénario de démonstration conseillé

### Étape 1 — Connexion

Utiliser un compte de test :
- `admin / admin123`
- `agent1 / agent123`

Montrer que l'accès passe par une authentification.

### Étape 2 — Gestion des clients

- afficher la liste existante ;
- ajouter un nouveau client ;
- modifier un client ;
- faire une recherche ;
- supprimer si nécessaire.

### Étape 3 — Gestion des véhicules

- afficher les véhicules ;
- ajouter un véhicule ;
- modifier ses informations ;
- rechercher par immatriculation, marque ou catégorie.

### Étape 4 — Gestion des locations

- créer une nouvelle location ;
- sélectionner un client ;
- sélectionner un véhicule ;
- choisir les dates de début et de fin ;
- enregistrer la location.

### Étape 5 — Retour du véhicule

- sélectionner une location non retournée ;
- cliquer sur `Retour véhicule` ;
- saisir la date de retour ;
- saisir le kilométrage parcouru ;
- valider et afficher le montant calculé.

### Étape 6 — Rapports

Présenter successivement :
- locations par véhicule ;
- revenu par catégorie ;
- véhicules les plus loués ;
- locations par client.

## 4. Explication rapide pendant l'oral

### Base de données

Expliquer que :
- les données sont normalisées ;
- les clés primaires et étrangères assurent l'intégrité ;
- les contraintes évitent les incohérences ;
- les procédures stockées centralisent les traitements.

### Application C#

Expliquer que :
- `Models` représente les entités ;
- `DataAccess` gère l'accès SQL Server ;
- `Forms` gère l'interface ;
- `MainForm` centralise la navigation.

### Calcul du montant

Rappeler la formule :

```text
montant = (nombre_de_jours * prix_jour) + (kmparcourus * prix_km)
```

## 5. Questions possibles du jury

### Pourquoi avoir utilisé des procédures stockées ?

Réponse possible : pour centraliser la logique SQL, sécuriser les opérations et simplifier le code C#.

### Pourquoi avoir choisi Windows Forms ?

Réponse possible : parce qu'il s'agit d'une technologie simple, adaptée à un projet académique de bureau et rapide à mettre en œuvre.

### Quelle amélioration proposeriez-vous ?

Réponse possible : sécuriser davantage l'authentification, gérer la disponibilité en temps réel et exporter les rapports en PDF.

## 6. Check-list avant soutenance

- scripts SQL testés sur SQL Server ;
- chaîne de connexion vérifiée ;
- projet Visual Studio s'ouvre correctement ;
- comptes de démonstration connus ;
- scénario de démonstration répété ;
- explication de l'architecture maîtrisée.
