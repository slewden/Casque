using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.WebHost.Endpoints;

namespace CasqueLib.Services
{
  /// <summary>
  /// Classe pour la compression des réponses
  /// </summary>
  /// <typeparam name="TRequest">J'en sais rien</typeparam>
  public class MyServiceRunner<TRequest> : ServiceRunner<TRequest>
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="MyServiceRunner{TRequest}" />.
    /// </summary>
    /// <param name="appHost">Host de l'appli</param>
    /// <param name="actionContext">Context action</param>
    public MyServiceRunner(IAppHost appHost, ActionContext actionContext)
      : base(appHost, actionContext)
    {
    }

    /// <summary>
    /// Action effectuée aprèes l'execution du service (compression !!)
    /// </summary>
    /// <param name="requestContext">Context request</param>
    /// <param name="response">Objet réponse</param>
    /// <returns>La réponse compressée</returns>
    public override object OnAfterExecute(IRequestContext requestContext, object response)
    {
      if ((response != null) && !(response is CompressedResult))
      {
        response = requestContext.ToOptimizedResult(response);
      }

      return base.OnAfterExecute(requestContext, response);
    }
  }
}
