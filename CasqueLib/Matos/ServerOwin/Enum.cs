namespace CasqueLib.Matos.ServerOwin
{
  /// <summary>
  /// La liste des actions possible sur le demon de piloage des lecteurs
  /// </summary>
  public enum EActionLecteur
  {
    /// <summary>
    /// Aucune action ou action inconnue
    /// </summary>
    Acune = 0,

    /// <summary>
    /// Démarre un lecteur
    /// </summary>
    Demarre = 1,

    /// <summary>
    /// Stoppe un lecteur
    /// </summary>
    Stoppe = 2,

    /// <summary>
    /// Efface les infos de lecture de la session en cours
    /// </summary>
    ResetLecture = 3,

    /// <summary>
    /// Dans quel état est le lecteur ?
    /// </summary>
    QueryStatut = 4,
  }

  /// <summary>
  /// La liste des actions possible sur le demon de piloage des encodeurs
  /// </summary>
  public enum EActionEncode
  { 
    /// <summary>
    /// Aucune action ou action inconnue
    /// </summary>
    Aucune = 0,

    /// <summary>
    /// Traite une commande (X tag pour x pièces à encoreder et relire + print etiquettes
    /// </summary>
    TraiteCommande = 1,

    /// <summary>
    /// Traite un assemblage (1 tag à encoder et relire)+ print etiquettes
    /// </summary>
    TraiteAssemblage = 2,

    /// <summary>
    /// Annule l'encodage en cours
    /// </summary>
    CancelEncodage = 3,

    /// <summary>
    /// Demande à l'encodeur ce qu'il fait ?
    /// </summary>
    QueryStatut = 4,
  }
}
