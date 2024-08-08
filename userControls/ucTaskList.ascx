<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTaskList.ascx.cs" Inherits="onlineLegalWF.userControls.ucTaskList" %>
 <link href="../StyleSheet/CustomStyle.css" rel="stylesheet" type="text/css"/>
<style type="text/css">
    .auto-style1 {
        padding: 0.5%;
        margin-left: 2.5%;
        font-family: Tahoma;
        font-size: 10pt;
        color: black;
        background-color: white;
        border: 1px solid silver;
        box-shadow: silver 2px 2px;
        border-radius: 3px 3px;
        min-height: 50px;
    }
</style>
<asp:Panel ID="Panel1" runat="server"  Height="600px" CssClass="div_90">
    <asp:GridView ID="gv1" runat="server" AutoGenerateColumns="False" CssClass="w-100 table">
        <Columns>
            <asp:TemplateField HeaderText="Document No">
                <ItemTemplate>
                    <asp:HyperLink ID="lbtnReqNo" runat="server" Font-Names="Tahoma" Font-Size="9pt" Text='<%# Bind("document_no") %>' ForeColor="#003399" NavigateUrl='<%# Bind("link_url_format") %>'></asp:HyperLink>
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

</asp:Panel>
<asp:HiddenField ID="hidLogin" runat="server" />
<asp:HiddenField ID="hidMode" runat="server" />

