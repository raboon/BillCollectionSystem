<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MtbBillCollection.Pages.Login" MasterPageFile="~/Pages/MasterPage.Master"%>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">



      <div align="center" style="margin: 30px; font-weight:bold">
          <asp:Image ID="Image1" runat="server" ImageUrl="~/images/loginoo.gif" />
          <table border="0" style="text-align:left;" cellpadding="3px" >
              <tr>
                  <td>
                      <div class="curve3_body" id="column_02">
                          <div class="curve3_body2">
                              <div id="curve3_head">
                                  <div id="curve3_head2" style="height: 150px">
                                      <div class="column_field">
                                          <table border="0" style="text-align:left;" cellpadding="3px" >
                                              <tr>
                                                  <td>
                                                  </td>
                                                  <td>
                                                      <asp:Label ID="Label1" runat="server" Text="Login Name:"></asp:Label>
                                                  </td>
                                                  <td>
                                                      <asp:TextBox ID="txtLoginName" runat="server" MaxLength="10"></asp:TextBox>
                                                  </td>
                                                  <td>
                                                  </td>
                                              </tr>
                                              <tr>
                                                  <td>
                                                  </td>
                                                  <td>
                                                      <asp:Label ID="Label2" runat="server" Text="Password:"></asp:Label>
                                                  </td>
                                                  <td>
                                                      <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="8"></asp:TextBox>
                                                  </td>
                                                  <td>
                                                  </td>
                                              </tr>
                                              <tr>
                                                  <td colspan="2">
                                                  </td>
                                                  <td>
                                                      <asp:ImageButton  ID="ImageButton1" runat="server" 
                ImageUrl="~/images/button.jpg" Height="22px" Width="72px" 
                onclick="btnLogin_Click"/>
                                                  </td>
                                              </tr>
                                          </table>
                                      </div>
                                  </div>
                              </div>
                              <div id="curve3_bottom" style="clear: both">
                                  <div id="curve3_bottom2">
                                  </div>
                              </div>
                          </div>
                      </div>
                  </td>
              </tr>
              <tr>
                  <td>
                      <asp:RequiredFieldValidator ID="RequiredFieldValidatorForUserName" 
        runat="server" ControlToValidate="txtLoginName" 
        ErrorMessage="User Name Cannot be Empty"></asp:RequiredFieldValidator>
                      <br />
                      <asp:RequiredFieldValidator ID="RequiredFieldValidatorForPassword" 
        runat="server" ErrorMessage="Password cannot be empty" 
        ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                      <br />
                      <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                  </td>
              </tr>
          </table>
      </div>


</asp:Content>

