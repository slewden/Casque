using System;
using System.Globalization;

namespace CasqueLib.Matos.Ping
{
  /// <summary>
  /// Notifie une information texte
  /// </summary>
  public class NotifiePingArgs : EventArgs 
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="NotifiePingArgs"/>
    /// </summary>
    /// <param name="nfo">L'info à notifier</param>
    /// <param name="st">Statut du ping</param>
    /// <param name="statutKoDepuisLe">Date de la première demande de ping infructueuse</param>
    public NotifiePingArgs(string nfo, EPingStatut st, DateTime statutKoDepuisLe)
    {
      this.Info = nfo;
      this.Statut = st;
      this.KoDepuisLe = statutKoDepuisLe;
    }

    /// <summary>
    /// L'information à notifier
    /// </summary>
    public string Info { get; private set; }

    /// <summary>
    /// Etat de la notification
    /// </summary>
    public EPingStatut Statut { get; private set; }

    /// <summary>
    /// Date depuis quand le ping est ko
    /// </summary>
    public DateTime KoDepuisLe { get; private set; }

    /// <summary>
    /// Méthode d'Affichage
    /// </summary>
    /// <returns>le texte de l'objet</returns>
    public override string ToString()
    {
      return string.Format(
        CultureInfo.CurrentCulture,
        "{0:dd/MM/yyyy HH:mm:ss.ff} : {1} : {2}",
        DateTime.Now,
        this.Statut,
        this.Info);
    }
  }
}
