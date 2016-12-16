namespace Microsoft.JScript
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.Expando;
    using System.Security;
    using System.Security.Permissions;

    internal sealed class TypeReferences
    {
        private Module _jscriptReferenceModule;
        private static readonly Microsoft.JScript.SimpleHashtable _predefinedTypeTable = new Microsoft.JScript.SimpleHashtable(0x22);
        private System.Type[] _typeTable;
        private const int TypeReferenceArrayLength = 0x53;
        private const int TypeReferenceStartOfSpecialCases = 0x51;

        static TypeReferences()
        {
            _predefinedTypeTable["boolean"] = typeof(bool);
            _predefinedTypeTable["byte"] = typeof(byte);
            _predefinedTypeTable["char"] = typeof(char);
            _predefinedTypeTable["decimal"] = typeof(decimal);
            _predefinedTypeTable["double"] = typeof(double);
            _predefinedTypeTable["float"] = typeof(float);
            _predefinedTypeTable["int"] = typeof(int);
            _predefinedTypeTable["long"] = typeof(long);
            _predefinedTypeTable["sbyte"] = typeof(sbyte);
            _predefinedTypeTable["short"] = typeof(short);
            _predefinedTypeTable["void"] = typeof(void);
            _predefinedTypeTable["uint"] = typeof(uint);
            _predefinedTypeTable["ulong"] = typeof(ulong);
            _predefinedTypeTable["ushort"] = typeof(ushort);
            _predefinedTypeTable["ActiveXObject"] = typeof(object);
            _predefinedTypeTable["Boolean"] = typeof(bool);
            _predefinedTypeTable["Number"] = typeof(double);
            _predefinedTypeTable["Object"] = typeof(object);
            _predefinedTypeTable["String"] = typeof(string);
            _predefinedTypeTable["Type"] = typeof(System.Type);
            _predefinedTypeTable["Array"] = TypeReference.ArrayObject;
            _predefinedTypeTable["Date"] = TypeReference.DateObject;
            _predefinedTypeTable["Enumerator"] = TypeReference.EnumeratorObject;
            _predefinedTypeTable["Error"] = TypeReference.ErrorObject;
            _predefinedTypeTable["EvalError"] = TypeReference.EvalErrorObject;
            _predefinedTypeTable["Function"] = TypeReference.ScriptFunction;
            _predefinedTypeTable["RangeError"] = TypeReference.RangeErrorObject;
            _predefinedTypeTable["ReferenceError"] = TypeReference.ReferenceErrorObject;
            _predefinedTypeTable["RegExp"] = TypeReference.RegExpObject;
            _predefinedTypeTable["SyntaxError"] = TypeReference.SyntaxErrorObject;
            _predefinedTypeTable["TypeError"] = TypeReference.TypeErrorObject;
            _predefinedTypeTable["URIError"] = TypeReference.URIErrorObject;
            _predefinedTypeTable["VBArray"] = TypeReference.VBArrayObject;
        }

        internal TypeReferences(Module jscriptReferenceModule)
        {
            this._jscriptReferenceModule = jscriptReferenceModule;
            this._typeTable = new System.Type[0x53];
        }

        internal static object GetConstantValue(System.Reflection.FieldInfo field)
        {
            if ((field.GetType().Assembly == typeof(TypeReferences).Assembly) || !field.DeclaringType.Assembly.ReflectionOnly)
            {
                return field.GetValue(null);
            }
            System.Type fieldType = field.FieldType;
            object rawConstantValue = field.GetRawConstantValue();
            if (fieldType.IsEnum)
            {
                return MetadataEnumValue.GetEnumValue(fieldType, rawConstantValue);
            }
            return rawConstantValue;
        }

        internal static object GetDefaultParameterValue(ParameterInfo parameter)
        {
            if ((parameter.GetType().Assembly != typeof(TypeReferences).Assembly) && parameter.Member.DeclaringType.Assembly.ReflectionOnly)
            {
                return parameter.RawDefaultValue;
            }
            return parameter.DefaultValue;
        }

        internal System.Type GetPredefinedType(string typeName)
        {
            object obj2 = _predefinedTypeTable[typeName];
            System.Type typeReference = obj2 as System.Type;
            if ((typeReference == null) && (obj2 is TypeReference))
            {
                typeReference = this.GetTypeReference((TypeReference) obj2);
            }
            return typeReference;
        }

        private System.Type GetTypeReference(TypeReference typeRef)
        {
            System.Type type = this._typeTable[(int) typeRef];
            if (type == null)
            {
                string str = "Microsoft.JScript.";
                if (typeRef >= TypeReference.BaseVsaStartup)
                {
                    switch (typeRef)
                    {
                        case TypeReference.BaseVsaStartup:
                            str = "Microsoft.Vsa.";
                            break;

                        case TypeReference.VsaEngine:
                            str = "Microsoft.JScript.Vsa.";
                            break;
                    }
                }
                type = this.JScriptReferenceModule.GetType(str + System.Enum.GetName(typeof(TypeReference), (int) typeRef));
                this._typeTable[(int) typeRef] = type;
            }
            return type;
        }

        internal static bool InExecutionContext(System.Type type)
        {
            if (type != null)
            {
                Assembly assembly = type.Assembly;
                if (assembly.ReflectionOnly)
                {
                    return (assembly.Location != typeof(TypeReferences).Assembly.Location);
                }
            }
            return true;
        }

        internal bool InReferenceContext(IReflect ireflect)
        {
            if ((ireflect != null) && (ireflect is System.Type))
            {
                return this.InReferenceContext((System.Type) ireflect);
            }
            return true;
        }

        internal bool InReferenceContext(MemberInfo member)
        {
            if (member == null)
            {
                return true;
            }
            if (member is JSMethod)
            {
                member = ((JSMethod) member).GetMethodInfo(null);
            }
            else if (member is JSMethodInfo)
            {
                member = ((JSMethodInfo) member).method;
            }
            return this.InReferenceContext(member.DeclaringType);
        }

        internal bool InReferenceContext(System.Type type)
        {
            if (type != null)
            {
                Assembly assembly = type.Assembly;
                if (!assembly.ReflectionOnly && (assembly == typeof(TypeReferences).Assembly))
                {
                    return !this.JScriptReferenceModule.Assembly.ReflectionOnly;
                }
            }
            return true;
        }

        private static MemberInfo MapMemberInfoToExecutionContext(MemberInfo member)
        {
            if (InExecutionContext(member.DeclaringType))
            {
                return member;
            }
            return typeof(TypeReferences).Module.ResolveMember(member.MetadataToken);
        }

        private MemberInfo MapMemberInfoToReferenceContext(MemberInfo member)
        {
            if (this.InReferenceContext(member.DeclaringType))
            {
                return member;
            }
            return this.JScriptReferenceModule.ResolveMember(member.MetadataToken);
        }

        internal static ConstructorInfo ToExecutionContext(ConstructorInfo constructor) => 
            ((ConstructorInfo) MapMemberInfoToExecutionContext(constructor));

        internal static System.Reflection.FieldInfo ToExecutionContext(System.Reflection.FieldInfo field) => 
            ((System.Reflection.FieldInfo) MapMemberInfoToExecutionContext(field));

        internal static IReflect ToExecutionContext(IReflect ireflect)
        {
            if (ireflect is System.Type)
            {
                return ToExecutionContext((System.Type) ireflect);
            }
            return ireflect;
        }

        internal static MethodInfo ToExecutionContext(MethodInfo method)
        {
            if (method is JSMethod)
            {
                method = ((JSMethod) method).GetMethodInfo(null);
            }
            else if (method is JSMethodInfo)
            {
                method = ((JSMethodInfo) method).method;
            }
            return (MethodInfo) MapMemberInfoToExecutionContext(method);
        }

        internal static PropertyInfo ToExecutionContext(PropertyInfo property) => 
            ((PropertyInfo) MapMemberInfoToExecutionContext(property));

        internal static System.Type ToExecutionContext(System.Type type)
        {
            if (InExecutionContext(type))
            {
                return type;
            }
            return typeof(TypeReferences).Module.ResolveType(type.MetadataToken, null, null);
        }

        internal ConstructorInfo ToReferenceContext(ConstructorInfo constructor) => 
            ((ConstructorInfo) this.MapMemberInfoToReferenceContext(constructor));

        internal System.Reflection.FieldInfo ToReferenceContext(System.Reflection.FieldInfo field) => 
            ((System.Reflection.FieldInfo) this.MapMemberInfoToReferenceContext(field));

        internal IReflect ToReferenceContext(IReflect ireflect)
        {
            if (ireflect is System.Type)
            {
                return this.ToReferenceContext((System.Type) ireflect);
            }
            return ireflect;
        }

        internal MethodInfo ToReferenceContext(MethodInfo method)
        {
            if (method is JSMethod)
            {
                method = ((JSMethod) method).GetMethodInfo(null);
            }
            else if (method is JSMethodInfo)
            {
                method = ((JSMethodInfo) method).method;
            }
            return (MethodInfo) this.MapMemberInfoToReferenceContext(method);
        }

        internal PropertyInfo ToReferenceContext(PropertyInfo property) => 
            ((PropertyInfo) this.MapMemberInfoToReferenceContext(property));

        internal System.Type ToReferenceContext(System.Type type)
        {
            if (this.InReferenceContext(type))
            {
                return type;
            }
            if (type.IsArray)
            {
                return Microsoft.JScript.Convert.ToType(Microsoft.JScript.TypedArray.ToRankString(type.GetArrayRank()), this.ToReferenceContext(type.GetElementType()));
            }
            return this.JScriptReferenceModule.ResolveType(type.MetadataToken, null, null);
        }

        internal System.Type AllowPartiallyTrustedCallersAttribute =>
            typeof(System.Security.AllowPartiallyTrustedCallersAttribute);

        internal System.Type ArgumentsObject =>
            this.GetTypeReference(TypeReference.ArgumentsObject);

        internal System.Type Array =>
            typeof(System.Array);

        internal System.Type ArrayConstructor =>
            this.GetTypeReference(TypeReference.ArrayConstructor);

        internal System.Type ArrayObject =>
            this.GetTypeReference(TypeReference.ArrayObject);

        internal System.Type ArrayOfObject =>
            typeof(object[]);

        internal System.Type ArrayOfString =>
            typeof(string[]);

        internal System.Type ArrayWrapper =>
            this.GetTypeReference(TypeReference.ArrayWrapper);

        internal System.Type Attribute =>
            typeof(System.Attribute);

        internal System.Type AttributeUsageAttribute =>
            typeof(System.AttributeUsageAttribute);

        internal System.Type BaseVsaStartup =>
            this.GetTypeReference(TypeReference.BaseVsaStartup);

        internal System.Type Binding =>
            this.GetTypeReference(TypeReference.Binding);

        internal System.Type BitwiseBinary =>
            this.GetTypeReference(TypeReference.BitwiseBinary);

        internal ConstructorInfo bitwiseBinaryConstructor =>
            this.BitwiseBinary.GetConstructor(new System.Type[] { this.Int32 });

        internal System.Type Boolean =>
            typeof(bool);

        internal System.Type BooleanObject =>
            this.GetTypeReference(TypeReference.BooleanObject);

        internal System.Type BreakOutOfFinally =>
            this.GetTypeReference(TypeReference.BreakOutOfFinally);

        internal ConstructorInfo breakOutOfFinallyConstructor =>
            this.BreakOutOfFinally.GetConstructor(new System.Type[] { this.Int32 });

        internal System.Type BuiltinFunction =>
            this.GetTypeReference(TypeReference.BuiltinFunction);

        internal System.Type Byte =>
            typeof(byte);

        internal MethodInfo callMethod =>
            this.LateBinding.GetMethod("Call", new System.Type[] { this.ArrayOfObject, this.Boolean, this.Boolean, this.VsaEngine });

        internal MethodInfo callValue2Method =>
            this.LateBinding.GetMethod("CallValue2", new System.Type[] { this.Object, this.Object, this.ArrayOfObject, this.Boolean, this.Boolean, this.VsaEngine });

        internal MethodInfo callValueMethod =>
            this.LateBinding.GetMethod("CallValue", new System.Type[] { this.Object, this.Object, this.ArrayOfObject, this.Boolean, this.Boolean, this.VsaEngine });

        internal MethodInfo changeTypeMethod =>
            this.SystemConvert.GetMethod("ChangeType", new System.Type[] { this.Object, this.TypeCode });

        internal System.Type Char =>
            typeof(char);

        internal MethodInfo checkIfDoubleIsIntegerMethod =>
            this.Convert.GetMethod("CheckIfDoubleIsInteger");

        internal MethodInfo checkIfSingleIsIntegerMethod =>
            this.Convert.GetMethod("CheckIfSingleIsInteger");

        internal System.Type ClassScope =>
            this.GetTypeReference(TypeReference.ClassScope);

        internal System.Type Closure =>
            this.GetTypeReference(TypeReference.Closure);

        internal ConstructorInfo closureConstructor =>
            this.Closure.GetConstructor(new System.Type[] { this.FunctionObject });

        internal System.Reflection.FieldInfo closureInstanceField =>
            this.StackFrame.GetField("closureInstance");

        internal System.Type CLSCompliantAttribute =>
            typeof(System.CLSCompliantAttribute);

        internal ConstructorInfo clsCompliantAttributeCtor =>
            this.CLSCompliantAttribute.GetConstructor(new System.Type[] { this.Boolean });

        internal System.Type CoClassAttribute =>
            typeof(System.Runtime.InteropServices.CoClassAttribute);

        internal System.Type CodeAccessSecurityAttribute =>
            typeof(System.Security.Permissions.CodeAccessSecurityAttribute);

        internal MethodInfo coerce2Method =>
            this.Convert.GetMethod("Coerce2");

        internal MethodInfo coerceTMethod =>
            this.Convert.GetMethod("CoerceT");

        internal System.Type CompilerGlobalScopeAttribute =>
            typeof(System.Runtime.CompilerServices.CompilerGlobalScopeAttribute);

        internal ConstructorInfo compilerGlobalScopeAttributeCtor =>
            this.CompilerGlobalScopeAttribute.GetConstructor(new System.Type[0]);

        internal MethodInfo constructArrayMethod =>
            this.ArrayConstructor.GetMethod("ConstructArray");

        internal MethodInfo constructObjectMethod =>
            this.ObjectConstructor.GetMethod("ConstructObject");

        internal System.Reflection.FieldInfo contextEngineField =>
            this.Globals.GetField("contextEngine");

        internal System.Type ContextStaticAttribute =>
            typeof(System.ContextStaticAttribute);

        internal ConstructorInfo contextStaticAttributeCtor =>
            this.ContextStaticAttribute.GetConstructor(new System.Type[0]);

        internal System.Type ContinueOutOfFinally =>
            this.GetTypeReference(TypeReference.ContinueOutOfFinally);

        internal ConstructorInfo continueOutOfFinallyConstructor =>
            this.ContinueOutOfFinally.GetConstructor(new System.Type[] { this.Int32 });

        internal System.Type Convert =>
            this.GetTypeReference(TypeReference.Convert);

        internal MethodInfo convertCharToStringMethod =>
            this.SystemConvert.GetMethod("ToString", new System.Type[] { this.Char });

        internal MethodInfo createVsaEngine =>
            this.VsaEngine.GetMethod("CreateEngine", new System.Type[0]);

        internal MethodInfo createVsaEngineWithType =>
            this.VsaEngine.GetMethod("CreateEngineWithType", new System.Type[] { this.RuntimeTypeHandle });

        internal System.Type DateObject =>
            this.GetTypeReference(TypeReference.DateObject);

        internal System.Type DateTime =>
            typeof(System.DateTime);

        internal ConstructorInfo dateTimeConstructor =>
            this.DateTime.GetConstructor(new System.Type[] { this.Int64 });

        internal MethodInfo dateTimeToInt64Method =>
            this.DateTime.GetProperty("Ticks").GetGetMethod();

        internal MethodInfo dateTimeToStringMethod =>
            this.DateTime.GetMethod("ToString", new System.Type[0]);

        internal System.Type DBNull =>
            typeof(System.DBNull);

        internal MethodInfo debugBreak =>
            this.Debugger.GetMethod("Break", new System.Type[0]);

        internal System.Type DebuggableAttribute =>
            typeof(System.Diagnostics.DebuggableAttribute);

        internal System.Type Debugger =>
            typeof(System.Diagnostics.Debugger);

        internal System.Type DebuggerHiddenAttribute =>
            typeof(System.Diagnostics.DebuggerHiddenAttribute);

        internal ConstructorInfo debuggerHiddenAttributeCtor =>
            this.DebuggerHiddenAttribute.GetConstructor(new System.Type[0]);

        internal System.Type DebuggerStepThroughAttribute =>
            typeof(System.Diagnostics.DebuggerStepThroughAttribute);

        internal ConstructorInfo debuggerStepThroughAttributeCtor =>
            this.DebuggerStepThroughAttribute.GetConstructor(new System.Type[0]);

        internal System.Type Decimal =>
            typeof(decimal);

        internal MethodInfo decimalCompare =>
            this.Decimal.GetMethod("Compare", new System.Type[] { this.Decimal, this.Decimal });

        internal ConstructorInfo decimalConstructor =>
            this.Decimal.GetConstructor(new System.Type[] { this.Int32, this.Int32, this.Int32, this.Boolean, this.Byte });

        internal MethodInfo decimalToDoubleMethod =>
            this.Decimal.GetMethod("ToDouble", new System.Type[] { this.Decimal });

        internal MethodInfo decimalToInt32Method =>
            this.Decimal.GetMethod("ToInt32", new System.Type[] { this.Decimal });

        internal MethodInfo decimalToInt64Method =>
            this.Decimal.GetMethod("ToInt64", new System.Type[] { this.Decimal });

        internal MethodInfo decimalToStringMethod =>
            this.Decimal.GetMethod("ToString", new System.Type[0]);

        internal MethodInfo decimalToUInt32Method =>
            this.Decimal.GetMethod("ToUInt32", new System.Type[] { this.Decimal });

        internal MethodInfo decimalToUInt64Method =>
            this.Decimal.GetMethod("ToUInt64", new System.Type[] { this.Decimal });

        internal System.Reflection.FieldInfo decimalZeroField =>
            this.Decimal.GetField("Zero");

        internal System.Type DefaultMemberAttribute =>
            typeof(System.Reflection.DefaultMemberAttribute);

        internal ConstructorInfo defaultMemberAttributeCtor =>
            this.DefaultMemberAttribute.GetConstructor(new System.Type[] { this.String });

        internal System.Type Delegate =>
            typeof(System.Delegate);

        internal MethodInfo deleteMemberMethod =>
            this.LateBinding.GetMethod("DeleteMember");

        internal MethodInfo deleteMethod =>
            this.LateBinding.GetMethod("Delete");

        internal System.Type Double =>
            typeof(double);

        internal MethodInfo doubleToBooleanMethod =>
            this.Convert.GetMethod("ToBoolean", new System.Type[] { this.Double });

        internal MethodInfo doubleToDecimalMethod =>
            this.Decimal.GetMethod("op_Explicit", new System.Type[] { this.Double });

        internal MethodInfo doubleToInt64 =>
            this.Runtime.GetMethod("DoubleToInt64");

        internal MethodInfo doubleToStringMethod =>
            this.Convert.GetMethod("ToString", new System.Type[] { this.Double });

        internal System.Type Empty =>
            this.GetTypeReference(TypeReference.Empty);

        internal System.Reflection.FieldInfo engineField =>
            this.ScriptObject.GetField("engine");

        internal System.Type Enum =>
            typeof(System.Enum);

        internal System.Type EnumeratorObject =>
            this.GetTypeReference(TypeReference.EnumeratorObject);

        internal System.Type Equality =>
            this.GetTypeReference(TypeReference.Equality);

        internal ConstructorInfo equalityConstructor =>
            this.Equality.GetConstructor(new System.Type[] { this.Int32 });

        internal MethodInfo equalsMethod =>
            this.Object.GetMethod("Equals", new System.Type[] { this.Object });

        internal System.Type ErrorObject =>
            this.GetTypeReference(TypeReference.ErrorObject);

        internal System.Type Eval =>
            this.GetTypeReference(TypeReference.Eval);

        internal System.Type EvalErrorObject =>
            this.GetTypeReference(TypeReference.EvalErrorObject);

        internal MethodInfo evaluateBitwiseBinaryMethod =>
            this.BitwiseBinary.GetMethod("EvaluateBitwiseBinary");

        internal MethodInfo evaluateEqualityMethod =>
            this.Equality.GetMethod("EvaluateEquality", new System.Type[] { this.Object, this.Object });

        internal MethodInfo evaluateNumericBinaryMethod =>
            this.NumericBinary.GetMethod("EvaluateNumericBinary");

        internal MethodInfo evaluatePlusMethod =>
            this.Plus.GetMethod("EvaluatePlus");

        internal MethodInfo evaluatePostOrPrefixOperatorMethod =>
            this.PostOrPrefixOperator.GetMethod("EvaluatePostOrPrefix");

        internal MethodInfo evaluateRelationalMethod =>
            this.Relational.GetMethod("EvaluateRelational");

        internal MethodInfo evaluateUnaryMethod =>
            this.NumericUnary.GetMethod("EvaluateUnary");

        internal System.Type EventInfo =>
            typeof(System.Reflection.EventInfo);

        internal System.Type Exception =>
            typeof(System.Exception);

        internal System.Type Expando =>
            this.GetTypeReference(TypeReference.Expando);

        internal MethodInfo fastConstructArrayLiteralMethod =>
            this.Globals.GetMethod("ConstructArrayLiteral");

        internal System.Type FieldAccessor =>
            this.GetTypeReference(TypeReference.FieldAccessor);

        internal System.Type FieldInfo =>
            typeof(System.Reflection.FieldInfo);

        internal System.Type ForIn =>
            this.GetTypeReference(TypeReference.ForIn);

        internal System.Type FunctionDeclaration =>
            this.GetTypeReference(TypeReference.FunctionDeclaration);

        internal System.Type FunctionExpression =>
            this.GetTypeReference(TypeReference.FunctionExpression);

        internal System.Type FunctionObject =>
            this.GetTypeReference(TypeReference.FunctionObject);

        internal System.Type FunctionWrapper =>
            this.GetTypeReference(TypeReference.FunctionWrapper);

        internal MethodInfo getCurrentMethod =>
            this.IEnumerator.GetProperty("Current", System.Type.EmptyTypes).GetGetMethod();

        internal MethodInfo getDefaultThisObjectMethod =>
            this.IActivationObject.GetMethod("GetDefaultThisObject");

        internal MethodInfo getEngineMethod =>
            this.INeedEngine.GetMethod("GetEngine");

        internal MethodInfo getEnumeratorMethod =>
            this.IEnumerable.GetMethod("GetEnumerator", System.Type.EmptyTypes);

        internal MethodInfo getFieldMethod =>
            this.IActivationObject.GetMethod("GetField", new System.Type[] { this.String, this.Int32 });

        internal MethodInfo getFieldValueMethod =>
            this.FieldInfo.GetMethod("GetValue", new System.Type[] { this.Object });

        internal MethodInfo getGlobalScopeMethod =>
            this.IActivationObject.GetMethod("GetGlobalScope");

        internal MethodInfo getLenientGlobalObjectMethod =>
            this.VsaEngine.GetProperty("LenientGlobalObject").GetGetMethod();

        internal MethodInfo getMemberValueMethod =>
            this.IActivationObject.GetMethod("GetMemberValue", new System.Type[] { this.String, this.Int32 });

        internal MethodInfo getMethodMethod =>
            this.Type.GetMethod("GetMethod", new System.Type[] { this.String });

        internal MethodInfo getNamespaceMethod =>
            this.Namespace.GetMethod("GetNamespace");

        internal MethodInfo getNonMissingValueMethod =>
            this.LateBinding.GetMethod("GetNonMissingValue");

        internal MethodInfo getOriginalArrayConstructorMethod =>
            this.VsaEngine.GetMethod("GetOriginalArrayConstructor");

        internal MethodInfo getOriginalObjectConstructorMethod =>
            this.VsaEngine.GetMethod("GetOriginalObjectConstructor");

        internal MethodInfo getOriginalRegExpConstructorMethod =>
            this.VsaEngine.GetMethod("GetOriginalRegExpConstructor");

        internal MethodInfo getParentMethod =>
            this.ScriptObject.GetMethod("GetParent");

        internal MethodInfo getTypeFromHandleMethod =>
            this.Type.GetMethod("GetTypeFromHandle", new System.Type[] { this.RuntimeTypeHandle });

        internal MethodInfo getTypeMethod =>
            this.Type.GetMethod("GetType", new System.Type[] { this.String });

        internal MethodInfo getValue2Method =>
            this.LateBinding.GetMethod("GetValue2");

        internal System.Type GlobalObject =>
            this.GetTypeReference(TypeReference.GlobalObject);

        internal System.Type Globals =>
            this.GetTypeReference(TypeReference.Globals);

        internal System.Type GlobalScope =>
            this.GetTypeReference(TypeReference.GlobalScope);

        internal ConstructorInfo globalScopeConstructor =>
            this.GlobalScope.GetConstructor(new System.Type[] { this.GlobalScope, this.VsaEngine });

        internal ConstructorInfo hashtableCtor =>
            this.SimpleHashtable.GetConstructor(new System.Type[] { this.UInt32 });

        internal MethodInfo hashTableGetEnumerator =>
            this.SimpleHashtable.GetMethod("GetEnumerator", System.Type.EmptyTypes);

        internal MethodInfo hashtableGetItem =>
            this.SimpleHashtable.GetMethod("get_Item", new System.Type[] { this.Object });

        internal MethodInfo hashtableRemove =>
            this.SimpleHashtable.GetMethod("Remove", new System.Type[] { this.Object });

        internal MethodInfo hashtableSetItem =>
            this.SimpleHashtable.GetMethod("set_Item", new System.Type[] { this.Object, this.Object });

        internal System.Type Hide =>
            this.GetTypeReference(TypeReference.Hide);

        internal System.Type IActivationObject =>
            this.GetTypeReference(TypeReference.IActivationObject);

        internal System.Type IConvertible =>
            typeof(System.IConvertible);

        internal System.Type IEnumerable =>
            typeof(System.Collections.IEnumerable);

        internal System.Type IEnumerator =>
            typeof(System.Collections.IEnumerator);

        internal System.Type IExpando =>
            typeof(System.Runtime.InteropServices.Expando.IExpando);

        internal System.Type IList =>
            typeof(System.Collections.IList);

        internal System.Type Import =>
            this.GetTypeReference(TypeReference.Import);

        internal System.Type In =>
            this.GetTypeReference(TypeReference.In);

        internal System.Type INeedEngine =>
            this.GetTypeReference(TypeReference.INeedEngine);

        internal System.Type Instanceof =>
            this.GetTypeReference(TypeReference.Instanceof);

        internal System.Type Int16 =>
            typeof(short);

        internal System.Type Int32 =>
            typeof(int);

        internal MethodInfo int32ToDecimalMethod =>
            this.Decimal.GetMethod("op_Implicit", new System.Type[] { this.Int32 });

        internal MethodInfo int32ToStringMethod =>
            this.Int32.GetMethod("ToString", new System.Type[0]);

        internal System.Type Int64 =>
            typeof(long);

        internal MethodInfo int64ToDecimalMethod =>
            this.Decimal.GetMethod("op_Implicit", new System.Type[] { this.Int64 });

        internal MethodInfo int64ToStringMethod =>
            this.Int64.GetMethod("ToString", new System.Type[0]);

        internal System.Type IntPtr =>
            typeof(System.IntPtr);

        internal MethodInfo isMissingMethod =>
            this.Binding.GetMethod("IsMissing");

        internal MethodInfo jScriptCompareMethod =>
            this.Relational.GetMethod("JScriptCompare");

        internal MethodInfo jScriptEqualsMethod =>
            this.Equality.GetMethod("JScriptEquals");

        internal MethodInfo jScriptEvaluateMethod1 =>
            this.Eval.GetMethod("JScriptEvaluate", new System.Type[] { this.Object, this.VsaEngine });

        internal MethodInfo jScriptEvaluateMethod2 =>
            this.Eval.GetMethod("JScriptEvaluate", new System.Type[] { this.Object, this.Object, this.VsaEngine });

        internal System.Type JScriptException =>
            this.GetTypeReference(TypeReference.JScriptException);

        internal MethodInfo jScriptExceptionValueMethod =>
            this.Try.GetMethod("JScriptExceptionValue");

        internal MethodInfo jScriptFunctionDeclarationMethod =>
            this.FunctionDeclaration.GetMethod("JScriptFunctionDeclaration");

        internal MethodInfo jScriptFunctionExpressionMethod =>
            this.FunctionExpression.GetMethod("JScriptFunctionExpression");

        internal MethodInfo jScriptGetEnumeratorMethod =>
            this.ForIn.GetMethod("JScriptGetEnumerator");

        internal MethodInfo jScriptImportMethod =>
            this.Import.GetMethod("JScriptImport");

        internal MethodInfo jScriptInMethod =>
            this.In.GetMethod("JScriptIn");

        internal MethodInfo jScriptInstanceofMethod =>
            this.Instanceof.GetMethod("JScriptInstanceof");

        internal MethodInfo jScriptPackageMethod =>
            this.Package.GetMethod("JScriptPackage");

        private Module JScriptReferenceModule =>
            this._jscriptReferenceModule;

        internal MethodInfo jScriptStrictEqualsMethod =>
            this.StrictEquality.GetMethod("JScriptStrictEquals", new System.Type[] { this.Object, this.Object });

        internal MethodInfo jScriptThrowMethod =>
            this.Throw.GetMethod("JScriptThrow");

        internal MethodInfo jScriptTypeofMethod =>
            this.Typeof.GetMethod("JScriptTypeof");

        internal MethodInfo jScriptWithMethod =>
            this.With.GetMethod("JScriptWith");

        internal System.Type JSError =>
            this.GetTypeReference(TypeReference.JSError);

        internal System.Type JSFunctionAttribute =>
            this.GetTypeReference(TypeReference.JSFunctionAttribute);

        internal ConstructorInfo jsFunctionAttributeConstructor =>
            this.JSFunctionAttribute.GetConstructor(new System.Type[] { this.JSFunctionAttributeEnum });

        internal System.Type JSFunctionAttributeEnum =>
            this.GetTypeReference(TypeReference.JSFunctionAttributeEnum);

        internal System.Type JSLocalField =>
            this.GetTypeReference(TypeReference.JSLocalField);

        internal ConstructorInfo jsLocalFieldConstructor =>
            this.JSLocalField.GetConstructor(new System.Type[] { this.String, this.RuntimeTypeHandle, this.Int32 });

        internal System.Type JSObject =>
            this.GetTypeReference(TypeReference.JSObject);

        internal System.Type LateBinding =>
            this.GetTypeReference(TypeReference.LateBinding);

        internal ConstructorInfo lateBindingConstructor =>
            this.LateBinding.GetConstructor(new System.Type[] { this.String });

        internal ConstructorInfo lateBindingConstructor2 =>
            this.LateBinding.GetConstructor(new System.Type[] { this.String, this.Object });

        internal System.Type LenientGlobalObject =>
            this.GetTypeReference(TypeReference.LenientGlobalObject);

        internal System.Reflection.FieldInfo localVarsField =>
            this.StackFrame.GetField("localVars");

        internal System.Type MathObject =>
            this.GetTypeReference(TypeReference.MathObject);

        internal System.Type MethodInvoker =>
            this.GetTypeReference(TypeReference.MethodInvoker);

        internal System.Type Missing =>
            this.GetTypeReference(TypeReference.Missing);

        internal System.Reflection.FieldInfo missingField =>
            this.Missing.GetField("Value");

        internal MethodInfo moveNextMethod =>
            this.IEnumerator.GetMethod("MoveNext", System.Type.EmptyTypes);

        internal System.Type Namespace =>
            this.GetTypeReference(TypeReference.Namespace);

        internal System.Type NotRecommended =>
            this.GetTypeReference(TypeReference.NotRecommended);

        internal System.Type NumberObject =>
            this.GetTypeReference(TypeReference.NumberObject);

        internal System.Type NumericBinary =>
            this.GetTypeReference(TypeReference.NumericBinary);

        internal ConstructorInfo numericBinaryConstructor =>
            this.NumericBinary.GetConstructor(new System.Type[] { this.Int32 });

        internal MethodInfo numericbinaryDoOpMethod =>
            this.NumericBinary.GetMethod("DoOp");

        internal System.Type NumericUnary =>
            this.GetTypeReference(TypeReference.NumericUnary);

        internal ConstructorInfo numericUnaryConstructor =>
            this.NumericUnary.GetConstructor(new System.Type[] { this.Int32 });

        internal System.Type Object =>
            typeof(object);

        internal System.Type ObjectConstructor =>
            this.GetTypeReference(TypeReference.ObjectConstructor);

        internal System.Reflection.FieldInfo objectField =>
            this.LateBinding.GetField("obj");

        internal System.Type ObsoleteAttribute =>
            typeof(System.ObsoleteAttribute);

        internal System.Type Override =>
            this.GetTypeReference(TypeReference.Override);

        internal System.Type Package =>
            this.GetTypeReference(TypeReference.Package);

        internal System.Type ParamArrayAttribute =>
            typeof(System.ParamArrayAttribute);

        internal System.Type Plus =>
            this.GetTypeReference(TypeReference.Plus);

        internal ConstructorInfo plusConstructor =>
            this.Plus.GetConstructor(new System.Type[0]);

        internal MethodInfo plusDoOpMethod =>
            this.Plus.GetMethod("DoOp");

        internal MethodInfo popScriptObjectMethod =>
            this.VsaEngine.GetMethod("PopScriptObject");

        internal ConstructorInfo postOrPrefixConstructor =>
            this.PostOrPrefixOperator.GetConstructor(new System.Type[] { this.Int32 });

        internal System.Type PostOrPrefixOperator =>
            this.GetTypeReference(TypeReference.PostOrPrefixOperator);

        internal MethodInfo pushScriptObjectMethod =>
            this.VsaEngine.GetMethod("PushScriptObject");

        internal MethodInfo pushStackFrameForMethod =>
            this.StackFrame.GetMethod("PushStackFrameForMethod");

        internal MethodInfo pushStackFrameForStaticMethod =>
            this.StackFrame.GetMethod("PushStackFrameForStaticMethod");

        internal System.Type RangeErrorObject =>
            this.GetTypeReference(TypeReference.RangeErrorObject);

        internal System.Type ReferenceAttribute =>
            this.GetTypeReference(TypeReference.ReferenceAttribute);

        internal ConstructorInfo referenceAttributeConstructor =>
            this.ReferenceAttribute.GetConstructor(new System.Type[] { this.String });

        internal System.Type ReferenceErrorObject =>
            this.GetTypeReference(TypeReference.ReferenceErrorObject);

        internal System.Type ReflectionMissing =>
            typeof(System.Reflection.Missing);

        internal MethodInfo regExpConstructMethod =>
            this.RegExpConstructor.GetMethod("Construct", new System.Type[] { this.String, this.Boolean, this.Boolean, this.Boolean });

        internal System.Type RegExpConstructor =>
            this.GetTypeReference(TypeReference.RegExpConstructor);

        internal System.Type RegExpObject =>
            this.GetTypeReference(TypeReference.RegExpObject);

        internal System.Type Relational =>
            this.GetTypeReference(TypeReference.Relational);

        internal ConstructorInfo relationalConstructor =>
            this.Relational.GetConstructor(new System.Type[] { this.Int32 });

        internal System.Type RequiredAttributeAttribute =>
            typeof(System.Runtime.CompilerServices.RequiredAttributeAttribute);

        internal System.Type ReturnOutOfFinally =>
            this.GetTypeReference(TypeReference.ReturnOutOfFinally);

        internal ConstructorInfo returnOutOfFinallyConstructor =>
            this.ReturnOutOfFinally.GetConstructor(new System.Type[0]);

        internal System.Type Runtime =>
            this.GetTypeReference(TypeReference.Runtime);

        internal System.Type RuntimeTypeHandle =>
            typeof(System.RuntimeTypeHandle);

        internal System.Type SByte =>
            typeof(sbyte);

        internal ConstructorInfo scriptExceptionConstructor =>
            this.JScriptException.GetConstructor(new System.Type[] { this.JSError });

        internal System.Type ScriptFunction =>
            this.GetTypeReference(TypeReference.ScriptFunction);

        internal System.Type ScriptObject =>
            this.GetTypeReference(TypeReference.ScriptObject);

        internal MethodInfo scriptObjectStackTopMethod =>
            this.VsaEngine.GetMethod("ScriptObjectStackTop");

        internal System.Type ScriptStream =>
            this.GetTypeReference(TypeReference.ScriptStream);

        internal MethodInfo setEngineMethod =>
            this.INeedEngine.GetMethod("SetEngine");

        internal MethodInfo setFieldValueMethod =>
            this.FieldInfo.GetMethod("SetValue", new System.Type[] { this.Object, this.Object });

        internal MethodInfo setIndexedPropertyValueStaticMethod =>
            this.LateBinding.GetMethod("SetIndexedPropertyValueStatic");

        internal MethodInfo setMemberValue2Method =>
            this.JSObject.GetMethod("SetMemberValue2", new System.Type[] { this.String, this.Object });

        internal MethodInfo setValueMethod =>
            this.LateBinding.GetMethod("SetValue");

        internal System.Type SimpleHashtable =>
            this.GetTypeReference(TypeReference.SimpleHashtable);

        internal System.Type Single =>
            typeof(float);

        internal System.Type StackFrame =>
            this.GetTypeReference(TypeReference.StackFrame);

        internal System.Type STAThreadAttribute =>
            typeof(System.STAThreadAttribute);

        internal System.Type StrictEquality =>
            this.GetTypeReference(TypeReference.StrictEquality);

        internal System.Type String =>
            typeof(string);

        internal MethodInfo stringConcat2Method =>
            this.String.GetMethod("Concat", new System.Type[] { this.String, this.String });

        internal MethodInfo stringConcat3Method =>
            this.String.GetMethod("Concat", new System.Type[] { this.String, this.String, this.String });

        internal MethodInfo stringConcat4Method =>
            this.String.GetMethod("Concat", new System.Type[] { this.String, this.String, this.String, this.String });

        internal MethodInfo stringConcatArrMethod =>
            this.String.GetMethod("Concat", new System.Type[] { this.ArrayOfString });

        internal MethodInfo stringEqualsMethod =>
            this.String.GetMethod("Equals", new System.Type[] { this.String, this.String });

        internal MethodInfo stringLengthMethod =>
            this.String.GetProperty("Length").GetGetMethod();

        internal System.Type StringObject =>
            this.GetTypeReference(TypeReference.StringObject);

        internal System.Type SyntaxErrorObject =>
            this.GetTypeReference(TypeReference.SyntaxErrorObject);

        internal System.Type SystemConvert =>
            typeof(System.Convert);

        internal System.Reflection.FieldInfo systemReflectionMissingField =>
            this.ReflectionMissing.GetField("Value");

        internal System.Type Throw =>
            this.GetTypeReference(TypeReference.Throw);

        internal MethodInfo throwTypeMismatch =>
            this.Convert.GetMethod("ThrowTypeMismatch");

        internal MethodInfo toBooleanMethod =>
            this.Convert.GetMethod("ToBoolean", new System.Type[] { this.Object, this.Boolean });

        internal MethodInfo toForInObjectMethod =>
            this.Convert.GetMethod("ToForInObject", new System.Type[] { this.Object, this.VsaEngine });

        internal MethodInfo toInt32Method =>
            this.Convert.GetMethod("ToInt32", new System.Type[] { this.Object });

        internal MethodInfo toNativeArrayMethod =>
            this.Convert.GetMethod("ToNativeArray");

        internal MethodInfo toNumberMethod =>
            this.Convert.GetMethod("ToNumber", new System.Type[] { this.Object });

        internal MethodInfo toObject2Method =>
            this.Convert.GetMethod("ToObject2", new System.Type[] { this.Object, this.VsaEngine });

        internal MethodInfo toObjectMethod =>
            this.Convert.GetMethod("ToObject", new System.Type[] { this.Object, this.VsaEngine });

        internal MethodInfo toStringMethod =>
            this.Convert.GetMethod("ToString", new System.Type[] { this.Object, this.Boolean });

        internal System.Type Try =>
            this.GetTypeReference(TypeReference.Try);

        internal System.Type Type =>
            typeof(System.Type);

        internal System.Type TypeCode =>
            typeof(System.TypeCode);

        internal System.Type TypedArray =>
            this.GetTypeReference(TypeReference.TypedArray);

        internal System.Type TypeErrorObject =>
            this.GetTypeReference(TypeReference.TypeErrorObject);

        internal System.Type Typeof =>
            this.GetTypeReference(TypeReference.Typeof);

        internal System.Type UInt16 =>
            typeof(ushort);

        internal System.Type UInt32 =>
            typeof(uint);

        internal MethodInfo uint32ToDecimalMethod =>
            this.Decimal.GetMethod("op_Implicit", new System.Type[] { this.UInt32 });

        internal MethodInfo uint32ToStringMethod =>
            this.UInt32.GetMethod("ToString", new System.Type[0]);

        internal System.Type UInt64 =>
            typeof(ulong);

        internal MethodInfo uint64ToDecimalMethod =>
            this.Decimal.GetMethod("op_Implicit", new System.Type[] { this.UInt64 });

        internal MethodInfo uint64ToStringMethod =>
            this.UInt64.GetMethod("ToString", new System.Type[0]);

        internal System.Type UIntPtr =>
            typeof(System.UIntPtr);

        internal MethodInfo uncheckedDecimalToInt64Method =>
            this.Runtime.GetMethod("UncheckedDecimalToInt64");

        internal System.Reflection.FieldInfo undefinedField =>
            this.Empty.GetField("Value");

        internal System.Type URIErrorObject =>
            this.GetTypeReference(TypeReference.URIErrorObject);

        internal System.Type ValueType =>
            typeof(System.ValueType);

        internal System.Type VBArrayObject =>
            this.GetTypeReference(TypeReference.VBArrayObject);

        internal System.Type Void =>
            typeof(void);

        internal System.Type VsaEngine =>
            this.GetTypeReference(TypeReference.VsaEngine);

        internal ConstructorInfo vsaEngineConstructor =>
            this.VsaEngine.GetConstructor(new System.Type[0]);

        internal System.Type With =>
            this.GetTypeReference(TypeReference.With);

        internal MethodInfo writeLineMethod =>
            this.ScriptStream.GetMethod("WriteLine");

        internal MethodInfo writeMethod =>
            this.ScriptStream.GetMethod("Write");

        private enum TypeReference
        {
            ArgumentsObject,
            ArrayConstructor,
            ArrayObject,
            ArrayWrapper,
            Binding,
            BitwiseBinary,
            BooleanObject,
            BreakOutOfFinally,
            BuiltinFunction,
            ClassScope,
            Closure,
            ContinueOutOfFinally,
            Convert,
            DateObject,
            Empty,
            EnumeratorObject,
            Equality,
            ErrorObject,
            Eval,
            EvalErrorObject,
            Expando,
            FieldAccessor,
            ForIn,
            FunctionDeclaration,
            FunctionExpression,
            FunctionObject,
            FunctionWrapper,
            GlobalObject,
            GlobalScope,
            Globals,
            Hide,
            IActivationObject,
            INeedEngine,
            Import,
            In,
            Instanceof,
            JSError,
            JSFunctionAttribute,
            JSFunctionAttributeEnum,
            JSLocalField,
            JSObject,
            JScriptException,
            LateBinding,
            LenientGlobalObject,
            MathObject,
            MethodInvoker,
            Missing,
            Namespace,
            NotRecommended,
            NumberObject,
            NumericBinary,
            NumericUnary,
            ObjectConstructor,
            Override,
            Package,
            Plus,
            PostOrPrefixOperator,
            RangeErrorObject,
            ReferenceAttribute,
            ReferenceErrorObject,
            RegExpConstructor,
            RegExpObject,
            Relational,
            ReturnOutOfFinally,
            Runtime,
            ScriptFunction,
            ScriptObject,
            ScriptStream,
            SimpleHashtable,
            StackFrame,
            StrictEquality,
            StringObject,
            SyntaxErrorObject,
            Throw,
            Try,
            TypedArray,
            TypeErrorObject,
            Typeof,
            URIErrorObject,
            VBArrayObject,
            With,
            BaseVsaStartup,
            VsaEngine
        }
    }
}

