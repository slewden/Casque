using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CasqueLib.Matos.Encodeur;
using CasqueLib.Matos.Lecteur;
using CasqueLib.Matos.ServerOwin;

namespace CasqueLib.Matos.ClientOwin
{
  /// <summary>
  /// Défini le contrat pour un encodeur
  /// </summary>
  public interface IWriter : IDisposable
  {
    /// <summary>
    /// Event pour notifier des infos au parent
    /// </summary>
    event EventHandler<SimpleEncodeurEventArgs> OnNotifie;

    /// <summary>
    /// Event pour notifier la progression
    /// </summary>
    event EventHandler<SimpleEncodeurProgressArgs> OnProgress;

    /// <summary>
    /// les paramètres de l'encodeur
    /// </summary>
    SimpleReaderParameters Parameters { set; }

    /// <summary>
    /// Le client
    /// </summary>
    string ClientId { get; set; }
   
    /// <summary>
    /// L'adresse de l'encodeur
    /// </summary>
    string AdresseIp { get; }

    /// <summary>
    /// L'action en cours de traitement
    /// </summary>
    EActionEncode Action { get; }

    /// <summary>
    /// La clé en cours de traitement
    /// </summary>
    int Cle { get; }

    /// <summary>
    /// La position actuelle dans le traitement
    /// </summary>
    int Index { get; }

    /// <summary>
    /// Le nombre d'étapes dans le traitement
    /// </summary>
    int Total { get; }

    /// <summary>
    /// Indique si l'encodeur est en cours de traitement d'une demande non compatible avec celle en paramètre
    /// </summary>
    /// <param name="e">les infos de la demande</param>
    /// <returns>true si l'encodeur ne peut pas traiter la demande</returns>
    bool Busy(HubConnectorEventEncodeur e);

    /// <summary>
    /// Lance le traitement d'une commande
    /// </summary>
    /// <param name="clientId">Le client</param>
    /// <param name="commandeCle">La clé de la commande</param>
    void ProcessCommande(string clientId, int commandeCle);

    /// <summary>
    /// lance le traitement d'un assemblage
    /// </summary>
    /// <param name="clientId">Le client</param>
    /// <param name="assemblageCle">La clé de l'assemblage</param>
    void ProcessAssemblage(string clientId, int assemblageCle);

    /// <summary>
    /// Annule le traitement en cours
    /// </summary>
    void Cancel();
  }
}
