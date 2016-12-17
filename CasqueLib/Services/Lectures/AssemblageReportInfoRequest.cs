using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Lectures
{
  /// <summary>
  /// Classe requête pour le détail des pièces d'un assemblage
  /// </summary>
  [Api("Casque")]
  [Route("/analyseAssemblage/{ApiKey}", Verbs = "GET")]
  public class AssemblageReportInfoRequest : RequestBase, IReturn<AssemblageReportInfoResponse>
  {
    /// <summary>
    /// La clé de l'assemblage pour lequel on veut un détail
    /// </summary>
    public int Cle { get; set; }
  }
}
