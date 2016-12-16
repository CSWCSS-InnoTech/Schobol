namespace System.Security
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    [Serializable]
    internal sealed class PermissionSetTriple
    {
        internal PermissionSet AssertSet;
        internal PermissionSet GrantSet;
        internal PermissionSet RefusedSet;
        private static RuntimeMethodHandle s_emptyRMH = new RuntimeMethodHandle(null);
        private static PermissionToken s_urlToken;
        private static PermissionToken s_zoneToken;

        internal PermissionSetTriple()
        {
            this.Reset();
        }

        internal PermissionSetTriple(PermissionSetTriple triple)
        {
            this.AssertSet = triple.AssertSet;
            this.GrantSet = triple.GrantSet;
            this.RefusedSet = triple.RefusedSet;
        }

        private static bool CheckAssert(PermissionSet pSet, CodeAccessPermission demand, PermissionToken permToken)
        {
            if (pSet != null)
            {
                pSet.CheckDecoded(demand, permToken);
                CodeAccessPermission asserted = (CodeAccessPermission) pSet.GetPermission(demand);
                try
                {
                    if ((pSet.IsUnrestricted() && demand.CanUnrestrictedOverride()) || demand.CheckAssert(asserted))
                    {
                        return false;
                    }
                }
                catch (ArgumentException)
                {
                }
            }
            return true;
        }

        private static bool CheckAssert(PermissionSet assertPset, PermissionSet demandSet, out PermissionSet newDemandSet)
        {
            newDemandSet = null;
            if (assertPset != null)
            {
                assertPset.CheckDecoded(demandSet);
                if (demandSet.CheckAssertion(assertPset))
                {
                    return false;
                }
                PermissionSet.RemoveAssertedPermissionSet(demandSet, assertPset, out newDemandSet);
            }
            return true;
        }

        internal bool CheckDemand(CodeAccessPermission demand, PermissionToken permToken, RuntimeMethodHandle rmh)
        {
            if (!CheckAssert(this.AssertSet, demand, permToken))
            {
                return false;
            }
            CodeAccessSecurityEngine.CheckHelper(this.GrantSet, this.RefusedSet, demand, permToken, rmh, null, SecurityAction.Demand, true);
            return true;
        }

        internal bool CheckDemandNoThrow(CodeAccessPermission demand, PermissionToken permToken) => 
            CodeAccessSecurityEngine.CheckHelper(this.GrantSet, this.RefusedSet, demand, permToken, s_emptyRMH, null, SecurityAction.Demand, false);

        internal bool CheckFlags(ref int flags)
        {
            if (this.AssertSet != null)
            {
                int specialFlags = SecurityManager.GetSpecialFlags(this.AssertSet, null);
                if ((flags & specialFlags) != 0)
                {
                    flags &= ~specialFlags;
                }
            }
            return ((SecurityManager.GetSpecialFlags(this.GrantSet, this.RefusedSet) & flags) == flags);
        }

        internal bool CheckSetDemand(PermissionSet demandSet, out PermissionSet alteredDemandset, RuntimeMethodHandle rmh)
        {
            alteredDemandset = null;
            if (!CheckAssert(this.AssertSet, demandSet, out alteredDemandset))
            {
                return false;
            }
            if (alteredDemandset != null)
            {
                demandSet = alteredDemandset;
            }
            CodeAccessSecurityEngine.CheckSetHelper(this.GrantSet, this.RefusedSet, demandSet, rmh, null, SecurityAction.Demand, true);
            return true;
        }

        internal bool CheckSetDemandNoThrow(PermissionSet demandSet) => 
            CodeAccessSecurityEngine.CheckSetHelper(this.GrantSet, this.RefusedSet, demandSet, s_emptyRMH, null, SecurityAction.Demand, false);

        internal bool IsEmpty() => 
            (((this.AssertSet == null) && (this.GrantSet == null)) && (this.RefusedSet == null));

        internal void Reset()
        {
            this.AssertSet = null;
            this.GrantSet = null;
            this.RefusedSet = null;
        }

        internal bool Update(PermissionSetTriple psTriple, out PermissionSetTriple retTriple)
        {
            retTriple = null;
            retTriple = this.UpdateAssert(psTriple.AssertSet);
            if ((psTriple.AssertSet != null) && psTriple.AssertSet.IsUnrestricted())
            {
                return true;
            }
            this.UpdateGrant(psTriple.GrantSet);
            this.UpdateRefused(psTriple.RefusedSet);
            return false;
        }

        internal PermissionSetTriple UpdateAssert(PermissionSet in_a)
        {
            PermissionSetTriple triple = null;
            if (in_a != null)
            {
                PermissionSet set;
                if (in_a.IsSubsetOf(this.AssertSet))
                {
                    return null;
                }
                if (this.GrantSet != null)
                {
                    set = in_a.Intersect(this.GrantSet);
                }
                else
                {
                    this.GrantSet = new PermissionSet(true);
                    set = in_a.Copy();
                }
                bool bFailedToCompress = false;
                if (this.RefusedSet != null)
                {
                    set = PermissionSet.RemoveRefusedPermissionSet(set, this.RefusedSet, out bFailedToCompress);
                }
                if (!bFailedToCompress)
                {
                    bFailedToCompress = PermissionSet.IsIntersectingAssertedPermissions(set, this.AssertSet);
                }
                if (bFailedToCompress)
                {
                    triple = new PermissionSetTriple(this);
                    this.Reset();
                    this.GrantSet = triple.GrantSet.Copy();
                }
                if (this.AssertSet == null)
                {
                    this.AssertSet = set;
                    return triple;
                }
                this.AssertSet.InplaceUnion(set);
            }
            return triple;
        }

        internal void UpdateGrant(PermissionSet in_g)
        {
            if (in_g != null)
            {
                if (this.GrantSet == null)
                {
                    this.GrantSet = in_g.Copy();
                }
                else
                {
                    this.GrantSet.InplaceIntersect(in_g);
                }
            }
        }

        internal void UpdateGrant(PermissionSet in_g, out ZoneIdentityPermission z, out UrlIdentityPermission u)
        {
            z = null;
            u = null;
            if (in_g != null)
            {
                if (this.GrantSet == null)
                {
                    this.GrantSet = in_g.Copy();
                }
                else
                {
                    this.GrantSet.InplaceIntersect(in_g);
                }
                z = (ZoneIdentityPermission) in_g.GetPermission(this.ZoneToken);
                u = (UrlIdentityPermission) in_g.GetPermission(this.UrlToken);
            }
        }

        internal void UpdateRefused(PermissionSet in_r)
        {
            if (in_r != null)
            {
                if (this.RefusedSet == null)
                {
                    this.RefusedSet = in_r.Copy();
                }
                else
                {
                    this.RefusedSet.InplaceUnion(in_r);
                }
            }
        }

        private PermissionToken UrlToken
        {
            get
            {
                if (s_urlToken == null)
                {
                    s_urlToken = PermissionToken.GetToken(typeof(UrlIdentityPermission));
                }
                return s_urlToken;
            }
        }

        private PermissionToken ZoneToken
        {
            get
            {
                if (s_zoneToken == null)
                {
                    s_zoneToken = PermissionToken.GetToken(typeof(ZoneIdentityPermission));
                }
                return s_zoneToken;
            }
        }
    }
}

