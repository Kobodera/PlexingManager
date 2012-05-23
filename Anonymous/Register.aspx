<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="Register.aspx.cs"
    Inherits="Anonymous_Register" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Register user</h2>
    <p>
        Register new user</p>
    
    <p>
        In order to register you must do so through the ingame browser with added trust. When you register you consent that information about your character and your characters 
        corporation and alliance affiliation will be stored in the Plexing Manager database. 
        </p>
    <asp:Panel ID="RegisterPanel" runat="server">
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Username:"></asp:Label>
                </td>
                <td style="width: 10px;">
                </td>
                <td>
                    <asp:Label ID="UsernameLabel" runat="server" Text=""></asp:Label>
                </td>
                <td style="width: 100px;" ></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Password:"></asp:Label>
                </td>
                <td style="width: 10px;">
                </td>
                <td>
                    <asp:TextBox ID="PasswordTextBox" TextMode="Password" runat="server"></asp:TextBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Confirm password:"></asp:Label>
                </td>
                <td style="width: 10px;">
                </td>
                <td>
                    <asp:TextBox ID="ConfirmPasswordTextBox" TextMode="Password" runat="server"></asp:TextBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="PasswordMismatchLabel" runat="server" CssClass="failureNotification" Text="Password is less than 6 characters or does not match" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="text-align: right;">
                    <asp:LinkButton ID="RegisterLinkButton" runat="server" OnClick="RegisterLinkButton_Click">Register</asp:LinkButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Label ID="ErrorLabel" runat="server" CssClass="failureNotification" Text="You are not part of an alliance, corporation or character that are permitted to register." Visible="false"></asp:Label>
 </asp:Content>
