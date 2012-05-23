using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.Security;

public partial class Anonymous_Register : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PasswordMismatchLabel.Visible = false;

            using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
            {
                if (EveTrusted)
                {
                    var character = context.PlexUsers.FirstOrDefault(x => x.CharacterId == CharacterId);
                    if (character != null)
                    {
                        ShowRegistryInformation(false, "Your character have already been registered.");
                        return;
                    }

                    var rules = from p in context.Rules
                                where p.Id == AllianceId || p.Id == CorpId || p.Id == CharacterId
                                select p;


                    //foreach (var rule in rules)
                    //{
                    //    if (rule.Allowed.HasValue && !rule.Allowed.Value)
                    //    {
                    //        ShowRegistryInformation(rules.Count() > 0, "You are not part of an alliance, corporation or character that are allowed to register.");
                    //        return;
                    //    }
                    //}

                    ShowRegistryInformation(rules.Count() > 0, "You are not part of an alliance, corporation or character that are allowed to register.");
                }
                else
                {
                    ShowRegistryInformation(false, "You are not using IGB trust. IGB trust is required to register. If you have trusted this site and still can't register try removing trust, close the IGB and try again. For some reason IGB trust fails sometimes for unknown reasons.");
                }
            }

            UsernameLabel.Text = CharacterName;
        }

    }

    private void ShowRegistryInformation(bool allowed, string text)
    {
        RegisterPanel.Visible = allowed;
        ErrorLabel.Text = text;
        ErrorLabel.Visible = !allowed;
    }

    protected void RegisterLinkButton_Click(object sender, EventArgs e)
    {
        PasswordMismatchLabel.Visible = false;

        if (PasswordTextBox.Text != ConfirmPasswordTextBox.Text || PasswordTextBox.Text.Trim().Length < 6)
        {
            PasswordMismatchLabel.Visible = true;
            return;
        }
        else
        {
            using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
            {
                PlexUser plexUser = context.PlexUsers.FirstOrDefault(x => x.CharacterId == CharacterId);

                if (plexUser != null)
                {
                    ShowRegistryInformation(false, "Your character have already been registered.");
                    //User already exists, dont register again
                    return;
                }
                else
                {
                    plexUser = new PlexUser();
                };

                plexUser.CharacterId = CharacterId;
                plexUser.CharacterName = CharacterName;
                plexUser.CorpId = CorpId;
                plexUser.CorpName = CorpName;
                plexUser.AllianceId = AllianceId;
                plexUser.AllianceName = AllianceName;
                plexUser.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(PasswordTextBox.Text, "md5");
                plexUser.Enabled = true;

                context.PlexUsers.InsertOnSubmit(plexUser);


                Corp corp = context.Corps.FirstOrDefault(x => x.CorpId == CorpId);

                //Check to see if the person registring belongs to a corp that have not previously been added
                if (corp == null)
                {
                    corp = new Corp()
                    {
                        CorpId = CorpId,
                        CorpName = CorpName,
                        AllianceId = AllianceId,
                        AllianceName = AllianceName,
                        Enabled = false
                    };

                    context.Corps.InsertOnSubmit(corp);
                }

                context.SubmitChanges();

                Response.Redirect(PageReferrer.Page_Login);
            }
        }
    }
}