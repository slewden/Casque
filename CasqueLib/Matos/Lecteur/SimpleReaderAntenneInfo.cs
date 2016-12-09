using System.Collections.Generic;
using System.Linq;

namespace CasqueLib.Matos.Lecteur
{
  /// <summary>
  /// Retranscrit ce qu'il se passe sur une Antenne
  /// Et fournit les constantes du lecteur
  /// </summary>
  public class SimpleReaderAntenneInfo
  {
    /// <summary>
    /// Valeur min du gain
    /// </summary>
    public const int GAINMIN = 10;
    
    /// <summary>
    /// Valeur max du gain
    /// </summary>
    public const int GAINMAX = 32;

    /// <summary>
    /// Valeur max du gain par défaut
    /// </summary>
    public const int GAINDEFAULT = 24;

    /// <summary>
    /// La premiere valeur acceptée pour une position d'antenne
    /// </summary>
    public const int FIRSTPOSITION = 1;
    
    /// <summary>
    /// La dernière valeur acceptée pour une position d'antenne
    /// </summary>
    public const int LASTPOSITION = 4;

    /// <summary>
    /// Code pour indiquer une position d'antenne non renseignée
    /// </summary>
    public const int ALLPOSITION = 9999;

    /// <summary>
    /// Nombre maximum d'éléments dans la liste anti-rebond
    /// </summary>
    private const int MAXLISTANTIREBOND = 16;
    
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="SimpleReaderAntenneInfo"/>
    /// </summary>
    public SimpleReaderAntenneInfo()
    {
      this.Lectures = new Queue<Lecture>(SimpleReaderAntenneInfo.MAXLISTANTIREBOND); // on ne garde que les x dernières lectures
    }

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="SimpleReaderAntenneInfo"/>
    /// Mémorise l'activité sur une antenne du lecteur
    /// </summary>
    /// <param name="index">Index de l'antenne de 0 à 3</param>
    /// <param name="position">Position sur la ruche de 1 à 4</param>
    /// <param name="gainDB">Le gain pour l'antenne</param>
    public SimpleReaderAntenneInfo(int index, int position, int gainDB)
    {
      this.AntenneIndex = index;
      this.Position = position;
      this.GainDB = gainDB;
      this.Active = position >= SimpleReaderAntenneInfo.FIRSTPOSITION && position <= SimpleReaderAntenneInfo.LASTPOSITION && gainDB >= SimpleReaderAntenneInfo.GAINMIN && gainDB <= GAINMAX;
      this.Lectures = new Queue<Lecture>(SimpleReaderAntenneInfo.MAXLISTANTIREBOND); // on ne garde que les x dernières lectures
    }

    /// <summary>
    /// Index de l'antenne de 0 à 3
    /// </summary>
    public int AntenneIndex { get; private set; }

    /// <summary>
    /// Indique si l'antenne est active ou pas
    /// </summary>
    public bool Active { get; private set; }

    /// <summary>
    /// Le gain de l'antenne
    /// </summary>
    public int GainDB { get; private set; }

    /// <summary>
    /// Position de l'antenne (de 1 à 4)
    /// </summary>
    public int Position { get; private set; }

    /// <summary>
    /// Liste des x dernières lectures de tag
    /// </summary>
    public Queue<Lecture> Lectures { get; private set; }

    /// <summary>
    /// Renvoie un simepleReaderAntenne info vide = avec les valeurs par défaut et non activé
    /// </summary>
    /// <param name="position">La position du lecteur</param>
    /// <returns>L'objet initialisé</returns>
    public static SimpleReaderAntenneInfo Empty(int position)
    {
      return new SimpleReaderAntenneInfo()
                      {
                        Active = false,
                        GainDB = SimpleReaderAntenneInfo.GAINDEFAULT,
                        Position = position,
                      };
    }

    /// <summary>
    /// Indique si oui ou non, le tag lu est dans le délai anti rebond ou pas
    /// </summary>
    /// <param name="tl">informations d'une lecture</param>
    /// <param name="delaiMs">Le délai d'anti rebond en Ms</param>
    /// <returns>true si le tag est dans la liste et dans le delai anti-rebond</returns>
    public bool AntiRebond(Lecture tl, uint delaiMs)
    {
      if (this.Lectures.Count == 0)
      { // Pas de données pas d'anti-rebond
        return false;
      }

      if (delaiMs <= 10)
      { // La config est considérée comme sans antirebond
        return false;
      }

      var nfos = this.Lectures.Where(x => x.NumeroTag.Equals(tl.NumeroTag));
      foreach (var nfo in nfos)
      {
        if (nfo.CompareTo(tl, delaiMs) == ETagLuComparaison.DansLeDelaiAntiRebond)
        { // on est dedans !!
          return true;
        }
      }

      return false;
    }
  }
}
