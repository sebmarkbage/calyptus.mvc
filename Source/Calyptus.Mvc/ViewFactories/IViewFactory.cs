﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.Mvc
{
    public interface IViewFactory
    {
		IView FindView(IViewTemplate template);
	}
}
