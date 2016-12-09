using System;
using System.Text;
using CasqueLib.Matos.Lecteur;

namespace CasqueLib.Matos.ServerOwin
{
  /// <summary>
  /// Information sur une demande d'action à un encodeur
  /// </summary>
  public class HubConnectorEventEncodeur : EventArgs
  {
    /// <summary>
    /// l'adresse Ip décryptée
    /// </summary>
    private string adresseIp = string.Empty;

    /// <summary>
    /// Action demandée pour l'encodeur
    /// </summary>
    public EActionEncode Action { get; set; }

    /// <summary>
    /// En fonction de l'action : clé de l'objet à encoder (commandeCle, ou assemblageCle)
    /// </summary>
    public int Cle { get; set; }

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
    /// Pour affichage
    /// </summary>
    /// <returns>Le texte a afficher</returns>
    public override string ToString()
    {
      return string.Format("{0} : {1} pour {2}", this.Action.GetName(), this.Cle, this.ClientId);
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
