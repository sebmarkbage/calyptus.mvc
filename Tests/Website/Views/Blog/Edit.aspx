<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Edit.aspx.cs" Inherits="Views_Blog_Edit" %>
<asp:Content ContentPlaceHolderID="top" runat="server">
	<h1>Blog #<%= Data.ID %></h1>
</asp:Content>
<asp:Content ContentPlaceHolderID="bottom" runat="server">
	<h2><%= Data.Title %></h2>
	<p><%  %></p>
</asp:Content>