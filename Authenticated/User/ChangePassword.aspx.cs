using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Authenticated_User_ChangePassword : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ChangePasswordLinkButton_Click(object sender, EventArgs e)
    {
        ErrorLabel.Visible = false;
        SuccessLabel.Visible = false;
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(ConnectionString))
        {
            var user = context.PlexUsers.FirstOrDefault(x => x.CharacterId == CharacterId);

            if (user != null)
            {
                if (FormsAuthentication.HashPasswordForStoringInConfigFile(OldPasswordTextBox.Text, "md5") == user.Password)
                {
                    if (NewPasswordTextBox.Text != ConfirmPasswordTextBox.Text || NewPasswordTextBox.Text.Length < 6)
                    {
                        ErrorLabel.Visible = true;
                        return;
                    }

                    user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(NewPasswordTextBox.Text, "md5");

                    context.SubmitChanges();
                    SuccessLabel.Visible = true;

                    return;
                }
                else
                {
                    ErrorLabel.Visible = true;
                }
            }
        }
    }
}