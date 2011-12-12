using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;

public partial class Authenticated_PlexingPeriod_PlexingPeriod : PageBase
{
    private int? PlexingPeriodId
    {
        get
        {
            return (int)ViewState["VS_PlexingPeriodId"];
        }
        set
        {
            ViewState["VS_PlexingPeriodId"] = value;
        }
    }

    private int? LastPlexingPeriodId
    {
        get
        {
            return (int?)ViewState["VS_LastPlexingPeriodId"];
        }
        set
        {
            ViewState["VS_LastPlexingPeriodId"] = value;
        }
    }

    private double? IskPerPointAfterTax
    {
        get
        {
            return (double?)ViewState["VS_IskPerPointAfterTax"];
        }
        set
        {
            ViewState["VS_IskPerPointAfterTax"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (string.IsNullOrEmpty(Request.QueryString["PlexingPeriodId"]))
                FillPlexingPeriods(null);
            else
                FillPlexingPeriods(Request.QueryString["PlexingPeriodId"].ToInt());
        }
    }

    public string GetPlexingPeriod(DateTime? fromDate, DateTime? toDate)
    {
        if (toDate.HasValue)
        {
            return string.Format("{0:d} - {1:d}", fromDate, toDate);
        }
        else
        {
            return string.Format("{0:d} - ", fromDate);
        }
    }

    private void FillPlexingPeriods(int? plexingPeriodId)
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            var plexingPeriods = from p in context.PlexingPeriods
                                 orderby p.FromDate descending
                                 select new PlexingPeriodListInfo() { PlexingPeriodId = p.PlexingPeriodId, FromDate = p.FromDate, ToDate = p.ToDate };

            List<PlexingPeriodListInfo> periods = plexingPeriods.ToList<PlexingPeriodListInfo>();

            int periodCounter = 0;
            int maxPeriods = 2;
            foreach (var period in periods)
            {
                var plexingDates = from p in context.Plexes
                                   where p.PlexingPeriodId == period.PlexingPeriodId
                                   orderby p.PlexingDate descending
                                   select p;

                Dictionary<string, DateInfos> infos = new Dictionary<string,DateInfos>();

                foreach (var plex in plexingDates)
                {
                    string datestring = new DateTime(plex.PlexingDate.Value.Year, plex.PlexingDate.Value.Month, plex.PlexingDate.Value.Day).ToString();
                    if (!infos.ContainsKey(datestring))
                    {
                        infos.Add(datestring, new DateInfos() { Date = new DateTime(plex.PlexingDate.Value.Year, plex.PlexingDate.Value.Month, plex.PlexingDate.Value.Day), PlexingPeriodId = period.PlexingPeriodId });

                        //Only add all plexing dates if you are an admin
                        if (periodCounter < maxPeriods || IsAdmin)
                        {
                            period.DateInfos.Add(new DateInfos() { Date = new DateTime(plex.PlexingDate.Value.Year, plex.PlexingDate.Value.Month, plex.PlexingDate.Value.Day), PlexingPeriodId = period.PlexingPeriodId });
                        }
                    }
                }
                periodCounter += 1;
                //period.DateInfos = infos.Values.ToList();
            }

            PlexingPeriodsDataList.DataSource = periods;
            PlexingPeriodsDataList.DataBind();

            if (plexingPeriods.Count() > 0)
            {
                if (plexingPeriodId.HasValue)
                {
                    FillPlexingPeriodData(plexingPeriodId.Value);
                    PlexingPeriodId = plexingPeriodId.Value;
                }
                else
                {
                    FillPlexingPeriodData(plexingPeriods.First().PlexingPeriodId);
                    PlexingPeriodId = plexingPeriods.First().PlexingPeriodId;
                    LastPlexingPeriodId = PlexingPeriodId;
                }
            }
        }
    }
    

    private void FillPlexingPeriodData(int plexingPeriodId)
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            PlexingPeriod pp = context.PlexingPeriods.FirstOrDefault(x => x.PlexingPeriodId == plexingPeriodId);

            if (pp != null)
            {
                PlexingPeriodDateLabel.Text = GetPlexingPeriod(pp.FromDate, pp.ToDate);
            }
        }

        List<PlexingPeriodInfo> infos = LoadPlexingPeriodData(plexingPeriodId);
        infos.Sort();

        PlexingPeriodInfoGridView.DataSource = infos;
        PlexingPeriodInfoGridView.DataBind();
    }

    private List<PlexingPeriodInfo> LoadPlexingPeriodData(int plexingPeriodId)
    {
        PlexingPeriodInfoGridView.Columns[3].Visible = plexingPeriodId != LastPlexingPeriodId && LastPlexingPeriodId.HasValue;

        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            PlexingPeriod period = context.PlexingPeriods.FirstOrDefault(x => x.PlexingPeriodId == plexingPeriodId);
            IskPerPointAfterTax = period.IskPerPointAfterTax.HasValue ? period.IskPerPointAfterTax.Value : 0;

            var plexPointinfos = from p in context.Plexes
                                 join pi in context.PlexInfos on p.PlexInfoId equals pi.PlexId
                                 where p.PlexingPeriodId == plexingPeriodId
                                 select new { p.PlexId, p.Participants, pi.Points };

            Dictionary<string, PlexingPeriodInfo> dict = new Dictionary<string, PlexingPeriodInfo>();

            double totalPoints = 0;

            foreach (var plexPointInfo in plexPointinfos)
            {
                totalPoints += plexPointInfo.Points.Value;

                string[] pilots = plexPointInfo.Participants.Split(',');

                double pilotPoints = ((double)plexPointInfo.Points.Value) / pilots.Count();

                foreach (string pilot in pilots)
                {
                    if (dict.ContainsKey(pilot.Trim()))
                    {
                        dict[pilot.Trim()].Plexes += 1;
                        dict[pilot.Trim()].Points += pilotPoints;
                        dict[pilot.Trim()].Payout = ((IskPerPointAfterTax.HasValue ? IskPerPointAfterTax.Value : 0) / 1000000) * dict[pilot.Trim()].Points;
                    }
                    else
                    {
                        dict.Add(pilot.Trim(), new PlexingPeriodInfo() { PlexId = plexPointInfo.PlexId, Name = pilot.Trim(), Plexes = 1, Points = pilotPoints, Payout = ((IskPerPointAfterTax.HasValue ? IskPerPointAfterTax.Value : 0) / 1000000) * pilotPoints });
                    }
                }
            }

            TotalPointsLabel.Text = Math.Round(totalPoints, 2).ToString();

            return dict.Values.ToList();
        }
    }

    protected void PlexingPeriodInfoGridView_RowCommand(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "ShowInfo")
        {
            Response.Redirect(string.Format("{0}?PlexingPeriodId={1}&CharacterName={2}", PageReferrer.Page_User_PlexingPeriod, PlexingPeriodId.Value, e.CommandArgument.ToString()));
        }
    }

    protected void DisplayPlexingPeriod(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "DisplayPlexingPeriod")
        {
            FillPlexingPeriodData(e.CommandArgument.ToString().ToInt());
        }
    }

    protected void DisplayPlexingDate(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "DisplayDatePlexes")
        {
            string[] command = e.CommandArgument.ToString().Split('|');

            FillPlexingPeriodDateData(command[0].ToInt(), DateTime.Parse(command[1]));
        }
    }

    private void FillPlexingPeriodDateData(int plexingPeriodId, DateTime date)
    {
        PlexingPeriodInfoGridView.Columns[3].Visible = plexingPeriodId != LastPlexingPeriodId;

        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            PlexingPeriod pp = context.PlexingPeriods.FirstOrDefault(x => x.PlexingPeriodId == plexingPeriodId);

            if (pp != null)
            {
                PlexingPeriodDateLabel.Text = GetPlexingPeriod(pp.FromDate, pp.ToDate);
            }
        }

        List<PlexingPeriodInfo> infos = LoadPlexingPeriodData(plexingPeriodId, date);
        infos.Sort();

        PlexingPeriodInfoGridView.DataSource = infos;
        PlexingPeriodInfoGridView.DataBind();
    }

    private List<PlexingPeriodInfo> LoadPlexingPeriodData(int plexingPeriodId, DateTime date)
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            PlexingPeriod period = context.PlexingPeriods.FirstOrDefault(x => x.PlexingPeriodId == plexingPeriodId);
            IskPerPointAfterTax = period.IskPerPointAfterTax.HasValue ? period.IskPerPointAfterTax.Value : 0;

            DateTime lastDate = new DateTime(date.Year, date.Month, date.Day).AddDays(1);

            var plexPointinfos = from p in context.Plexes
                                 join pi in context.PlexInfos on p.PlexInfoId equals pi.PlexId
                                 where p.PlexingPeriodId == plexingPeriodId && p.PlexingDate >= date && p.PlexingDate <= lastDate
                                 select new { p.PlexId, p.Participants, pi.Points };

            Dictionary<string, PlexingPeriodInfo> dict = new Dictionary<string, PlexingPeriodInfo>();

            double totalPoints = 0;

            foreach (var plexPointInfo in plexPointinfos)
            {
                totalPoints += plexPointInfo.Points.Value;
                string[] pilots = plexPointInfo.Participants.Split(',');

                double pilotPoints = ((double)plexPointInfo.Points.Value) / pilots.Count();

                foreach (string pilot in pilots)
                {
                    if (dict.ContainsKey(pilot.Trim()))
                    {
                        dict[pilot.Trim()].Plexes += 1;
                        dict[pilot.Trim()].Points += pilotPoints;
                        dict[pilot.Trim()].Payout = ((IskPerPointAfterTax.HasValue ? IskPerPointAfterTax.Value : 0) / 1000000) * dict[pilot.Trim()].Points;
                    }
                    else
                    {
                        dict.Add(pilot.Trim(), new PlexingPeriodInfo() { PlexId = plexPointInfo.PlexId, Name = pilot.Trim(), Plexes = 1, Points = pilotPoints, Payout = ((IskPerPointAfterTax.HasValue ? IskPerPointAfterTax.Value : 0) / 1000000) * pilotPoints });
                    }
                }
            }

            TotalPointsLabel.Text = totalPoints.ToString();

            return dict.Values.ToList();
        }
    }

    protected string FormatMillions(string value)
    {
        return string.Format("{0}m", value);
    }
}