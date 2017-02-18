<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.Master" AutoEventWireup="true" Culture="en-CA" CodeBehind="Clearing.aspx.cs" Inherits="MtbBillCollection.Pages.Clearing" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <fieldset style="margin:15px"> 
        <legend><asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Large" Height="25px">Search Criteria</asp:Label> </legend>
        <table border="0" style="text-align: left;" align="center">
            <tr>
                <td class="style25">
                    <asp:Label ID="Label3" runat="server" AssociatedControlID="ClientList" Text="Client:" />
                    <asp:DropDownList ID="ClientList" runat="server" Height="22px" Width="288px" DataTextField="ClientName"
                        DataValueField="ClientID" AutoPostBack="True" 
                        OnSelectedIndexChanged="ClientList_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="style19">
                    &nbsp;</td>
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
                        Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="MKE" />


                </td>
                <td class="style27">
                   To Date<asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                    <ajax:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtToDate"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                        ErrorTooltipEnabled="True" />
                    <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="ImgBntCalcTodate" Format="dd/MM/yyyy" />
                    <asp:ImageButton ID="ImgBntCalcTodate" runat="server" ImageUrl="~/images/calendar.gif"
                        CausesValidation="False" />
                    <asp:Label ID="dateFormat0" runat="server" Text="(dd/MM/yyyy)" ForeColor="Blue" Font-Bold="True"></asp:Label>
                    <ajax:MaskedEditValidator ID="MKV2" runat="server" ControlExtender="MaskedEditExtender1" 
                        ControlToValidate="txtToDate" EmptyValueMessage="Date is required" InvalidValueMessage="Invalid Date"
                        Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="MKE"
                        ErrorMessage="Invalid Date" />

                </td>
            </tr>
            <tr>
                <td class="style25">
                    <asp:CompareValidator ID="CompareValidator1" runat="server" 
                        ErrorMessage="From date can't be bigger then To date" 
                        ControlToCompare="txtToDate" ControlToValidate="txtFromDate" ForeColor="Red" 
                        Operator="LessThanEqual" ValidationGroup="MKE" EnableTheming="True" 
                        Type="Date"></asp:CompareValidator>
                </td>
                <td class="style27" align="right">
                    <asp:ImageButton ID="ButtonSearch" runat="server" ImageUrl="~/images/Search.gif"
                        Width="42px" OnClick="ButtonSearch_Click" Height="30px" 
                        ValidationGroup="MKE" ToolTip="Search" />
                </td>
            </tr>
        </table>
    </fieldset>
    <br />
    <fieldset style="margin:15px">
        <legend><asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Large" Height="25px">CLearing Box</asp:Label></legend>
         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
          <div>                  
                
                        <asp:GridView ID="gridCollList" runat="server" Height="335px" Width="775px"
                    ShowFooter="True" AlternatingRowStyle-BackColor="LightGray" 
                    CellPadding="4"
                    DataKeyNames="CollectionId" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                    AllowSorting="True" ForeColor="#333333" GridLines="None" Caption="&lt;div style=&quot;width:100%;Height:50Px;valign:top;border:1;border-color:White; background-color:GRAY&quot;&gt;&lt;br/&gt;&lt;Font Size=4; color=white&gt;&lt;b&gt; Collection Informtion&lt;/b&gt;&lt;/font&gt;&lt;br/&gt;&lt;/div&gt;"
                    CaptionAlign="Top" BorderColor="White" BorderWidth="1px">
                            <AlternatingRowStyle BackColor="White" ForeColor="#333333"></AlternatingRowStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="Mark For Clear" HeaderStyle-ForeColor="White">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cboApprove" runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CollDate" HeaderText="Collection Date" DataFormatString="{0:dd-MM-yyyy}"
                            HeaderStyle-ForeColor="White">
                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ProductName" HeaderText="Product" 
                            HeaderStyle-ForeColor="White">
                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="branch_name" HeaderText="Branch" 
                            HeaderStyle-ForeColor="White">
                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CollFrom" HeaderText="Depositor ID" 
                            HeaderStyle-ForeColor="White">
                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="InstTypeName" HeaderText="Instrument Type" 
                            HeaderStyle-ForeColor="White">
                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="InstNumber" HeaderText="Instrument No." 
                            HeaderStyle-ForeColor="White">
                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="BankName" HeaderText="Ref. Bank" 
                            HeaderStyle-ForeColor="White">
                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Colllection Amount" HeaderStyle-ForeColor="White"
                            FooterStyle-Font-Bold="True" ItemStyle-HorizontalAlign="Right" 
                            FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# GetUnitPrice(decimal.Parse(Eval("CollAmount").ToString())).ToString("N2")%>
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Right" Font-Bold="True"></FooterStyle>
                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:TemplateField>
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
                         <asp:Button runat="server" ID="Button1" Style="display: none;" />
     <ajaxtoolkit:modalpopupextender runat="server" id="MessegePopupExtender" behaviorid="MessegePopupExtenderBehavior"
        targetcontrolid="Button1" popupcontrolid="MessegePopup" RepositionMode="None" 
        backgroundcssclass="modalBackground" dropshadow="True" >    

    </ajaxtoolkit:modalpopupextender>
          
    <div>
      <asp:Panel runat="server" ScrollBars="None" CssClass="modalPopup" ID="MessegePopup" Style="display: none; height: 150;
        width: 250px; padding: 10px">
        <asp:Panel BackColor="White" runat="server" ID="pnl" style="height:25px;" >
            <asp:Label Font-Size="Large" BackColor="White" Text="" ID="PopupHeader" runat="server"></asp:Label>             
        </asp:Panel>
        <asp:Panel  runat="server" ID="Panel3" style="height:50px" >     
                <asp:Label Text="" ID="PopupMessage" runat="server"></asp:Label>              
        </asp:Panel> 
        <asp:Panel BackColor="LightGray"  HorizontalAlign="Center" runat="server" ID="Panel4" style="height:25px; vertical-align:middle; margin-top:2px" >
            <asp:LinkButton runat="server" ID="OkButton" ForeColor="White" Text="Ok" PostBackUrl="Clearing.aspx" 
                BackColor="DarkGray" onclick="OkButton_Click"/>     
        </asp:Panel>   
      </asp:Panel>      
    </div>
                    
                <br/>
                <asp:ImageButton ID="btnApprove" runat="server" 
                    ImageUrl="~/images/apply_f2.png" onclick="btnApprove_Click" Height="36px" 
                    Width="40px" />
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="btnCancel" runat="server" Height="36px" 
                    ImageUrl="~/images/cancel_f2.png" onclick="btnCancel_Click" Width="40px" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          </div>
          </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="OkButton" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ButtonSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
    </fieldset>
    <br />
   
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style19
        {
        }
        .style24
        {
            height: 46px;
            width: 53%;
        }
        .style25
        {
        }
        .style27
        {
        }
        </style>
</asp:Content>
