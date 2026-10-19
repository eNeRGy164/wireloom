
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-resolve-name-sequence-bound.idl
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

namespace CorpusResolveNameSequenceBound
{

    public class Sample :  IEquatable<Sample>
    {
        [Bound((SEQUENCE_BOUND.Value))]
        public ISequence<int> values { get; }

        public Sample()
        {
            values = new Rti.Types.Sequence<int>();
        }

        public Sample(ISequence<int>values)
        {
            this.values = values;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.values = new Rti.Types.Sequence<int>(other.values);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.values.Count);

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

            return this.values.SequenceEqual(other.values);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusResolveNameSequenceBound
