<%@ Page Title="AWCRenewInusurance Memo" Async="true" Language="C#" MasterPageFile="~/frmInsurance/SiteLigalWorkFlow.Master" AutoEventWireup="true" CodeBehind="InsuranceRenewAWC.aspx.cs" Inherits="onlineLegalWF.frmInsurance.InsuranceRenewAWC" %>
<%@ Register Src="~/userControls/ucPersonSign.ascx" TagPrefix="uc1" TagName="ucPersonSign" %>
<%@ Register Src="~/userControls/ucHeader.ascx" TagPrefix="uc2" TagName="ucHeader" %>
<%@ Register Src="~/userControls/ucAttachment.ascx" TagPrefix="uc3" TagName="ucAttachment" %>
<%@ Register Src="~/userControls/ucCommentlog.ascx" TagPrefix="uc4" TagName="ucCommentlog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="cell_content_100PC">
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
                                <label class="Label_md">Documnet No. </label>
                            </td>
                            <td>&nbsp;</td>
                            <td class="cell_content_80PC_TL">
                                <asp:Label ID="doc_no" runat="server" CssClass="Label_md"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">To </label>
                            </td>
                            <td>&nbsp;</td>
                            <td class="cell_content_80PC_TL">
                                <asp:TextBox ID="to" runat="server" CssClass="Text_400"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">From </label>
                            </td>
                            <td>&nbsp;</td>
                            <td class="cell_content_80PC_TL">
                                <asp:TextBox ID="company_name" runat="server" CssClass="Text_400"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">Subject </label>
                            </td>
                            <td>&nbsp;</td>
                            <td class="cell_content_80PC_TL">
                                <asp:TextBox ID="subject" runat="server" CssClass="Text_600" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">Description </label>
                            </td>
                            <td>&nbsp;</td>
                            <td class="cell_content_80PC_TL">
                                <asp:TextBox ID="description" runat="server" CssClass="Text_600" Height="150px" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <%--<tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">List </label>
                            </td>
                            <td>&nbsp;</td>
                            <td class="cell_content_80PC_TL">
                                <table>
                                    <tr class="text_center">
                                        <td colspan="6">
                                            <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="False" CellPadding="4" Font-Names="Tahoma" Font-Size="9pt" ForeColor="#333333" GridLines="None">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="No">
                                                        <ItemTemplate>
                                                            <asp:label ID="gvList_txt1" Text='<%# Bind("No") %>' runat="server" CssClass="Text_sm"></asp:label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Property Insured">
                                                        <ItemTemplate>
                                                            <asp:label ID="gvList_txt2" Text='<%# Bind("PropertyInsured") %>' runat="server" CssClass="Text_sm"></asp:label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="IAR">
                                                        <ItemTemplate>
                                                            <asp:label ID="gvList_txt3" Text='<%# Bind("IARSumInsured") %>' runat="server" CssClass="Text_sm"></asp:label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="BI">
                                                        <ItemTemplate>
                                                            <asp:label ID="gvList_txt4" Text='<%# Bind("BISumInsured") %>' runat="server" CssClass="Text_sm"></asp:label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="CGL($)">
                                                        <ItemTemplate>
                                                            <asp:label ID="gvList_txt5" Text='<%# Bind("CGLSumInsured") %>' runat="server" CssClass="Text_sm"></asp:label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="130px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PL">
                                                        <ItemTemplate>
                                                            <asp:Label ID="gvList_txt6" Text='<%# Bind("PLSumInsured") %>' runat="server" CssClass="Text_sm"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="130px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PV">
                                                        <ItemTemplate>
                                                            <asp:label ID="gvList_txt7" Text='<%# Bind("PVSumInsured") %>' runat="server" CssClass="Text_sm"></asp:label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="LPG">
                                                        <ItemTemplate>
                                                            <asp:label ID="gvList_txt8" Text='<%# Bind("LPGSumInsured") %>' runat="server" CssClass="Text_sm"></asp:label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="D&O">
                                                        <ItemTemplate>
                                                            <asp:label ID="gvList_txt9" Text='<%# Bind("DOSumInsured") %>' runat="server" CssClass="Text_sm"></asp:label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="100px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EditRowStyle BackColor="#999999" />
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="cell_content_20PC_TR"></td>
                            <td>&nbsp;</td>
                            <td class="cell_content_80PC_TL">
                                <asp:Button ID="btn_export_doc" runat="server" Text="Export Doc" CssClass="btn_normal_silver" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" OnClick="btn_export_doc_Click" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>--%>
                        <tr>
                            <td class="cell_content_20PC_TR">
                                <label class="Label_md">Summary Insured </label>
                            </td>
                            <td>&nbsp;</td>
                            <td class="cell_content_80PC_TL">
                                <table>
                                    <tr class="text_center">
                                        <td colspan="6">
                                            <asp:GridView ID="gv1" runat="server" AutoGenerateColumns="False" CellPadding="4" Font-Names="Tahoma" Font-Size="9pt" ForeColor="#333333" GridLines="None">
                                                <alternatingrowstyle backcolor="White" forecolor="#284775" />
                                                <columns>
                                                    <asp:TemplateField HeaderText="กลุ่มธุรกิจ">
                                                        <itemtemplate>
                                                            <asp:HiddenField ID="gv1txtRow_Sort" Value='<%# Bind("Row_Sort") %>' runat="server" />
                                                            <asp:TextBox ID="gv1txtTYPE_PROP" Text='<%# Bind("TYPE_PROP") %>' runat="server" CssClass="Text_sm"></asp:TextBox>
                                                        </itemtemplate>
                                                        <itemstyle width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="IAR">
                                                        <itemtemplate>
                                                            <asp:TextBox ID="gv1txtIAR" Text='<%# Bind("IAR") %>' runat="server" TextMode="Number" CssClass="Text_sm"></asp:TextBox>
                                                        </itemtemplate>
                                                        <itemstyle width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="BI">
                                                        <itemtemplate>
                                                            <asp:TextBox ID="gv1txtBI" Text='<%# Bind("BI") %>' runat="server" TextMode="Number" CssClass="Text_sm"></asp:TextBox>
                                                        </itemtemplate>
                                                        <itemstyle width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="CGL($)">
                                                        <itemtemplate>
                                                            <asp:TextBox ID="gv1txtCGL" Text='<%# Bind("CGL") %>' runat="server" TextMode="Number" CssClass="Text_sm"></asp:TextBox>
                                                        </itemtemplate>
                                                        <itemstyle width="130px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PL">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="gv1txtPL" Text='<%# Bind("PL") %>' runat="server" TextMode="Number" CssClass="Text_sm"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="130px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PV">
                                                        <itemtemplate>
                                                            <asp:TextBox ID="gv1txtPV" Text='<%# Bind("PV") %>' runat="server" TextMode="Number" CssClass="Text_sm"></asp:TextBox>
                                                        </itemtemplate>
                                                        <itemstyle width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="LPG">
                                                        <itemtemplate>
                                                            <asp:TextBox ID="gv1txtLPG" Text='<%# Bind("LPG") %>' runat="server" TextMode="Number" CssClass="Text_sm"></asp:TextBox>
                                                        </itemtemplate>
                                                        <itemstyle width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="D&O">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="gv1txtD_O" Text='<%# Bind("D_O") %>' runat="server" TextMode="Number" CssClass="Text_sm"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="100px" />
                                                    </asp:TemplateField>
                                                </columns>
                                                <editrowstyle backcolor="#999999" />
                                                <footerstyle backcolor="#5D7B9D" font-bold="True" forecolor="White" />
                                                <headerstyle backcolor="#5D7B9D" font-bold="True" forecolor="White" />
                                                <pagerstyle backcolor="#284775" forecolor="White" horizontalalign="Center" />
                                                <rowstyle backcolor="#F7F6F3" forecolor="#333333" />
                                                <selectedrowstyle backcolor="#E2DED6" font-bold="True" forecolor="#333333" />
                                                <sortedascendingcellstyle backcolor="#E9E7E2" />
                                                <sortedascendingheaderstyle backcolor="#506C8C" />
                                                <sorteddescendingcellstyle backcolor="#FFFDF8" />
                                                <sorteddescendingheaderstyle backcolor="#6F8DAE" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
            </td>
        </tr>

        <tr class="cell_content_100PC">
            <td colspan="2" class="cell_content_100PC">
                <asp:Button ID="btn_save" runat="server" CssClass="btn_normal_silver" Text="Save" OnClick="btn_save_Click" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                <asp:Button ID="btn_submit" runat="server" CssClass="btn_normal_silver" Text="Submit" OnClick="btn_submit_Click" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                <asp:Button ID="btn_gendocumnt" runat="server" CssClass="btn_normal_silver" Text="Preview" OnClick="btn_gendocumnt_Click" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                <asp:Button ID="btn_cancel" runat="server" CssClass="btn_normal_silver" Text="Cancel" OnClientClick="JavaScript:window.history.back(1); return false;" />
            </td>

        </tr>
        <tr class="cell_content_100PC">
            <td colspan="2" class="cell_content_100PC">
                <br />
                <asp:Panel ID="Panel3" runat="server" CssClass="div_90">
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
    <asp:HiddenField ID="hid_PID" runat="server" />
    <asp:HiddenField ID="hid_reqno" runat="server" />
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
