using System;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness
{
  /// <summary>
  /// Classe qui mappe une serie de lectures par un user sur un poste
  /// </summary>
  [Alias("lecture")]
  public class Lecture
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="Lecture"/>
    /// </summary>
    public Lecture()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="Lecture"/>
    /// </summary>
    /// <param name="tag">le Numero de tag</param>
    public Lecture(string tag)
    {
      this.Tag = tag;
      this.Nombre = 1;
      this.Debut = DateTime.Now;
      this.Fin = DateTime.Now;
    }

    /// <summary>
    /// Clé de la lecture
    /// </summary>
    [AutoIncrement]
    [Alias("lect_id")]
    public int Cle { get; set; }

    /// <summary>
    /// Clé de l'utilisateur
    /// </summary>
    [Alias("util_id")]
    public int UtilisateurCle { get; set; }

    /// <summary>
    /// Clé de l'utilisation poste
    /// </summary>
    [Alias("utpo_id")]
    public int UtilisationPosteCle { get; set; }

    /// <summary>
    /// Numéro de tag lu
    /// </summary>
    [Alias("lect_tag")]
    public string Tag { get; set; }

    /// <summary>
    /// Première Date de la lecture
    /// </summary>
    [Alias("lect_d_debut")]
    public DateTime Debut { get; set; }

    /// <summary>
    /// Nombre de lecture
    /// </summary>
    [Alias("lect_nombre")]
    public int Nombre { get; set; }

    /// <summary>
    /// Dernière Date de la lecture
    /// </summary>
    [Alias("lect_d_fin")]
    public DateTime Fin { get; set; }
  }
}
