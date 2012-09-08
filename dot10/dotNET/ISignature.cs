﻿using System.Collections.Generic;

namespace dot10.dotNET {
	/// <summary>
	/// All signatures implement this interface
	/// </summary>
	public interface ISignature {
	}

	/// <summary>
	/// Base class for sigs with a calling convention
	/// </summary>
	public class CallingConventionSig : ISignature {
		/// <summary>
		/// The calling convention
		/// </summary>
		protected CallingConvention callingConvention;

		/// <summary>
		/// Returns true if <see cref="CallingConvention.Default"/> is set
		/// </summary>
		public bool IsDefault {
			get { return (callingConvention & CallingConvention.Mask) == CallingConvention.Default; }
		}

		/// <summary>
		/// Returns true if <see cref="CallingConvention.C"/> is set
		/// </summary>
		public bool IsC {
			get { return (callingConvention & CallingConvention.Mask) == CallingConvention.C; }
		}

		/// <summary>
		/// Returns true if <see cref="CallingConvention.StdCall"/> is set
		/// </summary>
		public bool IsStdCall {
			get { return (callingConvention & CallingConvention.Mask) == CallingConvention.StdCall; }
		}

		/// <summary>
		/// Returns true if <see cref="CallingConvention.ThisCall"/> is set
		/// </summary>
		public bool IsThisCall {
			get { return (callingConvention & CallingConvention.Mask) == CallingConvention.ThisCall; }
		}

		/// <summary>
		/// Returns true if <see cref="CallingConvention.FastCall"/> is set
		/// </summary>
		public bool IsFastCall {
			get { return (callingConvention & CallingConvention.Mask) == CallingConvention.FastCall; }
		}

		/// <summary>
		/// Returns true if <see cref="CallingConvention.VarArg"/> is set
		/// </summary>
		public bool IsVarArg {
			get { return (callingConvention & CallingConvention.Mask) == CallingConvention.VarArg; }
		}

		/// <summary>
		/// Returns true if <see cref="CallingConvention.Field"/> is set
		/// </summary>
		public bool IsField {
			get { return (callingConvention & CallingConvention.Mask) == CallingConvention.Field; }
		}

		/// <summary>
		/// Returns true if <see cref="CallingConvention.LocalSig"/> is set
		/// </summary>
		public bool IsLocalSig {
			get { return (callingConvention & CallingConvention.Mask) == CallingConvention.LocalSig; }
		}

		/// <summary>
		/// Returns true if <see cref="CallingConvention.Property"/> is set
		/// </summary>
		public bool IsProperty {
			get { return (callingConvention & CallingConvention.Mask) == CallingConvention.Property; }
		}

		/// <summary>
		/// Returns true if <see cref="CallingConvention.Unmanaged"/> is set
		/// </summary>
		public bool IsUnmanaged {
			get { return (callingConvention & CallingConvention.Mask) == CallingConvention.Unmanaged; }
		}

		/// <summary>
		/// Returns true if <see cref="CallingConvention.GenericInst"/> is set
		/// </summary>
		public bool IsGenericInst {
			get { return (callingConvention & CallingConvention.Mask) == CallingConvention.GenericInst; }
		}

		/// <summary>
		/// Returns true if <see cref="CallingConvention.NativeVarArg"/> is set
		/// </summary>
		public bool IsNativeVarArg {
			get { return (callingConvention & CallingConvention.Mask) == CallingConvention.NativeVarArg; }
		}

		/// <summary>
		/// Gets/sets the <see cref="CallingConvention.Generic"/> bit
		/// </summary>
		public bool Generic {
			get { return (callingConvention & CallingConvention.Generic) != 0; }
			set {
				if (value)
					callingConvention |= CallingConvention.Generic;
				else
					callingConvention &= ~CallingConvention.Generic;
			}
		}

		/// <summary>
		/// Gets/sets the <see cref="CallingConvention.HasThis"/> bit
		/// </summary>
		public bool HasThis {
			get { return (callingConvention & CallingConvention.HasThis) != 0; }
			set {
				if (value)
					callingConvention |= CallingConvention.HasThis;
				else
					callingConvention &= ~CallingConvention.HasThis;
			}
		}

