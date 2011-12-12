using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class Anonymous_Test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var head in Request.Headers)
        {
            sb.AppendLine(string.Format("{0} = {1}", head.ToString(), Request.Headers[head.ToString()]));
        }

        TextBox1.Text = sb.ToString();
    }
}