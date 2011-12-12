using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PageReferrer
/// </summary>
public class PageReferrer
{
    //public PageReferrer()
    //{
    //}

    public static string Page_Default = "~/Default.aspx";

    public static string Page_Login = "~/Anonymous/Login.aspx";
    public static string Page_Register = "~/Anonymous/Register.aspx";

    public static string Page_Admin_Payout = "~/Authenticated/Admin/Payout.aspx";
    public static string Page_Admin_PlexInfo = "~/Authenticated/Admin/PlexInfo.aspx";
    public static string Page_Admin_Users = "~/Authenticated/Admin/Users.aspx";
    public static string Page_Admin_EditUser = "~/Authenticated/Admin/EditUser.aspx";

    public static string Page_FC_Fleet = "~/Authenticated/FC/Fleet.aspx";
    public static string Page_FC_SelectMembers = "~/Authenticated/FC/SelectMembers.aspx";

    public static string Page_PlexingPeriod_PlexingPeriod = "~/Authenticated/PlexingPeriod/PlexingPeriod.aspx";

    public static string Page_User_ChangePassword = "~/Authenticated/User/ChangePassword.aspx";
    public static string Page_User_PlexingPeriod = "~/Authenticated/User/PlexingPeriod.aspx";
}