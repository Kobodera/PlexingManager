using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EfficiencyInfo
/// </summary>
public class EfficiencyInfo
{
    public DateTime PlexingDate { get; set; }
    public string FCName { get; set; }
    public string Participants { get; set; }
    public int Points { get; set; }
}

public class PlexingSession
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int TotalPlexes { get; set; }
    public int TotalPoints { get; set; }
    public int TotalParticipants { get; set; }
    public string FCs { get; set; }

    public double PointsPerHour
    {
        get
        {
            TimeSpan ts = ToDate - FromDate;

            return Math.Round((TotalPoints / (ts.TotalHours + (ts.TotalHours / TotalPlexes))), 2);
        }
    }

    public double AverageParticipants
    {
        get
        {
            return Math.Round((((double)TotalParticipants) / TotalPlexes), 2);
        }
    }
}