using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;

public partial class Authenticated_Admin_Users : PageBase
{

    private bool FromAdmin
    {
        get
        {
            if (ViewState["FromAdmin"] == null)
                ViewState["FromAdmin"] = false;

            return (bool)ViewState["FromAdmin"];
        }
        set
        {
            ViewState["FromAdmin"] = value;
        }
    }

    private bool FromAlliance
    {
        get
        {
            if (ViewState["FromAlliance"] == null)
                ViewState["FromAlliance"] = false;

            return (bool)ViewState["FromAlliance"];
        }
        set
        {
            ViewState["FromAlliance"] = value;
        }
    }

    private bool FromCorp
    {
        get
        {
            if (ViewState["FromCorp"] == null)
                ViewState["FromCorp"] = false;

            return (bool)ViewState["FromCorp"];
        }
        set
        {
            ViewState["FromCorp"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FromAdmin = Request.QueryString["Admin"] == "1";
            FromAlliance = Request.QueryString["Alliance"] == "1";
            FromCorp = Request.QueryString["Corp"] == "1";

            SearchTextBox.Attributes.Add("onkeyup", ClientScript.GetPostBackEventReference(SearchTextBox, "doSearch"));
            DoSearch(SearchTextBox.Text);
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {

    }

    protected void TextChanged(object sender, EventArgs e)
    {
        DoSearch(SearchTextBox.Text);
    }

    private void DoSearch(string searchText)
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(ConnectionString))
        {
            if ((IsAdmin || IsSuperAdmin) && FromAdmin)
            {
                var users = from pu in context.PlexUsers
                            join pur in context.PlexUserRoles on pu.CharacterId equals pur.CharacterId into pu_pur
                            where pu.CharacterName.Contains(searchText)

                            from p in pu_pur.DefaultIfEmpty()
                            select new
                            {
                                CharacterId = pu.CharacterId,
                                CharacterName = pu.CharacterName,
                                CorpName = pu.CorpName,
                                AllianceName = pu.AllianceName,
                                Enabled = pu.Enabled,
                                Roles = p.Roles
                            };

                UsersGridView.DataSource = users.OrderBy(x => x.CharacterName);
                UsersGridView.DataBind();
            }
            else if (IsAllianceAdmin && FromAlliance)
            {
                var users = from pu in context.PlexUsers
                            join pur in context.PlexUserRoles on pu.CharacterId equals pur.CharacterId into pu_pur
                            where pu.CharacterName.Contains(searchText) && pu.AllianceId == AllianceId && pu.AllianceId != -1

                            from p in pu_pur.DefaultIfEmpty()
                            select new
                            {
                                CharacterId = pu.CharacterId,
                                CharacterName = pu.CharacterName,
                                CorpName = pu.CorpName,
                                AllianceName = pu.AllianceName,
                                Enabled = pu.Enabled,
                                Roles = p.Roles
                            };

                UsersGridView.DataSource = users.OrderBy(x => x.CharacterName);
                UsersGridView.DataBind();
            }
            else if (IsCorpAdmin && FromCorp)
            {
                var users = from pu in context.PlexUsers
                            join pur in context.PlexUserRoles on pu.CharacterId equals pur.CharacterId into pu_pur
                            where pu.CharacterName.Contains(searchText) && pu.CorpId == CorpId

                            from p in pu_pur.DefaultIfEmpty()
                            select new
                            {
                                CharacterId = pu.CharacterId,
                                CharacterName = pu.CharacterName,
                                CorpName = pu.CorpName,
                                AllianceName = pu.AllianceName,
                                Enabled = pu.Enabled,
                                Roles = p.Roles
                            };

                UsersGridView.DataSource = users.OrderBy(x => x.CharacterName);
                UsersGridView.DataBind();
            }
        }
    }

    private string GetOrigin()
    {
        if (FromAdmin) return "Admin";
        if (FromAlliance) return "Alliance";
        if (FromCorp) return "Corp";

        return string.Empty;
    }

    protected void UsersGridView_RowCommand(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "EditCharacter")
        {
            Response.Redirect(string.Format("{0}?CharacterId={1}&Origin={2}", PageReferrer.Page_Admin_EditUser, e.CommandArgument.ToString(), GetOrigin()));
        }
        else if (e.CommandName == "DeleteCharacter")
        {
            using (PlexingFleetDataContext context = new PlexingFleetDataContext(ConnectionString))
            {
                int characterId = e.CommandArgument.ToString().ToInt();

                var roles = context.PlexUserRoles.FirstOrDefault(x => x.CharacterId == characterId);

                if (roles != null && roles.Roles.Contains("Super"))
                    return;

                context.PlexUsers.DeleteOnSubmit(context.PlexUsers.FirstOrDefault(x => x.CharacterId == characterId));

                context.SubmitChanges();

                DoSearch(SearchTextBox.Text);
            }
        }
    }
}