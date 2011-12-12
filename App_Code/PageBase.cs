using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for PageBase
/// </summary>
public class PageBase : System.Web.UI.Page
{
    public PageBase()
    {
    }

    //EVE_TRUSTED
    public bool EveTrusted
    {
        get
        {
            if (Session["EVE_TRUSTED"] == null)
            {
                if (Request.Headers["EVE_TRUSTED"] != null)
                    Session["EVE_TRUSTED"] = Request.Headers["EVE_TRUSTED"].ToLower() == "yes";
                else
                    Session["EVE_TRUSTED"] = false;
            }

            return (bool)Session["EVE_TRUSTED"];
        }
        set
        {
            Session["EVE_TRUSTED"] = value;
        }
    }

    //EVE_CHARID
    public int CharacterId
    {
        get
        {
            if (Session["EVE_CHARID"] == null)
            {
                if (EveTrusted)
                {
                    Session["EVE_CHARID"] = Request.Headers["EVE_CHARID"].ToInt();
                }
                else
                    Session["EVE_CHARID"] = 0;
            }

            return (int)Session["EVE_CHARID"];
        }
        set
        {
            Session["EVE_CHARID"] = value;
        }
    }


    //EVE_CHARNAME
    public string CharacterName
    {
        get
        {
            if (Session["EVE_CHARNAME"] == null)
            {

                if (EveTrusted)
                    Session["EVE_CHARNAME"] = Request.Headers["EVE_CHARNAME"];
                //else
                //    Session["EVE_CHARNAME"] = string.Empty;
            }

            return (string)Session["EVE_CHARNAME"];
        }
        set
        {
            Session["EVE_CHARNAME"] = value;
        }
    }

    //EVE_CORPNAME
    public string CorpName
    {
        get
        {
            if (Session["EVE_CORPNAME"] == null)
            {

                if (EveTrusted)
                    Session["EVE_CORPNAME"] = Request.Headers["EVE_CORPNAME"];
                //else
                //    Session["EVE_CORPNAME"] = string.Empty;
            }

            return (string)Session["EVE_CORPNAME"];
        }
        set
        {
            Session["EVE_CORPNAME"] = value;
        }
    }

    //EVE_CORPID
    public int CorpId
    {
        get
        {
            if (Session["EVE_CORPID"] == null)
            {
                if (EveTrusted)
                {
                    Session["EVE_CORPID"] = Request.Headers["EVE_CORPID"].ToInt();
                }
                else
                    Session["EVE_CORPID"] = 0;
            }

            return (int)Session["EVE_CORPID"];
        }
        set
        {
            Session["EVE_CORPID"] = value;
        }
    }

    //EVE_ALLIANCENAME
    public string AllianceName
    {
        get
        {
            if (Session["EVE_ALLIANCENAME"] == null)
            {

                if (EveTrusted)
                    Session["EVE_ALLIANCENAME"] = Request.Headers["EVE_ALLIANCENAME"];
                else
                    Session["EVE_ALLIANCENAME"] = string.Empty;
            }

            return (string)Session["EVE_ALLIANCENAME"];
        }
        set
        {
            Session["EVE_ALLIANCENAME"] = value;
        }
    }

    //EVE_ALLIANCEID
    public int AllianceId
    {
        get
        {
            if (Session["EVE_ALLIANCEID"] == null)
            {
                if (EveTrusted)
                {
                    Session["EVE_ALLIANCEID"] = Request.Headers["EVE_ALLIANCEID"].ToInt();
                }
                else
                    Session["EVE_ALLIANCEID"] = 0;
            }

            return (int)Session["EVE_ALLIANCEID"];
        }
        set
        {
            Session["EVE_ALLIANCEID"] = value;
        }
    }

    //EVE_SOLARSYSTEMNAME
    public string SolarSystemName
    {
        get
        {
            if (Session["EVE_SOLARSYSTEMNAME"] == null)
            {

                if (EveTrusted)
                    Session["EVE_SOLARSYSTEMNAME"] = Request.Headers["EVE_SOLARSYSTEMNAME"];
                else
                    Session["EVE_SOLARSYSTEMNAME"] = string.Empty;
            }

            return (string)Session["EVE_SOLARSYSTEMNAME"];
        }
        set
        {
            Session["EVE_SOLARSYSTEMNAME"] = value;
        }
    }

    public bool IsAdmin
    {
        get
        {
            return User.IsInRole("Admin");
        }
    }

    public string ConnectionString
    {
        get
        {
            return WebConfigurationManager.ConnectionStrings["PlexManagerConnectionString"].ConnectionString;
        }
    }

    //    EVE_CHARNAME - Kobodera
    //EVE_CORPNAME - Selectus Pravus Lupus
    //EVE_CONSTELLATIONNAME - Unknown
    //EVE_CORPROLE - 1049477251775848448
    //EVE_CHARID - 134721779
    //EVE_CORPID - 340962950
    //EVE_ALLIANCENAME - Transmission Lost
    //EVE_SOLARSYSTEMNAME - J145735
    //EVE_REGIONNAME - Unknown
    //EVE_ALLIANCEID - 1178865538
    //EVE_TRUSTED - Yes
}