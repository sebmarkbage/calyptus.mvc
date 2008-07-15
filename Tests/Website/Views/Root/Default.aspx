﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Views_Root_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DefaultView</title>
</head>
<body>
	<%= URL(() => new RootController().Index()) %>
	<h1><%= Data.Title %></h1>
    <p><%= URL<RootController>(r => (r.Blogs() as Blog2Controller).Index2()) %></p>
    <p><a href="<%= URL<RootController>(r => r.Blog("My Test...! " + Data.Title).Edit(10)) %>">Test</a></p>
    <form action="postTest?ReturnUrl=%2fguestbook%2f" method="post">
		<input type="submit" name="submit" value="Skicka" />
    </form>
</body>
</html>
