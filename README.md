## Comment lancer le projet

- Cloner le projet en local **git clone https://github.com/hurtcraft/BlazorGameQuest**
- Déplacer vous dans le dossier: **BlazorGameQuest/BlazorGameClient**
- lancer le projet via **dotnet run**



### =========================================TEST=========================================
## Tests liés au joueur
-  Tester la **création d’un joueur** (nom, classe, points de vie, inventaire vide).  
-  Tester la **connexion** du joueur.  
-  Tester l’**évolution des statistiques** après un mise à jour du score.  
-  Tester la **perte de points de vie** après un combat.  
-  Tester la **mort du joueur** (passage à l’état “Game Over”).    

## Tests liés aux salles et au donjon
-  Tester la **création d’une salle** (identifiant, description, ennemis, trésors).  
-  Tester la **connexion entre salles** (liens nord/sud/est/ouest).  
-  Tester la **génération aléatoire** du donjon.    
-  Tester la **présence d’ennemis ou d’objets** dans une salle.  

## Tests système et logique
-  Tester la **cohérence des données** (ex : pas de salle orpheline, inventaire valide).  
-  Tester les **valeurs par défaut** (PV initiaux, stats minimales).  
-  Tester les **exceptions** (création de joueur sans nom, salle invalide, etc.).  

## Tests d’interface (Blazor)
-  Tester l’**affichage du joueur** (nom, PV).  
-  Tester l’**affichage des salles**.  

## =========================================V1=========================================
- ✅ création du projet Blazor et sa structure de base 
- ✅ implémentation des pages de base (accueil, connexion admin, connexion joueur, leaderboard, joueur, admin)
- ✅ mise en place du routing vers les differentes page apres connexion 
- ✅ Création "nouvelle interface"
- ✅ Création d'un composant Blazor pour une salle statique