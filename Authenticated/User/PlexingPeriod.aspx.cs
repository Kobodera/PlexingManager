using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;

public partial class Authenticated_PlexingPeriod_PlexingPeriod : PageBase
{
    private string CharName
    {
        get
        {
            if (ViewState["VS_CharName"] == null)
                return CharacterName;

            return (string)ViewState["VS_CharName"];
        }
        set
        {
            ViewState["VS_CharName"] = value;
        }
    }

    private int PlexingPeriodId
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

    private DateTime? SelectedDate
    {
        get
        {
            return (DateTime?)ViewState["VS_SelectedDate"];
        }
        set
        {
            ViewState["VS_SelectedDate"] = value;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BackLinkButton.Visible = false;

            if (!string.IsNullOrEmpty(Request.QueryString["CharacterName"]))
            {
                BackLinkButton.Visible = true;
                CharName = Request.QueryString["CharacterName"];
            }

            NameLabel.Text = CharName;

            if (string.IsNullOrEmpty(Request.QueryString["PlexingPeriodId"]))
                FillPlexingPeriods(null);
            else
                FillPlexingPeriods(Request.QueryString["PlexingPeriodId"].ToInt());
        }
    }

    public string GetPlexingPeriod(DateTime? fromDate, DateTime? toDate, string corpTag)
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

    public string GetPlexingPeriodTitle(DateTime? fromDate, DateTime? toDate, string corpTag)
    {
        if (toDate.HasValue)
        {
            return string.Format("{0:d} - {1:d} - ({2})", fromDate, toDate, corpTag);
        }
        else
        {
            return string.Format("{0:d} - ({1})", fromDate, corpTag);
        }
    }

    private void FillPlexingPeriods(int? plexingPeriodId)
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            var plexingPeriods = from p in context.PlexingPeriods
                                 orderby p.FromDate descending
                                 select new PlexingPeriodListInfo() { PlexingPeriodId = p.PlexingPeriodId, FromDate = p.FromDate, ToDate = p.ToDate, CorpId = p.CorpId, CorpTag = GetCorpTag(p.CorpId) };

            List<PlexingPeriodListInfo> periods = plexingPeriods.ToList<PlexingPeriodListInfo>();

