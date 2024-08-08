<%@ Page Title="PermitWorkAssign" EnableEventValidation = "false" Language="C#" MasterPageFile="~/frmInsurance/SiteLigalWorkFlow.Master" AutoEventWireup="true" CodeBehind="PermitWorkAssign.aspx.cs" Inherits="onlineLegalWF.frmPermit.PermitWorkAssign" %>
<%--<%@ Register Src="~/userControls/ucWorkflowlist.ascx" TagPrefix="uc2" TagName="ucWorkflowlist" %>--%>
<%@ Register Src="~/userControls/ucHeader.ascx" TagPrefix="uc1" TagName="ucHeader" %>
<%@ Register Src="~/userControls/ucTaskList.ascx" TagPrefix="uc2" TagName="ucTaskList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="cell_content_100PC">
        <tr>
            <td colspan="2">
                <div style="background-color:gainsboro;">
                    <uc1:ucHeader runat="server" id="ucHeader1" />
                </div>
                
            </td>
        </tr>
        
        <tr>
            <td colspan="2" class="cell_content_100PC">
                <%--<uc2:ucTaskList runat="server" id="ucTaskList1" />--%>
                <asp:Panel ID="Panel1" runat="server" Height="600px" CssClass="div_90">
                    <table class="w-100">
                        <tr>
                            <td style="text-align: right;" colspan="2" class="cell_content_100PC">
                                <label>Search by Subject :</label>
                                <asp:TextBox runat="server" ID="txtSearch" AutoPostBack="true" OnTextChanged="Search" />
                                <br />
                                <label>Type of Request :</label>
                                <asp:DropDownList runat="server" ID="ddlType_of_request" CssClass="Text_200" AutoPostBack="true" OnSelectedIndexChanged="SearchByTOR">
                                    <%--<asp:ListItem Value="0">All</asp:ListItem>--%>
                                </asp:DropDownList>
                                <br />
                                <label>Status :</label>
                                <asp:DropDownList runat="server" ID="ddl_status" CssClass="Text_200" AutoPostBack="true" OnSelectedIndexChanged="SearchByStatus">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                    <asp:ListItem Value="IN PROGRESS">IN PROGRESS</asp:ListItem>
                                    <asp:ListItem Value="COMPLETED">COMPLETED</asp:ListItem>
                                </asp:DropDownList>
                                <br />
                                <br />
                                <asp:Button ID="reset" runat="server" Text="Reset" AutoPostBack="true" OnClick="Reset" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;" colspan="2" class="cell_content_100PC">
                                <asp:Button ID="btn_Export" runat="server" CssClass="btn_normal_silver" Text="Export" OnClick="btn_Export_Click" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                            </td>
                        </tr>
                        <tr>
                            <asp:GridView ID="gv1" runat="server" AutoGenerateColumns="False" CssClass="w-100 table" AllowSorting="true" AllowPaging="true" PageSize="10" OnPageIndexChanging="gv1_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Document No">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lbtnReqNo" runat="server" Font-Names="Tahoma" Font-Size="9pt" Text='<%# Bind("document_no") %>' ForeColor="#003399" NavigateUrl='<%# Bind("link_url_format") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type Request">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTypeRequest" runat="server" Font-Names="Tahoma" Font-Size="9pt" Text='<%# Bind("tof_permitreq_desc") %>' ForeColor="#003399"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Subject">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubject" runat="server" Font-Names="Tahoma" Font-Size="9pt" Text='<%# Bind("subject") %>' ForeColor="#003399"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Request By">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReqBy" runat="server" Font-Names="Tahoma" Font-Size="9pt" Text='<%# Bind("submit_by") %>' ForeColor="#003399"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submitted Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubmittedDate" runat="server" Font-Names="Tahoma" Font-Size="9pt" Text='<%# Bind("created_datetime" , "{0:dd/MM/yy}") %>' ForeColor="#003399"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Font-Names="Tahoma" Font-Size="9pt" Text='<%# Bind("wf_status") %>' ForeColor="#003399"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last updated">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLastupdated" runat="server" Font-Names="Tahoma" Font-Size="9pt" Text='<%# Bind("updated_datetime" , "{0:dd/MM/yy}") %>' ForeColor="#003399"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last updated by">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLastupdateby" runat="server" Font-Names="Tahoma" Font-Size="9pt" Text='<%# Bind("updated_by") %>' ForeColor="#003399"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Assign To">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssignto" runat="server" Font-Names="Tahoma" Font-Size="9pt" Text='<%# Bind("assto_login") %>' ForeColor="#003399"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </tr>
                    </table>
                    
                    <asp:HiddenField ID="hidLogin" runat="server" />
                    <asp:HiddenField ID="hidMode" runat="server" />
                </asp:Panel>
            </td>
        </tr>
        
    </table>
</asp:Content>
