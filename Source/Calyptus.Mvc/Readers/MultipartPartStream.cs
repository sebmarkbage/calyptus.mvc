using System.IO;
using System;

namespace Calyptus.Mvc.Readers
{

	public class MultipartPartStream : Stream
	{
		private Part part;
		long position;

		public MultipartPartStream(MultipartPart part)
		{
			this.part = part;
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
				return this.position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int readBytes = part.ReadBody(buffer, offset, count);
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