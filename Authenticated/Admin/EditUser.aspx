<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="EditUser.aspx.cs" Inherits="Authenticated_Admin_EditUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <table>
        <tr>
            <td style="vertical-align: top;">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Character name:"></asp:Label>
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td>
                            <asp:Label ID="CharacterNameLabel" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Corporation:"></asp:Label>
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td>
                            <asp:Label ID="CorporationLabel" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Alliance"></asp:Label>
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td>
                            <asp:Label ID="AllianceLabel" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:CheckBox ID="BannedCheckBox" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <h3>
                                <asp:Label ID="Label4" runat="server" Text="Change password:"></asp:Label></h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="New password:"></asp:Label>
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td>
                            <asp:TextBox ID="NewPasswordTextBox" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Confirm password:"></asp:Label>
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td>
                            <asp:TextBox ID="ConfirmPasswordTextBox" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width:10px"></td>
            <td style="vertical-align: top;">
                <table>
                    <tr>
                        <td>
                        <h3>
                            <asp:Label ID="Label7" runat="server" Text="Permissions"></asp:Label></h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <asp:CheckBox ID="AdministratorCheckBox" runat="server" Text="Administrator" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="FCCheckBox" runat="server" Text="Fleet Commander" Visible="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="text-align:right;">
                <asp:LinkButton ID="LinkButton1" runat="server">Update</asp:LinkButton>&nbsp;
                <asp:LinkButton ID="LinkButton2" runat="server">Cancel</asp:LinkButton>
            </td>
        </tr>
    </table>
</asp:Content>
