<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.Master" AutoEventWireup="true" CodeBehind="RoleWiseScreenMapping.aspx.cs" Inherits="MtbBillCollection.Pages.RoleWiseScreenMapping" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
        }
        .style3
        {
            width: 135px;
        }
        .style4
        {
            width: 79px;
        }
        .style7
        {
            width: 66px;
        }
        .style8
        {
            width: 88px;
        }
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <table class="style1">
        <tr>
            <td align="left" class="style2">
                Screen Name:
                <asp:TextBox ID="ScreenName" runat="server" Height="22px" Width="376px" 
                    ValidationGroup="Screen"></asp:TextBox>
            </td>
            <td class="style4">
                <asp:CheckBox ID="cboManager" runat="server" Text="Manger" />
            </td>
            <td class="style7">
                <asp:CheckBox ID="cboIssuer" runat="server" Text="Issuer" />
            </td>
            <td class="style8">
                <asp:CheckBox ID="cboReviewer" runat="server" Text="Reviewer" />
            </td>
            <td class="style3">
                <asp:ImageButton ID="btnAdd" runat="server" Height="33px" 
                    ImageUrl="~/images/btnAddScreen.jpg" onclick="btnAdd_Click" 
                    ValidationGroup="Screen" />
            </td>
        </tr>
        <tr>
            <td align="center" class="style2" colspan="5">
                <asp:RequiredFieldValidator ID="ReqFieldValidatorForScreen" runat="server" 
                    ControlToValidate="ScreenName" ErrorMessage="Screen Name can not be empty." 
                    Font-Bold="True" ForeColor="Red" ValidationGroup="Screen"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="center" class="style2" colspan="5">
                <asp:Label ID="lblStatus" runat="server"></asp:Label>
            </td>
        </tr>
        </table>
    <br />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    Width="895px">
                    <Columns>
                        <asp:BoundField HeaderText="Screen Name" DataField="ScreenName" />
                        <asp:CheckBoxField DataField="IsManagerPermited" HeaderText="Manager" 
                            ReadOnly="True" />
                        <asp:CheckBoxField DataField="IsIssuerPermited" HeaderText="Issuer" />
                        <asp:CheckBoxField DataField="IsReviewerPermited" HeaderText="Reviewer" />
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" onclick="btnEdit_Click">Edit</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" onclick="btnDelete_Click" 
                                    onclientclick="return Confirm('Are you want to delete?')">Delete</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Content>
