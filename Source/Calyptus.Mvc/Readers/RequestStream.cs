using System.IO;
using System;

namespace Calyptus.Mvc.Readers
{
	public class RequestStream : Stream
	{
		long position;
		IHttpRequest request;

		public RequestStream(IHttpRequest request)
		{
			this.request = request;
			this.position = 0;
		}

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanSeek
		{
			get { return false; }
		}

		public override bool CanWrite
		{
			get { return false; }
		}

		public override void Flush()
		{
		}

		public override long Length
		{
			get { throw new NotImplementedException(); }
		}

		public override long Position
		{
			get
			{
				return position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int readBytes = this.request.ReadBody(buffer, offset, count);
			position += readBytes;
			return readBytes;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}
	}
}