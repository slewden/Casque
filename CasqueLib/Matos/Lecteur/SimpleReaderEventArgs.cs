using System;
using System.Globalization;

namespace CasqueLib.Matos.Lecteur
{
  /// <summary>
  /// Renvoie les infos quand le lecteur a besoin de parler à l'application
  /// </summary>
  public class SimpleReaderEventArgs : EventArgs
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="SimpleReaderEventArgs" />.
    /// </summary>
    /// <param name="txt">Message à passer</param>
    /// <param name="statut">Resultat Ok ou ko</param>
    /// <param name="action">Action associée</param>
    /// <param name="readerPosition">Position du lecteur</param>
    public SimpleReaderEventArgs(string txt, ELogType statut, EEtatLecteur action, int readerPosition = SimpleReaderAntenneInfo.ALLPOSITION)
    {
      this.Date = DateTime.Now;
      this.Contenu = txt;
      this.ActionResult = statut;
      this.Action = action;
      this.Position = readerPosition;
    }

    #region Properties
    /// <summary>
    /// Action associée à la remontée d'infos
    /// </summary>
    public EEtatLecteur Action { get; private set; }

    /// <summary>
    /// Date du dernier message
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// Texte du dernier message
    /// </summary>
    public string Contenu { get; private set; }

    /// <summary>
    /// Dernier message est une erreur
    /// </summary>
    public ELogType ActionResult { get; private set; }

    /// <summary>
    /// L'adresse IP du lecteur conserné
    /// </summary>
    public string AdresseIP { get; set; }

    /// <summary>
    /// Position du lecteur qui donne l'info
    /// </summary>
    public int Position { get; set; }
    #endregion

    /// <summary>
    /// Méthode d'Affichage
    /// </summary>
    /// <returns>le texte de l'objet</returns>
    public override string ToString()
    {
      string position = string.Empty;
      if (this.Position >= SimpleReaderAntenneInfo.FIRSTPOSITION && this.Position <= SimpleReaderAntenneInfo.LASTPOSITION)
      {
        position = string.Format(CultureInfo.CurrentCulture, " : Position {0}", this.Position);
      }

      return string.Format(
        CultureInfo.CurrentCulture,
        "{0:dd/MM/yyyy HH:mm:ss.ffff}{1} : {2}", 
        this.Date,
        position,
        this.Contenu);
    }
  }
}
