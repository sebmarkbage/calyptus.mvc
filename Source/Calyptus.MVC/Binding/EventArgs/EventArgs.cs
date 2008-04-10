using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC
{
	public class ErrorEventArgs : EventArgs
	{
		public Exception Exception { get; private set; }

		private bool _handled;

		public bool Handled { get { return _handled; } set { if (value) _handled = true; else throw new Exception("Handled can only be set to true. Cannot undo earlier handle."); } }

		public ErrorEventArgs(Exception e)
		{
			Exception = e;
		}
	}

	public class BeforeActionEventArgs : EventArgs
	{
		public object Controller { get; private set; }
		public object[] Parameters { get; private set; }

		public BeforeActionEventArgs(object controller, object[] parameters)
		{
			Controller = controller;
			Parameters = parameters;
		}
	}

	public class AfterActionEventArgs : EventArgs
	{
		public object Controller { get; private set; }
		public object[] Parameters { get; private set; }
		public object Value { get; private set; }

		public AfterActionEventArgs(object controller, object[] parameters, object returnValue)
		{
			Controller = controller;
			Parameters = parameters;
			Value = returnValue;
		}
	}

	public class BeforeRenderEventArgs : EventArgs
	{
		public object Value { get; private set; }

		public BeforeRenderEventArgs(object value)
		{
			Value = value;
		}
	}

	public class AfterRenderEventArgs : EventArgs
	{
		public object Value { get; private set; }

		public AfterRenderEventArgs(object value)
		{
			Value = value;
		}
	}
}
