
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-scoped.idl
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

namespace CorpusScopedUnion
{

    public class Payload :  IEquatable<Payload>
    {
        public int code { get; set; }
        [Bound(16)]
        public string label { get; set; } = string.Empty;

        public Payload()
        {
        }

        public Payload(int  code, string  label)
        {
            this.code = code;
            this.label = label;
        }

        public Payload(Payload other)
        {
            if (other == null)
            {
                return;
            }

            this.code = other.code;
            this.label = other.label;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.code);
            hash.Add(this.label);

            return hash.ToHashCode();
        }

        public bool Equals(Payload other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.code.Equals(other.code) && 
            this.label.Equals(other.label);
        }

        public override bool Equals(object obj) => this.Equals(obj as Payload);

        public override string ToString() => PayloadSupport.Instance.ToString(this);
    }

    public class Choice :  IEquatable<Choice>
    {

        private global::CorpusScopedUnion.Payload _qualifiedPayload;

        private ISequence<int> _values;

        public int Discriminator { get; private set; }

        public const int DefaultDiscriminator = 0;

        public global::CorpusScopedUnion.Payload qualifiedPayload
        {
            get
            {
                if (Discriminator != 10) 
                {
                    throw new InvalidOperationException("qualifiedPayload not selected");
                }
                return _qualifiedPayload;
            }

            set
            {
                _qualifiedPayload = value;
                Discriminator = 10;
            }
        }

        [Bound(4)]
        public ISequence<int> values
        {
            get
            {
                if (Discriminator != 11) 
                {
                    throw new InvalidOperationException("values not selected");
                }
                return _values;
            }

            set
            {
                _values = value;
                Discriminator = 11;
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
                case 10:
                this._qualifiedPayload = new global::CorpusScopedUnion.Payload(other.qualifiedPayload);
                break;
                case 11:
                this._values = new Rti.Types.Sequence<int>(other.values);
                break;
            }
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case 10:
                return qualifiedPayload;
                case 11:
                return values;

                default:
                return null;
            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case 10:
                return HashCode.Combine(Discriminator, this.qualifiedPayload);
                case 11:
                return HashCode.Combine(Discriminator, this.values.Count);
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
                case 10:
                return this.qualifiedPayload.Equals(other.qualifiedPayload);
                case 11:
                return this.values.SequenceEqual(other.values);
            }
            return true;
        }

        public override bool Equals(object obj) => this.Equals(obj as Choice);

        public override string ToString() => ChoiceSupport.Instance.ToString(this);
    }

} // namespace CorpusScopedUnion
