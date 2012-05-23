using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;

public partial class Authenticated_Admin_Payout : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["__EVENTARGUMENT"] != null && Request["__EVENTARGUMENT"] == "payoutchanged")
        {
        }

        if (Request["__EVENTARGUMENT"] != null && Request["__EVENTARGUMENT"] == "corptaxchanged")
        {
        }

        if (!IsPostBack)
        {
            FillPlexingPeriods(null);
            PayoutTextBox.Attributes.Add("onkeyup", ClientScript.GetPostBackEventReference(PayoutTextBox, "payoutchanged"));
            CorpTaxTextBox.Attributes.Add("onkeyup", ClientScript.GetPostBackEventReference(PayoutTextBox, "corptaxchanged"));
        }
    }

    private int? LastPeriodId
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

    private int? PlexingPeriodId
    {
        get
        {
            return (int?)ViewState["VS_PlexingPeriodId"];
        }
        set
        {
            ViewState["VS_PlexingPeriodId"] = value;
        }
    }


    private double IskPerPoint
    {
        get
        {
            if (ViewState["VS_IskPerPoint"] == null)
                IskPerPoint = 0;
            return (double)ViewState["VS_IskPerPoint"];
        }
        set
        {
            ViewState["VS_IskPerPoint"] = value;
        }
    }

    private double IskPerPointAfterTax
    {
        get
        {
            if (ViewState["VS_IskPerPointAfterTax"] == null)
                IskPerPointAfterTax = 0;
            return (double)ViewState["VS_IskPerPointAfterTax"];
        }
        set
        {
            ViewState["VS_IskPerPointAfterTax"] = value;
        }
    }

    private void ShowEndPeriodButton(bool show)
    {
        SaveLinkButton.Visible = !show;
        EndPeriodLinkButton.Visible = show;
    }

    private void CalculatePayout()
    {
        CalculatePayout(false);
    }

    private void CalculatePayout(bool savePayout)
    {
        double payout = PayoutTextBox.Text.ToDouble() * 1000000;
        double percent = CorpTaxTextBox.Text.ToDouble();

        double payoutAfterTax = payout * (1 - (percent / 100));
        double tax = (payout - payoutAfterTax);
        double points = GetPoints(PlexingPeriodId);

        IskPerPoint = payout / points;
        IskPerPointAfterTax = payoutAfterTax / points;

        TotalPayoutLabel.Text = FormatMillions(Math.Round((payoutAfterTax / 1000000), 2).ToString());
        TaxLabel.Text = FormatMillions(Math.Round(tax / 1000000, 0).ToString());

        IskPerPointLabel.Text = FormatMillions(Math.Round(IskPerPoint / 1000000, 3).ToString());
        IskPerPointAfterTaxLabel.Text = FormatMillions(Math.Round(IskPerPointAfterTax / 1000000, 3).ToString());

        TaxUpdatePanel.Update();
        TotalPayOutUpdatePanel.Update();
        IskPerPointAfterTaxUpdatePanel.Update();

        if (savePayout)
        {
            using (PlexingFleetDataContext context = new PlexingFleetDataContext(ConnectionString))
            {
                PlexingPeriod period = context.PlexingPeriods.FirstOrDefault(x => x.PlexingPeriodId == PlexingPeriodId);

                if (period != null)
                {
                    period.PayoutSum = payout;
                    period.CorpTax = CorpTaxTextBox.Text.ToDouble();
                    period.Points = points;
                    period.IskPerPoint = IskPerPoint;
                    period.IskPerPointAfterTax = IskPerPointAfterTax;

                    context.SubmitChanges();
                }
            }
        }

    }

    protected string FormatMillions(string value)
    {
        return FormatMillions(null, value);
    }

    protected string FormatMillions(int? characterId, string value)
    {
        if (characterId.HasValue)
            return string.Format("<url=showinfo:1375/{0}>{1}m</url>", characterId.Value, value);

        return string.Format("{0}m", value);
    }

    protected void PayoutChanged(object sender, EventArgs e)
    {
        CalculatePayout();

        FillPlexingPeriodData(PlexingPeriodId.Value);
        PlexingPeriodInfoUpdatePanel.Update();
    }

    private double GetPoints(int? plexingPeriodId)
    {
        if (plexingPeriodId.HasValue)
        {
            using (PlexingFleetDataContext context = new PlexingFleetDataContext(ConnectionString))
            {
                var plexinfos = from p in context.Plexes
                                join
                                    pi in context.PlexInfos on p.PlexInfoId equals pi.PlexId
                                where p.PlexingPeriodId == plexingPeriodId
                                select new { pi.Points };

                if (plexinfos.Count() == 0)
                    return 0;

                int? points = plexinfos.Sum(x => x.Points);

                return points.HasValue ? points.Value : 0;
            }
        }

        return 0;
    }

    private void FillPlexingPeriods(int? plexingPeriodId)
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            var plexingPeriods = from p in context.PlexingPeriods
                                 join c in context.Corps on p.CorpId equals c.CorpId
                                 orderby p.FromDate descending
                                 select new PlexingPeriodListInfo() { PlexingPeriodId = p.PlexingPeriodId, FromDate = p.FromDate, ToDate = p.ToDate, CorpId = p.CorpId, CorpTag = c.CorpTag};

            List<PlexingPeriodListInfo> periods = new List<PlexingPeriodListInfo>();// = plexingPeriods.ToList<PlexingPeriodListInfo>();

            foreach (var period in plexingPeriods)
            {
                if (IsSuperAdmin)
                {
                    periods.Add(period);
                }
                else
                {
                    if (period.CorpId == CorpId)
                        periods.Add(period);
                }
            }

            foreach (var period in periods)
            {
                var plexingDates = from p in context.Plexes
                                   where p.PlexingPeriodId == period.PlexingPeriodId
                                   orderby p.PlexingDate descending
                                   select p;

                Dictionary<string, DateInfos> infos = new Dictionary<string, DateInfos>();

                foreach (var plex in plexingDates)
                {
                    string datestring = new DateTime(plex.PlexingDate.Value.Year, plex.PlexingDate.Value.Month, plex.PlexingDate.Value.Day).ToString();
                    if (!infos.ContainsKey(datestring))
                    {
                        infos.Add(datestring, new DateInfos() { Date = new DateTime(plex.PlexingDate.Value.Year, plex.PlexingDate.Value.Month, plex.PlexingDate.Value.Day), PlexingPeriodId = period.PlexingPeriodId });
                        period.DateInfos.Add(new DateInfos() { Date = new DateTime(plex.PlexingDate.Value.Year, plex.PlexingDate.Value.Month, plex.PlexingDate.Value.Day), PlexingPeriodId = period.PlexingPeriodId });
                    }
                }

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
                    var period = plexingPeriods.FirstOrDefault(x => x.CorpId == CorpId);
                    if (period != null)
                    {
                        LoadPlexingPeriodPayoutData(period.PlexingPeriodId);
                        FillPlexingPeriodData(period.PlexingPeriodId);
                        PlexingPeriodId = period.PlexingPeriodId;
                        LastPeriodId = PlexingPeriodId;
                    }
                }
            }

            var plexingPeriod = context.PlexingPeriods.FirstOrDefault(x => x.PlexingPeriodId == PlexingPeriodId);

            if (plexingPeriod != null)
                ShowEndPeriodButton(!plexingPeriod.ToDate.HasValue);
        }
    }

    private void FillPlexingPeriodData(int plexingPeriodId)
    {
        PlexingPeriodId = plexingPeriodId;
        CalculatePayout();
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(ConnectionString))
        {
            PlexingPeriod pp = context.PlexingPeriods.FirstOrDefault(x => x.PlexingPeriodId == plexingPeriodId);

            if (pp != null)
            {
                PlexingPeriodDateLabel.Text = GetPlexingPeriodTitle(pp.FromDate, pp.ToDate, GetCorpTag(pp.CorpId));
            }
        }

        List<PlexingPeriodInfo> infos = LoadPlexingPeriodData(plexingPeriodId);
        infos.Sort();

        PlexingPeriodInfoGridView.DataSource = infos;
        PlexingPeriodInfoGridView.DataBind();
    }


    private List<PlexingPeriodInfo> LoadPlexingPeriodData(int plexingPeriodId)
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            var plexPointinfos = from p in context.Plexes
                                 join pi in context.PlexInfos on p.PlexInfoId equals pi.PlexId
                                 where p.PlexingPeriodId == plexingPeriodId
                                 select new { p.PlexId, p.Participants, pi.Points };

            Dictionary<string, PlexingPeriodInfo> dict = new Dictionary<string, PlexingPeriodInfo>();

            double totalPoints = 0;

            foreach (var plexPointInfo in plexPointinfos)
            {
                totalPoints += plexPointInfo.Points;

                string[] pilots = plexPointInfo.Participants.Split(',');

                double pilotPoints = ((double)plexPointInfo.Points) / pilots.Count();

                foreach (string pilot in pilots)
                {
                    if (dict.ContainsKey(pilot.Trim()))
                    {
                        dict[pilot.Trim()].Plexes += 1;
                        dict[pilot.Trim()].Points += pilotPoints;
                        dict[pilot.Trim()].Payout = (IskPerPointAfterTax / 1000000) * dict[pilot.Trim()].Points;
                    }
                    else
                    {
                        dict.Add(pilot.Trim(), new PlexingPeriodInfo() { PlexId = plexPointInfo.PlexId, CharacterId=GetCharacterIdByName(pilot.Trim()), Name = pilot.Trim(), Plexes = 1, Points = pilotPoints, Payout = (IskPerPointAfterTax / 1000000) * pilotPoints });
                    }
                }
            }

            TotalPointsLabel.Text = Math.Round(totalPoints, 2).ToString();

            return dict.Values.ToList();
        }
    }

    private int? GetCharacterIdByName(string name)
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            var user = context.PlexUsers.FirstOrDefault(x => x.CharacterName == name);

            if (user != null)
                return user.CharacterId;
        }

        return null;
    }

    private void FillPlexingPeriodDateData(int plexingPeriodId, DateTime date)
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            PlexingPeriod pp = context.PlexingPeriods.FirstOrDefault(x => x.PlexingPeriodId == plexingPeriodId);

            if (pp != null)
            {
                PlexingPeriodDateLabel.Text = GetPlexingPeriod(pp.FromDate, pp.ToDate, GetCorpTag(pp.CorpId));
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
            DateTime lastDate = new DateTime(date.Year, date.Month, date.Day).AddDays(1);

            var plexPointinfos = from p in context.Plexes
                                 join pi in context.PlexInfos on p.PlexInfoId equals pi.PlexId
                                 where p.PlexingPeriodId == plexingPeriodId && p.PlexingDate >= date && p.PlexingDate <= lastDate //&& p.CorpId == CorpId
                                 select new { p.PlexId, p.Participants, pi.Points };

            Dictionary<string, PlexingPeriodInfo> dict = new Dictionary<string, PlexingPeriodInfo>();

            double totalPoints = 0;

            foreach (var plexPointInfo in plexPointinfos)
            {
                totalPoints += plexPointInfo.Points;
                string[] pilots = plexPointInfo.Participants.Split(',');

                double pilotPoints = ((double)plexPointInfo.Points) / pilots.Count();

                foreach (string pilot in pilots)
                {
                    if (dict.ContainsKey(pilot.Trim()))
                    {
                        dict[pilot.Trim()].Plexes += 1;
                        dict[pilot.Trim()].Points += pilotPoints;
                        dict[pilot.Trim()].Payout = (IskPerPointAfterTax / 1000000) * dict[pilot.Trim()].Points;
                    }
                    else
                    {
                        dict.Add(pilot.Trim(), new PlexingPeriodInfo() { PlexId = plexPointInfo.PlexId, CharacterId = GetCharacterIdByName(pilot.Trim()), Name = pilot.Trim(), Plexes = 1, Points = pilotPoints, Payout = (IskPerPointAfterTax / 1000000) * pilotPoints });
                    }
                }
            }

            TotalPointsLabel.Text = totalPoints.ToString();

            return dict.Values.ToList();
        }
    }

    protected string GetPlexingPeriod(DateTime? fromDate, DateTime? toDate, string corpTag)
    {
        if (toDate.HasValue)
        {
            return string.Format("{0:d} - {1:d}", fromDate, toDate);
        }
        else
        {
            return string.Format("{0:d} - ({1})", fromDate, corpTag);
        }
    }

    private string GetPlexingPeriodTitle(DateTime? fromDate, DateTime? toDate, string corpTag)
    {
        if (toDate.HasValue)
        {
            return string.Format("{0:d} - {1:d} ({2})", fromDate, toDate, corpTag);
        }
        else
        {
            return string.Format("{0:d} - ({1})", fromDate, corpTag);
        }
    }

    private void LoadPlexingPeriodPayoutData(int plexingPeriodId)
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(ConnectionString))
        {
            PlexingPeriod period = context.PlexingPeriods.FirstOrDefault(x => x.PlexingPeriodId == plexingPeriodId);

            if (period != null)
            {
                if (period.PayoutSum.HasValue)
                    PayoutTextBox.Text = (period.PayoutSum.Value / 1000000).ToString();
                else
                    PayoutTextBox.Text = string.Empty;

                if (period.CorpTax.HasValue)
                    CorpTaxTextBox.Text = (period.CorpTax.Value).ToString();
                else
                    CorpTaxTextBox.Text = string.Empty;
            }
        }

        CalculatePayout();
    }

    protected void DisplayPlexingPeriod(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "DisplayPlexingPeriod")
        {
            using (PlexingFleetDataContext context = new PlexingFleetDataContext(ConnectionString))
            {
                LoadPlexingPeriodPayoutData(e.CommandArgument.ToString().ToInt());
                FillPlexingPeriodData(e.CommandArgument.ToString().ToInt());
                ShowEndPeriodButton(!context.PlexingPeriods.First(x => x.PlexingPeriodId == e.CommandArgument.ToString().ToInt()).ToDate.HasValue);
                PlexingPeriodInfoUpdatePanel.Update();
            }
        }
    }

    protected void DisplayPlexingDate(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "DisplayDatePlexes")
        {
            string[] command = e.CommandArgument.ToString().Split('|');

            LoadPlexingPeriodPayoutData(command[0].ToInt());
            FillPlexingPeriodDateData(command[0].ToInt(), DateTime.Parse(command[1]));
            ShowEndPeriodButton(LastPeriodId == command[0].ToInt());
            PlexingPeriodInfoUpdatePanel.Update();
        }
    }

    protected void PlexingPeriodInfoGridView_RowCommand(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "ShowInfo")
        {
            Response.Redirect(string.Format("{0}?PlexingPeriodId={1}&CharacterName={2}", PageReferrer.Page_User_PlexingPeriod, PlexingPeriodId.Value, e.CommandArgument.ToString()));
        }
    }
    protected void SaveLinkButton_Click(object sender, EventArgs e)
    {
        CalculatePayout(true);
    }
    protected void EndPeriodLinkButton_Click(object sender, EventArgs e)
    {
        CalculatePayout(true);

        using (PlexingFleetDataContext context = new PlexingFleetDataContext(ConnectionString))
        {
            PlexingPeriod period = context.PlexingPeriods.FirstOrDefault(x => x.PlexingPeriodId == PlexingPeriodId);

            if (period != null)
            {
                period.ToDate = DateTime.Now;
            }

            context.SubmitChanges();

            ShowEndPeriodButton(false);
        }
    }
}