using System.ComponentModel;

namespace CasqueServeur
{
  /// <summary>
  /// Classe pour l'installation du service
  /// </summary>
  [RunInstaller(true)]
  public partial class CasqueService : System.Configuration.Install.Installer
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="CasqueService"/>
    /// </summary>
    public CasqueService()
    {
      this.InitializeComponent();
    }
  }
}
