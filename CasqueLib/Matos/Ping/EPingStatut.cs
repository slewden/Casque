namespace CasqueLib.Matos.Ping
{
  /// <summary>
  /// Types d'actions pour le ping
  /// </summary>
  public enum EPingStatut
  {
    /// <summary>
    /// Attente d'instuction
    /// </summary>
    Waiting = 0,

    /// <summary>
    /// Demande en cours
    /// </summary>
    Requesting = 1,

    /// <summary>
    /// Reponse reçue
    /// </summary>
    ReceiveOk = 2,

    /// <summary>
    /// Time Out
    /// </summary>
    ErreurTimeOut = 3
  }
}
