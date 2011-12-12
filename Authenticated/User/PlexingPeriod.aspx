<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="PlexingPeriod.aspx.cs" Inherits="Authenticated_PlexingPeriod_PlexingPeriod" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div style="width: 100%;">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="text-align:right;">
                        &nbsp;<asp:LinkButton ID="BackLinkButton" runat="server" Visible="false" 
                            onclick="BackLinkButton_Click">Back</asp:LinkButton>
                    </td>
                </tr>
            </table>
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <h3>
                        <asp:Label ID="NameLabel" runat="server" Text="Label"></asp:Label>&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Plexing period:"></asp:Label>&nbsp;<asp:Label
                            ID="PlexingPeriodDateLabel" runat="server" Text="Label"></asp:Label>
                    </h3>
                </td>
                <td style="text-align: right">
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
                    <div style="width: 100%; height: 320px; overflow: auto;">
                        <asp:DataList ID="PlexingPeriodsDataList" runat="server" Width="100%">
                            <ItemTemplate>
                                <asp:LinkButton ID="PlexingPeriodLinkButton" runat="server" Text='<%# GetPlexingPeriod((DateTime?)Eval("FromDate"), (DateTime?)Eval("ToDate")) %>'
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
                    <div style="width: 100%; height: 320px; overflow: auto;">
                        <asp:GridView ID="PlexingPeriodInfoGridView" runat="server" Width="100%" AutoGenerateColumns="False"
                            EnableModelValidation="True" OnRowCommand="PlexingPeriodInfoGridView_RowCommand">
                            <AlternatingRowStyle BackColor="#202020" />
                            <Columns>
                                <asp:TemplateField HeaderText="Plex">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("PlexName") %>' Visible='<%# !IsAdmin %>'></asp:Label><asp:LinkButton
                                            ID="LinkButton1" runat="server" Text='<%# Bind("PlexName") %>' CommandName="EditPlex" CommandArgument='<%# Bind("PlexId") %>' Visible='<%# IsAdmin %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("PlexName") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="PlexingDate" HeaderText="Date" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="FC" HeaderText="FC" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Participants" HeaderText="Pilots"
                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" >
<HeaderStyle HorizontalAlign="Right"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Points" HeaderText="Points" HeaderStyle-HorizontalAlign="Right"
                                    ItemStyle-HorizontalAlign="Right" >
<HeaderStyle HorizontalAlign="Right"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="DeletePlex"
                                            Text="Delete" CommandArgument='<%# Eval("PlexId") %>' ImageUrl="~/Images/trashcan.gif" Visible='<%# IsAdmin %>' Width="16px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
