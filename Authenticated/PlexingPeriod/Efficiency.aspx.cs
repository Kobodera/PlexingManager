using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Text;

public partial class Authenticated_PlexingPeriod_Efficiency : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            DaysTextBox.Text = "30";

        CalculateEfficiency(Request.QueryString["Admin"] == "1", Request.QueryString["Alliance"] == "1", Request.QueryString["Corp"] == "1");
    }

    private void CalculateEfficiency(bool fromAdmin, bool fromAlliance, bool fromCorp)
    {
        using (PlexingFleetDataContext context = new PlexingFleetDataContext(WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString))
        {
            if ((IsSuperAdmin || IsAdmin) && fromAdmin)
            {
                List<EfficiencyInfo> plexInfos = (from p in context.Plexes
                                                  join
                                                      pu in context.PlexUsers on p.FCId equals pu.CharacterId
                                                  join
                                                      pi in context.PlexInfos on p.PlexInfoId equals pi.PlexId
                                                  join
                                                      co in context.Corps on p.CorpId equals co.CorpId
                                                  where p.PlexingDate >= DateTime.Now.Subtract(new TimeSpan(DaysTextBox.Text.ToInt(), 0, 0, 0))
                                                  orderby p.PlexingDate descending
                                                  select new EfficiencyInfo() { PlexingDate = p.PlexingDate.Value, FCName = pu.CharacterName, Participants = p.Participants, Points = pi.Points }).ToList();
                LoadPlexingEfficiencyInfo(plexInfos);
            }
            else if (IsAllianceAdmin && fromAlliance)
            {
                List<EfficiencyInfo> plexInfos = (from p in context.Plexes
                                                  join
                                                      pu in context.PlexUsers on p.FCId equals pu.CharacterId
                                                  join
                                                      pi in context.PlexInfos on p.PlexInfoId equals pi.PlexId
                                                  join
                                                      co in context.Corps on p.CorpId equals co.CorpId
                                                  where p.PlexingDate >= DateTime.Now.Subtract(new TimeSpan(DaysTextBox.Text.ToInt(), 0, 0, 0)) && co.AllianceId == AllianceId && co.AllianceId != -1
                                                  orderby p.PlexingDate descending
                                                  select new EfficiencyInfo() { PlexingDate = p.PlexingDate.Value, FCName = pu.CharacterName, Participants = p.Participants, Points = pi.Points }).ToList();
                LoadPlexingEfficiencyInfo(plexInfos);
            }
            else if (IsCorpAdmin && fromCorp)
            {
                List<EfficiencyInfo> plexInfos = (from p in context.Plexes
                                                  join
                                                      pu in context.PlexUsers on p.FCId equals pu.CharacterId
                                                  join
                                                      pi in context.PlexInfos on p.PlexInfoId equals pi.PlexId
                                                  join
                                                      co in context.Corps on p.CorpId equals co.CorpId
                                                  where p.PlexingDate >= DateTime.Now.Subtract(new TimeSpan(DaysTextBox.Text.ToInt(), 0, 0, 0)) && co.CorpId == CorpId
                                                  orderby p.PlexingDate descending
                                                  select new EfficiencyInfo() { PlexingDate = p.PlexingDate.Value, FCName = pu.CharacterName, Participants = p.Participants, Points = pi.Points }).ToList();
                LoadPlexingEfficiencyInfo(plexInfos);
            }
        }
    }

    private void LoadPlexingEfficiencyInfo(List<EfficiencyInfo> plexInfos)
    {
        Dictionary<string, string> FCs = new Dictionary<string, string>();

        List<PlexingSession> plexingSessions = new List<PlexingSession>();

        if (plexInfos.Count > 1)
        {
            DateTime? start = null;
            DateTime stop = DateTime.Now;

            int totalPlexes = 0;
            int totalPoints = 0;
            int totalParticipants = 0;

            foreach (var plexInfo in plexInfos)
            {
                if (start == null)
                {
                    start = plexInfo.PlexingDate;
                    stop = plexInfo.PlexingDate;
                    totalPoints += plexInfo.Points;
                    totalPlexes += 1;
                    totalParticipants = plexInfo.Participants.Split(',').Count();

                    if (!FCs.ContainsKey(plexInfo.FCName))
                        FCs.Add(plexInfo.FCName, plexInfo.FCName);
                }
                else
                {
                    //Last plex was made more than an hour ago so its not part of the same session
                    if (plexInfo.PlexingDate < stop.Subtract(new TimeSpan(1, 0, 0)))
                    {
                        PlexingSession session = new PlexingSession()
                        {
                            TotalParticipants = totalParticipants,
                            TotalPlexes = totalPlexes,
                            TotalPoints = totalPoints,
                            FCs = GetFCs(FCs.Values.ToList()),
                            FromDate = stop,
                            ToDate = start.Value,

                        };

                        //If there is only one plex done there is no way to measure efficiency
                        if (session.TotalPlexes > 1)
                            plexingSessions.Add(session);

                        start = plexInfo.PlexingDate;
                        stop = plexInfo.PlexingDate;
                        totalPlexes = 1;
                        totalPoints = plexInfo.Points;
                        totalParticipants = plexInfo.Participants.Split(',').Count();
                        FCs.Clear();

                        if (!FCs.ContainsKey(plexInfo.FCName))
                            FCs.Add(plexInfo.FCName, plexInfo.FCName);
                    }
                    else
                    {
                        stop = plexInfo.PlexingDate;
                        totalPlexes += 1;
                        totalPoints += plexInfo.Points;
                        totalParticipants += plexInfo.Participants.Split(',').Count();

                        if (!FCs.ContainsKey(plexInfo.FCName))
                            FCs.Add(plexInfo.FCName, plexInfo.FCName);

                    }
                }
            }
        }

        PlexingEfficiencyInfoGridView.DataSource = plexingSessions;
        PlexingEfficiencyInfoGridView.DataBind();
    }

    private string GetFCs(List<string> fcs)
    {
        StringBuilder sb = new StringBuilder();

        foreach (string str in fcs)
        {
            if (sb.Length > 0)
                sb.Append(", ");

            sb.Append(str);
        }

        return sb.ToString();
    }

    protected string FormatDate(DateTime fromDate, DateTime toDate)
    {
        return string.Format("{0} {1} - {2} {3}", fromDate.ToShortDateString(), fromDate.ToShortTimeString(), toDate.ToShortDateString(), toDate.ToShortTimeString());
    }
}