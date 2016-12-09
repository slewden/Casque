using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CasqueLib.Matos.ServerOwin;

namespace CasqueLib.Matos.Encodeur
{
  /// <summary>
  /// Event pour les évenements de progression liés à un encodeur
  /// </summary>
  public class SimpleEncodeurProgressArgs : EventArgs
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="SimpleEncodeurProgressArgs"/>
    /// </summary>
    /// <param name="clientId">Le client</param>
    /// <param name="action">L'action en cours</param>
    /// <param name="cle">La clé associées</param>
    /// <param name="index">L'index de progression</param>
    /// <param name="total">Le nombre d'étapes totale</param>
    public SimpleEncodeurProgressArgs(string clientId, EActionEncode action, int cle, int index, int total)
    {
      this.ClientId = clientId;
      this.Action = action;
      this.Cle = cle;
      this.Index = index;
      this.Total = total;
    }

    /// <summary>
    /// Le client
    /// </summary>
    public string ClientId { get; private set; }

    /// <summary>
    /// L'action en cours
    /// </summary>
    public EActionEncode Action { get; private set; }
    
    /// <summary>
    /// La clé associée
    /// </summary>
    public int Cle { get; private set; }

    /// <summary>
    /// La position en cours
    /// </summary>
    public int Index { get; private set; }

    /// <summary>
    /// Le nombre d'étape total
    /// </summary>
    public int Total { get; private set; }

    /// <summary>
    /// Pour affichage
    /// </summary>
    /// <returns>Le texte</returns>
    public override string ToString()
    {
      if (this.Index == 0 && this.Total == 0)
      {
        return "En Attente";
      }
      else
      {
        return string.Format("Progression de {0} sur {1}", this.Index, this.Total);
      }
    }
  }
}
