
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-alias-collections.idl
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

namespace CorpusCollectionAliases
{

    public class LongSequence :  IEquatable<LongSequence>
    {

        [Bound(3)]
        public ISequence<int> Value { get; }

        public LongSequence()
        {
            Value = new Rti.Types.Sequence<int>();
        }

        public LongSequence(ISequence<int>Value)
        {
            this.Value = Value;
        }

        public LongSequence(LongSequence other)
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

            hash.Add(Value.Count);

            return hash.ToHashCode();
        }

        public bool Equals(LongSequence other)
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

        public override bool Equals(object obj) => this.Equals(obj as LongSequence);

        public override string ToString() => LongSequenceSupport.Instance.ToString(this);
    }

    public class NestedSequence :  IEquatable<NestedSequence>
    {

        [Bound(2)]
        public ISequence<CorpusCollectionAliases.LongSequence> Value { get; }

        public NestedSequence()
        {
            Value = new Rti.Types.Sequence<CorpusCollectionAliases.LongSequence>();
        }

        public NestedSequence(ISequence<CorpusCollectionAliases.LongSequence>Value)
        {
            this.Value = Value;
        }

        public NestedSequence(NestedSequence other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new Rti.Types.Sequence<CorpusCollectionAliases.LongSequence>(other.Value.Select(element => new CorpusCollectionAliases.LongSequence(element)));

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(Value.Count);

            return hash.ToHashCode();
        }

        public bool Equals(NestedSequence other)
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

        public override bool Equals(object obj) => this.Equals(obj as NestedSequence);

        public override string ToString() => NestedSequenceSupport.Instance.ToString(this);
    }

    public class Sample :  IEquatable<Sample>
    {
        public CorpusCollectionAliases.NestedSequence values { get; set; }

        public Sample()
        {
            values = new CorpusCollectionAliases.NestedSequence();
        }

        public Sample(CorpusCollectionAliases.NestedSequence  values)
        {
            this.values = values;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.values = other.values;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(values);

            return hash.ToHashCode();
        }

        public bool Equals(Sample other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.values.Equals(other.values);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusCollectionAliases
