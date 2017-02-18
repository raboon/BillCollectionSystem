<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="MtbBillCollection.Pages.Register" MasterPageFile="~/Pages/MasterPage.Master" %>

<asp:Content ContentPlaceHolderID="mainContent" runat="server">  
    
<fieldset style="margin:15px">
<legend><asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Size="Large" Height="25px">Create New User</asp:Label></legend>
<table border="0" style="text-align:left;" align="center" width="650px">

    <tr>
        <td class="style2"><asp:Label ID="Label6" runat="server" Text="User Type:"></asp:Label></td>
        <td align="left">
            <asp:DropDownList ID="UserTypeList" runat="server" DataTextField="TypeName" 
                DataValueField="UserTypeId" height="22px" Width="130px">
            </asp:DropDownList>                    
        </td>
    </tr>
            
    <tr>
        <td class="style2"><asp:Label ID="Label1" runat="server" Text="Login Name:"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtLoginName" runat="server" MaxLength="10" 
                Width="135px"></asp:TextBox>

            <asp:RequiredFieldValidator ID="RequiredFieldValidatorForUserID" runat="server" 
                ControlToValidate="txtLoginName" ErrorMessage="User code can't be empty" 
                ForeColor="Red"></asp:RequiredFieldValidator>
        </td>
    </tr>

    <tr>
        <td class="style2"><asp:Label ID="Label2" runat="server" Text="User Name"></asp:Label></td>
        <td class="style1"><asp:TextBox ID="txtUserName" runat="server" Width="377px"></asp:TextBox></td>
    </tr>
            
    <tr>
        <td class="style2"><asp:Label ID="LabelBranch" runat="server" AssociatedControlID="BranchList">Branch: </asp:Label></td>
        <td><asp:DropDownList ID="BranchList" runat="server" height="22px"
                Width="130px" DataTextField="branch_name" DataValueField="branch_code"></asp:DropDownList></td>
    </tr>

    <tr>
        <td class="style2"><asp:Label ID="Label3" runat="server" Text="User Details"></asp:Label></td>
        <td class="style1"><asp:TextBox ID="txtUserDetails" 
                TextMode="MultiLine" Rows="5" runat="server" Width="375px" Height="55px"></asp:TextBox></td>
    </tr>

    <tr>
        <td class="style2"><asp:Label ID="Label4" runat="server" Text="Password:"></asp:Label></td><td class="style1">
            <asp:TextBox ID="txtPassword" runat="server" Width="168px" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidatorForPassword" runat="server" 
                ControlToValidate="txtPassword" ErrorMessage="Password can't be empty" 
                ForeColor="Red"></asp:RequiredFieldValidator>
            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidatorForPassword" 
                runat="server" ControlToValidate="txtPassword" 
                ErrorMessage="Password policy not matched" 
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                ForeColor="Red"></asp:RegularExpressionValidator>--%>
        </td>
    </tr>

    <tr>
        <td class="style2"><asp:Label ID="Label5" runat="server" Text="Retype Password:"></asp:Label></td>
            <td class="style1">
            <asp:TextBox ID="txtPasswordRe" runat="server" Width="166px" TextMode="Password"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidatorForPassword" runat="server" 
                    ControlToCompare="txtPassword" ControlToValidate="txtPasswordRe" 
                    ErrorMessage="Two password are not equal" ForeColor="Red"></asp:CompareValidator>
        </td>
    </tr>

    <tr>
        <td class="style2"/>
        <td class="style1">
                <asp:ImageButton ID="btnCreateNewUser" runat="server" 
            ImageUrl="~/images/addusers.png" 
                    OnClientClick="return confirm('Sure to create to new user?');" onclick="btnCreateNewUser_Click" 
                    Height="48px" Width="59px" />
        </td>
    </tr>
</table>
</fieldset>

 </asp:Content>

<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .style1
        {
            width: 702px;
        }
        .style2
        {
            width: 602px;
        }
    </style>
</asp:Content>


