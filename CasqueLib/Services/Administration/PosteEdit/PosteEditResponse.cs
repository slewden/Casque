using System.Collections.Generic;
using CasqueLib.Buisness;
using CasqueLib.Common;
using CasqueLib.Matos.Lecteur;

namespace CasqueLib.Services.Administration.PosteEdit
{
  /// <summary>
  /// Le resultat des traitements CRUD d'un poste
  /// </summary>
  public class PosteEditResponse
  {
    /// <summary>
    /// Le poste manipulé
    /// </summary>
    public Poste Poste { get; set; }

    /// <summary>
    /// Config du lecteur
    /// </summary>
    public SimpleReaderParameters Config { get; set; }

    /// <summary>
    /// La liste des types de postes possibles
    /// </summary>
    public List<NomCleImprimante> PosteTypes { get; set; }

    /// <summary>
    /// La liste des affectations possibles
    /// </summary>
    public List<DetailPageCode> Affectations { get; set; }
  }
}
