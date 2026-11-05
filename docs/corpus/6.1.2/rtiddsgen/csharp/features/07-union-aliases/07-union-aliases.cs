
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-aliases.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
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

        public CorpusUnionAliases.Kind Discriminator { get; private set; }

        public const CorpusUnionAliases.Kind DefaultDiscriminator = CorpusUnionAliases.Kind.Zero;

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
                return HashCode.Combine(Discriminator, zero);
                case CorpusUnionAliases.Kind.Ten:
                return HashCode.Combine(Discriminator, ten);
            }

            throw new InvalidOperationException("Invalid discriminator");
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
            throw new InvalidOperationException("Invalid discriminator");
        }

        public override bool Equals(object obj) => this.Equals(obj as Choice);

        public override string ToString() => ChoiceSupport.Instance.ToString(this);
    }

    public class ChoiceAlias :  IEquatable<ChoiceAlias>
    {

        public CorpusUnionAliases.Choice Value { get; set; }

        public ChoiceAlias()
        {
            Value = new CorpusUnionAliases.Choice();
        }

        public ChoiceAlias(CorpusUnionAliases.Choice  Value)
        {
            this.Value = Value;
        }

        public ChoiceAlias(ChoiceAlias other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new CorpusUnionAliases.Choice(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(Value);

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

        public CorpusUnionAliases.Choice Value { get; set; }

        public ChoiceAlias2()
        {
            Value = new CorpusUnionAliases.Choice();
        }

        public ChoiceAlias2(CorpusUnionAliases.Choice  Value)
        {
            this.Value = Value;
        }

        public ChoiceAlias2(ChoiceAlias2 other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new CorpusUnionAliases.Choice(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(Value);

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
        public CorpusUnionAliases.Choice value { get; set; }

        public Holder()
        {
            value = new CorpusUnionAliases.Choice();
        }

        public Holder(CorpusUnionAliases.Choice  value)
        {
            this.value = value;
        }

        public Holder(Holder other)
        {
            if (other == null)
            {
                return;
            }

            this.value = new CorpusUnionAliases.Choice(other.value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(value);

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
