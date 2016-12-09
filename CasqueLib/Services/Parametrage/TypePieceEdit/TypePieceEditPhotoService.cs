using System;
using System.IO;
using System.Linq;
using CasqueLib.Common;

namespace CasqueLib.Services.Parametrage.TypePieceEdit
{
  /// <summary>
  /// Service pour l'upload des photos
  /// </summary>
  public class TypePieceEditPhotoService : FsService
  {
    /// <summary>
    /// UpLoad la photo et renvoie son nom
    /// </summary>
    /// <param name="request">La demande</param>
    /// <returns>La réponse</returns>
    public object Post(TypePieceEditPhotoRequest request)
    {
      // Upload de la photo si elle est la
      string fileName = string.Empty;
      var uploadedFile = Request.Files.Where(x => x.ContentLength > 0).FirstOrDefault();
      if (uploadedFile != null)
      {
        fileName = this.WriteFile(uploadedFile.InputStream);
      }

      return new TypePieceEditPhotoResponse()
      {
        PhotoNom = fileName
      };
    }

    /// <summary>
    /// Supprime un fichier photo inutile
    /// </summary>
    /// <param name="fileName">Le nom du fichier à supprimer</param>
    internal static void DeletePhoto(string fileName)
    {
      string fullFileName = Folder.FullPath(Folder.EFolder.TypePiece, fileName);
      if (File.Exists(fullFileName))
      {
        try
        {
          File.Delete(fullFileName);
        }
        catch
        {
        }
      }
    }

    /// <summary>
    /// Attibue un nom à la photo et la sauve sur dique
    /// </summary>
    /// <param name="ms">le flux à sauver</param>
    /// <returns>le nom attribué sans le chemin</returns>
    private string WriteFile(Stream ms)
    {
      string fileName = string.Format("Photo-{0}.png", Guid.NewGuid());
      string fullFileName = Folder.FullPath(Folder.EFolder.TypePiece, fileName);
      using (var fich = File.Create(fullFileName))
      {
        ms.Position = 0;
        ms.CopyTo(fich);
      }

      return fileName;
    }
  }
}
