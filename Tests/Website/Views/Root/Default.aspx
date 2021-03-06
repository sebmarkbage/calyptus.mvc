﻿<%@ Page Language="C#" CodeFile="Default.aspx.cs" Inherits="Views_Root_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DefaultView</title>
</head>
<body>
	
	<a href="<%= URL<RootController>(r => r.Index()) %>"></a>
	
	<%-- <a href='<%$ Uri: RootController[0].Index() %>' runat="server"></a> --%>
	
	[Default]
	public void Display()
	{
	}
	
	[Post("editButton")]
	public void Edit(Post p)
	{
	}
	
	[Post("deleteButton")]
	public void Delete(string bla)
	{
	
	}
	
	<%--
	<form runat="server" action='<%$ Uri: RootController.Edit(new Post { ID = postID, Name = postName, Comment = postComment }) %>'>
		<input type="text" id="postID" runat="server" />
		<input type="text" id="postName" runat="server" />
		<input type="text" id="postComment" runat="server" />
		<input type="submit" name="editButton" runat="server" />
		<input type="submit" name="deleteButton" runat="server" />
	</form>
	--%>

	<form action="">
		<input type="text" name="p.ID" /><span cs:validate="p.ID is int">Enter a number</span>
		<input type="text" name="p.Name" />
		<input type="text" name="p.Comment" />
		<input type="submit" name="editButton" />
		<input type="submit" name="deleteButton" />
	</form>

	<h1><%-- Data.Title --%></h1>
    <p><%-- URL<RootController>(r => (r.Blogs() as Blog2Controller).Index2()) --%></p>
    <p><a href="<%= URL<RootController>(r => r.Blog("My Test...! " + Data.Title).Edit(10)) %>">Test</a></p>
    <form action="postTest?ReturnUrl=%2fguestbook%2f" method="post">
		<input type="submit" name="submit" value="Skicka" />
    </form>
</body>
</html>
