
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-duplicate-label.idl
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

namespace CorpusNegativeDuplicateLabel
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
                if (Discriminator != 1) 
                {
                    throw new InvalidOperationException("first not selected");
                }
                return _first;
            }

            set
            {
                _first = value;
                Discriminator = 1;
            }
        }

        public int second
        {
            get
            {
                if (Discriminator != 1) 
                {
                    throw new InvalidOperationException("second not selected");
                }
                return _second;
            }

            set
            {
                _second = value;
                Discriminator = 1;
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
                this._first = other.first;
                break;
                case 1:
                this._second = other.second;
                break;
            }
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case 1:
                return first;
                case 1:
                return second;

                default:
                return null;
            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case 1:
                return HashCode.Combine(Discriminator, this.first);
                case 1:
                return HashCode.Combine(Discriminator, this.second);
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
                return this.first.Equals(other.first);
                case 1:
                return this.second.Equals(other.second);
            }
            return true;
        }

        public override bool Equals(object obj) => this.Equals(obj as Choice);

        public override string ToString() => ChoiceSupport.Instance.ToString(this);
    }

} // namespace CorpusNegativeDuplicateLabel
