using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Text;

public partial class Authenticated_FC_Fleet : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PlexingPeriodLabel.Text = PlexingPeriodFrom.ToShortDateString();
            FillPlexInfos();
            FillFleet();
            FillPlexes();
        }
    }

    private int PlexingPeriodId
    {
        get
        {
            if (ViewState["VS_PLEXING_PERIOD_ID"] == null || ((int)ViewState["VS_PLEXING_PERIOD_ID"]) == -1)
            {
                using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
                {
                    if (PlexCorpDropDownList.Items.Count > 0 && !string.IsNullOrEmpty(PlexCorpDropDownList.SelectedValue))
                    {
                        PlexingPeriod period = context.PlexingPeriods.FirstOrDefault(x => x.ToDate == null && x.CorpId == int.Parse(PlexCorpDropDownList.SelectedValue));

                        //No active plexing period, create a new period
                        if (period == null)
                        {
                            period = new PlexingPeriod()
                            {
                                FromDate = DateTime.UtcNow,
                                CorpId = CorpId
                            };

                            context.PlexingPeriods.InsertOnSubmit(period);
                            context.SubmitChanges();
                        }
                        ViewState["VS_PLEXING_PERIOD_ID"] = period.PlexingPeriodId;
                    }
                    else
                        ViewState["VS_PLEXING_PERIOD_ID"] = -1;
                }
            }

            return (int)ViewState["VS_PLEXING_PERIOD_ID"];
        }
    }

    private DateTime PlexingPeriodFrom
    {
        get
        {
            if (ViewState["VS_PLEXING_PERIOD_FROM"] == null)
            {
                using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
                {
                    if (PlexingPeriodId != -1)
                    {
                        PlexingPeriod period = context.PlexingPeriods.FirstOrDefault(x => x.PlexingPeriodId == PlexingPeriodId);

                        ViewState["VS_PLEXING_PERIOD_FROM"] = period.FromDate;
                    }
                    else
                        ViewState["VS_PLEXING_PERIOD_FROM"] = DateTime.Now;

                }
            }

            return (DateTime)ViewState["VS_PLEXING_PERIOD_FROM"];
        }
    }

    private void FillPlexInfos()
    {
        PlexInfoDropDownList.Items.Clear();

        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            var plexes = from p in context.PlexInfos
                         orderby p.Name
                         select p;

            foreach (var plex in plexes)
            {
                PlexInfoDropDownList.Items.Add(new ListItem(plex.Name, plex.PlexId.ToString()));
            }

            if (IsAdmin)
            {
                //Display all active corps
                var corps = (from c in context.Corps
                             where c.Enabled
                             orderby c.CorpName
                             select c).Distinct();

                AddPlexCorps(corps.ToList());
            }
            else
            {
                //Only display active corps in the alliance
                var corps = (from c in context.Corps
                             where c.Enabled && ((c.AllianceId == AllianceId && AllianceId != -1) || c.CorpId == CorpId)
                             orderby c.CorpName
                             select c).Distinct();

                AddPlexCorps(corps.ToList());
            }

            PlexCorpDropDownList.Items.FindByValue(CorpId.ToString()).Selected = true;
        }
    }

    private void AddPlexCorps(List<Corp> corps)
    {
        foreach (var corp in corps)
        {
            if (string.IsNullOrEmpty(corp.CorpTag))
            {
                PlexCorpDropDownList.Items.Add(new ListItem(corp.CorpName.Substring(0, 10), corp.CorpId.ToString()));
            }
            else
            {
                PlexCorpDropDownList.Items.Add(new ListItem(corp.CorpTag, corp.CorpId.ToString()));
            }
        }
    }

    private void FillFleet()
    {
        PilotListBox.Items.Clear();

        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            Fleet fleet = context.Fleets.FirstOrDefault(x => x.FcId == CharacterId);

            if (fleet != null)
            {
                string[] pilots = fleet.Participants.Split(',');

                foreach (string pilot in pilots)
                {
                    PilotListBox.Items.Add(new ListItem(pilot, pilot));
                }
            }
        }
    }

    private void FillPlexes()
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            var p = from plexes in context.Plexes
                    join plexUsers in context.PlexUsers on plexes.FCId equals plexUsers.CharacterId
                    join plexInfo in context.PlexInfos on plexes.PlexInfoId equals plexInfo.PlexId
                    join corps in context.Corps on plexes.CorpId equals corps.CorpId
                    where (IsAdmin && plexes.PlexingDate >= GetCurrentPlexingPeriodDate()) || (plexes.FCId == CharacterId && plexes.PlexingDate >= GetCurrentPlexingPeriodDate()) || (IsAllianceAdmin && plexes.PlexingDate >= GetCurrentPlexingPeriodDate() && corps.AllianceId == AllianceId) || (IsCorpAdmin && plexes.PlexingDate >= GetCurrentPlexingPeriodDate() && plexes.CorpId == CorpId)
                    orderby plexes.PlexingDate descending
                    select new PlexListInfo() { PlexId = plexes.PlexId, FCName = plexUsers.CharacterName, PlexName = plexInfo.Name, PlexingDate = plexes.PlexingDate.Value, Participants = plexes.Participants, Points = plexInfo.Points, CorpTag = corps.CorpTag };

            PlexGridView.DataSource = p;
            PlexGridView.DataBind();
        }
    }

    private DateTime GetCurrentPlexingPeriodDate()
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            DateTime result = DateTime.Now;
            var plexingPeriods = from p in context.PlexingPeriods
                                 where p.ToDate == null
                                 orderby p.FromDate
                                 select p;

            foreach (var period in plexingPeriods)
            {
                if (period.FromDate < result)
                    result = period.FromDate.Value;
            }

            return result;
        }
    }

    protected string FormatPlex(string plexName, string FCName)
    {
        if (IsAdmin)
        {
            return string.Format("{0}<br />({1})", plexName, FCName);
        }
        else
        {
            return plexName;
        }
    }

    protected void UpdateFleetLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(PageReferrer.Page_FC_SelectMembers);
    }

    protected void RemoveSelectedLinkButton_Click(object sender, EventArgs e)
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
        FillFleet();
    }

    private void UpdateFleet()
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
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

            context.SubmitChanges();
        }
    }

    protected void AddPlexLinkButton_Click(object sender, EventArgs e)
    {
        int plexInfoId = PlexInfoDropDownList.SelectedValue.ToInt();

        Plex plex;

        if (PlexCorpDropDownList.SelectedValue.ToInt() != CorpId)
        {
            plex = new Plex() { FCId = CharacterId, PlexInfoId = plexInfoId, PlexingDate = DateTime.UtcNow, PlexingPeriodId = GetPlexingPeriodId(PlexCorpDropDownList.SelectedValue.ToInt()), Participants = GetPilots(), CorpId = int.Parse(PlexCorpDropDownList.SelectedValue) };
        }
        else
        {
            plex = new Plex() { FCId = CharacterId, PlexInfoId = plexInfoId, PlexingDate = DateTime.UtcNow, PlexingPeriodId = PlexingPeriodId, Participants = GetPilots(), CorpId = int.Parse(PlexCorpDropDownList.SelectedValue) };
        }

        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            context.Plexes.InsertOnSubmit(plex);
            context.SubmitChanges();
        }

        FillPlexes();
    }

    private int GetPlexingPeriodId(int corpId)
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            PlexingPeriod period = context.PlexingPeriods.FirstOrDefault(x => x.ToDate == null && x.CorpId == int.Parse(PlexCorpDropDownList.SelectedValue));
            return period.PlexingPeriodId;
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

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
    }

    protected void PlexGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditPlex")
        {
            int plexId = e.CommandArgument.ToString().ToInt();

            Response.Redirect(string.Format("{0}?PlexId={1}", PageReferrer.Page_FC_SelectMembers, plexId));
        }
        else if (e.CommandName == "DeletePlex")
        {
            int plexId = e.CommandArgument.ToString().ToInt();

            using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
            {
                Plex p = context.Plexes.FirstOrDefault(x => x.PlexId == plexId);

                if (p != null)
                {
                    context.Plexes.DeleteOnSubmit(p);
                    context.SubmitChanges();
                }

                FillPlexes();
            }
        }
    }
}