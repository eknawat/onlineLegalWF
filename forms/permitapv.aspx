<%@ Page Title="Approve" Async="true" Language="C#" MasterPageFile="~/frmInsurance/SiteLigalWorkFlow.Master" AutoEventWireup="true" CodeBehind="permitapv.aspx.cs" Inherits="onlineLegalWF.forms.permitapv" %>
<%@ Register Src="~/userControls/ucHeader.ascx" TagPrefix="uc2" TagName="ucHeader" %>
<%@ Register Src="~/userControls/ucAttachment.ascx" TagPrefix="uc3" TagName="ucAttachment" %>
<%@ Register Src="~/userControls/ucCommentlog.ascx" TagPrefix="uc4" TagName="ucCommentlog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="cell_content_100PC" style="font-family: Tahoma;">
        <tr>
            <td colspan="2">
                <div style="background-color: gainsboro;">
                    <uc2:ucHeader runat="server" ID="ucHeader1" />
                </div>
            </td>
        </tr>
        <tr class="cell_content_100PC">
            <td colspan="2" class="cell_content_100PC">
                <asp:Panel ID="Panel1" runat="server" CssClass="div_90">
                    <table class="cell_content_100PC">
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">Subject : </label>
                            </td>
                            <td class="cell_content_80PC_TL">
                                <asp:Label ID="subject" runat="server" CssClass="Label_md"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">Description : </label>
                            </td>
                            <td class="cell_content_80PC_TL">
                                <asp:Label ID="desc" runat="server" CssClass="Label_md"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">Document No. : </label>
                            </td>
                            <td class="cell_content_80PC_TL">
                            <asp:Label ID="doc_no" runat="server" CssClass="Label_md"></asp:Label>
                            <label class="Label_md">Submitted Date : </label>
                            <asp:Label ID="req_date" runat="server" CssClass="Label_md"></asp:Label>
                            </td>
                        </tr>
                        <tr id="section_comcode" runat="server" visible="false">
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">Com Code : </label>
                            </td>
                            <td class="cell_content_80PC_TL">
                                <asp:Label ID="com_code" runat="server" CssClass="Label_md"></asp:Label>
                                <label class="Label_md">Gl : </label>
                                <asp:Label ID="gl" runat="server" CssClass="Label_md"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">Stepname : </label>
                            </td>
                            <td class="cell_content_80PC_TL">
                                <asp:Label ID="step_name" runat="server" CssClass="Label_md"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <table style="margin-left: auto; margin-right: auto;">
                                    <tr class="cell_content_100PC">
                                        <td colspan="6">
                                            <iframe id="pdf_render" runat="server" width="900" height="600" frameborder="0"></iframe>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                </asp:Panel>
                <br />
            </td>

        </tr>
        <tr class="cell_content_100PC">
            <td colspan="6" class="cell_content_100PC">
                <uc4:ucCommentlog runat="server" ID="ucCommentlog1" />
            </td>
        </tr>
        <tr class="cell_content_100PC">
            <td colspan="2" class="cell_content_100PC">
                <br />
                <asp:Panel ID="Panel3" runat="server" CssClass="div_90">
                    <uc3:ucAttachment runat="server" ID="ucAttachment1" />
                </asp:Panel>
            </td>
        </tr>
        <tr class="cell_content_100PC">
            <td colspan="2" class="cell_content_100PC">
                <br />
                <asp:Panel ID="Panel2" runat="server" CssClass="div_90">
                    <uc3:ucAttachment runat="server" ID="ucAttachment2" />
                </asp:Panel>
            </td>
        </tr>
        <tr class="cell_content_100PC">
            <td colspan="2" class="cell_content_100PC">
                <br />
                <asp:Panel ID="Panel4" runat="server" CssClass="div_90">
                    <uc3:ucAttachment runat="server" ID="ucAttachment3" />
                </asp:Panel>
            </td>
        </tr>
        <tr class="cell_content_100PC">
            <td colspan="2" class="cell_content_100PC">
                <asp:Button ID="btn_Approve" runat="server" CssClass="btn_normal_blue" Text="Approve" OnClick="btn_Approve_Click" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                <asp:Button ID="btn_assign" runat="server" CssClass="btn_normal_blue" Text="Assign" Visible="false" OnClick="btn_assign_Click" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                <asp:Button ID="btn_Accept" runat="server" CssClass="btn_normal_blue" Text="Accept" Visible="false" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" OnClick="btn_Accept_Click" />
                <asp:Button ID="btn_Submit" runat="server" CssClass="btn_normal_blue" Text="Submit" Visible="false" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" OnClick="btn_Submit_Click" />
                <asp:Button ID="btn_send_requester" runat="server" CssClass="btn_normal_red" Text="Send Requester Update" Visible="false" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" OnClick="btn_send_requester_Click" />
                <asp:Button ID="btn_Reject" runat="server" CssClass="btn_normal_red" Text="Reject" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" OnClick="btn_Reject_Click" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="req_no" runat="server" />
    <asp:HiddenField ID="hid_PID" runat="server" />
    <asp:HiddenField ID="hid_bucode" runat="server" />
    <asp:HiddenField ID="hid_external_domain" runat="server" />
    <asp:HiddenField ID="hid_islandtax" runat="server" />
    <asp:HiddenField ID="hid_issignagetax" runat="server" />
    <asp:HiddenField ID="hid_permit_license_external" runat="server" />
    <asp:HiddenField ID="hid_permit_landtax_external" runat="server" />
    <asp:HiddenField ID="hid_permit_signagetax_external" runat="server" />
    <asp:HiddenField ID="hid_permit_tradmark_external" runat="server" />
    <asp:HiddenField ID="hid_permit_energy_external" runat="server" />
    <asp:HiddenField ID="hid_permit_utility_external" runat="server" />
    <asp:HiddenField ID="hid_assto_login" runat="server" />
    <div class="modal fade" id="assignModal" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header" style="border: 0;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" style="font-family: tahoma;">Modal Assign</h4>
                </div>
                <div class="modal-body" style="font-size: 10pt; font-family: tahoma;">
                    <table>
                        <tr>
                            <td class="cell_content_10PC_TR">
                                <label class="Label_md">Assign To : </label>
                            </td>
                            <td class="cell_content_90PC_TL">
                                <asp:DropDownList ID="ddlAssign_NameList" runat="server" CssClass="Text_200">
                                    <asp:ListItem Value="naruemol.w">naruemol.w</asp:ListItem>
                                    <asp:ListItem Value="kanita.s">kanita.s</asp:ListItem>
                                    <asp:ListItem Value="pattanis.r">pattanis.r</asp:ListItem>
                                    <asp:ListItem Value="suradach.k">suradach.k</asp:ListItem>
                                    <asp:ListItem Value="pichet.ku">pichet.ku</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer" style="text-align: left; border-top: 0;">
                    <asp:Button ID="btn_update_modal" runat="server" Text="Update" CssClass="btn_normal_blue" OnClick="Assign_Update_Click" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:Label ID="lblPID" runat="server" CssClass="Label_sm"></asp:Label>
    <div class="modal fade" id="modalreject" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header" style="border: 0;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <div><asp:Label ID="Label2" runat="server" CssClass="Label_lg" Text="Comment"></asp:Label></div>
                    <asp:TextBox ID="comment" runat="server" CssClass="Text_600" TextMode="MultiLine" Height="90"></asp:TextBox>
                </div>
                <div class="modal-footer" style="text-align: left; border-top: 0;">
                    <asp:Button ID="btn_reject_submit" runat="server" Text="Submit" CssClass="btn_normal_blue" OnClick="btn_reject_submit_Click" />
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function showModalReject() {
            $("#modalreject").modal('show');
        }
        function showModalAssign() {
            $("#assignModal").modal('show');
        }
    </script>
</asp:Content>