<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.Master" AutoEventWireup="true" Culture="en-CA" CodeBehind="MarkForSpeacial.aspx.cs" Inherits="MtbBillCollection.Pages.MarkForSpeacial" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"  TagPrefix="ajaxtoolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">       
     

    <style type="text/css">
        .style27
        {
            height: 14px;
            width: 141px;
        }
        .style34
        {
            height: 22px;
            width: 141px;
        }
        .style35
        {
            width: 141px;
        }
        .style36
        {
        }
        .style37
        {
            width: 512px;
        }
        .style38
        {
            width: 48%;
        }
        .style39
        {
            width: 704px;
        }
        .style40
        {
            width: 375px;
        }
        .style41
        {
            height: 14px;
        }
    </style>
     

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="AdminPanel">
          <fieldset style="margin:15px"> 
            <legend><asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Large" Height="25px">Search Criteria</asp:Label> </legend>            
                <table border="0" style="text-align: left;" align="center">
                    <tr>
                        <td class="style37" colspan="5">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger controlid="cboBranchWise" 
                                        eventname="CheckedChanged" />
                                </Triggers>
                                <ContentTemplate>
                                    <table border="0" style="text-align: left; height: 136px; width: 135%;" align="center">
                                        <tr>
                                            <td class="style35">
                                                <asp:Label ID="Label3" runat="server" AssociatedControlID="ClientList" 
                                                    Text=" Client:" />
                                            </td>
                                            <td style="font-weight: 700">
                                                <asp:DropDownList ID="ClientList" runat="server" AutoPostBack="True" 
                                                    DataTextField="ClientName" DataValueField="ClientID" Height="22px" 
                                                    Width="343px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style27" valign="middle">                                                
                                                <asp:Panel ID="pnlAdvaceSrc" runat="server">
                                                    &nbsp;<asp:CheckBox ID="cboBranchWise" runat="server" Text="Branch Wise : " 
                                                        TextAlign="Left" oncheckedchanged="cboBranchWise_CheckedChanged" 
                                                        AutoPostBack="True" />
                                                    &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Panel>
                        
                                            </td>
                                            <td valign="top" class="style41">
                                                <asp:Label ID="Label6" runat="server" AssociatedControlID="cboBranch" 
                                                    Text="Branch : " />
                                                <asp:DropDownList ID="cboBranch" runat="server" DataTextField="BranchName" 
                                                    DataValueField="BranchCode" Enabled="False" Height="21px" Width="271px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style34" valign="middle">
                                                <asp:Label ID="Label5" runat="server" AssociatedControlID="cboBranch" 
                                                    Text=" Coll Type :" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cboClear" runat="server" Checked="True" Text="Cleared" />
                                                <asp:CheckBox ID="cboApprove" runat="server" Text="Approved" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style34" valign="middle">
                                                <asp:Label ID="Label8" runat="server" AssociatedControlID="ProducTypeList" 
                                                    Text=" Product Type :" />
                                            </td>
                                            <td align="left">
                                                <asp:CheckBoxList ID="ProducTypeList" runat="server" 
                                                    DataTextField="ProductName" DataValueField="ProductID" ForeColor="Black" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style34" valign="middle">
                                                <asp:Label ID="Label7" runat="server" AssociatedControlID="InstrumentTypeList" 
                                                    Text=" InstrumentType :" />
                                            </td>
                                            <td align="left">
                                                <asp:CheckBoxList ID="InstrumentTypeList" runat="server" 
                                                    DataTextField="InstTypeShortName" DataValueField="InstTypeId" 
                                                    ForeColor="Black" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                       </td>
                    </tr>        
                    <tr>
                        <td class="style39" valign="middle" colspan="1">
                            &nbsp;From&nbsp;
                        </td>
                        <td colspan="1" class="style38">
                            <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                                <ajax:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtFromDate"
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
                        <td class="style40">
                            To&nbsp;
                        </td>
                        <td width="100%" colspan="2">
                                <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                                <ajax:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtToDate"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" />
                                <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                    PopupButtonID="ImgBntCalcTodate" Format="dd/MM/yyyy" />
                                <asp:ImageButton ID="ImgBntCalcTodate" runat="server" ImageUrl="~/images/calendar.gif"
                                    CausesValidation="False" />
                                <asp:Label ID="dateFormat0" runat="server" Text="(DD/MM/yyyy)" ForeColor="Blue" 
                                    Font-Bold="True"></asp:Label>
                                <ajax:MaskedEditValidator ID="MKV2" runat="server" ControlExtender="MaskedEditExtender2"
                                    ControlToValidate="txtToDate" EmptyValueMessage="Date is required" InvalidValueMessage="Invalid Date"
                                    Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="MKE"
                                    ErrorMessage="Invalid Date" />
                        </td>
        </tr>
                    <tr>
                        <td class="style36" colspan="4">
                            <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                ErrorMessage="From date can't be bigger then To date" 
                                ControlToCompare="txtToDate" ControlToValidate="txtFromDate" ForeColor="Red" 
                                Operator="LessThanEqual" ValidationGroup="abc" Type="Date"></asp:CompareValidator>
                        </td>
                        <td>
                            <asp:ImageButton ID="ButtonSearch" runat="server" ImageUrl="~/images/Search.gif"
                                Width="42px" OnClick="ButtonSearch_Click" Height="30px" 
                                ValidationGroup="abc" />
                        </td>
                    </tr>
                </table>                
            </fieldset>
            <br />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div id="Confirmation">
                    <asp:TextBox ID="txtConfirmation" runat="server" BorderStyle="None" 
                        Font-Bold="True" ForeColor="Green" Height="16px" ReadOnly="True" 
                        Width="546px"></asp:TextBox>
                </div>
                <fieldset style="margin:15px">
                    <legend><asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Large" Height="25px">Collection For Speacial Entry</asp:Label></legend>
                    <div>              
                        <asp:GridView ID="gridCollList" runat="server" AllowSorting="True" 
                            AlternatingRowStyle-BackColor="LightGray" AutoGenerateColumns="False" 
                            BorderColor="White" BorderWidth="1px" 
                            Caption="&lt;div style=&quot;width:100%;Height:50Px;valign:top;border:1;border-color:White; background-color:GRAY&quot;&gt;&lt;br/&gt;&lt;Font Size=4; color=white&gt;&lt;b&gt; Collection Informtion&lt;/b&gt;&lt;/font&gt;&lt;br/&gt;&lt;/div&gt;" 
                            CaptionAlign="Top" CellPadding="4" DataKeyNames="CollectionId" 
                            ForeColor="#333333" GridLines="None" Height="335px" 
                            ShowFooter="True" ShowHeaderWhenEmpty="True" Width="775px">
                            <AlternatingRowStyle BackColor="White" ForeColor="#333333" />
                            <Columns>
                                <asp:BoundField DataField="CollectionId" HeaderText="Collection Id" />
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
                                <asp:TemplateField HeaderText="Edit Speacial">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnSpEdit" runat="server" ForeColor="#3399FF" 
                                            onclick="btnSpEdit_Click">Edit</asp:LinkButton>
                                    </ItemTemplate>
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
                    </div>
                </fieldset>
                <asp:Button runat="server" ID="Button1" Style="display: none;" />
                <ajaxtoolkit:modalpopupextender runat="server" id="MessegePopupExtender" behaviorid="MessegePopupExtenderBehavior"
                        targetcontrolid="Button1" popupcontrolid="MessegePopup" RepositionMode="None" 
                        backgroundcssclass="modalBackground" dropshadow="True" >    

                 </ajaxtoolkit:modalpopupextender>          
                <div>
                    <asp:Panel runat="server" ScrollBars="None" CssClass="modalPopup" ID="MessegePopup" Style="display: none; height: 300;
                            width: 350px; padding: 10px">
                        <asp:Panel BackColor="White" runat="server" ID="pnl" style="height:25px;" >
                            <asp:Label Font-Size="Large" ForeColor="Blue" BackColor="White" Text="" ID="PopupHeader" runat="server"></asp:Label>             
                        </asp:Panel>
                        <asp:Panel  runat="server" ID="Panel3" style="height:250px" >     
                            <table>
                                <tr>
                                    <td>Collection Id: </td>
                                    <td>
                                        <asp:TextBox ID="txtCollectionId" ReadOnly="true" runat="server"></asp:TextBox>
                                    </td>
                                </tr>                
                                <tr>
                                    <td>Reason: </td>
                                    <td>
                                        <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine"></asp:TextBox> 
                                    </td>
                                </tr>  
                                <tr>
                                    <td>Password: </td>
                                    <td>
                                        <asp:TextBox ID="txtPassword" TextMode="Password" runat="server"></asp:TextBox>
                                    </td>
                                </tr>              
                            </table>          
                        </asp:Panel> 
                        <asp:Panel BackColor="LightGray"  HorizontalAlign="Center" runat="server" ID="Panel4" style="height:25px; vertical-align:middle; margin-top:2px" >
                            <asp:LinkButton runat="server" ID="OkButton" ForeColor="White" Text="Ok" 
                                        BackColor="DarkGray" onclick="OkButton_Click"/>
                            <asp:LinkButton runat="server" ID="LinkButton1" ForeColor="White" Text="Cancel" 
                                        BackColor="DarkGray" />       
                        </asp:Panel>   
                      </asp:Panel>      
                </div>

                <asp:Button runat="server" ID="Button2" Style="display: none;" />
                 <ajaxtoolkit:modalpopupextender runat="server" id="ModalPopupextender" behaviorid="MessegePopupExtenderBehavior"
                    targetcontrolid="Button2" popupcontrolid="MsgPopup" RepositionMode="None" 
                    backgroundcssclass="modalBackground" dropshadow="True" >    

                </ajaxtoolkit:modalpopupextender>
          
                <div>
                  <asp:Panel runat="server" ScrollBars="None" CssClass="modalPopup" ID="MsgPopup" Style="display: none; height: 150;
                    width: 250px; padding: 10px">
                    <asp:Panel BackColor="White" runat="server" ID="PnlHeader" style="height:25px;" >
                        <asp:Label Font-Size="Large" ForeColor="Blue" BackColor="White" Text="" ID="lblHeade" runat="server"></asp:Label>             
                    </asp:Panel>
                    <asp:Panel  runat="server" ID="Panel5" style="height:50px" >     
                            <asp:Label ForeColor="Black" BackColor="White" Text="" ID="PopupMessage" runat="server"></asp:Label>              
                    </asp:Panel> 
                    <asp:Panel BackColor="LightGray"  HorizontalAlign="Center" runat="server" ID="Panel6" style="height:25px; vertical-align:middle; margin-top:2px" >
                        <asp:LinkButton runat="server" ID="LinkButton2" ForeColor="White" Text="Ok" 
                            BackColor="DarkGray" />     
                    </asp:Panel>   
                  </asp:Panel>      
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="OkButton" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ButtonSearch" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>   

    
</asp:Content>
