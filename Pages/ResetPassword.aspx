<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="MtbBillCollection.Pages.PassRecoveryRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div align="center" style="margin: 30px; font-weight:bold">
        Password Recovery Request
      </div>

<div style="color:#000000;" align="center"/>

<table border="0" style="text-align:left;" cellpadding="3px" >
<tr><td>

<div class="curve3_body" id="column_02">
<div class="curve3_body2">
<div id="curve3_head">
</div>

</div>
</div>    

</td>
</tr>
<tr>
<td>

    <asp:RequiredFieldValidator ID="RequiredFieldValidatorForUserName" 
        runat="server" ControlToValidate="txtLoginName" 
        ErrorMessage="User Name Cannot be Empty"></asp:RequiredFieldValidator>
    <br />
    <br />
    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
</td></tr>
</table>



</asp:Content>
