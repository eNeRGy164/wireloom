
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-char.idl
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

namespace CorpusCharUnion
{

    public class Choice :  IEquatable<Choice>
    {

        private int _letter;

        private string _text = string.Empty;

        private bool _other;

        public char Discriminator { get; private set; }

        public const char DefaultDiscriminator = (char) 0;

        public int letter
        {
            get
            {
                if (Discriminator != 'a') 
                {
                    throw new InvalidOperationException("letter not selected");
                }
                return _letter;
            }

            set
            {
                _letter = value;
                Discriminator = 'a';
            }
        }

        [Bound(255)]
        public string text
        {
            get
            {
                if (Discriminator != 'z') 
                {
                    throw new InvalidOperationException("text not selected");
                }
                return _text;
            }

            set
            {
                _text = value;
                Discriminator = 'z';
            }
        }

        public bool other
        {
            get
            {
                if (Discriminator == 'a' || Discriminator == 'z')

                {
                    throw new InvalidOperationException("other not selected");
                }
                return _other;
            }

            set
            {
                _other = value;
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
                case 'a':
                this._letter = other.letter;
                break;
                case 'z':
                this._text = other.text;
                break;
                default:
                this._other = other.other;
                break;
            }
        }

        public void Setother(bool  other, char discriminator)
        {
            if (discriminator == 'a' || discriminator == 'z')

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
                case 'a':
                return letter;
                case 'z':
                return text;
                default:
                return other;

            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case 'a':
                return HashCode.Combine(Discriminator, this.letter);
                case 'z':
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
                case 'a':
                return this.letter.Equals(other.letter);
                case 'z':
                return this.text.Equals(other.text);
                default:
                return this.other.Equals(other.other);
            }
        }

        public override bool Equals(object obj) => this.Equals(obj as Choice);

        public override string ToString() => ChoiceSupport.Instance.ToString(this);
    }

} // namespace CorpusCharUnion
