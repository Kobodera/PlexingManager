using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PlexingPeriodListInfo
/// </summary>
public class PlexingPeriodListInfo
{
    public int PlexingPeriodId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int CorpId { get; set; }
    public string CorpTag { get; set; }

    public List<DateInfos> DateInfos { get; set; }

    public PlexingPeriodListInfo()
	{
        DateInfos = new List<DateInfos>();
	}
}

public class DateInfos
{
    public int PlexingPeriodId { get; set; }
    public DateTime Date { get; set; }
    public int PlexCount { get; set; }
}