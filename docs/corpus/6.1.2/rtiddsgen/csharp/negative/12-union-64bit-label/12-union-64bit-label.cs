
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-union-64bit-label.idl
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

namespace CorpusNegativeWideUnionLabel
{

    public class Choice :  IEquatable<Choice>
    {

        private int _value;

        public long Discriminator { get; private set; }

        public const long DefaultDiscriminator = 4294967296;

        public int value
        {
            get
            {
                if (Discriminator != 4294967296) 
                {
                    throw new InvalidOperationException("value not selected");
                }
                return _value;
            }

            set
            {
                _value = value;
                Discriminator = 4294967296;
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
                case 4294967296:
                this._value = other.value;
                break;
            }
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case 4294967296:
                return value;

                default:
                return null;
            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case 4294967296:
                return HashCode.Combine(Discriminator, value);
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
                case 4294967296:
                return this.value.Equals(other.value);
            }
            throw new InvalidOperationException("Invalid discriminator");
        }

        public override bool Equals(object obj) => this.Equals(obj as Choice);

        public override string ToString() => ChoiceSupport.Instance.ToString(this);
    }

} // namespace CorpusNegativeWideUnionLabel
