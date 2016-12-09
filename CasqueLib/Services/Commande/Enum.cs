namespace CasqueLib.Services.Commande
{
  /// <summary>
  /// Les statuts de commande
  /// </summary>
  public enum EStatutCommande
  {
    /// <summary>
    /// La commande n'est pas encore saisie totalement (pas validée)
    /// </summary>
    EncoursDeSaisie = 1,

    /// <summary>
    /// Commande validée, Les tags sont en cours d'encodage 
    /// </summary>
    TagEncode = 2,

    /// <summary>
    /// La comande est fini le email est parti au fournisseur
    /// </summary>
    EmailSendable = 3,

    /// <summary>
    /// La commande est encodée, mais rien n'est reçu
    /// </summary>
    SaisiePasRecue = 4,

    /// <summary>
    /// Une Partie de la commande est reçue
    /// </summary>
    RecuePartiellement = 5,

    /// <summary>
    /// Une Partie de la commande est reçue, et elle a été acquittée
    /// </summary>
    RecuePartiellementAcquittee = 6,

    /// <summary>
    /// L'ensemble des pièces de la commande sont reçues
    /// </summary>
    RecueTotalement = 7,
  }

  /// <summary>
  /// Helper pour les enums de commandes
  /// </summary>
  public static class EnumHelperCommande
  {
    /// <summary>
    /// Renvoie le libelle du statut de commande
    /// </summary>
    /// <param name="statut">Le statut</param>
    /// <returns>Le libellé</returns>
    public static string GetLibelle(this EStatutCommande statut)
    {
      switch (statut)
      {
        case EStatutCommande.EncoursDeSaisie:
          return "En cours de saisie";
        case EStatutCommande.TagEncode:
          return "Génération des étiquettes...";
        case EStatutCommande.EmailSendable:
          return "Prête pour envoi au fournisseur";
        case EStatutCommande.SaisiePasRecue:
          return "Envoyée, attente reception";
        case EStatutCommande.RecuePartiellement:
          return "Réception partielle des pièces";
        case EStatutCommande.RecuePartiellementAcquittee:
          return "Réception partielle des pièces, mais acquitée";
        case EStatutCommande.RecueTotalement:
          return "Réception complète";
      }

      return string.Empty;
    }
  }
}
