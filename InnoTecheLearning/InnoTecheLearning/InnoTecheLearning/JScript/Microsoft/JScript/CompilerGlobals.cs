namespace Microsoft.JScript
{
    using Microsoft.JScript.Vsa;
    using Microsoft.Vsa;
    using System;
    using System.Configuration.Assemblies;
    using System.IO;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Security.Policy;
    using System.Threading;

    internal sealed class CompilerGlobals
    {
        internal AssemblyBuilder assemblyBuilder;
        internal Stack BreakLabelStack = new Stack();
        internal TypeBuilder classwriter;
        internal Evidence compilationEvidence;
        internal Stack ContinueLabelStack = new Stack();
        internal SimpleHashtable documents = new SimpleHashtable(8);
        internal int FinallyStackTop;
        internal TypeBuilder globalScopeClassWriter;
        internal bool InsideFinally;
        internal bool InsideProtectedRegion;
        internal ModuleBuilder module;
        internal SimpleHashtable usedNames = new SimpleHashtable(0x20);

        internal CompilerGlobals(VsaEngine engine, string assemName, string assemblyFileName, PEFileKinds PEFileKind, bool save, bool run, bool debugOn, bool isCLSCompliant, Version version, Globals globals)
        {
            string fileName = null;
            string dir = null;
            if (assemblyFileName != null)
            {
                try
                {
                    dir = Path.GetDirectoryName(Path.GetFullPath(assemblyFileName));
                }
                catch (Exception exception)
                {
                    throw new VsaException(VsaError.AssemblyNameInvalid, assemblyFileName, exception);
                }
                catch
                {
                    throw new JScriptException(JSError.NonClsException);
                }
                fileName = Path.GetFileName(assemblyFileName);
                if ((assemName == null) || (string.Empty == assemName))
                {
                    assemName = Path.GetFileName(assemblyFileName);
                    if (Path.HasExtension(assemName))
                    {
                        assemName = assemName.Substring(0, assemName.Length - Path.GetExtension(assemName).Length);
                    }
                }
            }
            if ((assemName == null) || (assemName == string.Empty))
            {
                assemName = "JScriptAssembly";
            }
            if (fileName == null)
            {
                if (PEFileKind == PEFileKinds.Dll)
                {
                    fileName = "JScriptModule.dll";
                }
                else
                {
                    fileName = "JScriptModule.exe";
                }
            }
            AssemblyName name = new AssemblyName {
                CodeBase = assemblyFileName
            };
            if (globals.assemblyCulture != null)
            {
                name.CultureInfo = globals.assemblyCulture;
            }
            name.Flags = AssemblyNameFlags.None;
            if ((globals.assemblyFlags & AssemblyFlags.PublicKey) != AssemblyFlags.SideBySideCompatible)
            {
                name.Flags = AssemblyNameFlags.PublicKey;
            }
            AssemblyFlags flags = globals.assemblyFlags & AssemblyFlags.CompatibilityMask;
            if (flags == AssemblyFlags.NonSideBySideAppDomain)
            {
                name.VersionCompatibility = AssemblyVersionCompatibility.SameDomain;
            }
            else if (flags == AssemblyFlags.NonSideBySideProcess)
            {
                name.VersionCompatibility = AssemblyVersionCompatibility.SameProcess;
            }
            else if (flags == AssemblyFlags.NonSideBySideMachine)
            {
                name.VersionCompatibility = AssemblyVersionCompatibility.SameMachine;
            }
            else
            {
                name.VersionCompatibility = (AssemblyVersionCompatibility) 0;
            }
            name.HashAlgorithm = globals.assemblyHashAlgorithm;
            if (globals.assemblyKeyFileName != null)
            {
                try
                {
                    using (FileStream stream = new FileStream(globals.assemblyKeyFileName, FileMode.Open, FileAccess.Read))
                    {
                        StrongNameKeyPair pair = new StrongNameKeyPair(stream);
                        if (globals.assemblyDelaySign)
                        {
                            if (stream.Length == 160L)
                            {
                                byte[] buffer = new byte[160];
                                stream.Seek(0L, SeekOrigin.Begin);
                                stream.Read(buffer, 0, 160);
                                name.SetPublicKey(buffer);
                            }
                            else
                            {
                                name.SetPublicKey(pair.PublicKey);
                            }
                        }
                        else
                        {
                            byte[] publicKey = pair.PublicKey;
                            name.KeyPair = pair;
                        }
                    }
                    goto Label_025A;
                }
                catch
                {
                    globals.assemblyKeyFileNameContext.HandleError(JSError.InvalidAssemblyKeyFile, globals.assemblyKeyFileName);
                    goto Label_025A;
                }
            }
            if (globals.assemblyKeyName != null)
            {
                try
                {
                    StrongNameKeyPair pair2 = new StrongNameKeyPair(globals.assemblyKeyName);
                    byte[] buffer2 = pair2.PublicKey;
                    name.KeyPair = pair2;
                }
                catch
                {
                    globals.assemblyKeyNameContext.HandleError(JSError.InvalidAssemblyKeyFile, globals.assemblyKeyName);
                }
            }
        Label_025A:
            name.Name = assemName;
            if (version != null)
            {
                name.Version = version;
            }
            else if (globals.assemblyVersion != null)
            {
                name.Version = globals.assemblyVersion;
            }
            AssemblyBuilderAccess reflectionOnly = save ? (run ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Save) : AssemblyBuilderAccess.Run;
            if (engine.ReferenceLoaderAPI == LoaderAPI.ReflectionOnlyLoadFrom)
            {
                reflectionOnly = AssemblyBuilderAccess.ReflectionOnly;
            }
            if (globals.engine.genStartupClass)
            {
                this.assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(name, reflectionOnly, dir, globals.engine.Evidence);
            }
            else
            {
                this.assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(name, reflectionOnly, dir);
            }
            if (save)
            {
                this.module = this.assemblyBuilder.DefineDynamicModule("JScript Module", fileName, debugOn);
            }
            else
            {
                this.module = this.assemblyBuilder.DefineDynamicModule("JScript Module", debugOn);
            }
            if (isCLSCompliant)
            {
                this.module.SetCustomAttribute(new CustomAttributeBuilder(clsCompliantAttributeCtor, new object[] { isCLSCompliant }));
            }
            if (debugOn)
            {
                ConstructorInfo constructor = Typeob.DebuggableAttribute.GetConstructor(new Type[] { Typeob.Boolean, Typeob.Boolean });
                this.assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { (globals.assemblyFlags & AssemblyFlags.EnableJITcompileTracking) != AssemblyFlags.SideBySideCompatible, (globals.assemblyFlags & AssemblyFlags.DisableJITcompileOptimizer) != AssemblyFlags.SideBySideCompatible }));
            }
            this.compilationEvidence = globals.engine.Evidence;
            this.classwriter = null;
        }

        internal static ConstructorInfo bitwiseBinaryConstructor =>
            Globals.TypeRefs.bitwiseBinaryConstructor;

        internal static ConstructorInfo breakOutOfFinallyConstructor =>
            Globals.TypeRefs.breakOutOfFinallyConstructor;

        internal static MethodInfo callMethod =>
            Globals.TypeRefs.callMethod;

        internal static MethodInfo callValue2Method =>
            Globals.TypeRefs.callValue2Method;

        internal static MethodInfo callValueMethod =>
            Globals.TypeRefs.callValueMethod;

        internal static MethodInfo changeTypeMethod =>
            Globals.TypeRefs.changeTypeMethod;

        internal static MethodInfo checkIfDoubleIsIntegerMethod =>
            Globals.TypeRefs.checkIfDoubleIsIntegerMethod;

        internal static MethodInfo checkIfSingleIsIntegerMethod =>
            Globals.TypeRefs.checkIfSingleIsIntegerMethod;

        internal static ConstructorInfo closureConstructor =>
            Globals.TypeRefs.closureConstructor;

        internal static FieldInfo closureInstanceField =>
            Globals.TypeRefs.closureInstanceField;

        internal static ConstructorInfo clsCompliantAttributeCtor =>
            Globals.TypeRefs.clsCompliantAttributeCtor;

        internal static MethodInfo coerce2Method =>
            Globals.TypeRefs.coerce2Method;

        internal static MethodInfo coerceTMethod =>
            Globals.TypeRefs.coerceTMethod;

        internal static ConstructorInfo compilerGlobalScopeAttributeCtor =>
            Globals.TypeRefs.compilerGlobalScopeAttributeCtor;

        internal static MethodInfo constructArrayMethod =>
            Globals.TypeRefs.constructArrayMethod;

        internal static MethodInfo constructObjectMethod =>
            Globals.TypeRefs.constructObjectMethod;

        internal static FieldInfo contextEngineField =>
            Globals.TypeRefs.contextEngineField;

        internal static ConstructorInfo contextStaticAttributeCtor =>
            Globals.TypeRefs.contextStaticAttributeCtor;

        internal static ConstructorInfo continueOutOfFinallyConstructor =>
            Globals.TypeRefs.continueOutOfFinallyConstructor;

        internal static MethodInfo convertCharToStringMethod =>
            Globals.TypeRefs.convertCharToStringMethod;

        internal static MethodInfo createVsaEngine =>
            Globals.TypeRefs.createVsaEngine;

        internal static MethodInfo createVsaEngineWithType =>
            Globals.TypeRefs.createVsaEngineWithType;

        internal static ConstructorInfo dateTimeConstructor =>
            Globals.TypeRefs.dateTimeConstructor;

        internal static MethodInfo dateTimeToInt64Method =>
            Globals.TypeRefs.dateTimeToInt64Method;

        internal static MethodInfo dateTimeToStringMethod =>
            Globals.TypeRefs.dateTimeToStringMethod;

        internal static MethodInfo debugBreak =>
            Globals.TypeRefs.debugBreak;

        internal static ConstructorInfo debuggerHiddenAttributeCtor =>
            Globals.TypeRefs.debuggerHiddenAttributeCtor;

        internal static ConstructorInfo debuggerStepThroughAttributeCtor =>
            Globals.TypeRefs.debuggerStepThroughAttributeCtor;

        internal static MethodInfo decimalCompare =>
            Globals.TypeRefs.decimalCompare;

        internal static ConstructorInfo decimalConstructor =>
            Globals.TypeRefs.decimalConstructor;

        internal static MethodInfo decimalToDoubleMethod =>
            Globals.TypeRefs.decimalToDoubleMethod;

        internal static MethodInfo decimalToInt32Method =>
            Globals.TypeRefs.decimalToInt32Method;

        internal static MethodInfo decimalToInt64Method =>
            Globals.TypeRefs.decimalToInt64Method;

        internal static MethodInfo decimalToStringMethod =>
            Globals.TypeRefs.decimalToStringMethod;

        internal static MethodInfo decimalToUInt32Method =>
            Globals.TypeRefs.decimalToUInt32Method;

        internal static MethodInfo decimalToUInt64Method =>
            Globals.TypeRefs.decimalToUInt64Method;

        internal static FieldInfo decimalZeroField =>
            Globals.TypeRefs.decimalZeroField;

        internal static ConstructorInfo defaultMemberAttributeCtor =>
            Globals.TypeRefs.defaultMemberAttributeCtor;

        internal static MethodInfo deleteMemberMethod =>
            Globals.TypeRefs.deleteMemberMethod;

        internal static MethodInfo deleteMethod =>
            Globals.TypeRefs.deleteMethod;

        internal static MethodInfo doubleToBooleanMethod =>
            Globals.TypeRefs.doubleToBooleanMethod;

        internal static MethodInfo doubleToDecimalMethod =>
            Globals.TypeRefs.doubleToDecimalMethod;

        internal static MethodInfo doubleToInt64 =>
            Globals.TypeRefs.doubleToInt64;

        internal static MethodInfo doubleToStringMethod =>
            Globals.TypeRefs.doubleToStringMethod;

        internal static FieldInfo engineField =>
            Globals.TypeRefs.engineField;

        internal static ConstructorInfo equalityConstructor =>
            Globals.TypeRefs.equalityConstructor;

        internal static MethodInfo equalsMethod =>
            Globals.TypeRefs.equalsMethod;

        internal static MethodInfo evaluateBitwiseBinaryMethod =>
            Globals.TypeRefs.evaluateBitwiseBinaryMethod;

        internal static MethodInfo evaluateEqualityMethod =>
            Globals.TypeRefs.evaluateEqualityMethod;

        internal static MethodInfo evaluateNumericBinaryMethod =>
            Globals.TypeRefs.evaluateNumericBinaryMethod;

        internal static MethodInfo evaluatePlusMethod =>
            Globals.TypeRefs.evaluatePlusMethod;

        internal static MethodInfo evaluatePostOrPrefixOperatorMethod =>
            Globals.TypeRefs.evaluatePostOrPrefixOperatorMethod;

        internal static MethodInfo evaluateRelationalMethod =>
            Globals.TypeRefs.evaluateRelationalMethod;

        internal static MethodInfo evaluateUnaryMethod =>
            Globals.TypeRefs.evaluateUnaryMethod;

        internal static MethodInfo fastConstructArrayLiteralMethod =>
            Globals.TypeRefs.fastConstructArrayLiteralMethod;

        internal static MethodInfo getCurrentMethod =>
            Globals.TypeRefs.getCurrentMethod;

        internal static MethodInfo getDefaultThisObjectMethod =>
            Globals.TypeRefs.getDefaultThisObjectMethod;

        internal static MethodInfo getEngineMethod =>
            Globals.TypeRefs.getEngineMethod;

        internal static MethodInfo getEnumeratorMethod =>
            Globals.TypeRefs.getEnumeratorMethod;

        internal static MethodInfo getFieldMethod =>
            Globals.TypeRefs.getFieldMethod;

        internal static MethodInfo getFieldValueMethod =>
            Globals.TypeRefs.getFieldValueMethod;

        internal static MethodInfo getGlobalScopeMethod =>
            Globals.TypeRefs.getGlobalScopeMethod;

        internal static MethodInfo getLenientGlobalObjectMethod =>
            Globals.TypeRefs.getLenientGlobalObjectMethod;

        internal static MethodInfo getMemberValueMethod =>
            Globals.TypeRefs.getMemberValueMethod;

        internal static MethodInfo getMethodMethod =>
            Globals.TypeRefs.getMethodMethod;

        internal static MethodInfo getNamespaceMethod =>
            Globals.TypeRefs.getNamespaceMethod;

        internal static MethodInfo getNonMissingValueMethod =>
            Globals.TypeRefs.getNonMissingValueMethod;

        internal static MethodInfo getOriginalArrayConstructorMethod =>
            Globals.TypeRefs.getOriginalArrayConstructorMethod;

        internal static MethodInfo getOriginalObjectConstructorMethod =>
            Globals.TypeRefs.getOriginalObjectConstructorMethod;

        internal static MethodInfo getOriginalRegExpConstructorMethod =>
            Globals.TypeRefs.getOriginalRegExpConstructorMethod;

        internal static MethodInfo getParentMethod =>
            Globals.TypeRefs.getParentMethod;

        internal static MethodInfo getTypeFromHandleMethod =>
            Globals.TypeRefs.getTypeFromHandleMethod;

        internal static MethodInfo getTypeMethod =>
            Globals.TypeRefs.getTypeMethod;

        internal static MethodInfo getValue2Method =>
            Globals.TypeRefs.getValue2Method;

        internal static ConstructorInfo globalScopeConstructor =>
            Globals.TypeRefs.globalScopeConstructor;

        internal static ConstructorInfo hashtableCtor =>
            Globals.TypeRefs.hashtableCtor;

        internal static MethodInfo hashTableGetEnumerator =>
            Globals.TypeRefs.hashTableGetEnumerator;

        internal static MethodInfo hashtableGetItem =>
            Globals.TypeRefs.hashtableGetItem;

        internal static MethodInfo hashtableRemove =>
            Globals.TypeRefs.hashtableRemove;

        internal static MethodInfo hashtableSetItem =>
            Globals.TypeRefs.hashtableSetItem;

        internal static MethodInfo int32ToDecimalMethod =>
            Globals.TypeRefs.int32ToDecimalMethod;

        internal static MethodInfo int32ToStringMethod =>
            Globals.TypeRefs.int32ToStringMethod;

        internal static MethodInfo int64ToDecimalMethod =>
            Globals.TypeRefs.int64ToDecimalMethod;

        internal static MethodInfo int64ToStringMethod =>
            Globals.TypeRefs.int64ToStringMethod;

        internal static MethodInfo isMissingMethod =>
            Globals.TypeRefs.isMissingMethod;

        internal static MethodInfo jScriptCompareMethod =>
            Globals.TypeRefs.jScriptCompareMethod;

        internal static MethodInfo jScriptEqualsMethod =>
            Globals.TypeRefs.jScriptEqualsMethod;

        internal static MethodInfo jScriptEvaluateMethod1 =>
            Globals.TypeRefs.jScriptEvaluateMethod1;

        internal static MethodInfo jScriptEvaluateMethod2 =>
            Globals.TypeRefs.jScriptEvaluateMethod2;

        internal static MethodInfo jScriptExceptionValueMethod =>
            Globals.TypeRefs.jScriptExceptionValueMethod;

        internal static MethodInfo jScriptFunctionDeclarationMethod =>
            Globals.TypeRefs.jScriptFunctionDeclarationMethod;

        internal static MethodInfo jScriptFunctionExpressionMethod =>
            Globals.TypeRefs.jScriptFunctionExpressionMethod;

        internal static MethodInfo jScriptGetEnumeratorMethod =>
            Globals.TypeRefs.jScriptGetEnumeratorMethod;

        internal static MethodInfo jScriptImportMethod =>
            Globals.TypeRefs.jScriptImportMethod;

        internal static MethodInfo jScriptInMethod =>
            Globals.TypeRefs.jScriptInMethod;

        internal static MethodInfo jScriptInstanceofMethod =>
            Globals.TypeRefs.jScriptInstanceofMethod;

        internal static MethodInfo jScriptPackageMethod =>
            Globals.TypeRefs.jScriptPackageMethod;

        internal static MethodInfo jScriptStrictEqualsMethod =>
            Globals.TypeRefs.jScriptStrictEqualsMethod;

        internal static MethodInfo jScriptThrowMethod =>
            Globals.TypeRefs.jScriptThrowMethod;

        internal static MethodInfo jScriptTypeofMethod =>
            Globals.TypeRefs.jScriptTypeofMethod;

        internal static MethodInfo jScriptWithMethod =>
            Globals.TypeRefs.jScriptWithMethod;

        internal static ConstructorInfo jsFunctionAttributeConstructor =>
            Globals.TypeRefs.jsFunctionAttributeConstructor;

        internal static ConstructorInfo jsLocalFieldConstructor =>
            Globals.TypeRefs.jsLocalFieldConstructor;

        internal static ConstructorInfo lateBindingConstructor =>
            Globals.TypeRefs.lateBindingConstructor;

        internal static ConstructorInfo lateBindingConstructor2 =>
            Globals.TypeRefs.lateBindingConstructor2;

        internal static FieldInfo localVarsField =>
            Globals.TypeRefs.localVarsField;

        internal static FieldInfo missingField =>
            Globals.TypeRefs.missingField;

        internal static MethodInfo moveNextMethod =>
            Globals.TypeRefs.moveNextMethod;

        internal static ConstructorInfo numericBinaryConstructor =>
            Globals.TypeRefs.numericBinaryConstructor;

        internal static MethodInfo numericbinaryDoOpMethod =>
            Globals.TypeRefs.numericbinaryDoOpMethod;

        internal static ConstructorInfo numericUnaryConstructor =>
            Globals.TypeRefs.numericUnaryConstructor;

        internal static FieldInfo objectField =>
            Globals.TypeRefs.objectField;

        internal static ConstructorInfo plusConstructor =>
            Globals.TypeRefs.plusConstructor;

        internal static MethodInfo plusDoOpMethod =>
            Globals.TypeRefs.plusDoOpMethod;

        internal static MethodInfo popScriptObjectMethod =>
            Globals.TypeRefs.popScriptObjectMethod;

        internal static ConstructorInfo postOrPrefixConstructor =>
            Globals.TypeRefs.postOrPrefixConstructor;

        internal static MethodInfo pushScriptObjectMethod =>
            Globals.TypeRefs.pushScriptObjectMethod;

        internal static MethodInfo pushStackFrameForMethod =>
            Globals.TypeRefs.pushStackFrameForMethod;

        internal static MethodInfo pushStackFrameForStaticMethod =>
            Globals.TypeRefs.pushStackFrameForStaticMethod;

        internal static ConstructorInfo referenceAttributeConstructor =>
            Globals.TypeRefs.referenceAttributeConstructor;

        internal static MethodInfo regExpConstructMethod =>
            Globals.TypeRefs.regExpConstructMethod;

        internal static ConstructorInfo relationalConstructor =>
            Globals.TypeRefs.relationalConstructor;

        internal static ConstructorInfo returnOutOfFinallyConstructor =>
            Globals.TypeRefs.returnOutOfFinallyConstructor;

        internal static ConstructorInfo scriptExceptionConstructor =>
            Globals.TypeRefs.scriptExceptionConstructor;

        internal static MethodInfo scriptObjectStackTopMethod =>
            Globals.TypeRefs.scriptObjectStackTopMethod;

        internal static MethodInfo setEngineMethod =>
            Globals.TypeRefs.setEngineMethod;

        internal static MethodInfo setFieldValueMethod =>
            Globals.TypeRefs.setFieldValueMethod;

        internal static MethodInfo setIndexedPropertyValueStaticMethod =>
            Globals.TypeRefs.setIndexedPropertyValueStaticMethod;

        internal static MethodInfo setMemberValue2Method =>
            Globals.TypeRefs.setMemberValue2Method;

        internal static MethodInfo setValueMethod =>
            Globals.TypeRefs.setValueMethod;

        internal static MethodInfo stringConcat2Method =>
            Globals.TypeRefs.stringConcat2Method;

        internal static MethodInfo stringConcat3Method =>
            Globals.TypeRefs.stringConcat3Method;

        internal static MethodInfo stringConcat4Method =>
            Globals.TypeRefs.stringConcat4Method;

        internal static MethodInfo stringConcatArrMethod =>
            Globals.TypeRefs.stringConcatArrMethod;

        internal static MethodInfo stringEqualsMethod =>
            Globals.TypeRefs.stringEqualsMethod;

        internal static MethodInfo stringLengthMethod =>
            Globals.TypeRefs.stringLengthMethod;

        internal static FieldInfo systemReflectionMissingField =>
            Globals.TypeRefs.systemReflectionMissingField;

        internal static MethodInfo throwTypeMismatch =>
            Globals.TypeRefs.throwTypeMismatch;

        internal static MethodInfo toBooleanMethod =>
            Globals.TypeRefs.toBooleanMethod;

        internal static MethodInfo toForInObjectMethod =>
            Globals.TypeRefs.toForInObjectMethod;

        internal static MethodInfo toInt32Method =>
            Globals.TypeRefs.toInt32Method;

        internal static MethodInfo toNativeArrayMethod =>
            Globals.TypeRefs.toNativeArrayMethod;

        internal static MethodInfo toNumberMethod =>
            Globals.TypeRefs.toNumberMethod;

        internal static MethodInfo toObject2Method =>
            Globals.TypeRefs.toObject2Method;

        internal static MethodInfo toObjectMethod =>
            Globals.TypeRefs.toObjectMethod;

        internal static MethodInfo toStringMethod =>
            Globals.TypeRefs.toStringMethod;

        internal static MethodInfo uint32ToDecimalMethod =>
            Globals.TypeRefs.uint32ToDecimalMethod;

        internal static MethodInfo uint32ToStringMethod =>
            Globals.TypeRefs.uint32ToStringMethod;

        internal static MethodInfo uint64ToDecimalMethod =>
            Globals.TypeRefs.uint64ToDecimalMethod;

        internal static MethodInfo uint64ToStringMethod =>
            Globals.TypeRefs.uint64ToStringMethod;

        internal static MethodInfo uncheckedDecimalToInt64Method =>
            Globals.TypeRefs.uncheckedDecimalToInt64Method;

        internal static FieldInfo undefinedField =>
            Globals.TypeRefs.undefinedField;

        internal static ConstructorInfo vsaEngineConstructor =>
            Globals.TypeRefs.vsaEngineConstructor;

        internal static MethodInfo writeLineMethod =>
            Globals.TypeRefs.writeLineMethod;

        internal static MethodInfo writeMethod =>
            Globals.TypeRefs.writeMethod;
    }
}

