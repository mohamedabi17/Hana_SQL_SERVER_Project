# 🚀 QUICKSTART - LANCER EVASION EN 5 MINUTES

**Prérequis** : Windows + SQL Server + Visual Studio 2022+

---

## ✅ ÉTAPE 1 : Préparer la Base de Données (2 min)

### Ouvrir SQL Server Management Studio (SSMS)

1. Lancer **SQL Server Management Studio**
2. Connecter à : `(local)` ou `.`
3. Username/Password : Windows Authentication

### Exécuter les scripts SQL

**Script 1 : Créer la base et tables**
```
1. File → Open → File
2. Sélectionner : database/01_schema.sql
3. Appuyer F5 (Execute)
4. Output : "Commands completed successfully"
```

**Script 2 : Créer les procédures stockées**
```
1. File → Open → File
2. Sélectionner : database/02_stored_procedures.sql
3. Appuyer F5 (Execute)
4. Output : "Commands completed successfully"
```

**Script 3 : Créer les rapports**
```
1. File → Open → File
2. Sélectionner : database/03_reports.sql
3. Appuyer F5 (Execute)
4. Output : "Commands completed successfully"
```

### Vérifier la base
```sql
-- Dans SSMS, exécuter :
USE EVASIONDB;
SELECT * FROM dbo.CLIENT;
-- Résultat : 8 clients
```

---

## ✅ ÉTAPE 2 : Ouvrir Visual Studio (1 min)

1. Lancer **Visual Studio 2022**
2. **File** → **Open Folder**
3. Sélectionner : `/home/mohamedabi/Bureau/Hana_SQL_SERVER_Project`
4. Attendre le chargement (2-3 sec)

### Vérifier le projet
```
Solution Explorer (Ctrl+Alt+L) :
├── EVASION.WinForms
│   ├── Program.cs
│   ├── App.config
│   ├── DataAccess/
│   ├── Forms/
│   └── Models/
```

---

## ✅ ÉTAPE 3 : Compiler (1 min)

**Clavier** :
```
Ctrl + Shift + B
```

**Menu** :
```
Build → Build Solution
```

### Attendre l'output
```
========== Build: 1 succeeded ==========
```

✅ Si erreurs → vérifier connection string dans `App.config`

---

## ✅ ÉTAPE 4 : Lancer l'application (1 min)

**Clavier** :
```
F5    (avec debugger)
ou
Ctrl + F5  (sans debugger)
```

**Menu** :
```
Debug → Start Debugging
```

### LoginForm s'ouvre

```
Connexion à l'application EVASION

Username: admin
Password: admin123

Cliquer : "Se connecter"
```

---

## ✅ ÉTAPE 5 : Tester l'application (optionnel)

### Onglet Clients
```
1. Voir la liste (8 clients)
2. Chercher "Benali"
3. Ajouter/Modifier/Supprimer
```

### Onglet Véhicules
```
1. Voir la liste (12 véhicules)
2. Chercher par catégorie "SUV"
```

### Onglet Locations
```
1. Ajouter location
2. OBSERVER : Véhicules changent selon les dates (FEATURE CLÉE ⭐)
3. Sélectionner client et véhicule
4. Retourner location → Voir montant calculé
```

### Onglet Rapports
```
1. Cliquer "Locations/véhicule"
2. Cliquer "Revenu/catégorie"
3. Cliquer "Locations en attente"
```

---

## 🆘 TROUBLESHOOTING

### Erreur : "Connection to SQL Server failed"
```
Solution :
1. Vérifier SQL Server est running
2. Vérifier EVASIONDB existe (SELECT * FROM sys.databases)
3. Vérifier connection string dans App.config
   Server=.;Database=EVASIONDB;Trusted_Connection=True;
```

### Erreur : "Procedure not found"
```
Solution :
1. Exécuter database/02_stored_procedures.sql
2. Vérifier dans SSMS : USE EVASIONDB; EXEC sp_help GetClients;
```

### Application lance mais crash au login
```
Solution :
1. Vérifier credentials (admin/admin123)
2. Vérifier base EVASIONDB a des données
   SELECT * FROM dbo.UTILISATEUR;
```

### Erreur "Microsoft.Data.SqlClient" not found
```
Solution :
1. Attendre NuGet restore complet
2. Ou : dotnet restore EVASION.sln
```

---

## 📊 VÉRIFIER L'INSTALLATION

**Checklist SQL Server** :
```
□ SSMS peut se connecter au serveur local
□ Base EVASIONDB existe
□ Table CLIENT a 8 rows
□ Table UTILISATEUR a 2 rows (admin + agent1)
□ Procédure dbo.GetClients existe
```

**Checklist Visual Studio** :
```
□ Projet compile sans erreurs
□ Aucun warning sur SQL Client
□ Application lance (LoginForm visible)
```

**Checklist Application** :
```
□ LoginForm accepte admin/admin123
□ MainForm s'ouvre (4 onglets visibles)
□ Clients tab : 8 clients affichés
□ Locations tab : Créer location fonctionne
```

---

## 🎯 APRÈS LE DÉMARRAGE

**Pour la soutenance** :
1. Lire ONE_PAGER.md (sur le côté)
2. Relire PRESENTATION_GUIDE.md
3. Tester démo 2-3 fois
4. Être confiant !

**Fichiers importants** :
```
ONE_PAGER.md            ← À imprimer
PRESENTATION_GUIDE.md   ← À relire
TECHNICAL_REVIEW.md     ← Pour questions techniques
```

---

## 💡 TIPS

**Raccourcis Visual Studio** :
```
F5              Lancer (debug)
Ctrl+F5         Lancer (no debug)
Ctrl+Shift+B    Compiler
F7              Aller à la ligne
Ctrl+Alt+L      Solution Explorer
```

**Raccourcis SSMS** :
```
F5              Exécuter query
Ctrl+Shift+M    Disconnect
Ctrl+Shift+O    Open file
```

---

## ✅ RÉSULTAT

Après ces 5 minutes :

✅ Base de données créée et peuplée  
✅ Application compile  
✅ LoginForm fonctionne  
✅ Prêt pour soutenance !

---

**C'est bon ! Vous pouvez tester ! 🚀**

