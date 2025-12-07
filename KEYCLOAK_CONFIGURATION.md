# Guide de Configuration Keycloak pour BlazorGameQuest

Ce guide vous explique étape par étape comment configurer Keycloak pour sécuriser votre application BlazorGameQuest.

## Prérequis

- Docker Desktop installé et en cours d'exécution
- Le projet BlazorGameQuest configuré avec Keycloak dans docker-compose.yml

## Étape 1 : Démarrer les services Docker

1. Ouvrez un terminal dans le répertoire du projet
2. Exécutez la commande suivante pour démarrer tous les services (y compris Keycloak) :

```bash
docker-compose up --build
```

3. Attendez que tous les services soient démarrés. Keycloak peut prendre quelques minutes pour être complètement prêt.

## Étape 2 : Accéder à la console d'administration Keycloak

1. Ouvrez votre navigateur et accédez à : **http://localhost:8080**
2. Cliquez sur **Administration Console** en haut à droite
3. Connectez-vous avec :
   - **Username** : `admin`
   - **Password** : `admin`

## Étape 3 : Créer un Realm

1. Dans le menu de gauche, survolez le realm par défaut (Master) en haut à gauche
2. Cliquez sur **Create Realm** (ou **Créer un royaume**)
3. Entrez le nom : `blazorgamequest`
4. Cliquez sur **Create** (ou **Créer**)
5. Le nouveau realm est maintenant sélectionné

## Étape 4 : Créer les Rôles

Les rôles doivent être créés dans le realm `blazorgamequest`. Voici comment procéder :

### Méthode 1 : Via le menu Realm Roles (Recommandé)

1. **Assurez-vous d'être dans le bon realm** :
   - En haut à gauche, vérifiez que le realm sélectionné est **blazorgamequest** (pas "Master")
   - Si ce n'est pas le cas, cliquez sur le nom du realm en haut à gauche et sélectionnez **blazorgamequest**

2. **Accéder à Realm Roles** :
   - Dans le menu de gauche, cherchez **Realm roles** (ou **Rôles du royaume**)
   - **IMPORTANT** : Ne confondez pas avec "Roles" qui se trouve sous "Realm settings"
   - Cliquez directement sur **Realm roles** dans le menu principal de gauche

3. **Créer le rôle "joueur"** :
   - Vous devriez voir un tableau avec les rôles existants (il peut être vide au début)
   - Cliquez sur le bouton **Create role** (ou **Créer un rôle**) en haut à droite du tableau
   - Dans le formulaire qui s'ouvre :
     - **Role name** : Entrez exactement `joueur` (en minuscules)
     - **Description** : (optionnel) "Rôle pour les joueurs du jeu"
     - Laissez les autres options par défaut
   - Cliquez sur **Save** (ou **Enregistrer**)
   - Vous devriez maintenant voir le rôle "joueur" dans la liste

4. **Créer le rôle "admin"** :
   - Cliquez à nouveau sur **Create role** (ou **Créer un rôle**)
   - Dans le formulaire :
     - **Role name** : Entrez exactement `admin` (en minuscules)
     - **Description** : (optionnel) "Rôle administrateur avec accès complet"
   - Cliquez sur **Save** (ou **Enregistrer**)
   - Vous devriez maintenant voir les deux rôles : "joueur" et "admin" dans la liste

### Méthode 2 : Via Realm Settings (Alternative)

Si vous ne trouvez pas "Realm roles" directement dans le menu :

1. Dans le menu de gauche, allez dans **Realm settings** (ou **Paramètres du royaume**)
2. Cliquez sur l'onglet **Realm Roles** (ou **Rôles du royaume**) en haut de la page
3. Suivez ensuite les étapes 3 et 4 de la Méthode 1 ci-dessus

### Vérification

Après avoir créé les deux rôles, vous devriez voir dans la liste des rôles :
- ✅ **joueur**
- ✅ **admin**

Si vous ne voyez pas ces rôles, vérifiez que :
- Vous êtes bien dans le realm **blazorgamequest** (pas "Master")
- Les noms des rôles sont exactement en minuscules : `joueur` et `admin`
- Vous avez bien cliqué sur "Save" après chaque création

## Étape 5 : Créer un Client pour l'application Blazor

1. Dans le menu de gauche, allez dans **Clients** (ou **Clients**)
2. Cliquez sur **Create client** (ou **Créer un client**)
3. Configurez le client :
   - **Client type** : Sélectionnez **OpenID Connect**
   - Cliquez sur **Next** (ou **Suivant**)
4. Configurez les paramètres du client :
   - **Client ID** : `blazor-client`
   - Cliquez sur **Next** (ou **Suivant**)
5. Configurez les capacités du client :
   - Cochez **Client authentication** : **OFF** (pour un client public)
   - Cochez **Authorization** : **OFF**
   - Cochez **Standard flow** : **ON** (nécessaire pour OIDC)
   - Cochez **Direct access grants** : **ON** (pour permettre la connexion directe)
   - Cliquez sur **Next** (ou **Suivant**)
