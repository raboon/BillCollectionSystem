<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuthorizeBillCollection.aspx.cs" Culture="en-CA"
    Inherits="MtbBillCollection.Pages.AuthorizeBillCollection" MasterPageFile="~/Pages/MasterPage.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <fieldset style="margin:15px"> 
        <legend><asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Large" Height="25px">Search Criteria</asp:Label> </legend>
        <table border="0" style="text-align: left;" align="center" width="100%">
            <tr>
                <td class="style19">
                    <asp:Label ID="Label3" runat="server" AssociatedControlID="ClientList" Text="Client:" />
                    <asp:DropDownList ID="ClientList" runat="server" Height="23px" Width="267px" DataTextField="ClientName"
                        DataValueField="ClientID" AutoPostBack="True" 
                        OnSelectedIndexChanged="ClientList_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:RadioButton ID="rdoNormal" runat="server" Checked="True" Text="General" 
                        GroupName="collType" />
                    <asp:RadioButton ID="rdoSpeacial" runat="server" Text="Speacial" 
                        GroupName="collType" />
                </td>
            </tr>
            <tr>
                <td class="style24" valign="middle">
                    From Date<asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                    <ajax:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtFromDate"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                        ErrorTooltipEnabled="True" />
                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                        PopupButtonID="ImgBntCalcFromdate" Format="dd/MM/yyyy" />
                    <asp:ImageButton ID="ImgBntCalcFromdate" runat="server" ImageUrl="~/images/calendar.gif"
                        CausesValidation="False" />
                    <asp:Label ID="dateFormat" runat="server" Text="(DD/MM/yyyy)" ForeColor="Blue" Font-Bold="True"></asp:Label>
                    <ajax:MaskedEditValidator ID="MKv1" runat="server" ControlExtender="MaskedEditExtender1"
                        ControlToValidate="txtFromDate" EmptyValueMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                        Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" 
                        ValidationGroup="MKE" ErrorMessage="Invalid Date" ForeColor="Red" />


                </td>
                <td>
                   To Date<asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                    <ajax:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtToDate"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                        ErrorTooltipEnabled="True" />
                    <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="ImgBntCalcTodate" Format="dd/MM/yyyy" />
                    <asp:ImageButton ID="ImgBntCalcTodate" runat="server" ImageUrl="~/images/calendar.gif"
                        CausesValidation="False" />
                    <asp:Label ID="dateFormat0" runat="server" Text="(DD/MM/yyyy)" ForeColor="Blue" 
                        Font-Bold="True"></asp:Label>
                    <ajax:MaskedEditValidator ID="MKV2" runat="server" ControlExtender="MaskedEditExtender1" 
                        ControlToValidate="txtToDate" EmptyValueMessage="Date is required" InvalidValueMessage="Invalid Date"
                        Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="MKE"
                        ErrorMessage="Invalid Date" ForeColor="Red" />

                </td>
            </tr>
            <tr>
                <td class="style19">
                    <asp:CompareValidator ID="CompareValidator1" runat="server" 
                        ErrorMessage="From date can't be bigger then To date"  Type="Date"
                        ControlToCompare="txtToDate" ControlToValidate="txtFromDate" ForeColor="Red" 
                        Operator="LessThanEqual" ValidationGroup="MKE"></asp:CompareValidator>
                </td>
                <td>
                    <asp:ImageButton ID="ButtonSearch" runat="server" ImageUrl="~/images/Search.gif"
                        Width="42px" OnClick="ButtonSearch_Click" Height="30px" 
                        ValidationGroup="MKE" ToolTip="Search" />
                </td>
            </tr>
        </table>
    </fieldset>
    <br />
    <fieldset style="margin:15px">
        <legend><asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Large" Height="25px">Approve Box</asp:Label></legend>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
          <div>                  
                
                        <asp:GridView ID="gridCollList" runat="server" AllowSorting="True" 
                            AlternatingRowStyle-BackColor="LightGray" AutoGenerateColumns="False" 
                            BorderColor="White" BorderWidth="1px" 
                            Caption="&lt;div style=&quot;width:100%;Height:50Px;valign:top;border:1;border-color:White; background-color:GRAY&quot;&gt;&lt;br/&gt;&lt;Font Size=4; color=white&gt;&lt;b&gt; Collection Informtion&lt;/b&gt;&lt;/font&gt;&lt;br/&gt;&lt;/div&gt;" 
                            CaptionAlign="Top" CellPadding="4" DataKeyNames="CollectionId" 
                            ForeColor="#333333" GridLines="None" Height="335px" 
                            OnDataBound="gridCollList_DataBound" OnRowEditing="gridCollList_RowEditing" 
                            ShowFooter="True" ShowHeaderWhenEmpty="True" Width="775px">
                            <AlternatingRowStyle BackColor="White" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-ForeColor="White" HeaderText="Approve">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cboApprove" runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle ForeColor="White" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="CollDate" DataFormatString="{0:dd-MM-yyyy}" 
                                    HeaderStyle-ForeColor="White" HeaderText="Collection Date">
                                <HeaderStyle ForeColor="White" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ProductName" HeaderStyle-ForeColor="White" 
                                    HeaderText="Product">
                                <HeaderStyle ForeColor="White" />
                                </asp:BoundField>
                                <asp:BoundField DataField="branch_name" HeaderStyle-ForeColor="White" 
                                    HeaderText="Branch">
                                <HeaderStyle ForeColor="White" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CollFrom" HeaderStyle-ForeColor="White" 
                                    HeaderText="Depositor ID">
                                <HeaderStyle ForeColor="White" />
                                </asp:BoundField>
                                <asp:BoundField DataField="InstTypeName" HeaderStyle-ForeColor="White" 
                                    HeaderText="Instrument Type">
                                <HeaderStyle ForeColor="White" />
                                </asp:BoundField>
                                <asp:BoundField DataField="InstNumber" HeaderStyle-ForeColor="White" 
                                    HeaderText="Instrument No.">
                                <HeaderStyle ForeColor="White" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BankName" HeaderStyle-ForeColor="White" 
                                    HeaderText="Ref. Bank">
                                <HeaderStyle ForeColor="White" />
                                </asp:BoundField>
                                <asp:TemplateField FooterStyle-Font-Bold="True" 
                                    FooterStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="White" 
                                    HeaderText="Colllection Amount" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                <%# GetUnitPrice(decimal.Parse(Eval("CollAmount").ToString())).ToString("N2")%>
                                    </ItemTemplate>
                                    <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                    <HeaderStyle ForeColor="White" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:CommandField HeaderStyle-ForeColor="White" HeaderText="Update Info" 
                                    ShowEditButton="True">
                                <HeaderStyle ForeColor="White" />
                                <ItemStyle ForeColor="#0066CC" />
                                </asp:CommandField>
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="GRAY" Font-Bold="True" ForeColor="White" Height="5px" />
                            <HeaderStyle BackColor="GRAY" Font-Bold="True" ForeColor="White" 
                                Height="10px" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                    
                    
                <br/>
                <asp:ImageButton ID="btnApprove" runat="server" 
                    ImageUrl="~/images/apply_f2.png" onclick="btnApprove_Click" 
                    AlternateText="Approve" Height="36px" Width="40px" ToolTip="Approve" />
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="btnCancel" runat="server" 
                    ImageUrl="~/images/cancel_f2.png" onclick="btnCancel_Click1" height="36px" 
                    width="40px" ToolTip="Cancel" />
          </div>

     <asp:Button runat="server" ID="Button1" Style="display: none;" />
     <ajaxtoolkit:modalpopupextender runat="server" id="MessegePopupExtender" behaviorid="MessegePopupExtenderBehavior"
        targetcontrolid="Button1" popupcontrolid="MessegePopup" RepositionMode="None" 
        backgroundcssclass="modalBackground" dropshadow="True" >    

    </ajaxtoolkit:modalpopupextender>
          
    <div>
      <asp:Panel runat="server" ScrollBars="None" CssClass="modalPopup" ID="MessegePopup" Style="display: none; height: 150;
        width: 250px; padding: 10px">
        <asp:Panel BackColor="White" runat="server" ID="pnl" style="height:25px;" >
            <asp:Label Font-Size="Large" ForeColor="Blue" BackColor="White" Text="" ID="PopupHeader" runat="server"></asp:Label>             
        </asp:Panel>
        <asp:Panel  runat="server" ID="Panel3" style="height:50px" >     
                <asp:Label ForeColor="Black" BackColor="White" Text="" ID="PopupMessage" runat="server"></asp:Label>              
        </asp:Panel> 
        <asp:Panel BackColor="LightGray"  HorizontalAlign="Center" runat="server" ID="Panel4" style="height:25px; vertical-align:middle; margin-top:2px" >
            <asp:LinkButton runat="server" ID="OkButton" ForeColor="White" Text="Ok" 
                BackColor="DarkGray" onclick="OkButton_Click"/>     
        </asp:Panel>   
      </asp:Panel>      
    </div>

          </ContentTemplate>
          <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnApprove" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ButtonSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="OkButton" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ClientList" 
                            EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
    </fieldset>
    
    <br />
    
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style19
        {
            width: 52%;
        }
        .style24
        {
            height: 46px;
            width: 52%;
        }
    </style>
</asp:Content>
