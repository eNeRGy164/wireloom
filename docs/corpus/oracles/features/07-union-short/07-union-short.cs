
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-short.idl
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

namespace CorpusShortUnion
{

    public class Choice :  IEquatable<Choice>
    {

        private int _negative;

        private string _text = string.Empty;

        private bool _other;

        public short Discriminator { get; private set; }

        public const short DefaultDiscriminator = 0;

        public int negative
        {
            get
            {
                if (Discriminator != -1) 
                {
                    throw new InvalidOperationException("negative not selected");
                }
                return _negative;
            }

            set
            {
                _negative = value;
                Discriminator = -1;
            }
        }

        [Bound(255)]
        public string text
        {
            get
            {
                if (Discriminator != 2) 
                {
                    throw new InvalidOperationException("text not selected");
                }
                return _text;
            }

            set
            {
                _text = value;
                Discriminator = 2;
            }
        }

        public bool other
        {
            get
            {
                if (Discriminator == -1 || Discriminator == 2)

                {
                    throw new InvalidOperationException("other not selected");
                }
                return _other;
            }

            set
            {
                _other = value;
                Discriminator = 0;
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
                case -1:
                this._negative = other.negative;
                break;
                case 2:
                this._text = other.text;
                break;
                default:
                this._other = other.other;
                break;
            }
        }

        public void Setother(bool  other, short discriminator)
        {
            if (discriminator == -1 || discriminator == 2)

            {
                throw new ArgumentException("Invalid discriminator value for other", paramName: nameof(discriminator));
            }
            this._other = other;
            Discriminator = discriminator;
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case -1:
                return negative;
                case 2:
                return text;
                default:
                return other;

            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case -1:
                return HashCode.Combine(Discriminator, this.negative);
                case 2:
                return HashCode.Combine(Discriminator, this.text);
                default:
                return HashCode.Combine(Discriminator, this.other);
            }
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
                case -1:
                return this.negative.Equals(other.negative);
                case 2:
                return this.text.Equals(other.text);
                default:
                return this.other.Equals(other.other);
            }
        }

        public override bool Equals(object obj) => this.Equals(obj as Choice);

        public override string ToString() => ChoiceSupport.Instance.ToString(this);
    }

} // namespace CorpusShortUnion
