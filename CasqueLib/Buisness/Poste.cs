using System.Collections.Generic;
using CasqueLib.Common;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness
{
  /// <summary>
  /// Mappe un poste de lecteur, d'encodage ou d'impression
  /// </summary>
  [Alias("poste")]
  public class Poste
  {
    /// <summary>
    /// Les différents types de poste
    /// </summary>
    public enum EPosteType
    {
      /// <summary>
      /// Pas un poste
      /// </summary>
      None = 0,

      /// <summary>
      /// Poste d'encodage des tag et impression : utilisé lors de la commande
      /// </summary>
      EncodeTag = 1,

      /// <summary>
      /// Poste de lecture de tag : utilisé lors de l'arrivée des pièces, l'assemblage, les livraisons
      /// </summary>
      Lecture = 2,
    }

    /// <summary>
    /// Valeurs pour déclarer un lecteur ou un encodeur ON
    /// Les valeur doivent être une puissance de 2 distinct des EPosteType
    /// </summary>
    public enum EPosteIsOn
    {
      /// <summary>
      /// Un encodeur est on
      /// </summary>
      WriterIsON = 32,

      /// <summary>
      /// un lecteur est on
      /// </summary>
      LecteurIsON = 64,
    }

    /// <summary>
    /// La clé de la config du poste
    /// </summary>
    [AutoIncrement]
    [Alias("post_id")]
    public int Cle { get; set; }

    /// <summary>
    /// Le type de poste (en int)
    /// </summary>
    [Alias("post_type")]
    public int PosteTypeInt { get; set; }

    /// <summary>
    /// Le type de poste (typé)
    /// </summary>
    [Ignore]
    public EPosteType PosteType
    {
      get
      {
        return (EPosteType)this.PosteTypeInt;
      }

      set
      {
        this.PosteTypeInt = (int)value;
      }
    }

    /// <summary>
    /// Le type de poste (typé + nom)
    /// </summary>
    [Ignore]
    public NomCleImprimante PosteTypeNomCle
    {
      get
      {
        return new NomCleImprimante()
                    {
                      Nom = Poste.GetNomPosteType(this.PosteType),
                      Cle = this.PosteTypeInt,
                      Imprimante = this.PosteType == EPosteType.EncodeTag,
                    };
      }

      set
      {
        this.PosteTypeInt = value.Cle;
      }
    }

    /// <summary>
    /// Le nom du poste
    /// </summary>
    [Alias("post_nom")]
    public string Nom { get; set; }

    /// <summary>
    /// La description du poste
    /// </summary>
    [Alias("post_description")]
    public string Description { get; set; }

    /// <summary>
    /// L'adresse IP du poste
    /// </summary>
    [Alias("page_code")]
    public string PageCode { get; set; }

    /// <summary>
    /// L'affectation du lecteur à une fonction (typé + nom)
    /// </summary>
    [Ignore]
    public DetailPageCode Affectation
    {
      get
      {
        return new DetailPageCode(this.PageCode);
      }

      set
      {
        this.PageCode = value.Code;
      }
    }

    /// <summary>
    /// La config XML du poste fonction du type
    /// </summary>
    [Alias("post_configuration")]
    public string ConfigurationTxt { get; set; }

    /// <summary>
    /// L'adesse Ip du poste demandé
    /// </summary>
    [Ignore]
    public string AdresseIp { get; set; }

    /// <summary>
    /// La liste des infos des postes possibles
    /// </summary>
    /// <returns>La liste</returns>
    public static List<NomCleImprimante> ListPosteTypes()
    {
      List<NomCleImprimante> lst = new List<NomCleImprimante>();
      lst.Add(new NomCleImprimante() { Nom = Poste.GetNomPosteType(EPosteType.EncodeTag), Cle = 1, Imprimante = true });
      lst.Add(new NomCleImprimante() { Nom = Poste.GetNomPosteType(EPosteType.Lecture), Cle = 2, Imprimante = false });
      return lst;
    }

    /// <summary>
    /// La liste des affectations des postes possibles
    /// </summary>
    /// <returns>La liste</returns>
    public static List<DetailPageCode> ListAffectations()
    {
      List<DetailPageCode> lst = new List<DetailPageCode>();
      lst.Add(new DetailPageCode("Commande"));
      lst.Add(new DetailPageCode("Reception"));
      lst.Add(new DetailPageCode("Assemblage"));
      lst.Add(new DetailPageCode("AssemblagePrint"));
      lst.Add(new DetailPageCode("Livraison"));
      lst.Add(new DetailPageCode("Consultation"));
      return lst;
    }

    /// <summary>
    /// Indique si l'objet est correctement remplit pour se sauvegarder en bdd
    /// </summary>
    /// <returns>True si complet</returns>
    public bool IsComplet()
    {
      return !string.IsNullOrWhiteSpace(this.Nom) && this.PosteTypeInt > 0;
    }

    /// <summary>
    /// Renvoie le nom du type de poste
    /// </summary>
    /// <param name="pt">le type de poste</param>
    /// <returns>le nom</returns>
    private static string GetNomPosteType(EPosteType pt)
    {
      switch (pt)
      {
        case EPosteType.EncodeTag:
          return "Encodage et impression des étiquettes";
        case EPosteType.Lecture:
          return "Point de lectures multiple";
      }

      return string.Empty;
    }
  }
}
