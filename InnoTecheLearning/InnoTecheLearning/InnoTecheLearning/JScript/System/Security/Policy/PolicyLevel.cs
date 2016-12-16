namespace System.Security.Policy
{
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.Deployment.Internal.Isolation;
    using System.Deployment.Internal.Isolation.Manifest;
    using System.Globalization;
    using System.IO;
    using System.Runtime.Hosting;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Security;
    using System.Security.Permissions;
    using System.Security.Util;
    using System.Text;
    using System.Threading;

    [Serializable, ComVisible(true)]
    public sealed class PolicyLevel
    {
        private static string[] EcmaFullTrustAssemblies = new string[] { "mscorlib.resources", "System", "System.resources", "System.Xml", "System.Xml.resources", "System.Windows.Forms", "System.Windows.Forms.resources", "System.Data", "System.Data.resources" };
        private bool m_caching;
        private System.Security.Policy.ConfigId m_configId;
        private Encoding m_encoding;
        private ArrayList m_fullTrustAssemblies;
        private bool m_generateQuickCacheOnLoad;
        private string m_label;
        private bool m_loaded;
        private ArrayList m_namedPermissionSets;
        private string m_path;
        private SecurityElement m_permSetElement;
        private CodeGroup m_rootCodeGroup;
        private bool m_throwOnLoadError;
        [OptionalField(VersionAdded=2)]
        private PolicyLevelType m_type;
        private bool m_useDefaultCodeGroupsOnReset;
        private static string[] MicrosoftFullTrustAssemblies = new string[] { "System.Security", "System.Security.resources", "System.Drawing", "System.Drawing.resources", "System.Messaging", "System.Messaging.resources", "System.ServiceProcess", "System.ServiceProcess.resources", "System.DirectoryServices", "System.DirectoryServices.resources", "System.Deployment", "System.Deployment.resources" };
        private static string[][] s_extensibleNamedPermissionSetRegistryInfo;
        private static readonly string[] s_extensibleNamedPermissionSets = new string[] { "Internet", "LocalIntranet" };
        private static bool s_extensionsReadFromRegistry;
        private static readonly string[] s_FactoryPolicySearchStrings = new string[] { "{VERSION}", "{Policy_PS_FullTrust}", "{Policy_PS_Everything}", "{Policy_PS_Nothing}", "{Policy_PS_SkipVerification}", "{Policy_PS_Execution}" };
        private static object s_InternalSyncObject;
        private static readonly string s_internetPermissionSet = "<PermissionSet class=\"System.Security.NamedPermissionSet\"version=\"1\" Name=\"Internet\" Description=\"{Policy_PS_Internet}\"><Permission class=\"System.Security.Permissions.FileDialogPermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Access=\"Open\"/><Permission class=\"System.Security.Permissions.IsolatedStorageFilePermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" UserQuota=\"512000\" Allowed=\"ApplicationIsolationByUser\"/><Permission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Flags=\"Execution\"/><Permission class=\"System.Security.Permissions.UIPermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Window=\"SafeTopLevelWindows\" Clipboard=\"OwnClipboard\"/><IPermission class=\"System.Drawing.Printing.PrintingPermission, System.Drawing, Version={VERSION}, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a\"version=\"1\"Level=\"SafePrinting\"/></PermissionSet>";
        private static readonly string[] s_InternetPolicySearchStrings = new string[] { "{VERSION}", "{Policy_PS_Internet}" };
        private static readonly string s_localIntranetPermissionSet = "<PermissionSet class=\"System.Security.NamedPermissionSet\"version=\"1\" Name=\"LocalIntranet\" Description=\"{Policy_PS_LocalIntranet}\"><Permission class=\"System.Security.Permissions.EnvironmentPermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Read=\"USERNAME\"/><Permission class=\"System.Security.Permissions.FileDialogPermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Unrestricted=\"true\"/><Permission class=\"System.Security.Permissions.IsolatedStorageFilePermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Allowed=\"AssemblyIsolationByUser\" UserQuota=\"9223372036854775807\" Expiry=\"9223372036854775807\" Permanent=\"true\"/><Permission class=\"System.Security.Permissions.ReflectionPermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Flags=\"ReflectionEmit\"/><Permission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Flags=\"Execution, Assertion, BindingRedirects\"/><Permission class=\"System.Security.Permissions.UIPermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Unrestricted=\"true\"/><IPermission class=\"System.Net.DnsPermission, System, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Unrestricted=\"true\"/><IPermission class=\"System.Drawing.Printing.PrintingPermission, System.Drawing, Version={VERSION}, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a\"version=\"1\"Level=\"DefaultPrinting\"/></PermissionSet>";
        private static readonly string[] s_LocalIntranetPolicySearchStrings = new string[] { "{VERSION}", "{Policy_PS_LocalIntranet}" };
        private static readonly Version s_mscorlibVersion = Assembly.GetExecutingAssembly().GetVersion();
        private static readonly string[] s_reservedNamedPermissionSets = new string[] { "FullTrust", "Nothing", "Execution", "SkipVerification", "Internet", "LocalIntranet" };

        private PolicyLevel()
        {
        }

        internal PolicyLevel(PolicyLevelType type) : this(type, GetLocationFromType(type))
        {
        }

        internal PolicyLevel(PolicyLevelType type, string path) : this(type, path, System.Security.Policy.ConfigId.None)
        {
        }

        internal PolicyLevel(PolicyLevelType type, string path, System.Security.Policy.ConfigId configId)
        {
            this.m_type = type;
            this.m_path = path;
            this.m_loaded = path == null;
            if (this.m_path == null)
            {
                this.m_rootCodeGroup = this.CreateDefaultAllGroup();
                this.SetFactoryPermissionSets();
                this.SetDefaultFullTrustAssemblies();
            }
            this.m_configId = configId;
        }

        [Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
        public void AddFullTrustAssembly(StrongName sn)
        {
            if (sn == null)
            {
                throw new ArgumentNullException("sn");
            }
            this.AddFullTrustAssembly(new StrongNameMembershipCondition(sn.PublicKey, sn.Name, sn.Version));
        }

        [Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
        public void AddFullTrustAssembly(StrongNameMembershipCondition snMC)
        {
            if (snMC == null)
            {
                throw new ArgumentNullException("snMC");
            }
            this.CheckLoaded();
            IEnumerator enumerator = this.m_fullTrustAssemblies.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (((StrongNameMembershipCondition) enumerator.Current).Equals(snMC))
                {
                    throw new ArgumentException(Environment.GetResourceString("Argument_AssemblyAlreadyFullTrust"));
                }
            }
            lock (this.m_fullTrustAssemblies)
            {
                this.m_fullTrustAssemblies.Add(snMC);
            }
        }

        public void AddNamedPermissionSet(NamedPermissionSet permSet)
        {
            if (permSet == null)
            {
                throw new ArgumentNullException("permSet");
            }
            this.CheckLoaded();
            this.LoadAllPermissionSets();
            lock (this)
            {
                IEnumerator enumerator = this.m_namedPermissionSets.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (((NamedPermissionSet) enumerator.Current).Name.Equals(permSet.Name))
                    {
                        throw new ArgumentException(Environment.GetResourceString("Argument_DuplicateName"));
                    }
                }
                NamedPermissionSet set = (NamedPermissionSet) permSet.Copy();
                set.IgnoreTypeLoadFailures = true;
                this.m_namedPermissionSets.Add(set);
            }
        }

        private void Cache(int count, char[] serializedEvidence, PolicyStatement policy)
        {
            if (this.m_configId != System.Security.Policy.ConfigId.None)
            {
                byte[] data = new SecurityDocument(policy.ToXml(null, true)).m_data;
                Config.AddCacheEntry(this.m_configId, count, serializedEvidence, data);
            }
        }

        public NamedPermissionSet ChangeNamedPermissionSet(string name, PermissionSet pSet)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (pSet == null)
            {
                throw new ArgumentNullException("pSet");
            }
            for (int i = 0; i < s_reservedNamedPermissionSets.Length; i++)
            {
                if (s_reservedNamedPermissionSets[i].Equals(name))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ReservedNPMS"), new object[] { name }));
                }
            }
            NamedPermissionSet namedPermissionSetInternal = this.GetNamedPermissionSetInternal(name);
            if (namedPermissionSetInternal == null)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_NoNPMS"));
            }
            NamedPermissionSet set2 = (NamedPermissionSet) namedPermissionSetInternal.Copy();
            namedPermissionSetInternal.Reset();
            namedPermissionSetInternal.SetUnrestricted(pSet.IsUnrestricted());
            IEnumerator enumerator = pSet.GetEnumerator();
            while (enumerator.MoveNext())
            {
                namedPermissionSetInternal.SetPermission(((IPermission) enumerator.Current).Copy());
            }
            if (pSet is NamedPermissionSet)
            {
                namedPermissionSetInternal.Description = ((NamedPermissionSet) pSet).Description;
            }
            return set2;
        }

        private PolicyStatement CheckCache(int count, char[] serializedEvidence)
        {
            byte[] buffer;
            if (this.m_configId == System.Security.Policy.ConfigId.None)
            {
                return null;
            }
            if (!Config.GetCacheEntry(this.m_configId, count, serializedEvidence, out buffer))
            {
                return null;
            }
            PolicyStatement statement = new PolicyStatement();
            SecurityDocument doc = new SecurityDocument(buffer);
            statement.FromXml(doc, 0, null, true);
            return statement;
        }

        private void CheckLoaded()
        {
            if (!this.m_loaded)
            {
                lock (InternalSyncObject)
                {
                    if (!this.m_loaded)
                    {
                        this.LoadPolicyLevel();
                    }
                }
            }
        }

        public static PolicyLevel CreateAppDomainLevel() => 
            new PolicyLevel(PolicyLevelType.AppDomain);

        private static SecurityElement CreateCodeGroupElement(string codeGroupType, string permissionSetName, SecurityElement mshipElement)
        {
            SecurityElement element = new SecurityElement("CodeGroup");
            element.AddAttribute("class", "System.Security." + codeGroupType + ", mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            element.AddAttribute("version", "1");
            element.AddAttribute("PermissionSetName", permissionSetName);
            element.AddChild(mshipElement);
            return element;
        }

        private CodeGroup CreateDefaultAllGroup()
        {
            UnionCodeGroup group = new UnionCodeGroup();
            group.FromXml(CreateCodeGroupElement("UnionCodeGroup", "FullTrust", new AllMembershipCondition().ToXml()), this);
            group.Name = Environment.GetResourceString("Policy_AllCode_Name");
            group.Description = Environment.GetResourceString("Policy_AllCode_DescriptionFullTrust");
            return group;
        }

        private CodeGroup CreateDefaultMachinePolicy()
        {
            UnionCodeGroup group = new UnionCodeGroup();
            group.FromXml(CreateCodeGroupElement("UnionCodeGroup", "Nothing", new AllMembershipCondition().ToXml()), this);
            group.Name = Environment.GetResourceString("Policy_AllCode_Name");
            group.Description = Environment.GetResourceString("Policy_AllCode_DescriptionNothing");
            UnionCodeGroup group2 = new UnionCodeGroup();
            group2.FromXml(CreateCodeGroupElement("UnionCodeGroup", "FullTrust", new ZoneMembershipCondition(SecurityZone.MyComputer).ToXml()), this);
            group2.Name = Environment.GetResourceString("Policy_MyComputer_Name");
            group2.Description = Environment.GetResourceString("Policy_MyComputer_Description");
            StrongNamePublicKeyBlob blob = new StrongNamePublicKeyBlob("002400000480000094000000060200000024000052534131000400000100010007D1FA57C4AED9F0A32E84AA0FAEFD0DE9E8FD6AEC8F87FB03766C834C99921EB23BE79AD9D5DCC1DD9AD236132102900B723CF980957FC4E177108FC607774F29E8320E92EA05ECE4E821C0A5EFE8F1645C4C0C93C1AB99285D622CAA652C1DFAD63D745D6F2DE5F17E5EAF0FC4963D261C8A12436518206DC093344D5AD293");
            UnionCodeGroup group3 = new UnionCodeGroup();
            group3.FromXml(CreateCodeGroupElement("UnionCodeGroup", "FullTrust", new StrongNameMembershipCondition(blob, null, null).ToXml()), this);
            group3.Name = Environment.GetResourceString("Policy_Microsoft_Name");
            group3.Description = Environment.GetResourceString("Policy_Microsoft_Description");
            group2.AddChildInternal(group3);
            blob = new StrongNamePublicKeyBlob("00000000000000000400000000000000");
            UnionCodeGroup group4 = new UnionCodeGroup();
            group4.FromXml(CreateCodeGroupElement("UnionCodeGroup", "FullTrust", new StrongNameMembershipCondition(blob, null, null).ToXml()), this);
            group4.Name = Environment.GetResourceString("Policy_Ecma_Name");
            group4.Description = Environment.GetResourceString("Policy_Ecma_Description");
            group2.AddChildInternal(group4);
            group.AddChildInternal(group2);
            CodeGroup group5 = new UnionCodeGroup();
            group5.FromXml(CreateCodeGroupElement("UnionCodeGroup", "LocalIntranet", new ZoneMembershipCondition(SecurityZone.Intranet).ToXml()), this);
            group5.Name = Environment.GetResourceString("Policy_Intranet_Name");
            group5.Description = Environment.GetResourceString("Policy_Intranet_Description");
            CodeGroup group6 = new NetCodeGroup(new AllMembershipCondition()) {
                Name = Environment.GetResourceString("Policy_IntranetNet_Name"),
                Description = Environment.GetResourceString("Policy_IntranetNet_Description")
            };
            group5.AddChildInternal(group6);
            CodeGroup group7 = new FileCodeGroup(new AllMembershipCondition(), FileIOPermissionAccess.PathDiscovery | FileIOPermissionAccess.Read) {
                Name = Environment.GetResourceString("Policy_IntranetFile_Name"),
                Description = Environment.GetResourceString("Policy_IntranetFile_Description")
            };
            group5.AddChildInternal(group7);
            group.AddChildInternal(group5);
            CodeGroup group8 = new UnionCodeGroup();
            group8.FromXml(CreateCodeGroupElement("UnionCodeGroup", "Internet", new ZoneMembershipCondition(SecurityZone.Internet).ToXml()), this);
            group8.Name = Environment.GetResourceString("Policy_Internet_Name");
            group8.Description = Environment.GetResourceString("Policy_Internet_Description");
            CodeGroup group9 = new NetCodeGroup(new AllMembershipCondition()) {
                Name = Environment.GetResourceString("Policy_InternetNet_Name"),
                Description = Environment.GetResourceString("Policy_InternetNet_Description")
            };
            group8.AddChildInternal(group9);
            group.AddChildInternal(group8);
            CodeGroup group10 = new UnionCodeGroup();
            group10.FromXml(CreateCodeGroupElement("UnionCodeGroup", "Nothing", new ZoneMembershipCondition(SecurityZone.Untrusted).ToXml()), this);
            group10.Name = Environment.GetResourceString("Policy_Untrusted_Name");
            group10.Description = Environment.GetResourceString("Policy_Untrusted_Description");
            group.AddChildInternal(group10);
            CodeGroup group11 = new UnionCodeGroup();
            group11.FromXml(CreateCodeGroupElement("UnionCodeGroup", "Internet", new ZoneMembershipCondition(SecurityZone.Trusted).ToXml()), this);
            group11.Name = Environment.GetResourceString("Policy_Trusted_Name");
            group11.Description = Environment.GetResourceString("Policy_Trusted_Description");
            CodeGroup group12 = new NetCodeGroup(new AllMembershipCondition()) {
                Name = Environment.GetResourceString("Policy_TrustedNet_Name"),
                Description = Environment.GetResourceString("Policy_TrustedNet_Description")
            };
            group11.AddChildInternal(group12);
            group.AddChildInternal(group11);
            return group;
        }

        private static NamedPermissionSet CreateExecutionSet()
        {
            NamedPermissionSet set = new NamedPermissionSet("Execution", PermissionState.None) {
                m_descrResource = "Policy_PS_Execution"
            };
            set.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            return set;
        }

        private static NamedPermissionSet CreateFullTrustSet() => 
            new NamedPermissionSet("FullTrust", PermissionState.Unrestricted) { m_descrResource = "Policy_PS_FullTrust" };

        private static NamedPermissionSet CreateInternetSet()
        {
            PolicyLevel level = new PolicyLevel(PolicyLevelType.User);
            return level.GetNamedPermissionSet("Internet");
        }

        private static NamedPermissionSet CreateLocalIntranetSet()
        {
            PolicyLevel level = new PolicyLevel(PolicyLevelType.User);
            return level.GetNamedPermissionSet("LocalIntranet");
        }

        private static NamedPermissionSet CreateNothingSet() => 
            new NamedPermissionSet("Nothing", PermissionState.None) { m_descrResource = "Policy_PS_Nothing" };

        private static NamedPermissionSet CreateSkipVerificationSet()
        {
            NamedPermissionSet set = new NamedPermissionSet("SkipVerification", PermissionState.None) {
                m_descrResource = "Policy_PS_SkipVerification"
            };
            set.AddPermission(new SecurityPermission(SecurityPermissionFlag.SkipVerification));
            return set;
        }

        private static bool DependentAssembliesContainPermission(IAssemblyReferenceEntry[] asmEntries, SecurityElement se)
        {
            string str;
            string str2;
            string str3;
            bool flag = XMLUtil.ParseElementForAssemblyIdentification(se, out str, out str2, out str3);
            if (!flag)
            {
                return flag;
            }
            IEnumerator enumerator = asmEntries.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IAssemblyReferenceEntry current = (IAssemblyReferenceEntry) enumerator.Current;
                if (current != null)
                {
                    IReferenceIdentity referenceIdentity = current.ReferenceIdentity;
                    if (referenceIdentity != null)
                    {
                        string attribute = referenceIdentity.GetAttribute(null, "Name");
                        string strA = referenceIdentity.GetAttribute(null, "Version");
                        if ((string.Compare(attribute, str2, StringComparison.OrdinalIgnoreCase) == 0) && (string.Compare(strA, str3, StringComparison.OrdinalIgnoreCase) == 0))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private string DeriveLabelFromType()
        {
            switch (this.m_type)
            {
                case PolicyLevelType.User:
                    return Environment.GetResourceString("Policy_PL_User");

                case PolicyLevelType.Machine:
                    return Environment.GetResourceString("Policy_PL_Machine");

                case PolicyLevelType.Enterprise:
                    return Environment.GetResourceString("Policy_PL_Enterprise");

                case PolicyLevelType.AppDomain:
                    return Environment.GetResourceString("Policy_PL_AppDomain");
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumIllegalVal"), new object[] { (int) this.m_type }));
        }

        private void DeriveTypeFromLabel()
        {
            if (this.m_label.Equals(Environment.GetResourceString("Policy_PL_User")))
            {
                this.m_type = PolicyLevelType.User;
            }
            else if (this.m_label.Equals(Environment.GetResourceString("Policy_PL_Machine")))
            {
                this.m_type = PolicyLevelType.Machine;
            }
            else if (this.m_label.Equals(Environment.GetResourceString("Policy_PL_Enterprise")))
            {
                this.m_type = PolicyLevelType.Enterprise;
            }
            else
            {
                if (!this.m_label.Equals(Environment.GetResourceString("Policy_PL_AppDomain")))
                {
                    throw new ArgumentException(Environment.GetResourceString("Policy_Default"));
                }
                this.m_type = PolicyLevelType.AppDomain;
            }
        }

        private static void ExtendNamedPermissionSetsIfApplicable(PolicyStatement ps, Evidence evidence)
        {
            if (!s_extensionsReadFromRegistry)
            {
                ReadNamedPermissionSetExtensionsFromRegistry();
            }
            if (s_extensibleNamedPermissionSetRegistryInfo != null)
            {
                NamedPermissionSet permissionSetNoCopy = ps.GetPermissionSetNoCopy() as NamedPermissionSet;
                if (permissionSetNoCopy != null)
                {
                    int index = Array.IndexOf<string>(s_extensibleNamedPermissionSets, permissionSetNoCopy.Name);
                    if (index != -1)
                    {
                        IEnumerator hostEnumerator = evidence.GetHostEnumerator();
                        ActivationArguments current = null;
                        while (hostEnumerator.MoveNext())
                        {
                            current = hostEnumerator.Current as ActivationArguments;
                            if (current != null)
                            {
                                break;
                            }
                        }
                        if (current != null)
                        {
                            ActivationContext activationContext = current.ActivationContext;
                            if (activationContext != null)
                            {
                                PermissionSet namedPermissionSetExtensions = GetNamedPermissionSetExtensions(CmsUtils.GetDependentAssemblies(activationContext), s_extensibleNamedPermissionSetRegistryInfo[index]);
                                ps.GetPermissionSetNoCopy().InplaceUnion(namedPermissionSetExtensions);
                            }
                        }
                    }
                }
            }
        }

        private SecurityElement FindElement(string name)
        {
            SecurityElement element = this.FindElement(this.m_permSetElement, name);
            if (this.m_permSetElement.InternalChildren.Count == 0)
            {
                this.m_permSetElement = null;
            }
            return element;
        }

        private SecurityElement FindElement(SecurityElement element, string name)
        {
            IEnumerator enumerator = element.Children.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SecurityElement current = (SecurityElement) enumerator.Current;
                if (current.Tag.Equals("PermissionSet"))
                {
                    string str = current.Attribute("Name");
                    if ((str != null) && str.Equals(name))
                    {
                        element.InternalChildren.Remove(current);
                        return current;
                    }
                }
            }
            return null;
        }

        public void FromXml(SecurityElement e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            lock (this)
            {
                Hashtable hashtable;
                ArrayList list = new ArrayList();
                SecurityElement element = e.SearchForChildByTag("SecurityClasses");
                if (element != null)
                {
                    hashtable = new Hashtable();
                    IEnumerator enumerator = element.Children.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        SecurityElement current = (SecurityElement) enumerator.Current;
                        if (current.Tag.Equals("SecurityClass"))
                        {
                            string key = current.Attribute("Name");
                            string str2 = current.Attribute("Description");
                            if ((key != null) && (str2 != null))
                            {
                                hashtable.Add(key, str2);
                            }
                        }
                    }
                }
                else
                {
                    hashtable = null;
                }
                SecurityElement element3 = e.SearchForChildByTag("FullTrustAssemblies");
                if ((element3 != null) && (element3.InternalChildren != null))
                {
                    string assemblyQualifiedName = typeof(StrongNameMembershipCondition).AssemblyQualifiedName;
                    IEnumerator enumerator2 = element3.Children.GetEnumerator();
                    while (enumerator2.MoveNext())
                    {
                        StrongNameMembershipCondition condition = new StrongNameMembershipCondition();
                        condition.FromXml((SecurityElement) enumerator2.Current);
                        list.Add(condition);
                    }
                }
                this.m_fullTrustAssemblies = list;
                ArrayList list2 = new ArrayList();
                SecurityElement elem = e.SearchForChildByTag("NamedPermissionSets");
                SecurityElement element5 = null;
                if ((elem != null) && (elem.InternalChildren != null))
                {
                    element5 = this.UnnormalizeClassDeep(elem, hashtable);
                    this.FindElement(element5, "FullTrust");
                    this.FindElement(element5, "SkipVerification");
                    this.FindElement(element5, "Execution");
                    this.FindElement(element5, "Nothing");
                    this.FindElement(element5, "Internet");
                    this.FindElement(element5, "LocalIntranet");
                }
                if (element5 == null)
                {
                    element5 = new SecurityElement("NamedPermissionSets");
                }
                list2.Add(CreateFullTrustSet());
                list2.Add(CreateSkipVerificationSet());
                list2.Add(CreateExecutionSet());
                list2.Add(CreateNothingSet());
                element5.AddChild(GetInternetElement());
                element5.AddChild(GetLocalIntranetElement());
                foreach (PermissionSet set in list2)
                {
                    set.IgnoreTypeLoadFailures = true;
                }
                this.m_namedPermissionSets = list2;
                this.m_permSetElement = element5;
                SecurityElement element6 = e.SearchForChildByTag("CodeGroup");
                if (element6 == null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidXMLElement"), new object[] { "CodeGroup", base.GetType().FullName }));
                }
                CodeGroup group = XMLUtil.CreateCodeGroup(this.UnnormalizeClassDeep(element6, hashtable));
                if (group == null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidXMLElement"), new object[] { "CodeGroup", base.GetType().FullName }));
                }
                group.FromXml(element6, this);
                this.m_rootCodeGroup = group;
            }
        }

        private static string GenerateFriendlyName(string className, Hashtable classes)
        {
            if (classes.ContainsKey(className))
            {
                return (string) classes[className];
            }
            System.Type type = System.Type.GetType(className, false, false);
            if ((type != null) && !type.IsVisible)
            {
                type = null;
            }
            if (type == null)
            {
                return className;
            }
            if (!classes.ContainsValue(type.Name))
            {
                classes.Add(className, type.Name);
                return type.Name;
            }
            if (!classes.ContainsValue(type.FullName))
            {
                classes.Add(className, type.FullName);
                return type.FullName;
            }
            classes.Add(className, type.AssemblyQualifiedName);
            return type.AssemblyQualifiedName;
        }

        private ArrayList GenericResolve(Evidence evidence, out bool allConst)
        {
            CodeGroupStack stack = new CodeGroupStack();
            CodeGroup rootCodeGroup = this.m_rootCodeGroup;
            if (rootCodeGroup == null)
            {
                throw new PolicyException(Environment.GetResourceString("Policy_NonFullTrustAssembly"));
            }
            CodeGroupStackFrame element = new CodeGroupStackFrame {
                current = rootCodeGroup,
                parent = null
            };
            stack.Push(element);
            ArrayList list = new ArrayList();
            bool flag = false;
            allConst = true;
            bool flag2 = true;
            IEnumerator hostEnumerator = evidence.GetHostEnumerator();
            while (hostEnumerator.MoveNext() && flag2)
            {
                IDelayEvaluatedEvidence current = hostEnumerator.Current as IDelayEvaluatedEvidence;
                if ((current != null) && !current.IsVerified)
                {
                    flag2 = false;
                }
            }
            Exception exception = null;
            while (!stack.IsEmpty())
            {
                element = stack.Pop();
                IUnionSemanticCodeGroup group2 = null;
                if (flag2)
                {
                    group2 = element.current as IUnionSemanticCodeGroup;
                }
                FirstMatchCodeGroup group3 = element.current as FirstMatchCodeGroup;
                if (!(element.current.MembershipCondition is IConstantMembershipCondition) || ((group2 == null) && (group3 == null)))
                {
                    allConst = false;
                }
                try
                {
                    if (group2 != null)
                    {
                        element.policy = group2.InternalResolve(evidence);
                    }
                    else
                    {
                        element.policy = PolicyManager.ResolveCodeGroup(element.current, evidence);
                    }
                }
                catch (Exception exception2)
                {
                    if (exception == null)
                    {
                        exception = exception2;
                    }
                }
                if (element.policy != null)
                {
                    ExtendNamedPermissionSetsIfApplicable(element.policy, evidence);
                    if ((element.policy.Attributes & PolicyStatementAttribute.Exclusive) != PolicyStatementAttribute.Nothing)
                    {
                        if (flag)
                        {
                            throw new PolicyException(Environment.GetResourceString("Policy_MultipleExclusive"));
                        }
                        list.RemoveRange(0, list.Count);
                        list.Add(element);
                        flag = true;
                    }
                    if (group2 != null)
                    {
                        IList childrenInternal = element.current.GetChildrenInternal();
                        if ((childrenInternal != null) && (childrenInternal.Count > 0))
                        {
                            IEnumerator enumerator = childrenInternal.GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                CodeGroupStackFrame frame2 = new CodeGroupStackFrame {
                                    current = (CodeGroup) enumerator.Current,
                                    parent = element
                                };
                                stack.Push(frame2);
                            }
                        }
                    }
                    if (!flag)
                    {
                        list.Add(element);
                    }
                }
            }
            if (exception != null)
            {
                throw exception;
            }
            return list;
        }

        internal static PermissionSet GetBuiltInSet(string name)
        {
            if (name != null)
            {
                if (name.Equals("FullTrust"))
                {
                    return CreateFullTrustSet();
                }
                if (name.Equals("Nothing"))
                {
                    return CreateNothingSet();
                }
                if (name.Equals("Execution"))
                {
                    return CreateExecutionSet();
                }
                if (name.Equals("SkipVerification"))
                {
                    return CreateSkipVerificationSet();
                }
                if (name.Equals("Internet"))
                {
                    return CreateInternetSet();
                }
                if (name.Equals("LocalIntranet"))
                {
                    return CreateLocalIntranetSet();
                }
            }
            return null;
        }

        private static SecurityElement GetInternetElement()
        {
            string[] replaceStrings = new string[s_InternetPolicySearchStrings.Length];
            replaceStrings[0] = "2.0.0.0";
            replaceStrings[1] = Environment.GetResourceString("Policy_PS_Internet");
            return new Parser(s_internetPermissionSet, s_InternetPolicySearchStrings, replaceStrings).GetTopElement();
        }

        private static SecurityElement GetLocalIntranetElement()
        {
            string[] replaceStrings = new string[s_LocalIntranetPolicySearchStrings.Length];
            replaceStrings[0] = "2.0.0.0";
            replaceStrings[1] = Environment.GetResourceString("Policy_PS_LocalIntranet");
            return new Parser(s_localIntranetPermissionSet, s_LocalIntranetPolicySearchStrings, replaceStrings).GetTopElement();
        }

        internal static string GetLocationFromType(PolicyLevelType type)
        {
            switch (type)
            {
                case PolicyLevelType.User:
                    return (Config.UserDirectory + "security.config");

                case PolicyLevelType.Machine:
                    return (Config.MachineDirectory + "security.config");

                case PolicyLevelType.Enterprise:
                    return (Config.MachineDirectory + "enterprisesec.config");
            }
            return null;
        }

        public NamedPermissionSet GetNamedPermissionSet(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            NamedPermissionSet namedPermissionSetInternal = this.GetNamedPermissionSetInternal(name);
            if (namedPermissionSetInternal != null)
            {
                return new NamedPermissionSet(namedPermissionSetInternal);
            }
            return null;
        }

        private static PermissionSet GetNamedPermissionSetExtensions(IAssemblyReferenceEntry[] asmEntries, string[] extnList)
        {
            if (extnList == null)
            {
                return null;
            }
            SecurityElement et = null;
            for (int i = 0; i < extnList.Length; i++)
            {
                string str = extnList[i];
                if (!string.IsNullOrEmpty(str))
                {
                    SecurityElement se = SecurityElement.FromString(str);
                    if (DependentAssembliesContainPermission(asmEntries, se))
                    {
                        if (et == null)
                        {
                            et = PermissionSet.CreateEmptyPermissionSetXml();
                        }
                        et.AddChild(se);
                    }
                }
            }
            PermissionSet set = null;
            if (et != null)
            {
                set = new PermissionSet(PermissionState.None);
                set.FromXml(et);
            }
            return set;
        }

        internal NamedPermissionSet GetNamedPermissionSetInternal(string name)
        {
            this.CheckLoaded();
            lock (InternalSyncObject)
            {
                IEnumerator enumerator = this.m_namedPermissionSets.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    NamedPermissionSet current = (NamedPermissionSet) enumerator.Current;
                    if (current.Name.Equals(name))
                    {
                        return current;
                    }
                }
                if (this.m_permSetElement != null)
                {
                    SecurityElement et = this.FindElement(name);
                    if (et != null)
                    {
                        NamedPermissionSet set2 = new NamedPermissionSet {
                            Name = name
                        };
                        this.m_namedPermissionSets.Add(set2);
                        try
                        {
                            set2.FromXml(et, false, true);
                        }
                        catch
                        {
                            this.m_namedPermissionSets.Remove(set2);
                            return null;
                        }
                        if (set2.Name != null)
                        {
                            return set2;
                        }
                        this.m_namedPermissionSets.Remove(set2);
                        return null;
                    }
                }
                return null;
            }
        }

        private static bool IsFullTrustAssembly(ArrayList fullTrustAssemblies, Evidence evidence)
        {
            if ((fullTrustAssemblies.Count != 0) && (evidence != null))
            {
                lock (fullTrustAssemblies)
                {
                    IEnumerator enumerator = fullTrustAssemblies.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        StrongNameMembershipCondition current = (StrongNameMembershipCondition) enumerator.Current;
                        if (current.Check(evidence))
                        {
                            if (Environment.GetCompatibilityFlag(CompatibilityFlag.FullTrustListAssembliesInGac))
                            {
                                if (new ZoneMembershipCondition().Check(evidence))
                                {
                                    return true;
                                }
                            }
                            else if (new GacMembershipCondition().Check(evidence))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private void LoadAllPermissionSets()
        {
            if ((this.m_permSetElement != null) && (this.m_permSetElement.InternalChildren != null))
            {
                lock (InternalSyncObject)
                {
                    while ((this.m_permSetElement != null) && (this.m_permSetElement.InternalChildren.Count != 0))
                    {
                        SecurityElement et = (SecurityElement) this.m_permSetElement.Children[this.m_permSetElement.InternalChildren.Count - 1];
                        this.m_permSetElement.InternalChildren.RemoveAt(this.m_permSetElement.InternalChildren.Count - 1);
                        if (et.Tag.Equals("PermissionSet") && et.Attribute("class").Equals("System.Security.NamedPermissionSet"))
                        {
                            NamedPermissionSet set = new NamedPermissionSet();
                            set.FromXmlNameOnly(et);
                            if (set.Name != null)
                            {
                                this.m_namedPermissionSets.Add(set);
                                try
                                {
                                    set.FromXml(et, false, true);
                                    continue;
                                }
                                catch
                                {
                                    this.m_namedPermissionSets.Remove(set);
                                    continue;
                                }
                            }
                        }
                    }
                    this.m_permSetElement = null;
                }
            }
        }

        private Exception LoadError(string message)
        {
            if (((this.m_type != PolicyLevelType.User) && (this.m_type != PolicyLevelType.Machine)) && (this.m_type != PolicyLevelType.Enterprise))
            {
                return new ArgumentException(message);
            }
            Config.WriteToEventLog(message);
            return null;
        }

        private void LoadPolicyLevel()
        {
            Exception exception = null;
            CodeAccessPermission.AssertAllPossible();
            if (File.InternalExists(this.m_path))
            {
                SecurityElement element;
                Encoding encoding = Encoding.UTF8;
                try
                {
                    element = SecurityElement.FromString(encoding.GetString(ReadFile(this.m_path)));
                }
                catch (Exception exception2)
                {
                    string message;
                    if (!string.IsNullOrEmpty(exception2.Message))
                    {
                        message = exception2.Message;
                    }
                    else
                    {
                        message = exception2.GetType().AssemblyQualifiedName;
                    }
                    exception = this.LoadError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Error_SecurityPolicyFileParseEx"), new object[] { this.Label, message }));
                    goto Label_022A;
                }
                if (element == null)
                {
                    exception = this.LoadError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Error_SecurityPolicyFileParse"), new object[] { this.Label }));
                }
                else
                {
                    SecurityElement element2 = element.SearchForChildByTag("mscorlib");
                    if (element2 == null)
                    {
                        exception = this.LoadError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Error_SecurityPolicyFileParse"), new object[] { this.Label }));
                    }
                    else
                    {
                        SecurityElement element3 = element2.SearchForChildByTag("security");
                        if (element3 == null)
                        {
                            exception = this.LoadError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Error_SecurityPolicyFileParse"), new object[] { this.Label }));
                        }
                        else
                        {
                            SecurityElement element4 = element3.SearchForChildByTag("policy");
                            if (element4 == null)
                            {
                                exception = this.LoadError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Error_SecurityPolicyFileParse"), new object[] { this.Label }));
                            }
                            else
                            {
                                SecurityElement e = element4.SearchForChildByTag("PolicyLevel");
                                if (e != null)
                                {
                                    try
                                    {
                                        this.FromXml(e);
                                        this.m_loaded = true;
                                        return;
                                    }
                                    catch (Exception)
                                    {
                                        exception = this.LoadError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Error_SecurityPolicyFileParse"), new object[] { this.Label }));
                                        goto Label_022A;
                                    }
                                }
                                exception = this.LoadError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Error_SecurityPolicyFileParse"), new object[] { this.Label }));
                            }
                        }
                    }
                }
            }
        Label_022A:
            this.SetDefault();
            this.m_loaded = true;
            if (exception != null)
            {
                throw exception;
            }
        }

        private SecurityElement NormalizeClass(SecurityElement elem, Hashtable classes)
        {
            if ((elem.m_lAttributes != null) && (elem.m_lAttributes.Count != 0))
            {
                int count = elem.m_lAttributes.Count;
                for (int i = 0; i < count; i += 2)
                {
                    string str = (string) elem.m_lAttributes[i];
                    if (str.Equals("class"))
                    {
                        string className = (string) elem.m_lAttributes[i + 1];
                        elem.m_lAttributes[i + 1] = GenerateFriendlyName(className, classes);
                        return elem;
                    }
                }
            }
            return elem;
        }

        private SecurityElement NormalizeClassDeep(SecurityElement elem, Hashtable classes)
        {
            this.NormalizeClass(elem, classes);
            if ((elem.InternalChildren != null) && (elem.InternalChildren.Count > 0))
            {
                IEnumerator enumerator = elem.Children.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    this.NormalizeClassDeep((SecurityElement) enumerator.Current, classes);
                }
            }
            return elem;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            if (this.m_label != null)
            {
                this.DeriveTypeFromLabel();
            }
        }

        private static byte[] ReadFile(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                int length = (int) stream.Length;
                byte[] buffer = new byte[length];
                length = stream.Read(buffer, 0, length);
                stream.Close();
                return buffer;
            }
        }

        private static void ReadNamedPermissionSetExtensionsFromRegistry()
        {
            if (!s_extensionsReadFromRegistry)
            {
                lock (InternalSyncObject)
                {
                    bool flag = false;
                    if (!s_extensionsReadFromRegistry)
                    {
                        string[][] strArray = null;
                        new RegistryPermission(RegistryPermissionAccess.Read, "HKEY_LOCAL_MACHINE").Assert();
                        using (RegistryKey key2 = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\.NETFramework", false))
                        {
                            if (key2 != null)
                            {
                                using (RegistryKey key3 = key2.OpenSubKey(@"Security\Policy\Extensions\NamedPermissionSets", false))
                                {
                                    if (key3 != null)
                                    {
                                        strArray = new string[s_extensibleNamedPermissionSets.Length][];
                                        for (int i = 0; i < s_extensibleNamedPermissionSets.Length; i++)
                                        {
                                            using (RegistryKey key4 = key3.OpenSubKey(s_extensibleNamedPermissionSets[i], false))
                                            {
                                                if (key4 != null)
                                                {
                                                    string[] subKeyNames = key4.InternalGetSubKeyNames();
                                                    strArray[i] = new string[subKeyNames.Length];
                                                    for (int j = 0; j < subKeyNames.Length; j++)
                                                    {
                                                        using (RegistryKey key5 = key4.OpenSubKey(subKeyNames[j], false))
                                                        {
                                                            string str = key5.GetValue("Xml") as string;
                                                            strArray[i][j] = str;
                                                            flag = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (flag)
                        {
                            s_extensibleNamedPermissionSetRegistryInfo = strArray;
                        }
                        s_extensionsReadFromRegistry = true;
                    }
                }
            }
        }

        public void Recover()
        {
            if (this.m_configId == System.Security.Policy.ConfigId.None)
            {
                throw new PolicyException(Environment.GetResourceString("Policy_RecoverNotFileBased"));
            }
            lock (this)
            {
                if (!Config.RecoverData(this.m_configId))
                {
                    throw new PolicyException(Environment.GetResourceString("Policy_RecoverNoConfigFile"));
                }
                this.m_loaded = false;
                this.m_rootCodeGroup = null;
                this.m_namedPermissionSets = null;
                this.m_fullTrustAssemblies = new ArrayList();
            }
        }

        [Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
        public void RemoveFullTrustAssembly(StrongName sn)
        {
            if (sn == null)
            {
                throw new ArgumentNullException("assembly");
            }
            this.RemoveFullTrustAssembly(new StrongNameMembershipCondition(sn.PublicKey, sn.Name, sn.Version));
        }

        [Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
        public void RemoveFullTrustAssembly(StrongNameMembershipCondition snMC)
        {
            if (snMC == null)
            {
                throw new ArgumentNullException("snMC");
            }
            this.CheckLoaded();
            object current = null;
            IEnumerator enumerator = this.m_fullTrustAssemblies.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (((StrongNameMembershipCondition) enumerator.Current).Equals(snMC))
                {
                    current = enumerator.Current;
                    break;
                }
            }
            if (current == null)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_AssemblyNotFullTrust"));
            }
            lock (this.m_fullTrustAssemblies)
            {
                this.m_fullTrustAssemblies.Remove(current);
            }
        }

        public NamedPermissionSet RemoveNamedPermissionSet(NamedPermissionSet permSet)
        {
            if (permSet == null)
            {
                throw new ArgumentNullException("permSet");
            }
            return this.RemoveNamedPermissionSet(permSet.Name);
        }

        public NamedPermissionSet RemoveNamedPermissionSet(string name)
        {
            this.CheckLoaded();
            this.LoadAllPermissionSets();
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            int index = -1;
            for (int i = 0; i < s_reservedNamedPermissionSets.Length; i++)
            {
                if (s_reservedNamedPermissionSets[i].Equals(name))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ReservedNPMS"), new object[] { name }));
                }
            }
            ArrayList namedPermissionSets = this.m_namedPermissionSets;
            for (int j = 0; j < namedPermissionSets.Count; j++)
            {
                if (((NamedPermissionSet) namedPermissionSets[j]).Name.Equals(name))
                {
                    index = j;
                    break;
                }
            }
            if (index == -1)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_NoNPMS"));
            }
            ArrayList list2 = new ArrayList {
                this.m_rootCodeGroup
            };
            for (int k = 0; k < list2.Count; k++)
            {
                CodeGroup group = (CodeGroup) list2[k];
                if ((group.PermissionSetName != null) && group.PermissionSetName.Equals(name))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_NPMSInUse"), new object[] { name }));
                }
                IEnumerator enumerator = group.Children.GetEnumerator();
                if (enumerator != null)
                {
                    while (enumerator.MoveNext())
                    {
                        list2.Add(enumerator.Current);
                    }
                }
            }
            NamedPermissionSet set = (NamedPermissionSet) namedPermissionSets[index];
            namedPermissionSets.RemoveAt(index);
            return set;
        }

        public void Reset()
        {
            this.SetDefault();
        }

        public PolicyStatement Resolve(Evidence evidence) => 
            this.Resolve(evidence, 0, null);

        internal PolicyStatement Resolve(Evidence evidence, int count, char[] serializedEvidence)
        {
            if (evidence == null)
            {
                throw new ArgumentNullException("evidence");
            }
            PolicyStatement policy = null;
            if (serializedEvidence != null)
            {
                policy = this.CheckCache(count, serializedEvidence);
            }
            if (policy == null)
            {
                bool flag;
                this.CheckLoaded();
                if ((this.m_fullTrustAssemblies != null) && IsFullTrustAssembly(this.m_fullTrustAssemblies, evidence))
                {
                    policy = new PolicyStatement(new PermissionSet(true), PolicyStatementAttribute.Nothing);
                    flag = true;
                }
                else
                {
                    ArrayList list = this.GenericResolve(evidence, out flag);
                    policy = new PolicyStatement {
                        PermissionSet = null
                    };
                    IEnumerator enumerator = list.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        PolicyStatement statement2 = ((CodeGroupStackFrame) enumerator.Current).policy;
                        if (statement2 != null)
                        {
                            policy.GetPermissionSetNoCopy().InplaceUnion(statement2.GetPermissionSetNoCopy());
                            policy.Attributes |= statement2.Attributes;
                            if (statement2.HasDependentEvidence)
                            {
                                foreach (IDelayEvaluatedEvidence evidence2 in statement2.DependentEvidence)
                                {
                                    evidence2.MarkUsed();
                                }
                            }
                        }
                    }
                }
                if (flag && (serializedEvidence != null))
                {
                    serializedEvidence = PolicyManager.MakeEvidenceArray(evidence, false);
                    this.Cache(count, serializedEvidence, policy);
                }
            }
            return policy;
        }

        public CodeGroup ResolveMatchingCodeGroups(Evidence evidence)
        {
            if (evidence == null)
            {
                throw new ArgumentNullException("evidence");
            }
            return this.RootCodeGroup.ResolveMatchingCodeGroups(evidence);
        }

        private void SetDefault()
        {
            lock (this)
            {
                string path = GetLocationFromType(this.m_type) + ".default";
                if (File.InternalExists(path))
                {
                    PolicyLevel level = new PolicyLevel(this.m_type, path);
                    this.m_rootCodeGroup = level.RootCodeGroup;
                    this.m_namedPermissionSets = (ArrayList) level.NamedPermissionSets;
                    this.m_fullTrustAssemblies = (ArrayList) level.FullTrustAssemblies;
                    this.m_loaded = true;
                }
                else
                {
                    this.m_namedPermissionSets = null;
                    this.m_rootCodeGroup = null;
                    this.m_permSetElement = null;
                    this.m_rootCodeGroup = (this.m_type == PolicyLevelType.Machine) ? this.CreateDefaultMachinePolicy() : this.CreateDefaultAllGroup();
                    this.SetFactoryPermissionSets();
                    this.SetDefaultFullTrustAssemblies();
                    this.m_loaded = true;
                }
            }
        }

        private void SetDefaultFullTrustAssemblies()
        {
            this.m_fullTrustAssemblies = new ArrayList();
            StrongNamePublicKeyBlob blob = new StrongNamePublicKeyBlob("00000000000000000400000000000000");
            for (int i = 0; i < EcmaFullTrustAssemblies.Length; i++)
            {
                StrongNameMembershipCondition condition = new StrongNameMembershipCondition(blob, EcmaFullTrustAssemblies[i], s_mscorlibVersion);
                this.m_fullTrustAssemblies.Add(condition);
            }
            StrongNamePublicKeyBlob blob2 = new StrongNamePublicKeyBlob("002400000480000094000000060200000024000052534131000400000100010007D1FA57C4AED9F0A32E84AA0FAEFD0DE9E8FD6AEC8F87FB03766C834C99921EB23BE79AD9D5DCC1DD9AD236132102900B723CF980957FC4E177108FC607774F29E8320E92EA05ECE4E821C0A5EFE8F1645C4C0C93C1AB99285D622CAA652C1DFAD63D745D6F2DE5F17E5EAF0FC4963D261C8A12436518206DC093344D5AD293");
            for (int j = 0; j < MicrosoftFullTrustAssemblies.Length; j++)
            {
                StrongNameMembershipCondition condition2 = new StrongNameMembershipCondition(blob2, MicrosoftFullTrustAssemblies[j], s_mscorlibVersion);
                this.m_fullTrustAssemblies.Add(condition2);
            }
        }

        private void SetFactoryPermissionSets()
        {
            lock (this)
            {
                this.m_namedPermissionSets = new ArrayList();
                string[] replaceStrings = new string[s_FactoryPolicySearchStrings.Length];
                replaceStrings[0] = "2.0.0.0";
                replaceStrings[1] = Environment.GetResourceString("Policy_PS_FullTrust");
                replaceStrings[2] = Environment.GetResourceString("Policy_PS_Everything");
                replaceStrings[3] = Environment.GetResourceString("Policy_PS_Nothing");
                replaceStrings[4] = Environment.GetResourceString("Policy_PS_SkipVerification");
                replaceStrings[5] = Environment.GetResourceString("Policy_PS_Execution");
                this.m_permSetElement = new Parser(PolicyLevelData.s_defaultPermissionSets, s_FactoryPolicySearchStrings, replaceStrings).GetTopElement();
                this.m_permSetElement.AddChild(GetInternetElement());
                this.m_permSetElement.AddChild(GetLocalIntranetElement());
            }
        }

        public SecurityElement ToXml()
        {
            this.CheckLoaded();
            this.LoadAllPermissionSets();
            SecurityElement element = new SecurityElement("PolicyLevel");
            element.AddAttribute("version", "1");
            Hashtable classes = new Hashtable();
            lock (this)
            {
                SecurityElement child = new SecurityElement("NamedPermissionSets");
                IEnumerator enumerator = this.m_namedPermissionSets.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    child.AddChild(this.NormalizeClassDeep(((NamedPermissionSet) enumerator.Current).ToXml(), classes));
                }
                SecurityElement element3 = this.NormalizeClassDeep(this.m_rootCodeGroup.ToXml(this), classes);
                SecurityElement element4 = new SecurityElement("FullTrustAssemblies");
                enumerator = this.m_fullTrustAssemblies.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    element4.AddChild(this.NormalizeClassDeep(((StrongNameMembershipCondition) enumerator.Current).ToXml(), classes));
                }
                SecurityElement element5 = new SecurityElement("SecurityClasses");
                IDictionaryEnumerator enumerator2 = classes.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    SecurityElement element6 = new SecurityElement("SecurityClass");
                    element6.AddAttribute("Name", (string) enumerator2.Value);
                    element6.AddAttribute("Description", (string) enumerator2.Key);
                    element5.AddChild(element6);
                }
                element.AddChild(element5);
                element.AddChild(child);
                element.AddChild(element3);
                element.AddChild(element4);
            }
            return element;
        }

        private SecurityElement UnnormalizeClass(SecurityElement elem, Hashtable classes)
        {
            if (((classes != null) && (elem.m_lAttributes != null)) && (elem.m_lAttributes.Count != 0))
            {
                int count = elem.m_lAttributes.Count;
                for (int i = 0; i < count; i += 2)
                {
                    string str = (string) elem.m_lAttributes[i];
                    if (str.Equals("class"))
                    {
                        string str2 = (string) elem.m_lAttributes[i + 1];
                        string str3 = (string) classes[str2];
                        if (str3 != null)
                        {
                            elem.m_lAttributes[i + 1] = str3;
                        }
                        return elem;
                    }
                }
            }
            return elem;
        }

        private SecurityElement UnnormalizeClassDeep(SecurityElement elem, Hashtable classes)
        {
            this.UnnormalizeClass(elem, classes);
            if ((elem.InternalChildren != null) && (elem.InternalChildren.Count > 0))
            {
                IEnumerator enumerator = elem.Children.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    this.UnnormalizeClassDeep((SecurityElement) enumerator.Current, classes);
                }
            }
            return elem;
        }

        internal System.Security.Policy.ConfigId ConfigId =>
            this.m_configId;

        [Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
        public IList FullTrustAssemblies
        {
            get
            {
                this.CheckLoaded();
                return new ArrayList(this.m_fullTrustAssemblies);
            }
        }

        private static object InternalSyncObject
        {
            get
            {
                if (s_InternalSyncObject == null)
                {
                    object obj2 = new object();
                    Interlocked.CompareExchange(ref s_InternalSyncObject, obj2, null);
                }
                return s_InternalSyncObject;
            }
        }

        public string Label
        {
            get
            {
                if (this.m_label == null)
                {
                    this.m_label = this.DeriveLabelFromType();
                }
                return this.m_label;
            }
        }

        public IList NamedPermissionSets
        {
            get
            {
                this.CheckLoaded();
                this.LoadAllPermissionSets();
                ArrayList list = new ArrayList(this.m_namedPermissionSets.Count);
                IEnumerator enumerator = this.m_namedPermissionSets.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    list.Add(((NamedPermissionSet) enumerator.Current).Copy());
                }
                return list;
            }
        }

        internal string Path =>
            this.m_path;

        public CodeGroup RootCodeGroup
        {
            get
            {
                this.CheckLoaded();
                return this.m_rootCodeGroup;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("RootCodeGroup");
                }
                this.CheckLoaded();
                this.m_rootCodeGroup = value.Copy();
            }
        }

        public string StoreLocation =>
            GetLocationFromType(this.m_type);

        [ComVisible(false)]
        public PolicyLevelType Type =>
            this.m_type;
    }
}

