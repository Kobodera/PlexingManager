using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PlexInfo
/// </summary>
public class PlexListInfo
{
    public int PlexId { get; set; }
    public string PlexName { get; set; }
    public DateTime PlexingDate { get; set; }
    public double Points { get; set; }
    public string Participants { get; set; }
    public double Payout { get; set; }
    public string CorpTag { get; set; }
    public string FCName { get; set; }


    public double PointsAwarded
    {
        get { return Math.Round((Points / Participants.Split(',').Count()), 2); }
    }

	public PlexListInfo()
	{
	}
}