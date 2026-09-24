
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-union.idl
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

namespace CorpusUnionKeys
{

    public class Identity :  IEquatable<Identity>
    {

        private int _number;

        private string _text = string.Empty;

        public int Discriminator { get; private set; }

        public const int DefaultDiscriminator = 0;

        public int number
        {
            get
            {
                if (Discriminator != 0) 
                {
                    throw new InvalidOperationException("number not selected");
                }
                return _number;
            }

            set
            {
                _number = value;
                Discriminator = 0;
            }
        }

        [Bound(255)]
        public string text
        {
            get
            {
                if (Discriminator != 1) 
                {
                    throw new InvalidOperationException("text not selected");
                }
                return _text;
            }

            set
            {
                _text = value;
                Discriminator = 1;
            }
        }

        public Identity()
        {
            Discriminator = DefaultDiscriminator;
        }

        public Identity(Identity other)
        {
            if (other == null)
            {
                return;
            }

            this.Discriminator = other.Discriminator;
            switch (Discriminator)
            {
                case 0:
                this._number = other.number;
                break;
                case 1:
                this._text = other.text;
                break;
            }
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case 0:
                return number;
                case 1:
                return text;

                default:
                return null;
            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case 0:
                return HashCode.Combine(Discriminator, this.number);
                case 1:
                return HashCode.Combine(Discriminator, this.text);
            }
            return HashCode.Combine(Discriminator);
        }

        public bool Equals(Identity other)
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
                case 0:
                return this.number.Equals(other.number);
                case 1:
                return this.text.Equals(other.text);
            }
            return true;
        }

        public override bool Equals(object obj) => this.Equals(obj as Identity);

        public override string ToString() => IdentitySupport.Instance.ToString(this);
    }

    public class Sample :  IEquatable<Sample>
    {
        [Key]
        public global::CorpusUnionKeys.Identity identity { get; set; }
        public int payload { get; set; }

        public Sample()
        {
            identity = new global::CorpusUnionKeys.Identity();
        }

        public Sample(global::CorpusUnionKeys.Identity  identity, int  payload)
        {
            this.identity = identity;
            this.payload = payload;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.identity = new global::CorpusUnionKeys.Identity(other.identity);
            this.payload = other.payload;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.identity);
            hash.Add(this.payload);

            return hash.ToHashCode();
        }

        public bool Equals(Sample other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.identity.Equals(other.identity) && 
            this.payload.Equals(other.payload);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusUnionKeys
