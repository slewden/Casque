using System.Collections.Generic;
using CasqueLib.Matos.ServerOwin;

namespace CasqueReaderSimulator
{
  /// <summary>
  /// Mémorise les infos d'un lecteur en route
  /// </summary>
  public class LecteurDatas
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="LecteurDatas"/>
    /// </summary>
    /// <param name="clientId">Le code client</param>
    /// <param name="xmlConfig">Le xml de config</param>
    public LecteurDatas(string clientId, string xmlConfig)
    {
      this.ClientId = clientId;
      this.XMLConfig = xmlConfig;
      this.Lectures = new List<Compteur>();
    }

    #region Properties
    /// <summary>
    /// Lae lecteur est en route
    /// </summary>
    public bool EnRoute { get; set; }

    /// <summary>
    /// La clé du client
    /// </summary>
    public string ClientId { get; private set; }

    /// <summary>
    /// Le xml de config
    /// </summary>
    public string XMLConfig { get; private set; }

    /// <summary>
    /// La liste des lectures
    /// </summary>
    public List<Compteur> Lectures { get; private set; }

    /// <summary>
    /// Clé de la commande reçue
    /// </summary>
    public int CommandeId { get; private set; }

    /// <summary>
    /// L'index dans la progression
    /// </summary>
    public int CommandeProgressionIndex { get; set; }

    /// <summary>
    /// Le total dans la progression
    /// </summary>
    public int CommandeProgressionTotal { get; set; }

    /// <summary>
    /// La clé de l'assemblage
    /// </summary>
    public int AssemblageId { get; private set; }

    /// <summary>
    /// Action start demandée
    /// </summary>
    public bool ActionStart { get; set; }

    /// <summary>
    /// Action stop demandée
    /// </summary>
    public bool ActionStop { get; set; }

    /// <summary>
    /// Action reset demandée
    /// </summary>
    public bool ActionReset { get; set; }

    /// <summary>
    /// Action encode commande demandée
    /// </summary>
    public bool ActionEncodeCommande { get; private set; }

    /// <summary>
    /// Action encode assemblage demandée
    /// </summary>
    public bool ActionEncodeAssemblage { get; private set; }

    /// <summary>
    /// Type d'action d'encodage en cours
    /// </summary>
    public EActionEncode EncodeAction
    {
      get
      {
        if (this.ActionEncodeCommande)
        {
          return EActionEncode.TraiteCommande;
        }
        else if (this.ActionEncodeAssemblage)
        {
          return EActionEncode.TraiteAssemblage;
        }
        else
        {
          return EActionEncode.Aucune;
        }
      }
    }

    /// <summary>
    /// la clé en fonction du Type d'action d'encodage en cours
    /// </summary>
    public int EncodeCle
    {
      get
      {
        if (this.ActionEncodeCommande)
        {
          return this.CommandeId;
        }
        else if (this.ActionEncodeAssemblage)
        {
          return this.AssemblageId;
        }
        else
        {
          return 0;
        }
      }
    }

    /// <summary>
    /// Action en cours pour la lecture
    /// </summary>
    public EActionLecteur LectureAction
    {
      get
      {
        if (this.ActionStart)
        {
          return EActionLecteur.Demarre;
        }
        else if (this.ActionReset)
        {
          return EActionLecteur.ResetLecture;
        }
        else if (this.ActionStop)
        {
          return EActionLecteur.Stoppe;
        }
        else
        {
          return EActionLecteur.Acune;
        }
      }
    }
    #endregion

    #region Méthodes lecteurs
    /// <summary>
    /// Notifie la fin du démarrage d'un lecteur
    /// </summary>
    /// <param name="connector">Le hub</param>
    /// <param name="error">est ce une erreur</param>
    /// <param name="message">le message</param>
    public void Start(HubConnector connector, bool error, string message)
    {
      this.EnRoute = true;
      this.Lectures.Clear();
      connector.Notify(this.ClientId, error, EActionLecteur.Demarre, message);
      this.ActionStart = false;
    }

    /// <summary>
    /// Notifie la fin de l'arrêt d'un lecteur
    /// </summary>
    /// <param name="connector">Le hub</param>
    /// <param name="error">est ce une erreur</param>
    /// <param name="message">le message</param>
    public void Stop(HubConnector connector, bool error, string message)
    {
      this.EnRoute = false;
      this.Lectures.Clear();
      connector.Notify(this.ClientId, error, EActionLecteur.Stoppe, message);
      this.ActionStop = false;
    }

