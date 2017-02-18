<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.Master" AutoEventWireup="true" Culture="en-CA" CodeBehind="CollectionLogReport.aspx.cs" Inherits="MtbBillCollection.Pages.CollectionLogReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 99%;
        }
        .style13
        {
            width: 293px;
            height: 41px;
        }
        .style14
        {
            height: 41px;
        }
        .style15
        {
            height: 41px;
        }
        .style16
        {
            height: 28px;
        }
        .style17
        {
            height: 41px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
<fieldset id="SearchCriteria" style="SearchCriteria">
    <legend>Search Criteria</legend>
    <table class="style1">
        <tr>
                <td class="style16" valign="middle" colspan="2" align="left">
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
                        ValidationGroup="MKE" ForeColor="Red" />


                   To Date<asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                    <ajax:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtToDate"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                        ErrorTooltipEnabled="True" />
                    <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="ImgBntCalcTodate" Format="dd/MM/yyyy" />
                    <asp:ImageButton ID="ImgBntCalcTodate" runat="server" ImageUrl="~/images/calendar.gif"
                        CausesValidation="False" />
                    <asp:Label ID="dateFormat0" runat="server" Text="(DD/MM/yyyy)" ForeColor="Blue" Font-Bold="True"></asp:Label>
                    <ajax:MaskedEditValidator ID="MKV2" runat="server" ControlExtender="MaskedEditExtender1" 
                        ControlToValidate="txtToDate" EmptyValueMessage="Date is required" InvalidValueMessage="Invalid Date"
                        Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="MKE"
                        ErrorMessage="Invalid Date" ForeColor="Red" />


                </td>
                </tr>
        <tr>
            <td class="style13" align="left">
                File Type
                <asp:DropDownList ID="fileTypeList" runat="server" Height="22px" 
                    Width="145px">
                </asp:DropDownList>
                </td>
            <td class="style17">
                <asp:ImageButton ID="btnSearch" runat="server" Height="30px" 
                    ImageUrl="~/images/Search.gif" onclick="btnSearch_Click" Width="40px" />
            </td>
        </tr>
        </table>
    </fieldset>
    <asp:GridView ID="collectiomInfo" runat="server" AutoGenerateColumns="False" 
                    EnableViewState="False" Width="885px">
                    <Columns>
                        <asp:BoundField DataField="FileName" HeaderText="File Name" />
                        <asp:BoundField DataField="CreationDate" HeaderText="Creation Date" />
                        <asp:BoundField DataField="FileSlnO" HeaderText="Sl No" />
                        <asp:BoundField DataField="FileInfoType" HeaderText="File Type" />
                        <asp:BoundField DataField="NoOfTrans" HeaderText="No Of Trans." />
                        <asp:BoundField DataField="CreationTime" HeaderText="Creation Time" />
                    </Columns>
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
            <asp:Label Font-Size="Large" ForeColor="Blue" BackColor="White" Text="" ID="PopupHeader" runat="server"></asp:Label>             
        </asp:Panel>
        <asp:Panel  runat="server" ID="Panel3" style="height:50px" >     
                <asp:Label ForeColor="Black" BackColor="White" Text="" ID="PopupMessage" runat="server"></asp:Label>              
        </asp:Panel> 
        <asp:Panel BackColor="LightGray"  HorizontalAlign="Center" runat="server" ID="Panel4" style="height:25px; vertical-align:middle; margin-top:2px" >
            <asp:LinkButton runat="server" ID="OkButton" ForeColor="White" Text="Ok" 
                BackColor="DarkGray"/>     
        </asp:Panel>   
      </asp:Panel>      
    </div>

    </asp:Content>
