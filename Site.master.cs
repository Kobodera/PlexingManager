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
        string browser = Request.ServerVariables["HTTP_USER_AGENT"];

        //Fix to make IE work. If you put this fix directly on the menu then the menu will not work in the IGB.
        //This fix seems to be working however. It will only add the fix if it can detect that it is a IE client
        //connecting to the plexing manager.
        if (browser.Contains("MSIE"))
        {
            MainMenu.DynamicMenuStyle.CssClass = "IE8Fix";
        }


        bool isSuper = Page.User.IsInRole("Super");
        bool isAdmin = Page.User.IsInRole("Admin") || Page.User.IsInRole("Super");
        bool isAlliance = Page.User.IsInRole("Alliance") || isSuper || isAdmin;
        bool isCorp = Page.User.IsInRole("Corporation") || isSuper || isAdmin || isAlliance;
        bool isFC = Page.User.IsInRole("FleetCommander") || isSuper || isAdmin || isAlliance || isCorp;

        //PayoutLinkButton.Visible = isAdmin;
        //UsersLinkButton.Visible = isAdmin;
        //EfficiencyLinkButton.Visible = isAdmin;

        BuildMenu(isAdmin, isSuper, isFC, isAlliance, isCorp);
    }

    private void BuildMenu(bool isAdmin, bool isSuper, bool isFC, bool isAlliance, bool isCorporation)
    {
        MenuItem fleetCommand = new MenuItem("Fleet commander");
        fleetCommand.NavigateUrl = PageReferrer.Page_FC_Fleet;
        MainMenu.Items.Add(fleetCommand);

        MenuItem plexingPeriod = new MenuItem("Plexing period");
        plexingPeriod.NavigateUrl = PageReferrer.Page_PlexingPeriod_PlexingPeriod;
        MainMenu.Items.Add(plexingPeriod);

        MenuItem admin = new MenuItem("Admin");
        MenuItem corporation = new MenuItem("Corporation");
        MenuItem alliance = new MenuItem("Alliance");

        if (isCorporation)
            MainMenu.Items.Add(corporation);

        if (isAlliance)
            MainMenu.Items.Add(alliance);

        if (isAdmin)
        {
            MainMenu.Items.Add(admin);
        }

        if (isCorporation)
        {
            MenuItem payout = new MenuItem("Payout");
            payout.NavigateUrl = PageReferrer.Page_Admin_Payout;
            corporation.ChildItems.Add(payout);

            MenuItem users = new MenuItem("Users");
            users.NavigateUrl = string.Format("{0}?Corp=1", PageReferrer.Page_Admin_Users);
            corporation.ChildItems.Add(users);

            MenuItem efficiency = new MenuItem("Efficiency");
            efficiency.NavigateUrl = string.Format("{0}?Corp=1", PageReferrer.Page_PlexingPeriod_Efficiency);
            corporation.ChildItems.Add(efficiency);
        }

        if (isAlliance)
        {
            MenuItem users = new MenuItem("Users");
            users.NavigateUrl = string.Format("{0}?Alliance=1", PageReferrer.Page_Admin_Users);
            alliance.ChildItems.Add(users);

            MenuItem efficiency = new MenuItem("Efficiency");
            efficiency.NavigateUrl = string.Format("{0}?Alliance=1", PageReferrer.Page_PlexingPeriod_Efficiency);
            alliance.ChildItems.Add(efficiency);
        }

        if (isAdmin)
        {
            MenuItem users = new MenuItem("Users");
            users.NavigateUrl = string.Format("{0}?Admin=1", PageReferrer.Page_Admin_Users);
            admin.ChildItems.Add(users);

            MenuItem efficiency = new MenuItem("Efficiency");
            efficiency.NavigateUrl = string.Format("{0}?Admin=1", PageReferrer.Page_PlexingPeriod_Efficiency);
            admin.ChildItems.Add(efficiency);
        }
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