    /// <summary>
    /// Notifie la fin du reset des tag lus d'un lecteur
    /// </summary>
    /// <param name="connector">Le hub</param>
    /// <param name="error">est ce une erreur</param>
    /// <param name="message">le message</param>
    public void Reset(HubConnector connector, bool error, string message)
    {
      this.Lectures.Clear();
      connector.Notify(this.ClientId, error, EActionLecteur.ResetLecture, message);
      this.ActionReset = false;
    }
    #endregion

    #region Méthodes Encodeurs
    /// <summary>
    /// Défini une demande d'encodage d'une commande
    /// </summary>
    /// <param name="connector">Le hub</param>
    /// <param name="cmd">la clé de la commande</param>
    /// <returns>True si ok</returns>
    public bool SetCommande(HubConnector connector, int cmd)
    {
      if (this.EncodeAction == EActionEncode.Aucune)
      {
        this.CommandeId = cmd;
        this.ActionEncodeCommande = true;
        this.CommandeProgressionIndex = 0;
        this.CommandeProgressionTotal = 1; // Arbitraire a ce stade
        this.Progresse(connector, true);
        return true;
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// Défini une demande d'encodage d'un assemblage
    /// </summary>
    /// <param name="connector">Le hub</param>
    /// <param name="asseId">la clé de l'assemblage</param>
    /// <returns>true is ok</returns>
    public bool SetAssemblage(HubConnector connector, int asseId)
    {
      if (this.EncodeAction == EActionEncode.Aucune)
      {
        this.AssemblageId = asseId;
        this.ActionEncodeAssemblage = true;
        this.CommandeProgressionIndex = 0;
        this.CommandeProgressionTotal = 1;
        this.Progresse(connector, true);
        return true;
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// Envoie une info de progression d'impression d'une commande
    /// </summary>
    /// <param name="connector">Le hub</param>
    /// <param name="progresse">incremente le compteur après envoie</param>
    public void Progresse(HubConnector connector, bool progresse)
    {
      // on renvoie toujour l'etat en cours de progression : cette méthode sert à savoir ou en est le lecteur
      connector.Progresse(this.ClientId, this.EncodeAction, this.EncodeCle, this.CommandeProgressionIndex, this.CommandeProgressionTotal);

      if (this.EncodeCle > 0 && this.EncodeAction != EActionEncode.Aucune)
      {
        if (progresse && this.CommandeProgressionIndex < this.CommandeProgressionTotal)
        {
          this.CommandeProgressionIndex++;
        }
      }
    }

    /// <summary>
    /// Notifie l'annulation de tout encodage en cours
    /// </summary>
    /// <param name="connector">Le hub</param>
    /// <param name="message">le message</param>
    public void EncodeCancel(HubConnector connector, string message)
    {
      connector.Rapporte(this.ClientId, true, this.EncodeAction, this.EncodeCle, message);
      this.CommandeId = 0;
      this.AssemblageId = 0;
      this.ActionEncodeCommande = false;
      this.ActionEncodeAssemblage = false;
      this.CommandeProgressionIndex = 0;
      this.CommandeProgressionTotal = 0;
    }

    /// <summary>
    /// Notifie la fin de l'encodage d'une commande
    /// </summary>
    /// <param name="connector">Le hub</param>
    /// <param name="error">est ce une erreur</param>
    /// <param name="message">le message</param>
    public void EncodeCommande(HubConnector connector, bool error, string message)
    {
      if (this.CommandeId > 0)
      {
        connector.Rapporte(this.ClientId, error, EActionEncode.TraiteCommande, this.CommandeId, message);
        this.CommandeId = 0;
        this.ActionEncodeCommande = false;
        this.CommandeProgressionIndex = 0;
        this.CommandeProgressionTotal = 0;
      }
    }

    /// <summary>
    /// Notifie la fin de l'encodage d'un assemblage
    /// </summary>
    /// <param name="connector">Le hub</param>
    /// <param name="error">est ce une erreur</param>
    /// <param name="message">le message</param>
    public void EncodeAssemblage(HubConnector connector, bool error, string message)
    {
      if (this.AssemblageId > 0)
      {
        connector.Rapporte(this.ClientId, error, EActionEncode.TraiteAssemblage, this.AssemblageId, message);
        this.AssemblageId = 0;
        this.ActionEncodeAssemblage = false;
        this.CommandeProgressionIndex = 0;
        this.CommandeProgressionTotal = 0;
      }
    }
    #endregion
  }
}
