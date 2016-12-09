using System;
using System.ServiceProcess;

namespace CasqueServeur
{
  /// <summary>
  /// Classe de démarrage du service
  /// </summary>
  public static class Program
  {
    /// <summary>
    /// Point d'entrée principal de l'application.
    /// </summary>
    /// <param name="args">Arguments de la ligne de commande</param>
    public static void Main(string[] args)
    {
      if (Environment.UserInteractive)
      {
        CasqueServeur service1 = new CasqueServeur();
        service1.TestStartupAndStop(args);
      }
      else
      {
        ServiceBase[] servicesToRun = new ServiceBase[] 
            { 
                new CasqueServeur() 
            };
        ServiceBase.Run(servicesToRun);
      }
    }
  }
}
