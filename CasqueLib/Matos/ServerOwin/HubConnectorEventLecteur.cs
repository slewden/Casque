using System;
using CasqueLib.Matos.Lecteur;

namespace CasqueLib.Matos.ServerOwin
{
  /// <summary>
  /// Information sur une demande d'action à un lecteur
  /// </summary>
  public class HubConnectorEventLecteur : EventArgs
  {
    /// <summary>
    /// l'adresse Ip décryptée
    /// </summary>
    private string adresseIp = string.Empty;

    /// <summary>
    /// Action demandée pour le lecteur
    /// </summary>
    public EActionLecteur Action { get; set; }

    /// <summary>
    /// L'identifiant du client sur le hub a qui notifier les infos
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// Les paramètres du lecteur en XML
    /// </summary>
    public string XmlParameter { get; set; }

    /// <summary>
    /// L'adresse IP du lecteur à appeller
    /// </summary>
    public string AdresseIp
    {
      get
      { 
        if (string.IsNullOrWhiteSpace(this.adresseIp))
        {
          this.DecrypteParametre();
        }

        return this.adresseIp;
      }
    }

    /// <summary>
    /// Pour affichage dans les log
    /// </summary>
    /// <returns>le texte a afficher</returns>
    public override string ToString()
    {
      return string.Format("{0} pour client {1} vers lecteur {2}", this.Action.GetName(), this.ClientId, this.AdresseIp);
    }

    /// <summary>
    /// Analyse le XML de paramètre pour extraire l'adresse IP
    /// </summary>
    private void DecrypteParametre()
    {
      SimpleReaderParameters param = new SimpleReaderParameters(this.XmlParameter);
      if (param.IsValid.Valid)
      {
        this.adresseIp = param.AdresseIP;
      }
    }
  }
}
