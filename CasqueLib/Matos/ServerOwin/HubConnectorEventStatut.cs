using System;
using System.Text;
using Microsoft.AspNet.SignalR.Client;

namespace CasqueLib.Matos.ServerOwin
{
  /// <summary>
  /// Classe de paramètre pour les changements de statut de connexion du hub
  /// </summary>
  public class HubConnectorEventStatut : EventArgs
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="HubConnectorEventStatut"/>
    /// </summary>
    /// <param name="oldStatus">L'ancien statut</param>
    /// <param name="newStatus">le nouveau statut</param>
    /// <param name="fromError">Indique si le changement est due à une erreur ou pas</param>
    /// <param name="message">Le message associé</param>
    /// <param name="param">Paramètre supplémentaire optionnel</param>
    public HubConnectorEventStatut(ConnectionState oldStatus, ConnectionState newStatus, bool fromError, string message, object param = null)
    {
      this.OldStatus = oldStatus;
      this.NewStatus = newStatus;
      this.FromError = fromError;
      this.Message = message;
      this.Parameter = param;
    }

    /// <summary>
    /// L'ancien statut
    /// </summary>
    public ConnectionState OldStatus { get; private set; }

    /// <summary>
    /// Le nouveau statut
    /// </summary>
    public ConnectionState NewStatus { get; private set; }

    /// <summary>
    /// Indique si le changement est due à une erreur ou pas
    /// </summary>
    public bool FromError { get; private set; }

    /// <summary>
    /// Le message associé
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// Paramètre supplémentaire optionnel
    /// </summary>
    public object Parameter { get; private set; }

    /// <summary>
    /// Pour affichage
    /// </summary>
    /// <returns>Le texte a afficher</returns>
    public override string ToString()
    {
      StringBuilder res = new StringBuilder();
      if (this.OldStatus != this.NewStatus)
      {
        res.AppendFormat("Changement de statut de {0} vers {1}", this.OldStatus, this.NewStatus);

        if (this.FromError)
        {
          res.Append(" : une erreur a généré ce message");
        }
      }

      if (!string.IsNullOrWhiteSpace(this.Message))
      {
        if (this.OldStatus == this.NewStatus)
        {
          res.Append(this.FromError ? "Erreur" : "Message");
        }

        res.AppendFormat(" : {0}", this.Message);
      }

      if (this.Parameter != null && !string.IsNullOrWhiteSpace(this.Parameter.ToString()))
      {
        res.AppendFormat(", {0}", this.Parameter);
      }

      return res.ToString();
    }
  }
}
