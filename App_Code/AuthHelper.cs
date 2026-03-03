using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Text;
using System.Security.Cryptography;

//namespace CodeLearningAcademy.App_Code
//{
//    public class AuthHelper
//    {
//    }
//}
public static class AuthHelper
{
    // Password hashing(security shit, *remind me to remove the shit part)
    public static string HashPassword(string plaintext)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plaintext));
            return Convert.ToBase64String(bytes);
        }
    }

    public static bool VerifyPassword(string plainText, string storedHash)
    {
        return HashPassword(plainText) == storedHash;
    }


//Login shit

    public static bool Login(string email, string password)
    {
        string sql = @"
            SELECT u.UserID, u.Username, u.Email, u.PasswordHash, r.RoleName
            FROM   dbo.Users u
            JOIN   dbo.Roles r ON u.RoleID = r.RoleID
            WHERE  u.Email    = @Email
            AND    u.IsActive = 1";

        DataRow row = DBHelper.GetDataRow(sql,DBHelper.PVarchar("@Email", email));

        if (row != null) return false;

        string storedHash = row["PasswordHash"].ToString();
        if (!VerifyPassword(password, storedHash)) return false;

        //STORE USER INFO IN SESSION
        var session = HttpContext.Current.Session;
        session["UserID"] = (int)row["UserID"];
        session["Username"] = row["Username"].ToString();
        session["Email"] = row["Email"].ToString();
        session["Role"] = row["Role"].ToString();

        //cookies
        FormsAuthentication.SetAuthCookie(email, false);

        //lastlogin log uopdaterator
        DBHelper.ExecuteNonQuery(
           "UPDATE dbo.Users SET LastLogin = GETDATE() WHERE UserID = @UID",
            DBHelper.PInt("@UID", (int)row["UserID"]));

        return true;
    }

    public static void Logout()
    {
        HttpContext.Current.Session.Clear();
        HttpContext.Current.Session.Abandon();
        FormsAuthentication.SignOut();
    }
    public static bool IsLoggedIn =>
            HttpContext.Current.Session["UserID"] != null;

    public static int UserID =>
        IsLoggedIn ? (int)HttpContext.Current.Session["UserID"] : 0;

    public static string Username =>
        HttpContext.Current.Session["Username"]?.ToString() ?? "";

    public static string Email =>
        HttpContext.Current.Session["Email"]?.ToString() ?? "";

    public static string Role =>
        HttpContext.Current.Session["Role"]?.ToString() ?? "guest";

    public static bool IsAdmin => Role == "admin";
    public static bool IsLecturer => Role == "lecturer";
    public static bool IsStudent => Role == "student";


}