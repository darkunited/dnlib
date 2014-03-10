﻿/*
    Copyright (C) 2012-2014 de4dot@gmail.com

    Permission is hereby granted, free of charge, to any person obtaining
    a copy of this software and associated documentation files (the
    "Software"), to deal in the Software without restriction, including
    without limitation the rights to use, copy, modify, merge, publish,
    distribute, sublicense, and/or sell copies of the Software, and to
    permit persons to whom the Software is furnished to do so, subject to
    the following conditions:

    The above copyright notice and this permission notice shall be
    included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
    IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
    CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
    TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
    SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.IO;
using dnlib.IO;

namespace dnlib.DotNet.MD {
	/// <summary>
	/// Stores some/all rows of a table
	/// </summary>
	abstract class HotTableStream : IDisposable {
		protected const int MAX_TABLES = (int)Table.GenericParamConstraint + 1;
		internal const uint HOT_HEAP_DIR_SIZE = 4 + MAX_TABLES * 4;

		protected readonly IImageStream fullStream;
		protected readonly long baseOffset;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fullStream">Data stream</param>
		/// <param name="baseOffset">Offset in <paramref name="fullStream"/> of start of the
		/// hot table directory header</param>
		protected HotTableStream(IImageStream fullStream, long baseOffset) {
			this.fullStream = fullStream;
			this.baseOffset = baseOffset;
		}

		/// <summary>
		/// Must be called once after creating it so it can initialize
		/// </summary>
		/// <param name="mask">Offset mask (<c>0xFFFFFFFF</c> or <c>0xFFFFFFFFFFFFFFFF</c>)</param>
		public abstract void Initialize(long mask);

		/// <summary>
		/// Returns a reader positioned at a table row
		/// </summary>
		/// <param name="table">Table</param>
		/// <param name="rid">A valid row ID (i.e., &gt;= <c>1</c> and &lt;= number of rows)</param>
		/// <returns>The reader (owned by us) or <c>null</c> if the row isn't present</returns>
		public IImageStream GetTableReader(MDTable table, uint rid) {
			long offset;
			if (GetRowOffset(table, rid, out offset)) {
				fullStream.Position = offset;
				return fullStream;
			}

			return null;
		}

		/// <summary>
		/// Returns the offset (in <see cref="fullStream"/>) of a row
		/// </summary>
		/// <param name="table">Table</param>
		/// <param name="rid">A valid row ID (i.e., &gt;= <c>1</c> and &lt;= number of rows)</param>
		/// <param name="offset">Updated with the offset</param>
		/// <returns><c>true</c> if the row exists, <c>false</c> if the row doesn't exist</returns>
		protected abstract bool GetRowOffset(MDTable table, uint rid, out long offset);

		/// <summary>
		/// Add offsets
		/// </summary>
		/// <param name="mask">Mask</param>
		/// <param name="baseOffset">Base offset</param>
		/// <param name="displ">Displacement</param>
		/// <returns>Returns <c>0</c> if <paramref name="displ"/> is <c>0</c>, else returns
		/// the sum of <paramref name="baseOffset"/> and <paramref name="displ"/> masked
		/// by <paramref name="mask"/></returns>
		protected static long AddOffsets(long mask, long baseOffset, long displ) {
			if (displ == 0)
				return 0;
			return (baseOffset + displ) & mask;
		}

		/// <inheritdoc/>
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Dispose method
		/// </summary>
		/// <param name="disposing"><c>true</c> if called by <see cref="Dispose()"/></param>
		protected virtual void Dispose(bool disposing) {
			if (disposing) {
				if (fullStream != null)
					fullStream.Dispose();
			}
		}
	}

	/// <summary>
	/// Hot table stream (CLR 2.0)
	/// </summary>
	sealed class HotTableStreamCLR20 : HotTableStream {
		TableHeader[] tableHeaders;

		class TableHeader {
			public uint numRows;
			public long posTable1;
			public long posTable2;
			public long posData;
			public int shift;
			public uint mask;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fullStream">Data stream</param>
		/// <param name="baseOffset">Offset in <paramref name="fullStream"/> of start of the
		/// hot table directory header</param>
		public HotTableStreamCLR20(IImageStream fullStream, long baseOffset)
			: base(fullStream, baseOffset) {
		}

		/// <inheritdoc/>
		[System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
		public override void Initialize(long mask) {
			tableHeaders = new TableHeader[MAX_TABLES];
			for (int i = 0; i < tableHeaders.Length; i++) {
				fullStream.Position = baseOffset + 4 + i * 4;
				int headerOffs = fullStream.ReadInt32();
				if (headerOffs == 0)
					continue;
				var headerBaseOffs = (baseOffset + headerOffs) & mask;
				fullStream.Position = headerBaseOffs;
				try {
					var header = new TableHeader {
						numRows = fullStream.ReadUInt32(),
						posTable1 = AddOffsets(mask, headerBaseOffs, fullStream.ReadInt32()),
						posTable2 = (headerBaseOffs + fullStream.ReadInt32()) & mask,
						posData = (headerBaseOffs + fullStream.ReadInt32()) & mask,
						shift = fullStream.ReadUInt16(),
					};
					header.mask = (1U << header.shift) - 1;
					tableHeaders[i] = header;
				}
				// Ignore exceptions. The CLR only reads these values when needed. Assume
				// that this was invalid data that the CLR will never read anyway.
				catch (AccessViolationException) {
				}
				catch (IOException) {
				}
			}
		}

		/// <inheritdoc/>
		protected override bool GetRowOffset(MDTable table, uint rid, out long offset) {
			offset = 0;
			if ((uint)table.Table >= (uint)tableHeaders.Length)
				return false;
			var header = tableHeaders[(int)table.Table];
			if (header == null)
				return false;

			// Check whether the whole table is in memory
			if (header.posTable1 == 0) {
				offset = header.posData + (rid - 1) * table.RowSize;
				return true;
			}

			fullStream.Position = header.posTable1 + (rid & header.mask) * 2;
			int index = fullStream.ReadUInt16();
			int stop = fullStream.ReadUInt16();
			fullStream.Position = header.posTable2 + index;
			byte highBits = (byte)(rid >> header.shift);
			while (index < stop) {
				if (fullStream.ReadByte() == highBits) {
					offset = header.posData + index * table.RowSize;
					return true;
				}
				index++;
			}

			offset = 0;
			return false;
		}
	}

	/// <summary>
	/// Hot table stream (CLR 4.0)
	/// </summary>
	sealed class HotTableStreamCLR40 : HotTableStream {
		TableHeader[] tableHeaders;

		class TableHeader {
			public uint numRows;
			public long posTable1;
			public long posTable2;
			public long posIndexes;
			public long posData;
			public int shift;
			public uint mask;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fullStream">Data stream</param>
		/// <param name="baseOffset">Offset in <paramref name="fullStream"/> of start of the
		/// hot table directory header</param>
		public HotTableStreamCLR40(IImageStream fullStream, long baseOffset)
			: base(fullStream, baseOffset) {
		}

		/// <inheritdoc/>
		[System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
		public override void Initialize(long mask) {
			tableHeaders = new TableHeader[MAX_TABLES];
			for (int i = 0; i < tableHeaders.Length; i++) {
				fullStream.Position = baseOffset + 4 + i * 4;
				int headerOffs = fullStream.ReadInt32();
				if (headerOffs == 0)
					continue;
				var headerBaseOffs = (baseOffset + headerOffs) & mask;
				fullStream.Position = headerBaseOffs;
				try {
					var header = new TableHeader {
						numRows = fullStream.ReadUInt32(),
						posTable1 = AddOffsets(mask, headerBaseOffs, fullStream.ReadInt32()),
						posTable2 = (headerBaseOffs + fullStream.ReadInt32()) & mask,
						posIndexes = (headerBaseOffs + fullStream.ReadInt32()) & mask,
						posData = (headerBaseOffs + fullStream.ReadInt32()) & mask,
						shift = fullStream.ReadUInt16(),
					};
					header.mask = (1U << header.shift) - 1;
					tableHeaders[i] = header;
				}
				// Ignore exceptions. The CLR only reads these values when needed. Assume
				// that this was invalid data that the CLR will never read anyway.
				catch (AccessViolationException) {
				}
				catch (IOException) {
				}
			}
		}

		/// <inheritdoc/>
		protected override bool GetRowOffset(MDTable table, uint rid, out long offset) {
			offset = 0;
			if ((uint)table.Table >= (uint)tableHeaders.Length)
				return false;
			var header = tableHeaders[(int)table.Table];
			if (header == null)
				return false;

			// Check whether the whole table is in memory
			if (header.posTable1 == 0) {
				offset = header.posData + (rid - 1) * table.RowSize;
				return true;
			}

			fullStream.Position = header.posTable1 + (rid & header.mask) * 2;
			int index = fullStream.ReadUInt16();
			int stop = fullStream.ReadUInt16();
			fullStream.Position = header.posTable2 + index;
			byte highBits = (byte)(rid >> header.shift);
			while (index < stop) {
				if (fullStream.ReadByte() == highBits) {
					index = fullStream.ReadUInt16At(header.posIndexes + index * 2);
					offset = header.posData + index * table.RowSize;
					return true;
				}
				index++;
			}

			offset = 0;
			return false;
		}
	}
}