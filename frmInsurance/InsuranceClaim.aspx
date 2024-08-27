<%@ Page Title="ClaimRequest" Async="true" Language="C#" MasterPageFile="SiteLigalWorkFlow.Master" AutoEventWireup="true" CodeBehind="InsuranceClaim.aspx.cs" Inherits="onlineLegalWF.frmInsurance.InsuranceClaim" %>
<%@ Register Src="~/userControls/ucPersonSign.ascx" TagPrefix="uc1" TagName="ucPersonSign" %>
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
                    <table>
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">Document No.</label>
                            </td>
                            <td>&nbsp;</td>
                            <td class="cell_content_80PC_TL">
                                <asp:Label ID="doc_no" runat="server" CssClass="Label_md"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">BU</label>
                            </td>
                            <td>&nbsp;</td>
                            <td class="cell_content_80PC_TL">
                                <asp:DropDownList ID="ddl_bu" runat="server" CssClass="text_200" OnSelectedIndexChanged="ddl_bu_Changed" AutoPostBack="true">
                                    <asp:ListItem>-Please select-</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">Company</label>
                            </td>
                            <td>&nbsp;</td>
                            <td class="cell_content_80PC_TL">
                                <asp:TextBox ID="company_name" runat="server" CssClass="Text_600" Enabled="false"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">Incident</label>
                            </td>
                            <td>&nbsp;</td>
                            <td class="cell_content_80PC_TL">
                                <asp:TextBox ID="incident" runat="server" CssClass="Text_400"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">Date Occurred </label>
                            </td>
                            <td>&nbsp;</td>
                            <td class="cell_content_80PC_TL">
                                <table>
                                    <tr>
                                        <td class="cell_content_40PC_TL">
                                            <asp:TextBox ID="occurred_date" TextMode="Date" runat="server" CssClass="Text_200"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td class="cell_content_PC_TL">
                                            <label class="Label_md">Date Submission </label>
                                        </td>
                                        <td class="cell_content_40PC_TL">
                                            <asp:TextBox ID="submission_date" TextMode="Date" runat="server" CssClass="Text_200"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">Incident Summary</label>
                            </td>
                            <td>&nbsp;</td>
                            <td class="cell_content_80PC_TL">
                                <asp:TextBox ID="incident_summary" runat="server" TextMode="MultiLine" CssClass="Text_600" Height="150"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">Claim Result</label>
                            </td>
                            <td class="cell_content_80PC_TL" colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">Surveyor </label>
                            </td>
                            <td>&nbsp;</td>
                            <td class="cell_content_80PC_TL">
                                <table>
                                    <tr>
                                        <td class="cell_content_40PC_TL">
                                            <asp:TextBox ID="surveyor_name" runat="server" CssClass="Text_200"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td class="cell_content_PC_TL">
                                            <label class="Label_md">Company</label>
                                        </td>
                                        <td class="cell_content_40PC_TL">
                                            <asp:TextBox ID="surveyor_company" runat="server" CssClass="Text_200"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">Date of Settlement </label>
                            </td>
                            <td>&nbsp;</td>
                            <td class="cell_content_80PC_TL">
                                <table>
                                    <tr>
                                        <td class="cell_content_40PC_TL">
                                            <asp:TextBox ID="settlement_date" TextMode="Date" runat="server" CssClass="Text_200"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td class="cell_content_PC_TL">
                                            <label class="Label_md">Day of Settlement </label>
                                        </td>
                                        <td class="cell_content_40PC_TL">
                                            <asp:TextBox ID="settlement_day" TextMode="Number" runat="server" CssClass="Text_200"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <%--<td class="cell_content_20PC_TR">
                            <label class="Label_md">List Estimated losses to claim </label>
                        </td>
                        <td>&nbsp;</td>--%>
                            <td colspan="4">
                                <table style="border-collapse: collapse; font-size: 9pt; margin-left: auto; margin-right: auto;">
                                    <tr class="cell_content_100PC" style="color: White; background-color: #5D7B9D; font-weight: bold;">
                                        <td>
                                            <label>Estimated losses to claim</label><br />
                                            <label>มูลค่าความเสียหาย</label>
                                        </td>
                                        <td style="text-align: center;">
                                            <label>IAR</label><br />
                                            <label>ทรัพย์สินเสียหาย</label>
                                        </td>
                                        <td style="text-align: center;">
                                            <label>BI</label><br />
                                            <label>ธุรกิจหยุดชะงัก</label>
                                        </td>
                                        <td style="text-align: center;">
                                            <label>PL/CGL</label><br />
                                            <label>รับผิดบุคคลภายนอก</label>
                                        </td>
                                        <td style="text-align: center;">
                                            <label>PV</label><br />
                                            <label>สาเหตุทางการเมือง</label>
                                        </td>
                                        <td style="text-align: center;">
                                            <label>Total</label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Amount to claim มูลค่าเรียกร้อง (a)</label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="iar_atc" runat="server" TextMode="Number" CssClass="Text_150" OnTextChanged="AtcCalculate" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="bi_atc" runat="server" TextMode="Number" CssClass="Text_150" OnTextChanged="AtcCalculate" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="pl_cgl_atc" runat="server" TextMode="Number" CssClass="Text_150" OnTextChanged="AtcCalculate" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="pv_atc" runat="server" TextMode="Number" CssClass="Text_150" OnTextChanged="AtcCalculate" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttl_atc" runat="server" TextMode="Number" CssClass="Text_150"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Deductible ค่ารับผิดส่วนแรก (b)</label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="iar_ded" runat="server" TextMode="Number" CssClass="Text_150" OnTextChanged="DedCalculate" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="bi_ded" runat="server" TextMode="Number" CssClass="Text_150" OnTextChanged="DedCalculate" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="pl_cgl_ded" runat="server" TextMode="Number" CssClass="Text_150" OnTextChanged="DedCalculate" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="pv_ded" runat="server" TextMode="Number" CssClass="Text_150" OnTextChanged="DedCalculate" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttl_ded" runat="server" TextMode="Number" CssClass="Text_150"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Proceeds from claim มูลค่าสินไหม (C)</label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="iar_pfc" runat="server" TextMode="Number" CssClass="Text_150" OnTextChanged="PfcCalculate" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="bi_pfc" runat="server" TextMode="Number" CssClass="Text_150" OnTextChanged="PfcCalculate" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="pl_cgl_pfc" runat="server" TextMode="Number" CssClass="Text_150" OnTextChanged="PfcCalculate" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="pv_pfc" runat="server" TextMode="Number" CssClass="Text_150" OnTextChanged="PfcCalculate" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttl_pfc" runat="server" TextMode="Number" CssClass="Text_150"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Under amount to claim ส่วนต่างมูลค่าการเคลม (a-b-c)</label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="iar_uatc" runat="server" TextMode="Number" CssClass="Text_150" OnTextChanged="UatcCalculate" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="bi_uatc" runat="server" TextMode="Number" CssClass="Text_150" OnTextChanged="UatcCalculate" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="pl_cgl_uatc" runat="server" TextMode="Number" CssClass="Text_150" OnTextChanged="UatcCalculate" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="pv_uatc" runat="server" TextMode="Number" CssClass="Text_150" OnTextChanged="UatcCalculate" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttl_uatc" runat="server" TextMode="Number" CssClass="Text_150"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <%--<td>&nbsp;</td>--%>
                        </tr>
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">Remark</label>
                            </td>
                            <td>&nbsp;</td>
                            <td class="cell_content_80PC_TL">
                                <asp:TextBox ID="remark" runat="server" TextMode="MultiLine" CssClass="Text_600" Height="150"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
            </td>
        </tr>
        <%--<tr class="cell_content_100PC">
            <td colspan="2" class="cell_content_100PC">
                <label>For Property</label>
                <br />
            </td>
        </tr>
        <tr class="cell_content_100PC">
            <td colspan="2" class="cell_content_100PC">
                <asp:Panel ID="Panel2" runat="server" CssClass="div_90" Height="200px">
                    <table class="cell_content_100PC" style="font-family: Tahoma;">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Submitted By"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Supported By"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Supported By"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:ucpersonsign runat="server" id="ucPersonSignProp1" />
                            </td>
                            <td>
                                <uc1:ucpersonsign runat="server" id="ucPersonSignProp2" />
                            </td>
                            <td>
                                <uc1:ucpersonsign runat="server" id="ucPersonSignProp3" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>

        <tr class="cell_content_100PC">
            <td colspan="2" class="cell_content_100PC">
                <label>For AWC</label>
                <br />
            </td>
        </tr>
        <tr class="cell_content_100PC">
            <td colspan="2" class="cell_content_100PC">
                <asp:Panel ID="Panel3" runat="server" CssClass="div_90" Height="200px">
                    <table class="cell_content_100PC" style="font-family: Tahoma;">
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Submitted By"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Supported By"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Supported By"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:ucpersonsign runat="server" id="ucPersonSignAWC1" />
                            </td>
                            <td>
                                <uc1:ucpersonsign runat="server" id="ucPersonSignAWC2" />
                            </td>
                            <td>
                                <uc1:ucpersonsign runat="server" id="ucPersonSignAWC3" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>--%>

        <tr class="cell_content_100PC">
            <td colspan="2" class="cell_content_100PC">
                <asp:Button ID="btn_save" runat="server" CssClass="btn_normal_silver" Text="Save" OnClick="btn_save_Click" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                <%--<asp:Button ID="btn_submit" runat="server" CssClass="btn_normal_silver" Text="Submit" OnClick="btn_submit_Click" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />--%>
                <asp:Button ID="btn_gendocumnt" runat="server" CssClass="btn_normal_silver" Text="Preview" OnClick="btn_gendocumnt_Click" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                <asp:Button ID="btn_cancel" runat="server" CssClass="btn_normal_silver" Text="Cancel" OnClientClick="JavaScript:window.history.back(1); return false;" />
            </td>
        </tr>
        <tr class="cell_content_100PC">
            <td colspan="2" class="cell_content_100PC">
                <br />
                <asp:Panel ID="Panel4" runat="server" CssClass="div_90">
                    <uc3:ucAttachment runat="server" ID="ucAttachment1" />
                </asp:Panel>
                <br />
            </td>

        </tr>
        <tr class="cell_content_100PC">
            <td colspan="2" class="cell_content_100PC">
                <uc4:ucCommentlog runat="server" ID="ucCommentlog1" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="claim_no" runat="server" />
    <asp:HiddenField ID="hid_PID" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:Label ID="lblPID" runat="server" CssClass="Label_sm"></asp:Label>
    <div class="modal fade" id="modaldocument" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header" style="border: 0;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <iframe id="pdf_render" runat="server" width="865" height="600" frameborder="0"></iframe>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function showModalDoc() {
            $("#modaldocument").modal('show');
        }
    </script>
</asp:Content>