		/// <summary>
		/// Gets/sets the <see cref="CallingConvention.ExplicitThis"/> bit
		/// </summary>
		public bool ExplicitThis {
			get { return (callingConvention & CallingConvention.ExplicitThis) != 0; }
			set {
				if (value)
					callingConvention |= CallingConvention.ExplicitThis;
				else
					callingConvention &= ~CallingConvention.ExplicitThis;
			}
		}

		/// <summary>
		/// Gets/sets the <see cref="CallingConvention.ReservedByCLR"/> bit
		/// </summary>
		public bool ReservedByCLR {
			get { return (callingConvention & CallingConvention.ReservedByCLR) != 0; }
			set {
				if (value)
					callingConvention |= CallingConvention.ReservedByCLR;
				else
					callingConvention &= ~CallingConvention.ReservedByCLR;
			}
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		protected CallingConventionSig() {
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="callingConvention">The calling convention</param>
		protected CallingConventionSig(CallingConvention callingConvention) {
			this.callingConvention = callingConvention;
		}
	}

	/// <summary>
	/// A field signature
	/// </summary>
	public sealed class FieldSig : CallingConventionSig {
		ITypeSig type;

		/// <summary>
		/// The field type
		/// </summary>
		public ITypeSig Type {
			get { return type; }
			set { type = value; }
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public FieldSig() {
			this.callingConvention = CallingConvention.Field;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="type">Field type</param>
		public FieldSig(ITypeSig type) {
			this.callingConvention = CallingConvention.Field;
			this.type = type;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="type">Field type</param>
		/// <param name="callingConvention">The calling convention (must have Field set)</param>
		internal FieldSig(CallingConvention callingConvention, ITypeSig type) {
			this.callingConvention = callingConvention;
			this.type = type;
		}

		/// <inheritdoc/>
		public override string ToString() {
			if (type == null)
				return "<<<NULL>>>";
			return type.FullName;
		}
	}

	/// <summary>
	/// Method sig base class
	/// </summary>
	public abstract class MethodBaseSig : CallingConventionSig {
		/// <summary></summary>
		protected ITypeSig retType;
		/// <summary></summary>
		protected IList<ITypeSig> parameters;
		/// <summary></summary>
		protected uint genParamCount;
		/// <summary></summary>
		protected IList<ITypeSig> paramsAfterSentinel;

		/// <summary>
		/// Gets/sets the calling convention
		/// </summary>
		public CallingConvention CallingConvention {
			get { return callingConvention; }
			set { callingConvention = value; }
		}

		/// <summary>
		/// Gets/sets the return type
		/// </summary>
		public ITypeSig RetType {
			get { return retType; }
			set { retType = value; }
		}

		/// <summary>
		/// Gets the parameters. This is never <c>null</c>
		/// </summary>
		public IList<ITypeSig> Params {
			get { return parameters; }
		}

		/// <summary>
		/// Gets/sets the generic param count
		/// </summary>
		public uint GenParamCount {
			get { return genParamCount; }
			set { genParamCount = value; }
		}

		/// <summary>
		/// Gets the parameters that are present after the sentinel. Note that this is <c>null</c>
		/// if there's no sentinel. It can still be empty even if it's not <c>null</c>.
		/// </summary>
		public IList<ITypeSig> ParamsAfterSentinel {
			get { return paramsAfterSentinel; }
			set { paramsAfterSentinel = value; }
		}
	}

	/// <summary>
	/// A method signature
	/// </summary>
	public sealed class MethodSig : MethodBaseSig {
		/// <summary>
		/// Creates a static MethodSig
		/// </summary>
		/// <param name="retType">Return type</param>
		public static MethodSig CreateStatic(ITypeSig retType) {
			return new MethodSig(CallingConvention.Default, 0, retType);
		}

		/// <summary>
		/// Creates a static MethodSig
		/// </summary>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		public static MethodSig CreateStatic(ITypeSig retType, ITypeSig argType1) {
			return new MethodSig(CallingConvention.Default, 0, retType, argType1);
		}

		/// <summary>
		/// Creates a static MethodSig
		/// </summary>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		/// <param name="argType2">Arg type #2</param>
		public static MethodSig CreateStatic(ITypeSig retType, ITypeSig argType1, ITypeSig argType2) {
			return new MethodSig(CallingConvention.Default, 0, retType, argType1, argType2);
		}

		/// <summary>
		/// Creates a static MethodSig
		/// </summary>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		/// <param name="argType2">Arg type #2</param>
		/// <param name="argType3">Arg type #3</param>
		public static MethodSig CreateStatic(ITypeSig retType, ITypeSig argType1, ITypeSig argType2, ITypeSig argType3) {
			return new MethodSig(CallingConvention.Default, 0, retType, argType1, argType2, argType3);
		}

		/// <summary>
		/// Creates a static MethodSig
		/// </summary>
		/// <param name="retType">Return type</param>
		/// <param name="argTypes">Argument types</param>
		public static MethodSig CreateStatic(ITypeSig retType, params ITypeSig[] argTypes) {
			return new MethodSig(CallingConvention.Default, 0, retType, argTypes);
		}

		/// <summary>
		/// Creates an instance MethodSig
		/// </summary>
		/// <param name="retType">Return type</param>
		public static MethodSig CreateInstance(ITypeSig retType) {
			return new MethodSig(CallingConvention.Default | CallingConvention.HasThis, 0, retType);
		}

		/// <summary>
		/// Creates an instance MethodSig
		/// </summary>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		public static MethodSig CreateInstance(ITypeSig retType, ITypeSig argType1) {
			return new MethodSig(CallingConvention.Default | CallingConvention.HasThis, 0, retType, argType1);
		}

		/// <summary>
		/// Creates an instance MethodSig
		/// </summary>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		/// <param name="argType2">Arg type #2</param>
		public static MethodSig CreateInstance(ITypeSig retType, ITypeSig argType1, ITypeSig argType2) {
			return new MethodSig(CallingConvention.Default | CallingConvention.HasThis, 0, retType, argType1, argType2);
		}

		/// <summary>
		/// Creates an instance MethodSig
		/// </summary>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		/// <param name="argType2">Arg type #2</param>
		/// <param name="argType3">Arg type #3</param>
		public static MethodSig CreateInstance(ITypeSig retType, ITypeSig argType1, ITypeSig argType2, ITypeSig argType3) {
			return new MethodSig(CallingConvention.Default | CallingConvention.HasThis, 0, retType, argType1, argType2, argType3);
		}

		/// <summary>
		/// Creates an instance MethodSig
		/// </summary>
		/// <param name="retType">Return type</param>
		/// <param name="argTypes">Argument types</param>
		public static MethodSig CreateInstance(ITypeSig retType, params ITypeSig[] argTypes) {
			return new MethodSig(CallingConvention.Default | CallingConvention.HasThis, 0, retType, argTypes);
		}

		/// <summary>
		/// Creates a static generic MethodSig
		/// </summary>
		/// <param name="genParamCount">Number of generic parameters</param>
		/// <param name="retType">Return type</param>
		public static MethodSig CreateStaticGeneric(uint genParamCount, ITypeSig retType) {
			return new MethodSig(CallingConvention.Default | CallingConvention.Generic, genParamCount, retType);
		}

		/// <summary>
		/// Creates a static generic MethodSig
		/// </summary>
		/// <param name="genParamCount">Number of generic parameters</param>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		public static MethodSig CreateStaticGeneric(uint genParamCount, ITypeSig retType, ITypeSig argType1) {
			return new MethodSig(CallingConvention.Default | CallingConvention.Generic, genParamCount, retType, argType1);
		}

		/// <summary>
		/// Creates a static generic MethodSig
		/// </summary>
		/// <param name="genParamCount">Number of generic parameters</param>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		/// <param name="argType2">Arg type #2</param>
		public static MethodSig CreateStaticGeneric(uint genParamCount, ITypeSig retType, ITypeSig argType1, ITypeSig argType2) {
			return new MethodSig(CallingConvention.Default | CallingConvention.Generic, genParamCount, retType, argType1, argType2);
		}

		/// <summary>
		/// Creates a static generic MethodSig
		/// </summary>
		/// <param name="genParamCount">Number of generic parameters</param>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		/// <param name="argType2">Arg type #2</param>
		/// <param name="argType3">Arg type #3</param>
		public static MethodSig CreateStaticGeneric(uint genParamCount, ITypeSig retType, ITypeSig argType1, ITypeSig argType2, ITypeSig argType3) {
			return new MethodSig(CallingConvention.Default | CallingConvention.Generic, genParamCount, retType, argType1, argType2, argType3);
		}

		/// <summary>
		/// Creates a static generic MethodSig
		/// </summary>
		/// <param name="genParamCount">Number of generic parameters</param>
		/// <param name="retType">Return type</param>
		/// <param name="argTypes">Argument types</param>
		public static MethodSig CreateStaticGeneric(uint genParamCount, ITypeSig retType, params ITypeSig[] argTypes) {
			return new MethodSig(CallingConvention.Default | CallingConvention.Generic, genParamCount, retType, argTypes);
		}

		/// <summary>
		/// Creates an instance generic MethodSig
		/// </summary>
		/// <param name="genParamCount">Number of generic parameters</param>
		/// <param name="retType">Return type</param>
		public static MethodSig CreateInstanceGeneric(uint genParamCount, ITypeSig retType) {
			return new MethodSig(CallingConvention.Default | CallingConvention.HasThis | CallingConvention.Generic, genParamCount, retType);
		}

		/// <summary>
		/// Creates an instance generic MethodSig
		/// </summary>
		/// <param name="genParamCount">Number of generic parameters</param>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		public static MethodSig CreateInstanceGeneric(uint genParamCount, ITypeSig retType, ITypeSig argType1) {
			return new MethodSig(CallingConvention.Default | CallingConvention.HasThis | CallingConvention.Generic, genParamCount, retType, argType1);
		}

		/// <summary>
		/// Creates an instance generic MethodSig
		/// </summary>
		/// <param name="genParamCount">Number of generic parameters</param>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		/// <param name="argType2">Arg type #2</param>
		public static MethodSig CreateInstanceGeneric(uint genParamCount, ITypeSig retType, ITypeSig argType1, ITypeSig argType2) {
			return new MethodSig(CallingConvention.Default | CallingConvention.HasThis | CallingConvention.Generic, genParamCount, retType, argType1, argType2);
		}

		/// <summary>
		/// Creates an instance generic MethodSig
		/// </summary>
		/// <param name="genParamCount">Number of generic parameters</param>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		/// <param name="argType2">Arg type #2</param>
		/// <param name="argType3">Arg type #3</param>
		public static MethodSig CreateInstanceGeneric(uint genParamCount, ITypeSig retType, ITypeSig argType1, ITypeSig argType2, ITypeSig argType3) {
			return new MethodSig(CallingConvention.Default | CallingConvention.HasThis | CallingConvention.Generic, genParamCount, retType, argType1, argType2, argType3);
		}

		/// <summary>
		/// Creates an instance generic MethodSig
		/// </summary>
		/// <param name="genParamCount">Number of generic parameters</param>
		/// <param name="retType">Return type</param>
		/// <param name="argTypes">Argument types</param>
		public static MethodSig CreateInstanceGeneric(uint genParamCount, ITypeSig retType, params ITypeSig[] argTypes) {
			return new MethodSig(CallingConvention.Default | CallingConvention.HasThis | CallingConvention.Generic, genParamCount, retType, argTypes);
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public MethodSig() {
			this.parameters = new List<ITypeSig>();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="callingConvention">Calling convention</param>
		public MethodSig(CallingConvention callingConvention) {
			this.callingConvention = callingConvention;
			this.parameters = new List<ITypeSig>();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="callingConvention">Calling convention</param>
		/// <param name="genParamCount">Number of generic parameters</param>
		public MethodSig(CallingConvention callingConvention, uint genParamCount) {
			this.callingConvention = callingConvention;
			this.genParamCount = genParamCount;
			this.parameters = new List<ITypeSig>();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="callingConvention">Calling convention</param>
		/// <param name="genParamCount">Number of generic parameters</param>
		/// <param name="retType">Return type</param>
		public MethodSig(CallingConvention callingConvention, uint genParamCount, ITypeSig retType) {
			this.callingConvention = callingConvention;
			this.genParamCount = genParamCount;
			this.retType = retType;
			this.parameters = new List<ITypeSig>();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="callingConvention">Calling convention</param>
		/// <param name="genParamCount">Number of generic parameters</param>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		public MethodSig(CallingConvention callingConvention, uint genParamCount, ITypeSig retType, ITypeSig argType1) {
			this.callingConvention = callingConvention;
			this.genParamCount = genParamCount;
			this.retType = retType;
			this.parameters = new List<ITypeSig> { argType1 };
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="callingConvention">Calling convention</param>
		/// <param name="genParamCount">Number of generic parameters</param>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		/// <param name="argType2">Arg type #2</param>
		public MethodSig(CallingConvention callingConvention, uint genParamCount, ITypeSig retType, ITypeSig argType1, ITypeSig argType2) {
			this.callingConvention = callingConvention;
			this.genParamCount = genParamCount;
			this.retType = retType;
			this.parameters = new List<ITypeSig> { argType1, argType2 };
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="callingConvention">Calling convention</param>
		/// <param name="genParamCount">Number of generic parameters</param>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		/// <param name="argType2">Arg type #2</param>
		/// <param name="argType3">Arg type #3</param>
		public MethodSig(CallingConvention callingConvention, uint genParamCount, ITypeSig retType, ITypeSig argType1, ITypeSig argType2, ITypeSig argType3) {
			this.callingConvention = callingConvention;
			this.genParamCount = genParamCount;
			this.retType = retType;
			this.parameters = new List<ITypeSig> { argType1, argType2, argType3 };
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="callingConvention">Calling convention</param>
		/// <param name="genParamCount">Number of generic parameters</param>
		/// <param name="retType">Return type</param>
		/// <param name="argTypes">Argument types</param>
		public MethodSig(CallingConvention callingConvention, uint genParamCount, ITypeSig retType, params ITypeSig[] argTypes) {
			this.callingConvention = callingConvention;
			this.genParamCount = genParamCount;
			this.retType = retType;
			this.parameters = new List<ITypeSig>(argTypes);
		}
	}

	/// <summary>
	/// A property signature
	/// </summary>
	public sealed class PropertySig : MethodBaseSig {
		/// <summary>
		/// Creates a static PropertySig
		/// </summary>
		/// <param name="retType">Return type</param>
		public static PropertySig CreateStatic(ITypeSig retType) {
			return new PropertySig(false, retType);
		}

		/// <summary>
		/// Creates a static PropertySig
		/// </summary>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		public static PropertySig CreateStatic(ITypeSig retType, ITypeSig argType1) {
			return new PropertySig(false, retType, argType1);
		}

		/// <summary>
		/// Creates a static PropertySig
		/// </summary>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		/// <param name="argType2">Arg type #2</param>
		public static PropertySig CreateStatic(ITypeSig retType, ITypeSig argType1, ITypeSig argType2) {
			return new PropertySig(false, retType, argType1, argType2);
		}

		/// <summary>
		/// Creates a static PropertySig
		/// </summary>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		/// <param name="argType2">Arg type #2</param>
		/// <param name="argType3">Arg type #3</param>
		public static PropertySig CreateStatic(ITypeSig retType, ITypeSig argType1, ITypeSig argType2, ITypeSig argType3) {
			return new PropertySig(false, retType, argType1, argType2, argType3);
		}

		/// <summary>
		/// Creates a static PropertySig
		/// </summary>
		/// <param name="retType">Return type</param>
		/// <param name="argTypes">Argument types</param>
		public static PropertySig CreateStatic(ITypeSig retType, params ITypeSig[] argTypes) {
			return new PropertySig(false, retType, argTypes);
		}

		/// <summary>
		/// Creates an instance PropertySig
		/// </summary>
		/// <param name="retType">Return type</param>
		public static PropertySig CreateInstance(ITypeSig retType) {
			return new PropertySig(true, retType);
		}

		/// <summary>
		/// Creates an instance PropertySig
		/// </summary>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		public static PropertySig CreateInstance(ITypeSig retType, ITypeSig argType1) {
			return new PropertySig(true, retType, argType1);
		}

		/// <summary>
		/// Creates an instance PropertySig
		/// </summary>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		/// <param name="argType2">Arg type #2</param>
		public static PropertySig CreateInstance(ITypeSig retType, ITypeSig argType1, ITypeSig argType2) {
			return new PropertySig(true, retType, argType1, argType2);
		}

		/// <summary>
		/// Creates an instance PropertySig
		/// </summary>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		/// <param name="argType2">Arg type #2</param>
		/// <param name="argType3">Arg type #3</param>
		public static PropertySig CreateInstance(ITypeSig retType, ITypeSig argType1, ITypeSig argType2, ITypeSig argType3) {
			return new PropertySig(true, retType, argType1, argType2, argType3);
		}

		/// <summary>
		/// Creates an instance PropertySig
		/// </summary>
		/// <param name="retType">Return type</param>
		/// <param name="argTypes">Argument types</param>
		public static PropertySig CreateInstance(ITypeSig retType, params ITypeSig[] argTypes) {
			return new PropertySig(true, retType, argTypes);
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public PropertySig() {
			this.callingConvention = CallingConvention.Property;
			this.parameters = new List<ITypeSig>();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="callingConvention">Calling convention (must have Property set)</param>
		internal PropertySig(CallingConvention callingConvention) {
			this.callingConvention = callingConvention;
			this.parameters = new List<ITypeSig>();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="hasThis"><c>true</c> if instance, <c>false</c> if static</param>
		public PropertySig(bool hasThis) {
			this.callingConvention = CallingConvention.Property | (hasThis ? CallingConvention.HasThis : 0);
			this.parameters = new List<ITypeSig>();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="hasThis"><c>true</c> if instance, <c>false</c> if static</param>
		/// <param name="retType">Return type</param>
		public PropertySig(bool hasThis, ITypeSig retType) {
			this.callingConvention = CallingConvention.Property | (hasThis ? CallingConvention.HasThis : 0);
			this.retType = retType;
			this.parameters = new List<ITypeSig>();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="hasThis"><c>true</c> if instance, <c>false</c> if static</param>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		public PropertySig(bool hasThis, ITypeSig retType, ITypeSig argType1) {
			this.callingConvention = CallingConvention.Property | (hasThis ? CallingConvention.HasThis : 0);
			this.retType = retType;
			this.parameters = new List<ITypeSig> { argType1 };
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="hasThis"><c>true</c> if instance, <c>false</c> if static</param>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		/// <param name="argType2">Arg type #2</param>
		public PropertySig(bool hasThis, ITypeSig retType, ITypeSig argType1, ITypeSig argType2) {
			this.callingConvention = CallingConvention.Property | (hasThis ? CallingConvention.HasThis : 0);
			this.retType = retType;
			this.parameters = new List<ITypeSig> { argType1, argType2 };
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="hasThis"><c>true</c> if instance, <c>false</c> if static</param>
		/// <param name="retType">Return type</param>
		/// <param name="argType1">Arg type #1</param>
		/// <param name="argType2">Arg type #2</param>
		/// <param name="argType3">Arg type #3</param>
		public PropertySig(bool hasThis, ITypeSig retType, ITypeSig argType1, ITypeSig argType2, ITypeSig argType3) {
			this.callingConvention = CallingConvention.Property | (hasThis ? CallingConvention.HasThis : 0);
			this.retType = retType;
			this.parameters = new List<ITypeSig> { argType1, argType2, argType3 };
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="hasThis"><c>true</c> if instance, <c>false</c> if static</param>
		/// <param name="retType">Return type</param>
		/// <param name="argTypes">Argument types</param>
		public PropertySig(bool hasThis, ITypeSig retType, params ITypeSig[] argTypes) {
			this.callingConvention = CallingConvention.Property | (hasThis ? CallingConvention.HasThis : 0);
			this.retType = retType;
			this.parameters = new List<ITypeSig>(argTypes);
		}
	}

	/// <summary>
	/// A local variables signature
	/// </summary>
	public sealed class LocalSig : CallingConventionSig {
		readonly IList<ITypeSig> locals;

		/// <summary>
		/// All local types. This is never <c>null</c>.
		/// </summary>
		public IList<ITypeSig> Locals {
			get { return locals; }
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public LocalSig() {
			this.callingConvention = CallingConvention.LocalSig;
			this.locals = new List<ITypeSig>();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="callingConvention">Calling convention (must have LocalSig set)</param>
		/// <param name="count">Number of locals</param>
		internal LocalSig(CallingConvention callingConvention, uint count) {
			this.callingConvention = callingConvention;
			this.locals = new List<ITypeSig>((int)count);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="local1">Local type #1</param>
		public LocalSig(ITypeSig local1) {
			this.callingConvention = CallingConvention.LocalSig;
			this.locals = new List<ITypeSig> { local1 };
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="local1">Local type #1</param>
		/// <param name="local2">Local type #2</param>
		public LocalSig(ITypeSig local1, ITypeSig local2) {
			this.callingConvention = CallingConvention.LocalSig;
			this.locals = new List<ITypeSig> { local1, local2 };
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="local1">Local type #1</param>
		/// <param name="local2">Local type #2</param>
		/// <param name="local3">Local type #3</param>
		public LocalSig(ITypeSig local1, ITypeSig local2, ITypeSig local3) {
			this.callingConvention = CallingConvention.LocalSig;
			this.locals = new List<ITypeSig> { local1, local2, local3 };
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="locals">All locals</param>
		public LocalSig(params ITypeSig[] locals) {
			this.callingConvention = CallingConvention.LocalSig;
			this.locals = new List<ITypeSig>(locals);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="locals">All locals</param>
		public LocalSig(IList<ITypeSig> locals) {
			this.callingConvention = CallingConvention.LocalSig;
			this.locals = new List<ITypeSig>(locals);
		}
	}

	/// <summary>
	/// An instantiated generic method signature
	/// </summary>
	public sealed class GenericInstMethodSig : CallingConventionSig {
		readonly IList<ITypeSig> genericArgs;

		/// <summary>
		/// Gets the generic arguments (must be instantiated types, i.e., closed types)
		/// </summary>
		public IList<ITypeSig> GenericArguments {
			get { return genericArgs; }
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public GenericInstMethodSig() {
			this.callingConvention = CallingConvention.GenericInst;
			this.genericArgs = new List<ITypeSig>();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="callingConvention">Calling convention (must have GenericInst set)</param>
		/// <param name="size">Number of generic args</param>
		internal GenericInstMethodSig(CallingConvention callingConvention, uint size) {
			this.callingConvention = callingConvention;
			this.genericArgs = new List<ITypeSig>((int)size);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="arg1">Generic arg #1</param>
		public GenericInstMethodSig(ITypeSig arg1) {
			this.callingConvention = CallingConvention.GenericInst;
			this.genericArgs = new List<ITypeSig> { arg1 };
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="arg1">Generic arg #1</param>
		/// <param name="arg2">Generic arg #2</param>
		public GenericInstMethodSig(ITypeSig arg1, ITypeSig arg2) {
			this.callingConvention = CallingConvention.GenericInst;
			this.genericArgs = new List<ITypeSig> { arg1, arg2 };
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="arg1">Generic arg #1</param>
		/// <param name="arg2">Generic arg #2</param>
		/// <param name="arg3">Generic arg #3</param>
		public GenericInstMethodSig(ITypeSig arg1, ITypeSig arg2, ITypeSig arg3) {
			this.callingConvention = CallingConvention.GenericInst;
			this.genericArgs = new List<ITypeSig> { arg1, arg2, arg3 };
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="args">Generic args</param>
		public GenericInstMethodSig(params ITypeSig[] args) {
			this.callingConvention = CallingConvention.GenericInst;
			this.genericArgs = new List<ITypeSig>(args);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="args">Generic args</param>
		public GenericInstMethodSig(IList<ITypeSig> args) {
			this.callingConvention = CallingConvention.GenericInst;
			this.genericArgs = new List<ITypeSig>(args);
		}
	}
}