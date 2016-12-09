namespace CasqueLib.Matos.Lecteur
{
  /// <summary>
  /// Type de resultat d'une action
  /// </summary>
  public enum ELogType
  {
    /// <summary>
    /// Information de base (en cours d'action)
    /// </summary>
    Normal,

    /// <summary>
    /// Début d'une action 
    /// </summary>
    Action,

    /// <summary>
    /// Fin d'une action OK
    /// </summary>
    RapportOk,

    /// <summary>
    /// Erreur rencontrée
    /// </summary>
    Erreur
  }

  /// <summary>
  /// La liste des actions qu'envoie un lecteur
  /// </summary>
  public enum EEtatLecteur
  {
    /// <summary>
    /// Le lecteur est en train de s'arreter
    /// </summary>
    Stop,

    /// <summary>
    /// Le lecteur est en train de demarrer
    /// </summary>
    Start,

    /// <summary>
    /// Initialise les lectures
    /// </summary>
    Reset,

    /// <summary>
    /// Un tag a été lu
    /// </summary>
    Tag,

    /// <summary>
    /// Un message KeepAlive a été reçu du lecteur
    /// Utilisé pour savoir si le lecteur est toujours 
    /// vivant quand y a pas de lectures en cours depuis longtemps
    /// </summary>
    KeepAlive,
   }

  /// <summary>
  /// Permet de connaitre le résultat de la comparaison entre 2 TagLu
  /// </summary>
  public enum ETagLuComparaison
  {
    /// <summary>
    /// Ils n'ont rien à voir entre eux : un est nul l'autre pas, ou pas même lecteur, ou pas le même numéro de tag
    /// </summary>
    Differents,

    /// <summary>
    /// Même tag, même lecteur : temps entre les 2 lectures inférieur au délai antiRebond
    /// </summary>
    DansLeDelaiAntiRebond,

    /// <summary>
    /// Même tag, même lecteur : temps entre les 2 lectures supérieur au délai antiRebond
    /// </summary>
    HorsDelaiAntiRebond,
  }
}
