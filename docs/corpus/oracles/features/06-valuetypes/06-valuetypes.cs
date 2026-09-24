
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 06-valuetypes.idl
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

namespace CorpusValueTypes
{

    public class BaseValue :  IEquatable<BaseValue>
    {
        public int baseValue { get; set; }

        public BaseValue()
        {
        }

        public BaseValue(int  baseValue)
        {
            this.baseValue = baseValue;
        }

        public BaseValue(BaseValue other)
        {
            if (other == null)
            {
                return;
            }

            this.baseValue = other.baseValue;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.baseValue);

            return hash.ToHashCode();
        }

        public bool Equals(BaseValue other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.baseValue.Equals(other.baseValue);
        }

        public override bool Equals(object obj) => this.Equals(obj as BaseValue);

        public override string ToString() => BaseValueSupport.Instance.ToString(this);
    }

    public class DerivedValue :  global::CorpusValueTypes.BaseValue, IEquatable<DerivedValue>
    {
        [Bound(16)]
        public string name { get; set; } = string.Empty;

        public DerivedValue()
        {
        }

        public DerivedValue(int  baseValue, string  name) : base(baseValue)
        {
            this.name = name;
        }

        public DerivedValue(DerivedValue other) : base(other)
        {
            if (other == null)
            {
                return;
            }

            this.name = other.name;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(base.GetHashCode());

            hash.Add(this.name);

            return hash.ToHashCode();
        }

        public bool Equals(DerivedValue other)
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

            return this.name.Equals(other.name);
        }

        public override bool Equals(object obj) => this.Equals(obj as DerivedValue);

        public override string ToString() => DerivedValueSupport.Instance.ToString(this);
    }

    public class Holder :  IEquatable<Holder>
    {
        public global::CorpusValueTypes.DerivedValue value { get; set; }

        public Holder()
        {
            value = new global::CorpusValueTypes.DerivedValue();
        }

        public Holder(global::CorpusValueTypes.DerivedValue  value)
        {
            this.value = value;
        }

        public Holder(Holder other)
        {
            if (other == null)
            {
                return;
            }

            this.value = new global::CorpusValueTypes.DerivedValue(other.value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.value);

            return hash.ToHashCode();
        }

        public bool Equals(Holder other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.value.Equals(other.value);
        }

        public override bool Equals(object obj) => this.Equals(obj as Holder);

        public override string ToString() => HolderSupport.Instance.ToString(this);
    }

} // namespace CorpusValueTypes
