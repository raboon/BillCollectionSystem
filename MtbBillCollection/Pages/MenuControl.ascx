<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuControl.ascx.cs" Inherits="MtbBillCollection.Pages.MenuControl" %>

<style type="text/css">
    .style2
    {
        width: 793px;
    }
</style>

<asp:Panel runat="server" ID="pnlMenuItems" Visible="false">
    <table border="0" style="text-align:left; width: 908px;background-color:#DDDDDD">
    <tr>
        <td>
            <asp:Literal ID="LitCollDate" runat="server"/>
        </td>
        <td>
            <asp:Literal ID="LitUserName" runat="server"/> 
            <asp:Literal ID="LitBranchName" runat="server"/>            
        </td>        
        <td align="right" style="color: #0000FF">
            <asp:Literal ID="LitLogMenu" runat="server"></asp:Literal>             
        </td>
    </tr>
    </table>   

    <div id="MenuStyle">
        <table border="0px" style="text-align:left; width: 908px;background-color:GRAY; margin:0px">            
            <tr> 
                <td width="100%">                    
                    <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" Width="100%">                        
                        <LevelMenuItemStyles>
                            <asp:MenuItemStyle BorderColor="White" BorderStyle="Dotted" BorderWidth="1px" 
                                Font-Underline="False" HorizontalPadding="10px" />
                        </LevelMenuItemStyles>
                    </asp:Menu>
                    
                </td>
          </tr>
        </table>
    </div>
</asp:Panel>