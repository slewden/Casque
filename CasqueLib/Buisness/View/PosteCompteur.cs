using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// Ajoute à un poste le nombre de fois ou il est référencé
  /// </summary>
  public class PosteCompteur : Poste
  {
    /// <summary>
    /// Le nombre de fois ou un poste est référencé (pour l'activation du bouton delete)
    /// </summary>
    [Alias("nombre")]
    public int Nombre { get; set; }

    /// <summary>
    /// Compleète les infos
    /// </summary>
    public void Complete()
    {
      var c = new Matos.Lecteur.SimpleReaderParameters(this.ConfigurationTxt);
      this.AdresseIp = c.AdresseIP;
    }
  }
}
