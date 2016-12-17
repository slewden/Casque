using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Lectures
{
  /// <summary>
  /// Classe réponse pour le web service de détail des pièces d'un assemblage
  /// </summary>
  public class AssemblageReportInfoResponse
  {
    /// <summary>
    /// La listes des pièces d'un assemblage
    /// </summary>
    public List<EtiquetteReportInfo> Pieces { get; set; }
  }
}
