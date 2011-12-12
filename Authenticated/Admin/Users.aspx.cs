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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SearchTextBox.Attributes.Add("onkeyup", ClientScript.GetPostBackEventReference(SearchTextBox, "doSearch"));
            DoSearch(SearchTextBox.Text);
        }
        //        DataSet ds = new DataSet();

        //        string str = @"
        //            <eveapi version=""2"">
        //                <currentTime>2011-01-11 22:26:04</currentTime>
        //                <result>
        //                    <rowset name=""members"" key=""characterID"" columns=""characterID,name,startDateTime,baseID,base,title,logonDateTime,logoffDateTime,locationID,location,shipTypeID,shipType,roles,grantableRoles"">
        //                        <row characterID=""150336922"" name=""corpexport"" startDateTime=""2007-06-13 14:39:00"" baseID=""0"" base="""" title=""asdf"" logonDateTime=""2007-06-16 21:12:00"" logoffDateTime=""2007-06-16 21:36:00"" locationID=""60011566"" location=""Bourynes VII - Moon 2 - University of Caille School"" shipTypeID=""606"" shipType=""Velator"" roles=""0"" grantableRoles=""0""/>
        //                        <row characterID=""150337897"" name=""corpslave"" startDateTime=""2007-06-14 13:14:00"" baseID=""0"" base="""" title="""" logonDateTime=""2007-06-16 21:14:00"" logoffDateTime=""2007-06-16 21:35:00"" locationID=""60011566"" location=""Bourynes VII - Moon 2 - University of Caille School"" shipTypeID=""670"" shipType=""Capsule"" roles=""22517998271070336"" grantableRoles=""0""/>
        //                    </rowset>
        //                </result>
        //                <cachedUntil>2011-01-12 02:05:05</cachedUntil>
        //            </eveapi>
        //            ";

        //        using (StringReader sr = new StringReader(str))
        //        {
        //            ds.ReadXml(sr);
        //        }

        //        StringBuilder sb = new StringBuilder();

        //        foreach (DataRow table in ds.Tables["row"].Rows)
        //        {
        //            sb.AppendLine(table["name"].ToString());
        //        }

        //        TextBox1.Text = sb.ToString();
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

            UsersGridView.DataSource = users;
            UsersGridView.DataBind();
        }
    }

    protected void UsersGridView_RowCommand(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "EditCharacter")
        {
            Response.Redirect(string.Format("{0}?CharacterId={1}", PageReferrer.Page_Admin_EditUser, e.CommandArgument.ToString()));
        }
        else if (e.CommandName == "DeleteCharacter")
        {
        }
    }

}