            foreach (var period in periods)
            {
                var plexingDates = from p in context.Plexes
                                   where p.PlexingPeriodId == period.PlexingPeriodId && p.Participants.Contains(CharName)
                                   orderby p.PlexingDate descending
                                   select p;

                Dictionary<string, DateInfos> infos = new Dictionary<string,DateInfos>();

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
                    //FillPlexingPeriodData(plexingPeriods.First().PlexingPeriodId);
                    //PlexingPeriodId = plexingPeriods.First().PlexingPeriodId;
                    if (plexingPeriods.FirstOrDefault(x => x.CorpId == CorpId) != null)
                        FillPlexingPeriodData(plexingPeriods.FirstOrDefault(x => x.CorpId == CorpId).PlexingPeriodId);
                    else
                        FillPlexingPeriodData(plexingPeriods.First().PlexingPeriodId);

                    PlexingPeriodId = plexingPeriods.First().PlexingPeriodId;
                    //LastPlexingPeriodId = PlexingPeriodId;
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
                PlexingPeriodDateLabel.Text = GetPlexingPeriodTitle(pp.FromDate, pp.ToDate, GetCorpTag(pp.CorpId));
            }
        }

        List<MyPlexingPeriodInfo> infos = LoadPlexingPeriodData(plexingPeriodId);

        PlexingPeriodInfoGridView.DataSource = infos;
        PlexingPeriodInfoGridView.DataBind();
    }

    private List<MyPlexingPeriodInfo> LoadPlexingPeriodData(int plexingPeriodId)
    {
        List<MyPlexingPeriodInfo> result = new List<MyPlexingPeriodInfo>();

        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            var plexPointinfos = from p in context.Plexes
                                 join pi in context.PlexInfos on p.PlexInfoId equals pi.PlexId
                                 join u in context.PlexUsers on p.FCId equals u.CharacterId
                                 where p.PlexingPeriodId == plexingPeriodId && p.Participants.Contains(CharName)
                                 select new { p.PlexId, pi.Name, u.CharacterName, p.PlexingDate, p.Participants, pi.Points };

            //Dictionary<string, PlexingPeriodInfo> dict = new Dictionary<string, PlexingPeriodInfo>();

            double totalpoints = 0;

            foreach (var plexPointInfo in plexPointinfos)
            {
                string[] pilots = plexPointInfo.Participants.Split(',');

                double pilotPoints = ((double)plexPointInfo.Points) / pilots.Count();
                totalpoints += pilotPoints;

                result.Add(new MyPlexingPeriodInfo() { PlexId = plexPointInfo.PlexId, PlexName = plexPointInfo.Name, PlexingDate = plexPointInfo.PlexingDate.Value, FC = plexPointInfo.CharacterName, Points = pilotPoints, Participants = pilots.Count() });
            }

            TotalPointsLabel.Text = Math.Round(totalpoints, 2).ToString();

            result.Sort();

            return result;
        }
    }

    protected void DisplayPlexingPeriod(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "DisplayPlexingPeriod")
        {
            SelectedDate = null;
            FillPlexingPeriodData(e.CommandArgument.ToString().ToInt());
        }
    }

    protected void DisplayPlexingDate(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "DisplayDatePlexes")
        {
            string[] command = e.CommandArgument.ToString().Split('|');

            SelectedDate = DateTime.Parse(command[1]);
            FillPlexingPeriodDateData(command[0].ToInt(), SelectedDate.Value);
        }
    }

    private void FillPlexingPeriodDateData(int plexingPeriodId, DateTime date)
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            PlexingPeriod pp = context.PlexingPeriods.FirstOrDefault(x => x.PlexingPeriodId == plexingPeriodId);

            if (pp != null)
            {
                PlexingPeriodDateLabel.Text = GetPlexingPeriodTitle(pp.FromDate, pp.ToDate, GetCorpTag(pp.CorpId));
            }
        }

        List<MyPlexingPeriodInfo> infos = LoadPlexingPeriodData(plexingPeriodId, date);

        PlexingPeriodInfoGridView.DataSource = infos;
        PlexingPeriodInfoGridView.DataBind();
    }

    private List<MyPlexingPeriodInfo> LoadPlexingPeriodData(int plexingPeriodId, DateTime date)
    {
        List<MyPlexingPeriodInfo> result = new List<MyPlexingPeriodInfo>();

        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            DateTime lastDate = new DateTime(date.Year, date.Month, date.Day).AddDays(1);

            var plexPointinfos = from p in context.Plexes
                                 join pi in context.PlexInfos on p.PlexInfoId equals pi.PlexId
                                 join u in context.PlexUsers on p.FCId equals u.CharacterId
                                 where p.PlexingPeriodId == plexingPeriodId && p.Participants.Contains(CharName) && p.PlexingDate >= date && p.PlexingDate <= lastDate
                                 select new { p.PlexId, pi.Name, u.CharacterName, p.PlexingDate, p.Participants, pi.Points };


            double totalpoints = 0;

            foreach (var plexPointInfo in plexPointinfos)
            {
                string[] pilots = plexPointInfo.Participants.Split(',');

                double pilotPoints = ((double)plexPointInfo.Points) / pilots.Count();

                totalpoints += pilotPoints;

                result.Add(new MyPlexingPeriodInfo() { PlexId = plexPointInfo.PlexId, PlexName = plexPointInfo.Name, PlexingDate = plexPointInfo.PlexingDate.Value, FC = plexPointInfo.CharacterName, Points = pilotPoints, Participants = pilots.Count() });
            }

            TotalPointsLabel.Text = Math.Round(totalpoints, 2).ToString();

            result.Sort();

            return result;
        }
    }

    protected void BackLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(string.Format("{0}?PlexingPeriodId={1}", PageReferrer.Page_PlexingPeriod_PlexingPeriod, PlexingPeriodId));
    }

    protected void PlexingPeriodInfoGridView_RowCommand(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "EditPlex")
        {
            Response.Redirect(string.Format("{0}?PlexId={1}&ReturnUrl={2}&PlexingPeriodId={3}&CharacterName={4}", PageReferrer.Page_FC_SelectMembers, e.CommandArgument.ToString(), PageReferrer.Page_User_PlexingPeriod, PlexingPeriodId, CharName));
        }
        else if (e.CommandName == "DeletePlex")
        {
            int plexId = e.CommandArgument.ToString().ToInt();

            using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
            {
                Plex p = context.Plexes.FirstOrDefault(x => x.PlexId == plexId);

                if (p != null)
                {
                    p.Participants = p.Participants.Replace(string.Format("{0}, ", CharName), "").Replace(string.Format(", {0}", CharName), "");
                    p.Participants = p.Participants.Replace(CharName, "");

                    if (string.IsNullOrEmpty(p.Participants.Trim()))
                        context.Plexes.DeleteOnSubmit(p);
                    context.SubmitChanges();
                }

                if (SelectedDate.HasValue)
                {
                    FillPlexingPeriodDateData(PlexingPeriodId, SelectedDate.Value);
                }
                else
                {
                    FillPlexingPeriodData(PlexingPeriodId);
                }
            }
        }

    }
}