
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 06-alias-composition.idl
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

namespace CorpusAggregateComposition
{

    public class Identifier :  IEquatable<Identifier>
    {

        public int Value { get; set; }

        public Identifier()
        {
        }

        public Identifier(int  Value)
        {
            this.Value = Value;
        }

        public Identifier(Identifier other)
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

            hash.Add(Value);

            return hash.ToHashCode();
        }

        public bool Equals(Identifier other)
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

        public override bool Equals(object obj) => this.Equals(obj as Identifier);

        public override string ToString() => IdentifierSupport.Instance.ToString(this);
    }

    public enum State
    {
        idle,
        run
    }
    public static class Max
    {
        public const int Value = 4;
    }

    public class Sample :  IEquatable<Sample>
    {
        public int id { get; set; }

        public CorpusAggregateComposition.State state { get; set; }

        [Bound((CorpusAggregateComposition.Max.Value))]
        public ISequence<float> values { get; }

        public Sample()
        {
            state = (CorpusAggregateComposition.State) 0;
            values = new Rti.Types.Sequence<float>();
        }

        public Sample(int  id, CorpusAggregateComposition.State  state, ISequence<float>values)
        {
            this.id = id;
            this.state = state;
            this.values = values;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.id = other.id;
            this.state = other.state;
            this.values = new Rti.Types.Sequence<float>(other.values);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(id);
            hash.Add(state);
            hash.Add(values.Count);

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

            return this.id.Equals(other.id) &&
            this.state.Equals(other.state) &&
            this.values.SequenceEqual(other.values);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

    public class Named :  IEquatable<Named>
    {
        public CorpusAggregateComposition.Sample sample { get; set; }

        public Named()
        {
            sample = new CorpusAggregateComposition.Sample();
        }

        public Named(CorpusAggregateComposition.Sample  sample)
        {
            this.sample = sample;
        }

        public Named(Named other)
        {
            if (other == null)
            {
                return;
            }

            this.sample = new CorpusAggregateComposition.Sample(other.sample);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(sample);

            return hash.ToHashCode();
        }

        public bool Equals(Named other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.sample.Equals(other.sample);
        }

        public override bool Equals(object obj) => this.Equals(obj as Named);

        public override string ToString() => NamedSupport.Instance.ToString(this);
    }

} // namespace CorpusAggregateComposition
