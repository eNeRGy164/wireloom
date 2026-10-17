
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 05-array-of-sequences.idl
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

namespace CorpusArrayOfSequences
{

    public class Sample :  IEquatable<Sample>
    {
        [Bound(100)]
        public ISequence<int> values { get; }
        [Bound(3)]
        public ISequence<string> names { get; }

        public Sample()
        {
            values = new Rti.Types.Sequence<int>();
            names = new Rti.Types.Sequence<string>();
        }

        public Sample(ISequence<int>values, ISequence<string>names)
        {
            this.values = values;
            this.names = names;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.values = new Rti.Types.Sequence<int>(other.values);
            this.names = new Rti.Types.Sequence<string>(other.names);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.values.Count);
            hash.Add(this.names.Count);

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

            return this.values.SequenceEqual(other.values) && 
            this.names.SequenceEqual(other.names);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusArrayOfSequences
