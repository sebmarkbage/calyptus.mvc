using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Calyptus.Mvc.Readers
{
	public class MultipartReader : IEnumerable<MultipartReader.Part>
	{
		private Stream stream;
		private byte[] boundary;

		public MultipartReader(Stream stream, string boundary)
		{
			this.stream = stream;
			this.boundary = Encoding.ASCII.GetBytes("--" + boundary);
		}

		public IEnumerator<MultipartPart> GetEnumerator()
		{
			return null;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

	}
}
