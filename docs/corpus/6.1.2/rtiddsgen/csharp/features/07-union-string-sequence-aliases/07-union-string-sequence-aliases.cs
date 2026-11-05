
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-string-sequence-aliases.idl
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

namespace RuntimeUnionStringSequences
{

    public class NarrowTexts :  IEquatable<NarrowTexts>
    {

        [Bound(3)]
        public ISequence<string> Value { get; }

        public NarrowTexts()
        {
            Value = new Rti.Types.Sequence<string>();
        }

        public NarrowTexts(ISequence<string>Value)
        {
            this.Value = Value;
        }

        public NarrowTexts(NarrowTexts other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new Rti.Types.Sequence<string>(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(Value.Count);

            return hash.ToHashCode();
        }

        public bool Equals(NarrowTexts other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Value.SequenceEqual(other.Value);
        }

        public override bool Equals(object obj) => this.Equals(obj as NarrowTexts);

        public override string ToString() => NarrowTextsSupport.Instance.ToString(this);
    }

    public class WideTexts :  IEquatable<WideTexts>
    {

        [Bound(3)]
        public ISequence<string> Value { get; }

        public WideTexts()
        {
            Value = new Rti.Types.Sequence<string>();
        }

        public WideTexts(ISequence<string>Value)
        {
            this.Value = Value;
        }

        public WideTexts(WideTexts other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new Rti.Types.Sequence<string>(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(Value.Count);

            return hash.ToHashCode();
        }

        public bool Equals(WideTexts other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Value.SequenceEqual(other.Value);
        }

        public override bool Equals(object obj) => this.Equals(obj as WideTexts);

        public override string ToString() => WideTextsSupport.Instance.ToString(this);
    }

    public class BoundedNarrowTexts :  IEquatable<BoundedNarrowTexts>
    {

        [Bound(3)]
        public ISequence<string> Value { get; }

        public BoundedNarrowTexts()
        {
            Value = new Rti.Types.Sequence<string>();
        }

        public BoundedNarrowTexts(ISequence<string>Value)
        {
            this.Value = Value;
        }

        public BoundedNarrowTexts(BoundedNarrowTexts other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new Rti.Types.Sequence<string>(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(Value.Count);

            return hash.ToHashCode();
        }

        public bool Equals(BoundedNarrowTexts other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Value.SequenceEqual(other.Value);
        }

        public override bool Equals(object obj) => this.Equals(obj as BoundedNarrowTexts);

        public override string ToString() => BoundedNarrowTextsSupport.Instance.ToString(this);
    }

    public class BoundedWideTexts :  IEquatable<BoundedWideTexts>
    {

        [Bound(3)]
        public ISequence<string> Value { get; }

        public BoundedWideTexts()
        {
            Value = new Rti.Types.Sequence<string>();
        }

        public BoundedWideTexts(ISequence<string>Value)
        {
            this.Value = Value;
        }

        public BoundedWideTexts(BoundedWideTexts other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new Rti.Types.Sequence<string>(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(Value.Count);

            return hash.ToHashCode();
        }

        public bool Equals(BoundedWideTexts other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Value.SequenceEqual(other.Value);
        }

        public override bool Equals(object obj) => this.Equals(obj as BoundedWideTexts);

        public override string ToString() => BoundedWideTextsSupport.Instance.ToString(this);
    }

    public class DirectStringSequences :  IEquatable<DirectStringSequences>
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

        public DirectStringSequences()
        {
            Discriminator = DefaultDiscriminator;
            _narrow = new Rti.Types.Sequence<string>();
        }

        public DirectStringSequences(DirectStringSequences other)
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
                return HashCode.Combine(Discriminator, narrow.Count);
                case 1:
                return HashCode.Combine(Discriminator, wide.Count);
            }

            throw new InvalidOperationException("Invalid discriminator");
        }

        public bool Equals(DirectStringSequences other)
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
            throw new InvalidOperationException("Invalid discriminator");
        }

        public override bool Equals(object obj) => this.Equals(obj as DirectStringSequences);

        public override string ToString() => DirectStringSequencesSupport.Instance.ToString(this);
    }

    public class StringSequenceAliases :  IEquatable<StringSequenceAliases>
    {

        private RuntimeUnionStringSequences.NarrowTexts _narrow;

        private RuntimeUnionStringSequences.WideTexts _wide;

        private RuntimeUnionStringSequences.BoundedNarrowTexts _boundedNarrow;

        private RuntimeUnionStringSequences.BoundedWideTexts _boundedWide;

        public int Discriminator { get; private set; }

        public const int DefaultDiscriminator = 0;

        public RuntimeUnionStringSequences.NarrowTexts narrow
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

        public RuntimeUnionStringSequences.WideTexts wide
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

        public RuntimeUnionStringSequences.BoundedNarrowTexts boundedNarrow
        {
            get
            {
                if (Discriminator != 2) 
                {
                    throw new InvalidOperationException("boundedNarrow not selected");
                }
                return _boundedNarrow;
            }

            set
            {
                _boundedNarrow = value;
                Discriminator = 2;
            }
        }

        public RuntimeUnionStringSequences.BoundedWideTexts boundedWide
        {
            get
            {
                if (Discriminator != 3) 
                {
                    throw new InvalidOperationException("boundedWide not selected");
                }
                return _boundedWide;
            }

            set
            {
                _boundedWide = value;
                Discriminator = 3;
            }
        }

        public StringSequenceAliases()
        {
            Discriminator = DefaultDiscriminator;
            _narrow = new RuntimeUnionStringSequences.NarrowTexts();
        }

        public StringSequenceAliases(StringSequenceAliases other)
        {
            if (other == null)
            {
                return;
            }

            this.Discriminator = other.Discriminator;
            switch (Discriminator)
            {
                case 0:
                this._narrow = other.narrow;
                break;
                case 1:
                this._wide = other.wide;
                break;
                case 2:
                this._boundedNarrow = other.boundedNarrow;
                break;
                case 3:
                this._boundedWide = other.boundedWide;
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
                case 2:
                return boundedNarrow;
                case 3:
                return boundedWide;

                default:
                return null;
            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case 0:
                return HashCode.Combine(Discriminator, narrow);
                case 1:
                return HashCode.Combine(Discriminator, wide);
                case 2:
                return HashCode.Combine(Discriminator, boundedNarrow);
                case 3:
                return HashCode.Combine(Discriminator, boundedWide);
            }

            throw new InvalidOperationException("Invalid discriminator");
        }

        public bool Equals(StringSequenceAliases other)
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
                return this.narrow.Equals(other.narrow);
                case 1:
                return this.wide.Equals(other.wide);
                case 2:
                return this.boundedNarrow.Equals(other.boundedNarrow);
                case 3:
                return this.boundedWide.Equals(other.boundedWide);
            }
            throw new InvalidOperationException("Invalid discriminator");
        }

        public override bool Equals(object obj) => this.Equals(obj as StringSequenceAliases);

        public override string ToString() => StringSequenceAliasesSupport.Instance.ToString(this);
    }

} // namespace RuntimeUnionStringSequences
