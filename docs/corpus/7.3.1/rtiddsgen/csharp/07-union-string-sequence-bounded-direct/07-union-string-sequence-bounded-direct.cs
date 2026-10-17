
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-string-sequence-bounded-direct.idl
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

namespace RuntimeUnionBoundedStringSequences
{

    public class DirectBoundedStringSequences :  IEquatable<DirectBoundedStringSequences>
    {

        private ISequence<string> _narrow;

        private ISequence<string> _wide;

        public int Discriminator { get; private set; }

        public const int DefaultDiscriminator = 0;

        [Bound(3)]
        public ISequence<string> narrow
        {
            get
            {
                if (Discriminator != 0) 
                {
                    throw new InvalidOperationException("narrow not selected");
                }
                return _narrow;
            }

            set
            {
                _narrow = value;
                Discriminator = 0;
            }
        }

        [Bound(3)]
        public ISequence<string> wide
        {
            get
            {
                if (Discriminator != 1) 
                {
                    throw new InvalidOperationException("wide not selected");
                }
                return _wide;
            }

            set
            {
                _wide = value;
                Discriminator = 1;
            }
        }

        public DirectBoundedStringSequences()
        {
            Discriminator = DefaultDiscriminator;

            _narrow = new Rti.Types.Sequence<string>();
        }

        public DirectBoundedStringSequences(DirectBoundedStringSequences other)
        {
            if (other == null)
            {
                return;
            }

            this.Discriminator = other.Discriminator;
            switch (Discriminator)
            {
                case 0:
                this._narrow = new Rti.Types.Sequence<string>(other.narrow);
                break;
                case 1:
                this._wide = new Rti.Types.Sequence<string>(other.wide);
                break;
            }
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case 0:
                return narrow;
                case 1:
                return wide;

                default:
                return null;
            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case 0:
                return HashCode.Combine(Discriminator, this.narrow.Count);
                case 1:
                return HashCode.Combine(Discriminator, this.wide.Count);
            }
            return HashCode.Combine(Discriminator);
        }

        public bool Equals(DirectBoundedStringSequences other)
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
                return this.narrow.SequenceEqual(other.narrow);
                case 1:
                return this.wide.SequenceEqual(other.wide);
            }
            return true;
        }

        public override bool Equals(object obj) => this.Equals(obj as DirectBoundedStringSequences);

        public override string ToString() => DirectBoundedStringSequencesSupport.Instance.ToString(this);
    }

} // namespace RuntimeUnionBoundedStringSequences
