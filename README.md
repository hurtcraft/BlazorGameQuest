## Groupe
Patrick Wu, Julien Weng

## Comment lancer le projet

- Cloner le projet en local **git clone https://github.com/hurtcraft/BlazorGameQuest**
### pour le Back end
- Déplacer vous dans le dossier: **BlazorGameQuest/BlazorGameQuestGameApi**
- lancer le projet via **dotnet run**
### pour le Front end
- Déplacer vous dans le dossier: **BlazorGameQuest/BlazorGameClient** 
- lancer le projet via **dotnet run**



## TEST
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

## V1
- ✅ création du projet Blazor et sa structure de base 
- ✅ implémentation des pages de base (accueil, connexion admin, connexion joueur, leaderboard, joueur, admin)
- ✅ mise en place du routing vers les differentes page apres connexion 
- ✅ Création "nouvelle interface"
- ✅ Création d'un composant Blazor pour une salle statique


## V2
- ✅ Définition des principaux model (Joueurs, Admin, Donjon)
- ✅ Implémentation de EFCore InMemory
- ✅ Mise en place du routing vers les differentes page apres connexion 
- ✅ Création des microservices
- ✅ Ajout swagger
- ✅ Test via swagger IHM


## V3
- ✅ Rendez vous directement dans http://localhost:5000/game pour jouer
    - Déplacement avec les flèches directionnelles
    - Attaque avec la touche J
    - Les potions rouges rendent des pts de vie
    - Les potions bleus sont des poisons et enlèvent des pts de vie
    - Les passages secret peuvent être vos amis comme ennemies
    - Gare au monstres qui rodent dans le Donjon, en tuer vous rapporte des points
    - Les pièces et les clefs vous rapporte également des points 
    - Lorsque vous mourrez ou finissez le jeu, un écran de fin s'affiche en conséquence.
    - Vous pouvez alors soit rejouer directement dans ce cas la sauvegarde de l'état du joueur ne se fait pas, soit sauvegarder, dans ce cas l'état du joueur est sauvegardé et vous serez redirigé vers le tableau des scores 

- ✅ Rendez vous dans http://localhost:5000/mapEditor pour editer des niveaux
    - Selectionner les items à placer en deroulant l'onglet "mapAssets"
    - Placer les élèments de jeu via le click gauche, pauser plusieurs élèments en maintenant le click gauche
    - Effacer les élèments via le click droit, effacer plusieurs élèments en maintenant le click droit
    - Certains élèments ne sont pas totalement implémentés mais le necessaire l'est
    - Une fois satisfait de votre donjon, cliquer sur déployer map pour la sauvegarder coté serveur
    - Vous pouvez également éditer d'ancien donjon via l'onglet "charger Map"
    - N'oubliez pas de rafraichir l'onglet "charger Map" via le bouton ⟳ en haut à gauche de l'onglet. Cette opération est utile après chaque déploiement de Donjon.

## V4
- ✅ tableau de bord admin
- ✅ controle des joueurs
- ✅ création des maps
- ✅ export des joueurs


## assets utilisés
- https://xzany.itch.io/top-down-adventurer-character
- https://pixel-poem.itch.io/dungeon-assetpuck


## Déploiement avec Docker

### Prérequis
- Docker Desktop installé et en cours d'exécution

### Commandes Docker

**Première fois ou après modification du code :**
docker-compose up --build

**Les fois suivantes (si rien n'a changé) :**
docker-compose up

**Démarrer en arrière-plan :**
docker-compose up -d

### Accès à l'application
- **Client Blazor** : http://localhost:5000
- **API** : http://localhost:5001
- **Swagger UI** : http://localhost:5001/swagger