using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace CasqueLib.Matos.Lecteur
{
  /// <summary>
  /// Classe qui gère les options de configuration d'un lecteur
  /// </summary>
  public class SimpleReaderParameters
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="SimpleReaderParameters"/>
    /// </summary>
    public SimpleReaderParameters()
    {
      this.Seuil = 1;
    }
    
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="SimpleReaderParameters"/>
    /// </summary>
    /// <param name="xml">Le xml de config</param>
    public SimpleReaderParameters(string xml)
    {
      // Init des antennes
      this.Antennes = new SimpleReaderAntenneInfo[SimpleReaderAntenneInfo.LASTPOSITION];
      for (int i = 0; i < this.Antennes.Length; i++)
      {
        this.Antennes[i] = SimpleReaderAntenneInfo.Empty(i + 1);
      }

      this.Seuil = 1;

      if (!string.IsNullOrWhiteSpace(xml))
      {
        XElement rac = null;
        try
        {
          rac = XElement.Parse(xml);
        }
        finally
        {
          if (rac != null)
          {
            XElement adr = rac.Element("adresseIp");
            if (adr != null)
            { // y a une info adresse IP
              this.AdresseIP = adr.Value;
            }

            XElement sl = rac.Element("seuil");
            if (sl != null)
            {
              int n;
              if (int.TryParse(sl.Value, out n))
              { // c'est bien un nombre
                if (n > 0)
                { // c'est un nombre positif supérieur à 0
                  this.Seuil = n;
                }
              }
            }

            XElement ke = rac.Element("keepAlive");
            if (ke != null)
            { // y a un keep alive
              XAttribute att = ke.Attribute("intervalSec");
              if (att != null && !string.IsNullOrWhiteSpace(att.Value))
              {
                uint n;
                if (uint.TryParse(att.Value, out n))
                { // délai valide
                  this.KeepAliveIntervalSeconds = n;
                }
              }
            }

            XElement ar = rac.Element("antiRebond");
            if (ar != null)
            { // y a un anti-rebond
              XAttribute att = ke.Attribute("intervalMs");
              if (att != null && !string.IsNullOrWhiteSpace(att.Value))
              {
                uint n;
                if (uint.TryParse(att.Value, out n))
                { // délai valide
                  this.AntiRebonIntervalMilliseconds = n;
                }
              }
            }

            XElement ants = rac.Element("antennes");
            if (ants != null)
            {
              int index, position, gainDb;
              foreach (XElement ant in ants.Elements("antenne"))
              {
                index = this.GetInt(ant, "index", -1);
                position = this.GetInt(ant, "position", -1);
                gainDb = this.GetInt(ant, "gainDb", -1);
                if (index >= 0 && index < SimpleReaderAntenneInfo.LASTPOSITION
                   && position >= SimpleReaderAntenneInfo.FIRSTPOSITION && position <= SimpleReaderAntenneInfo.LASTPOSITION
                   && gainDb >= SimpleReaderAntenneInfo.GAINMIN && gainDb <= SimpleReaderAntenneInfo.GAINMAX)
                {
                  this.Antennes[index] = new SimpleReaderAntenneInfo(index, position, gainDb);
                }
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// Adresse IP V4 du lecteur ou url de connexion
    /// </summary>
    public string AdresseIP { get; private set; }

    /// <summary>
    /// Nombre de lectures a constater avant de notifier un Tag Read
    /// Par défaut 0 ou 1 ==> notification à la première lecture
    /// </summary>
    public int Seuil { get; private set; }

    /// <summary>
    /// Infos sur les antennes
    /// </summary>
    public SimpleReaderAntenneInfo[] Antennes { get; private set; }

    /// <summary>
    /// Fréquence d'envoie en secondes si 0 pas fait !
    /// </summary>
    public uint KeepAliveIntervalSeconds { get; private set; }

    /// <summary>
    /// Délai d'antirebond en ms  désactivé si inférieur à 10ms
    /// </summary>
    public uint AntiRebonIntervalMilliseconds { get; private set; }

    /// <summary>
    /// Renvoie les infos si le reader parameter est ou pas valide
    /// </summary>
    public ValidInfo IsValid
    {
      get
      {
        if (string.IsNullOrEmpty(this.AdresseIP))
        { // pas d'adresse ==> erreur
          return ValidInfo.Ko("L'adresse du lecteur ne peut être vide");
        }

        if (!SimpleReaderParameters.ValideUrl(this.AdresseIP))
        { // adresse invalide ==> erreur
          return ValidInfo.Ko("L'adresse du lecteur n'est pas une adresse IP V4 valide");
        }

        if (!this.IsOneAntennesValide())
        {
          return ValidInfo.Ko("Il faut configurer au moins une antenne (Emplacement et Gain)");
        }

        return ValidInfo.Ok();
      }
    }

    /// <summary>
    /// Indique si l'adresse IP du lecteur est Ok
    /// </summary>
    /// <param name="url">L'adresse IP du lecteur à valider</param>
    /// <returns>True si ok</returns>
    public static bool ValideUrl(string url)
    {
      if (string.IsNullOrEmpty(url))
      { // pas d'adresse ==> erreur
        return false;
      }

      Regex r = new Regex(@"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}");
      if (!r.IsMatch(url))
      { // adresse invalide ==> erreur
        return false;
      }

      return true;
    }

    /// <summary>
    /// Indique si au moins une antenne est bien configurée et active
    /// </summary>
    /// <returns>True si une antenne est configurée</returns>
    public bool IsOneAntennesValide()
    {
      if (this.Antennes != null)
      {
        for (int ant = 0; ant < SimpleReaderAntenneInfo.LASTPOSITION; ant++)
        {
          if (this.Antennes[ant] != null && this.Antennes[ant].Active)
          {
            return true;
          }
        }
      }

      return false;
    }

    /// <summary>
    /// Renvoie la config en XML pour rechargement ultérieur
    /// </summary>
    /// <returns>LE xml de la config</returns>
    public override string ToString()
    {
      XElement rac = new XElement("config");
      rac.Add(new XElement("adresseIp", this.AdresseIP));

      if (this.Seuil > 0)
      {
        rac.Add(new XElement("seuil", this.Seuil));
      }

      if (this.KeepAliveIntervalSeconds > 0)
      {
        rac.Add(new XElement("keepAlive", new XAttribute("intervalSec", this.KeepAliveIntervalSeconds)));
      }

      if (this.AntiRebonIntervalMilliseconds > 0)
      {
        rac.Add(new XElement("antiRebond", new XAttribute("intervalMs", this.AntiRebonIntervalMilliseconds)));
      }

      XElement ant = new XElement("antennes");
      rac.Add(ant);
      XElement e;
      foreach (SimpleReaderAntenneInfo antenne in this.Antennes)
      {
        if (antenne != null)
        {
          e = new XElement("antenne");
          e.Add(new XAttribute("index", antenne.AntenneIndex));
          e.Add(new XAttribute("gainDb", antenne.GainDB));
          e.Add(new XAttribute("position", antenne.Position));
          ant.Add(e);
        }
      }

      return rac.ToString();
    }

    /// <summary>
    /// Renvoie un int lu dans l'attribut 'attribut' du noeud xml 'elem'
    /// </summary>
    /// <param name="elem">Le noeud xml</param>
    /// <param name="attribut">Le nom de l'attribut à lire</param>
    /// <param name="defaultValue">La valeur si non trouvé ou invalide</param>
    /// <returns>la valeur lue</returns>
    private int GetInt(XElement elem, string attribut, int defaultValue)
    {
      XAttribute att = elem.Attribute(attribut);
      if (att != null && !string.IsNullOrWhiteSpace(att.Value))
      {
        int n;
        if (int.TryParse(att.Value, out n))
        {
          return n;
        }
      }

      return defaultValue;
    }

    /// <summary>
    /// Classe pour renvoyer les informations de validité des paramètres
    /// </summary>
    public class ValidInfo
    {
      /// <summary>
      /// Initialise une nouvelle instance de la classe <see cref="ValidInfo"/>
      /// </summary>
      /// <param name="ok">est valide ou pas</param>
      /// <param name="message">Le message</param>
      private ValidInfo(bool ok, string message)
      {
        this.Valid = ok;
        this.Message = message;
      }

      /// <summary>
      /// Est valide ou pas
      /// </summary>
      public bool Valid { get; private set; }

      /// <summary>
      /// Le message si non valide (vide sinon)
      /// </summary>
      public string Message { get; private set; }

      /// <summary>
      /// Renvoie un objet ValidInfo OK
      /// </summary>
      /// <returns>l'objet ValidInfo OK</returns>
      public static ValidInfo Ok()
      {
        return new ValidInfo(true, string.Empty);
      }

      /// <summary>
      /// Renvoie un objet ValidInfo KO
      /// </summary>
      /// <param name="message">LE message d'erreur associé</param>
      /// <returns>L'objet ValidInfo KO</returns>
      public static ValidInfo Ko(string message)
      {
        return new ValidInfo(false, message);
      }
    }
  }
}
