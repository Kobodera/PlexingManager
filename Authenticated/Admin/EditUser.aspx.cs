using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Authenticated_Admin_EditUser : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["CharacterId"]))
            {
                LoadCharacterInfo(Request.QueryString["CharacterId"].ToInt());
            }
        }
    }

    private void LoadCharacterInfo(int characterId)
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(ConnectionString))
        {
            var user = context.PlexUsers.FirstOrDefault(x => x.CharacterId == characterId);

            if (user != null)
            {
                CharacterIdLabel.Text = user.CharacterId.ToString();
                CharacterNameLabel.Text = user.CharacterName;
                CorporationLabel.Text = user.CorpName;
                AllianceLabel.Text = user.AllianceName;
                BannedCheckBox.Checked = !user.Enabled;
            }

            var userPermissions = from up in context.PlexUserRoles
                                  where up.CharacterId == characterId
                                  select up;

            foreach (var permission in userPermissions)
            {
                AdministratorCheckBox.Checked = permission.Roles.ToLower().Contains("admin");
            }

            var plexInfos = from p in context.Plexes
                           where p.Participants.Contains(user.CharacterName)
                           orderby p.PlexingDate descending
                           select p;

            PlexesLabel.Text = plexInfos.Count().ToString();

            if (plexInfos.Count() > 0)
            {
                LastPlexLabel.Text = plexInfos.First().PlexingDate.Value.ToShortDateString();
            }

            int occations = 0;
            string currentDate = string.Empty;

            foreach (var plexInfo in plexInfos)
            {
                string temp = plexInfo.PlexingDate.Value.ToShortDateString();

                if (temp != currentDate)
                {
                    occations += 1;
                    currentDate = temp;
                }
            }

            OccationsLabel.Text = occations.ToString();

        }
    }

    private void SetPermission(PlexingFleetDataContext context, PlexUserRole role, bool permissionChecked, string permissionName)
    {
        if (permissionChecked)
        {

            if (role != null)
            {
                if (!role.Roles.Contains(permissionName))
                {
                    if (role.Roles.Length > 0)
                        role.Roles += ",";

                    role.Roles += permissionName;
                }
            }
            else
            {
                role = new PlexUserRole();
                role.CharacterId = CharacterIdLabel.Text.ToInt();
                role.Roles = permissionName;

                context.PlexUserRoles.InsertOnSubmit(role);
            }
        }
        else
        {
            if (role != null)
            {
                role.Roles = role.Roles.Replace(string.Format("{0},", permissionName), "").Replace(string.Format(",{0}", permissionName), "");

                if (role.Roles == permissionName || role.Roles.Trim().Length == 0)
                {
                    context.PlexUserRoles.DeleteOnSubmit(role);
                }
            }
        }
    }

    protected void UpdateLinkButton_Click(object sender, EventArgs e)
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(ConnectionString))
        {
            PlexUser user = context.PlexUsers.FirstOrDefault(x => x.CharacterId == CharacterIdLabel.Text.ToInt());

            user.Enabled = !BannedCheckBox.Checked;

            if (NewPasswordTextBox.Text.Trim() != string.Empty && NewPasswordTextBox.Text == ConfirmPasswordTextBox.Text)
            {
                user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(NewPasswordTextBox.Text, "md5");
            }

            PlexUserRole role = context.PlexUserRoles.FirstOrDefault(x => x.CharacterId == CharacterIdLabel.Text.ToInt());

            SetPermission(context, role, AdministratorCheckBox.Checked, "Admin");

            context.SubmitChanges();

        }

        Response.Redirect(PageReferrer.Page_Admin_Users);
    }

    protected void CancelLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(PageReferrer.Page_Admin_Users);
    }
}