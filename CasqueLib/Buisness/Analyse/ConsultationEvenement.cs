using System;

namespace CasqueLib.Buisness.Analyse
{
  /// <summary>
  /// Un évènement d'historique d'un tag
  /// </summary>
  public class ConsultationEvenement
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="ConsultationEvenement"/>
    /// </summary>
    /// <param name="typ">Le type</param>
    /// <param name="dt">La date</param>
    /// <param name="desc">La descriptions</param>
    public ConsultationEvenement(int typ, DateTime dt, string desc)
    {
      this.TypeEvenement = typ;
      this.Date = dt;
      this.Description = desc;
    }

    /// <summary>
    /// Le type
    /// </summary>
    public int TypeEvenement { get; set; }

    /// <summary>
    /// La date
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// La description
    /// </summary>
    public string Description { get; set; }
  }
}
