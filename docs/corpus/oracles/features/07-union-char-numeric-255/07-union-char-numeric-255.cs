
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-char-numeric-255.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.1.
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

namespace CorpusNumericChar255Union
{

    public class Choice :  IEquatable<Choice>
    {

        private int _value;

        private int _fallback;

        public char Discriminator { get; private set; }

        public const char DefaultDiscriminator = (char) 0;

        public int value
        {
            get
            {
                if (Discriminator != 255)
                {
                    throw new InvalidOperationException("value not selected");
                }
                return _value;
            }

            set
            {
                _value = value;
                Discriminator = 255;
            }
        }

        public int fallback
        {
            get
            {
                if (Discriminator == 255)

                {
                    throw new InvalidOperationException("fallback not selected");
                }
                return _fallback;
            }

            set
            {
                _fallback = value;
                Discriminator = (char) 0;
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
                case 255:
                this._value = other.value;
                break;
                default:
                this._fallback = other.fallback;
                break;
            }
        }

        public void Setfallback(int  fallback, char discriminator)
        {
            if (discriminator == 255)

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
                case 255:
                return value;
                default:
                return fallback;

            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case 255:
                return HashCode.Combine(Discriminator, this.value);
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
                case 255:
                return this.value.Equals(other.value);
                default:
                return this.fallback.Equals(other.fallback);
            }
        }

        public override bool Equals(object obj) => this.Equals(obj as Choice);

        public override string ToString() => ChoiceSupport.Instance.ToString(this);
    }

} // namespace CorpusNumericChar255Union
