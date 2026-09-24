
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-aliases.idl
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

namespace CorpusUnionAliases
{

    public enum Kind
    {
        Zero = 0,
        Ten = 10
    }

    public class Choice :  IEquatable<Choice>
    {

        private int _zero;

        private string _ten = string.Empty;

        public global::CorpusUnionAliases.Kind Discriminator { get; private set; }

        public const global::CorpusUnionAliases.Kind DefaultDiscriminator = (global::CorpusUnionAliases.Kind) CorpusUnionAliases.Kind.Zero;

        public int zero
        {
            get
            {
                if (Discriminator != CorpusUnionAliases.Kind.Zero) 
                {
                    throw new InvalidOperationException("zero not selected");
                }
                return _zero;
            }

            set
            {
                _zero = value;
                Discriminator = CorpusUnionAliases.Kind.Zero;
            }
        }

        [Bound(255)]
        public string ten
        {
            get
            {
                if (Discriminator != CorpusUnionAliases.Kind.Ten) 
                {
                    throw new InvalidOperationException("ten not selected");
                }
                return _ten;
            }

            set
            {
                _ten = value;
                Discriminator = CorpusUnionAliases.Kind.Ten;
            }
        }

        public Choice()
        {
            Discriminator = DefaultDiscriminator;
        }

        public Choice(Choice other)
        {
            if (other == null)
            {
                return;
            }

            this.Discriminator = other.Discriminator;
            switch (Discriminator)
            {
                case CorpusUnionAliases.Kind.Zero:
                this._zero = other.zero;
                break;
                case CorpusUnionAliases.Kind.Ten:
                this._ten = other.ten;
                break;
            }
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case CorpusUnionAliases.Kind.Zero:
                return zero;
                case CorpusUnionAliases.Kind.Ten:
                return ten;

                default:
                return null;
            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case CorpusUnionAliases.Kind.Zero:
                return HashCode.Combine(Discriminator, this.zero);
                case CorpusUnionAliases.Kind.Ten:
                return HashCode.Combine(Discriminator, this.ten);
            }
            return HashCode.Combine(Discriminator);
        }

        public bool Equals(Choice other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.Discriminator != other.Discriminator)
            {
                return false;
            }

            switch (Discriminator)
            {
                case CorpusUnionAliases.Kind.Zero:
                return this.zero.Equals(other.zero);
                case CorpusUnionAliases.Kind.Ten:
                return this.ten.Equals(other.ten);
            }
            return true;
        }

        public override bool Equals(object obj) => this.Equals(obj as Choice);

        public override string ToString() => ChoiceSupport.Instance.ToString(this);
    }

    public class ChoiceAlias :  IEquatable<ChoiceAlias>
    {

        public global::CorpusUnionAliases.Choice Value { get; set; }

        public ChoiceAlias()
        {
            Value = new global::CorpusUnionAliases.Choice();
        }

        public ChoiceAlias(global::CorpusUnionAliases.Choice  Value)
        {
            this.Value = Value;
        }

        public ChoiceAlias(ChoiceAlias other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new global::CorpusUnionAliases.Choice(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.Value);

            return hash.ToHashCode();
        }

        public bool Equals(ChoiceAlias other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Value.Equals(other.Value);
        }

        public override bool Equals(object obj) => this.Equals(obj as ChoiceAlias);

        public override string ToString() => ChoiceAliasSupport.Instance.ToString(this);
    }

    public class ChoiceAlias2 :  IEquatable<ChoiceAlias2>
    {

        public global::CorpusUnionAliases.Choice Value { get; set; }

        public ChoiceAlias2()
        {
            Value = new global::CorpusUnionAliases.Choice();
        }

        public ChoiceAlias2(global::CorpusUnionAliases.Choice  Value)
        {
            this.Value = Value;
        }

        public ChoiceAlias2(ChoiceAlias2 other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new global::CorpusUnionAliases.Choice(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.Value);

            return hash.ToHashCode();
        }

        public bool Equals(ChoiceAlias2 other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Value.Equals(other.Value);
        }

        public override bool Equals(object obj) => this.Equals(obj as ChoiceAlias2);

        public override string ToString() => ChoiceAlias2Support.Instance.ToString(this);
    }

    public class Holder :  IEquatable<Holder>
    {
        public global::CorpusUnionAliases.Choice value { get; set; }

        public Holder()
        {
            value = new global::CorpusUnionAliases.Choice();
        }

        public Holder(global::CorpusUnionAliases.Choice  value)
        {
            this.value = value;
        }

        public Holder(Holder other)
        {
            if (other == null)
            {
                return;
            }

            this.value = new global::CorpusUnionAliases.Choice(other.value);

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

} // namespace CorpusUnionAliases
