using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Anonymous_AccessRequest : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RequestPanel.Visible = false;
        InfoPanel.Visible = true;
        if (!IsPostBack)
        {
            if (EveTrusted)
            {
                RequestPanel.Visible = true;
                InfoPanel.Visible = false;

                LoadData();
            }
        }
    }

    private void LoadData()
    {
        RequestedByLabel.Text = CharacterName;
        CorpNameLabel.Text = CorpName;
        AllianceNameLabel.Text = AllianceId != -1 ? AllianceName : string.Empty;

        if (AllianceId != -1)
        {
            using (PlexingFleetDataContext context = new PlexingFleetDataContext(ConnectionString))
            {
                var user = context.PlexUsers.FirstOrDefault(x => x.CharacterId == CharacterId);
                if (user != null)
                {
                    if (!user.Enabled)
                    {
                        InfoPanel.Visible = true;
                        RequestPanel.Visible = false;
                    }

                    PasswordRow.Visible = false;
                    ConfirmPasswordRow.Visible = false;
                }

                var alliance = context.Corps.First(x => x.AllianceId == AllianceId);

                if (alliance != null)
                {
                    AllianceTickerTextBox.Text = alliance.AllianceTag;
                }
            }
        }
    }

    protected void RequestLinkButton_Click(object sender, EventArgs e)
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(ConnectionString))
        {
            PasswordMismatchLabel.Visible = false;
            var user = context.PlexUsers.FirstOrDefault(x => x.CharacterId == CharacterId);

            if (user == null)
            {
                if (PasswordTextBox.Text != ConfirmTextBox.Text || PasswordTextBox.Text.Trim().Length < 6)
                {
                    PasswordMismatchLabel.Visible = true;
                    return;
                }
            }

            var corp = context.Corps.FirstOrDefault(x => x.CorpId == CorpId);

            if (corp != null)
            {
                CreateUser(context, false);
            }
            else
            {
                //New corp
                if (CreateCorp(context))
                    CreateUser(context, true);
            }
        }
    }

    private bool CreateCorp(PlexingFleetDataContext context)
    {
        Rule rule = context.Rules.FirstOrDefault(x => x.Id.Value == CorpId);

        if (rule == null)
        {
            rule = new Rule()
            {
                Id = CorpId,
                RuleName = CorpName,
                Allowed = true,
            };

            context.Rules.InsertOnSubmit(rule);
        }
        else
        {
            if (rule.Allowed.HasValue && !rule.Allowed.Value)
                return false;
        }

        Corp corp = new Corp()
        {
            CorpId = this.CorpId,
            CorpName = this.CorpName,
            CorpTag = this.CorpTickerTextBox.Text,
            AllianceId = this.AllianceId == 0 ? -1 : this.AllianceId,
            AllianceName = this.AllianceName,
            AllianceTag = this.AllianceTickerTextBox.Text,
            Enabled = true
        };

        context.Corps.InsertOnSubmit(corp);

        context.SubmitChanges();

        return true;
    }

    private void CreateUser(PlexingFleetDataContext context, bool newCorp)
    {
        PlexUser user = context.PlexUsers.FirstOrDefault(x => x.CharacterId == CharacterId);

        if (user == null)
        {
            user = new PlexUser()
            {
                CharacterId = this.CharacterId,
                CharacterName = this.CharacterName,
                AllianceId = this.AllianceId,
                CorpId = this.CorpId,
                AllianceName = this.AllianceName,
                CorpName = this.CorpName,
                Password = FormsAuthentication.HashPasswordForStoringInConfigFile(PasswordTextBox.Text.Trim(), "md5"),
                Enabled = true
            };

            context.PlexUsers.InsertOnSubmit(user);

            if (newCorp)
            {
                PlexUserRole role = new PlexUserRole()
                {
                    CharacterId = this.CharacterId,
                    Roles = "Corporation"
                };

                context.PlexUserRoles.InsertOnSubmit(role);
            }
        }
        else
        {
            //Only do this if the user enabled.
            if (user.Enabled)
            {
                user.AllianceId = this.AllianceId;
                user.CorpId = this.CorpId;
                user.AllianceName = this.AllianceName;
                user.CorpName = this.CorpName;

                if (newCorp)
                {
                    var userRoles = context.PlexUserRoles.FirstOrDefault(x => x.CharacterId == CharacterId);

                    //If no roles were found then add the one registring the corp admin rights for that corp.
                    if (userRoles == null)
                    {
                        PlexUserRole role = new PlexUserRole();
                        role.CharacterId = CharacterId;
                        role.Roles = "Corporation";
                        context.PlexUserRoles.InsertOnSubmit(role);
                    }
                    else
                    {
                        //Only add the corporation role if it is missing. 
                        if (!userRoles.Roles.Contains("Corporation"))
                        {
                            userRoles.Roles += ",Corporation";
                        }
                    }
                }
            }
            else
            {
                user.AllianceId = this.AllianceId;
                user.CorpId = this.CorpId;
                user.AllianceName = this.AllianceName;
                user.CorpName = this.CorpName;
            }
        }

        context.SubmitChanges();
    }

}