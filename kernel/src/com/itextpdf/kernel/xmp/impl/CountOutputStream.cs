//Copyright (c) 2006, Adobe Systems Incorporated
//All rights reserved.
//
//        Redistribution and use in source and binary forms, with or without
//        modification, are permitted provided that the following conditions are met:
//        1. Redistributions of source code must retain the above copyright
//        notice, this list of conditions and the following disclaimer.
//        2. Redistributions in binary form must reproduce the above copyright
//        notice, this list of conditions and the following disclaimer in the
//        documentation and/or other materials provided with the distribution.
//        3. All advertising materials mentioning features or use of this software
//        must display the following acknowledgement:
//        This product includes software developed by the Adobe Systems Incorporated.
//        4. Neither the name of the Adobe Systems Incorporated nor the
//        names of its contributors may be used to endorse or promote products
//        derived from this software without specific prior written permission.
//
//        THIS SOFTWARE IS PROVIDED BY ADOBE SYSTEMS INCORPORATED ''AS IS'' AND ANY
//        EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//        WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//        DISCLAIMED. IN NO EVENT SHALL ADOBE SYSTEMS INCORPORATED BE LIABLE FOR ANY
//        DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//        (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//        LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//        ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//        (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//        SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//        http://www.adobe.com/devnet/xmp/library/eula-xmp-library-java.html
using System.IO;

namespace com.itextpdf.kernel.xmp.impl
{
	/// <summary>An <code>OutputStream</code> that counts the written bytes.</summary>
	/// <since>08.11.2006</since>
	public sealed class CountOutputStream : Stream
	{
		/// <summary>the decorated output stream</summary>
		private readonly Stream @out;

		/// <summary>the byte counter</summary>
		private int bytesWritten = 0;

		/// <summary>Constructor with providing the output stream to decorate.</summary>
		/// <param name="out">an <code>OutputStream</code></param>
		internal CountOutputStream(Stream @out)
		{
			this.@out = @out;
		}

		/// <summary>Counts the written bytes.</summary>
		/// <seealso cref="System.IO.Stream.Write(byte[], int, int)"/>
		/// <exception cref="System.IO.IOException"/>
		public override void Write(byte[] buf, int off, int len)
		{
			@out.Write(buf, off, len);
			bytesWritten += len;
		}

		/// <summary>Counts the written bytes.</summary>
		/// <seealso cref="System.IO.Stream.Write(byte[])"/>
		/// <exception cref="System.IO.IOException"/>
		public override void Write(byte[] buf)
		{
			@out.Write(buf);
			bytesWritten += buf.Length;
		}

		/// <summary>Counts the written bytes.</summary>
		/// <seealso cref="System.IO.Stream.Write(int)"/>
		/// <exception cref="System.IO.IOException"/>
		public override void Write(int b)
		{
			@out.Write(b);
			bytesWritten++;
		}

		/// <returns>the bytesWritten</returns>
		public int GetBytesWritten()
		{
			return bytesWritten;
		}
	}
}
