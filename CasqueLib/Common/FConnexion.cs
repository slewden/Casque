using System;
using System.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;

namespace CasqueLib.Common
{
  /// <summary>
  /// Fourni une connexion à la bdd
  /// </summary>
  public class FConnexion : IDisposable
  {
    /// <summary>
    /// La connexion
    /// </summary>
    private IDbConnection cnn = null;

    /// <summary>
    /// La connexion
    /// </summary>
    public IDbConnection Db
    {
      get
      {
        if (this.cnn == null)
        {
          var baseFactory = new OrmLiteConnectionFactory(System.Configuration.ConfigurationManager.ConnectionStrings["casque"].ToString(), SqlServerOrmLiteDialectProvider.Instance);
          this.cnn = baseFactory.Open();
          this.cnn.Open();
        }

        if (this.cnn.State == ConnectionState.Closed)
        {
          this.cnn.Open();
        }

        return this.cnn;
      }

      set
      {
        this.cnn = value;
      }
    }

    /// <summary>
    /// Dispo de la connexion
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Execute une requette non query
    /// </summary>
    /// <param name="cmd">le sql</param>
    /// <returns>le resultat</returns>
    public int ExecuteNonQuery(string cmd)
    {
      return this.Db.ExecuteNonQuery(cmd);
    }

    /// <summary>
    /// Dispose de la connexion
    /// </summary>
    /// <param name="act">action de dispose</param>
    protected virtual void Dispose(bool act)
    {
      if (this.cnn != null)
      {
        this.cnn.Dispose();
      }
    }
  }
}
