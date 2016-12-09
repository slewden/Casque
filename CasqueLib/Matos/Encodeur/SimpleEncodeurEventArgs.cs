using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CasqueLib.Matos.ServerOwin;

namespace CasqueLib.Matos.Encodeur
{
  /// <summary>
  /// Renvoie les infos quand un encodeur a besoin de parler à l'application
  /// </summary>
  public class SimpleEncodeurEventArgs : EventArgs
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="SimpleEncodeurEventArgs"/>
    /// </summary>
    /// <param name="clientId">Le client</param>
    /// <param name="action">L'action demandée</param>
    /// <param name="cle">La clé associée</param>
    /// <param name="erreur">Sucess ou erreur</param>
    /// <param name="msg">Le message</param>
    public SimpleEncodeurEventArgs(string clientId, EActionEncode action, int cle, bool erreur, string msg)
    {
      this.ClientId = clientId;
      this.Action = action;
      this.Cle = cle;
      this.Erreur = erreur;
      this.Message = msg;
    }

    /// <summary>
    /// Le client
    /// </summary>
    public string ClientId { get; private set; }

    /// <summary>
    /// L'action demandé
    /// </summary>
    public EActionEncode Action { get; private set; }

    /// <summary>
    /// La clé fonction de l'action
    /// </summary>
    public int Cle { get; private set; }

    /// <summary>
    /// Le résultat
    /// </summary>
    public bool Erreur { get; private set; }

    /// <summary>
    /// Le message
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// pour affichage
    /// </summary>
    /// <returns>Le texte</returns>
    public override string ToString()
    {
      return string.Format("Action {0}({1}) : {2} {3}", this.Action.GetName(), this.Cle, this.Erreur ? "Erreur" : "Ok", this.Message);
    }
  }
}
