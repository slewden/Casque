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
  /// Contrat pour les lecteurs
  /// </summary>
  public interface IReader : IDisposable
  {
    /// <summary>
    /// Le lecteur publi des informations
    /// </summary>
    event EventHandler<SimpleReaderEventArgs> OnNotifie;

    /// <summary>
    /// les paramètres du lecteur
    /// </summary>
    SimpleReaderParameters Parameters { set; }

    /// <summary>
    /// Le client
    /// </summary>
    string ClientId { get; set; }

    /// <summary>
    /// L'adresse du lecteur
    /// </summary>
    string AdresseIp { get;  }

    /// <summary>
    /// Le lecteur peut démarrer
    /// </summary>
    bool CanStart { get; }

    /// <summary>
    /// Le lecteur est en route
    /// </summary>
    bool Running { get; }

    /// <summary>
    /// le lecteur peut s'arreter
    /// </summary>
    bool CanStop { get; }

    /// <summary>
    /// Démarre le lecteur
    /// </summary>
    /// <param name="clientId">le client associé</param>
    void Start(string clientId);

    /// <summary>
    /// Arrete le lecteur
    /// </summary>
    void Stop();

    /// <summary>
    /// Reinitialise les lectures
    /// </summary>
    void ResetLectures();
  }
}
