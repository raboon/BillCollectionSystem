<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.Master" AutoEventWireup="true" CodeBehind="RecoverPassword.aspx.cs" Inherits="MtbBillCollection.Pages.RecoverPassword" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
 
 
 <fieldset style="margin:15px">
 
 <legend><asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Size="Large" Height="25px">Filter</asp:Label></legend>

 <table border="0px" style="text-align:center" align="center" width="650px">
    <tr>
        <td>
            LogIn Name:<asp:DropDownList ID="listMachingCriteria" runat="server" 
                width="107px">
            </asp:DropDownList>
        </td>
        <td align="left" colspan="2">
            UserTypeId<asp:TextBox ID="LogInName" runat="server" Height="22px" 
                ValidationGroup="Screen" Width="242px">All</asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            User Type:&nbsp; <asp:DropDownList ID="listUserType" runat="server" 
                Width="107px" DataTextField="TypeName" DataValueField="UserTypeId">
            </asp:DropDownList>
        </td>
        <td align="left" >
            Branch&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:DropDownList ID="listBranch" runat="server" Height="22px" width="242px" 
                DataTextField="branch_name" DataValueField="branch_code">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="center" colspan="3">
            <asp:ImageButton ID="btnAdd" runat="server" Height="34px" 
                ImageUrl="~/images/Search.gif" onclick="btnSearch_Click" 
                ValidationGroup="Screen" Width="43px" />
        </td>
    </tr>
    <tr>
        <td align="center" colspan="3">
            <asp:RequiredFieldValidator ID="ReqFieldValidatorForScreen" runat="server" 
                ControlToValidate="LogInName" ErrorMessage="LogIn Name can not be empty." 
                Font-Bold="True" ForeColor="Red" ValidationGroup="Screen"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td align="center" colspan="3">
            <asp:Label ID="lblStatus" runat="server"></asp:Label>
        </td>
    </tr>
</table>

</fieldset>

<br/>

<fieldset style="width:870px; margin:1px" class="ResultBox">
<legend >Result</legend>
    <asp:GridView ID="GridView1" width="865px" runat="server" AutoGenerateColumns="False" >
        <Columns>
            <asp:BoundField DataField="UserName" HeaderText="User Name" />
            <asp:BoundField DataField="UserDetails" HeaderText="UserDetails" />
            <asp:BoundField DataField="BranchCode" HeaderText="BranchCode" />
            <asp:BoundField DataField="TypeName" HeaderText="UserType" />
            <asp:BoundField DataField="LoginName" HeaderText="Log In Name" />
            <asp:TemplateField HeaderText="Reset Password">
                <ItemTemplate>
                    <asp:Button ID="btnForReset" runat="server" Text="Go To Reset" 
                        onclick="btnForReset_Click" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</fieldset>
<asp:scriptmanager ID="Scriptmanager1" runat="server" EnablePartialRendering="true"></asp:scriptmanager>    
   

<asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none;"/>
 
   <ajaxtoolkit:modalpopupextender runat="server" id="programmaticModalPopup" behaviorid="programmaticModalPopupBehavior"
        targetcontrolid="hiddenTargetControlForModalPopup" popupcontrolid="programmaticPopup" 
        backgroundcssclass="modalBackground" dropshadow="True" popupdraghandlecontrolid="programmaticPopupDragHandle" CancelControlID="hideModalPopupViaServer" >    

    </ajaxtoolkit:modalpopupextender>


          
    <div>
      <asp:Panel runat="server" CssClass="modalPopup" ID="programmaticPopup" Style="display: none;
        width: 400px; padding: 10px">
        <fieldset style="width:370px">
          <legend>Set New Password</legend>
        <asp:Panel runat="server" ID="pnl" style="height:50px; ">
            <asp:Label ID="lblHeader" runat="server" Text="Set New Password"></asp:Label>
        </asp:Panel>
        <asp:Panel runat="server" ID="PanelBody" style="height:200px; ">            
            <table border="0" style="text-align:left; width: 100%; margin-top:20px;">
                <tr>
                    <td >
                        LogIn Name:
                    </td>
                    <td align="left" colspan="1">
                        <asp:Label ID="txtLogInName" runat="server" ValidationGroup="popup" Height="22px" Width="130px"></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td>
                        New Password:
                    </td>
                    <td align="left" class="style2">
                        <asp:TextBox ID="txtNewPass" runat="server" Height="22px" TextMode='Password' MaxLength='8'
                            ValidationGroup="popup" Width="130px"></asp:TextBox>                            
                    </td>                                     
                </tr> 
                <tr>                    
                    <td align="left" colspan="2">                        
                            <asp:RequiredFieldValidator ID="ReqFieldValidatorForNewPass" ForeColor="Red" runat="server" ValidationGroup="popup" ControlToValidate="txtNewPass" ErrorMessage="New Password can not be Empty"></asp:RequiredFieldValidator>                   
                    </td>                                     
                </tr>  
                <tr>
                    <td>
                        Retype Password:
                    </td>
                    <td align="left" class="style2">
                        <asp:TextBox ID="txtRetypePass" runat="server" Height="22px" TextMode='Password' MaxLength='8'
                            ValidationGroup="popup" Width="130px"></asp:TextBox>                        
                    </td>  
                </tr>
                <tr>                    
                    <td align="left" colspan="2">                        
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" runat="server" ValidationGroup="popup" ControlToValidate="txtRetypePass" ErrorMessage="Retype Password can not be Empty."></asp:RequiredFieldValidator>                   
                    </td>                                     
                </tr>
                <tr>                    
                    <td align="left" colspan="2">                        
                            <asp:CompareValidator ID="CompareValidator" ForeColor="Red" runat="server" ValidationGroup="popup" ControlToValidate="txtRetypePass" ControlToCompare="txtNewPass" ErrorMessage="Two Password does't match."></asp:CompareValidator>                   
                    </td>                                     
                </tr>           
            </table>            
        </asp:Panel>
          <asp:LinkButton runat="server" ID="hideModalPopupViaServer" Text="Close"  />
          <asp:LinkButton runat="server" ID="ResetPass" Text="Reset" OnClick="ResetPass_Click" ValidationGroup="popup"/>
          </fieldset>
      </asp:Panel>      
    </div>
    
</asp:Content>
