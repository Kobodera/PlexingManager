<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="Authenticated_User_ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Old password:"></asp:Label>
            </td>
            <td style="width:10px"></td>
            <td>
                <asp:TextBox ID="OldPasswordTextBox" runat="server" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="New password:"></asp:Label>
            </td>
            <td style="width:10px"></td>
            <td>
                <asp:TextBox ID="NewPasswordTextBox" runat="server" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Confirm password:"></asp:Label>
            </td>
            <td style="width:10px"></td>
            <td>
                <asp:TextBox ID="ConfirmPasswordTextBox" runat="server" TextMode="Password"></asp:TextBox>
            </td>
            <td>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*" 
                    ControlToCompare="NewPasswordTextBox" 
                    ControlToValidate="ConfirmPasswordTextBox"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="text-align:right">
                <asp:LinkButton ID="ChangePasswordLinkButton" runat="server" 
                    onclick="ChangePasswordLinkButton_Click">Change Password</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Label ID="SuccessLabel" CssClass="successNotification" runat="server" Text="Password changed!" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:Label ID="ErrorLabel" CssClass="failureNotification" runat="server" Text="Password is less than 6 characters or old/new passwords does not match" Visible="false"></asp:Label>
</asp:Content>

