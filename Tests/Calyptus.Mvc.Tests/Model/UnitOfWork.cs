using System;
using System.Collections.Generic;
using System.Linq;

namespace Calyptus.Mvc.Tests
{
	public class UnitOfWork
	{
		public IQueryable<User> Users { get; set; }
		public IQueryable<Blog> Blogs { get; set; }
		public IQueryable<Post> Posts { get; set; }

		public void Add(User user)
		{
		}

		public void Add(Blog blog)
		{
		}

		public void Add(Post post)
		{
		}

		public void Remove(User user)
		{
		}

		public void Remove(Blog blog)
		{
		}

		public void Remove(Post post)
		{
		}
	}
}
