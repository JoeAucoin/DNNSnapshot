<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewDNNSnapshot.ascx.cs" Inherits="GIBS.Modules.DNNSnapshot.ViewDNNSnapshot" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
    <div>
       
    
        <table class="table table-striped table-bordered table-list">

            <tr>
                <td></td>
            </tr>
            <tr>
                <td><dnn:label id="lblURL" runat="server" controlname="txtURL"></dnn:label>
                    <asp:TextBox ID="txtURL" runat="server" Font-Bold="True" Font-Size="8pt" 
                        Width="333px" ValidationGroup="GetURLValid"></asp:TextBox>
<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtURL"
                        ErrorMessage="URL required with or without 'http://'" Font-Bold="False" Font-Size="8pt"
                        ValidationGroup="GetURLValid" Width="244px"></asp:RequiredFieldValidator>

                </td>
            </tr>
            

            <tr>
                <td><asp:Button ID="Button2" runat="server" CssClass="btn btn-primary btn-default" OnClick="Button2_Click" Text="Get Snaphot" ValidationGroup="GetURLValid" Visible="True" />
                    </td>
            </tr>


            <tr>
                <td>
                    <dnn:label id="lblRequestTime" runat="server" controlname="lblTime" suffix=":" Visible="False"></dnn:label>
                    <asp:Label ID="lblTime" runat="server" Font-Bold="False" Font-Size="8pt" ForeColor="#C00000"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="ImageBox" runat="server" BorderColor="#404040" BorderStyle="Solid" CssClass="img-responsive"
                        BorderWidth="1px" Visible="False" /></td>
            </tr>            

        </table>
        <asp:Label ID="lblDebug" runat="server" CssClass="Normal"/>
        </div>

