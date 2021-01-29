<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="Microsoft.SqlServer.Msxml6_interop, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91">
<script runat="server">
    private void Page_Load(object sender, System.EventArgs e)
    {
        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/TestReport.rdlc");
        ReportViewer1.LocalReport.Refresh();
    }
</script>
<form id="Form1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="false"></rsweb:ReportViewer>
</form>