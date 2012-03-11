using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Configuration;

public partial class Authenticated_FC_SelectMembers : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["__EVENTARGUMENT"] != null && Request["__EVENTARGUMENT"] == "moveToFleet")
        {
            if (SearchResultListBox.SelectedItem != null)
            {
                if (PilotListBox.Items.FindByValue(SearchResultListBox.SelectedValue) == null)
                {
                    AddPilot(SearchResultListBox.SelectedItem.Text, SearchResultListBox.SelectedItem.Value);
                    //PilotListBox.Items.Add(new ListItem(SearchResultListBox.SelectedItem.Text, SearchResultListBox.SelectedValue));

                    if (PilotListBox.Items[0].Value == "")
                    {
                        PilotListBox.Items.RemoveAt(0);
                    }

                    UpdateFleet();
                    DoSearch(SearchTextBox.Text);
                }
            }
        }

        if (Request["__EVENTARGUMENT"] != null && Request["__EVENTARGUMENT"] == "moveFromFleet")
        {
            PilotListBox.Items.RemoveAt(PilotListBox.SelectedIndex);
            UpdateFleet();
            DoSearch(SearchTextBox.Text);
        }

        //SearchResultListBox.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(SearchResultListBox, "moveToFleet"));
        //PilotListBox.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(SearchResultListBox, "moveFromFleet"));

        if (!IsPostBack)
        {
            // A little trick to force a postback whenever someone releases a key
            //SearchTextBox.Attributes.Add("onKeyUp", string.Format("__doPostBack('{0}', 'abc')", UpdatePanel1.ClientID));

            SearchResultListBox.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(SearchResultListBox, "moveToFleet"));
            PilotListBox.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(SearchResultListBox, "moveFromFleet"));
            SearchTextBox.Attributes.Add("onkeyup", ClientScript.GetPostBackEventReference(SearchTextBox, "doSearch"));

            if (!string.IsNullOrEmpty(Request.QueryString["PlexId"]))
            {
                PlexId = Request.QueryString["PlexId"].ToInt();
            }

            if (!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
            {
                ReturnUrl = Request.QueryString["ReturnUrl"];
                PlexingPeriodId = Request.QueryString["PlexingPeriodId"].ToInt();
                CharName = Request.QueryString["CharacterName"];
            }

            LoadFleet();
            DoSearch("");
        }
    }

    private void AddPilot(string pilotName, string pilotValue)
    {
        for (int i = 0; i < PilotListBox.Items.Count; i++)
        {
            if (PilotListBox.Items[i].Value.CompareTo(pilotName) == 1)
            {
                PilotListBox.Items.Insert(i, new ListItem(pilotName, pilotValue));
                UpdateFleet();
                return;
            }
        }

        PilotListBox.Items.Add(new ListItem(pilotName, pilotValue));
        UpdateFleet();
    }

    private int? PlexId
    {
        get
        {
            return (int?)ViewState["VS_PLEX_ID"];
        }
        set
        {
            ViewState["VS_PLEX_ID"] = value;
        }
    }

    private int PlexingPeriodId
    {
        get
        {
            return (int)ViewState["VS_PLEXING_PERIOD_ID"];
        }
        set
        {
            ViewState["VS_PLEXING_PERIOD_ID"] = value;
        }
    }

    public string CharName 
    {
        get
        {
            return (string)ViewState["VS_CHAR_NAME"];
        }
        set
        {
            ViewState["VS_CHAR_NAME"] = value;
        }
    }

    private string ReturnUrl
    {
        get
        {
            return (string)ViewState["VS_RETURN_URL"];
        }
        set
        {
            ViewState["VS_RETURN_URL"] = value;
        }
    }

    protected void TextChanged(object sender, EventArgs e)
    {
        DoSearch(SearchTextBox.Text);
    }

    private void LoadFleet()
    {
        PilotListBox.Items.Clear();

        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            if (PlexId.HasValue)
            {
                Plex plex = context.Plexes.FirstOrDefault(x => x.PlexId == PlexId);

                if (plex != null)
                {
                    string[] pilots = plex.Participants.Split(',');

                    foreach (string pilot in pilots)
                    {
                        PilotListBox.Items.Add(new ListItem(pilot.Trim(), pilot.Trim()));
                    }
                }
            }
            else
            {
                Fleet fleet = context.Fleets.FirstOrDefault(x => x.FcId == CharacterId);

                if (fleet != null)
                {
                    string[] pilots = fleet.Participants.Split(',');

                    foreach (string pilot in pilots)
                    {
                        PilotListBox.Items.Add(new ListItem(pilot.Trim(), pilot.Trim()));
                    }
                }
            }
        }
    }

    private void DoSearch(string searchString)
    {
        SearchResultListBox.Items.Clear();

        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            if (SearchTextBox.Text.Trim() == string.Empty)
            {
                var users = from u in context.PlexUsers
                            where u.Enabled
                            orderby u.CharacterName
                            select u;

                foreach (var user in users)
                {
                    if (PilotListBox.Items.FindByValue(user.CharacterName) == null)
                        SearchResultListBox.Items.Add(new ListItem(user.CharacterName, user.CharacterName));
                }
            }
            else
            {
                var users = from u in context.PlexUsers
                            where u.CharacterName.Contains(SearchTextBox.Text) && (u.Enabled)
                            orderby u.CharacterName
                            select u;

                foreach (var user in users)
                {
                    if (PilotListBox.Items.FindByValue(user.CharacterName) == null)
                        SearchResultListBox.Items.Add(new ListItem(user.CharacterName, user.CharacterName));
                }
            }
        }
    }

    protected void SelectButton_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in SearchResultListBox.Items)
        {
            if (item.Selected)
            {
                if (PilotListBox.Items.FindByValue(item.Value) == null)
                {
                    PilotListBox.Items.Add(new ListItem(item.Text, item.Value));
                }
            }
        }

        if (PilotListBox.Items[0].Value == "")
        {
            PilotListBox.Items.RemoveAt(0);
        }

        UpdateFleet();

        DoSearch(SearchTextBox.Text);
    }

    protected void AddGuestLinkButton_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(SearchTextBox.Text.Trim()))
        {
            AddPilot(SearchTextBox.Text, SearchTextBox.Text);

            UpdateFleet();

            if (PlexId.HasValue)
            {
                Response.Redirect(string.Format("{0}?PlexId={1}", PageReferrer.Page_FC_SelectMembers, PlexId));
            }
            else
            {
                Response.Redirect(PageReferrer.Page_FC_SelectMembers);
            }
        }
    }

    protected void DeselectButton_Click(object sender, EventArgs e)
    {
        List<ListItem> removedItems = new List<ListItem>();

        foreach (ListItem item in PilotListBox.Items)
        {
            if (item.Selected)
            {
                removedItems.Add(item);
            }
        }

        foreach (ListItem item in removedItems)
        {
            PilotListBox.Items.Remove(item);
        }

        UpdateFleet();

        DoSearch(SearchTextBox.Text);
    }

    private void UpdateFleet()
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            if (PlexId.HasValue)
            {
                Plex plex = context.Plexes.FirstOrDefault(x => x.PlexId == PlexId);

                plex.Participants = GetPilots();
            }
            else
            {
                Fleet fleet = context.Fleets.FirstOrDefault(x => x.FcId == CharacterId);

                if (fleet == null)
                {
                    fleet = new Fleet() { FcId = CharacterId, Participants = GetPilots() };
                    context.Fleets.InsertOnSubmit(fleet);
                }
                else
                {
                    fleet.Participants = GetPilots();
                }
            }

            context.SubmitChanges();
        }
    }

    private string GetPilots()
    {
        StringBuilder sb = new StringBuilder();
        foreach (ListItem item in PilotListBox.Items)
        {
            if (sb.Length != 0)
                sb.Append(", ");

            sb.Append(item.Text);
        }

        return sb.ToString();
    }
    protected void SaveLinkButton_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ReturnUrl))
            Response.Redirect(PageReferrer.Page_FC_Fleet);
        else
            Response.Redirect(string.Format("{0}?PlexingPeriodId={1}&CharacterName={2}", ReturnUrl, PlexingPeriodId, CharName));
    }

    protected void ClearFleetLinkButton_Click(object sender, EventArgs e)
    {
        PilotListBox.Items.Clear();
        DoSearch("");
    }
}