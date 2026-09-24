
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-inherited.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

using System;
using System.Reflection;
using System.Collections.Generic;
using Rti.Types;
using System.Linq;
using Omg.Types;

namespace CorpusInheritedKeys
{

    public class Base :  IEquatable<Base>
    {
        [Key]
        public int tenant { get; set; }
        public int baseValue { get; set; }

        public Base()
        {
        }

        public Base(int  tenant, int  baseValue)
        {
            this.tenant = tenant;
            this.baseValue = baseValue;
        }

        public Base(Base other)
        {
            if (other == null)
            {
                return;
            }

            this.tenant = other.tenant;
            this.baseValue = other.baseValue;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.tenant);
            hash.Add(this.baseValue);

            return hash.ToHashCode();
        }

        public bool Equals(Base other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.tenant.Equals(other.tenant) && 
            this.baseValue.Equals(other.baseValue);
        }

        public override bool Equals(object obj) => this.Equals(obj as Base);

        public override string ToString() => BaseSupport.Instance.ToString(this);
    }

    public class Derived :  global::CorpusInheritedKeys.Base, IEquatable<Derived>
    {
        [Key]
        public int localId { get; set; }
        public int payload { get; set; }

        public Derived()
        {
        }

        public Derived(int  tenant, int  baseValue, int  localId, int  payload) : base(tenant, baseValue)
        {
            this.localId = localId;
            this.payload = payload;
        }

        public Derived(Derived other) : base(other)
        {
            if (other == null)
            {
                return;
            }

            this.localId = other.localId;
            this.payload = other.payload;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(base.GetHashCode());

            hash.Add(this.localId);
            hash.Add(this.payload);

            return hash.ToHashCode();
        }

        public bool Equals(Derived other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if(!base.Equals(other))
            {
                return false;
            }

            return this.localId.Equals(other.localId) && 
            this.payload.Equals(other.payload);
        }

        public override bool Equals(object obj) => this.Equals(obj as Derived);

        public override string ToString() => DerivedSupport.Instance.ToString(this);
    }

} // namespace CorpusInheritedKeys
