using System;
using System.Web.UI;

//namespace CodeLearningAcademy.App_Code
//{
//    public class BasePage
//    {
//    }
//}


public class  BasedPage : Page
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
}