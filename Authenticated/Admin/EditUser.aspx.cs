using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Authenticated_Admin_EditUser : PageBase
{
    private string Origin
    {
        get
        {
            if (ViewState["Origin"] == null)
                ViewState["Origin"] = "Orgin";

            return (string)ViewState["Origin"];
        }
        set
        {
            ViewState["Origin"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Origin"]))
                Origin = Request.QueryString["Origin"];

            if (!string.IsNullOrEmpty(Request.QueryString["CharacterId"]))
            {
                var user = GetUser(Request.QueryString["CharacterId"].ToInt());
                
                if (IsSuperAdmin ||
                    IsAdmin ||
                    IsAllianceAdmin && AllianceId == user.AllianceId && AllianceId != -1 ||
                    IsCorpAdmin && CorpId == user.CorpId)
                {
                    LoadCharacterInfo(Request.QueryString["CharacterId"].ToInt());
                }
            }
        }
    }

    private PlexUser GetUser(int characterId)
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(ConnectionString))
        {
            var user = context.PlexUsers.FirstOrDefault(x => x.CharacterId == characterId);

            return user;
        }
    }


    private void LoadCorporationInfo(int corporationId)
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(ConnectionString))
        {
            var corps = from c in context.Corps
                        orderby c.CorpName
                        select c;

            CorporationDropDownList.Items.Clear();

            foreach (var corp in corps)
            {
                ListItem item = new ListItem(corp.CorpName, corp.CorpId.ToString());
                CorporationDropDownList.Items.Add(item);
            }


            ListItem corporation = CorporationDropDownList.Items.FindByValue(corporationId.ToString());

            if (corporation != null)
            {
                corporation.Selected = true;
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
                LoadCorporationInfo(user.CorpId);
                CharacterIdLabel.Text = user.CharacterId.ToString();
                CharacterNameLabel.Text = user.CharacterName;
                //CorporationLabel.Text = user.CorpName;
                AllianceLabel.Text = user.AllianceName;
                BannedCheckBox.Checked = !user.Enabled;
            }

            var userPermissions = from up in context.PlexUserRoles
                                  where up.CharacterId == characterId
                                  select up;

            foreach (var permission in userPermissions)
            {
                SuperAdminCheckBox.Checked = permission.Roles.ToLower().Contains("super");
                AdministratorCheckBox.Checked = permission.Roles.ToLower().Contains("admin");
                AllianceCheckBox.Checked = permission.Roles.ToLower().Contains("alliance");
                CorporationCheckBox.Checked = permission.Roles.ToLower().Contains("corporation");
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

            SuperAdminRow.Visible = IsSuperAdmin;
            AdminRow.Visible = IsSuperAdmin || IsAdmin;
            AllianceAdminRow.Visible = IsSuperAdmin || IsAdmin || (IsAllianceAdmin && user.AllianceId == AllianceId && AllianceId != -1);
            CorpAdminRow.Visible = IsSuperAdmin || IsAdmin || (IsAllianceAdmin && user.AllianceId == AllianceId && AllianceId != -1) || (IsCorpAdmin && user.CorpId == CorpId);
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

            if (user.CorpId != CorporationDropDownList.SelectedValue.ToInt())
            {
                var corp = context.Corps.FirstOrDefault(x => x.CorpId == CorporationDropDownList.SelectedValue.ToInt());

                if (corp != null)
                {
                    user.CorpId = corp.CorpId;
                    user.AllianceId = corp.AllianceId;
                    user.CorpName = corp.CorpName;
                    user.AllianceName = corp.AllianceName;
                }
            }

            if (NewPasswordTextBox.Text.Trim() != string.Empty && NewPasswordTextBox.Text == ConfirmPasswordTextBox.Text)
            {
                user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(NewPasswordTextBox.Text, "md5");
            }

            PlexUserRole role = context.PlexUserRoles.FirstOrDefault(x => x.CharacterId == CharacterIdLabel.Text.ToInt());

            SetPermission(context, role, SuperAdminCheckBox.Checked, "Super");
            SetPermission(context, role, AdministratorCheckBox.Checked, "Admin");
            SetPermission(context, role, AllianceCheckBox.Checked, "Alliance");
            SetPermission(context, role, CorporationCheckBox.Checked, "Corporation");

            context.SubmitChanges();
        }

        Response.Redirect(string.Format("{0}?{1}=1", PageReferrer.Page_Admin_Users, Origin));
    }

    protected void CancelLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(string.Format("{0}?{1}=1", PageReferrer.Page_Admin_Users, Origin));
    }
}