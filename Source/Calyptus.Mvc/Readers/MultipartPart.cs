using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Calyptus.Mvc.Readers
{
	public class MultipartPart
	{
		public string ReadHeader(string name)
		{
			return null;
		}

		public int ReadBody(byte[] buffer, int offset, int count)
		{
			return 0;
		}
	}
}
