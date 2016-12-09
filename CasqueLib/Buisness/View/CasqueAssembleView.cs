using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// vue du compteur d'asemblages
  /// </summary>
  [Alias("v_casque_assemble")]
  public class CasqueAssembleView : Casque
  {
    /// <summary>
    /// Le nombre 
    /// </summary>
    [Alias("nombre")]
    public int Nombre { get; set; }

    /// <summary>
    /// Statut de l'assemblage
    /// 1 = En cours de construction, 2 = En stock, 3 = livré
    /// </summary>
    [Alias("statut")]
    public int StatutInt { get; set; }

    /// <summary>
    /// Nom du statut
    /// </summary>
    [Ignore]
    public string StatutNom
    {
      get
      {
        switch (this.StatutInt)
        {
          case 1:
            return "En cours";
          case 2:
            return "En Stock";
          case 3:
            return "Livré";
        }

        return string.Format("??{0} ??", this.StatutInt);
      }
    }
  }
}
