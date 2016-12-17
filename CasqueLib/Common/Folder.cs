using System;
using System.IO;

namespace CasqueLib.Common
{
  /// <summary>
  /// Gère l'ensemble des dossiers sur disque du projet
  /// Fournit les urls relatives à la racine du site ou les chemins physiques
  /// </summary>
  public static class Folder
  {
    /// <summary>
    /// Le sous dossier principal du site ou sont mis les fichiers générés
    /// </summary>
    public const string FOLDERRACINE = @"Output\";

    /// <summary>
    /// Mémorise la racine du site
    /// (valide après un appel à la méthode EnsureExistsAll)
    /// </summary>
    private static string racineSite = string.Empty;

    /// <summary>
    /// Les dossiers possibles (le dossier Physique est un ToString de l'enum)
    /// </summary>
    public enum EFolder
    {
      /// <summary>
      /// Les photos des casques
      /// </summary>
      Casque,

      /// <summary>
      /// Les photos des types de pièces
      /// </summary>
      TypePiece,

      /// <summary>
      /// Dossier qui contient les pièces jointes des commandes
      /// </summary>
      Commande,

      /// <summary>
      /// Dossier qui contient les pièces jointes des livraisons
      /// </summary>
      Livraison,

      /// <summary>
      /// Dossier qui contient les templates des étiquettes à imprimer
      /// </summary>
      ModeleEtiquette,
    }

    /// <summary>
    /// S'assure que tous les dossiers de l'application sont opérationnels
    /// Doit être appellé une fois à l'initialisation du namespace (dans le Global.asax.cs)
    /// </summary>
    /// <param name="racine">La racine du site</param>
    public static void RegisterFolders(string racine)
    {
      Folder.racineSite = racine;
      foreach (EFolder fld in Enum.GetValues(typeof(EFolder)))
      { // on parcours tous les dossiers et on les vérifie
        Folder.EnsureExists(fld);
      }
    }

    /// <summary>
    /// Renvoie l'Url relative à la racine du site du fichier dans le folder demandé
    /// </summary>
    /// <param name="fld">Le folder</param>
    /// <param name="fileName">le nom du fichier</param>
    /// <returns>L'url relative du fichier</returns>
    public static string RelativeUrl(EFolder fld, string fileName)
    {
      if (!string.IsNullOrWhiteSpace(fileName))
      {
        string fl = Path.Combine(Folder.FOLDERRACINE, fld.ToString(), fileName);
        return "/" + fl.Replace("\\", "/");
      }
      else
      {
        return string.Empty;
      }
    }

    /// <summary>
    /// Renvoie le chemin physique complet du fichier dans le folder demandé
    /// </summary>
    /// <param name="fld">Le folder</param>
    /// <param name="fileName">le nom du fichier</param>
    /// <returns>Le chemin physique complet du fichier</returns>
    public static string FullPath(EFolder fld, string fileName)
    {
      if (!string.IsNullOrWhiteSpace(fileName))
      {
        return Path.Combine(Folder.FullPath(fld), fileName);
      }
      else
      {
        return string.Empty;
      }
    }

    /// <summary>
    /// Renvoie le chemin physique complet du fichier dans le folder demandé
    /// </summary>
    /// <param name="fileUrl">l'url du fichier</param>
    /// <returns>Le chemin physique complet du fichier</returns>
    public static string FullPath(string fileUrl)
    {
      if (string.IsNullOrWhiteSpace(Folder.racineSite))
      { // séquence d'ordonancement des méthodes pas bonne
        throw new NotImplementedException("Vous devez appeller la méthode Folder.EnsureExistsAll(racine) avant tout appel a cette méthode");
      }

      string file = fileUrl;
      if (file.StartsWith("/") || file.StartsWith("\\"))
      {
        file = file.Substring(1);
      }

      return Path.Combine(Folder.racineSite, file.Replace("/", "\\"));
    }

    #region Méthodes Privée
    /// <summary>
    /// Renvoie le chemin physique complet du folder demandé
    /// </summary>
    /// <param name="fld">Le folder</param>
    /// <returns>le chemin complet</returns>
    private static string FullPath(EFolder fld)
    {
      if (string.IsNullOrWhiteSpace(Folder.racineSite))
      { // séquence d'ordonancement des méthodes pas bonne
        throw new NotImplementedException("Vous devez appeller la méthode Folder.EnsureExistsAll(racine) avant tout appel a cette méthode");
      }

      return Path.Combine(Folder.racineSite, Folder.FOLDERRACINE, fld.ToString() + @"\");
    }

    /// <summary>
    /// S'assure que le folder existe
    /// </summary>
    /// <param name="fld">Le dossier à checker</param>
    private static void EnsureExists(EFolder fld)
    {
      string folder = Folder.FullPath(fld);

      switch (fld)
      {
        case EFolder.Commande:
        case EFolder.Livraison:
          try
          {
            Directory.Delete(folder, true);
          }
          catch
          { //// (Exception ex)
            ////using (FConnexion cnn = new FConnexion())
            ////{
            ////  Log.Save(cnn.Db, string.Format("EnsureExists('{0}')", folder), ex.ToString());
            ////}
          }

          break;
      }

      if (!Directory.Exists(folder))
      {
        try
        {
          Directory.CreateDirectory(folder);
        }
        catch
        { //// (Exception ex)
          ////using (FConnexion cnn = new FConnexion())
          ////{
          ////  Log.Save(cnn.Db, string.Format("EnsureExists('{0}')", folder), ex.ToString());
          ////}
        }
      }
    }
    #endregion
  }
}
