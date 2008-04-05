using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.MVC
{
	public class RouteTree
	{
		public ControllerNode[] EntryControllers { get; private set; }

		public RouteTree(ControllerNode[] controllers)
		{
			EntryControllers = controllers;
		}
	}

	public class ControllerNode
	{
		public Type Class { get; private set; }
		public MethodNode[] Methods { get; private set; }

		public ControllerNode(Type classType, MethodNode[] methods)
		{
			this.Class = classType;
			this.Methods = methods;
		}	
	}

	public class MethodNode
	{
		public MethodInfo Method { get; private set; }
		public ControllerNode[] ChildControllers { get; private set; }
		public string Path { get; private set; }
		public string Verb { get; private set; }

		public MethodNode(MethodInfo method, ControllerNode[] children, string path, string verb)
		{
			this.Method = method;
			this.ChildControllers = children;
			this.Path = path;
			this.Verb = verb;
		}
	}
}
