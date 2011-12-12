<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeFile="Users.aspx.cs" Inherits="Authenticated_Admin_Users" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Search"></asp:Label>
            </td>
            <td style="width: 10px;">
            </td>
            <td>
                <asp:TextBox ID="SearchTextBox" runat="server" OnTextChanged="TextChanged" AutoPostBack="True"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="SearchTextBox" EventName="TextChanged" />
                    </Triggers>
                    <ContentTemplate>
                        <div style="height:350px; overflow:auto; vertical-align:top;">
                        <asp:GridView ID="UsersGridView" runat="server" Width="100%" AutoGenerateColumns="False"
                            EnableModelValidation="True" OnRowCommand="UsersGridView_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Name" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CommandName="EditCharacter"
                                            CommandArgument='<%# Eval("CharacterId") %>' Text='<%# Eval("CharacterName") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="CorpName" HeaderText="Corporation">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Enabled" HeaderText="Active">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Roles" HeaderText="Roles">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="DeleteCharacter"
                                            Text="Delete" CommandArgument='<%# Eval("CharacterId") %>' ImageUrl="~/Images/trashcan.gif" />
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
</asp:Content>
