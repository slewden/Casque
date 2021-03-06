﻿//-- Configuration & constantes ------------------------------------------------------------------------------------
angular.module('$noConst', []).constant('NOCONFIG', {
  PAGESIZE: 8,  // pagination pour l'admin

  ACTIONLECTEURSTART: 1,         // Démarrer un lecteur
  ACTIONLECTEURSTOP: 2,          // Arreter un lecteur
  ACTIONLECTEURRESETLECTURES: 3, // Initialiser la liste des lectures
  ACTIONLECTEURQUERYSTATUT: 4,   // Renvoie le statut du lecteur

  READER_STOPPED: 0,
  READER_STATING: 1,
  READER_STARTED: 2,
  READER_STOPPING: 3,

  READER_UNKNOW: -1,

});

//-- Traduction ----------------------------------------------------------------------------------------------------
var selecteLanguage = 'fr';
var translations = {
  // page edition/visu d'une caisse
  Common: {
    TitreApplication: 'Tracking Center',
    CopyRightApplication: 'Tracking center ©Acreon 2016',
    PageTitle: 'Page',
    NotFound: 'Aucun résultat avec ces filtres',
    CRUDAdd: 'Ajouter',
    CRUDEdit: 'Modifier',
    CRUDDel: 'Supprimer',
    CRUDDetail: 'Voir en détail',
    CRUDRemove: 'Retirer',
    BoutonEnregistrer: 'Enregistrer',
    BoutonSuivant: 'Enregistrer et suivant',
    BoutonAnuller: 'Annuler',
    BoutonRetour: 'Retour',
    BoutonPrint: 'Lancer l\'impression...',
    BoutonRetry: 'Réessayer',
    TipsMailTo: 'Envoyer un email',
    TipsHide: 'Masquer',
    TipsLaunchSearch: 'Lancer la recherche',
    TipsInitSearch: 'Initialiser la recherche',
    TipsSort: 'Changer de tri',
    LblTotal: 'Total',
    Aucun: '[Aucun]',
    Aucune: '[Aucune]',
    NoEmail: '[Aucun email]',
    NoAdresse: '[Aucune adresse]',
    ErrorGet: 'Chargement impossible. Erreur {{ code }} : {{ text }}',
    ErrorPost: 'Enregistrement impossible. Erreur {{ code }} : {{ text }}',
    ErrorPut: 'Enregistrement impossible. Erreur {{ code }} : {{ text }}',
    ErrorUpload: 'Envoie de l\'image impossible. Erreur {{ code }} : {{ text }}',
    ErrorDelete: 'Suppression impossible. Erreur {{ code }} : {{ text }}',
    ConfirmDelete: 'Etes-vous certain de vouloir supprimer {{ nom }} et tout ce qui le concerne ?',
    TitreTotal: 'Total :',
  },
  Validation: {
    Requis: 'Requis',
  },
  Header: {
    TipUser: 'Bonjour {{ nom }}',
    Home: 'L\'accueil',
    Quit: 'Quitter',
    Retour: 'Retour',
  },
  Login: {
    Titre: 'Idendification',
    Login: 'Login',
    Password: 'Mot de passe',
    BtValid: 'Entrer',
    ErrorLogin1: 'Erreur d\'authentification, Réessayez',
    ErrorLogin2: 'Erreur d\'authentification, Réessayez',
    ErrorLogin3: 'L\'authentification n\'est plus valide veuillez vous identifier de nouveau',
  },
  Home: {
    Bienvenu: 'Bienvenue dans Tracking Center,  {{ user }}',
  },
  CommandeResume: {
    Title: 'Répartition des commmandes',
    SousTitre: 'Les commandes',
    NotFound: 'Aucune commande pour l\'instant',
    All: 'Toutes les commandes',
    Statut1: 'Commandes en cours de saisie',
    Statut2: 'Commandes en cours d\'encodage des étiquettes',
    Statut3: 'Commandes attente d\'envoi',
    Statut4: 'Commandes envoyée au fournisseur',
    Statut5: 'Commandes reçues partiellement',
    Statut6: 'Commandes reçues partiellement, mais acquittées',
    Statut7: 'Commandes entièrement reçues',
    CRUDAdd: 'Ajouter une nouvelle commande',
    CRUDList: 'Voir la liste',
    CRUDListTotal: 'Voir toutes les commandes',
    BtAcquitte: 'Acquitter la commande',
    TipsNombreCommande: 'Nombre de commandes dans ce statut',
    TipsNombrePiece: 'Nombre de pièces dans les commandes de ce statut',
  },
  Commande: {
    SousTitre: 'Gestion des commandes',
    SearchText: 'Numéro, fournisseur, opérateur',
    TitreEdit: 'Modification d\'une commande',
    TitreAdd: 'Ajout d\'une commande',
    TitrePrint: 'Impression des étiquettes d\'une commande',
    LblNumero: 'Numéro',
    RequisNumero: 'Le numéro de commande est obligatoire',
    MaxLenghtNumero: 'Le numéro de commande ne peut dépasser les 20 caractères',
    LblFournisseur: 'Fournisseur',
    RequisFournisseur: 'Le fournisseur est obligatoire',
    LblUtilisateur: 'Opérateur',
    LblValidation: 'Validation',
    LblWaitValidation: 'Attente du début d\'impression des étiquettes...',
    LblStatut: 'Statut',
    CommandeValidee: 'La commande a été validée le {{ date | datews: \'short\'}}',
    TipsValider: 'La commande est valide : démarrer l\'édition des étiquettes',
    TipsNotValider: 'La commande n\'est pas encore définitive',
    CommandeAcquittee: 'Commande acquittée le {{ date | datews: \'short\'}}',
    LblCreation: 'Création',
    LblQuiQaund: 'Par {{ user}} le {{ date |datews: \'short\'}}',
    TipsQuantite: 'Nombre de pièces commandées',
    LblQuantite: 'Qté',
    FormatQuantite: 'La quantité doit être un nombre',
    LblTotal: 'Total ',
    TipsNombreEtiquette: 'Nombre d\'étiquettes à créer pour cette commande',
    TipsNombrePiece: 'Nombre total de produit commandé',
    TipsAddPiece: 'Ajouter la même pièce',
    RequisPieceDansCommande: 'Aucune étiquette dans la commande',
    LblImpression: 'Edition des étiquettes',
    LblImpressionFini: 'Edition des étiquettes finie le {{ date | datews: \'short\'}}',
    LblImpressionEncours: 'Impression en cours...',
    LblEmail: 'Envoie au fournisseur',
    LblEmailFini: 'Envoie au fournisseur fait le {{ date | datews: \'short\'}}',
    TipsEnvoieEmail: 'Envoyer la commande par email au fournisseur et imprimer le bon de commande',
    TipsNotEnvoieEmail: 'La commande est prête, mais n\'est pas envoyée au fournisseur',
    LivraisonAttendueLeOk: 'Livraison attendue le {{ date | datews: \'shortDate\' }} soit dans {{ jour | absolute }} j',
    LivraisonAttendueLeKo: 'Livraison attendue le {{ date | datews: \'shortDate\' }} en retard de {{ jour | absolute }} j',
    LivraisonAttendueToday: 'Livraison aujourd\'hui',
    LblReception: 'Réception',
    LblDelaiReception: 'Délai de réception',
    FormatDelaiSemaine: 'Le délai de livraison doit un être un nombre',
    TipsAcquittee: 'La commande doit être considérée comme livrée, même s\'il manque des pièces',
    TipsNotAcquittee: 'Attente des pièces commandées manquantes',
    ConfirmDelete: 'Etes-vous certain de vouloir supprimer la commande N° {{ nom }}  et tout ce qui la concerne ?',
    ConfirmAcquitte: 'Etes-vous certain de vouloir acquitter la commande N° {{ nom }} les pièces non reçues ne seront plus attendues ?',
    ErrorPrinting: 'Impossible d\'envoyer la demande d\'impression des étiquettes de la commande : {{ text }}',
    progressing: 'Action en cours...',
    progression8: 'Impression des étiquettes',
    TipsPrintBC: 'Imprimer le bon de commande',
    TipsSendAutreEmail: 'Envoyer l\'email au fournisseur de nouveau',
    EmailAutre: 'Adresse supplémentaire',
    BtSendAutreEmail: 'Envoyer',
    ConfirmMailSend: 'Le bon de commande a été envoyé au fournisseur',
    TotalNBProduitEtiquette: 'Nombre total de produits étiquettés',
  },
  Utilisateur: {
    SousTitre: 'Gestion des opérateurs',
    SearchText: 'Nom, Email',

    TitreEdit: 'Modification d\'un opérateur',
    TitreAdd: 'Ajout d\'un opérateur',
    TitreDetail: 'Détail d\'un opérateur',
    LblNom: 'Prénom et nom',
    RequisNom: 'Le prénom et nom de l\'opérateur est requis',
    MaxLenghtNom: 'Le prénom et nom ne doit pas dépasser 100 caractères',
    LblEmail: 'Email',
    MaxLenghtEmail: 'L\'email ne doit pas dépasser 100 caractères',
    LblLogin: 'Login',
    RequisLogin: 'Le login de l\'opérateur est requis',
    FormatEmail: 'Le format de l\'adresse email n\'est pas valide',
    MaxLenghtLogin: 'Le login ne doit pas dépasser 20 caractères',
    MinLenghtLogin: 'Le login doit être d\'au moins 5 caractères',
    LblActif: 'Actif',
    TipsActif: 'L\'opérateur peut se connecter au système',
    TipsNotActif: 'L\'opérateur n\'a pas accès au système',
    LblPassword: 'Mot de passe',
    RequisPassword: 'Le mot de passe de l\'opérateur est requis',
    MaxLenghtPassword: 'Le mot de passe ne doit pas dépasser 20 caractères',
    MinLenghtPassword: 'Le mot de passe doit être d\'au moins 5 caractères',
    LblConfirm: 'Confirmez le mot de passe',
    RequisConfirm: 'Le confirmation est requise',
    ErrorConfirm: 'La confirmation n\'est pas identique au mot de passe',
  },
  UtilisateurDroit: {
    SousTitre: 'Définition des autorisations d\'un opérateurs',
    Titre: 'Autorisations de {{ nom }}',
    AccessNull: 'A définir',
    Access0: 'Pas d\'accès',
    Access1: 'Lecture seule',
    Access2: 'Accès complet',
    AccessFils: 'Accès selon les choix des fils',
  },
  Carton: {
    SousTitre: 'Gestion des cartons d\'expédition',
    SearchText: 'Nom, code, description',
    LblNom: 'Nom',
    LblCode: 'Code',
    LblDescription: 'Description',
    TitreEdit: 'Modification d\'un carton',
    TitreAdd: 'Ajout d\'un carton',
    TitreDetail: 'Détail d\'un carton',
    RequisNom: 'Le nom du carton est requis',
    MaxLenghtNom: 'Le nom ne doit pas dépasser 100 caractères',
    RequisCode: 'Le code du carton est requis',
    MaxLenghtCode: 'Le code ne doit pas dépasser 50 caractères',
  },
  Couleur: {
    SousTitre: 'Gestion des couleurs de produits',
    SearchText: 'Nom, code, description',
    LblNom: 'Nom',
    LblCode: 'Code couleur',
    LblDescription: 'Description',
    TitreEdit: 'Modification d\'une couleur',
    TitreAdd: 'Ajout d\'une couleur',
    TitreDetail: 'Détail d\'une couleur',
    RequisNom: 'Le nom de la couleur est requis',
    MaxLenghtNom: 'Le nom ne doit pas dépasser 100 caractères',
    RequisCode: 'Le code de la couleur est requis',
  },
  Taille: {
    SousTitre: 'Gestion des tailles des pièces',
    SearchText: 'Nom, code, description',
    LblNom: 'Nom',
    LblCode: 'Code',
    LblDescription: 'Description',
    TitreEdit: 'Modification d\'une taille',
    TitreAdd: 'Ajout d\'une taille',
    TitreDetail: 'Détail d\'une taille',
    RequisNom: 'Le nom de la taille est requis',
    MaxLenghtNom: 'Le nom ne doit pas dépasser 100 caractères',
    RequisCode: 'Le code de la taille est requis',
    MaxLenghtCode: 'Le code ne doit pas dépasser 10 caractères',
  },
  Client: {
    SousTitre: 'Gestion des clients',
    SearchText: 'Nom, email, adresse',
    LblNom: 'Nom',
    LblAdresseCommande: 'Adresse de facturation',
    LblAdresseLivraison: 'Adresse d\'expédition',
    LblEmail: 'Email',
    TitreEdit: 'Modification d\'un client',
    TitreAdd: 'Ajout d\'un client',
    TitreDetail: 'Détail d\'un client',
    RequisNom: 'Le nom du client est requis',
    MaxLenghtNom: 'Le nom ne doit pas dépasser 200 caractères',
    MaxLenghtEmail: 'L\'adresse email ne doit pas dépasser 100 caractères',
    FormatEmail: 'Le format de l\'adresse email n\'est pas valide',
  },
  Fournisseur: {
    SousTitre: 'Gestion des fournisseurs',
    SearchText: 'Nom, email, adresse',
    LblNom: 'Nom',
    LblAdresseCommande: 'Adresse fournisseur',
    LblAdresseLivraison: 'Adresse d\'expédition',
    LblEmail: 'Email',
    LblNbPiece: 'Pièces',
    TipsNbPiece: 'Nombre de types de pièces gérées pour ce fournisseur',
    TitreEdit: 'Modification d\'un fournisseur',
    TitreAdd: 'Ajout d\'un fournisseur',
    TitreDetail: 'Détail d\'un fournisseur',
    RequisNom: 'Le nom du fournisseur est requis',
    MaxLenghtNom: 'Le nom ne doit pas dépasser 200 caractères',
    MaxLenghtEmail: 'L\'adresse email ne doit pas dépasser 100 caractères',
    FormatEmail: 'Le format de l\'adresse email n\'est pas valide',
    TipsTypePieceFournisseur: 'Déclarer les types de pièces fournies par ce fournisseur',
  },
  FournisseurPiece: {
    SousTitre: 'Gestion des pièce fournies',
    Titre: '{{ nom }} fourni :',
    LblTypePiece: 'Pièce',
    LblPrix: 'Prix',
    Lblfrais: 'Frais',
    TipsPrix: 'Prix unitaire',
    TipsFrais: 'Frais fixe à la commande',
    FormatPrix: 'Le prix unitaire doit être un nombre',
    FormatFrais: 'Les frais doivent être un nombre',
  },
  TypePiece: {
    SousTitre: 'Gestion des types de pièces',
    SearchText: 'Nom, code, description',
    LblNom: 'Nom',
    LblCode: 'Code',
    LblAvecTag: 'Etiqueté',
    LblDescription: 'Description',
    LblPhoto: 'Photo',
    LblNombre: 'Desc',
    LblTailles: 'Tailles',
    LblCouleurs: 'Couleurs',
    TitreEdit: 'Modification d\'un type de pièce',
    TitreAdd: 'Ajout d\'un type de pièce',
    TitreDetail: 'Détail d\'un type de pièce',
    RequisNom: 'Le nom du type de pièce est requis',
    MaxLenghtNom: 'Le nom ne doit pas dépasser 100 caractères',
    RequisCode: 'Le code du type de pièce est requis',
    MaxLenghtCode: 'Le code ne doit pas dépasser 20 caractères',
    TipsAvecTag: 'Une étiquette pour chaque pièce de ce type',
    TipsSansTag: 'Non',
    TipsNbCouleur: 'Nombre de couleur possibles',
    TipsNbTaille: 'Nombre de tailles possibles',
    LblNombreCommande: 'Commandes',
    TipsNbCommande: 'Nombre de pièces commandées',
    TipsNbStock: 'Nombre de pièces reçues',
    TipsNbUtilisee: 'Nombre de pièces utilisées',
    LblNombreFournisseur: 'Fournisseur',
    TipsNbFournisseur:'Nombre de fournisseurs pour cette pièce',
  },
  Casque: {
    SousTitre: 'Gestion des casques',
    SearchText: 'Nom, code, description',
    LblNom: 'Nom',
    LblCode: 'Code',
    LblDescription: 'Description',
    LblPhoto: 'Photo',
    LblNombrePiece: 'Pièces',
    TipsNombrePiece: 'Nombre de pièce constituant ce casque',
    TipsConstitue: 'Constitution du casque avec des pièces',
    TitreEdit: 'Modification d\'un casque',
    TitreAdd: 'Ajout d\'un casque',
    TitreDetail: 'Détail d\'un casque',
    RequisNom: 'Le nom du casque est requis',
    MaxLenghtNom: 'Le nom ne doit pas dépasser 100 caractères',
    RequisCode: 'Le code du casque est requis',
    MaxLenghtCode: 'Le code ne doit pas dépasser 20 caractères',
    TipsNombreStock: 'Nombre de casque en stock',
    TipsNombreLivre: 'Nombre de modèles livrés',
    TipsNombreTaille: 'Nombre de pièces qui ont une taille précise pour ce casque',
    TipsNombreCouleur: 'Nombre de pièces qui ont une couelur précise pour ce casque',
    lblNombreAssemblage: 'Stocks',
  },
  CasqueConstitue: {
    SousTitre: 'Constitution d\'un casque',
    Titre: 'Se constitue de :',
    LblTypePiece: 'Pièce',
    LblCouleur: 'Couleur',
    LblTaille: 'Taille',
    LblTypePieceAdd: 'Pièces disponibles',
    TipsPieceEtiquette: 'Cette pièce dispose d\'une étiquette électronique',
    CannotEditDesc: 'Constitution non modifiable car des assemblages sont déjà enregistrés',
  },
  Poste: {
    SousTitre: 'Gestion des points de lectures',
    SearchText: 'Nom, code, description',
    LblPosteType: 'Type de point',
    LblNom: 'Nom',
    LblAffectation: 'Affectation',
    LblDescription: 'Description',
    LblConfigurationTxt: 'Configuration',
    TitreEdit: 'Modification d\'un point de lectures',
    TitreAdd: 'Ajout d\'un point de lectures',
    TitreRead: 'Détail d\'un point de lecture',
    RequisPosteType: 'Le type de poste est requis',
    RequisNom: 'Le nom du type de poste est requis',
    MaxLenghtNom: 'Le nom ne doit pas dépasser 100 caractères',
    RequisAffectation: 'L\'affectation est requise',
    RequisAdresseIp: 'Le code du poste est requis',
    RequisPosteTypePourAffectation: 'Sélectionnez un type de point',
    LblAdressIp: 'Adresse IP',
    MaxLenghtAdresseIp: 'Le code ne doit pas dépasser 15 caractères',
    FormatAdresseIp: 'Le format de l\'adresse Ip n\'est pas valide',
    LblAntennes: 'Antennes',
    Antenne1: 'N°1',
    Antenne2: 'N°2',
    Antenne3: 'N°3',
    Antenne4: 'N°4',
    TitreGain: 'Gain',
    UnitGain: 'db',
    LblSeuil: 'Seuil lecture',
    TipsSeuil: 'Nombre de lectures effectuées par le lecteur avant de diffuser le numéro de l\'étiquette',
  },
  Reception: {
    SousTitre: 'Réception de commandes',
    lblPoste: 'Sélectionnez un lecteur',
    BtStart: 'Démarrer la lecture',
    BtStop: 'Terminer',
    BtCancel: 'Annuler',
    BtReset: 'Recommencer',
    ConfirmCancel: 'Etes-vous certain de vouloir annuler la réception en cours ? Toutes les données pré enregistrées seront perdues',
    ReceptionEnCours: 'Lecture des pièces en cours...',

    BtMasquerCommandeFinie: 'Masque cette commande',
    BtMasquerInconnus: 'Masquer',
    BtValideCommandePartielle: 'Valider la réception partielle de cette commande',
    BtValideCommandeTotale: 'Valider la réception complète de cette commande',
    CommandeSaved: 'Réception commande enregistrée',

    Commande: 'Commande :',
    CommandeComplete: ': Déjà reçue et complète',
    Fournisseur: 'Fournisseur',
    Recu: 'Reçu',
    Sur: 'sur',
    TotalCommande: 'Total : {{ total }} pièces dans la commande.',
    TotalDont: 'Dont {{ nombre }} déjà recuès.',
    TotalLu: 'Nombre d\'étiquettes lues :',
    BadTagList: 'Etiquette non reconnues :',
  },
  HubLecteur: {
    HubClosed: 'Non connecté au lecteur',
    HubOpen: 'Connecté au lecteur',
    StatutUnknow: 'Inconnu (la connexion au lecteur n\'est pas établie)',
    Statut0: 'Stoppé',
    Statut1: 'Démarrage en cours',
    Statut2: 'En route',
    Statut3: 'Arrêt en cours',

    Statut5: 'Annulation...',
    Statut555: 'Finalisation...',

    StatutConnecte: 'Connecté au lecteur. Identifiant : ',
    StatutDisconnected: 'Non connecté au lecteur',
    StatutConnectionFail: 'Impossible d\'établir la connexion; Réessayez.',
    LecteurStartFail: 'Impossible de démarrer le lecteur : {{ msg }}',

    ConfirmReset: 'Etes vous certain de vouloir recommencer le lot de lecture ? Toutes les lectures déjà effectuées seront perdues',
    LecteurQuering: 'Analyse du statut du lecteur...',
    LecteurStarting: 'Lecteur en cours de démarrage...',
    LecteurStarted: 'Lecteur en cours d\'acquisitions...',
    LecteurStopping: 'Lecteur en cours d\'arrêt...',
    LecteurStopped: 'Lecteur stoppé.',
    LecteurStopFail: 'Impossible de stopper le lecteur : {{ msg }}',
    LecteurCanceled: 'Lecteur stoppé',
    LecteurSaved: 'Opération terminé avec succès',

    HubError: 'Erreur : {{ msg }}',
    HubNotify: 'Message : {{ msg }}',
    LecturesReseting: 'Initialisation des lectures...',
    LecturesReseted: 'Lectures ré-initialisées',
    ResetFail: 'Impossible d\'effectuer une ré-initialisation des lectures {{ msg }}',
    RestartConnection: 'Se connecter au lecteur',
    HubNoDriverForReader: 'Le pilote des lecteurs n\'est pas en route.',
    HubNoDriverForWriter: 'Le pilote des imprimantes n\'est pas en route.',
    HubErrorReaderStatuUnknow: 'Impossible de savoir si les lecteur sont opérationnels',
    ErrorHubClosed: 'Impossible de se connecter au lecteur le hub est fermé',
    ErrorWhenCanceling: 'Annulation impossible. Erreur {{ code }} : {{ text }}',
    ErrorWhenRemoveCommande: 'Impossible de retrouver la commande pour la retirer de la liste',
    ConfirmCancelSession: 'Etes vous certain de vouloir interrompre cette session ?',
    ErrorCancelSession: 'Arrêt de la session impossible. Erreur {{ code }} : {{ text }}',
    ErrorWriter: 'Erreur lors de la recherche du driver d\'imprimante : {{ text }}',

    progression1: 'Connexion au hub...',
    progressionOk1: 'Connexion au hub Ok',
    progression2: 'Recherche des lecteurs...',
    progression2p: 'Recherche des imprimantes...',
    progression3: 'Driver des lecteurs non démarré.',
    progression4: 'Driver des imprimantes non démarré.',
    progression42: 'Veuillez corriger et ',
    progression5: 'Drivers Ok.',
    progressNoReader: 'Aucun lecteur disponible.',
    progressNoWriter: 'Aucune imprimante disponible.',
    progressSession: 'Vérifier les sessions en cours...',
    ErrorNoSession: 'Aucun lecteur en cours d\'utilisation. Vérifiez le paramétrage des postes !',
    ErrorNoSessionI: 'Aucune imprimande en cours d\'utilisation. Vérifiez le paramétrage des postes !',
    SessionList: 'Sessions en cours',
    SessionSince: 'depuis le {{ date | datews: \'short\'}}',
    TipsCloseSession: 'Terminer cette session',
    progression6: 'Recherche de l\'état du lecteur...',
    progression8: 'Lecteur {{ poste }} OK.',
    progression8p: 'Imprimante {{ poste }} OK.',
    progression11: 'Impression en cours...',
    progression13: 'Assemblage terminé : Casque créé',
    LblSelWriter: 'Sélectionnez une imprimante',
    BtStartPrinting: 'Démarrer l\'impression',
    progressionFinie: 'Finie',
  },
  Assemblage: {
    SousTitre: 'Assemblage de pièces',
    ConfirmCancel: 'Etes-vous certain de vouloir annuler cet assemblage en cours ? Toutes les données pré enregistrées seront perdues',
    CommandeSaved: 'Assemblage enregistré, impression de l\'étiquette casque',
    ErrorCmdPrint: 'Impossible d\'envoyer la demande d\'impression de l\'étiquette : {{ text }}',
    ConfirmCancelAssemblage: 'Etes vous certain de vouloir annuler l\'assemblage en cours ?',
    ErrorCancelAssemblage: 'Annulation de l\'assemblage impossible. Erreur {{ code }} : {{ text }}',
    Saved: 'Assemblage enregistré',
    ChoosePrinter: 'Sélectionner une imprimante d\'étiquette',
    PrintTag: 'Imprimer l\'étiquette',
    Cancel: 'Annuler l\'assemblage',
    Next: 'Assemblage suivant',
    AssPossible: 'Possible :',
    AssComplet: 'Complet :',
    AssEtiquette: 'Etiquettes lues :',
    AnalyseEncore: 'Analyser de nouveau ces étiquettes',
    AllTagRead: 'Toutes les étiquettes du casque ont été lues',
    BoutonValide: 'Valider l\'assemblage',
    TipTagRead: 'L\'étiquette a été lue',
    TipsPieceCT: 'Couleur : {{ couleur }}, Taille : {{ taille}}',
    TipsPieceC: 'Couleur : {{ couleur }}',
    TipsPieceT: 'Taille : {{ taille}}',
    BadTagList: 'Etiquettes non valides pour l\'assemblage :',
  },
  Livraison: {
    SousTitre: 'Livraison aux clients',
    ConfirmCancel: 'Etes-vous certain de vouloir annuler cette livraison ? Toutes les données pré enregistrées seront perdues',
    CommandeSaved: 'Livraison enregistrée',
    ErrorSaveCarton: 'Sauvegarde du carton livré impossible. Erreur {{ code }} : {{ text }}',
    ConfirmCancelLivraison: 'Etes vous certain de vouloir annuler la livraison en cours ? Tous les cartons constitués seront perdus',
    ErrorCancelLivraison: 'Annulation de la livraison impossible. Erreur {{ code }} : {{ text }}',
    ErrorSaveLivraison: 'Sauvegarde de la livraison impossible. Erreur {{ code }} : {{ text }}',
    CartonIndex: 'Carton N° {{ numero }}',
    WaitingRead: 'Attente des lectures...',
    SaveAndContinue: 'Enregistrer et suivant',
    SaveAndClose: 'Enregistrer et terminer',
    WaitingList: 'Etiquettes lues en attente d\'analyse :',
    BadTagList: 'Etiquette non valide pour une livraison :',
    Titre: 'Livraison',
    Reference: 'Référence : {{ reference }} le {{ creation | datews : \'short\' }}',
    NombreCarton: 'Nombre de cartons :',
    NombreCasque: 'Nombre de casques :',
    ChooseClient: 'Indiquer le client destinataire de la livraison',
    BtTerminer: 'Terminer la livraison',
    PrintTheBl: 'Imprimer le BL',
    ConfirmMailSend: 'Le bon de livraison a été envoyé au client',
  },
  AssemblageResume: {
    SousTitre: 'Les assemblages',
    LblCreation: 'Création',
    LblCreationBy: 'par',
    LblValidation: 'Validation',
    LblStatut: 'Statut',
    LblTag: 'Tag',
    ColPiece: 'Pièce',
    ColCouleur: 'Couleur',
    ColTaille: 'Taille',
    ColEtiquette: 'Etiquette',
    ColCreation: 'Création',
    ColEntreeStock: 'Entrée en stock',
    ColAssemblage: 'Assemblage',
    TipsShowcommande: 'Voir la commande de cette pièce',
  },
  AssemblageList: {
    SousTitre: 'Liste des assemblages',
    Titre: 'Inventaire des assemblages',
    AssemblageListAll: 'Tous les assemblages',
    AssemblageListStatut1: 'Les assemblages en cours de constitution',
    AssemblageListStatut2: 'Les assemblages en stock',
    AssemblageListStatut3: 'Les assemblages livrés',
    CasqueListAll: '[Tous les casques]',
    CRUDList: 'Voir ce type d\'assemblage',
    SearchText: 'Casque, tag, opérateur',
    LblCasqueNom: 'Casque',
    LblUtilisateurAssemble: 'Opérateur',
    LblCreation: 'Création',
    LblStatut: 'Etat',
    LblNombre: 'Nombre',
    NotFound: 'Aucun assemblage pour l\'instant',
  },
  AssemblageDetail: {
    Titre: 'Détail d\'un assemblage',
  },
  LivraisonResume: {
    SousTitre: 'Les livraisons',
    Titre0: 'Top 10 des livraisons clients',
    Titre1: 'Les 10 dernières livraisons',
    LivraisonResume: 'Aucune livraison pour l\'instant',
    NotFound: 'Aucune livraison',
    TipsNombre: 'Nombre de livraison pour ce client',
    TipsNombrePiece: 'Nombre de casques livrées à ce client',
    AucunClient: 'Livraison(s) Incomplète(s)',
  },
  LivraisonList: {
    SousTitre: 'Liste des livraisons',
    LblClient: 'Client',
    LblUtilisateur: 'Opérateur',
    LblValidation: 'Validation',
    LblNombreCarton: 'Cartons',
    LblNombrePiece: 'Casques',
    All: 'Toutes les livraisons',
    Statut1: 'Livraisons incomplètes (sans client)',
    Statut2: 'Livraisons complètes',
  },
  LivraisonDetail: {
    SousTitre: 'Gestion des livraisons',
    Titre: 'Détail d\'une livraison',
    LblClient: 'Client',
    LblReference: 'Référence',
    LblCreation: 'Création',
    LblCreationBy: 'par',
    LblValidation: 'Validation',
    LblTotal: 'Nombre de casques',
    ColCarton: 'Cartons',
    ColCasque: 'Casques',
    NoValidation: 'Sélectionnez un client pour finaliser la livraison',
    TipsAdresse: 'Addresse postale du client',
    PrintBL: 'Imprimer le bon de livraison',
    MailBL: 'Envoyer le BL par email au client',
    MailSend: 'Le Bl a été envoyé par email au client',
  },
  LivraisonPrint: {
    Titre: 'Bon de livraison',
    lblReference: 'Référence : {{ numero }}',
    LblCreation: 'Toulon le {{ date | datews : \'short\' }}',
    LblClient: 'Destinataire',
    LblAdresse: 'Adresse',
    LblTotal: 'Nombre de pièces livrées au total : ',
    ColQuantite: 'Qte',
    ColEtiquette: 'Etiquettes',
    EtiquetteTitre: 'Ref {{ index }}',
  },
  CommandePrint: {
    Titre: 'Bon de commande',
    lblReference: 'Référence : {{ numero }}',
    LblCreation: 'Toulon le {{ date | datews : \'short\' }}',
    LblClient: 'Destinataire',
    LblAdresse: 'Adresse',
    LblTotal: 'Nombre de pièces commandées au total : ',
    ColQuantite: 'Qte',
    ColEtiquette: 'Etiquettes',
    EtiquetteTitre: 'Ref {{ index }}',
  },
  MailConfig: {
    SousTitre: 'Configuration des emails',
    TitreEdit: 'Configuration des emails',
    LblHost: 'Adresse du serveur',
    RequisHost: 'L\'adresse du serveur d\'email  est requise',
    MaxLenghtHost: 'L\'adresse du serveur d\'email ne doit pas dépasser 200 caractères',
    LblPort: 'Port du serveur',
    FormatPort: 'Le port doit être un nombre',
    LblUser: 'Utilisateur',
    MaxLenghtUser: 'L\'utilisateur ne doit pas dépasser 200 caractères',
    LblPassword: 'Mot de passe',
    MaxLenghtPassword: 'Le mot de passe ne doit pas dépasser 200 caractères',
    LblFromEmail: 'Email de l\'expéditeur',
    RequisFromEmail:'L\'email de l\'expéditeur est requis',
    MaxLenghtFromEmail: 'L\'email de l\'expéditeur ne doit pas dépasser 100 caractères',
    FormatFromEmail: 'Le format de l\'adresse email de l\'expéditeur n\'est pas valide',
    LblBCCEmails: 'Emails en copies cachés',
    FormatBCCEmails: 'L\'emails en copie caché n\'est pas valide (séparez les destinataires par des virgules ","',
    LblSubjetFournisseur: 'Sujet des emails de commande fournisseur',
    RequisSubjetFournisseur: 'Le sujet des emails de commandes fournisseurs est requis',
    LblSubjetClient: 'Sujet des emails de livraison client',
    RequisSubjetClient: 'Le sujet des emails de livraisons clients est requis',
    HelpSubjectFournisseur: 'Liste des marqueurs possibles pour le sujet des emails de commandes fournisseurs',
    HelpSubjectClient: 'Liste des marqueurs possibles pour le sujet des emails de livraisons clients',
    HelpBCCEMails: 'Liste d\'adresses email séparées par des virgules'
  },
};


