<%@ Page Title="Tracking Request" Language="C#" MasterPageFile="~/frmInsurance/SiteLigalWorkFlow.Master" AutoEventWireup="true" CodeBehind="InsuranceTrackingRequest.aspx.cs" Inherits="onlineLegalWF.frmInsurance.InsuranceTrackingRequest" %>
<%@ Register Src="~/userControls/ucHeader.ascx" TagPrefix="uc2" TagName="ucHeader" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="cell_content_100PC">
        <tr>
            <td colspan="2">
                <div style="background-color: gainsboro;">
                    <uc2:ucHeader runat="server" ID="ucHeader1" />
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="div_90">
                    <asp:GridView ID="gv1" runat="server" AutoGenerateColumns="False" CssClass="w-100 table" AllowSorting="true" AllowPaging="true" PageSize="20" OnPageIndexChanging="gv1_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="Document No">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lbtnReqNo" runat="server" Font-Names="Tahoma" Font-Size="9pt" Text='<%# Bind("document_no") %>' ForeColor="#003399" NavigateUrl='<%# Bind("link_url_format") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Bussiness Group/BU">
                                <ItemTemplate>
                                    <asp:Label ID="lblTypeRequest" runat="server" Font-Names="Tahoma" Font-Size="9pt" Text='<%# Bind("bu_desc") %>' ForeColor="#003399"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
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
                    <%--<asp:ListView ID="ListView1" runat="server">
                        <LayoutTemplate>
                            <table id="itemPlaceholderContainer" runat="server" class="table">
                                <tr class="gv_header_blue" style="height: 30px;">
                                    <th>No.</th>
                                    <th>Bussiness Group/BU</th>
                                    <th>Status</th>
                                    <th>Submitted Date</th>
                                    <th>IAR</th>
                                    <th>BI</th>
                                    <th>CGL/PL</th>
                                    <th>PV</th>
                                    <th>LPG</th>
                                    <th>D&O</th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("ProcressID") %>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("BuName") %>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("Status") %>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("RequestDate") %>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("IARSumInsured") %>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("BISumInsured") %>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("CGLPLSumInsured") %>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("PVSumInsured") %>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("LPGSumInsured") %>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("DOSumInsured") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>--%>
                    <%--<br />
                    <div>
                        <asp:Button runat="server" CssClass="btn_normal_blue pointer" Text="Approve" OnClick="Approve_Click" />
                        <asp:Button runat="server" CssClass="btn_normal_silver pointer" Text="Preview" />
                    </div>--%>
                </div>
            </td>
        </tr>
    </table>
    
    <asp:HiddenField ID="hidLogin" runat="server" />
<asp:HiddenField ID="hidMode" runat="server" />
</asp:Content>

