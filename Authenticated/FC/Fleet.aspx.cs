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
            if (ViewState["VS_PLEXING_PERIOD_ID"] == null)
            {
                using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
                {
                    PlexingPeriod period = context.PlexingPeriods.FirstOrDefault(x => x.ToDate == null);

                    //No active plexing period, create a new period
                    if (period == null)
                    {
                        period = new PlexingPeriod()
                        {
                            FromDate = DateTime.UtcNow
                        };

                        context.PlexingPeriods.InsertOnSubmit(period);
                        context.SubmitChanges();
                    }

                    ViewState["VS_PLEXING_PERIOD_ID"] = period.PlexingPeriodId;
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
                    PlexingPeriod period = context.PlexingPeriods.FirstOrDefault(x => x.PlexingPeriodId == PlexingPeriodId);

                    ViewState["VS_PLEXING_PERIOD_FROM"] = period.FromDate;
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
                    join plexInfo in context.PlexInfos on plexes.PlexInfoId equals plexInfo.PlexId
                    where plexes.FCId == CharacterId && plexes.PlexingPeriodId == PlexingPeriodId
                    orderby plexes.PlexingDate descending
                    select new PlexListInfo() { PlexId = plexes.PlexId, PlexName = plexInfo.Name, PlexingDate = plexes.PlexingDate.Value, Participants = plexes.Participants, Points = plexInfo.Points.Value };

            PlexGridView.DataSource = p;
            PlexGridView.DataBind();
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

        Plex plex = new Plex() { FCId = CharacterId, PlexInfoId = plexInfoId, PlexingDate = DateTime.UtcNow, PlexingPeriodId = PlexingPeriodId, Participants = GetPilots() };

        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            context.Plexes.InsertOnSubmit(plex);
            context.SubmitChanges();
        }

        FillPlexes();
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