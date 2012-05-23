<%@ Page Title="Efficiency" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Efficiency.aspx.cs" Inherits="Authenticated_PlexingPeriod_Efficiency" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div style="width: 100%;">
        <table width="100%">
            <tr>
                <td>
                    <h3>
                        <asp:Label ID="Label1" runat="server" Text="Plexing efficiency the last"></asp:Label>
                        <asp:TextBox ID="DaysTextBox" runat="server" Width="30"></asp:TextBox>
                        <asp:Label ID="Label3" runat="server" Text="days"></asp:Label>
                        <asp:LinkButton ID="RefreshLinkButton" runat="server">Refresh</asp:LinkButton>
                    </h3>
                </td>
                <td style="text-align: right;">
                    <h3>
                        <asp:Label ID="Label2" runat="server" Text="Total Points:" Visible="false"></asp:Label>&nbsp;<asp:Label
                            ID="TotalPointsLabel" runat="server" Text="Label" Visible="false"></asp:Label>
                    </h3>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="width: 100%; height: 336px; overflow: auto;">
                        <asp:UpdatePanel ID="EfficiencyUpdatePanel" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="RefreshLinkButton" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:GridView ID="PlexingEfficiencyInfoGridView" runat="server" Width="100%" AutoGenerateColumns="False"
                                    EnableModelValidation="True">
                                    <AlternatingRowStyle BackColor="#202020" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Session">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# FormatDate((DateTime)Eval("FromDate"), (DateTime)Eval("ToDate")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FCs" HeaderText="FCs" HeaderStyle-HorizontalAlign="Right"
                                            ItemStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TotalPlexes" HeaderText="Plexes" HeaderStyle-HorizontalAlign="Right"
                                            ItemStyle-HorizontalAlign="Right">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TotalPoints" HeaderText="Points" HeaderStyle-HorizontalAlign="Right"
                                            ItemStyle-HorizontalAlign="Right">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AverageParticipants" HeaderText="Participants (Average)"
                                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PointsPerHour" HeaderText="Point/Hour" HeaderStyle-HorizontalAlign="Right"
                                            ItemStyle-HorizontalAlign="Right">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
