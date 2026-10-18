
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-int32.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
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

namespace CorpusUnionInt32
{

    public class Choice :  IEquatable<Choice>
    {

        private int _minimum;

        private int _maximum;

        private int _fallback;

        public int Discriminator { get; private set; }

        public const int DefaultDiscriminator = 0;

        public int minimum
        {
            get
            {
                if (Discriminator != -2147483648) 
                {
                    throw new InvalidOperationException("minimum not selected");
                }
                return _minimum;
            }

            set
            {
                _minimum = value;
                Discriminator = -2147483648;
            }
        }

        public int maximum
        {
            get
            {
                if (Discriminator != 2147483647) 
                {
                    throw new InvalidOperationException("maximum not selected");
                }
                return _maximum;
            }

            set
            {
                _maximum = value;
                Discriminator = 2147483647;
            }
        }

        public int fallback
        {
            get
            {
                if (Discriminator == -2147483648 || Discriminator == 2147483647)

                {
                    throw new InvalidOperationException("fallback not selected");
                }
                return _fallback;
            }

            set
            {
                _fallback = value;
                Discriminator = DefaultDiscriminator;
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
                case -2147483648:
                this._minimum = other.minimum;
                break;
                case 2147483647:
                this._maximum = other.maximum;
                break;
                default:
                this._fallback = other.fallback;
                break;
            }
        }

        public void Setfallback(int  fallback, int discriminator)
        {
            if (discriminator == -2147483648 || discriminator == 2147483647)

            {
                throw new ArgumentException("Invalid discriminator value for fallback", paramName: nameof(discriminator));
            }
            this._fallback = fallback;
            Discriminator = discriminator;
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case -2147483648:
                return minimum;
                case 2147483647:
                return maximum;
                default:
                return fallback;

            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case -2147483648:
                return HashCode.Combine(Discriminator, this.minimum);
                case 2147483647:
                return HashCode.Combine(Discriminator, this.maximum);
                default:
                return HashCode.Combine(Discriminator, this.fallback);
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
                case -2147483648:
                return this.minimum.Equals(other.minimum);
                case 2147483647:
                return this.maximum.Equals(other.maximum);
                default:
                return this.fallback.Equals(other.fallback);
            }
        }

        public override bool Equals(object obj) => this.Equals(obj as Choice);

        public override string ToString() => ChoiceSupport.Instance.ToString(this);
    }

} // namespace CorpusUnionInt32
