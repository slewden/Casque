using System;
using System.Net.NetworkInformation;

namespace CasqueLib.Matos.Ping
{
  /// <summary>
  /// Envoie une demande PING à une url jusqu'a réponse ou nombre de tentatives atteind
  /// </summary>
  public class Pinger
  {
    #region Membres privés
    /// <summary>
    /// Délai d'attente avant une décision de time Out
    /// </summary>
    private TimeSpan dureeAttente;
    #endregion

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="Pinger"/>
    /// </summary>
    /// <param name="urlLecteur">Url du lecteur</param>
    /// <param name="delai">Le temps d'attente avant de déclarer un time out</param>
    public Pinger(string urlLecteur, TimeSpan delai)
    {
      if (string.IsNullOrEmpty(urlLecteur))
      { // on s'assure que les erreur de programmation soit détectées !!
        throw new ArgumentException("URL vide", "urlLecteur");
      }

      if (delai == null || delai == TimeSpan.Zero)
      { // on s'assure que les erreur de programmation soit détectées !!
        throw new ArgumentException("Delai vide", "delai");
      }

      this.Url = urlLecteur;
      this.dureeAttente = delai;
      this.LastDate = DateTime.MinValue;
      this.Statut = EPingStatut.Waiting;
      this.KoDepuisLe = DateTime.MaxValue; // jamais !
    }

    #region Event, propriétés & méthodes publiques
    /// <summary>
    /// Pour notifier le parent de ce qu'il se passe
    /// </summary>
    public event EventHandler<NotifiePingArgs> OnPingAction;

    /// <summary>
    /// L'url à interroger
    /// </summary>
    public string Url { get; private set; }

    /// <summary>
    /// Renvoie le statut du ping
    /// </summary>
    public EPingStatut Statut { get; private set; }

    /// <summary>
    /// Date à laquelle on a envoyé le ping
    /// </summary>
    public DateTime LastDate { get; private set; }

    /// <summary>
    /// Date à laquelle on est Ko
    /// </summary>
    public DateTime KoDepuisLe { get; private set; }

    /// <summary>
    /// Démarre le ping
    /// </summary>
    public void Ping()
    {
      using (System.Net.NetworkInformation.Ping png = new System.Net.NetworkInformation.Ping())
      {
        png.PingCompleted += this.PngPingCompleted;
        this.LastDate = DateTime.Now;
        this.Statut = EPingStatut.Requesting;
        this.Notifie(string.Format("Envoie d'un ping au lecteur ({0}) ...", this.Url));
        png.SendAsync(this.Url, (int)this.dureeAttente.TotalMilliseconds, null);
      }
    }

    #endregion

    #region Méthodes privées
    /// <summary>
    /// Envoie un event au parent pour donner l'info
    /// </summary>
    /// <param name="msg">Message à fournir</param>
    private void Notifie(string msg)
    {
      if (this.OnPingAction != null)
      {
        this.OnPingAction(this, new NotifiePingArgs(msg, this.Statut, this.KoDepuisLe));
      }
    }

    /// <summary>
    /// Event de réception du pong
    /// </summary>
    /// <param name="sender">qui appelle</param>
    /// <param name="e">les infos du pong</param>
    private void PngPingCompleted(object sender, PingCompletedEventArgs e)
    {
      if (!e.Cancelled && e.Error == null && e.Reply != null)
      { // pas d'annulation ni d'erreur
        if (e.Reply.Status == IPStatus.Success)
        { // statut ok
          this.Statut = EPingStatut.ReceiveOk;
          this.KoDepuisLe = DateTime.MaxValue; // Jamais
          this.Notifie(string.Format("Réception de la réponse du lecteur ({0})", this.Url));
          return;
        }
      }

      if (this.KoDepuisLe == DateTime.MaxValue)
      { // premier ko => on positionne la date
        this.KoDepuisLe = DateTime.Now;
      }

      this.Statut = EPingStatut.ErreurTimeOut;
      this.Notifie(string.Format("Pas de réponse du lecteur ({0})", this.Url));
    }
    #endregion
  }
}