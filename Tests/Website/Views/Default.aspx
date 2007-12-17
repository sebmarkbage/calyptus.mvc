﻿<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%= HttpUtility.HtmlEncode(Controller.Page.Name) %></title>
</head>
<body>
    <div>
        <% foreach(Page p in Controller.Page.Children) { %>
            <a href="<%= URL(() => new PageController(Controller, p.Name).Default()) %>"><%= HttpUtility.HtmlEncode(p.Name) %></a>
        <% } %>
    </div>
    <a href="<%= URL(() => new PageController(new RootController(new CultureInfo("en-US")), Controller.Page.Name) ) %>">English</a>
    <!--
    <div c:foreach="Page p in Controller.Page.Children">
        <a c:actionhref="new PageController(Controller, p.Name).Default()"><c:content value="p.Name" /></a>
    </div>
    -->
</body>
</html>