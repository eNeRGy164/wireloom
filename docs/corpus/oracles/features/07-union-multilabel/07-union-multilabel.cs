
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-multilabel.idl
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

namespace CorpusMultiLabelUnion
{

    public class Choice :  IEquatable<Choice>
    {

        private int _number;

        private string _text = string.Empty;

        public int Discriminator { get; private set; }

        public const int DefaultDiscriminator = 0;

        public int number
        {
            get
            {
                if (Discriminator != 1 && Discriminator != 5) 
                {
                    throw new InvalidOperationException("number not selected");
                }
                return _number;
            }

            set
            {
                _number = value;
                Discriminator = 1;
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
                case 1:
                case 5:
                this._number = other.number;
                break;
                case 2:
                this._text = other.text;
                break;
            }
        }

        public void Setnumber(int  number, int discriminator)
        {
            if (discriminator != 1 && discriminator != 5) 
            {
                throw new ArgumentException("Invalid discriminator value for number", paramName: nameof(discriminator));
            }
            this._number = number;
            Discriminator = discriminator;
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case 1:
                case 5:
                return number;
                case 2:
                return text;

                default:
                return null;
            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case 1:
                case 5:
                return HashCode.Combine(Discriminator, this.number);
                case 2:
                return HashCode.Combine(Discriminator, this.text);
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
                case 1:
                case 5:
                return this.number.Equals(other.number);
                case 2:
                return this.text.Equals(other.text);
            }
            return true;
        }

        public override bool Equals(object obj) => this.Equals(obj as Choice);

        public override string ToString() => ChoiceSupport.Instance.ToString(this);
    }

} // namespace CorpusMultiLabelUnion
