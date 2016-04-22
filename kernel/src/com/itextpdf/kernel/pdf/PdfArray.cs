/*
$Id: f522ac692e901f6b9d4673840c268cdda2e10dbb $

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
using System;
using System.Collections.Generic;
using com.itextpdf.kernel;
using com.itextpdf.kernel.geom;

namespace com.itextpdf.kernel.pdf
{
	/// <summary>A representation of an array as described in the PDF specification.</summary>
	/// <remarks>
	/// A representation of an array as described in the PDF specification. A PdfArray can contain any
	/// subclass of
	/// <see cref="PdfObject"/>
	/// .
	/// </remarks>
	public class PdfArray : PdfObject, IEnumerable<PdfObject>
	{
		private const long serialVersionUID = 1617495612878046869L;

		protected internal IList<PdfObject> list;

		/// <summary>Create a new, empty PdfArray.</summary>
		public PdfArray()
			: base()
		{
			list = new List<PdfObject>();
		}

		/// <summary>
		/// Create a new PdfArray with the provided PdfObject as the first item in the
		/// array.
		/// </summary>
		/// <param name="obj">first item in the array</param>
		public PdfArray(PdfObject obj)
			: this()
		{
			list.Add(obj);
		}

		/// <summary>Create a new PdfArray.</summary>
		/// <remarks>Create a new PdfArray. The array is filled with the items of the provided PdfArray.
		/// 	</remarks>
		/// <param name="arr">PdfArray containing items that will added to this PdfArray</param>
		public PdfArray(com.itextpdf.kernel.pdf.PdfArray arr)
			: this()
		{
			list.AddAll(arr.list);
		}

		/// <summary>Create a new PdfArray.</summary>
		/// <remarks>
		/// Create a new PdfArray. The array is filled with the four values of the Rectangle in the
		/// follozing order: left, bottom, right, top.
		/// </remarks>
		/// <param name="rectangle">Rectangle whose 4 values will be added to the PdfArray</param>
		public PdfArray(Rectangle rectangle)
		{
			list = new List<PdfObject>(4);
			Add(new PdfNumber(rectangle.GetLeft()));
			Add(new PdfNumber(rectangle.GetBottom()));
			Add(new PdfNumber(rectangle.GetRight()));
			Add(new PdfNumber(rectangle.GetTop()));
		}

		/// <summary>Create a new PdfArray.</summary>
		/// <remarks>Create a new PdfArray. The PdfObjects in the list will be added to the PdfArray.
		/// 	</remarks>
		/// <param name="objects">List of PdfObjects to be added to this PdfArray</param>
		public PdfArray(IList<PdfObject> objects)
		{
			list = new List<PdfObject>(objects.Count);
			foreach (PdfObject element in objects)
			{
				Add(element);
			}
		}

		/// <summary>
		/// Create a new PdfArray filled with the values in the float[] as
		/// <see cref="PdfNumber"/>
		/// .
		/// </summary>
		/// <param name="numbers">values to be added to this PdfArray</param>
		public PdfArray(float[] numbers)
		{
			list = new List<PdfObject>(numbers.Length);
			foreach (float f in numbers)
			{
				list.Add(new PdfNumber(f));
			}
		}

		/// <summary>
		/// Create a new PdfArray filled with the values in the double[] as
		/// <see cref="PdfNumber"/>
		/// .
		/// </summary>
		/// <param name="numbers">values to be added to this PdfArray</param>
		public PdfArray(double[] numbers)
		{
			list = new List<PdfObject>(numbers.Length);
			foreach (double f in numbers)
			{
				list.Add(new PdfNumber(f));
			}
		}

		/// <summary>
		/// Create a new PdfArray filled with the values in the int[] as
		/// <see cref="PdfNumber"/>
		/// .
		/// </summary>
		/// <param name="numbers">values to be added to this PdfArray</param>
		public PdfArray(int[] numbers)
		{
			list = new List<PdfObject>(numbers.Length);
			foreach (float i in numbers)
			{
				list.Add(new PdfNumber(i));
			}
		}

		/// <summary>
		/// Create a new PdfArray filled with the values in the boolean[] as
		/// <see cref="PdfBoolean"/>
		/// .
		/// </summary>
		/// <param name="values">values to be added to this PdfArray</param>
		public PdfArray(bool[] values)
		{
			list = new List<PdfObject>(values.Length);
			foreach (bool b in values)
			{
				list.Add(new PdfBoolean(b));
			}
		}

		/// <summary>Create a new PdfArray filled with a list of Strings.</summary>
		/// <remarks>
		/// Create a new PdfArray filled with a list of Strings. The boolean value decides if the Strings
		/// should be added as
		/// <see cref="PdfName"/>
		/// (true) or as
		/// <see cref="PdfString"/>
		/// (false).
		/// </remarks>
		/// <param name="strings">list of strings to be added to the list</param>
		/// <param name="asNames">indicates whether the strings should be added as PdfName (true) or as PdfString (false)
		/// 	</param>
		public PdfArray(IList<String> strings, bool asNames)
		{
			list = new List<PdfObject>(strings.Count);
			foreach (String s in strings)
			{
				list.Add(asNames ? new PdfName(s) : new PdfString(s));
			}
		}

		public virtual int Size()
		{
			return list.Count;
		}

		public virtual bool IsEmpty()
		{
			return list.IsEmpty();
		}

		public virtual bool Contains(PdfObject o)
		{
			return list.Contains(o);
		}

		public override IEnumerator<PdfObject> GetEnumerator()
		{
			return list.GetEnumerator();
		}

		public virtual void Add(PdfObject pdfObject)
		{
			list.Add(pdfObject);
		}

		public virtual void Remove(PdfObject o)
		{
			list.Remove(o);
		}

		/// <summary>Adds the Collection of PdfObjects.</summary>
		/// <param name="c">the Collection of PdfObjects to be added</param>
		/// <seealso cref="System.Collections.IList{E}.AddAll(System.Collections.ICollection{E})
		/// 	"/>
		public virtual void AddAll(ICollection<PdfObject> c)
		{
			list.AddAll(c);
		}

		/// <summary>
		/// Adds content of the
		/// <c>PdfArray</c>
		/// .
		/// </summary>
		/// <param name="a">
		/// the
		/// <c>PdfArray</c>
		/// to be added
		/// </param>
		/// <seealso cref="System.Collections.IList{E}.AddAll(System.Collections.ICollection{E})
		/// 	"/>
		public virtual void AddAll(com.itextpdf.kernel.pdf.PdfArray a)
		{
			if (a != null)
			{
				AddAll(a.list);
			}
		}

		public virtual void Clear()
		{
			list.Clear();
		}

		/// <summary>Gets the (direct) PdfObject at the specified index.</summary>
		/// <param name="index">index of the PdfObject in the PdfArray</param>
		/// <returns>the PdfObject at the position in the PdfArray</returns>
		public virtual PdfObject Get(int index)
		{
			return Get(index, true);
		}

		/// <summary>Sets the PdfObject at the specified index in the PdfArray.</summary>
		/// <param name="index">the position to set the PdfObject</param>
		/// <param name="element">PdfObject to be added</param>
		/// <returns>true if the operation changed the PdfArray</returns>
		/// <seealso cref="System.Collections.IList{E}.Set(int, System.Object)"/>
		public virtual PdfObject Set(int index, PdfObject element)
		{
			return list[index] = element;
		}

		/// <summary>Adds the specified PdfObject qt the specified index.</summary>
		/// <remarks>Adds the specified PdfObject qt the specified index. All objects after this index will be shifted by 1.
		/// 	</remarks>
		/// <param name="index">position to insert the PdfObject</param>
		/// <param name="element">PdfObject to be added</param>
		/// <seealso cref="System.Collections.IList{E}.Insert(int, System.Object)"/>
		public virtual void Add(int index, PdfObject element)
		{
			list.Insert(index, element);
		}

		/// <summary>Removes the PdfObject at the specified index.</summary>
		/// <param name="index">position of the PdfObject to be removed</param>
		/// <returns>true if the operation changes the PdfArray</returns>
		/// <seealso cref="System.Collections.IList{E}.RemoveAt(int)"/>
		public virtual void Remove(int index)
		{
			list.RemoveAt(index);
		}

		/// <summary>Gets the first index of the specified PdfObject.</summary>
		/// <param name="o">PdfObject to find the index of</param>
		/// <returns>index of the PdfObject</returns>
		/// <seealso cref="System.Collections.IList{E}.IndexOf(System.Object)"/>
		public virtual int IndexOf(PdfObject o)
		{
			return list.IndexOf(o);
		}

		/// <summary>Returns a sublist of this PdfArray, starting at fromIndex (inclusive) and ending at toIndex (exclusive).
		/// 	</summary>
		/// <param name="fromIndex">the position of the first element in the sublist (inclusive)
		/// 	</param>
		/// <param name="toIndex">the position of the last element in the sublist (exclusive)
		/// 	</param>
		/// <returns>List of PdfObjects</returns>
		/// <seealso cref="System.Collections.IList{E}.SubList(int, int)"/>
		public virtual IList<PdfObject> SubList(int fromIndex, int toIndex)
		{
			return list.SubList(fromIndex, toIndex);
		}

		public override byte GetType()
		{
			return ARRAY;
		}

		/// <summary>Marks object to be saved as indirect.</summary>
		/// <param name="document">a document the indirect reference will belong to.</param>
		/// <returns>object itself.</returns>
		public override PdfObject MakeIndirect(PdfDocument document)
		{
			return (com.itextpdf.kernel.pdf.PdfArray)base.MakeIndirect(document);
		}

		/// <summary>Marks object to be saved as indirect.</summary>
		/// <param name="document">a document the indirect reference will belong to.</param>
		/// <returns>object itself.</returns>
		public override PdfObject MakeIndirect(PdfDocument document, PdfIndirectReference
			 reference)
		{
			return (com.itextpdf.kernel.pdf.PdfArray)base.MakeIndirect(document, reference);
		}

		/// <summary>Copies object to a specified document.</summary>
		/// <remarks>
		/// Copies object to a specified document.
		/// Works only for objects that are read from existing document, otherwise an exception is thrown.
		/// </remarks>
		/// <param name="document">document to copy object to.</param>
		/// <returns>copied object.</returns>
		public override PdfObject CopyTo(PdfDocument document)
		{
			return (com.itextpdf.kernel.pdf.PdfArray)base.CopyTo(document, true);
		}

		/// <summary>Copies object to a specified document.</summary>
		/// <remarks>
		/// Copies object to a specified document.
		/// Works only for objects that are read from existing document, otherwise an exception is thrown.
		/// </remarks>
		/// <param name="document">document to copy object to.</param>
		/// <param name="allowDuplicating">
		/// indicates if to allow copy objects which already have been copied.
		/// If object is associated with any indirect reference and allowDuplicating is false then already existing reference will be returned instead of copying object.
		/// If allowDuplicating is true then object will be copied and new indirect reference will be assigned.
		/// </param>
		/// <returns>copied object.</returns>
		public override PdfObject CopyTo(PdfDocument document, bool allowDuplicating)
		{
			return (com.itextpdf.kernel.pdf.PdfArray)base.CopyTo(document, allowDuplicating);
		}

		public override String ToString()
		{
			String @string = "[";
			foreach (PdfObject entry in list)
			{
				PdfIndirectReference indirectReference = entry.GetIndirectReference();
				@string = @string + (indirectReference == null ? entry.ToString() : indirectReference
					.ToString()) + " ";
			}
			@string += "]";
			return @string;
		}

		/// <param name="asDirect">true is to extract direct object always.</param>
		/// <exception cref="com.itextpdf.kernel.PdfException"/>
		public virtual PdfObject Get(int index, bool asDirect)
		{
			if (!asDirect)
			{
				return list[index];
			}
			else
			{
				PdfObject obj = list[index];
				if (obj.GetType() == INDIRECT_REFERENCE)
				{
					return ((PdfIndirectReference)obj).GetRefersTo(true);
				}
				else
				{
					return obj;
				}
			}
		}

		/// <summary>Returns the element at the specified index as a PdfArray.</summary>
		/// <remarks>Returns the element at the specified index as a PdfArray. If the element isn't a PdfArray, null is returned.
		/// 	</remarks>
		/// <param name="index">position of the element to be returned</param>
		/// <returns>the element at the index as a PdfArray</returns>
		public virtual com.itextpdf.kernel.pdf.PdfArray GetAsArray(int index)
		{
			PdfObject direct = Get(index, true);
			if (direct != null && direct.GetType() == PdfObject.ARRAY)
			{
				return (com.itextpdf.kernel.pdf.PdfArray)direct;
			}
			return null;
		}

		/// <summary>Returns the element at the specified index as a PdfDictionary.</summary>
		/// <remarks>Returns the element at the specified index as a PdfDictionary. If the element isn't a PdfDictionary, null is returned.
		/// 	</remarks>
		/// <param name="index">position of the element to be returned</param>
		/// <returns>the element at the index as a PdfDictionary</returns>
		public virtual PdfDictionary GetAsDictionary(int index)
		{
			PdfObject direct = Get(index, true);
			if (direct != null && direct.GetType() == PdfObject.DICTIONARY)
			{
				return (PdfDictionary)direct;
			}
			return null;
		}

		/// <summary>Returns the element at the specified index as a PdfStream.</summary>
		/// <remarks>Returns the element at the specified index as a PdfStream. If the element isn't a PdfStream, null is returned.
		/// 	</remarks>
		/// <param name="index">position of the element to be returned</param>
		/// <returns>the element at the index as a PdfStream</returns>
		public virtual PdfStream GetAsStream(int index)
		{
			PdfObject direct = Get(index, true);
			if (direct != null && direct.GetType() == PdfObject.STREAM)
			{
				return (PdfStream)direct;
			}
			return null;
		}

		/// <summary>Returns the element at the specified index as a PdfNumber.</summary>
		/// <remarks>Returns the element at the specified index as a PdfNumber. If the element isn't a PdfNumber, null is returned.
		/// 	</remarks>
		/// <param name="index">position of the element to be returned</param>
		/// <returns>the element at the index as a PdfNumber</returns>
		public virtual PdfNumber GetAsNumber(int index)
		{
			PdfObject direct = Get(index, true);
			if (direct != null && direct.GetType() == PdfObject.NUMBER)
			{
				return (PdfNumber)direct;
			}
			return null;
		}

		/// <summary>Returns the element at the specified index as a PdfName.</summary>
		/// <remarks>Returns the element at the specified index as a PdfName. If the element isn't a PdfName, null is returned.
		/// 	</remarks>
		/// <param name="index">position of the element to be returned</param>
		/// <returns>the element at the index as a PdfName</returns>
		public virtual PdfName GetAsName(int index)
		{
			PdfObject direct = Get(index, true);
			if (direct != null && direct.GetType() == PdfObject.NAME)
			{
				return (PdfName)direct;
			}
			return null;
		}

		/// <summary>Returns the element at the specified index as a PdfString.</summary>
		/// <remarks>Returns the element at the specified index as a PdfString. If the element isn't a PdfString, null is returned.
		/// 	</remarks>
		/// <param name="index">position of the element to be returned</param>
		/// <returns>the element at the index as a PdfString</returns>
		public virtual PdfString GetAsString(int index)
		{
			PdfObject direct = Get(index, true);
			if (direct != null && direct.GetType() == PdfObject.STRING)
			{
				return (PdfString)direct;
			}
			return null;
		}

		/// <summary>Returns the element at the specified index as a PdfBoolean.</summary>
		/// <remarks>Returns the element at the specified index as a PdfBoolean. If the element isn't a PdfBoolean, null is returned.
		/// 	</remarks>
		/// <param name="index">position of the element to be returned</param>
		/// <returns>the element at the index as a PdfBoolean</returns>
		public virtual PdfBoolean GetAsBoolean(int index)
		{
			PdfObject direct = Get(index, true);
			if (direct != null && direct.GetType() == PdfObject.BOOLEAN)
			{
				return (PdfBoolean)direct;
			}
			return null;
		}

		/// <summary>Returns the first four elements of this array as a PdfArray.</summary>
		/// <remarks>
		/// Returns the first four elements of this array as a PdfArray. The first four values need to be
		/// PdfNumbers, if not a PdfException will be thrown.
		/// </remarks>
		/// <returns>Rectangle of the first four values</returns>
		/// <exception cref="com.itextpdf.kernel.PdfException">if one of the first values isn't a PdfNumber
		/// 	</exception>
		public virtual Rectangle ToRectangle()
		{
			try
			{
				float x1 = GetAsNumber(0).FloatValue();
				float y1 = GetAsNumber(1).FloatValue();
				float x2 = GetAsNumber(2).FloatValue();
				float y2 = GetAsNumber(3).FloatValue();
				return new Rectangle(x1, y1, x2 - x1, y2 - y1);
			}
			catch (Exception e)
			{
				throw new PdfException(PdfException.CannotConvertPdfArrayToRectanle, e, this);
			}
		}

		protected internal override PdfObject NewInstance()
		{
			return new com.itextpdf.kernel.pdf.PdfArray();
		}

		protected internal override void CopyContent(PdfObject from, PdfDocument document
			)
		{
			base.CopyContent(from, document);
			com.itextpdf.kernel.pdf.PdfArray array = (com.itextpdf.kernel.pdf.PdfArray)from;
			foreach (PdfObject entry in array.list)
			{
				Add(entry.ProcessCopying(document, false));
			}
		}

		/// <summary>Release content of PdfArray.</summary>
		protected internal virtual void ReleaseContent()
		{
			list = null;
		}
	}
}
