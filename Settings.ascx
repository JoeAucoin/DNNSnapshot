<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="GIBS.Modules.DNNSnapshot.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table cellspacing="0" cellpadding="2" border="0" summary="Snapshot Settings">
    <tr>
        <td class="SubHead" width="150" valign="top"><dnn:label id="lblUrlToCheck" runat="server" controlname="txtUrlToCheck" suffix=":"></dnn:label></td>
        <td valign="bottom" >
            <asp:textbox id="txtUrlToCheck" cssclass="NormalTextBox" width="300" maxlength="800" runat="server" />
        </td>
    </tr>
  
    <tr>
        <td class="SubHead" width="150" valign="top"><dnn:label id="lblEmailAddress" runat="server" controlname="txtEmailAddress" suffix=":"></dnn:label></td>
        <td valign="bottom" >
            <asp:textbox id="txtEmailAddress" cssclass="NormalTextBox" width="300" maxlength="300" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top"><dnn:label id="lblEmailSubject" runat="server" controlname="txtEmailSubject" suffix=":"></dnn:label></td>
        <td valign="bottom" >
            <asp:textbox id="txtEmailSubject" cssclass="NormalTextBox" width="300" maxlength="300" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top"><dnn:label id="lblBrowserWidth" runat="server" controlname="txtBrowserWidth" suffix=":"></dnn:label></td>
        <td valign="bottom" >
            <asp:textbox id="txtBrowserWidth" cssclass="NormalTextBox" Text="800" width="40" maxlength="4" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top"><dnn:label id="lblBrowserHeight" runat="server" controlname="txtBrowserHeight" suffix=":"></dnn:label></td>
        <td valign="bottom" >
            <asp:textbox id="txtBrowserHeight" cssclass="NormalTextBox" Text="600" width="40" maxlength="4" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top"><dnn:label id="lblThumbWidth" runat="server" controlname="txtThumbWidth" suffix=":"></dnn:label></td>
        <td valign="bottom" >
            <asp:textbox id="txtThumbWidth" cssclass="NormalTextBox" width="40" maxlength="4" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top"><dnn:label id="lblThumbHeight" runat="server" controlname="txtThumbHeight" suffix=":"></dnn:label></td>
        <td valign="bottom" >
            <asp:textbox id="txtThumbHeight" cssclass="NormalTextBox" width="40" maxlength="4" runat="server" />
        </td>
    </tr>
    <tr>
		<td class="SubHead" width="150"><dnn:label id="plDefaultImageFolder" runat="server" controlname="drpDefaultImageFolder" suffix=":"></dnn:label></td>
		<td valign="top"><asp:DropDownList id="drpDefaultImageFolder" Runat="server" width="250px" CssClass="NormalTextBox" /></td>
	</tr>
    <tr>
        <td class="SubHead" width="150" valign="top"><dnn:label id="lblAutoLoad" runat="server" controlname="txtAutoLoad" suffix=":"></dnn:label></td>
        <td valign="bottom" ><asp:RadioButtonList ID="rblAutoLoad" runat="server"  
            RepeatDirection="Horizontal"> 
            <asp:ListItem Text="Yes" Value="true"></asp:ListItem> 
            <asp:ListItem Text="No" Value="false"></asp:ListItem> 
        </asp:RadioButtonList></td>
    </tr>
</table>