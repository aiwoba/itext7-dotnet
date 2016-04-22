/*
$Id: d9c7e8d40b5ce067e7fc78f13dd980d09a3184bb $

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
using com.itextpdf.kernel.pdf;

namespace com.itextpdf.kernel.pdf.navigation
{
	public class PdfExplicitDestination : PdfDestination
	{
		private const long serialVersionUID = -1515785642472963298L;

		public PdfExplicitDestination()
			: this(new PdfArray())
		{
		}

		public PdfExplicitDestination(PdfArray pdfObject)
			: base(pdfObject)
		{
		}

		public override PdfObject GetDestinationPage(IDictionary<String, PdfObject> names
			)
		{
			return ((PdfArray)GetPdfObject()).Get(0);
		}

		public override PdfDestination ReplaceNamedDestination(IDictionary<Object, PdfObject
			> names)
		{
			return this;
		}

		public static com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination CreateXYZ
			(PdfPage page, float left, float top, float zoom)
		{
			return Create(page, PdfName.XYZ, left, float.NaN, float.NaN, top, zoom);
		}

		public static com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination CreateXYZ
			(int pageNum, float left, float top, float zoom)
		{
			return Create(pageNum, PdfName.XYZ, left, float.NaN, float.NaN, top, zoom);
		}

		public static com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination CreateFit
			(PdfPage page)
		{
			return Create(page, PdfName.Fit, float.NaN, float.NaN, float.NaN, float.NaN, float
				.NaN);
		}

		public static com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination CreateFit
			(int pageNum)
		{
			return Create(pageNum, PdfName.Fit, float.NaN, float.NaN, float.NaN, float.NaN, float
				.NaN);
		}

		public static com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination CreateFitH
			(PdfPage page, float top)
		{
			return Create(page, PdfName.FitH, float.NaN, float.NaN, float.NaN, top, float.NaN
				);
		}

		public static com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination CreateFitH
			(int pageNum, float top)
		{
			return Create(pageNum, PdfName.FitH, float.NaN, float.NaN, float.NaN, top, float.
				NaN);
		}

		public static com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination CreateFitV
			(PdfPage page, float left)
		{
			return Create(page, PdfName.FitV, left, float.NaN, float.NaN, float.NaN, float.NaN
				);
		}

		public static com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination CreateFitV
			(int pageNum, float left)
		{
			return Create(pageNum, PdfName.FitV, left, float.NaN, float.NaN, float.NaN, float
				.NaN);
		}

		public static com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination CreateFitR
			(PdfPage page, float left, float bottom, float right, float top)
		{
			return Create(page, PdfName.FitR, left, bottom, right, top, float.NaN);
		}

		public static com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination CreateFitR
			(int pageNum, float left, float bottom, float right, float top)
		{
			return Create(pageNum, PdfName.FitR, left, bottom, right, top, float.NaN);
		}

		public static com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination CreateFitB
			(PdfPage page)
		{
			return Create(page, PdfName.FitB, float.NaN, float.NaN, float.NaN, float.NaN, float
				.NaN);
		}

		public static com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination CreateFitB
			(int pageNum)
		{
			return Create(pageNum, PdfName.FitB, float.NaN, float.NaN, float.NaN, float.NaN, 
				float.NaN);
		}

		public static com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination CreateFitBH
			(PdfPage page, float top)
		{
			return Create(page, PdfName.FitBH, float.NaN, float.NaN, float.NaN, top, float.NaN
				);
		}

		public static com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination CreateFitBH
			(int pageNum, float top)
		{
			return Create(pageNum, PdfName.FitBH, float.NaN, float.NaN, float.NaN, top, float
				.NaN);
		}

		public static com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination CreateFitBV
			(PdfPage page, float left)
		{
			return Create(page, PdfName.FitBH, left, float.NaN, float.NaN, float.NaN, float.NaN
				);
		}

		public static com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination CreateFitBV
			(int pageNum, float left)
		{
			return Create(pageNum, PdfName.FitBH, left, float.NaN, float.NaN, float.NaN, float
				.NaN);
		}

		public static com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination Create(PdfPage
			 page, PdfName type, float left, float bottom, float right, float top, float zoom
			)
		{
			return new com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination().Add(page).
				Add(type).Add(left).Add(bottom).Add(right).Add(top).Add(zoom);
		}

		public static com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination Create(int
			 pageNum, PdfName type, float left, float bottom, float right, float top, float 
			zoom)
		{
			return new com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination().Add(--pageNum
				).Add(type).Add(left).Add(bottom).Add(right).Add(top).Add(zoom);
		}

		protected internal override bool IsWrappedObjectMustBeIndirect()
		{
			return false;
		}

		private com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination Add(float value
			)
		{
			if (!float.IsNaN(value))
			{
				((PdfArray)GetPdfObject()).Add(new PdfNumber(value));
			}
			return this;
		}

		private com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination Add(int value)
		{
			((PdfArray)GetPdfObject()).Add(new PdfNumber(value));
			return this;
		}

		private com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination Add(PdfPage page
			)
		{
			((PdfArray)GetPdfObject()).Add(page.GetPdfObject());
			return this;
		}

		private com.itextpdf.kernel.pdf.navigation.PdfExplicitDestination Add(PdfName type
			)
		{
			((PdfArray)GetPdfObject()).Add(type);
			return this;
		}
	}
}
