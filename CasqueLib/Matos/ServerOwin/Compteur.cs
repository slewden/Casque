using System;
using Newtonsoft.Json;

namespace CasqueLib.Matos.ServerOwin
{
  /// <summary>
  /// Pour compter et transférer les lectures
  /// </summary>
  public class Compteur
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="Compteur"/>
    /// </summary>
    /// <param name="tag">Le N° de tag à compter</param>
    public Compteur(string tag)
    {
      this.Tag = tag;
      this.Debut = DateTime.Now;
      this.Fin = DateTime.Now;
      this.Nombre = 1;
      this.Notifie = false;
    }

    /// <summary>
    /// Le tag lu
    /// </summary>
    [JsonProperty(PropertyName = "tag")]
    public string Tag { get; private set; }
    
    /// <summary>
    /// As t on notifié la lecture ou pas ?
    /// </summary>
    [JsonProperty(PropertyName = "notifie")]
    public bool Notifie { get; set; }

    /// <summary>
    /// Le nombre de lecture
    /// </summary>
    [JsonProperty(PropertyName = "nombre")]
    public int Nombre { get; private set; }

    /// <summary>
    /// La date de première lecture
    /// </summary>
    [JsonProperty(PropertyName = "debut")]
    public DateTime Debut { get; private set; }

    /// <summary>
    /// La date de dernière lecture
    /// </summary>
    [JsonProperty(PropertyName = "fin")]
    public DateTime Fin { get; private set; }

    /// <summary>
    /// incrémentation du compteur
    /// </summary>
    /// <param name="cpt">Le compteur à incrémenter</param>
    /// <returns>le compteur incrémenté</returns>
    public static Compteur operator ++(Compteur cpt)
    {
      cpt.Nombre++;
      cpt.Fin = DateTime.Now;
      return cpt;
    }

    /// <summary>
    /// Pour affichage 
    /// </summary>
    /// <returns>le texte à afficher</returns>
    public override string ToString()
    {
      return string.Format("{0} x {1} en {2:N1}s", this.Tag, this.Nombre, this.Fin.Subtract(this.Debut).TotalSeconds);
    }

    /// <summary>
    /// Code de hash pour les comparaisons
    /// </summary>
    /// <returns>la code</returns>
    public override int GetHashCode()
    {
      return this.Tag.GetHashCode();
    }

    /// <summary>
    /// Pour les comparaisons
    /// </summary>
    /// <param name="obj">l'objet à comparer</param>
    /// <returns>true si ok</returns>
    public override bool Equals(object obj)
    {
      Compteur cpt = obj as Compteur;
      if ((object)cpt == null)
      {
        return false;
      }
      else
      {
        return this.Tag.Equals(cpt.Tag);
      }
    }
  }
}
