namespace CasqueLib.Common
{
  /// <summary>
  /// Classe pour avoir un libellé et un code
  /// </summary>
  public class DetailPageCode
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="DetailPageCode"/>
    /// </summary>
    /// <param name="code">le page code</param>
    public DetailPageCode(string code)
    {
      this.Code = code;
      this.Nom = DetailPageCode.GetNomAffectation(this.Code);
      this.Imprimante = DetailPageCode.GetImprimante(this.Code);
    }

    /// <summary>
    /// Le nom
    /// </summary>
    public string Nom { get; set; }

    /// <summary>
    /// La clé : string en code
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Indique si c'est pour une impression (true) ou lecture (false)
    /// </summary>
    public bool Imprimante { get; set; }

    /// <summary>
    /// Renvoie le nom des affectations possibles
    /// </summary>
    /// <param name="pc">le page code</param>
    /// <returns>le nom</returns>
    private static string GetNomAffectation(string pc)
    {
      if (!string.IsNullOrWhiteSpace(pc))
      {
        switch (pc.ToLower())
        {
          case "commande":
            return "Etiquettes de commande";
          case "reception":
            return "Réception";
          case "assemblage":
            return "Assemblage";
          case "assemblageprint":
            return "Etiquette d'assemblage";
          case "livraison":
            return "Livraison";
          case "consultation":
            return "Consultation";
        }
      }

      return string.Empty;
    }

    /// <summary>
    /// Renvoie le bool imprimante d'un affectation
    /// </summary>
    /// <param name="pc">le page code</param>
    /// <returns>le nom</returns>
    private static bool GetImprimante(string pc)
    {
      if (!string.IsNullOrWhiteSpace(pc))
      {
        switch (pc.ToLower())
        {
          case "commande":
          case "assemblageprint":
            return true;
          case "reception":
          case "assemblage":
          case "livraison":
          case "consultation":
            return false;
        }
      }

      return false;
    }
  }
}
