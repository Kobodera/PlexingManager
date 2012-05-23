<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="PlexingPeriod.aspx.cs" Inherits="Authenticated_PlexingPeriod_PlexingPeriod" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div style="width: 100%;">
        <table width="100%">
            <tr>
                <td>
                    <h3>
                        <asp:Label ID="Label1" runat="server" Text="Plexing period:"></asp:Label>&nbsp;<asp:Label
                            ID="PlexingPeriodDateLabel" runat="server" Text="Label"></asp:Label>
                    </h3>
                </td>
                <td style="text-align: right;">
                    <h3>
                        <asp:Label ID="Label2" runat="server" Text="Total Points:"></asp:Label>&nbsp;<asp:Label
                            ID="TotalPointsLabel" runat="server" Text="Label"></asp:Label>
                    </h3>
                </td>
            </tr>
        </table>
        <table style="width: 100%" border="1">
            <tr>
                <td style="width: 25%">
                    <div style="width: 100%; height: 330px; overflow: auto;">
                        <asp:DataList ID="PlexingPeriodsDataList" runat="server" Width="100%">
                            <ItemTemplate>
                                <asp:LinkButton ID="PlexingPeriodLinkButton" runat="server" Text='<%# GetPlexingPeriod((DateTime?)Eval("FromDate"), (DateTime?)Eval("ToDate"), (string)Eval("CorpTag")) %>'
                                    CommandName="DisplayPlexingPeriod" CommandArgument='<%# Eval("PlexingPeriodId") %>'
                                    OnCommand="DisplayPlexingPeriod">LinkButton</asp:LinkButton>
                                <asp:DataList ID="DateDataList" runat="server" DataSource='<%# Eval("DateInfos") %>'>
                                    <ItemTemplate>
                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="DateLinkButton" runat="server" Text='<%# string.Format("{0:d}", (DateTime)Eval("Date")) %>'
                                            CommandName="DisplayDatePlexes" CommandArgument='<%# string.Format("{0}|{1}", Eval("PlexingPeriodId"), Eval("Date")) %>'
                                            OnCommand="DisplayPlexingDate"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </td>
                <td style="width: 100%">
                    <div style="width: 100%; height: 330px; overflow: auto;">
                        <asp:GridView ID="PlexingPeriodInfoGridView" runat="server" Width="100%" AutoGenerateColumns="False"
                            EnableModelValidation="True" OnRowCommand="PlexingPeriodInfoGridView_RowCommand">
                            <AlternatingRowStyle BackColor="#202020" />
                            <Columns>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="Label1" runat="server" Text='<%# Bind("Name") %>' CommandName="ShowInfo"
                                            CommandArgument='<%# string.Format("{0}|{1}", Eval("PlexingPeriodId"), Eval("Name")) %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Plexes" HeaderText="Plexes" HeaderStyle-HorizontalAlign="Right"
                                    ItemStyle-HorizontalAlign="Right">
                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Points" HeaderText="Points" HeaderStyle-HorizontalAlign="Right"
                                    ItemStyle-HorizontalAlign="Right">
                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Payout">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# FormatMillions(Eval("Payout").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
