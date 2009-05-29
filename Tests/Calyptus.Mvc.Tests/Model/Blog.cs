using System;
using System.Collections.Generic;

namespace Calyptus.Mvc.Tests
{
	public class Blog
	{
		public int ID { get; set; }
		public string Title { get; set; }
		public ICollection<Post> Posts { get; set; }
	}
}
