﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calyptus.MVC.Helpers;
using System.Reflection;
using Calyptus.MVC.Binding.Mapping;

namespace Calyptus.MVC.Binding
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class ActionAttribute : ActionBaseAttribute
	{
		public string Verb { set { Mappings.Add(new VerbMapping(value)); } }
		public string ContentType { set { Mappings.Add(new ContentTypeMapping(value)); } }
		public string Path { set { Mappings.Add(new PathMapping(value)); } }
		
		/*
		 * GET /Keyword
		 * GET /?action=Keyword
		 * GET /?Keyword=whatever
		 * POST /   Content-Type: form/urlencoded / multipart/urlencoded    action=Keyword
		 * POST /   Content-Type: form/urlencoded / multipart/urlencoded    Keyword=whatever
		 * POST /   Content-Type: application/json      { 'action' : Keyword }
		 * */

		public override void Initialize(MethodInfo method)
		{
			//if (this.assembly == null)
			//	this.assembly = method.DeclaringType.Assembly;
		
		}
	}
}