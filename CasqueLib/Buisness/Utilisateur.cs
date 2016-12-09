using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace CasqueLib.Buisness
{
  /// <summary>
  /// Mappe un utilisateur
  /// </summary>
  [Alias("utilisateur")]
  public class Utilisateur
  {
    /// <summary>
    /// Temps de session = 30 minutes
    /// </summary>
    private const int DelaiSessionMinute = 30;

    /// <summary>
    /// Les utilisateurs logé
    /// </summary>
    private static List<Utilisateur> lesUsers = null;

    /// <summary>
    /// Le guid d'authentification
    /// </summary>
    private Guid apiKey;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="Utilisateur"/>
    /// </summary>
    public Utilisateur()
    {
      this.DateLimiteSession = DateTime.Now.AddMinutes(Utilisateur.DelaiSessionMinute);
    }

    #region Properties
    /// <summary>
    /// La clé de l'utilisateur
    /// </summary>
    [AutoIncrement]
    [Alias("util_id")]
    public int Cle { get; set; }

    /// <summary>
    /// Le prénom et le nom
    /// </summary>
    [Alias("util_prenom_nom")]
    public string Nom { get; set; }

    /// <summary>
    /// Le login
    /// </summary>
    [Alias("util_login")]
    public string Login { get; set; }

    /// <summary>
    /// Le mot de passe
    /// </summary>
    [Alias("util_password")]
    public string Password { get; set; }

    /// <summary>
    /// L'utilisateur est actif ou pas
    /// </summary>
    [Alias("util_actif")]
    public bool Actif { get; set; }

    /// <summary>
    /// L'adresse email de l'utilisateur
    /// </summary>
    [Alias("util_email")]
    public string Email { get; set; }

    /// <summary>
    /// La clé d'authentification
    /// </summary>
    [Ignore]
    public Guid ApiKey
    {
      get
      {
        if (this.apiKey == Guid.Empty)
        {
          this.apiKey = Guid.NewGuid();
        }

        return this.apiKey;
      }
    }

    /// <summary>
    /// Date limite de session
    /// </summary>
    [Ignore]
    public DateTime DateLimiteSession { get; private set; }
    #endregion

    /// <summary>
    /// Ajoute un user à l'application
    /// </summary>
    /// <param name="user">l'utilisateur préremplit</param>
    /// <returns>L'utilisateur ajouté ou null si déjà présent</returns>
    public static Utilisateur Add(Utilisateur user)
    {
      if (Utilisateur.lesUsers == null)
      {
        Utilisateur.lesUsers = new List<Utilisateur>();
      }

      var u = Utilisateur.lesUsers.Where(x => x.Login == user.Login && x.Password == user.Password);
      if (!u.Any())
      {
        Utilisateur.lesUsers.Add(user);
        return user;
      }
      else
      { // indique qu'on a rien ajouté
        return u.FirstOrDefault();
      }
    }

    /// <summary>
    /// Retire un user de l'application
    /// </summary>
    /// <param name="user">L'utilisateur à retirer</param>
    public static void Remove(Utilisateur user)
    {
      if (Utilisateur.lesUsers == null || !Utilisateur.lesUsers.Any())
      { // on a rien à faire c'est vide
        return;
      }

      Utilisateur.lesUsers.Remove(user);
    }

    /// <summary>
    /// Renvoie l'utilisateur authentifié
    /// </summary>
    /// <param name="db">La connexion à la base de données</param>
    /// <param name="login">le login</param>
    /// <param name="pass">Le password</param>
    /// <returns>L'utlisateur ou null</returns>
    public static Utilisateur Get(IDbConnection db, string login, string pass)
    {
      Utilisateur ut = db.Select<Utilisateur>(x => x.Login == login && x.Password == pass && x.Actif).FirstOrDefault();
      if (ut != null)
      { // utilisateur trouvé : on repousse sa date de fin de session
        ut.DateLimiteSession = DateTime.Now.AddMinutes(Utilisateur.DelaiSessionMinute);
      }

      return ut;
    }

    /// <summary>
    /// Authentifie si le guidTxt est un utilisateur logé
    /// </summary>
    /// <param name="guidtxt">Le guid en texte</param>
    /// <returns>L'utlisateur s'il est trouvé dans la liste des users authentifiés</returns>
    public static Utilisateur IsAuthentified(string guidtxt)
    {
      if (string.IsNullOrWhiteSpace(guidtxt) || Utilisateur.lesUsers == null || !Utilisateur.lesUsers.Any())
      { // pas d'info valide ou pas de user logé !
        return null;
      }

      Guid g;
      if (!Guid.TryParse(guidtxt, out g))
      {
        return null;
      }

      Utilisateur ut = Utilisateur.lesUsers.Where(x => x.ApiKey == g).FirstOrDefault();
      if (ut == null)
      { // Not found
        return null;
      }
      else if (ut.DateLimiteSession < DateTime.Now)
      { // session expirée : log off
        Utilisateur.Remove(ut);
        return null;
      }
      else
      { // utilisateur trouvé : on repousse sa date de fin de session
        ut.DateLimiteSession = DateTime.Now.AddMinutes(Utilisateur.DelaiSessionMinute);
        return ut;
      }
    }

    /// <summary>
    /// Indique si l'objet est correctement remplit pour se sauvegarder en bdd
    /// </summary>
    /// <returns>True si complet</returns>
    public bool IsComplet()
    {
      return !string.IsNullOrWhiteSpace(this.Nom) && !string.IsNullOrWhiteSpace(this.Login) && !string.IsNullOrWhiteSpace(this.Password);
    }
  }
}
