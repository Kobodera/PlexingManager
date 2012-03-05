<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Fleet.aspx.cs" Inherits="Authenticated_FC_Fleet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <table width="100%" border="1">
        <tr>
            <td style="width: 30%; vertical-align: top">
                <div style="width: 100%;">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:LinkButton ID="UpdateFleetLinkButton" runat="server" OnClick="UpdateFleetLinkButton_Click">Edit fleet</asp:LinkButton>
                            </td>
                            <td style="text-align: right">
                                <asp:LinkButton ID="RemoveSelectedLinkButton" runat="server" OnClick="RemoveSelectedLinkButton_Click">Remove selected</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 100%; height: 350px; background-color: #202020; overflow: auto;
                    vertical-align: top;">
                    <asp:UpdatePanel ID="FleetUpdatePanel" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="RemoveSelectedLinkButton" EventName="Click" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:ListBox ID="PilotListBox" runat="server" SelectionMode="Multiple" 
                                Width="100%" Height="350px" BackColor="#202020" ForeColor="White"></asp:ListBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
            <td style="vertical-align: top;">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:DropDownList ID="PlexCorpDropDownList" runat="server">
                            </asp:DropDownList>&nbsp;
                        </td>
                        <td>
                            <asp:DropDownList ID="PlexInfoDropDownList" runat="server">
                            </asp:DropDownList>&nbsp;<asp:LinkButton ID="AddPlexLinkButton" runat="server" 
                                onclick="AddPlexLinkButton_Click">Add plex</asp:LinkButton>
                        </td>
                        <td style="text-align:right;">
                                <b>Plexing Period: <asp:Label ID="PlexingPeriodLabel" runat="server" Text="Label"></asp:Label></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="vertical-align: top;">
                            <asp:UpdatePanel ID="PlexInfoUpdatePanel" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="AddPlexLinkButton" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <div style="width:100%; height:340px; overflow:auto;">
                                    <asp:GridView ID="PlexGridView" runat="server" Width="100%" AutoGenerateColumns="False"
                                        EnableModelValidation="True" OnRowCommand="PlexGridView_RowCommand">
                                        <AlternatingRowStyle BackColor="#202020" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Plex" ShowHeader="False" HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" 
                                                        CommandName="EditPlex" Text='<%# Eval("PlexName") %>' 
                                                        CommandArgument='<%# Eval("PlexId") %>' onclick="LinkButton1_Click"></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PlexingDate" HeaderText="Date" HeaderStyle-Width="70px">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PointsAwarded" HeaderText="Points" HeaderStyle-Width="50px" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Participants" HeaderText="Participants" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CorpTag" HeaderText="Corp" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" 
                                                        CommandName="DeletePlex" Text="Delete" 
                                                        CommandArgument='<%# Eval("PlexId") %>' ImageUrl="~/Images/trashcan.gif" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
