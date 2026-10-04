
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-optional-string-sequences.idl
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

namespace CorpusOptionalStringSequences
{

    public class Sample :  IEquatable<Sample>
    {
        [Optional]
        [Bound(4)]
        public ISequence<string> narrowValues { get; set; }
        [Optional]
        [Bound(4)]
        public ISequence<string> wideValues { get; set; }

        public Sample()
        {
        }

        public Sample(ISequence<string>narrowValues, ISequence<string>wideValues)
        {
            this.narrowValues = narrowValues;
            this.wideValues = wideValues;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            if(other.narrowValues != null)
            {
                this.narrowValues = new Rti.Types.Sequence<string>(other.narrowValues);
            }
            if(other.wideValues != null)
            {
                this.wideValues = new Rti.Types.Sequence<string>(other.wideValues);
            }

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            if (this.narrowValues != null)
            {
                hash.Add(this.narrowValues.Count);
            }
            else
            {
                hash.Add(-1);
            }
            if (this.wideValues != null)
            {
                hash.Add(this.wideValues.Count);
            }
            else
            {
                hash.Add(-1);
            }

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

            return (ReferenceEquals(this.narrowValues, other.narrowValues) || (this.narrowValues != null && other.narrowValues != null && this.narrowValues.SequenceEqual(other.narrowValues))) &&
            (ReferenceEquals(this.wideValues, other.wideValues) || (this.wideValues != null && other.wideValues != null && this.wideValues.SequenceEqual(other.wideValues)));
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusOptionalStringSequences
