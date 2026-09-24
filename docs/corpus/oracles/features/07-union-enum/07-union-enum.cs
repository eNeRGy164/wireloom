
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-enum.idl
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

namespace CorpusEnumUnion
{

    public enum Kind
    {
        Number,
        Text,
        PayloadValue
    }

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

        private int _number;

        private string _text = string.Empty;

        private global::CorpusEnumUnion.Payload _payload;

        public global::CorpusEnumUnion.Kind Discriminator { get; private set; }

        public const global::CorpusEnumUnion.Kind DefaultDiscriminator = (global::CorpusEnumUnion.Kind) CorpusEnumUnion.Kind.Number;

        public int number
        {
            get
            {
                if (Discriminator != CorpusEnumUnion.Kind.Number) 
                {
                    throw new InvalidOperationException("number not selected");
                }
                return _number;
            }

            set
            {
                _number = value;
                Discriminator = CorpusEnumUnion.Kind.Number;
            }
        }

        [Bound(255)]
        public string text
        {
            get
            {
                if (Discriminator != CorpusEnumUnion.Kind.Text) 
                {
                    throw new InvalidOperationException("text not selected");
                }
                return _text;
            }

            set
            {
                _text = value;
                Discriminator = CorpusEnumUnion.Kind.Text;
            }
        }

        public global::CorpusEnumUnion.Payload payload
        {
            get
            {
                if (Discriminator != CorpusEnumUnion.Kind.PayloadValue) 
                {
                    throw new InvalidOperationException("payload not selected");
                }
                return _payload;
            }

            set
            {
                _payload = value;
                Discriminator = CorpusEnumUnion.Kind.PayloadValue;
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
                case CorpusEnumUnion.Kind.Number:
                this._number = other.number;
                break;
                case CorpusEnumUnion.Kind.Text:
                this._text = other.text;
                break;
                case CorpusEnumUnion.Kind.PayloadValue:
                this._payload = new global::CorpusEnumUnion.Payload(other.payload);
                break;
            }
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case CorpusEnumUnion.Kind.Number:
                return number;
                case CorpusEnumUnion.Kind.Text:
                return text;
                case CorpusEnumUnion.Kind.PayloadValue:
                return payload;

                default:
                return null;
            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case CorpusEnumUnion.Kind.Number:
                return HashCode.Combine(Discriminator, this.number);
                case CorpusEnumUnion.Kind.Text:
                return HashCode.Combine(Discriminator, this.text);
                case CorpusEnumUnion.Kind.PayloadValue:
                return HashCode.Combine(Discriminator, this.payload);
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
                case CorpusEnumUnion.Kind.Number:
                return this.number.Equals(other.number);
                case CorpusEnumUnion.Kind.Text:
                return this.text.Equals(other.text);
                case CorpusEnumUnion.Kind.PayloadValue:
                return this.payload.Equals(other.payload);
            }
            return true;
        }

        public override bool Equals(object obj) => this.Equals(obj as Choice);

        public override string ToString() => ChoiceSupport.Instance.ToString(this);
    }

} // namespace CorpusEnumUnion
