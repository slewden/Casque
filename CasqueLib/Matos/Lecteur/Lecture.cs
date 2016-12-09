using System;

namespace CasqueLib.Matos.Lecteur
{
  /// <summary>
  /// Mémorise une lecture d'un tag par un lecteur
  /// </summary>
  public class Lecture
  {
    /// <summary>
    /// Un numero de Lecture vide : parfois renvoyé par le lecteur
    /// </summary>
    private const string EMPTYTAGNUMERO = "00 00 00 00 00";

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="Lecture"/>
    /// </summary>
    /// <param name="dt">La date</param>
    /// <param name="position">La position du lecteur</param>
    /// <param name="tag">le Tag number</param>
    public Lecture(DateTime dt, int position, string tag)
    {
      this.Date = dt;
      this.LecteurPosition = position;
      this.NumeroTag = tag;
    }

    #region Public porperties
    /// <summary>
    /// Renvoie le numéro de tag
    /// </summary>
    public string NumeroTag { get; private set; }

    /// <summary>
    /// Renvoie la Date de lecture
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// Renvoie la position de la lecture (Emplacement de l'antenne qui a lu, pas son index)
    /// </summary>
    public int LecteurPosition { get; private set; }

    /// <summary>
    /// Indique si le numéro de tag correspond au tag vide (00 00 00 00 00)
    /// </summary>
    public bool IsEmptyNumero
    {
      get
      {
        return Lecture.EMPTYTAGNUMERO.Equals(this.NumeroTag);
      }
    }
    #endregion

    /// <summary>
    /// Compare 2 tag lu pour voir si le délai antiRebond s'applique
    /// </summary>
    /// <param name="tag">Information de Lecture à comparer</param>
    /// <param name="delaiMs">le delai en Ms de l'antirebond à utiliser</param>
    /// <returns>Le résultat de la comparaison</returns>
    public ETagLuComparaison CompareTo(Lecture tag, uint delaiMs)
    {
      if (tag == null)
      { // pas de seconde lectures ==> différent
        return ETagLuComparaison.Differents;
      }
      else if (tag.LecteurPosition != this.LecteurPosition)
      { // pas le même lecteur ==> différent (car déplacement du tag !)
        return ETagLuComparaison.Differents;
      }
      else if (tag.NumeroTag != this.NumeroTag)
      { // pas le même Numéro de tag ==> différent
        return ETagLuComparaison.Differents;
      }
      else if (Convert.ToUInt32(this.Date.Subtract(tag.Date).Duration().TotalMilliseconds) <= delaiMs)
      { // même tag, même lecteur, mais intervale de temps inférieur à l'antiRebond
        return ETagLuComparaison.DansLeDelaiAntiRebond;
      }
      else
      { // même tag, même lecteur, et intervale de temps supérieur à l'antiRebond
        return ETagLuComparaison.HorsDelaiAntiRebond;
      }
    }

    /// <summary>
    /// Pour stockage dans les fichiers de sortie
    /// </summary>
    /// <returns>Le texte du fichier</returns>
    public override string ToString()
    {
      return string.Format("{0:dd/MM/yyyy HH:mm:ss:ffff};{1};{2}", this.Date, this.LecteurPosition, this.NumeroTag);
    }
  }
}
