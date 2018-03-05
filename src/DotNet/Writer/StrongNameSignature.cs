// dnlib: See LICENSE.txt for more info

﻿using System.IO;
using dnlib.IO;
using dnlib.PE;

namespace dnlib.DotNet.Writer {
	/// <summary>
	/// Strong name signature chunk
	/// </summary>
	public sealed class StrongNameSignature : IChunk {
		FileOffset offset;
		RVA rva;
		int size;

		/// <inheritdoc/>
		public FileOffset FileOffset => offset;

		/// <inheritdoc/>
		public RVA RVA => rva;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="size">Size of strong name signature</param>
		public StrongNameSignature(int size) => this.size = size;

		/// <inheritdoc/>
		public void SetOffset(FileOffset offset, RVA rva) {
			this.offset = offset;
			this.rva = rva;
		}

		/// <inheritdoc/>
		public uint GetFileLength() => (uint)size;

		/// <inheritdoc/>
		public uint GetVirtualSize() => GetFileLength();

		/// <inheritdoc/>
		public void WriteTo(BinaryWriter writer) => writer.WriteZeros(size);
	}
}
