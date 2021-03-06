﻿<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="EditUser.aspx.cs" Inherits="Authenticated_Admin_EditUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <table>
        <tr>
            <td style="vertical-align: top;">
                <table>
                    <tr valign="top";>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Character ID:"></asp:Label>
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td>
                            <asp:Label ID="CharacterIdLabel" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
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
                            <asp:DropDownList ID="CorporationDropDownList" runat="server">
                            </asp:DropDownList>
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
                            <asp:CheckBox ID="BannedCheckBox" runat="server" Text="Inactive" />
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
                            <asp:TextBox ID="NewPasswordTextBox" runat="server" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Confirm password:"></asp:Label>
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td>
                            <asp:TextBox ID="ConfirmPasswordTextBox" runat="server" TextMode="Password"></asp:TextBox>
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
                    <tr id="SuperAdminRow" runat="server">
                        <td style="width:120px;">
                            <nobr><asp:CheckBox ID="SuperAdminCheckBox" runat="server" Text="Super Admin" /></nobr>
                        </td>
                    </tr>
                    <tr id="AdminRow" runat="server">
                        <td>
                            <nobr><asp:CheckBox ID="AdministratorCheckBox" runat="server" Text="Admin" /></nobr>
                        </td>
                    </tr>
                    <tr id="AllianceAdminRow" runat="server">
                        <td>
                            <nobr><asp:CheckBox ID="AllianceCheckBox" runat="server" Text="Alliance Admin" /></nobr>
                        </td>
                    </tr>
                    <tr id="CorpAdminRow" runat="server">
                        <td>
                            <nobr><asp:CheckBox ID="CorporationCheckBox" runat="server" Text="Corp Admin" /></nobr>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width:10px;">
            </td>
            <td style="width:280px;" rowspan="2" valign="top">
                <div style="width: 100%; height: 200px; overflow: auto; vertical-align: top;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Plexes:"></asp:Label></td>
                            <td style="width:10px;"></td>
                            <td>
                                <asp:Label ID="PlexesLabel" runat="server" Text="Plexes"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="Occations:"></asp:Label></td>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="OccationsLabel" runat="server" Text="Occations"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Last plex:"></asp:Label></td>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="LastPlexLabel" runat="server" Text="Last plex"></asp:Label></td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr valign="top">
            <td colspan="3" style="text-align:right;">
                <asp:LinkButton ID="UpdateLinkButton" runat="server" 
                    onclick="UpdateLinkButton_Click">Update</asp:LinkButton>&nbsp;
                <asp:LinkButton ID="CancelLinkButton" runat="server" 
                    onclick="CancelLinkButton_Click">Cancel</asp:LinkButton>
            </td>
        </tr>
    </table>
</asp:Content>
