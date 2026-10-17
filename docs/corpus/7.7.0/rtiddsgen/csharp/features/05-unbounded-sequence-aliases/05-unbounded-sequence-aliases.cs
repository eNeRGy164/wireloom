
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 05-unbounded-sequence-aliases.idl
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

namespace CorpusUnboundedSequences
{

    public class Numbers :  IEquatable<Numbers>
    {

        [Bound(100)]
        public ISequence<int> Value { get; }

        public Numbers()
        {
            Value = new Rti.Types.Sequence<int>();
        }

        public Numbers(ISequence<int>Value)
        {
            this.Value = Value;
        }

        public Numbers(Numbers other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new Rti.Types.Sequence<int>(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.Value.Count);

            return hash.ToHashCode();
        }

        public bool Equals(Numbers other)
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

        public override bool Equals(object obj) => this.Equals(obj as Numbers);

        public override string ToString() => NumbersSupport.Instance.ToString(this);
    }

    public enum State
    {
        Idle,
        Ready
    }

    public class Child :  IEquatable<Child>
    {
        public int value { get; set; }

        public Child()
        {
        }

        public Child(int  value)
        {
            this.value = value;
        }

        public Child(Child other)
        {
            if (other == null)
            {
                return;
            }

            this.value = other.value;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.value);

            return hash.ToHashCode();
        }

        public bool Equals(Child other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.value.Equals(other.value);
        }

        public override bool Equals(object obj) => this.Equals(obj as Child);

        public override string ToString() => ChildSupport.Instance.ToString(this);
    }

    public class Scalar :  IEquatable<Scalar>
    {

        public int Value { get; set; }

        public Scalar()
        {
        }

        public Scalar(int  Value)
        {
            this.Value = Value;
        }

        public Scalar(Scalar other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = other.Value;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.Value);

            return hash.ToHashCode();
        }

        public bool Equals(Scalar other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Value.Equals(other.Value);
        }

        public override bool Equals(object obj) => this.Equals(obj as Scalar);

        public override string ToString() => ScalarSupport.Instance.ToString(this);
    }

    public class States :  IEquatable<States>
    {

        [Bound(100)]
        public ISequence<global::CorpusUnboundedSequences.State> Value { get; }

        public States()
        {
            Value = new Rti.Types.Sequence<global::CorpusUnboundedSequences.State>();
        }

        public States(ISequence<global::CorpusUnboundedSequences.State>Value)
        {
            this.Value = Value;
        }

        public States(States other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new Rti.Types.Sequence<global::CorpusUnboundedSequences.State>(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.Value.Count);

            return hash.ToHashCode();
        }

        public bool Equals(States other)
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

        public override bool Equals(object obj) => this.Equals(obj as States);

        public override string ToString() => StatesSupport.Instance.ToString(this);
    }

    public class Children :  IEquatable<Children>
    {

        [Bound(100)]
        public ISequence<global::CorpusUnboundedSequences.Child> Value { get; }

        public Children()
        {
            Value = new Rti.Types.Sequence<global::CorpusUnboundedSequences.Child>();
        }

        public Children(ISequence<global::CorpusUnboundedSequences.Child>Value)
        {
            this.Value = Value;
        }

        public Children(Children other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new Rti.Types.Sequence<global::CorpusUnboundedSequences.Child>(other.Value.Select(element => new global::CorpusUnboundedSequences.Child(element)));

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.Value.Count);

            return hash.ToHashCode();
        }

        public bool Equals(Children other)
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

        public override bool Equals(object obj) => this.Equals(obj as Children);

        public override string ToString() => ChildrenSupport.Instance.ToString(this);
    }

    public class Scalars :  IEquatable<Scalars>
    {

        [Bound(100)]
        public ISequence<int> Value { get; }

        public Scalars()
        {
            Value = new Rti.Types.Sequence<int>();
        }

        public Scalars(ISequence<int>Value)
        {
            this.Value = Value;
        }

        public Scalars(Scalars other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new Rti.Types.Sequence<int>(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.Value.Count);

            return hash.ToHashCode();
        }

        public bool Equals(Scalars other)
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

        public override bool Equals(object obj) => this.Equals(obj as Scalars);

        public override string ToString() => ScalarsSupport.Instance.ToString(this);
    }

    public class Texts :  IEquatable<Texts>
    {

        [Bound(100)]
        public ISequence<string> Value { get; }

        public Texts()
        {
            Value = new Rti.Types.Sequence<string>();
        }

        public Texts(ISequence<string>Value)
        {
            this.Value = Value;
        }

        public Texts(Texts other)
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

            hash.Add(this.Value.Count);

            return hash.ToHashCode();
        }

        public bool Equals(Texts other)
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

        public override bool Equals(object obj) => this.Equals(obj as Texts);

        public override string ToString() => TextsSupport.Instance.ToString(this);
    }

    public class WideTexts :  IEquatable<WideTexts>
    {

        [Bound(100)]
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

            hash.Add(this.Value.Count);

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

    public class BoundedTexts :  IEquatable<BoundedTexts>
    {

        [Bound(100)]
        public ISequence<string> Value { get; }

        public BoundedTexts()
        {
            Value = new Rti.Types.Sequence<string>();
        }

        public BoundedTexts(ISequence<string>Value)
        {
            this.Value = Value;
        }

        public BoundedTexts(BoundedTexts other)
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

            hash.Add(this.Value.Count);

            return hash.ToHashCode();
        }

        public bool Equals(BoundedTexts other)
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

        public override bool Equals(object obj) => this.Equals(obj as BoundedTexts);

        public override string ToString() => BoundedTextsSupport.Instance.ToString(this);
    }

    public class BoundedWideTexts :  IEquatable<BoundedWideTexts>
    {

        [Bound(100)]
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

            hash.Add(this.Value.Count);

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

    public class NumbersChain :  IEquatable<NumbersChain>
    {

        public global::CorpusUnboundedSequences.Numbers Value { get; set; }

        public NumbersChain()
        {
            Value = new global::CorpusUnboundedSequences.Numbers();
        }

        public NumbersChain(global::CorpusUnboundedSequences.Numbers  Value)
        {
            this.Value = Value;
        }

        public NumbersChain(NumbersChain other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new global::CorpusUnboundedSequences.Numbers(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.Value);

            return hash.ToHashCode();
        }

        public bool Equals(NumbersChain other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Value.Equals(other.Value);
        }

        public override bool Equals(object obj) => this.Equals(obj as NumbersChain);

        public override string ToString() => NumbersChainSupport.Instance.ToString(this);
    }

} // namespace CorpusUnboundedSequences
