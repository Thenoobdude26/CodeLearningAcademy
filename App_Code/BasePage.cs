using System;
using System.Web.UI;

//namespace CodeLearningAcademy.App_Code
//{
//    public class BasePage
//    {
//    }
//}


public class BasedPage : Page
{
    protected bool IsLoggedIn => AuthHelper.IsLoggedIn;
    protected int CurrentUserId => AuthHelper.UserID;
    protected string CurrentUserName => AuthHelper.Username;
    protected string CurrentRole => AuthHelper.Role;
    protected bool IsAdmin => AuthHelper.IsAdmin;
    protected bool IsLecturer => AuthHelper.IsLecturer;
    protected bool IsStudent => AuthHelper.IsStudent;

    //redircects login if not loggged in

    protected void RequireLogin()
    {
        if (!IsLoggedIn)
            Response.Redirect("~/Login.aspx", true);
    }

    //redirects to home if not admin
    protected void RequireAdmin()
    {
        RequireLogin();
        if (!IsAdmin)
            Response.Redirect("~/Default.aspx", true);
    }
    //Redircers to Home if not a lecturer
    protected void RequiredLecturer()
    {
        if (!IsLecturer && !IsAdmin)
            ResponseRedirect("~/Default.aspx", true);
    }

    //Reduirects to home is not a strudent
    protected void RequireStudent()
    {
        RequireLogin();
        if (!IsStudent)
            response.Redirect("~/Default.aspx", true);
    }
     protected string QS(string key)
        => Request.QueryString[key] ?? "";

    /// <summary>Parses an int query string, returns 0 if missing/invalid.</summary>
    protected int QSInt(string key)
    {
        int.TryParse(Request.QueryString[key], out int val);
        return val;
    }
}