6. Configurez les URLs de redirection :
   - **Valid redirect URIs** : Ajoutez les URLs suivantes (une par ligne) :
     ```
     http://localhost:5000/authentication/login-callback
     http://localhost:5000/authentication/logout-callback
     ```
   - **Web origins** : Ajoutez :
     ```
     http://localhost:5000
     ```
   - Cliquez sur **Save** (ou **Enregistrer**)
7. **Configurer l'audience du token d'accès** (IMPORTANT pour que l'API accepte le token) :
   - Dans la page du client `blazor-client`, allez dans l'onglet **Advanced settings** (ou **Paramètres avancés**)
   - Trouvez la section **Fine Grain OpenID Connect Configuration**
   - Dans le champ **Access Token Audience** (ou **Audience du token d'accès**), ajoutez :
     ```
     blazor-client
     ```
   - **IMPORTANT** : Cela garantit que le token d'accès contient `blazor-client` comme audience, ce qui est nécessaire pour que l'API puisse valider le token
   - Cliquez sur **Save** (ou **Enregistrer**)

## Étape 6 : Configurer les Mappers pour les Rôles (CRITIQUE pour l'accès admin)

**IMPORTANT** : Cette étape est essentielle pour que les rôles fonctionnent correctement dans l'application.

1. Dans la page du client `blazor-client`, allez dans l'onglet **Client scopes** (ou **Portées du client**)
2. Cliquez sur le scope **roles** dans la liste (pas sur le client lui-même, mais sur le scope)
3. Allez dans l'onglet **Mappers** (ou **Mappeurs**)
4. **Vérifiez s'il existe déjà un mapper "realm roles"** :
   - Si **OUI** : Vérifiez qu'il a les bonnes configurations (voir ci-dessous)
   - Si **NON** : Créez-le en suivant les étapes ci-dessous

5. **Créer ou modifier le mapper "realm roles"** :
   - Cliquez sur **Add mapper** (ou **Ajouter un mappeur**) si absent, ou cliquez sur le mapper existant pour le modifier
   - Sélectionnez **By realm role** (ou **Par rôle du royaume**)
   - Configurez comme suit :
     - **Name** : `realm roles`
     - **Token Claim Name** : `roles` (IMPORTANT : exactement "roles")
     - **Claim JSON Type** : `String` (ou `JSON` si vous voulez un tableau)
     - **Add to ID token** : **ON** ✅
     - **Add to access token** : **ON** ✅ (TRÈS IMPORTANT)
     - **Add to userinfo** : **ON** ✅
     - **Multivalued** : **ON** (si vous avez plusieurs rôles)
   - Cliquez sur **Save** (ou **Enregistrer**)

6. **Vérification** :
   - Le mapper doit apparaître dans la liste des mappers du scope "roles"
   - Assurez-vous que "Add to access token" est bien activé

## Étape 7 : Créer des Utilisateurs

### Créer un utilisateur joueur

1. Dans le menu de gauche, allez dans **Users** (ou **Utilisateurs**)
2. Cliquez sur **Create new user** (ou **Créer un nouvel utilisateur**)
3. Remplissez les informations :
   - **Username** : `joueur1`
   - **Email** : `joueur1@example.com` (optionnel)
   - **First name** : `Joueur` (optionnel)
   - **Last name** : `Un` (optionnel)
   - Activez **Email Verified** si vous avez fourni un email
   - Activez **Enabled**
   - Cliquez sur **Create** (ou **Créer**)
4. Configurez le mot de passe :
   - Allez dans l'onglet **Credentials** (ou **Identifiants**)
   - Cliquez sur **Set password** (ou **Définir le mot de passe**)
   - **Password** : Entrez un mot de passe (ex: `joueur1`)
   - **Password confirmation** : Confirmez le mot de passe
   - Désactivez **Temporary** (pour que l'utilisateur n'ait pas à changer le mot de passe à la première connexion)
   - Cliquez sur **Save** (ou **Enregistrer**)
   - Confirmez dans la popup
5. Attribuer le rôle joueur :
   - Allez dans l'onglet **Role mapping** (ou **Attribution de rôles**)
   - Cliquez sur le bouton **Assign role** (ou **Attribuer un rôle**) en haut à droite
   - Une fenêtre modale s'ouvre avec une liste de rôles
   - **IMPORTANT** : En haut de cette fenêtre, il y a des filtres. Cliquez sur **Filter by realm roles** (ou **Filtrer par rôles du royaume**)
   - Vous devriez maintenant voir les rôles "joueur" et "admin" dans la liste
   - Cochez la case à côté de **joueur**
   - Cliquez sur **Assign** (ou **Attribuer**) en bas de la fenêtre
   - Le rôle "joueur" devrait maintenant apparaître dans la liste des rôles assignés à cet utilisateur

### Créer un utilisateur admin

1. Répétez les étapes 1-4 ci-dessus avec :
   - **Username** : `admin1`
   - **Email** : `admin1@example.com` (optionnel)
   - **Password** : `admin1` (ou un autre mot de passe)
2. Pour attribuer le rôle admin :
   - Allez dans l'onglet **Role mapping** (ou **Attribution de rôles**)
   - Cliquez sur le bouton **Assign role** (ou **Attribuer un rôle**) en haut à droite
   - Une fenêtre modale s'ouvre avec une liste de rôles
   - **IMPORTANT** : En haut de cette fenêtre, cliquez sur **Filter by realm roles** (ou **Filtrer par rôles du royaume**)
   - Vous devriez voir les rôles "joueur" et "admin" dans la liste
   - Cochez la case à côté de **admin**
   - Cliquez sur **Assign** (ou **Attribuer**) en bas de la fenêtre
   - Le rôle "admin" devrait maintenant apparaître dans la liste des rôles assignés à cet utilisateur

## Étape 8 : Vérifier la Configuration

1. Vérifiez que le realm `blazorgamequest` est bien sélectionné
2. Vérifiez que le client `blazor-client` existe et est configuré correctement
3. Vérifiez que les rôles `joueur` et `admin` existent
4. Vérifiez que vous avez créé au moins un utilisateur avec chaque rôle

## Étape 9 : Tester l'Application

1. Assurez-vous que tous les services Docker sont en cours d'exécution
2. Accédez à l'application Blazor : **http://localhost:5000**
3. Vous devriez être redirigé vers la page de connexion Keycloak
4. Connectez-vous avec un des utilisateurs créés (ex: `joueur1` / `joueur1`)
5. Après connexion, vous devriez être redirigé vers l'application

## Dépannage

### Problème : Keycloak ne démarre pas

- Vérifiez que le port 8080 n'est pas déjà utilisé
- Vérifiez les logs Docker : `docker-compose logs keycloak`

### Problème : Erreur de redirection après connexion

- Vérifiez que les **Valid redirect URIs** dans Keycloak correspondent exactement à `http://localhost:5000/authentication/login-callback`
- Vérifiez que **Web origins** contient `http://localhost:5000`

### Problème : Les rôles ne sont pas reconnus

- Vérifiez que le mapper **realm roles** est bien configuré dans le client scope **roles**
- Vérifiez que les rôles sont bien attribués aux utilisateurs dans l'onglet **Role mapping**

### Problème : Je ne trouve pas "Realm roles" dans le menu

**Solution** :
1. Vérifiez que vous êtes bien dans le realm **blazorgamequest** (pas "Master")
2. Dans le menu de gauche, cherchez **Realm roles** (c'est un élément de menu principal, pas un sous-menu)
3. Si vous ne le voyez toujours pas :
   - Allez dans **Realm settings** → onglet **Realm Roles**
   - Ou utilisez la barre de recherche en haut du menu de gauche et tapez "realm roles"

### Problème : Les rôles n'apparaissent pas dans "Filter by realm roles"

**Causes possibles** :
1. **Vous n'avez pas créé les rôles** : Retournez à l'Étape 4 et créez-les
2. **Vous êtes dans le mauvais realm** : Vérifiez que vous êtes dans **blazorgamequest** (pas "Master")
3. **Les rôles ont été créés dans un autre realm** : Vérifiez dans quel realm vous avez créé les rôles

**Solution** :
1. Allez dans **Realm roles** (menu de gauche)
2. Vérifiez que vous voyez bien "joueur" et "admin" dans la liste
3. Si la liste est vide, créez les rôles comme décrit à l'Étape 4
4. Si vous voyez les rôles mais qu'ils n'apparaissent pas lors de l'assignation :
   - Vérifiez que vous avez bien cliqué sur **Filter by realm roles** (pas "Filter by clients" ou autre)
   - Rafraîchissez la page et réessayez

### Problème : L'API retourne 401 Unauthorized

- Vérifiez que le token JWT est bien envoyé dans les requêtes HTTP
- Vérifiez que l'**Audience** dans la configuration JWT de l'API correspond au **Client ID** dans Keycloak (`blazor-client`)

## Notes Importantes

- **En production**, vous devrez :
  - Utiliser HTTPS au lieu de HTTP
  - Configurer des certificats SSL valides
  - Changer les mots de passe par défaut
  - Configurer des politiques de mot de passe plus strictes
  - Activer la vérification HTTPS dans les configurations (`RequireHttpsMetadata = true`)

- Les URLs de redirection doivent correspondre exactement à celles configurées dans Keycloak

- Les rôles doivent être mappés correctement dans les tokens JWT pour que l'autorisation fonctionne

## Structure des Rôles

- **joueur** : Accès aux pages de jeu (Game, TableauScore) et aux API Player et Donjon (lecture)
- **admin** : Accès complet, y compris les pages Admin et MapEditor, et toutes les API (y compris AdminController)

