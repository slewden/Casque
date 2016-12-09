using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasqueLib
{
  /// <summary>
  /// Types d'accès aux pages
  /// </summary>
  public enum ETypeAccess
  {
    /// <summary>
    /// Aucun droit
    /// </summary>
    Aucun = 1,

    /// <summary>
    /// Droit de consulter
    /// </summary>
    Voir = 2,

    /// <summary>
    /// Droit de consulter et modifier
    /// </summary>
    Editer = 3,
  }

  /// <summary>
  /// Le configuration générales 
  /// </summary>
  public enum EConfiguration
  {
    /// <summary>
    /// Nombre limite de quantité pour une ligne commandée
    /// </summary>
    NombreMaxiQuantite = 1,
  }
}
