using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.Security;

public partial class Anonymous_Login : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoginButton.Style.Add(HtmlTextWriterStyle.Visibility, "hidden");

            if (EveTrusted)
            {
                using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
                {
                    var user = context.PlexUsers.FirstOrDefault(x => x.CharacterId == CharacterId);

                    if (user != null)
                    {
                        UsernameTextBox.Text = CharacterName;

                        if (user.AllianceId != AllianceId || user.CorpId != CorpId)
                        {
                            //User have switched alliance and/or corp check if new alliance/corp is allowed

                            throw new NotImplementedException("User have switched corp and/or alliance since you registered.");
                        }

                        ShowLogin(true);
                        return;
                    }
                    else
                    {
                        Response.Redirect(PageReferrer.Page_Register);
                    }
                }
            }
            else
            {
                RememberMeCheckBox.Visible = false;
                RememberMeCheckBox.Checked = false;
                //ShowLogin(false);
            }
        }

    }
 
    private void ShowLogin(bool allowed)
    {
        LoginPanel.Visible = allowed;
        ErrorLabel.Visible = !allowed;
    }

    protected void LoginLinkButton_Click(object sender, EventArgs e)
    {
        string roles;
        if (AuthenticateUser(UsernameTextBox.Text, PasswordTextBox.Text, out roles))
        {
            Login(roles);
        }
    }
    

    private void Login(string roles)
    {
        FormsAuthenticationTicket tkt;
        string cookiestr;
        HttpCookie ck;
        tkt = new FormsAuthenticationTicket(1, CharacterName, DateTime.Now,
            DateTime.Now.AddMinutes(240), RememberMeCheckBox.Checked, roles);
        cookiestr = FormsAuthentication.Encrypt(tkt);
        ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
        if (RememberMeCheckBox.Checked)
            ck.Expires = tkt.Expiration;
        ck.Path = FormsAuthentication.FormsCookiePath;
        Response.Cookies.Add(ck);

        string strRedirect;
        strRedirect = Request["ReturnUrl"];
        if (strRedirect == null)
            strRedirect = PageReferrer.Page_Default;
        Response.Redirect(strRedirect, true);
    }

    private bool AuthenticateUser(string username, string password, out string roles)
    {
        ErrorLabel.Visible = false;
        PasswordLabel.Visible = false;

        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            var user = context.PlexUsers.FirstOrDefault(x => x.CharacterName == username);

            if (user == null)
            {
                ErrorLabel.Visible = true;
                roles = "";
                return false;
            }

            if (user.Password == FormsAuthentication.HashPasswordForStoringInConfigFile(password, "md5"))
            {
                if (user.Enabled)
                {
                    CharacterId = user.CharacterId;
                    CharacterName = user.CharacterName;
                    CorpId = user.CorpId;
                    CorpName = user.CorpName;
                    AllianceId = user.AllianceId;
                    AllianceName = user.AllianceName;
                    SolarSystemName = "Uknown";

                    var userRoles = context.PlexUserRoles.FirstOrDefault(x => x.CharacterId == CharacterId);

                    if (userRoles != null)
                        roles = userRoles.Roles;
                    else
                        roles = "";

                    return true;
                }
                else
                {
                    ErrorLabel.Visible = true;
                    roles = "";
                    return false;
                }
            }
        }

        PasswordLabel.Visible = true;

        roles = "";

        return false;
    }
    protected void LoginButton_Click(object sender, EventArgs e)
    {
        string roles;
        if (AuthenticateUser(UsernameTextBox.Text, PasswordTextBox.Text, out roles))
        {
            Login(roles);
        }
    }
}
