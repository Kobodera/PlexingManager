<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="SelectMembers.aspx.cs" Inherits="Authenticated_FC_SelectMembers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <table>
        <tr>
            <td style="width: 200px;">
                <asp:TextBox ID="SearchTextBox" runat="server" Style="width: 230px;" OnTextChanged="TextChanged"
                    AutoPostBack="true">
                </asp:TextBox>
            </td>
            <td style="width: 10px;">
            </td>
            <td style="width: 100px;">
                <asp:LinkButton ID="AddGuestLinkButton" runat="server" OnClick="AddGuestLinkButton_Click">Add Guest Pilot</asp:LinkButton>
            </td>
            <td style="width: 410px; text-align: right;">
                <asp:LinkButton ID="SaveLinkButton" runat="server" OnClick="SaveLinkButton_Click">Back</asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="SearchTextBox" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="SelectButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="AddGuestLinkButton" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Search result"></asp:Label>
                    </td>
                    <td>
                    </td>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Fleet"></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    <asp:LinkButton ID="ClearFleetLinkButton" runat="server" OnClick="ClearFleetLinkButton_Click">Clear</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 45%; background: #202020; vertical-align: top;">
                        <div style="height: 320px; overflow: auto;">
                            <asp:ListBox ID="SearchResultListBox" runat="server" Width="100%" Height="100%" BackColor="#202020"
                                ForeColor="White" SelectionMode="Multiple"></asp:ListBox>
                        </div>
                    </td>
                    <td style="width: 10%; text-align: center;">
                        <p>
                            <asp:Button ID="SelectButton" runat="server" Text=">" OnClick="SelectButton_Click"
                                Visible="false" />
                        </p>
                        <p>
                            <asp:Button ID="DeselectButton" runat="server" Text="<" OnClick="DeselectButton_Click"
                                Visible="false" />
                        </p>
                    </td>
                    <td style="width: 45%; background: #202020; vertical-align: top;">
                        <div style="height: 320px; overflow: auto;">
                            <asp:ListBox ID="PilotListBox" runat="server" Width="100%" Height="100%" BackColor="#202020"
                                ForeColor="White" SelectionMode="Multiple"></asp:ListBox>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
