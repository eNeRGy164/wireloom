
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-unbounded-support.idl
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

namespace CorpusUnboundedSupport
{

    public class LongValues :  IEquatable<LongValues>
    {

        public ISequence<int> Value { get; }

        public LongValues()
        {
            Value = new Rti.Types.Sequence<int>();
        }

        public LongValues(ISequence<int>Value)
        {
            this.Value = Value;
        }

        public LongValues(LongValues other)
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

        public bool Equals(LongValues other)
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

        public override bool Equals(object obj) => this.Equals(obj as LongValues);

        public override string ToString() => LongValuesSupport.Instance.ToString(this);
    }

    public class Sample :  IEquatable<Sample>
    {
        public ISequence<int> values { get; }
        [Bound(-1)]
        public string text { get; set; } = string.Empty;
        [Bound(-1)]
        public string wideText { get; set; } = string.Empty;
        public ISequence<string> labels { get; }
        public global::CorpusUnboundedSupport.LongValues aliasValues { get; set; }
        [Bound(4)]
        public ISequence<int> boundedValues { get; }
        [Bound(8)]
        public string boundedText { get; set; } = string.Empty;

        public Sample()
        {
            values = new Rti.Types.Sequence<int>();
            labels = new Rti.Types.Sequence<string>();
            aliasValues = new global::CorpusUnboundedSupport.LongValues();
            boundedValues = new Rti.Types.Sequence<int>();
        }

        public Sample(ISequence<int>values, string  text, string  wideText, ISequence<string>labels, global::CorpusUnboundedSupport.LongValues  aliasValues, ISequence<int>boundedValues, string  boundedText)
        {
            this.values = values;
            this.text = text;
            this.wideText = wideText;
            this.labels = labels;
            this.aliasValues = aliasValues;
            this.boundedValues = boundedValues;
            this.boundedText = boundedText;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.values = new Rti.Types.Sequence<int>(other.values);
            this.text = other.text;
            this.wideText = other.wideText;
            this.labels = new Rti.Types.Sequence<string>(other.labels);
            this.aliasValues = new global::CorpusUnboundedSupport.LongValues(other.aliasValues);
            this.boundedValues = new Rti.Types.Sequence<int>(other.boundedValues);
            this.boundedText = other.boundedText;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.values.Count);
            hash.Add(this.text);
            hash.Add(this.wideText);
            hash.Add(this.labels.Count);
            hash.Add(this.aliasValues);
            hash.Add(this.boundedValues.Count);
            hash.Add(this.boundedText);

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
            this.text.Equals(other.text) && 
            this.wideText.Equals(other.wideText) && 
            this.labels.SequenceEqual(other.labels) && 
            this.aliasValues.Equals(other.aliasValues) && 
            this.boundedValues.SequenceEqual(other.boundedValues) && 
            this.boundedText.Equals(other.boundedText);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusUnboundedSupport
