<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="AccessRequest.aspx.cs" Inherits="Anonymous_AccessRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <h2>
        Plexing Manager Access Request</h2>
    <asp:Panel ID="InfoPanel" runat="server">
        <p><asp:Label ID="Label6" runat="server" Text="You either need to request access to the Plexing Manager through the ingame browser or your user is disabled. If you cant see any registration
                information even if you have set trust and use the IGB then try removing trust, close the IGB and try again. For some reason the IGB trust system
                is not totally reliable. Retrying usually fixes the problem however."></asp:Label>
            </p>
    </asp:Panel>
    <asp:Panel ID="RequestPanel" runat="server">
        <table>
            <tr>
                <td colspan="3">
                    <h3><asp:Label ID="Label11" runat="server" Text="User information"></asp:Label></h3>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Requested by:"></asp:Label>
                </td>
                <td style="width: 10px;">
                </td>
                <td>
                    <asp:Label ID="RequestedByLabel" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr runat="server" id="PasswordRow">
                <td>
                    <asp:Label ID="Label9" runat="server" Text="Password:"></asp:Label>
                </td>
                <td style="width: 10px;">
                </td>
                <td>
                    <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr runat="server" id="ConfirmPasswordRow">
                <td>
                    <asp:Label ID="Label10" runat="server" Text="Confirm:"></asp:Label>
                </td>
                <td style="width: 10px;">
                </td>
                <td>
                    <asp:TextBox ID="ConfirmTextBox" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <h3><asp:Label ID="Label12" runat="server" Text="Corporatioon information"></asp:Label></h3>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Corporation:"></asp:Label>
                </td>
                <td style="width: 10px;">
                </td>
                <td>
                    <asp:Label ID="CorpNameLabel" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="Corp ticker:"></asp:Label>
                </td>
                <td style="width: 10px;">
                </td>
                <td>
                    <asp:TextBox ID="CorpTickerTextBox" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="* Corp ticker is not supplied by IGB trust so you will have to set that manually."></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Alliance:"></asp:Label>
                </td>
                <td style="width: 10px;">
                </td>
                <td>
                    <asp:Label ID="AllianceNameLabel" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Alliance ticker:"></asp:Label>
                </td>
                <td style="width: 10px;">
                </td>
                <td>
                    <asp:TextBox ID="AllianceTickerTextBox" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="* Alliance ticker is not supplied by IGB trust so you may have to set that manually."></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="text-align: right">
                    <asp:LinkButton ID="RequestLinkButton" runat="server" 
                        onclick="RequestLinkButton_Click">Request Access</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="PasswordMismatchLabel" runat="server" CssClass="failureNotification" Text="Password is less than 6 characters or does not match" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>

</asp:Content>
