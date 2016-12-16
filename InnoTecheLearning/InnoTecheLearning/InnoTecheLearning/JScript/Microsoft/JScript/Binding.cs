using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
    public abstract class Binding : AST
    {
        private IReflect[] argIRs;

        protected MemberInfo defaultMember;

        private IReflect defaultMemberReturnIR;

        private bool isArrayElementAccess;

        private bool isArrayConstructor;

        protected bool isAssignmentToDefaultIndexedProperty;

        protected bool isFullyResolved;

        protected bool isNonVirtual;

        internal MemberInfo[] members;

        internal MemberInfo member;

        protected string name;

        private bool giveErrors;

        internal static ConstantWrapper ReflectionMissingCW = new ConstantWrapper(System.Reflection.Missing.Value, null);

        private static ConstantWrapper JScriptMissingCW = new ConstantWrapper(Missing.Value, null);

        internal Binding(Context context, string name) : base(context)
        {
            this.argIRs = null;
            this.defaultMember = null;
            this.defaultMemberReturnIR = null;
            this.isArrayElementAccess = false;
            this.isArrayConstructor = false;
            this.isAssignmentToDefaultIndexedProperty = false;
            this.isFullyResolved = true;
            this.isNonVirtual = false;
            this.members = null;
            this.member = null;
            this.name = name;
            this.giveErrors = true;
        }

        private bool Accessible(bool checkSetter)
        {
            if (this.member == null)
            {
                return false;
            }
            MemberTypes memberType = this.member.MemberType;
            if (memberType <= MemberTypes.Method)
            {
                switch (memberType)
                {
                    case MemberTypes.Constructor:
                        return this.AccessibleConstructor();
                    case MemberTypes.Event:
                        return false;
                    case MemberTypes.Constructor | MemberTypes.Event:
                        break;
                    case MemberTypes.Field:
                        return this.AccessibleField(checkSetter);
                    default:
                        if (memberType == MemberTypes.Method)
                        {
                            return this.AccessibleMethod();
                        }
                        break;
                }
            }
            else
            {
                if (memberType == MemberTypes.Property)
                {
                    return this.AccessibleProperty(checkSetter);
                }
                if (memberType != MemberTypes.TypeInfo)
                {
                    if (memberType == MemberTypes.NestedType)
                    {
                        if (!((Type)this.member).IsNestedPublic)
                        {
                            if (this.giveErrors)
                            {
                                this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
                            }
                            return false;
                        }
                        return !checkSetter;
                    }
                }
                else
                {
                    if (!((Type)this.member).IsPublic)
                    {
                        if (this.giveErrors)
                        {
                            this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
                        }
                        return false;
                    }
                    return !checkSetter;
                }
            }
            return false;
        }

        private bool AccessibleConstructor()
        {
            ConstructorInfo constructorInfo = (ConstructorInfo)this.member;
            if ((constructorInfo is JSConstructor && ((JSConstructor)this.member).GetClassScope().owner.isAbstract) || (!(constructorInfo is JSConstructor) && constructorInfo.DeclaringType.IsAbstract))
            {
                this.context.HandleError(JSError.CannotInstantiateAbstractClass);
                return false;
            }
            if (constructorInfo.IsPublic)
            {
                return true;
            }
            if (constructorInfo is JSConstructor && ((JSConstructor)constructorInfo).IsAccessibleFrom(base.Globals.ScopeStack.Peek()))
            {
                return true;
            }
            if (this.giveErrors)
            {
                this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
            }
            return false;
        }

        private bool AccessibleField(bool checkWritable)
        {
            FieldInfo fieldInfo = (FieldInfo)this.member;
            if (checkWritable && (fieldInfo.IsInitOnly || fieldInfo.IsLiteral))
            {
                return false;
            }
            if (!fieldInfo.IsPublic)
            {
                JSWrappedField jSWrappedField = fieldInfo as JSWrappedField;
                if (jSWrappedField != null)
                {
                    fieldInfo = (this.member = jSWrappedField.wrappedField);
                }
                JSClosureField jSClosureField = fieldInfo as JSClosureField;
                JSMemberField jSMemberField = ((jSClosureField != null) ? jSClosureField.field : fieldInfo) as JSMemberField;
                if (jSMemberField == null)
                {
                    if ((!fieldInfo.IsFamily && !fieldInfo.IsFamilyOrAssembly) || !Binding.InsideClassThatExtends(base.Globals.ScopeStack.Peek(), fieldInfo.ReflectedType))
                    {
                        if (this.giveErrors)
                        {
                            this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
                        }
                        return false;
                    }
                }
                else if (!jSMemberField.IsAccessibleFrom(base.Globals.ScopeStack.Peek()))
                {
                    if (this.giveErrors)
                    {
                        this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
                    }
                    return false;
                }
            }
            if (fieldInfo.IsLiteral && fieldInfo is JSVariableField)
            {
                ClassScope classScope = ((JSVariableField)fieldInfo).value as ClassScope;
                if (classScope != null && !classScope.owner.IsStatic)
                {
                    Lookup lookup = this as Lookup;
                    if (lookup == null || !lookup.InStaticCode() || lookup.InFunctionNestedInsideInstanceMethod())
                    {
                        return true;
                    }
                    if (this.giveErrors)
                    {
                        this.context.HandleError(JSError.InstanceNotAccessibleFromStatic, this.isFullyResolved);
                    }
                    return true;
                }
            }
            if (fieldInfo.IsStatic || fieldInfo.IsLiteral || this.defaultMember != null || !(this is Lookup) || !((Lookup)this).InStaticCode())
            {
                return true;
            }
            if (fieldInfo is JSWrappedField && fieldInfo.DeclaringType == Typeob.LenientGlobalObject)
            {
                return true;
            }
            if (this.giveErrors)
            {
                if (!fieldInfo.IsStatic && this is Lookup && ((Lookup)this).InStaticCode())
                {
                    this.context.HandleError(JSError.InstanceNotAccessibleFromStatic, this.isFullyResolved);
                }
                else
                {
                    this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
                }
            }
            return false;
        }

        private bool AccessibleMethod()
        {
            MethodInfo meth = (MethodInfo)this.member;
            return this.AccessibleMethod(meth);
        }

        private bool AccessibleMethod(MethodInfo meth)
        {
            if (meth == null)
            {
                return false;
            }
            if (this.isNonVirtual && meth.IsAbstract)
            {
                this.context.HandleError(JSError.InvalidCall);
                return false;
            }
            if (!meth.IsPublic)
            {
                JSWrappedMethod jSWrappedMethod = meth as JSWrappedMethod;
                if (jSWrappedMethod != null)
                {
                    meth = jSWrappedMethod.method;
                }
                JSClosureMethod jSClosureMethod = meth as JSClosureMethod;
                JSFieldMethod jSFieldMethod = ((jSClosureMethod != null) ? jSClosureMethod.method : meth) as JSFieldMethod;
                if (jSFieldMethod == null)
                {
                    if ((meth.IsFamily || meth.IsFamilyOrAssembly) && Binding.InsideClassThatExtends(base.Globals.ScopeStack.Peek(), meth.ReflectedType))
                    {
                        return true;
                    }
                    if (this.giveErrors)
                    {
                        this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
                    }
                    return false;
                }
                else if (!jSFieldMethod.IsAccessibleFrom(base.Globals.ScopeStack.Peek()))
                {
                    if (this.giveErrors)
                    {
                        this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
                    }
                    return false;
                }
            }
            if (meth.IsStatic || this.defaultMember != null || !(this is Lookup) || !((Lookup)this).InStaticCode())
            {
                return true;
            }
            if (meth is JSWrappedMethod && ((Lookup)this).CanPlaceAppropriateObjectOnStack(((JSWrappedMethod)meth).obj))
            {
                return true;
            }
            if (this.giveErrors)
            {
                if (!meth.IsStatic && this is Lookup && ((Lookup)this).InStaticCode())
                {
                    this.context.HandleError(JSError.InstanceNotAccessibleFromStatic, this.isFullyResolved);
                }
                else
                {
                    this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
                }
            }
            return false;
        }

        private bool AccessibleProperty(bool checkSetter)
        {
            PropertyInfo prop = (PropertyInfo)this.member;
            if (this.AccessibleMethod(checkSetter ? JSProperty.GetSetMethod(prop, true) : JSProperty.GetGetMethod(prop, true)))
            {
                return true;
            }
            if (this.giveErrors && !checkSetter)
            {
                this.context.HandleError(JSError.WriteOnlyProperty);
            }
            return false;
        }

        internal static bool AssignmentCompatible(IReflect lhir, AST rhexpr, IReflect rhir, bool reportError)
        {
            if (rhexpr is ConstantWrapper)
            {
                object obj = rhexpr.Evaluate();
                if (obj is ClassScope)
                {
                    if ((Type)lhir == Typeob.Type || (Type)lhir == Typeob.Object || (Type)lhir == Typeob.String)
                    {
                        return true;
                    }
                    if (reportError)
                    {
                        rhexpr.context.HandleError(JSError.TypeMismatch);
                    }
                    return false;
                }
                else
                {
                    ClassScope classScope = lhir as ClassScope;
                    if (classScope != null)
                    {
                        EnumDeclaration enumDeclaration = classScope.owner as EnumDeclaration;
                        if (enumDeclaration != null)
                        {
                            ConstantWrapper constantWrapper = rhexpr as ConstantWrapper;
                            if (constantWrapper != null && constantWrapper.value is string)
                            {
                                FieldInfo field = classScope.GetField((string)constantWrapper.value, BindingFlags.Static | BindingFlags.Public);
                                if (field == null)
                                {
                                    return false;
                                }
                                enumDeclaration.PartiallyEvaluate();
                                constantWrapper.value = new DeclaredEnumValue(((JSMemberField)field).value, field.Name, classScope);
                            }
                            if ((Type)rhir == Typeob.String)
                            {
                                return true;
                            }
                            lhir = enumDeclaration.baseType.ToType();
                        }
                    }
                    else if (lhir is Type)
                    {
                        Type type = lhir as Type;
                        if (type.IsEnum)
                        {
                            ConstantWrapper constantWrapper2 = rhexpr as ConstantWrapper;
                            if (constantWrapper2 != null && constantWrapper2.value is string)
                            {
                                FieldInfo field2 = type.GetField((string)constantWrapper2.value, BindingFlags.Static | BindingFlags.Public);
                                if (field2 == null)
                                {
                                    return false;
                                }
                                constantWrapper2.value = MetadataEnumValue.GetEnumValue(field2.FieldType, field2.GetRawConstantValue());
                            }
                            if ((Type)rhir == Typeob.String)
                            {
                                return true;
                            }
                            lhir = Enum.GetUnderlyingType(type);
                        }
                    }
                    if (lhir is Type)
                    {
                        try
                        {
                            Convert.CoerceT(obj, (Type)lhir);
                            bool result = true;
                            return result;
                        }
                        catch
                        {
                            if ((Type)lhir == Typeob.Single && obj is double)
                            {
                                if (((ConstantWrapper)rhexpr).isNumericLiteral)
                                {
                                    bool result = true;
                                    return result;
                                }
                                double num = (double)obj;
                                float num2 = (float)num;
                                if (num.ToString(CultureInfo.InvariantCulture).Equals(num2.ToString(CultureInfo.InvariantCulture)))
                                {
                                    ((ConstantWrapper)rhexpr).value = num2;
                                    bool result = true;
                                    return result;
                                }
                            }
                            if ((Type)lhir == Typeob.Decimal)
                            {
                                ConstantWrapper constantWrapper3 = rhexpr as ConstantWrapper;
                                if (constantWrapper3 != null && constantWrapper3.isNumericLiteral)
                                {
                                    try
                                    {
                                        Convert.CoerceT(constantWrapper3.context.GetCode(), Typeob.Decimal);
                                        bool result = true;
                                        return result;
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            if (reportError)
                            {
                                rhexpr.context.HandleError(JSError.TypeMismatch);
                            }
                        }
                        return false;
                    }
                }
            }
            else if (rhexpr is ArrayLiteral)
            {
                return ((ArrayLiteral)rhexpr).AssignmentCompatible(lhir, reportError);
            }
            if ((Type)rhir == Typeob.Object)
            {
                return true;
            }
            if ((Type)rhir == Typeob.Double && Convert.IsPrimitiveNumericType(lhir))
            {
                return true;
            }
            if (lhir is Type && Typeob.Delegate.IsAssignableFrom((Type)lhir) && (Type)rhir == Typeob.ScriptFunction && rhexpr is Binding && ((Binding)rhexpr).IsCompatibleWithDelegate((Type)lhir))
            {
                return true;
            }
            if (Convert.IsPromotableTo(rhir, lhir))
            {
                return true;
            }
            if (Convert.IsJScriptArray(rhir) && Binding.ArrayAssignmentCompatible(rhexpr, lhir))
            {
                return true;
            }
            if ((Type)lhir == Typeob.String)
            {
                return true;
            }
            if ((Type)rhir == Typeob.String && ((Type)lhir == Typeob.Boolean || Convert.IsPrimitiveNumericType(lhir)))
            {
                if (reportError)
                {
                    rhexpr.context.HandleError(JSError.PossibleBadConversionFromString);
                }
                return true;
            }
            if (((Type)lhir == Typeob.Char && (Type)rhir == Typeob.String) || Convert.IsPromotableTo(lhir, rhir) || (Convert.IsPrimitiveNumericType(lhir) && Convert.IsPrimitiveNumericType(rhir)))
            {
                if (reportError)
                {
                    rhexpr.context.HandleError(JSError.PossibleBadConversion);
                }
                return true;
            }
            if (reportError)
            {
                rhexpr.context.HandleError(JSError.TypeMismatch);
            }
            return false;
        }

        private static bool ArrayAssignmentCompatible(AST ast, IReflect lhir)
        {
            if (!Convert.IsArray(lhir))
            {
                return false;
            }
            if ((Type)lhir == Typeob.Array)
            {
                ast.context.HandleError(JSError.ArrayMayBeCopied);
                return true;
            }
            if (Convert.GetArrayRank(lhir) == 1)
            {
                ast.context.HandleError(JSError.ArrayMayBeCopied);
                return true;
            }
            return false;
        }

        internal void CheckIfDeletable()
        {
            if (this.member != null || this.defaultMember != null)
            {
                this.context.HandleError(JSError.NotDeletable);
            }
            this.member = null;
            this.defaultMember = null;
        }

        internal void CheckIfUseless()
        {
            if (this.members == null || this.members.Length == 0)
            {
                return;
            }
            this.context.HandleError(JSError.UselessExpression);
        }

        internal static bool CheckParameters(ParameterInfo[] pars, IReflect[] argIRs, ASTList argAST, Context ctx)
        {
            return Binding.CheckParameters(pars, argIRs, argAST, ctx, 0, false, true);
        }

        internal static bool CheckParameters(ParameterInfo[] pars, IReflect[] argIRs, ASTList argAST, Context ctx, int offset, bool defaultIsUndefined, bool reportError)
        {
            int num = argIRs.Length;
            int num2 = pars.Length;
            bool flag = false;
            if (num > num2 - offset)
            {
                num = num2 - offset;
                flag = true;
            }
            int i = 0;
            while (i < num)
            {
                IReflect reflect = (pars[i + offset] is ParameterDeclaration) ? ((ParameterDeclaration)pars[i + offset]).ParameterIReflect : pars[i + offset].ParameterType;
                IReflect rhir = argIRs[i];
                if (i == num - 1 && ((reflect is Type && Typeob.Array.IsAssignableFrom((Type)reflect)) || reflect is TypedArray) && CustomAttribute.IsDefined(pars[i + offset], typeof(ParamArrayAttribute), false))
                {
                    int num3 = argIRs.Length;
                    if (i == num3 - 1 && Binding.AssignmentCompatible(reflect, argAST[i], argIRs[i], false))
                    {
                        return true;
                    }
                    IReflect lhir = (reflect is Type) ? ((Type)reflect).GetElementType() : ((TypedArray)reflect).elementType;
                    for (int j = i; j < num3; j++)
                    {
                        if (!Binding.AssignmentCompatible(lhir, argAST[j], argIRs[j], reportError))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    if (!Binding.AssignmentCompatible(reflect, argAST[i], rhir, reportError))
                    {
                        return false;
                    }
                    i++;
                }
            }
            if (flag && reportError)
            {
                ctx.HandleError(JSError.TooManyParameters);
            }
            if (offset == 0 && num < num2 && !defaultIsUndefined)
            {
                for (int k = num; k < num2; k++)
                {
                    if (TypeReferences.GetDefaultParameterValue(pars[k]) == System.Convert.DBNull)
                    {
                        ParameterDeclaration parameterDeclaration = pars[k] as ParameterDeclaration;
                        if (parameterDeclaration != null)
                        {
                            parameterDeclaration.PartiallyEvaluate();
                        }
                        if (k < num2 - 1 || !CustomAttribute.IsDefined(pars[k], typeof(ParamArrayAttribute), false))
                        {
                            if (reportError)
                            {
                                ctx.HandleError(JSError.TooFewParameters);
                            }
                            IReflect reflect2 = (pars[k + offset] is ParameterDeclaration) ? ((ParameterDeclaration)pars[k + offset]).ParameterIReflect : pars[k + offset].ParameterType;
                            Type type = reflect2 as Type;
                            if (type != null && type.IsValueType && !type.IsPrimitive && !type.IsEnum)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        internal override bool Delete()
        {
            return this.EvaluateAsLateBinding().Delete();
        }

        internal override object Evaluate()
        {
            object @object = this.GetObject();
            MemberInfo memberInfo = this.member;
            if (memberInfo != null)
            {
                MemberTypes memberType = memberInfo.MemberType;
                switch (memberType)
                {
                    case MemberTypes.Event:
                        return null;
                    case MemberTypes.Constructor | MemberTypes.Event:
                        break;
                    case MemberTypes.Field:
                        return ((FieldInfo)memberInfo).GetValue(@object);
                    default:
                        if (memberType == MemberTypes.Property)
                        {
                            MemberInfo[] array = new MemberInfo[]
                            {
                            JSProperty.GetGetMethod((PropertyInfo)memberInfo, false)
                            };
                            return LateBinding.CallOneOfTheMembers(array, new object[0], false, @object, null, null, null, base.Engine);
                        }
                        if (memberType == MemberTypes.NestedType)
                        {
                            return memberInfo;
                        }
                        break;
                }
            }
            if (this.members != null && this.members.Length > 0)
            {
                if (this.members.Length == 1 && this.members[0].MemberType == MemberTypes.Method)
                {
                    MethodInfo methodInfo = (MethodInfo)this.members[0];
                    Type type = (methodInfo is JSMethod) ? null : methodInfo.DeclaringType;
                    if (type == Typeob.GlobalObject || (type != null && type != Typeob.StringObject && type != Typeob.NumberObject && type != Typeob.BooleanObject && type.IsSubclassOf(Typeob.JSObject)))
                    {
                        return Globals.BuiltinFunctionFor(@object, TypeReferences.ToExecutionContext(methodInfo));
                    }
                }
                return new FunctionWrapper(this.name, @object, this.members);
            }
            return this.EvaluateAsLateBinding().GetValue();
        }

        private MemberInfoList GetAllKnownInstanceBindingsForThisName()
        {
            IReflect[] allEligibleClasses = this.GetAllEligibleClasses();
            MemberInfoList memberInfoList = new MemberInfoList();
            IReflect[] array = allEligibleClasses;
            for (int i = 0; i < array.Length; i++)
            {
                IReflect reflect = array[i];
                if (reflect is ClassScope)
                {
                    if (((ClassScope)reflect).ParentIsInSamePackage())
                    {
                        memberInfoList.AddRange(reflect.GetMember(this.name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
                    }
                    else
                    {
                        memberInfoList.AddRange(reflect.GetMember(this.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
                    }
                }
                else
                {
                    memberInfoList.AddRange(reflect.GetMember(this.name, BindingFlags.Instance | BindingFlags.Public));
                }
            }
            return memberInfoList;
        }

        private IReflect[] GetAllEligibleClasses()
        {
            ArrayList arrayList = new ArrayList(16);
            ClassScope classScope = null;
            PackageScope packageScope = null;
            ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
            while (scriptObject is WithObject || scriptObject is BlockScope)
            {
                scriptObject = scriptObject.GetParent();
            }
            if (scriptObject is FunctionScope)
            {
                scriptObject = ((FunctionScope)scriptObject).owner.enclosing_scope;
            }
            if (scriptObject is ClassScope)
            {
                classScope = (ClassScope)scriptObject;
                packageScope = classScope.package;
            }
            if (classScope != null)
            {
                classScope.AddClassesFromInheritanceChain(this.name, arrayList);
            }
            if (packageScope != null)
            {
                packageScope.AddClassesExcluding(classScope, this.name, arrayList);
            }
            else
            {
                ((IActivationObject)scriptObject).GetGlobalScope().AddClassesExcluding(classScope, this.name, arrayList);
            }
            IReflect[] array = new IReflect[arrayList.Count];
            arrayList.CopyTo(array);
            return array;
        }

        protected abstract object GetObject();

        protected abstract void HandleNoSuchMemberError();

        internal override IReflect InferType(JSField inference_target)
        {
            if (this.isArrayElementAccess)
            {
                IReflect reflect = this.defaultMemberReturnIR;
                if (!(reflect is TypedArray))
                {
                    return ((Type)reflect).GetElementType();
                }
                return ((TypedArray)reflect).elementType;
            }
            else if (this.isAssignmentToDefaultIndexedProperty)
            {
                if (this.member is PropertyInfo)
                {
                    return ((PropertyInfo)this.member).PropertyType;
                }
                return Typeob.Object;
            }
            else
            {
                MemberInfo memberInfo = this.member;
                if (memberInfo is FieldInfo)
                {
                    JSWrappedField jSWrappedField = memberInfo as JSWrappedField;
                    if (jSWrappedField != null)
                    {
                        memberInfo = jSWrappedField.wrappedField;
                    }
                    if (memberInfo is JSVariableField)
                    {
                        return ((JSVariableField)memberInfo).GetInferredType(inference_target);
                    }
                    return ((FieldInfo)memberInfo).FieldType;
                }
                else if (memberInfo is PropertyInfo)
                {
                    JSWrappedProperty jSWrappedProperty = memberInfo as JSWrappedProperty;
                    if (jSWrappedProperty != null)
                    {
                        memberInfo = jSWrappedProperty.property;
                    }
                    if (memberInfo is JSProperty)
                    {
                        return ((JSProperty)memberInfo).PropertyIR();
                    }
                    PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                    if (propertyInfo.DeclaringType == Typeob.GlobalObject)
                    {
                        return (IReflect)propertyInfo.GetValue(base.Globals.globalObject, null);
                    }
                    return propertyInfo.PropertyType;
                }
                else
                {
                    if (memberInfo is Type)
                    {
                        return Typeob.Type;
                    }
                    if (memberInfo is EventInfo)
                    {
                        return Typeob.EventInfo;
                    }
                    if (this.members.Length > 0 && base.Engine.doFast)
                    {
                        return Typeob.ScriptFunction;
                    }
                    return Typeob.Object;
                }
            }
        }

        internal virtual IReflect InferTypeOfCall(JSField inference_target, bool isConstructor)
        {
            if (!this.isFullyResolved)
            {
                return Typeob.Object;
            }
            if (this.isArrayConstructor)
            {
                return this.defaultMemberReturnIR;
            }
            if (this.isArrayElementAccess)
            {
                IReflect reflect = this.defaultMemberReturnIR;
                if (!(reflect is TypedArray))
                {
                    return ((Type)reflect).GetElementType();
                }
                return ((TypedArray)reflect).elementType;
            }
            else
            {
                MemberInfo memberInfo = this.member;
                if (memberInfo is JSFieldMethod)
                {
                    if (!isConstructor)
                    {
                        return ((JSFieldMethod)memberInfo).ReturnIR();
                    }
                    return Typeob.Object;
                }
                else
                {
                    if (memberInfo is MethodInfo)
                    {
                        return ((MethodInfo)memberInfo).ReturnType;
                    }
                    if (memberInfo is JSConstructor)
                    {
                        return ((JSConstructor)memberInfo).GetClassScope();
                    }
                    if (memberInfo is ConstructorInfo)
                    {
                        return ((ConstructorInfo)memberInfo).DeclaringType;
                    }
                    if (memberInfo is Type)
                    {
                        return (Type)memberInfo;
                    }
                    if (memberInfo is FieldInfo && ((FieldInfo)memberInfo).IsLiteral)
                    {
                        object obj = (memberInfo is JSVariableField) ? ((JSVariableField)memberInfo).value : TypeReferences.GetConstantValue((FieldInfo)memberInfo);
                        if (obj is ClassScope || obj is TypedArray)
                        {
                            return (IReflect)obj;
                        }
                    }
                    return Typeob.Object;
                }
            }
        }

        private static bool InsideClassThatExtends(ScriptObject scope, Type type)
        {
            while (scope is WithObject || scope is BlockScope)
            {
                scope = scope.GetParent();
            }
            if (scope is ClassScope)
            {
                return type.IsAssignableFrom(((ClassScope)scope).GetBakedSuperType());
            }
            return scope is FunctionScope && Binding.InsideClassThatExtends(((FunctionScope)scope).owner.enclosing_scope, type);
        }

        internal void InvalidateBinding()
        {
            this.isAssignmentToDefaultIndexedProperty = false;
            this.isArrayConstructor = false;
            this.isArrayElementAccess = false;
            this.defaultMember = null;
            this.member = null;
            this.members = new MemberInfo[0];
        }

        internal bool IsCompatibleWithDelegate(Type delegateType)
        {
            MethodInfo method = delegateType.GetMethod("Invoke");
            ParameterInfo[] parameters = method.GetParameters();
            Type returnType = method.ReturnType;
            MemberInfo[] array = this.members;
            for (int i = 0; i < array.Length; i++)
            {
                MemberInfo memberInfo = array[i];
                if (memberInfo is MethodInfo)
                {
                    MethodInfo methodInfo = (MethodInfo)memberInfo;
                    Type type;
                    bool result;
                    if (methodInfo is JSFieldMethod)
                    {
                        IReflect reflect = ((JSFieldMethod)methodInfo).ReturnIR();
                        if (reflect is ClassScope)
                        {
                            type = ((ClassScope)reflect).GetBakedSuperType();
                        }
                        else if (reflect is Type)
                        {
                            type = (Type)reflect;
                        }
                        else
                        {
                            type = Convert.ToType(reflect);
                        }
                        if (((JSFieldMethod)methodInfo).func.isExpandoMethod)
                        {
                            result = false;
                            return result;
                        }
                    }
                    else
                    {
                        type = methodInfo.ReturnType;
                    }
                    if (type != returnType || !Class.ParametersMatch(parameters, methodInfo.GetParameters()))
                    {
                        goto IL_DC;
                    }
                    this.member = methodInfo;
                    this.isFullyResolved = true;
                    result = true;
                    return result;
                }
                IL_DC:;
            }
            return false;
        }

        public static bool IsMissing(object value)
        {
            return value is Missing;
        }

        private MethodInfo LookForParameterlessPropertyGetter()
        {
            int i = 0;
            int num = this.members.Length;
            while (i < num)
            {
                PropertyInfo propertyInfo = this.members[i] as PropertyInfo;
                if (propertyInfo != null)
                {
                    MethodInfo getMethod = propertyInfo.GetGetMethod(true);
                    if (getMethod != null)
                    {
                        ParameterInfo[] parameters = getMethod.GetParameters();
                        if (parameters != null && parameters.Length != 0)
                        {
                            goto IL_3B;
                        }
                    }
                    i++;
                    continue;
                }
                IL_3B:
                return null;
            }
            try
            {
                MethodInfo methodInfo = JSBinder.SelectMethod(this.members, new IReflect[0]);
                if (methodInfo != null && methodInfo.IsSpecialName)
                {
                    return methodInfo;
                }
            }
            catch (AmbiguousMatchException)
            {
            }
            return null;
        }

        internal override bool OkToUseAsType()
        {
            MemberInfo memberInfo = this.member;
            if (memberInfo is Type)
            {
                return this.isFullyResolved = true;
            }
            if (memberInfo is FieldInfo)
            {
                FieldInfo fieldInfo = (FieldInfo)memberInfo;
                if (fieldInfo.IsLiteral)
                {
                    return (!(fieldInfo is JSMemberField) || !(((JSMemberField)fieldInfo).value is ClassScope) || fieldInfo.IsStatic) && (this.isFullyResolved = true);
                }
                if (!(memberInfo is JSField) && fieldInfo.IsStatic && fieldInfo.GetValue(null) is Type)
                {
                    return this.isFullyResolved = true;
                }
            }
            return false;
        }

        private int PlaceValuesForHiddenParametersOnStack(ILGenerator il, MethodInfo meth, ParameterInfo[] pars)
        {
            int num = 0;
            if (meth is JSFieldMethod)
            {
                FunctionObject func = ((JSFieldMethod)meth).func;
                if (func != null && func.isMethod)
                {
                    return 0;
                }
                if (this is Lookup)
                {
                    ((Lookup)this).TranslateToILDefaultThisObject(il);
                }
                else
                {
                    this.TranslateToILObject(il, Typeob.Object, false);
                }
                base.EmitILToLoadEngine(il);
                return 0;
            }
            else
            {
                object[] customAttributes = CustomAttribute.GetCustomAttributes(meth, typeof(JSFunctionAttribute), false);
                if (customAttributes == null || customAttributes.Length == 0)
                {
                    return 0;
                }
                JSFunctionAttributeEnum attributeValue = ((JSFunctionAttribute)customAttributes[0]).attributeValue;
                if ((attributeValue & JSFunctionAttributeEnum.HasThisObject) != JSFunctionAttributeEnum.None)
                {
                    num = 1;
                    Type parameterType = pars[0].ParameterType;
                    if (this is Lookup && parameterType == Typeob.Object)
                    {
                        ((Lookup)this).TranslateToILDefaultThisObject(il);
                    }
                    else if (Typeob.ArrayObject.IsAssignableFrom(this.member.DeclaringType))
                    {
                        this.TranslateToILObject(il, Typeob.ArrayObject, false);
                    }
                    else
                    {
                        this.TranslateToILObject(il, parameterType, false);
                    }
                }
                if ((attributeValue & JSFunctionAttributeEnum.HasEngine) != JSFunctionAttributeEnum.None)
                {
                    num++;
                    base.EmitILToLoadEngine(il);
                }
                return num;
            }
        }

        private bool ParameterlessPropertyValueIsCallable(MethodInfo meth, ASTList args, IReflect[] argIRs, bool constructor, bool brackets)
        {
            ParameterInfo[] parameters = meth.GetParameters();
            if (parameters == null || parameters.Length == 0)
            {
                if ((meth is JSWrappedMethod && ((JSWrappedMethod)meth).GetWrappedObject() is GlobalObject) || argIRs.Length > 0 || (!(meth is JSMethod) && Typeob.ScriptFunction.IsAssignableFrom(meth.ReturnType)))
                {
                    this.member = this.ResolveOtherKindOfCall(args, argIRs, constructor, brackets);
                    return true;
                }
                IReflect reflect = (meth is JSFieldMethod) ? ((JSFieldMethod)meth).ReturnIR() : meth.ReturnType;
                if ((Type)reflect == Typeob.Object || (Type)reflect == Typeob.ScriptFunction)
                {
                    this.member = this.ResolveOtherKindOfCall(args, argIRs, constructor, brackets);
                    return true;
                }
                this.context.HandleError(JSError.InvalidCall);
            }
            return false;
        }

        internal static void PlaceArgumentsOnStack(ILGenerator il, ParameterInfo[] pars, ASTList args, int offset, int rhoffset, AST missing)
        {
            int count = args.count;
            int num = count + offset;
            int num2 = pars.Length - rhoffset;
            bool flag = num2 > 0 && CustomAttribute.IsDefined(pars[num2 - 1], typeof(ParamArrayAttribute), false) && (count != num2 || !Convert.IsArrayType(args[count - 1].InferType(null)));
            Type type = flag ? pars[--num2].ParameterType.GetElementType() : null;
            if (num > num2)
            {
                num = num2;
            }
            for (int i = offset; i < num; i++)
            {
                Type parameterType = pars[i].ParameterType;
                AST aST = args[i - offset];
                if (aST is ConstantWrapper && ((ConstantWrapper)aST).value == System.Reflection.Missing.Value)
                {
                    object defaultParameterValue = TypeReferences.GetDefaultParameterValue(pars[i]);
                    ((ConstantWrapper)aST).value = ((defaultParameterValue != System.Convert.DBNull) ? defaultParameterValue : null);
                }
                if (parameterType.IsByRef)
                {
                    aST.TranslateToILReference(il, parameterType.GetElementType());
                }
                else
                {
                    aST.TranslateToIL(il, parameterType);
                }
            }
            if (num < num2)
            {
                for (int j = num; j < num2; j++)
                {
                    Type parameterType2 = pars[j].ParameterType;
                    if (TypeReferences.GetDefaultParameterValue(pars[j]) == System.Convert.DBNull)
                    {
                        if (parameterType2.IsByRef)
                        {
                            missing.TranslateToILReference(il, parameterType2.GetElementType());
                        }
                        else
                        {
                            missing.TranslateToIL(il, parameterType2);
                        }
                    }
                    else if (parameterType2.IsByRef)
                    {
                        new ConstantWrapper(TypeReferences.GetDefaultParameterValue(pars[j]), null).TranslateToILReference(il, parameterType2.GetElementType());
                    }
                    else
                    {
                        new ConstantWrapper(TypeReferences.GetDefaultParameterValue(pars[j]), null).TranslateToIL(il, parameterType2);
                    }
                }
            }
            if (flag)
            {
                num -= offset;
                num2 = ((count > num) ? (count - num) : 0);
                ConstantWrapper.TranslateToILInt(il, num2);
                il.Emit(OpCodes.Newarr, type);
                bool flag2 = type.IsValueType && !type.IsPrimitive;
                for (int k = 0; k < num2; k++)
                {
                    il.Emit(OpCodes.Dup);
                    ConstantWrapper.TranslateToILInt(il, k);
                    if (flag2)
                    {
                        il.Emit(OpCodes.Ldelema, type);
                    }
                    args[k + num].TranslateToIL(il, type);
                    Binding.TranslateToStelem(il, type);
                }
            }
        }

        internal bool RefersToMemoryLocation()
        {
            return this.isFullyResolved && (this.isArrayElementAccess || this.member is FieldInfo);
        }

        internal override void ResolveCall(ASTList args, IReflect[] argIRs, bool constructor, bool brackets)
        {
            this.argIRs = argIRs;
            if (this.members != null && this.members.Length != 0)
            {
                MemberInfo memberInfo = null;
                if (!(this is CallableExpression))
                {
                    if (constructor)
                    {
                        if (brackets)
                        {
                            goto IL_1B8;
                        }
                    }
                    try
                    {
                        if (constructor)
                        {
                            memberInfo = (this.member = JSBinder.SelectConstructor(this.members, argIRs));
                        }
                        else
                        {
                            MethodInfo methodInfo;
                            memberInfo = (this.member = (methodInfo = JSBinder.SelectMethod(this.members, argIRs)));
                            if (methodInfo != null && methodInfo.IsSpecialName)
                            {
                                if (this.name == methodInfo.Name)
                                {
                                    if (this.name.StartsWith("get_", StringComparison.Ordinal) || this.name.StartsWith("set_", StringComparison.Ordinal))
                                    {
                                        this.context.HandleError(JSError.NotMeantToBeCalledDirectly);
                                        this.member = null;
                                        return;
                                    }
                                }
                                else if (this.ParameterlessPropertyValueIsCallable(methodInfo, args, argIRs, constructor, brackets))
                                {
                                    return;
                                }
                            }
                        }
                    }
                    catch (AmbiguousMatchException)
                    {
                        if (constructor)
                        {
                            this.context.HandleError(JSError.AmbiguousConstructorCall, this.isFullyResolved);
                        }
                        else
                        {
                            MethodInfo methodInfo2 = this.LookForParameterlessPropertyGetter();
                            if (methodInfo2 == null || !this.ParameterlessPropertyValueIsCallable(methodInfo2, args, argIRs, constructor, brackets))
                            {
                                this.context.HandleError(JSError.AmbiguousMatch, this.isFullyResolved);
                                this.member = null;
                            }
                        }
                        return;
                    }
                    catch (JScriptException ex)
                    {
                        this.context.HandleError((JSError)(ex.ErrorNumber & 65535), ex.Message, true);
                        return;
                    }
                }
                IL_1B8:
                if (memberInfo == null)
                {
                    memberInfo = this.ResolveOtherKindOfCall(args, argIRs, constructor, brackets);
                }
                if (memberInfo != null)
                {
                    if (!this.Accessible(false))
                    {
                        this.member = null;
                        return;
                    }
                    this.WarnIfObsolete();
                    if (memberInfo is MethodBase)
                    {
                        if (CustomAttribute.IsDefined(memberInfo, typeof(JSFunctionAttribute), false) && !(this.defaultMember is PropertyInfo))
                        {
                            int num = 0;
                            object[] customAttributes = CustomAttribute.GetCustomAttributes(memberInfo, typeof(JSFunctionAttribute), false);
                            JSFunctionAttributeEnum attributeValue = ((JSFunctionAttribute)customAttributes[0]).attributeValue;
                            if ((constructor && !(memberInfo is ConstructorInfo)) || (attributeValue & JSFunctionAttributeEnum.HasArguments) != JSFunctionAttributeEnum.None)
                            {
                                this.member = LateBinding.SelectMember(this.members);
                                this.defaultMember = null;
                                return;
                            }
                            if ((attributeValue & JSFunctionAttributeEnum.HasThisObject) != JSFunctionAttributeEnum.None)
                            {
                                num = 1;
                            }
                            if ((attributeValue & JSFunctionAttributeEnum.HasEngine) != JSFunctionAttributeEnum.None)
                            {
                                num++;
                            }
                            if (!Binding.CheckParameters(((MethodBase)memberInfo).GetParameters(), argIRs, args, this.context, num, true, this.isFullyResolved))
                            {
                                this.member = null;
                                return;
                            }
                        }
                        else
                        {
                            if (constructor && memberInfo is JSFieldMethod)
                            {
                                this.member = LateBinding.SelectMember(this.members);
                                return;
                            }
                            if (constructor && memberInfo is ConstructorInfo && !(memberInfo is JSConstructor) && Typeob.Delegate.IsAssignableFrom(memberInfo.DeclaringType))
                            {
                                this.context.HandleError(JSError.DelegatesShouldNotBeExplicitlyConstructed);
                                this.member = null;
                                return;
                            }
                            if (!Binding.CheckParameters(((MethodBase)memberInfo).GetParameters(), argIRs, args, this.context, 0, false, this.isFullyResolved))
                            {
                                this.member = null;
                            }
                        }
                    }
                    return;
                }
                return;
            }
            if (!constructor || !this.isFullyResolved || !base.Engine.doFast)
            {
                this.HandleNoSuchMemberError();
                return;
            }
            if (this.member != null && (this.member is Type || (this.member is FieldInfo && ((FieldInfo)this.member).IsLiteral)))
            {
                this.context.HandleError(JSError.NoConstructor);
                return;
            }
            this.HandleNoSuchMemberError();
        }

        internal override object ResolveCustomAttribute(ASTList args, IReflect[] argIRs, AST target)
        {
            try
            {
                this.ResolveCall(args, argIRs, true, false);
            }
            catch (AmbiguousMatchException)
            {
                this.context.HandleError(JSError.AmbiguousConstructorCall);
                return null;
            }
            JSConstructor jSConstructor = this.member as JSConstructor;
            if (jSConstructor != null)
            {
                ClassScope classScope = jSConstructor.GetClassScope();
                if (classScope.owner.IsCustomAttribute())
                {
                    return classScope;
                }
            }
            else
            {
                ConstructorInfo constructorInfo = this.member as ConstructorInfo;
                if (constructorInfo != null)
                {
                    Type declaringType = constructorInfo.DeclaringType;
                    if (Typeob.Attribute.IsAssignableFrom(declaringType))
                    {
                        object[] customAttributes = CustomAttribute.GetCustomAttributes(declaringType, typeof(AttributeUsageAttribute), false);
                        if (customAttributes.Length > 0)
                        {
                            return declaringType;
                        }
                    }
                }
            }
            this.context.HandleError(JSError.InvalidCustomAttributeClassOrCtor);
            return null;
        }

        internal void ResolveLHValue()
        {
            MemberInfo memberInfo = this.member = LateBinding.SelectMember(this.members);
            if ((memberInfo != null && !this.Accessible(true)) || (this.member == null && this.members.Length > 0))
            {
                this.context.HandleError(JSError.AssignmentToReadOnly, this.isFullyResolved);
                this.member = null;
                this.members = new MemberInfo[0];
                return;
            }
            if (memberInfo is JSPrototypeField)
            {
                this.member = null;
                this.members = new MemberInfo[0];
                return;
            }
            this.WarnIfNotFullyResolved();
            this.WarnIfObsolete();
        }

        private MemberInfo ResolveOtherKindOfCall(ASTList argList, IReflect[] argIRs, bool constructor, bool brackets)
        {
            MemberInfo memberInfo = this.member = LateBinding.SelectMember(this.members);
            if (memberInfo is PropertyInfo && !(memberInfo is JSProperty) && memberInfo.DeclaringType == Typeob.GlobalObject)
            {
                PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                Type propertyType = propertyInfo.PropertyType;
                if (propertyType == Typeob.Type)
                {
                    memberInfo = (Type)propertyInfo.GetValue(null, null);
                }
                else if (constructor && brackets)
                {
                    MethodInfo method = propertyType.GetMethod("CreateInstance", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                    if (method != null)
                    {
                        Type returnType = method.ReturnType;
                        if (returnType == Typeob.BooleanObject)
                        {
                            memberInfo = Typeob.Boolean;
                        }
                        else if (returnType == Typeob.StringObject)
                        {
                            memberInfo = Typeob.String;
                        }
                        else
                        {
                            memberInfo = returnType;
                        }
                    }
                }
            }
            CallableExpression callableExpression = this as CallableExpression;
            if (callableExpression != null)
            {
                ConstantWrapper constantWrapper = callableExpression.expression as ConstantWrapper;
                if (constantWrapper != null && constantWrapper.InferType(null) is Type)
                {
                    memberInfo = new JSGlobalField(null, null, constantWrapper.value, FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Literal);
                }
            }
            if (memberInfo is Type)
            {
                if (constructor)
                {
                    if (brackets)
                    {
                        this.isArrayConstructor = true;
                        this.defaultMember = memberInfo;
                        this.defaultMemberReturnIR = new TypedArray((Type)memberInfo, argIRs.Length);
                        int i = 0;
                        int num = argIRs.Length;
                        while (i < num)
                        {
                            if ((Type)argIRs[i] != Typeob.Object && !Convert.IsPrimitiveNumericType(argIRs[i]))
                            {
                                argList[i].context.HandleError(JSError.TypeMismatch, this.isFullyResolved);
                                break;
                            }
                            i++;
                        }
                        return this.member = memberInfo;
                    }
                    ConstructorInfo[] constructors = ((Type)memberInfo).GetConstructors(BindingFlags.Instance | BindingFlags.Public);
                    if (constructors == null || constructors.Length == 0)
                    {
                        this.context.HandleError(JSError.NoConstructor);
                        this.member = null;
                        return null;
                    }
                    this.members = constructors;
                    this.ResolveCall(argList, argIRs, true, brackets);
                    return this.member;
                }
                else
                {
                    if (!brackets && argIRs.Length == 1)
                    {
                        return memberInfo;
                    }
                    this.context.HandleError(JSError.InvalidCall);
                    return this.member = null;
                }
            }
            else
            {
                if (memberInfo is JSPrototypeField)
                {
                    return this.member = null;
                }
                if (memberInfo is FieldInfo && ((FieldInfo)memberInfo).IsLiteral)
                {
                    if (!this.AccessibleField(false))
                    {
                        return this.member = null;
                    }
                    object obj = (memberInfo is JSVariableField) ? ((JSVariableField)memberInfo).value : TypeReferences.GetConstantValue((FieldInfo)memberInfo);
                    if (obj is ClassScope || obj is Type)
                    {
                        if (constructor)
                        {
                            if (brackets)
                            {
                                this.isArrayConstructor = true;
                                this.defaultMember = memberInfo;
                                this.defaultMemberReturnIR = new TypedArray((obj is ClassScope) ? ((IReflect)obj) : ((IReflect)obj), argIRs.Length);
                                int j = 0;
                                int num2 = argIRs.Length;
                                while (j < num2)
                                {
                                    if ((Type)argIRs[j] != Typeob.Object && !Convert.IsPrimitiveNumericType(argIRs[j]))
                                    {
                                        argList[j].context.HandleError(JSError.TypeMismatch, this.isFullyResolved);
                                        break;
                                    }
                                    j++;
                                }
                                return this.member = memberInfo;
                            }
                            ConstantWrapper constantWrapper2;
                            if (obj is ClassScope && !((ClassScope)obj).owner.isStatic && this is Member && (constantWrapper2 = (((Member)this).rootObject as ConstantWrapper)) != null && !(constantWrapper2.Evaluate() is Namespace))
                            {
                                ((Member)this).rootObject.context.HandleError(JSError.NeedInstance);
                                return null;
                            }
                            this.members = ((obj is ClassScope) ? ((ClassScope)obj).constructors : ((Type)obj).GetConstructors(BindingFlags.Instance | BindingFlags.Public));
                            if (this.members == null || this.members.Length == 0)
                            {
                                this.context.HandleError(JSError.NoConstructor);
                                this.member = null;
                                return null;
                            }
                            this.ResolveCall(argList, argIRs, true, brackets);
                            return this.member;
                        }
                        else
                        {
                            if (!brackets && argIRs.Length == 1)
                            {
                                Type type = obj as Type;
                                return this.member = ((type != null) ? type : memberInfo);
                            }
                            this.context.HandleError(JSError.InvalidCall);
                            return this.member = null;
                        }
                    }
                    else if (obj is TypedArray)
                    {
                        if (!constructor)
                        {
                            if (argIRs.Length == 1 && !brackets)
                            {
                                return this.member = memberInfo;
                            }
                            goto IL_8E1;
                        }
                        else
                        {
                            if (brackets && argIRs.Length != 0)
                            {
                                this.isArrayConstructor = true;
                                this.defaultMember = memberInfo;
                                this.defaultMemberReturnIR = new TypedArray((IReflect)obj, argIRs.Length);
                                int k = 0;
                                int num3 = argIRs.Length;
                                while (k < num3)
                                {
                                    if ((Type)argIRs[k] != Typeob.Object && !Convert.IsPrimitiveNumericType(argIRs[k]))
                                    {
                                        argList[k].context.HandleError(JSError.TypeMismatch, this.isFullyResolved);
                                        break;
                                    }
                                    k++;
                                }
                                return this.member = memberInfo;
                            }
                            goto IL_8E1;
                        }
                    }
                    else if (obj is FunctionObject)
                    {
                        FunctionObject functionObject = (FunctionObject)obj;
                        if (!functionObject.isExpandoMethod && !functionObject.Must_save_stack_locals && (functionObject.own_scope.ProvidesOuterScopeLocals == null || functionObject.own_scope.ProvidesOuterScopeLocals.count == 0))
                        {
                            return this.member = ((JSVariableField)this.member).GetAsMethod(functionObject.own_scope);
                        }
                        return this.member;
                    }
                }
                IReflect reflect = this.InferType(null);
                Type type2 = reflect as Type;
                if (!brackets && ((type2 != null && Typeob.ScriptFunction.IsAssignableFrom(type2)) || reflect is ScriptFunction))
                {
                    this.defaultMember = memberInfo;
                    if (type2 == null)
                    {
                        this.defaultMemberReturnIR = Globals.TypeRefs.ToReferenceContext(reflect.GetType());
                        this.member = this.defaultMemberReturnIR.GetMethod(constructor ? "CreateInstance" : "Invoke", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                        if (this.member == null)
                        {
                            this.defaultMemberReturnIR = Typeob.ScriptFunction;
                            this.member = this.defaultMemberReturnIR.GetMethod(constructor ? "CreateInstance" : "Invoke", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                        }
                        return this.member;
                    }
                    if (constructor && this.members.Length != 0 && this.members[0] is JSFieldMethod)
                    {
                        JSFieldMethod jSFieldMethod = (JSFieldMethod)this.members[0];
                        jSFieldMethod.func.PartiallyEvaluate();
                        if (!jSFieldMethod.func.isExpandoMethod)
                        {
                            this.context.HandleError(JSError.NotAnExpandoFunction, this.isFullyResolved);
                        }
                    }
                    this.defaultMemberReturnIR = type2;
                    return this.member = type2.GetMethod(constructor ? "CreateInstance" : "Invoke", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                }
                else
                {
                    if ((Type)reflect == Typeob.Type)
                    {
                        this.member = null;
                        return null;
                    }
                    if ((Type)reflect == Typeob.Object || (reflect is ScriptObject && brackets && !(reflect is ClassScope)))
                    {
                        return memberInfo;
                    }
                    if (reflect is TypedArray || (reflect is Type && ((Type)reflect).IsArray))
                    {
                        int num4 = argIRs.Length;
                        int num5 = (reflect is TypedArray) ? ((TypedArray)reflect).rank : ((Type)reflect).GetArrayRank();
                        if (num4 != num5)
                        {
                            this.context.HandleError(JSError.IncorrectNumberOfIndices, this.isFullyResolved);
                        }
                        else
                        {
                            for (int l = 0; l < num5; l++)
                            {
                                if ((Type)argIRs[l] != Typeob.Object && (!Convert.IsPrimitiveNumericType(argIRs[l]) || Convert.IsBadIndex(argList[l])))
                                {
                                    argList[l].context.HandleError(JSError.TypeMismatch, this.isFullyResolved);
                                    break;
                                }
                            }
                        }
                        if (constructor)
                        {
                            if (!brackets)
                            {
                                goto IL_8E1;
                            }
                            if (!(reflect is TypedArray))
                            {
                                ((Type)reflect).GetElementType();
                            }
                            else
                            {
                                IReflect arg_781_0 = ((TypedArray)reflect).elementType;
                            }
                            if ((Type)reflect != Typeob.Object && !(reflect is ClassScope) && (!(reflect is Type) || Typeob.Type.IsAssignableFrom((Type)reflect) || Typeob.ScriptFunction.IsAssignableFrom((Type)reflect)))
                            {
                                goto IL_8E1;
                            }
                        }
                        this.isArrayElementAccess = true;
                        this.defaultMember = memberInfo;
                        this.defaultMemberReturnIR = reflect;
                        return null;
                    }
                    if (!constructor)
                    {
                        if (brackets && (Type)reflect == Typeob.String && (this.argIRs.Length != 1 || !Convert.IsPrimitiveNumericType(argIRs[0])))
                        {
                            reflect = Typeob.StringObject;
                        }
                        MemberInfo[] array = (brackets || !(reflect is ScriptObject)) ? JSBinder.GetDefaultMembers(reflect) : null;
                        if (array != null && array.Length > 0)
                        {
                            try
                            {
                                this.defaultMember = memberInfo;
                                this.defaultMemberReturnIR = reflect;
                                MemberInfo result = this.member = JSBinder.SelectMethod(this.members = array, argIRs);
                                return result;
                            }
                            catch (AmbiguousMatchException)
                            {
                                this.context.HandleError(JSError.AmbiguousMatch, this.isFullyResolved);
                                MemberInfo result = this.member = null;
                                return result;
                            }
                        }
                        if (!brackets && reflect is Type && Typeob.Delegate.IsAssignableFrom((Type)reflect))
                        {
                            this.defaultMember = memberInfo;
                            this.defaultMemberReturnIR = reflect;
                            return this.member = ((Type)reflect).GetMethod("Invoke");
                        }
                    }
                }
                IL_8E1:
                if (constructor)
                {
                    this.context.HandleError(JSError.NeedType, this.isFullyResolved);
                }
                else if (brackets)
                {
                    this.context.HandleError(JSError.NotIndexable, this.isFullyResolved);
                }
                else
                {
                    this.context.HandleError(JSError.FunctionExpected, this.isFullyResolved);
                }
                return this.member = null;
            }
        }

        protected void ResolveRHValue()
        {
            MemberInfo memberInfo = this.member = LateBinding.SelectMember(this.members);
            JSLocalField jSLocalField = this.member as JSLocalField;
            if (jSLocalField != null)
            {
                FunctionObject functionObject = jSLocalField.value as FunctionObject;
                if (functionObject != null)
                {
                    FunctionScope functionScope = functionObject.enclosing_scope as FunctionScope;
                    if (functionScope != null)
                    {
                        functionScope.closuresMightEscape = true;
                    }
                }
            }
            if (memberInfo is JSPrototypeField)
            {
                this.member = null;
                return;
            }
            if (!this.Accessible(false))
            {
                this.member = null;
                return;
            }
            this.WarnIfObsolete();
            this.WarnIfNotFullyResolved();
        }

        internal override void SetPartialValue(AST partial_value)
        {
            Binding.AssignmentCompatible(this.InferType(null), partial_value, partial_value.InferType(null), this.isFullyResolved);
        }

        internal void SetPartialValue(ASTList argList, IReflect[] argIRs, AST partial_value, bool inBrackets)
        {
            if (this.members == null || this.members.Length == 0)
            {
                this.HandleNoSuchMemberError();
                this.isAssignmentToDefaultIndexedProperty = true;
                return;
            }
            this.PartiallyEvaluate();
            IReflect reflect = this.InferType(null);
            this.isAssignmentToDefaultIndexedProperty = true;
            if ((Type)reflect == Typeob.Object)
            {
                JSVariableField jSVariableField = this.member as JSVariableField;
                if (jSVariableField == null || !jSVariableField.IsLiteral || !(jSVariableField.value is ClassScope))
                {
                    return;
                }
                reflect = Typeob.Type;
            }
            else
            {
                if (reflect is TypedArray || (reflect is Type && ((Type)reflect).IsArray))
                {
                    bool flag = false;
                    int num = argIRs.Length;
                    int num2 = (reflect is TypedArray) ? ((TypedArray)reflect).rank : ((Type)reflect).GetArrayRank();
                    if (num != num2)
                    {
                        this.context.HandleError(JSError.IncorrectNumberOfIndices, this.isFullyResolved);
                        flag = true;
                    }
                    for (int i = 0; i < num2; i++)
                    {
                        if (!flag && i < num && (Type)argIRs[i] != Typeob.Object && (!Convert.IsPrimitiveNumericType(argIRs[i]) || Convert.IsBadIndex(argList[i])))
                        {
                            argList[i].context.HandleError(JSError.TypeMismatch, this.isFullyResolved);
                            flag = true;
                        }
                    }
                    this.isArrayElementAccess = true;
                    this.isAssignmentToDefaultIndexedProperty = false;
                    this.defaultMember = this.member;
                    this.defaultMemberReturnIR = reflect;
                    IReflect lhir = (reflect is TypedArray) ? ((TypedArray)reflect).elementType : ((Type)reflect).GetElementType();
                    Binding.AssignmentCompatible(lhir, partial_value, partial_value.InferType(null), this.isFullyResolved);
                    return;
                }
                MemberInfo[] defaultMembers = JSBinder.GetDefaultMembers(reflect);
                if (defaultMembers != null && defaultMembers.Length > 0 && this.member != null)
                {
                    try
                    {
                        PropertyInfo propertyInfo = JSBinder.SelectProperty(defaultMembers, argIRs);
                        if (propertyInfo == null)
                        {
                            this.context.HandleError(JSError.NotIndexable, Convert.ToTypeName(reflect));
                        }
                        else if (JSProperty.GetSetMethod(propertyInfo, true) == null)
                        {
                            if ((Type)reflect == Typeob.String)
                            {
                                this.context.HandleError(JSError.UselessAssignment);
                            }
                            else
                            {
                                this.context.HandleError(JSError.AssignmentToReadOnly, this.isFullyResolved && base.Engine.doFast);
                            }
                        }
                        else if (Binding.CheckParameters(propertyInfo.GetIndexParameters(), argIRs, argList, this.context, 0, false, true))
                        {
                            this.defaultMember = this.member;
                            this.defaultMemberReturnIR = reflect;
                            this.members = defaultMembers;
                            this.member = propertyInfo;
                        }
                    }
                    catch (AmbiguousMatchException)
                    {
                        this.context.HandleError(JSError.AmbiguousMatch, this.isFullyResolved);
                        this.member = null;
                    }
                    return;
                }
            }
            this.member = null;
            if (!inBrackets)
            {
                this.context.HandleError(JSError.IllegalAssignment);
                return;
            }
            this.context.HandleError(JSError.NotIndexable, Convert.ToTypeName(reflect));
        }

        internal override void SetValue(object value)
        {
            MemberInfo memberInfo = this.member;
            object @object = this.GetObject();
            if (memberInfo is FieldInfo)
            {
                FieldInfo fieldInfo = (FieldInfo)memberInfo;
                if (fieldInfo.IsLiteral || fieldInfo.IsInitOnly)
                {
                    return;
                }
                if (!(fieldInfo is JSField) || fieldInfo is JSWrappedField)
                {
                    value = Convert.CoerceT(value, fieldInfo.FieldType, false);
                }
                fieldInfo.SetValue(@object, value, BindingFlags.SuppressChangeType, null, null);
                return;
            }
            else if (memberInfo is PropertyInfo)
            {
                PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                if (@object is ClassScope && !(propertyInfo is JSProperty))
                {
                    JSProperty.SetValue(propertyInfo, ((WithObject)((ClassScope)@object).GetParent()).contained_object, value, null);
                    return;
                }
                if (!(propertyInfo is JSProperty))
                {
                    value = Convert.CoerceT(value, propertyInfo.PropertyType, false);
                }
                JSProperty.SetValue(propertyInfo, @object, value, null);
                return;
            }
            else
            {
                if (this.members == null || this.members.Length == 0)
                {
                    this.EvaluateAsLateBinding().SetValue(value);
                    return;
                }
                throw new JScriptException(JSError.IllegalAssignment);
            }
        }

        internal override void TranslateToIL(ILGenerator il, Type rtype)
        {
            this.TranslateToIL(il, rtype, false, false);
        }

        internal void TranslateToIL(ILGenerator il, Type rtype, bool calledFromDelete)
        {
            this.TranslateToIL(il, rtype, false, false, calledFromDelete);
        }

        private void TranslateToIL(ILGenerator il, Type rtype, bool preSet, bool preSetPlusGet)
        {
            this.TranslateToIL(il, rtype, preSet, preSetPlusGet, false);
        }

        private void TranslateToIL(ILGenerator il, Type rtype, bool preSet, bool preSetPlusGet, bool calledFromDelete)
        {
            if (this.member is FieldInfo)
            {
                FieldInfo fieldInfo = (FieldInfo)this.member;
                bool flag = fieldInfo.IsStatic || fieldInfo.IsLiteral;
                if (fieldInfo.IsLiteral && fieldInfo is JSMemberField)
                {
                    object value = ((JSMemberField)fieldInfo).value;
                    FunctionObject functionObject = value as FunctionObject;
                    flag = (functionObject == null || !functionObject.isExpandoMethod);
                }
                if (!flag || fieldInfo is JSClosureField)
                {
                    this.TranslateToILObject(il, fieldInfo.DeclaringType, true);
                    if (preSetPlusGet)
                    {
                        il.Emit(OpCodes.Dup);
                    }
                    flag = false;
                }
                if (!preSet)
                {
                    object obj = (fieldInfo is JSField) ? ((JSField)fieldInfo).GetMetaData() : ((fieldInfo is JSFieldInfo) ? ((JSFieldInfo)fieldInfo).field : fieldInfo);
                    if (obj is FieldInfo && !((FieldInfo)obj).IsLiteral)
                    {
                        il.Emit(flag ? OpCodes.Ldsfld : OpCodes.Ldfld, (FieldInfo)obj);
                    }
                    else if (obj is LocalBuilder)
                    {
                        il.Emit(OpCodes.Ldloc, (LocalBuilder)obj);
                    }
                    else
                    {
                        if (fieldInfo.IsLiteral)
                        {
                            new ConstantWrapper(TypeReferences.GetConstantValue(fieldInfo), this.context).TranslateToIL(il, rtype);
                            return;
                        }
                        Convert.EmitLdarg(il, (short)obj);
                    }
                    Convert.Emit(this, il, fieldInfo.FieldType, rtype);
                }
                return;
            }
            if (this.member is PropertyInfo)
            {
                PropertyInfo prop = (PropertyInfo)this.member;
                MethodInfo methodInfo = preSet ? JSProperty.GetSetMethod(prop, true) : JSProperty.GetGetMethod(prop, true);
                if (methodInfo != null)
                {
                    bool flag2 = methodInfo.IsStatic && !(methodInfo is JSClosureMethod);
                    if (!flag2)
                    {
                        Type declaringType = methodInfo.DeclaringType;
                        if (declaringType == Typeob.StringObject && methodInfo.Name.Equals("get_length"))
                        {
                            this.TranslateToILObject(il, Typeob.String, false);
                            methodInfo = CompilerGlobals.stringLengthMethod;
                        }
                        else
                        {
                            this.TranslateToILObject(il, declaringType, true);
                        }
                    }
                    if (!preSet)
                    {
                        methodInfo = this.GetMethodInfoMetadata(methodInfo);
                        if (flag2)
                        {
                            il.Emit(OpCodes.Call, methodInfo);
                        }
                        else
                        {
                            if (preSetPlusGet)
                            {
                                il.Emit(OpCodes.Dup);
                            }
                            if (!this.isNonVirtual && methodInfo.IsVirtual && !methodInfo.IsFinal && (!methodInfo.ReflectedType.IsSealed || !methodInfo.ReflectedType.IsValueType))
                            {
                                il.Emit(OpCodes.Callvirt, methodInfo);
                            }
                            else
                            {
                                il.Emit(OpCodes.Call, methodInfo);
                            }
                        }
                        Convert.Emit(this, il, methodInfo.ReturnType, rtype);
                    }
                    return;
                }
                if (preSet)
                {
                    return;
                }
                if (this is Lookup)
                {
                    il.Emit(OpCodes.Ldc_I4, 5041);
                    il.Emit(OpCodes.Newobj, CompilerGlobals.scriptExceptionConstructor);
                    il.Emit(OpCodes.Throw);
                    return;
                }
                il.Emit(OpCodes.Ldnull);
                return;
            }
            else
            {
                if (!(this.member is MethodInfo))
                {
                    object obj2 = null;
                    if (this is Lookup)
                    {
                        ((Lookup)this).TranslateToLateBinding(il);
                    }
                    else
                    {
                        if (!this.isFullyResolved && !preSet && !preSetPlusGet)
                        {
                            obj2 = this.TranslateToSpeculativeEarlyBindings(il, rtype, false);
                        }
                        ((Member)this).TranslateToLateBinding(il, obj2 != null);
                        if (!this.isFullyResolved && preSetPlusGet)
                        {
                            obj2 = this.TranslateToSpeculativeEarlyBindings(il, rtype, true);
                        }
                    }
                    if (preSetPlusGet)
                    {
                        il.Emit(OpCodes.Dup);
                    }
                    if (!preSet)
                    {
                        if (this is Lookup && !calledFromDelete)
                        {
                            il.Emit(OpCodes.Call, CompilerGlobals.getValue2Method);
                        }
                        else
                        {
                            il.Emit(OpCodes.Call, CompilerGlobals.getNonMissingValueMethod);
                        }
                        Convert.Emit(this, il, Typeob.Object, rtype);
                        if (obj2 != null)
                        {
                            il.MarkLabel((Label)obj2);
                        }
                    }
                    return;
                }
                MethodInfo methodInfoMetadata = this.GetMethodInfoMetadata((MethodInfo)this.member);
                if (Typeob.Delegate.IsAssignableFrom(rtype))
                {
                    if (!methodInfoMetadata.IsStatic)
                    {
                        Type declaringType2 = methodInfoMetadata.DeclaringType;
                        this.TranslateToILObject(il, declaringType2, false);
                        if (declaringType2.IsValueType)
                        {
                            il.Emit(OpCodes.Box, declaringType2);
                        }
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldnull);
                    }
                    if (methodInfoMetadata.IsVirtual && !methodInfoMetadata.IsFinal && (!methodInfoMetadata.ReflectedType.IsSealed || !methodInfoMetadata.ReflectedType.IsValueType))
                    {
                        il.Emit(OpCodes.Dup);
                        il.Emit(OpCodes.Ldvirtftn, methodInfoMetadata);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldftn, methodInfoMetadata);
                    }
                    ConstructorInfo constructor = rtype.GetConstructor(new Type[]
                    {
                        Typeob.Object,
                        Typeob.UIntPtr
                    });
                    if (constructor == null)
                    {
                        constructor = rtype.GetConstructor(new Type[]
                        {
                            Typeob.Object,
                            Typeob.IntPtr
                        });
                    }
                    il.Emit(OpCodes.Newobj, constructor);
                    return;
                }
                if (this.member is JSExpandoIndexerMethod)
                {
                    MemberInfo memberInfo = this.member;
                    this.member = this.defaultMember;
                    this.TranslateToIL(il, Typeob.Object);
                    this.member = memberInfo;
                    return;
                }
                il.Emit(OpCodes.Ldnull);
                Convert.Emit(this, il, Typeob.Object, rtype);
                return;
            }
        }

        internal override void TranslateToILCall(ILGenerator il, Type rtype, ASTList argList, bool construct, bool brackets)
        {
            MemberInfo memberInfo = this.member;
            if (this.defaultMember != null)
            {
                if (this.isArrayConstructor)
                {
                    TypedArray typedArray = (TypedArray)this.defaultMemberReturnIR;
                    Type type = Convert.ToType(typedArray.elementType);
                    int rank = typedArray.rank;
                    if (rank == 1)
                    {
                        argList[0].TranslateToIL(il, Typeob.Int32);
                        il.Emit(OpCodes.Newarr, type);
                    }
                    else
                    {
                        Type type2 = typedArray.ToType();
                        Type[] array = new Type[rank];
                        for (int i = 0; i < rank; i++)
                        {
                            array[i] = Typeob.Int32;
                        }
                        int j = 0;
                        int count = argList.count;
                        while (j < count)
                        {
                            argList[j].TranslateToIL(il, Typeob.Int32);
                            j++;
                        }
                        TypeBuilder typeBuilder = type as TypeBuilder;
                        if (typeBuilder != null)
                        {
                            MethodInfo arrayMethod = ((ModuleBuilder)type2.Module).GetArrayMethod(type2, ".ctor", CallingConventions.HasThis, Typeob.Void, array);
                            il.Emit(OpCodes.Newobj, arrayMethod);
                        }
                        else
                        {
                            ConstructorInfo constructor = type2.GetConstructor(array);
                            il.Emit(OpCodes.Newobj, constructor);
                        }
                    }
                    Convert.Emit(this, il, typedArray.ToType(), rtype);
                    return;
                }
                this.member = this.defaultMember;
                IReflect reflect = this.defaultMemberReturnIR;
                Type type3 = (reflect is Type) ? ((Type)reflect) : Convert.ToType(reflect);
                this.TranslateToIL(il, type3);
                if (this.isArrayElementAccess)
                {
                    int k = 0;
                    int count2 = argList.count;
                    while (k < count2)
                    {
                        argList[k].TranslateToIL(il, Typeob.Int32);
                        k++;
                    }
                    Type elementType = type3.GetElementType();
                    int arrayRank = type3.GetArrayRank();
                    if (arrayRank == 1)
                    {
                        Binding.TranslateToLdelem(il, elementType);
                    }
                    else
                    {
                        Type[] array2 = new Type[arrayRank];
                        for (int l = 0; l < arrayRank; l++)
                        {
                            array2[l] = Typeob.Int32;
                        }
                        MethodInfo arrayMethod2 = base.compilerGlobals.module.GetArrayMethod(type3, "Get", CallingConventions.HasThis, elementType, array2);
                        il.Emit(OpCodes.Call, arrayMethod2);
                    }
                    Convert.Emit(this, il, elementType, rtype);
                    return;
                }
                this.member = memberInfo;
            }
            if (memberInfo is MethodInfo)
            {
                MethodInfo methodInfo = (MethodInfo)memberInfo;
                Type declaringType = methodInfo.DeclaringType;
                Type reflectedType = methodInfo.ReflectedType;
                ParameterInfo[] parameters = methodInfo.GetParameters();
                if (!methodInfo.IsStatic && this.defaultMember == null)
                {
                    this.TranslateToILObject(il, declaringType, true);
                }
                if (methodInfo is JSClosureMethod)
                {
                    this.TranslateToILObject(il, declaringType, false);
                }
                int offset = 0;
                ConstantWrapper constantWrapper;
                if (methodInfo is JSFieldMethod || CustomAttribute.IsDefined(methodInfo, typeof(JSFunctionAttribute), false))
                {
                    offset = this.PlaceValuesForHiddenParametersOnStack(il, methodInfo, parameters);
                    constantWrapper = Binding.JScriptMissingCW;
                }
                else
                {
                    constantWrapper = Binding.ReflectionMissingCW;
                }
                if (argList.count == 1 && constantWrapper == Binding.JScriptMissingCW && this.defaultMember is PropertyInfo)
                {
                    il.Emit(OpCodes.Ldc_I4_1);
                    il.Emit(OpCodes.Newarr, Typeob.Object);
                    il.Emit(OpCodes.Dup);
                    il.Emit(OpCodes.Ldc_I4_0);
                    argList[0].TranslateToIL(il, Typeob.Object);
                    il.Emit(OpCodes.Stelem_Ref);
                }
                else
                {
                    Binding.PlaceArgumentsOnStack(il, parameters, argList, offset, 0, constantWrapper);
                }
                methodInfo = this.GetMethodInfoMetadata(methodInfo);
                if (!this.isNonVirtual && methodInfo.IsVirtual && !methodInfo.IsFinal && (!reflectedType.IsSealed || !reflectedType.IsValueType))
                {
                    il.Emit(OpCodes.Callvirt, methodInfo);
                }
                else
                {
                    il.Emit(OpCodes.Call, methodInfo);
                }
                Convert.Emit(this, il, methodInfo.ReturnType, rtype);
                return;
            }
            if (memberInfo is ConstructorInfo)
            {
                ConstructorInfo constructorInfo = (ConstructorInfo)memberInfo;
                ParameterInfo[] parameters2 = constructorInfo.GetParameters();
                bool flag = false;
                if (CustomAttribute.IsDefined(constructorInfo, typeof(JSFunctionAttribute), false))
                {
                    object[] customAttributes = CustomAttribute.GetCustomAttributes(constructorInfo, typeof(JSFunctionAttribute), false);
                    flag = ((((JSFunctionAttribute)customAttributes[0]).attributeValue & JSFunctionAttributeEnum.IsInstanceNestedClassConstructor) != JSFunctionAttributeEnum.None);
                }
                if (flag)
                {
                    Binding.PlaceArgumentsOnStack(il, parameters2, argList, 0, 1, Binding.ReflectionMissingCW);
                    this.TranslateToILObject(il, parameters2[parameters2.Length - 1].ParameterType, false);
                }
                else
                {
                    Binding.PlaceArgumentsOnStack(il, parameters2, argList, 0, 0, Binding.ReflectionMissingCW);
                }
                Type obtype;
                if (memberInfo is JSConstructor && (obtype = ((JSConstructor)memberInfo).OuterClassType()) != null)
                {
                    this.TranslateToILObject(il, obtype, false);
                }
                Type declaringType2 = constructorInfo.DeclaringType;
                bool flag2;
                if (constructorInfo is JSConstructor)
                {
                    constructorInfo = ((JSConstructor)constructorInfo).GetConstructorInfo(base.compilerGlobals);
                    flag2 = true;
                }
                else
                {
                    flag2 = Typeob.INeedEngine.IsAssignableFrom(declaringType2);
                }
                il.Emit(OpCodes.Newobj, constructorInfo);
                if (flag2)
                {
                    il.Emit(OpCodes.Dup);
                    base.EmitILToLoadEngine(il);
                    il.Emit(OpCodes.Callvirt, CompilerGlobals.setEngineMethod);
                }
                Convert.Emit(this, il, declaringType2, rtype);
                return;
            }
            Type type4 = memberInfo as Type;
            if (type4 != null)
            {
                AST aST = argList[0];
                if (aST is NullLiteral)
                {
                    aST.TranslateToIL(il, type4);
                    Convert.Emit(this, il, type4, rtype);
                    return;
                }
                IReflect reflect2 = aST.InferType(null);
                if ((Type)reflect2 == Typeob.ScriptFunction && Typeob.Delegate.IsAssignableFrom(type4))
                {
                    aST.TranslateToIL(il, type4);
                }
                else
                {
                    Type type5 = Convert.ToType(reflect2);
                    aST.TranslateToIL(il, type5);
                    Convert.Emit(this, il, type5, type4, true);
                }
                Convert.Emit(this, il, type4, rtype);
                return;
            }
            else
            {
                if (memberInfo is FieldInfo && ((FieldInfo)memberInfo).IsLiteral)
                {
                    object obj = (memberInfo is JSVariableField) ? ((JSVariableField)memberInfo).value : TypeReferences.GetConstantValue((FieldInfo)memberInfo);
                    if (obj is Type || obj is ClassScope || obj is TypedArray)
                    {
                        AST aST2 = argList[0];
                        if (aST2 is NullLiteral)
                        {
                            il.Emit(OpCodes.Ldnull);
                            return;
                        }
                        ClassScope classScope = obj as ClassScope;
                        if (classScope != null)
                        {
                            EnumDeclaration enumDeclaration = classScope.owner as EnumDeclaration;
                            if (enumDeclaration != null)
                            {
                                obj = enumDeclaration.baseType.ToType();
                            }
                        }
                        Type type6 = Convert.ToType(aST2.InferType(null));
                        aST2.TranslateToIL(il, type6);
                        Type type7 = (obj is Type) ? ((Type)obj) : ((obj is ClassScope) ? Convert.ToType((ClassScope)obj) : ((TypedArray)obj).ToType());
                        Convert.Emit(this, il, type6, type7, true);
                        if (!rtype.IsEnum)
                        {
                            Convert.Emit(this, il, type7, rtype);
                        }
                        return;
                    }
                }
                LocalBuilder localBuilder = null;
                int m = 0;
                int count3 = argList.count;
                while (m < count3)
                {
                    if (argList[m] is AddressOf)
                    {
                        localBuilder = il.DeclareLocal(Typeob.ArrayOfObject);
                        break;
                    }
                    m++;
                }
                object obj2 = null;
                if (memberInfo == null && (this.members == null || this.members.Length == 0))
                {
                    if (this is Lookup)
                    {
                        ((Lookup)this).TranslateToLateBinding(il);
                    }
                    else
                    {
                        obj2 = this.TranslateToSpeculativeEarlyBoundCalls(il, rtype, argList, construct, brackets);
                        ((Member)this).TranslateToLateBinding(il, obj2 != null);
                    }
                    argList.TranslateToIL(il, Typeob.ArrayOfObject);
                    if (localBuilder != null)
                    {
                        il.Emit(OpCodes.Dup);
                        il.Emit(OpCodes.Stloc, localBuilder);
                    }
                    if (construct)
                    {
                        il.Emit(OpCodes.Ldc_I4_1);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4_0);
                    }
                    if (brackets)
                    {
                        il.Emit(OpCodes.Ldc_I4_1);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4_0);
                    }
                    base.EmitILToLoadEngine(il);
                    il.Emit(OpCodes.Call, CompilerGlobals.callMethod);
                    Convert.Emit(this, il, Typeob.Object, rtype);
                    if (localBuilder != null)
                    {
                        int n = 0;
                        int count4 = argList.count;
                        while (n < count4)
                        {
                            AddressOf addressOf = argList[n] as AddressOf;
                            if (addressOf != null)
                            {
                                addressOf.TranslateToILPreSet(il);
                                il.Emit(OpCodes.Ldloc, localBuilder);
                                ConstantWrapper.TranslateToILInt(il, n);
                                il.Emit(OpCodes.Ldelem_Ref);
                                Convert.Emit(this, il, Typeob.Object, Convert.ToType(addressOf.InferType(null)));
                                addressOf.TranslateToILSet(il, null);
                            }
                            n++;
                        }
                    }
                    if (obj2 != null)
                    {
                        il.MarkLabel((Label)obj2);
                    }
                    return;
                }
                this.TranslateToILWithDupOfThisOb(il);
                argList.TranslateToIL(il, Typeob.ArrayOfObject);
                if (localBuilder != null)
                {
                    il.Emit(OpCodes.Dup);
                    il.Emit(OpCodes.Stloc, localBuilder);
                }
                if (construct)
                {
                    il.Emit(OpCodes.Ldc_I4_1);
                }
                else
                {
                    il.Emit(OpCodes.Ldc_I4_0);
                }
                if (brackets)
                {
                    il.Emit(OpCodes.Ldc_I4_1);
                }
                else
                {
                    il.Emit(OpCodes.Ldc_I4_0);
                }
                base.EmitILToLoadEngine(il);
                il.Emit(OpCodes.Call, CompilerGlobals.callValueMethod);
                Convert.Emit(this, il, Typeob.Object, rtype);
                if (localBuilder != null)
                {
                    int num = 0;
                    int count5 = argList.count;
                    while (num < count5)
                    {
                        AddressOf addressOf2 = argList[num] as AddressOf;
                        if (addressOf2 != null)
                        {
                            addressOf2.TranslateToILPreSet(il);
                            il.Emit(OpCodes.Ldloc, localBuilder);
                            ConstantWrapper.TranslateToILInt(il, num);
                            il.Emit(OpCodes.Ldelem_Ref);
                            Convert.Emit(this, il, Typeob.Object, Convert.ToType(addressOf2.InferType(null)));
                            addressOf2.TranslateToILSet(il, null);
                        }
                        num++;
                    }
                }
                return;
            }
        }

        internal override void TranslateToILDelete(ILGenerator il, Type rtype)
        {
            if (this is Lookup)
            {
                ((Lookup)this).TranslateToLateBinding(il);
            }
            else
            {
                ((Member)this).TranslateToLateBinding(il, false);
            }
            il.Emit(OpCodes.Call, CompilerGlobals.deleteMethod);
            Convert.Emit(this, il, Typeob.Boolean, rtype);
        }

        protected abstract void TranslateToILObject(ILGenerator il, Type obtype, bool noValue);

        internal override void TranslateToILPreSet(ILGenerator il)
        {
            this.TranslateToIL(il, null, true, false);
        }

        internal override void TranslateToILPreSet(ILGenerator il, ASTList argList)
        {
            if (this.isArrayElementAccess)
            {
                this.member = this.defaultMember;
                IReflect reflect = this.defaultMemberReturnIR;
                Type type = (reflect is Type) ? ((Type)reflect) : Convert.ToType(reflect);
                this.TranslateToIL(il, type);
                int i = 0;
                int count = argList.count;
                while (i < count)
                {
                    argList[i].TranslateToIL(il, Typeob.Int32);
                    i++;
                }
                if (type.GetArrayRank() == 1)
                {
                    Type elementType = type.GetElementType();
                    if (elementType.IsValueType && !elementType.IsPrimitive && !elementType.IsEnum)
                    {
                        il.Emit(OpCodes.Ldelema, elementType);
                    }
                }
                return;
            }
            if (this.member is PropertyInfo && this.defaultMember != null)
            {
                PropertyInfo propertyInfo = (PropertyInfo)this.member;
                this.member = this.defaultMember;
                this.TranslateToIL(il, Convert.ToType(this.defaultMemberReturnIR));
                this.member = propertyInfo;
                Binding.PlaceArgumentsOnStack(il, propertyInfo.GetIndexParameters(), argList, 0, 0, Binding.ReflectionMissingCW);
                return;
            }
            base.TranslateToILPreSet(il, argList);
        }

        internal override void TranslateToILPreSetPlusGet(ILGenerator il)
        {
            this.TranslateToIL(il, Convert.ToType(this.InferType(null)), false, true);
        }

        internal override void TranslateToILPreSetPlusGet(ILGenerator il, ASTList argList, bool inBrackets)
        {
            if (this.isArrayElementAccess)
            {
                this.member = this.defaultMember;
                IReflect reflect = this.defaultMemberReturnIR;
                Type type = (reflect is Type) ? ((Type)reflect) : Convert.ToType(reflect);
                this.TranslateToIL(il, type);
                il.Emit(OpCodes.Dup);
                int arrayRank = type.GetArrayRank();
                LocalBuilder[] array = new LocalBuilder[arrayRank];
                int i = 0;
                int count = argList.count;
                while (i < count)
                {
                    argList[i].TranslateToIL(il, Typeob.Int32);
                    array[i] = il.DeclareLocal(Typeob.Int32);
                    il.Emit(OpCodes.Dup);
                    il.Emit(OpCodes.Stloc, array[i]);
                    i++;
                }
                Type elementType = type.GetElementType();
                if (arrayRank == 1)
                {
                    Binding.TranslateToLdelem(il, elementType);
                }
                else
                {
                    Type[] array2 = new Type[arrayRank];
                    for (int j = 0; j < arrayRank; j++)
                    {
                        array2[j] = Typeob.Int32;
                    }
                    MethodInfo method = type.GetMethod("Get", array2);
                    il.Emit(OpCodes.Call, method);
                }
                LocalBuilder local = il.DeclareLocal(elementType);
                il.Emit(OpCodes.Stloc, local);
                for (int k = 0; k < arrayRank; k++)
                {
                    il.Emit(OpCodes.Ldloc, array[k]);
                }
                if (arrayRank == 1 && elementType.IsValueType && !elementType.IsPrimitive)
                {
                    il.Emit(OpCodes.Ldelema, elementType);
                }
                il.Emit(OpCodes.Ldloc, local);
                return;
            }
            if (this.member != null && this.defaultMember != null)
            {
                this.member = this.defaultMember;
                this.defaultMember = null;
            }
            base.TranslateToILPreSetPlusGet(il, argList, inBrackets);
        }

        internal override object TranslateToILReference(ILGenerator il, Type rtype)
        {
            if (this.member is FieldInfo)
            {
                FieldInfo fieldInfo = (FieldInfo)this.member;
                Type fieldType = fieldInfo.FieldType;
                if (rtype == fieldType)
                {
                    bool isStatic = fieldInfo.IsStatic;
                    if (!isStatic)
                    {
                        this.TranslateToILObject(il, fieldInfo.DeclaringType, true);
                    }
                    object obj = (fieldInfo is JSField) ? ((JSField)fieldInfo).GetMetaData() : ((fieldInfo is JSFieldInfo) ? ((JSFieldInfo)fieldInfo).field : fieldInfo);
                    if (obj is FieldInfo)
                    {
                        if (fieldInfo.IsInitOnly)
                        {
                            LocalBuilder local = il.DeclareLocal(fieldType);
                            il.Emit(isStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, (FieldInfo)obj);
                            il.Emit(OpCodes.Stloc, local);
                            il.Emit(OpCodes.Ldloca, local);
                        }
                        else
                        {
                            il.Emit(isStatic ? OpCodes.Ldsflda : OpCodes.Ldflda, (FieldInfo)obj);
                        }
                    }
                    else if (obj is LocalBuilder)
                    {
                        il.Emit(OpCodes.Ldloca, (LocalBuilder)obj);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldarga, (short)obj);
                    }
                    return null;
                }
            }
            return base.TranslateToILReference(il, rtype);
        }

        internal override void TranslateToILSet(ILGenerator il, AST rhvalue)
        {
            if (this.isArrayElementAccess)
            {
                IReflect reflect = this.defaultMemberReturnIR;
                Type type = (reflect is Type) ? ((Type)reflect) : Convert.ToType(reflect);
                int arrayRank = type.GetArrayRank();
                Type elementType = type.GetElementType();
                if (rhvalue != null)
                {
                    rhvalue.TranslateToIL(il, elementType);
                }
                if (arrayRank == 1)
                {
                    Binding.TranslateToStelem(il, elementType);
                    return;
                }
                Type[] array = new Type[arrayRank + 1];
                for (int i = 0; i < arrayRank; i++)
                {
                    array[i] = Typeob.Int32;
                }
                array[arrayRank] = elementType;
                MethodInfo arrayMethod = base.compilerGlobals.module.GetArrayMethod(type, "Set", CallingConventions.HasThis, Typeob.Void, array);
                il.Emit(OpCodes.Call, arrayMethod);
                return;
            }
            else
            {
                if (this.isAssignmentToDefaultIndexedProperty)
                {
                    if (this.member is PropertyInfo && this.defaultMember != null)
                    {
                        PropertyInfo propertyInfo = (PropertyInfo)this.member;
                        MethodInfo methodInfo = JSProperty.GetSetMethod(propertyInfo, false);
                        JSWrappedMethod jSWrappedMethod = methodInfo as JSWrappedMethod;
                        if (jSWrappedMethod == null || !(jSWrappedMethod.GetWrappedObject() is GlobalObject))
                        {
                            methodInfo = this.GetMethodInfoMetadata(methodInfo);
                            if (rhvalue != null)
                            {
                                rhvalue.TranslateToIL(il, propertyInfo.PropertyType);
                            }
                            if (methodInfo.IsVirtual && !methodInfo.IsFinal && (!methodInfo.ReflectedType.IsSealed || !methodInfo.ReflectedType.IsValueType))
                            {
                                il.Emit(OpCodes.Callvirt, methodInfo);
                                return;
                            }
                            il.Emit(OpCodes.Call, methodInfo);
                            return;
                        }
                    }
                    base.TranslateToILSet(il, rhvalue);
                    return;
                }
                if (this.member is FieldInfo)
                {
                    FieldInfo fieldInfo = (FieldInfo)this.member;
                    if (rhvalue != null)
                    {
                        rhvalue.TranslateToIL(il, fieldInfo.FieldType);
                    }
                    if (fieldInfo.IsLiteral || fieldInfo.IsInitOnly)
                    {
                        il.Emit(OpCodes.Pop);
                        return;
                    }
                    object obj = (fieldInfo is JSField) ? ((JSField)fieldInfo).GetMetaData() : ((fieldInfo is JSFieldInfo) ? ((JSFieldInfo)fieldInfo).field : fieldInfo);
                    FieldInfo fieldInfo2 = obj as FieldInfo;
                    if (fieldInfo2 != null)
                    {
                        il.Emit(fieldInfo2.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, fieldInfo2);
                        return;
                    }
                    if (obj is LocalBuilder)
                    {
                        il.Emit(OpCodes.Stloc, (LocalBuilder)obj);
                        return;
                    }
                    il.Emit(OpCodes.Starg, (short)obj);
                    return;
                }
                else
                {
                    if (!(this.member is PropertyInfo))
                    {
                        object obj2 = this.TranslateToSpeculativeEarlyBoundSet(il, rhvalue);
                        if (rhvalue != null)
                        {
                            rhvalue.TranslateToIL(il, Typeob.Object);
                        }
                        il.Emit(OpCodes.Call, CompilerGlobals.setValueMethod);
                        if (obj2 != null)
                        {
                            il.MarkLabel((Label)obj2);
                        }
                        return;
                    }
                    PropertyInfo propertyInfo2 = (PropertyInfo)this.member;
                    if (rhvalue != null)
                    {
                        rhvalue.TranslateToIL(il, propertyInfo2.PropertyType);
                    }
                    MethodInfo methodInfo2 = JSProperty.GetSetMethod(propertyInfo2, true);
                    if (methodInfo2 == null)
                    {
                        il.Emit(OpCodes.Pop);
                        return;
                    }
                    methodInfo2 = this.GetMethodInfoMetadata(methodInfo2);
                    if (methodInfo2.IsStatic && !(methodInfo2 is JSClosureMethod))
                    {
                        il.Emit(OpCodes.Call, methodInfo2);
                        return;
                    }
                    if (!this.isNonVirtual && methodInfo2.IsVirtual && !methodInfo2.IsFinal && (!methodInfo2.ReflectedType.IsSealed || !methodInfo2.ReflectedType.IsValueType))
                    {
                        il.Emit(OpCodes.Callvirt, methodInfo2);
                        return;
                    }
                    il.Emit(OpCodes.Call, methodInfo2);
                    return;
                }
            }
        }

        protected abstract void TranslateToILWithDupOfThisOb(ILGenerator il);

        private static void TranslateToLdelem(ILGenerator il, Type etype)
        {
            switch (Type.GetTypeCode(etype))
            {
                case TypeCode.Object:
                case TypeCode.Decimal:
                case TypeCode.DateTime:
                case TypeCode.String:
                    if (etype.IsValueType)
                    {
                        il.Emit(OpCodes.Ldelema, etype);
                        il.Emit(OpCodes.Ldobj, etype);
                        return;
                    }
                    il.Emit(OpCodes.Ldelem_Ref);
                    break;
                case TypeCode.DBNull:
                case 17:
                    break;
                case TypeCode.Boolean:
                case TypeCode.Byte:
                    il.Emit(OpCodes.Ldelem_U1);
                    return;
                case TypeCode.Char:
                case TypeCode.UInt16:
                    il.Emit(OpCodes.Ldelem_U2);
                    return;
                case TypeCode.SByte:
                    il.Emit(OpCodes.Ldelem_I1);
                    return;
                case TypeCode.Int16:
                    il.Emit(OpCodes.Ldelem_I2);
                    return;
                case TypeCode.Int32:
                    il.Emit(OpCodes.Ldelem_I4);
                    return;
                case TypeCode.UInt32:
                    il.Emit(OpCodes.Ldelem_U4);
                    return;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    il.Emit(OpCodes.Ldelem_I8);
                    return;
                case TypeCode.Single:
                    il.Emit(OpCodes.Ldelem_R4);
                    return;
                case TypeCode.Double:
                    il.Emit(OpCodes.Ldelem_R8);
                    return;
                default:
                    return;
            }
        }

        private object TranslateToSpeculativeEarlyBoundSet(ILGenerator il, AST rhvalue)
        {
            this.giveErrors = false;
            object obj = null;
            bool flag = true;
            LocalBuilder local = null;
            LocalBuilder localBuilder = null;
            Label label = il.DefineLabel();
            MemberInfoList allKnownInstanceBindingsForThisName = this.GetAllKnownInstanceBindingsForThisName();
            int i = 0;
            int count = allKnownInstanceBindingsForThisName.count;
            while (i < count)
            {
                MemberInfo memberInfo = allKnownInstanceBindingsForThisName[i];
                FieldInfo fieldInfo = null;
                MethodInfo methodInfo = null;
                PropertyInfo propertyInfo = null;
                if (memberInfo is FieldInfo)
                {
                    fieldInfo = (FieldInfo)memberInfo;
                    if (!fieldInfo.IsLiteral)
                    {
                        if (!fieldInfo.IsInitOnly)
                        {
                            goto IL_A4;
                        }
                    }
                }
                else if (memberInfo is PropertyInfo)
                {
                    propertyInfo = (PropertyInfo)memberInfo;
                    if (propertyInfo.GetIndexParameters().Length <= 0 && (methodInfo = JSProperty.GetSetMethod(propertyInfo, true)) != null)
                    {
                        goto IL_A4;
                    }
                }
                IL_2A8:
                i++;
                continue;
                IL_A4:
                this.member = memberInfo;
                if (this.Accessible(true))
                {
                    if (flag)
                    {
                        flag = false;
                        if (rhvalue == null)
                        {
                            localBuilder = il.DeclareLocal(Typeob.Object);
                            il.Emit(OpCodes.Stloc, localBuilder);
                        }
                        il.Emit(OpCodes.Dup);
                        il.Emit(OpCodes.Ldfld, CompilerGlobals.objectField);
                        local = il.DeclareLocal(Typeob.Object);
                        il.Emit(OpCodes.Stloc, local);
                        obj = il.DefineLabel();
                    }
                    Type declaringType = memberInfo.DeclaringType;
                    il.Emit(OpCodes.Ldloc, local);
                    il.Emit(OpCodes.Isinst, declaringType);
                    LocalBuilder local2 = il.DeclareLocal(declaringType);
                    il.Emit(OpCodes.Dup);
                    il.Emit(OpCodes.Stloc, local2);
                    il.Emit(OpCodes.Brfalse, label);
                    il.Emit(OpCodes.Ldloc, local2);
                    if (rhvalue == null)
                    {
                        il.Emit(OpCodes.Ldloc, localBuilder);
                    }
                    if (fieldInfo != null)
                    {
                        if (rhvalue == null)
                        {
                            Convert.Emit(this, il, Typeob.Object, fieldInfo.FieldType);
                        }
                        else
                        {
                            rhvalue.TranslateToIL(il, fieldInfo.FieldType);
                        }
                        if (fieldInfo is JSField)
                        {
                            il.Emit(OpCodes.Stfld, (FieldInfo)((JSField)fieldInfo).GetMetaData());
                        }
                        else if (fieldInfo is JSFieldInfo)
                        {
                            il.Emit(OpCodes.Stfld, ((JSFieldInfo)fieldInfo).field);
                        }
                        else
                        {
                            il.Emit(OpCodes.Stfld, fieldInfo);
                        }
                    }
                    else
                    {
                        if (rhvalue == null)
                        {
                            Convert.Emit(this, il, Typeob.Object, propertyInfo.PropertyType);
                        }
                        else
                        {
                            rhvalue.TranslateToIL(il, propertyInfo.PropertyType);
                        }
                        methodInfo = this.GetMethodInfoMetadata(methodInfo);
                        if (methodInfo.IsVirtual && !methodInfo.IsFinal && (!declaringType.IsSealed || !declaringType.IsValueType))
                        {
                            il.Emit(OpCodes.Callvirt, methodInfo);
                        }
                        else
                        {
                            il.Emit(OpCodes.Call, methodInfo);
                        }
                    }
                    il.Emit(OpCodes.Pop);
                    il.Emit(OpCodes.Br, (Label)obj);
                    il.MarkLabel(label);
                    label = il.DefineLabel();
                    goto IL_2A8;
                }
                goto IL_2A8;
            }
            if (localBuilder != null)
            {
                il.Emit(OpCodes.Ldloc, localBuilder);
            }
            this.member = null;
            return obj;
        }

        private object TranslateToSpeculativeEarlyBindings(ILGenerator il, Type rtype, bool getObjectFromLateBindingInstance)
        {
            this.giveErrors = false;
            object obj = null;
            bool flag = true;
            LocalBuilder local = null;
            Label label = il.DefineLabel();
            MemberInfoList allKnownInstanceBindingsForThisName = this.GetAllKnownInstanceBindingsForThisName();
            int i = 0;
            int count = allKnownInstanceBindingsForThisName.count;
            while (i < count)
            {
                MemberInfo memberInfo = allKnownInstanceBindingsForThisName[i];
                if (memberInfo is FieldInfo || (memberInfo is PropertyInfo && ((PropertyInfo)memberInfo).GetIndexParameters().Length <= 0 && JSProperty.GetGetMethod((PropertyInfo)memberInfo, true) != null))
                {
                    this.member = memberInfo;
                    if (this.Accessible(false))
                    {
                        if (flag)
                        {
                            flag = false;
                            if (getObjectFromLateBindingInstance)
                            {
                                il.Emit(OpCodes.Dup);
                                il.Emit(OpCodes.Ldfld, CompilerGlobals.objectField);
                            }
                            else
                            {
                                this.TranslateToILObject(il, Typeob.Object, false);
                            }
                            local = il.DeclareLocal(Typeob.Object);
                            il.Emit(OpCodes.Stloc, local);
                            obj = il.DefineLabel();
                        }
                        Type declaringType = memberInfo.DeclaringType;
                        il.Emit(OpCodes.Ldloc, local);
                        il.Emit(OpCodes.Isinst, declaringType);
                        LocalBuilder local2 = il.DeclareLocal(declaringType);
                        il.Emit(OpCodes.Dup);
                        il.Emit(OpCodes.Stloc, local2);
                        il.Emit(OpCodes.Brfalse_S, label);
                        il.Emit(OpCodes.Ldloc, local2);
                        if (memberInfo is FieldInfo)
                        {
                            FieldInfo fieldInfo = (FieldInfo)memberInfo;
                            if (fieldInfo.IsLiteral)
                            {
                                il.Emit(OpCodes.Pop);
                                goto IL_25F;
                            }
                            if (fieldInfo is JSField)
                            {
                                il.Emit(OpCodes.Ldfld, (FieldInfo)((JSField)fieldInfo).GetMetaData());
                            }
                            else if (fieldInfo is JSFieldInfo)
                            {
                                il.Emit(OpCodes.Ldfld, ((JSFieldInfo)fieldInfo).field);
                            }
                            else
                            {
                                il.Emit(OpCodes.Ldfld, fieldInfo);
                            }
                            Convert.Emit(this, il, fieldInfo.FieldType, rtype);
                        }
                        else if (memberInfo is PropertyInfo)
                        {
                            MethodInfo methodInfo = JSProperty.GetGetMethod((PropertyInfo)memberInfo, true);
                            methodInfo = this.GetMethodInfoMetadata(methodInfo);
                            if (methodInfo.IsVirtual && !methodInfo.IsFinal && (!declaringType.IsSealed || declaringType.IsValueType))
                            {
                                il.Emit(OpCodes.Callvirt, methodInfo);
                            }
                            else
                            {
                                il.Emit(OpCodes.Call, methodInfo);
                            }
                            Convert.Emit(this, il, methodInfo.ReturnType, rtype);
                        }
                        il.Emit(OpCodes.Br, (Label)obj);
                        il.MarkLabel(label);
                        label = il.DefineLabel();
                    }
                }
                IL_25F:
                i++;
            }
            il.MarkLabel(label);
            if (!flag && !getObjectFromLateBindingInstance)
            {
                il.Emit(OpCodes.Ldloc, local);
            }
            this.member = null;
            return obj;
        }

        private object TranslateToSpeculativeEarlyBoundCalls(ILGenerator il, Type rtype, ASTList argList, bool construct, bool brackets)
        {
            this.giveErrors = false;
            object obj = null;
            bool flag = true;
            LocalBuilder local = null;
            Label label = il.DefineLabel();
            IReflect[] allEligibleClasses = this.GetAllEligibleClasses();
            if (construct)
            {
                return obj;
            }
            IReflect[] array = allEligibleClasses;
            for (int i = 0; i < array.Length; i++)
            {
                IReflect reflect = array[i];
                MemberInfo[] match = reflect.GetMember(this.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                try
                {
                    MemberInfo memberInfo = JSBinder.SelectCallableMember(match, this.argIRs);
                    MethodInfo methodInfo;
                    if (memberInfo != null && memberInfo.MemberType == MemberTypes.Property)
                    {
                        methodInfo = ((PropertyInfo)memberInfo).GetGetMethod(true);
                        ParameterInfo[] parameters;
                        if (methodInfo == null || (parameters = methodInfo.GetParameters()) == null || parameters.Length == 0)
                        {
                            goto IL_274;
                        }
                    }
                    else
                    {
                        methodInfo = (memberInfo as MethodInfo);
                    }
                    if (methodInfo != null)
                    {
                        if (Binding.CheckParameters(methodInfo.GetParameters(), this.argIRs, argList, this.context, 0, true, false))
                        {
                            if (methodInfo is JSFieldMethod)
                            {
                                FunctionObject func = ((JSFieldMethod)methodInfo).func;
                                if (func != null && (func.attributes & MethodAttributes.VtableLayoutMask) == MethodAttributes.PrivateScope && ((ClassScope)reflect).ParentIsInSamePackage())
                                {
                                    goto IL_274;
                                }
                            }
                            else if (methodInfo is JSWrappedMethod && ((JSWrappedMethod)methodInfo).obj is ClassScope && ((JSWrappedMethod)methodInfo).GetPackage() == ((ClassScope)reflect).package)
                            {
                                goto IL_274;
                            }
                            this.member = methodInfo;
                            if (this.Accessible(false))
                            {
                                if (flag)
                                {
                                    flag = false;
                                    this.TranslateToILObject(il, Typeob.Object, false);
                                    local = il.DeclareLocal(Typeob.Object);
                                    il.Emit(OpCodes.Stloc, local);
                                    obj = il.DefineLabel();
                                }
                                Type declaringType = methodInfo.DeclaringType;
                                il.Emit(OpCodes.Ldloc, local);
                                il.Emit(OpCodes.Isinst, declaringType);
                                LocalBuilder local2 = il.DeclareLocal(declaringType);
                                il.Emit(OpCodes.Dup);
                                il.Emit(OpCodes.Stloc, local2);
                                il.Emit(OpCodes.Brfalse, label);
                                il.Emit(OpCodes.Ldloc, local2);
                                Binding.PlaceArgumentsOnStack(il, methodInfo.GetParameters(), argList, 0, 0, Binding.ReflectionMissingCW);
                                methodInfo = this.GetMethodInfoMetadata(methodInfo);
                                if (methodInfo.IsVirtual && !methodInfo.IsFinal && (!declaringType.IsSealed || declaringType.IsValueType))
                                {
                                    il.Emit(OpCodes.Callvirt, methodInfo);
                                }
                                else
                                {
                                    il.Emit(OpCodes.Call, methodInfo);
                                }
                                Convert.Emit(this, il, methodInfo.ReturnType, rtype);
                                il.Emit(OpCodes.Br, (Label)obj);
                                il.MarkLabel(label);
                                label = il.DefineLabel();
                            }
                        }
                    }
                }
                catch (AmbiguousMatchException)
                {
                }
                IL_274:;
            }
            il.MarkLabel(label);
            if (!flag)
            {
                il.Emit(OpCodes.Ldloc, local);
            }
            this.member = null;
            return obj;
        }

        internal static void TranslateToStelem(ILGenerator il, Type etype)
        {
            switch (Type.GetTypeCode(etype))
            {
                case (System.TypeCode)TypeCode.Object:
                case TypeCode.Decimal:
                case TypeCode.DateTime:
                case TypeCode.String:
                    if (etype.IsValueType)
                    {
                        il.Emit(OpCodes.Stobj, etype);
                        return;
                    }
                    il.Emit(OpCodes.Stelem_Ref);
                    break;
                case TypeCode.DBNull:
                case (TypeCode)17:
                    break;
                case TypeCode.Boolean:
                case TypeCode.SByte:
                case TypeCode.Byte:
                    il.Emit(OpCodes.Stelem_I1);
                    return;
                case TypeCode.Char:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    il.Emit(OpCodes.Stelem_I2);
                    return;
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    il.Emit(OpCodes.Stelem_I4);
                    return;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    il.Emit(OpCodes.Stelem_I8);
                    return;
                case TypeCode.Single:
                    il.Emit(OpCodes.Stelem_R4);
                    return;
                case TypeCode.Double:
                    il.Emit(OpCodes.Stelem_R8);
                    return;
                default:
                    return;
            }
        }

        private void WarnIfNotFullyResolved()
        {
            if (this.isFullyResolved || this.member == null)
            {
                return;
            }
            if (this.member is JSVariableField && ((JSVariableField)this.member).type == null)
            {
                return;
            }
            if (!base.Engine.doFast && this.member is IWrappedMember)
            {
                return;
            }
            for (ScriptObject scriptObject = base.Globals.ScopeStack.Peek(); scriptObject != null; scriptObject = scriptObject.GetParent())
            {
                if (scriptObject is WithObject && !((WithObject)scriptObject).isKnownAtCompileTime)
                {
                    this.context.HandleError(JSError.AmbiguousBindingBecauseOfWith);
                    return;
                }
                if (scriptObject is ActivationObject && !((ActivationObject)scriptObject).isKnownAtCompileTime)
                {
                    this.context.HandleError(JSError.AmbiguousBindingBecauseOfEval);
                    return;
                }
            }
        }

        private void WarnIfObsolete()
        {
            Binding.WarnIfObsolete(this.member, this.context);
        }

        internal static void WarnIfObsolete(MemberInfo member, Context context)
        {
            if (member == null)
            {
                return;
            }
            object[] customAttributes = CustomAttribute.GetCustomAttributes(member, typeof(ObsoleteAttribute), false);
            string message;
            bool treatAsError;
            if (customAttributes != null && customAttributes.Length > 0)
            {
                ObsoleteAttribute obsoleteAttribute = (ObsoleteAttribute)customAttributes[0];
                message = obsoleteAttribute.Message;
                treatAsError = obsoleteAttribute.IsError;
            }
            else
            {
                customAttributes = CustomAttribute.GetCustomAttributes(member, typeof(NotRecommended), false);
                if (customAttributes == null || customAttributes.Length <= 0)
                {
                    return;
                }
                NotRecommended notRecommended = (NotRecommended)customAttributes[0];
                message = ": " + notRecommended.Message;
                treatAsError = false;
            }
            context.HandleError(JSError.Deprecated, message, treatAsError);
        }

        private MethodInfo GetMethodInfoMetadata(MethodInfo method)
        {
            if (method is JSMethod)
            {
                return ((JSMethod)method).GetMethodInfo(base.compilerGlobals);
            }
            if (method is JSMethodInfo)
            {
                return ((JSMethodInfo)method).method;
            }
            return method;
        }
    }
}
