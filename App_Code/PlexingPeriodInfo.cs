using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PlexingPeriodInfo
/// </summary>
public class PlexingPeriodInfo : IComparable
{
    public int PlexId { get; set; }
    public int? CharacterId { get; set; }
    public string Name { get; set; }
    public int Plexes { get; set; }

    private double points;
    public double Points 
    {
        get { return Math.Round(points, 2); }
        set { points = value; } 
    }

    private double payout;
    public double Payout
    {
        get { return Math.Round(payout, 0); }
        set { payout = value; }
    }

    public PlexingPeriodInfo()
	{
	}

    #region IComparable Members

    public int CompareTo(object obj)
    {
        PlexingPeriodInfo compareTo = (PlexingPeriodInfo)obj;
        if (Points == compareTo.Points)
        {
            return Name.CompareTo(compareTo.Name);
        }
        else
            return compareTo.Points.CompareTo(Points);
    }

    #endregion
}

public class MyPlexingPeriodInfo : IComparable
{
    public int PlexId { get; set; }
    public string PlexName { get; set; }
    public string FC { get; set; }
    public DateTime PlexingDate { get; set; }
    public int Participants { get; set; }
    public double Payout { get; set; }

    private double points;
    public double Points 
    { 
        get { return Math.Round(points, 2); }
        set { points = value; } 
    }

    #region IComparable Members

    public int CompareTo(object obj)
    {
        MyPlexingPeriodInfo compareTo = (MyPlexingPeriodInfo)obj;
        if (PlexingDate == compareTo.PlexingDate)
        {
            return Points.CompareTo(compareTo.Points);
        }
        else
            return compareTo.PlexingDate.CompareTo(PlexingDate);
    }

    #endregion
}