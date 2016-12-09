using System.Collections.Generic;
using System.Linq;

namespace CasqueLib.Buisness.Analyse
{
  /// <summary>
  /// Détail d'une commande
  /// Utilisé lors de la réception de pièces
  /// </summary>
  public class DetailCommande
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="DetailCommande"/>
    /// </summary>
    /// <param name="cle">La clé de la commande</param>
    /// <param name="numero">Le Numéro de commande</param>
    /// <param name="clientNom">Le nom du client</param>
    public DetailCommande(int cle, string numero, string clientNom)
    {
      this.Cle = cle;
      this.Numero = numero;
      this.FournisseurNom = clientNom;
      this.Pieces = new List<DetailCommandePiece>();
    }

    /// <summary>
    /// La clé de la commande
    /// </summary>
    public int Cle { get; private set; }

    /// <summary>
    /// Le Numéro de commande
    /// </summary>
    public string Numero { get; private set; }

    /// <summary>
    /// Le nom du client
    /// </summary>
    public string FournisseurNom { get; private set; }

    /// <summary>
    /// La liste des pièce dans la commande
    /// </summary>
    public List<DetailCommandePiece> Pieces { get;  set; }

    /// <summary>
    /// Le nombre total de tag dans la commande
    /// </summary>
    public int TotalTag
    {
      get
      {
        if (this.Pieces == null || !this.Pieces.Any())
        {
          return 0;
        }
        else
        {
          return this.Pieces.Sum(x => x.TotalTag);
        }
      }
    }

    /// <summary>
    /// Le nombre total de tag attendus dans la commande
    /// </summary>
    public int TotalAttendus
    {
      get
      {
        if (this.Pieces == null || !this.Pieces.Any())
        {
          return 0;
        }
        else
        {
          return this.Pieces.Sum(x => x.TotalAttendus);
        }
      }
    }
  }
}
