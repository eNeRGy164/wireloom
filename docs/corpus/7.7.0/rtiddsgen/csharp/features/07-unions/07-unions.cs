
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-unions.idl
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

namespace CorpusUnions
{

    public class Choice :  IEquatable<Choice>
    {

        private int _number;

        private string _text = string.Empty;

        private bool _flag;

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

        public bool flag
        {
            get
            {
                if (Discriminator == 0 || Discriminator == 1)

                {
                    throw new InvalidOperationException("flag not selected");
                }
                return _flag;
            }

            set
            {
                _flag = value;
                Discriminator = 2;
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
                case 0:
                this._number = other.number;
                break;
                case 1:
                this._text = other.text;
                break;
                default:
                this._flag = other.flag;
                break;
            }
        }

        public void Setflag(bool  flag, int discriminator)
        {
            if (discriminator == 0 || discriminator == 1)

            {
                throw new ArgumentException("Invalid discriminator value for flag", paramName: nameof(discriminator));
            }
            this._flag = flag;
            Discriminator = discriminator;
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
                return flag;

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
                default:
                return HashCode.Combine(Discriminator, this.flag);
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
                case 0:
                return this.number.Equals(other.number);
                case 1:
                return this.text.Equals(other.text);
                default:
                return this.flag.Equals(other.flag);
            }
        }

        public override bool Equals(object obj) => this.Equals(obj as Choice);

        public override string ToString() => ChoiceSupport.Instance.ToString(this);
    }

} // namespace CorpusUnions
