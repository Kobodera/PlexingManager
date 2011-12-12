<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="Login.aspx.cs"
    Inherits="Anonymous_Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Log In
    </h2>
    <p>
        Please enter your username and password.
    </p>
    <asp:Panel ID="LoginPanel" runat="server" DefaultButton="LoginButton">
    <table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Username:"></asp:Label>
            </td>
            <td style="width:10px;">
            </td>
            <td>
                <asp:TextBox ID="UsernameTextBox" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Password:"></asp:Label>
            </td>
            <td style="width:10px;">
            </td>
            <td>
                <asp:TextBox ID="PasswordTextBox" TextMode="Password" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:CheckBox ID="RememberMeCheckBox" runat="server" Text="Remember me" />
            </td>
        </tr>
        <tr>
            <td colspan="3" style="text-align:right;">
                <asp:Button ID="LoginButton" runat="server" Text="Button" 
                    UseSubmitBehavior="false" onclick="LoginButton_Click" />
                <asp:LinkButton ID="LoginLinkButton" runat="server" 
                    onclick="LoginLinkButton_Click" >Log in</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label ID="PasswordLabel" runat="server" Text="Wrong username or password" CssClass="failureNotification" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:Label ID="ErrorLabel" runat="server" CssClass="failureNotification" Text="You are either not registered or allowed to log in." Visible="false"></asp:Label>
</asp:Content>
