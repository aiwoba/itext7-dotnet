/*
$Id: f06ebf18e6ed720a2701f2e258e9cf5e1aef3977 $

This file is part of the iText (R) project.
Copyright (c) 1998-2016 iText Group NV
Authors: Bruno Lowagie, Paulo Soares, et al.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using com.itextpdf.io;
using com.itextpdf.io.log;
using com.itextpdf.kernel;
using com.itextpdf.kernel.crypto;

namespace com.itextpdf.kernel.pdf
{
	public abstract class PdfObject
	{
		private const long serialVersionUID = -3852543867469424720L;

		public const byte ARRAY = 1;

		public const byte BOOLEAN = 2;

		public const byte DICTIONARY = 3;

		public const byte LITERAL = 4;

		public const byte INDIRECT_REFERENCE = 5;

		public const byte NAME = 6;

		public const byte NULL = 7;

		public const byte NUMBER = 8;

		public const byte STREAM = 9;

		public const byte STRING = 10;

		protected internal const short FLUSHED = 1;

		protected internal const short FREE = 2;

		protected internal const short READING = 4;

		protected internal const short MODIFIED = 8;

		protected internal const short ORIGINAL_OBJECT_STREAM = 16;

		protected internal const short MUST_BE_FLUSHED = 32;

		protected internal const short MUST_BE_INDIRECT = 64;

		protected internal const short FORBID_RELEASE = 128;

		protected internal const short READ_ONLY = 256;

		/// <summary>If object is flushed the indirect reference is kept here.</summary>
		protected internal PdfIndirectReference indirectReference = null;

		/// <summary>Indicate same special states of PdfIndirectObject or PdfObject like @see Free, @see Reading, @see Modified.
		/// 	</summary>
		private short state;

		public PdfObject()
		{
		}

		// Indicates if the object has been flushed.
		// Indicates that the indirect reference of the object could be reused or have to be marked as free.
		// Indicates that definition of the indirect reference of the object still not found (e.g. keys in XRefStm).
		// Indicates that object changed (using in stamp mode).
		// Indicates that the indirect reference of the object represents ObjectStream from original document.
		// When PdfReader read ObjectStream reference marked as OriginalObjectStream
		// to avoid further reusing.
		// For internal usage only. Marks objects that shall be written to the output document.
		// Option is needed to build the correct PDF objects tree when closing the document.
		// As a result it avoids writing unused (removed) objects.
		// Indicates that the object shall be indirect when it is written to the document.
		// It is used to postpone the creation of indirect reference for the objects that shall be indirect,
		// so it is possible to create such objects without PdfDocument instance.
		// Indicates that the object is highly sensitive and we do not want to release it even if release() is called.
		// This flag can be set in stamping mode in object wrapper constructors and is automatically set when setModified
		// flag is set (we do not want to release changed objects).
		// The flag is set automatically for some wrappers that need document even in reader mode (FormFields etc).
		// Indicates that we do not want this object to be ever written into the resultant document
		// (because of multiple objects read from the same reference inconsistency).
		/// <summary>Gets object type.</summary>
		/// <returns>object type.</returns>
		public abstract byte GetType();

		/// <summary>Flushes the object to the document.</summary>
		/// <exception cref="com.itextpdf.kernel.PdfException"/>
		public void Flush()
		{
			Flush(true);
		}

		/// <summary>Flushes the object to the document.</summary>
		/// <param name="canBeInObjStm">indicates whether object can be placed into object stream.
		/// 	</param>
		/// <exception cref="com.itextpdf.kernel.PdfException"/>
		public void Flush(bool canBeInObjStm)
		{
			if (IsFlushed() || GetIndirectReference() == null)
			{
				//TODO log meaningless call of flush: object is direct or released
				//TODO also if object is mustBeIndirect log that flush call is premature
				return;
			}
			try
			{
				PdfDocument document = GetIndirectReference().GetDocument();
				if (document != null)
				{
					document.CheckIsoConformance(this, IsoKey.PDF_OBJECT);
					document.FlushObject(this, canBeInObjStm && GetType() != STREAM && GetType() != INDIRECT_REFERENCE
						 && GetIndirectReference().GetGenNumber() == 0);
				}
			}
			catch (System.IO.IOException e)
			{
				throw new PdfException(PdfException.CannotFlushObject, e, this);
			}
		}

		/// <summary>Gets the indirect reference associated with the object.</summary>
		/// <remarks>
		/// Gets the indirect reference associated with the object.
		/// The indirect reference is used when flushing object to the document.
		/// </remarks>
		/// <returns>indirect reference.</returns>
		public virtual PdfIndirectReference GetIndirectReference()
		{
			return indirectReference;
		}

		/// <summary>Checks if object is indirect.</summary>
		/// <remarks>
		/// Checks if object is indirect.
		/// <br />
		/// Note:
		/// Return value
		/// <see langword="true"/>
		/// doesn't necessarily mean that indirect reference of this object
		/// is not null at the moment. Object could be marked as indirect and
		/// be transformed to indirect on flushing.
		/// <br />
		/// E.g. all PdfStreams are transformed to indirect objects when they are written, but they don't always
		/// have indirect references at any given moment.
		/// </remarks>
		/// <returns>
		/// returns
		/// <see langword="true"/>
		/// if object is indirect or is to be indirect in the resultant document.
		/// </returns>
		public virtual bool IsIndirect()
		{
			return indirectReference != null || CheckState(com.itextpdf.kernel.pdf.PdfObject.
				MUST_BE_INDIRECT);
		}

		/// <summary>Marks object to be saved as indirect.</summary>
		/// <param name="document">a document the indirect reference will belong to.</param>
		/// <returns>object itself.</returns>
		public virtual com.itextpdf.kernel.pdf.PdfObject MakeIndirect(PdfDocument document
			, PdfIndirectReference reference)
		{
			if (document == null || indirectReference != null)
			{
				return this;
			}
			if (document.GetWriter() == null)
			{
				throw new PdfException(PdfException.ThereIsNoAssociatePdfWriterForMakingIndirects
					);
			}
			if (reference == null)
			{
				indirectReference = document.CreateNextIndirectReference();
				indirectReference.SetRefersTo(this);
			}
			else
			{
				indirectReference = reference;
				indirectReference.SetRefersTo(this);
			}
			ClearState(MUST_BE_INDIRECT);
			return this;
		}

		/// <summary>Marks object to be saved as indirect.</summary>
		/// <param name="document">a document the indirect reference will belong to.</param>
		/// <returns>object itself.</returns>
		public virtual com.itextpdf.kernel.pdf.PdfObject MakeIndirect(PdfDocument document
			)
		{
			return MakeIndirect(document, null);
		}

		/// <summary>Indicates is the object has been flushed or not.</summary>
		/// <returns>true is object has been flushed, otherwise false.</returns>
		public virtual bool IsFlushed()
		{
			PdfIndirectReference indirectReference = GetIndirectReference();
			return (indirectReference != null && indirectReference.CheckState(FLUSHED));
		}

		/// <summary>Indicates is the object has been set as modified or not.</summary>
		/// <remarks>Indicates is the object has been set as modified or not. Useful for incremental updates (e.g. appendMode).
		/// 	</remarks>
		/// <returns>true is object has been set as modified, otherwise false.</returns>
		public virtual bool IsModified()
		{
			PdfIndirectReference indirectReference = GetIndirectReference();
			return (indirectReference != null && indirectReference.CheckState(MODIFIED));
		}

		/// <summary>Creates clone of the object which belongs to the same document as original object.
		/// 	</summary>
		/// <remarks>
		/// Creates clone of the object which belongs to the same document as original object.
		/// New object shall not be used in other documents.
		/// </remarks>
		/// <returns>cloned object.</returns>
		public virtual com.itextpdf.kernel.pdf.PdfObject Clone()
		{
			com.itextpdf.kernel.pdf.PdfObject newObject = NewInstance();
			if (indirectReference != null || CheckState(MUST_BE_INDIRECT))
			{
				newObject.SetState(MUST_BE_INDIRECT);
			}
			newObject.CopyContent(this, null);
			return newObject;
		}

		/// <summary>Copies object to a specified document.</summary>
		/// <remarks>
		/// Copies object to a specified document.
		/// <br/><br/>
		/// NOTE: Works only for objects that are read from document opened in reading mode, otherwise an exception is thrown.
		/// </remarks>
		/// <param name="document">document to copy object to.</param>
		/// <returns>copied object.</returns>
		public virtual com.itextpdf.kernel.pdf.PdfObject CopyTo(PdfDocument document)
		{
			return CopyTo(document, true);
		}

		/// <summary>Copies object to a specified document.</summary>
		/// <remarks>
		/// Copies object to a specified document.
		/// <br/><br/>
		/// NOTE: Works only for objects that are read from document opened in reading mode, otherwise an exception is thrown.
		/// </remarks>
		/// <param name="document">document to copy object to.</param>
		/// <param name="allowDuplicating">
		/// indicates if to allow copy objects which already have been copied.
		/// If object is associated with any indirect reference and allowDuplicating is false then already existing reference will be returned instead of copying object.
		/// If allowDuplicating is true then object will be copied and new indirect reference will be assigned.
		/// </param>
		/// <returns>copied object.</returns>
		public virtual com.itextpdf.kernel.pdf.PdfObject CopyTo(PdfDocument document, bool
			 allowDuplicating)
		{
			if (document == null)
			{
				throw new PdfException(PdfException.DocumentToCopyToCannotBeNull);
			}
			if (indirectReference != null)
			{
				if (indirectReference.GetWriter() != null || CheckState(MUST_BE_INDIRECT))
				{
					throw new PdfException(PdfException.CannotCopyIndirectObjectFromTheDocumentThatIsBeingWritten
						);
				}
				if (!indirectReference.GetReader().IsOpenedWithFullPermission())
				{
					throw new BadPasswordException(BadPasswordException.PdfReaderNotOpenedWithOwnerPassword
						);
				}
			}
			return ProcessCopying(document, allowDuplicating);
		}

		//TODO comment! Add note about flush, modified flag and xref.
		public virtual void SetModified()
		{
			if (indirectReference != null)
			{
				indirectReference.SetState(MODIFIED);
				SetState(FORBID_RELEASE);
			}
		}

		public virtual void Release()
		{
			// In case ForbidRelease flag is set, release will not be performed.
			if (CheckState(FORBID_RELEASE))
			{
				Logger logger = LoggerFactory.GetLogger(typeof(com.itextpdf.kernel.pdf.PdfObject)
					);
				logger.Warn(LogMessageConstant.FORBID_RELEASE_IS_SET);
			}
			else
			{
				if (indirectReference != null && indirectReference.GetReader() != null && !indirectReference
					.CheckState(FLUSHED))
				{
					indirectReference.refersTo = null;
					indirectReference = null;
					SetState(READ_ONLY);
				}
			}
		}

		//TODO log reasonless call of method
		/// <summary>
		/// Checks if this <CODE>PdfObject</CODE> is of the type
		/// <CODE>PdfNull</CODE>.
		/// </summary>
		/// <returns><CODE>true</CODE> or <CODE>false</CODE></returns>
		public virtual bool IsNull()
		{
			return GetType() == NULL;
		}

		/// <summary>
		/// Checks if this <CODE>PdfObject</CODE> is of the type
		/// <CODE>PdfBoolean</CODE>.
		/// </summary>
		/// <returns><CODE>true</CODE> or <CODE>false</CODE></returns>
		public virtual bool IsBoolean()
		{
			return GetType() == BOOLEAN;
		}

		/// <summary>
		/// Checks if this <CODE>PdfObject</CODE> is of the type
		/// <CODE>PdfNumber</CODE>.
		/// </summary>
		/// <returns><CODE>true</CODE> or <CODE>false</CODE></returns>
		public virtual bool IsNumber()
		{
			return GetType() == NUMBER;
		}

		/// <summary>
		/// Checks if this <CODE>PdfObject</CODE> is of the type
		/// <CODE>PdfString</CODE>.
		/// </summary>
		/// <returns><CODE>true</CODE> or <CODE>false</CODE></returns>
		public virtual bool IsString()
		{
			return GetType() == STRING;
		}

		/// <summary>
		/// Checks if this <CODE>PdfObject</CODE> is of the type
		/// <CODE>PdfName</CODE>.
		/// </summary>
		/// <returns><CODE>true</CODE> or <CODE>false</CODE></returns>
		public virtual bool IsName()
		{
			return GetType() == NAME;
		}

		/// <summary>
		/// Checks if this <CODE>PdfObject</CODE> is of the type
		/// <CODE>PdfArray</CODE>.
		/// </summary>
		/// <returns><CODE>true</CODE> or <CODE>false</CODE></returns>
		public virtual bool IsArray()
		{
			return GetType() == ARRAY;
		}

		/// <summary>
		/// Checks if this <CODE>PdfObject</CODE> is of the type
		/// <CODE>PdfDictionary</CODE>.
		/// </summary>
		/// <returns><CODE>true</CODE> or <CODE>false</CODE></returns>
		public virtual bool IsDictionary()
		{
			return GetType() == DICTIONARY;
		}

		/// <summary>
		/// Checks if this <CODE>PdfObject</CODE> is of the type
		/// <CODE>PdfStream</CODE>.
		/// </summary>
		/// <returns><CODE>true</CODE> or <CODE>false</CODE></returns>
		public virtual bool IsStream()
		{
			return GetType() == STREAM;
		}

		/// <summary>
		/// Checks if this <CODE>PdfObject</CODE> is of the type
		/// <CODE>PdfIndirectReference</CODE>.
		/// </summary>
		/// <returns>
		/// <CODE>true</CODE> if this is an indirect reference,
		/// otherwise <CODE>false</CODE>
		/// </returns>
		public virtual bool IsIndirectReference()
		{
			return GetType() == INDIRECT_REFERENCE;
		}

		/// <summary>
		/// Checks if this <CODE>PdfObject</CODE> is of the type
		/// <CODE>PdfLiteral</CODE>.
		/// </summary>
		/// <returns>
		/// <CODE>true</CODE> if this is a literal,
		/// otherwise <CODE>false</CODE>
		/// </returns>
		public virtual bool IsLiteral()
		{
			return GetType() == LITERAL;
		}

		/// <summary>Creates new instance of object.</summary>
		/// <returns>new instance of object.</returns>
		protected internal abstract com.itextpdf.kernel.pdf.PdfObject NewInstance();

		protected internal virtual com.itextpdf.kernel.pdf.PdfObject SetIndirectReference
			(PdfIndirectReference indirectReference)
		{
			this.indirectReference = indirectReference;
			return this;
		}

		/// <summary>Checks state of the flag of current object.</summary>
		/// <param name="state">special flag to check</param>
		/// <returns>true if the state was set.</returns>
		protected internal virtual bool CheckState(short state)
		{
			return (this.state & state) == state;
		}

		/// <summary>Sets special states of current object.</summary>
		/// <param name="state">special flag of current object</param>
		protected internal virtual com.itextpdf.kernel.pdf.PdfObject SetState(short state
			)
		{
			this.state |= state;
			return this;
		}

		/// <summary>Clear state of the flag of current object.</summary>
		/// <param name="state">special flag state to clear</param>
		protected internal virtual com.itextpdf.kernel.pdf.PdfObject ClearState(short state
			)
		{
			this.state &= ~state;
			return this;
		}

		/// <summary>Copies object content from object 'from'.</summary>
		/// <param name="from">object to copy content from.</param>
		/// <param name="document">document to copy object to.</param>
		protected internal virtual void CopyContent(com.itextpdf.kernel.pdf.PdfObject from
			, PdfDocument document)
		{
			if (IsFlushed())
			{
				throw new PdfException(PdfException.CannotCopyFlushedObject, this);
			}
		}

		/// <summary>
		/// Processes two cases of object copying:
		/// <ol>
		/// <li>copying to the other document</li>
		/// <li>cloning inside of the current document</li>
		/// </ol>
		/// This two cases are distinguished by the state of <code>document</code> parameter:
		/// the second case is processed if <code>document</code> is <code>null</code>.
		/// </summary>
		/// <param name="documentTo">if not null: document to copy object to; otherwise indicates that object is to be cloned.
		/// 	</param>
		/// <param name="allowDuplicating">
		/// indicates if to allow copy objects which already have been copied.
		/// If object is associated with any indirect reference and allowDuplicating is false then already existing reference will be returned instead of copying object.
		/// If allowDuplicating is true then object will be copied and new indirect reference will be assigned.
		/// </param>
		/// <returns>copied object.</returns>
		internal virtual com.itextpdf.kernel.pdf.PdfObject ProcessCopying(PdfDocument documentTo
			, bool allowDuplicating)
		{
			if (documentTo != null)
			{
				//copyTo case
				PdfWriter writer = documentTo.GetWriter();
				if (writer == null)
				{
					throw new PdfException(PdfException.CannotCopyToDocumentOpenedInReadingMode);
				}
				return writer.CopyObject(this, documentTo, allowDuplicating);
			}
			else
			{
				//clone case
				com.itextpdf.kernel.pdf.PdfObject obj = this;
				if (obj.IsIndirectReference())
				{
					com.itextpdf.kernel.pdf.PdfObject refTo = ((PdfIndirectReference)obj).GetRefersTo
						();
					obj = refTo != null ? refTo : obj;
				}
				if (obj.IsIndirect() && !allowDuplicating)
				{
					return obj;
				}
				return obj.Clone();
			}
		}
	}
}
