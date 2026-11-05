
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-duplicate-default.idl
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

namespace CorpusNegativeDuplicateDefault
{

    public class Choice :  IEquatable<Choice>
    {

        private int _first;

        private int _second;

        public int Discriminator { get; private set; }

        public const int DefaultDiscriminator = 0;

        public int first
        {
            get
            {
                if ()

                {
                    throw new InvalidOperationException("first not selected");
                }
                return _first;
            }

            set
            {
                _first = value;
                Discriminator = DefaultDiscriminator;
            }
        }

        public int second
        {
            get
            {
                if ()

                {
                    throw new InvalidOperationException("second not selected");
                }
                return _second;
            }

            set
            {
                _second = value;
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
                default:
                this._first = other.first;
                break;
                default:
                this._second = other.second;
                break;
            }
        }

        public void Setfirst(int  first, int discriminator)
        {
            if ()

            {
                throw new ArgumentException("Invalid discriminator value for first", paramName: nameof(discriminator));
            }
            this._first = first;
            Discriminator = discriminator;
        }
        public void Setsecond(int  second, int discriminator)
        {
            if ()

            {
                throw new ArgumentException("Invalid discriminator value for second", paramName: nameof(discriminator));
            }
            this._second = second;
            Discriminator = discriminator;
        }

        public object Get()
        {
            switch (Discriminator)
            {
                default:
                return first;
                default:
                return second;

            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                default:
                return HashCode.Combine(Discriminator, first);
                default:
                return HashCode.Combine(Discriminator, second);
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
                default:
                return this.first.Equals(other.first);
                default:
                return this.second.Equals(other.second);
            }
            throw new InvalidOperationException("Invalid discriminator");
        }

        public override bool Equals(object obj) => this.Equals(obj as Choice);

        public override string ToString() => ChoiceSupport.Instance.ToString(this);
    }

} // namespace CorpusNegativeDuplicateDefault
