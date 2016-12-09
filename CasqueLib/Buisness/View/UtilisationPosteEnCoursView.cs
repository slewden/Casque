using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// Vue des utilisations poste en cours
  /// </summary>
  [Alias("v_utilisation_poste_en_cours")]
  public class UtilisationPosteEnCoursView : UtilisationPoste
  {
    /// <summary>
    /// Prénom et nom de l'utilisateur qui manipule
    /// </summary>
    [Alias("util_prenom_nom")]
    public string UtilisateurNom { get; set; }
    
    /// <summary>
    /// Email de l'utilisateur qui manipule
    /// </summary>
    [Alias("util_email")]
    public string UtilisateurEmail { get; set; }
    
    /// <summary>
    /// Nom du poste utilisé
    /// </summary>
    [Alias("post_nom")]
    public string PosteNom { get; set; }

    /// <summary>
    /// Page Code utilisé
    /// </summary>
    [Alias("page_code")]
    public string PageCode { get; set; }
  }
}
