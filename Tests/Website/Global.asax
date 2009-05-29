<%@ Application Language="C#" %>
<%@ Import Namespace="Calyptus.Mvc" %>
<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
		Routing.AddEntryController<RootController>();
    }
       
</script>
