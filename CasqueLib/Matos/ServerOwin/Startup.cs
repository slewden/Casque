using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;

namespace CasqueLib.Matos.ServerOwin
{
  /// <summary>
  /// Classe de démarrage du serveur OWIN
  /// </summary>
  public class Startup
  {
    /// <summary>
    /// Configure le serveur OWIN
    /// </summary>
    /// <param name="app">L'application à configurer</param>
    public void Configuration(IAppBuilder app)
    {
      app.UseCors(CorsOptions.AllowAll); // cross domain

#if DEBUG
      // Active les message d'erreur détaillés
      var hubConfiguration = new HubConfiguration();
      hubConfiguration.EnableDetailedErrors = true;
#endif

      app.MapSignalR();
    }
  }
}