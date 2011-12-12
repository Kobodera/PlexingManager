<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Payout.aspx.cs"
    Inherits="Authenticated_Admin_Payout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div style="width: 100%; height: 380px;">
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
                <td style="width: 25%" rowspan="2">
                    <div style="width: 100%; height: 330px; overflow: auto;">
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
                <td>
                    <table width="100%">
                        <tr>
                            <td style="width: 125px;">
                                <asp:Label ID="Label3" runat="server" Text="Payout (m ISK):" ToolTip="The payout in millions of ISK"></asp:Label>
                            </td>
                            <td style="width: 10px;">
                            </td>
                            <td style="width: 130px;">
                                <asp:TextBox ID="PayoutTextBox" runat="server" AutoPostBack="true" OnTextChanged="PayoutChanged"></asp:TextBox>
                            </td>
                            <td style="width: 10px;">
                            </td>
                            <td style="width: 85px;">
                                <asp:Label ID="Label7" runat="server" Text="Total (m ISK:)"></asp:Label>
                            </td>
                            <td style="width: 10px;">
                            </td>
                            <td>
                                <asp:UpdatePanel ID="TotalPayOutUpdatePanel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="TotalPayoutLabel" runat="server" Text=""></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td rowspan="2" style="text-align: right; vertical-align: top; width: 110px;">
                                <asp:LinkButton ID="SaveLinkButton" runat="server" Visible="false" OnClick="SaveLinkButton_Click">Save changes</asp:LinkButton>
                                <asp:LinkButton ID="EndPeriodLinkButton" runat="server" OnClick="EndPeriodLinkButton_Click">End Period</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Corp Tax (%):"></asp:Label>
                            </td>
                            <td style="width: 10px;">
                            </td>
                            <td>
                                <asp:TextBox ID="CorpTaxTextBox" runat="server" AutoPostBack="true" OnTextChanged="PayoutChanged"></asp:TextBox>
                            </td>
                            <td style="width: 10px;">
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Tax (m ISK):"></asp:Label>
                            </td>
                            <td style="width: 10px;">
                            </td>
                            <td>
                                <asp:UpdatePanel ID="TaxUpdatePanel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="TaxLabel" runat="server" Text=""></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="ISK/Point (m):"></asp:Label>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="IskPerPointUpdatePanel" runat="server">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="PayoutTextBox" EventName="TextChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="CorpTaxTextBox" EventName="TextChanged" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:Label ID="IskPerPointLabel" runat="server" Text=""></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="ISK/Point AT (m):" ToolTip="ISK/Point after tax has been deducted"></asp:Label>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="IskPerPointAfterTaxUpdatePanel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="IskPerPointAfterTaxLabel" runat="server" Text=""></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td style="text-align: right;">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 100%">
                    <div style="width: 100%; height: 230px; overflow: auto;">
                        <asp:UpdatePanel ID="PlexingPeriodInfoUpdatePanel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="PlexingPeriodInfoGridView" runat="server" Width="100%" AutoGenerateColumns="False"
                                    EnableModelValidation="True" OnRowCommand="PlexingPeriodInfoGridView_RowCommand">
                                    <AlternatingRowStyle BackColor="#202020" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="Label1" runat="server" Text='<%# Bind("Name") %>' CommandName="ShowInfo"
                                                    CommandArgument='<%# Bind("Name") %>'></asp:LinkButton>
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
                                        <asp:TemplateField HeaderText="Payout (m)">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# FormatMillions((int?)Eval("CharacterId"), Eval("Payout").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
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
