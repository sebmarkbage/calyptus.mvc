using System;
using System.Collections.Generic;

namespace Calyptus.Mvc.Tests
{
	public class User
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public ICollection<Blog> Blogs { get; set; }
	}
}
