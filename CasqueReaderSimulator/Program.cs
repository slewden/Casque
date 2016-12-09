using System;
using System.Windows.Forms;

namespace CasqueReaderSimulator
{
  /// <summary>
  /// Classe de démarrage de l'application
  /// </summary>
  public static class Program
  {
    /// <summary>
    /// Point d'entrée principal de l'application.
    /// </summary>
    [STAThread]
    public static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new FSimulateur());
    }
  }
}
