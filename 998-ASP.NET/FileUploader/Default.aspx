<%@ Page Title="FileUpload" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FileUploader._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h3>File Upload</h3>
        <p>
            <asp:FileUpload id="FileUpLoad1" runat="server" /> 
            </p>
        <p>
            <asp:Button ID="UploadBtn" runat="server" OnClick="UploadBtn_Click" Text="Upload" Width="184px" />
        </p>
        <p>
            <asp:Label ID="Label1" runat="server" Text="upload status"></asp:Label>
        </p>
    </div>

    </asp:Content>
