using System;

namespace CasqueLib.Matos.Lecteur
{
  /// <summary>
  /// CLasse pour les paramètres de log 
  /// utilisé pour la console des lectures d'un lecteur
  /// </summary>
  public class SimpleReaderConsoleArgs : EventArgs
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="SimpleReaderConsoleArgs"/>
    /// </summary>
    /// <param name="dt">Date de la lecture</param>
    /// <param name="position">Position de l'antenne qui lit (ATTENTION : c'est pas l'index c'est la position)</param>
    /// <param name="num">numéro de tag lu</param>
    public SimpleReaderConsoleArgs(DateTime dt, int position, string num)
    {
      this.Date = dt;
      this.Position = position;
      this.Numero = num;
    }

    /// <summary>
    /// Numéro de tag lu
    /// </summary>
    public string Numero { get; private set; }

    /// <summary>
    /// Position de l'antenne qui lit (ATTENTION : c'est pas l'index c'est la position)
    /// </summary>
    public int Position { get; private set; }

    /// <summary>
    /// Date de la lecture
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// Pour affichage
    /// </summary>
    /// <returns>Le texte à afficher</returns>
    public override string ToString()
    {
      return string.Format("{0:g} : {1} : {2}", this.Date, this.Position, this.Numero);
    }
  }
}
