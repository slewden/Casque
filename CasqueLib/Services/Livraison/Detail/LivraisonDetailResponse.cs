using System.Collections.Generic;
using System.Linq;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Livraison.Detail
{
  /// <summary>
  /// Le resultat du détail d'une livraison
  /// </summary>
  public class LivraisonDetailResponse
  {
    /// <summary>
    /// La livraison
    /// </summary>
    public LivraisonView Livraison { get; set; }

    /// <summary>
    /// les cartons qui constituent cette livraison
    /// </summary>
    public List<CartonLivreInfo> Cartons { get; set; }

    /// <summary>
    /// Les clients (remplis ssi la livraison est incomplète)
    /// </summary>
    public List<ClientView> Clients { get; set; }

    /// <summary>
    /// Factorise les cartons et les pièces à l'intérieur
    /// </summary>
    /// <param name="lst">Les cartons View loadé en SQL</param>
    public void FactoriseCartons(IEnumerable<CartonLivreView> lst)
    {
      this.Cartons = new List<CartonLivreInfo>();

      this.Cartons = lst.Select(x => new CartonLivreInfo()
      {
        Cle = x.Cle,
        Nom = x.Nom,
        Code = x.Code,
        Description = x.Description,
        CartonIndex = x.CartonIndex,
      }).Distinct().ToList();

      int cptCmd = 0;
      int cptCas;
      foreach (var c in this.Cartons)
      {
        c.Casques = lst.Where(x => x.Cle == c.Cle && x.CartonIndex == c.CartonIndex).Select(z => new CasqueLivreInfo()
                                      {
                                        CasqueCle = z.CasqueCle,
                                        CasqueNom = z.CasqueNom,
                                        CasqueCode = z.CasqueCode,
                                        CasquePhoto = z.CasquePhoto,
                                      }).Distinct().ToList();
        foreach (var k in c.Casques)
        {
          k.Etiquettes = new List<EtiquetteInfo>();
          cptCas = 0;
          foreach (string e in lst.Where(x => x.Cle == c.Cle && x.CartonIndex == c.CartonIndex && x.CasqueCle == k.CasqueCle).Select(a => a.Etiquette).ToList())
          {
            cptCmd++;
            cptCas++;
            k.Etiquettes.Add(new EtiquetteInfo() { Index = cptCas, IndexCommande = cptCmd, Numero = e });
          }
        }
      }
    }
  }
}
