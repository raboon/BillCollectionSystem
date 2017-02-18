
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="MtbBillCollection.Pages.ChangePassword" MasterPageFile="~/Pages/MasterPage.Master"%>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">
<fieldset style="margin:15px">
      

<legend><asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Size="Large" Height="25px">Change Password</asp:Label></legend>
<div>
    <asp:ValidationSummary ID="ValidationSummary" runat="server" ForeColor="Red" 
        ValidationGroup="ValidationSummary" />
</div>
          
      
<div style="color: #000000" align="center"/>
<table border="0px" style="text-align:left; width: 425px;" cellpadding="3px" >
    <tr>
        <td class="style2">
            <asp:Label ID="lblOldPassword" runat="server" Text="Old Password:"></asp:Label>
        </td>
        <td class="style1">
            <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" 
                MaxLength="10" ValidationGroup="ValidationSummary"></asp:TextBox>

    <asp:RequiredFieldValidator ID="RequiredFieldValidatorForUserName" 
        runat="server" ControlToValidate="txtOldPassword" 
        ErrorMessage="Old Password can't be empty" 
        ValidationGroup="ValidationSummary" ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="style2">
            <asp:Label ID="lblNewPassword" runat="server" Text="New Password:"></asp:Label>
        </td>
        <td class="style1">
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="8" 
                ValidationGroup="ValidationSummary"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidatorForPassword" 
        runat="server" ErrorMessage="New Password can't be empty" 
        ControlToValidate="txtPassword" ValidationGroup="ValidationSummary" 
                ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="style2">
            <asp:Label ID="lblNewPasdRe" runat="server" Text="Re-Type New Password:"></asp:Label>
        </td>
        <td class="style1">
            <asp:TextBox ID="txtPasswordRe" runat="server" TextMode="Password" 
                MaxLength="8" ValidationGroup="ValidationSummary"></asp:TextBox>
    <asp:CompareValidator ID="CompareValidator2" runat="server" 
        ControlToCompare="txtPassword" ControlToValidate="txtPasswordRe" 
        ErrorMessage="Two Password are not same" 
        ValidationGroup="ValidationSummary" ForeColor="Red">*</asp:CompareValidator>
    &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:RequiredFieldValidator ID="RequiredFieldValidatorForRe" runat="server" 
                ControlToValidate="txtPasswordRe" ErrorMessage="Please retype password." 
                ForeColor="Red" ValidationGroup="ValidationSummary">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    
    
    <tr>
        <td class="style2">&nbsp;</td>
        <td class="style1">
            <asp:ImageButton  ID="ImageButton1" runat="server" OnClientClick=""
                ImageUrl="~/images/backup.png" Height="32px" Width="44px" 
                onclick="ImageButton1_Click" ValidationGroup="ValidationSummary" />
        </td>
    </tr>
</table>
</fieldset>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Blue"></asp:Label>

    <br />
    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>


</asp:Content>


<asp:Content ID="Content2" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .style1
        {
            width: 196px;
        }
        .style2
        {
            width: 187px;
        }
    </style>
</asp:Content>



