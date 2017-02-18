<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewCollections.aspx.cs" Culture="en-CA" Inherits="MtbBillCollection.Pages.Collections" MasterPageFile="~/Pages/MasterPage.Master" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">

<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
</asp:ScriptManager>
    <br />

<fieldset style="margin:15px"> 
<legend><asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Size="Large" Height="25px">Search Criteria</asp:Label></legend>
<table border="0" style="text-align:left; width: 100%; margin-top:20px;">
    <tr>
        <td class="style7"><asp:Label ID="Label3" runat="server" AssociatedControlID="ClientList" Text="Client:"/></td>
        <td class="style2">
            <asp:DropDownList ID="ClientList" runat="server" Height="27px" Width="227px" 
                DataTextField="ClientName" DataValueField="ClientID">
            </asp:DropDownList>
        </td>
    </tr>

    <tr>
                <td class="style24" valign="middle" colspan="1">
                    From Date</td>
                <td colspan="2" width="100%">
                    <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
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
        <td class="style8" align="center" colspan="5">
             <asp:CompareValidator ID="CompareValidator1" runat="server" 
                        ErrorMessage="From date can't be bigger then To date" 
                        ControlToCompare="txtToDate" ControlToValidate="txtFromDate" ForeColor="Red" 
                        Operator="LessThanEqual" ValidationGroup="MKE" Type="Date"></asp:CompareValidator>
        </td>

    </tr>


    <tr>
        <td class="style28" colspan="1"><asp:Label ID="Label5" runat="server" 
                AssociatedControlID="ProducTypeList"  Text="Product   "/></td>
        <td colspan="4" align="left"><asp:CheckBoxList ID="ProducTypeList" runat="server"  
                DataTextField="ProductName" DataValueField="ProductID" ForeColor="Black" /></td>

    </tr>
    <tr>
        <td class="style28" colspan="1"><asp:Label ID="Label1" runat="server" AssociatedControlID="CollectionStatusType">Collection </asp:Label></td>
        <td colspan="4" align="left"><asp:CheckBoxList ID="CollectionStatusType" runat="server"  DataTextField="CollStatus" DataValueField="CollStatusID" ForeColor="Black" /></td>
        </tr>


    <tr>
        <td class="style28" colspan="1"><asp:Label ID="Label4" runat="server" AssociatedControlID="InstrumentTypeList">Instrument </asp:Label></td>
        <td align="left" class="style27" colspan="4"><asp:CheckBoxList ID="InstrumentTypeList" runat="server"  DataTextField="InstTypeShortName" DataValueField="InstTypeId" ForeColor="Black" /></td>

        <td align="center" class="style26" width="100%">
            <asp:ImageButton ID="ButtonSearch" runat="server" 
                ImageUrl="~/images/Search.gif" Width="42px" 
                onclick="ButtonSearch_Click" Height="30px" ToolTip="Search" 
                ValidationGroup="MKE" />
        </td>
        
    </tr>
</table>
</fieldset>
<table border="0" style="text-align:left; width: 908px; margin-top:20px;">
    <tr>
    <td colspan="6">
    <asp:Panel ID="Panel1" runat="server" Width='700px'>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" Width="350px"
            AutoDataBind="True" ReuseParameterValuesOnRefresh="True" ShowAllPageIds="True" 
            ToolPanelView="None" GroupTreeImagesFolderUrl="" Height="50px" 
            ToolbarImagesFolderUrl="" ToolPanelWidth="200px" />
    </asp:Panel>    
    </td>
    </tr>
</table>

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
            <asp:LinkButton runat="server" ID="OkButton" ForeColor="White" Text="Ok" 
                BackColor="DarkGray" />     
        </asp:Panel>   
      </asp:Panel>      
    </div>
    
</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .style2
        {
            height: 26px;
            width: 735px;
        }
        .style7
        {
            height: 26px;
            width: 238px;
        }
        .style8
        {
        }
        .style26
        {
            width: 140px;
        }
        .style27
        {
            width: 416px;
        }
        .style24
        {
            height: 46px;
            width: 238px;
        }
        .style28
        {
            width: 238px;
        }
    </style>
</asp:Content>
