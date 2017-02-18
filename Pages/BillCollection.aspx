<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="BillCollection.aspx.cs" Inherits="MtbBillCollection.BillCollection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">

     <script type="text/javascript" language="javascript">
         <!--
            function Validate() {
                if (document.getElementById('<%=InstrumentAmount.ClientID %>').value == "") {
                    alert('please entry Amount.');
                    return false;
                }
                else if (document.getElementById('<%=TranssactionTypeList.ClientID %>').value != 1) {
                    if (document.getElementById('<%=InstrumentNo.ClientID %>').value == ""
                    && document.getElementById('<%=InstrumentNo.ClientID %>').value == '') {
                        alert('please entry Instrument No.');
                        return false;
                    }
                    else if (document.getElementById('<%=InstrumentDate.ClientID %>').value == ""
                    && document.getElementById('<%=InstrumentDate.ClientID %>').value == '') {

                        alert('please entry Instrument Date.');
                        return false;
                    }
                }


                if (document.getElementById('<%=CollectionFrom.ClientID %>').value == "") {
                    if (document.getElementById('<%=ProductList.ClientID %>').value == 1) {
                        alert('please entry Pos Code.');

                    }
                    else if (document.getElementById('<%=ProductList.ClientID %>').value == 2) {
                        alert('please entry Mob. No.');
                    }
                    return false;
                }
                return confirm("Are you want to save.");
            }
            window.onload = function () {
                //document.getElementById('<%= InstrumentAmount.ClientID %>').onkeydown = key_event;   
                document.getElementById('<%= InstrumentAmount.ClientID %>').onkeypress = key_event;
            }

            function key_event(evt) {
                //write any code here that you want

                //to perform when a key is pressed.       

                //here is one example for de-activating the 'esc'
                //        alert(document.getElementById('<%= InstrumentAmount.ClientID %>').value);

                //alert(evt.keyCode);               
                if ((parseInt(evt.charCode) > 47 && parseInt(evt.charCode) < 58) || parseInt(evt.charCode) == 46)
                    return true;
                if (parseInt(evt.charCode) == 0 && (parseInt(evt.keyCode) == 8 || parseInt(evt.keyCode) == 9 || parseInt(evt.keyCode) == 46 || (parseInt(evt.keyCode) > 36 && parseInt(evt.keyCode) < 41)))
                    return true;
                return false;
            }
            
            -->
        </script>

        <fieldset style="margin:15px">         
        <legend><asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="Large" Height="25px">Bill Collection Entry</asp:Label> </legend>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                     <table border="0" style="text-align: left;" align="center" width="650px">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" AssociatedControlID="ClientList">Client : </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ClientList" runat="server" Height="21px" Width="190px" DataTextField="ClientName"
                        DataValueField="ClientId" AutoPostBack="True" OnSelectedIndexChanged="ClientList_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" AssociatedControlID="ProductList">Product Type: </asp:Label>
                </td>
                <td>                   
                    <asp:DropDownList ID="ProductList" runat="server" Height="21px" Width="190px" DataTextField="ProductName"
                        DataValueField="ProductID" AutoPostBack="True" OnSelectedIndexChanged="ProductList_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCollectionType" runat="server" AssociatedControlID="CollectionTypeList">Collection Type: </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="CollectionTypeList" runat="server" Height="21px" Width="190px"
                        DataTextField="CollectionType" DataValueField="CollectionTypeId">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCollectionFromID" runat="server" AssociatedControlID="CollectionFrom"
                        Text="POS Code:" />
                </td>
                <td>
                    <asp:TextBox ID="CollectionFrom" runat="server" CssClass="textEntry" Width="190px"
                        Height="22px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelTranssactionType" runat="server" AssociatedControlID="TranssactionTypeList">Transsaction Type: </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="TranssactionTypeList" runat="server" Height="22px" Width="190px"
                        DataTextField="InstTypeName" DataValueField="InstTypeId" AutoPostBack="True"
                        OnSelectedIndexChanged="TranssactionTypeList_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="InstrumentNoLabel" runat="server" AssociatedControlID="InstrumentNo">Instrument No: </asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="InstrumentNo" runat="server" CssClass="textEntry" Height="22px"
                        Width="190px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelInstrumentDate" runat="server" AssociatedControlID="InstrumentDate">Instrument Date: </asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="InstrumentDate" runat="server" CssClass="textEntry" Height="22px"
                        Width="190px"></asp:TextBox>
                    <asp:ImageButton ID="ImgBntCalc" runat="server" ImageUrl="~/images/calendar.gif"
                        CausesValidation="False" />
                    <asp:Label ID="dateFormat" runat="server" Text="(DD/MM/YYYY)" ForeColor="Blue" Font-Bold="True"></asp:Label>
                    <ajax:MaskedEditExtender ID="MaskedEditExtender5" runat="server" TargetControlID="InstrumentDate"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                        ErrorTooltipEnabled="True" />
                    <ajax:MaskedEditValidator ID="MaskedEditValidator5" runat="server" ControlExtender="MaskedEditExtender5"
                        ControlToValidate="InstrumentDate" EmptyValueMessage="Date is required" InvalidValueMessage="Date is invalid"
                        Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="MKE" />
                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="InstrumentDate"
                        PopupButtonID="ImgBntCalc" Format="dd/MM/yyyy" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelInstrumentAmount" runat="server" AssociatedControlID="InstrumentAmount">Instrument Amount: </asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="InstrumentAmount" runat="server" CssClass="textEntry" Height="22px"
                        Width="190px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="InstrumentAmount"
                        ErrorMessage="Not a valid amount" ValidationExpression="^\$?[0-9]+(,[0-9]{3})*(\.[0-9]{2})?$"
                        ForeColor="Red"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelInstrumentBank" runat="server" AssociatedControlID="InstrumentBankList">Instrument Bank: </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="InstrumentBankList" runat="server" Height="22px" Width="190px"
                        DataTextField="bankName" DataValueField="BankId">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelRemarks" runat="server" AssociatedControlID="txtRemarks">Remarks: </asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="textEntry" Height="61px" TextMode="MultiLine"
                        Width="333px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:ImageButton ID="SaveButton" runat="server" Height="43px" OnClientClick="return Validate()"
                        ImageUrl="~/images/backup.png" OnClick="SaveButton_Click" Width="68px" 
                        ToolTip="Submit Collection Info" />  
                </td>                  
            </tr>
        </table>
        <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none;"/>
 
   <ajaxtoolkit:modalpopupextender runat="server" id="programmaticModalPopup" behaviorid="programmaticModalPopupBehavior"
        targetcontrolid="hiddenTargetControlForModalPopup" popupcontrolid="programmaticPopup" RepositionMode="None" 
        backgroundcssclass="modalBackground" dropshadow="True" popupdraghandlecontrolid="programmaticPopupDragHandle" CancelControlID="hideModalPopupViaServer" >    

    </ajaxtoolkit:modalpopupextender>
          
    <div>
      <asp:Panel runat="server" ScrollBars="None" CssClass="modalPopup" ID="programmaticPopup" Style="display: none; height: 150;
        width: 250px; padding: 10px">
        <asp:Panel BackColor="White" runat="server" ID="pnl" style="height:25px;" >
            <asp:Label Font-Size="Large" Text="" ID="PopupHeader" runat="server"></asp:Label>             
        </asp:Panel>
        <asp:Panel  runat="server" ID="PanelBody" style="height:50px" >     
                <asp:Label Text="" ID="PopupMessage" runat="server"></asp:Label>              
        </asp:Panel> 
        <asp:Panel BackColor="LightGray" HorizontalAlign="Center" runat="server" ID="Panel1" style="height:25px; vertical-align:middle; margin-top:2px" >            
            <asp:LinkButton Width="25%" Height="100%" runat="server" ID="hideModalPopupViaServer" ForeColor="WHITE" Font-Bold="true" Text="Ok" BackColor="DarkGray"/>     
        </asp:Panel>   
      </asp:Panel>      
    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ClientList" 
                        EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ProductList" 
                        EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="CollectionTypeList" 
                        EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="SaveButton" EventName="Click" />
                </Triggers>
              </asp:UpdatePanel>
        </fieldset>
        <p class="submitButton">
            &nbsp;</p>
       
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="head">
    <%--<style type="text/css">
        .style1
        {
            height: 33px;
        }
        .textEntry
        {
        }
        .modalBackground
        {
            position: absolute;
            z-index: 100;
            top: 0px;
            left: 0px;
            background-color: Gray;
            filter: alpha(opacity=60);
            -moz-opacity: 0.6;
            opacity: 0.6;
        }
        .modalPopup
        {
            background-color: #ffffff;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
            width: 250px;
            overflow: scroll;
        }
    </style>--%>
</asp:Content>
