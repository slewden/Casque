using CasqueLib.Buisness;

namespace CasqueLib.Matos.ServerOwin
{
  /// <summary>
  /// Classe qui maintient les données d'un llecteur dans le hub
  /// </summary>
  public class HubLecteurInfo
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="HubLecteurInfo"/>
    /// </summary>
    /// <param name="typeMateriel">Le type de matériel</param>
    /// <param name="cnn">l'id de connexion</param>
    public HubLecteurInfo(Poste.EPosteType typeMateriel, string cnn)
    {
      this.Type = typeMateriel;
      this.ConnexionId = cnn;
    }

    /// <summary>
    /// Le type de matériel
    /// </summary>
    public Poste.EPosteType Type { get; set; }

    /// <summary>
    /// L'id du lecteur dans le hub
    /// </summary>
    public string ConnexionId { get; set; }

    /// <summary>
    /// Le lecteur est on ou pas
    /// </summary>
    public bool IsOn { get; set; }

    /// <summary>
    /// pour les comparaisons
    /// </summary>
    /// <param name="obj">L'objet comparé</param>
    /// <returns>true si c'est le même</returns>
    public override bool Equals(object obj)
    {
      HubLecteurInfo l = obj as HubLecteurInfo;
      if ((object)l != null)
      {
        return this.Type == l.Type && this.ConnexionId == l.ConnexionId;
      }

      return false;
    }

    /// <summary>
    /// retourne le code de hash
    /// </summary>
    /// <returns>le code de hash</returns>
    public override int GetHashCode()
    {
      return (this.Type.ToString() + this.ConnexionId).GetHashCode();
    }

    /// <summary>
    /// pour Affichage
    /// </summary>
    /// <returns>le texte à afficher</returns>
    public override string ToString()
    {
      return this.ConnexionId;
    }
  }
}
