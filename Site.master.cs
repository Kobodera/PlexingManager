using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ShowMenu();
    }

    private void ShowMenu()
    {
        bool isAdmin = Page.User.IsInRole("Admin") || Page.User.IsInRole("Super");

        PayoutLinkButton.Visible = isAdmin;
        UsersLinkButton.Visible = isAdmin;
        EfficiencyLinkButton.Visible = isAdmin;
            
    }

    protected void PlexingPeriodLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(PageReferrer.Page_PlexingPeriod_PlexingPeriod);
    }
    protected void FcLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(PageReferrer.Page_FC_Fleet);
    }
    protected void HeadLoginStatus_LoggingOut(object sender, LoginCancelEventArgs e)
    {
        FormsAuthentication.SignOut();
    }
    protected void ChangePasswordLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(PageReferrer.Page_User_ChangePassword);
    }
    protected void PayoutLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(PageReferrer.Page_Admin_Payout);
    }
    protected void UsersLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(PageReferrer.Page_Admin_Users);
    }

    protected void EfficiencyLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(PageReferrer.Page_PlexingPeriod_Efficiency);
    }
}
