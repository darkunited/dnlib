﻿// dnlib: See LICENSE.txt for more info

using dnlib.DotNet.Pdb.Symbols;
using dnlib.IO;

namespace dnlib.DotNet.Pdb.Managed {
	sealed class DbiVariable : SymbolVariable {
		public override string Name => name;
		string name;

		public override PdbLocalAttributes Attributes => attributes;
		PdbLocalAttributes attributes;

		public override int Index => index;
		int index;

		public override PdbCustomDebugInfo[] CustomDebugInfos => emptyPdbCustomDebugInfos;
		static readonly PdbCustomDebugInfo[] emptyPdbCustomDebugInfos = new PdbCustomDebugInfo[0];

		public void Read(IImageStream stream) {
			index = stream.ReadInt32();
			stream.Position += 10;
			attributes = GetAttributes(stream.ReadUInt16());
			name = PdbReader.ReadCString(stream);
		}

		static PdbLocalAttributes GetAttributes(uint flags) {
			PdbLocalAttributes res = 0;
			const int fCompGenx = 4;
			if ((flags & fCompGenx) != 0)
				res |= PdbLocalAttributes.DebuggerHidden;
			return res;
		}
	}
}
