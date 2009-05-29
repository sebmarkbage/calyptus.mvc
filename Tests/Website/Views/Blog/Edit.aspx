<%@ Page Language="C#" MasterPageFile="~/Views/Root/MasterPage.master"AutoEventWireup="true" CodeFile="Edit.aspx.cs" Inherits="Views_Blog_Edit" %>
<asp:Content ContentPlaceHolderID="top" runat="server">
	<h1>Blog #<%= Data.ID %></h1>
</asp:Content>
<asp:Content ContentPlaceHolderID="bottom" runat="server">
	<h2><%= Data.Title %></h2>
	<p><a href=""></a><%= URL(Data.ChangeTitle) %></p>
</asp:Content>