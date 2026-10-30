
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-unbounded-receive-lifecycle.idl
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

namespace CorpusUnboundedReceiveLifecycle
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

    public class OptionalState :  IEquatable<OptionalState>
    {
        public int value { get; set; }
        [Optional]
        [Bound(-1)]
        public string note { get; set; } = null;
        public ISequence<string> labels { get; }

        public OptionalState()
        {
            labels = new Rti.Types.Sequence<string>();
        }

        public OptionalState(int  value, string  note, ISequence<string>labels)
        {
            this.value = value;
            this.note = note;
            this.labels = labels;
        }

        public OptionalState(OptionalState other)
        {
            if (other == null)
            {
                return;
            }

            this.value = other.value;
            this.note = other.note;
            this.labels = new Rti.Types.Sequence<string>(other.labels);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.value);
            hash.Add(this.note);
            hash.Add(this.labels.Count);

            return hash.ToHashCode();
        }

        public bool Equals(OptionalState other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.value.Equals(other.value) && 
            Equals(this.note, other.note) && 
            this.labels.SequenceEqual(other.labels);
        }

        public override bool Equals(object obj) => this.Equals(obj as OptionalState);

        public override string ToString() => OptionalStateSupport.Instance.ToString(this);
    }

    public class Sample :  IEquatable<Sample>
    {
        public int phaseId { get; set; }
        public ISequence<int> values { get; }
        [Bound(-1)]
        public string text { get; set; } = string.Empty;
        [Bound(-1)]
        public string wideText { get; set; } = string.Empty;
        public ISequence<string> labels { get; }
        public global::CorpusUnboundedReceiveLifecycle.LongValues aliasValues { get; set; }
        [Bound(4)]
        public ISequence<int> boundedValues { get; }
        [Bound(8)]
        public string boundedText { get; set; } = string.Empty;
        [Optional]
        public global::CorpusUnboundedReceiveLifecycle.OptionalState optionalState { get; set; }

        public Sample()
        {
            values = new Rti.Types.Sequence<int>();
            labels = new Rti.Types.Sequence<string>();
            aliasValues = new global::CorpusUnboundedReceiveLifecycle.LongValues();
            boundedValues = new Rti.Types.Sequence<int>();
        }

        public Sample(int  phaseId, ISequence<int>values, string  text, string  wideText, ISequence<string>labels, global::CorpusUnboundedReceiveLifecycle.LongValues  aliasValues, ISequence<int>boundedValues, string  boundedText, global::CorpusUnboundedReceiveLifecycle.OptionalState  optionalState)
        {
            this.phaseId = phaseId;
            this.values = values;
            this.text = text;
            this.wideText = wideText;
            this.labels = labels;
            this.aliasValues = aliasValues;
            this.boundedValues = boundedValues;
            this.boundedText = boundedText;
            this.optionalState = optionalState;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.phaseId = other.phaseId;
            this.values = new Rti.Types.Sequence<int>(other.values);
            this.text = other.text;
            this.wideText = other.wideText;
            this.labels = new Rti.Types.Sequence<string>(other.labels);
            this.aliasValues = new global::CorpusUnboundedReceiveLifecycle.LongValues(other.aliasValues);
            this.boundedValues = new Rti.Types.Sequence<int>(other.boundedValues);
            this.boundedText = other.boundedText;
            if(other.optionalState != null)
            {
                this.optionalState = new global::CorpusUnboundedReceiveLifecycle.OptionalState(other.optionalState);
            }

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.phaseId);
            hash.Add(this.values.Count);
            hash.Add(this.text);
            hash.Add(this.wideText);
            hash.Add(this.labels.Count);
            hash.Add(this.aliasValues);
            hash.Add(this.boundedValues.Count);
            hash.Add(this.boundedText);
            hash.Add(this.optionalState);

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

            return this.phaseId.Equals(other.phaseId) && 
            this.values.SequenceEqual(other.values) && 
            this.text.Equals(other.text) && 
            this.wideText.Equals(other.wideText) && 
            this.labels.SequenceEqual(other.labels) && 
            this.aliasValues.Equals(other.aliasValues) && 
            this.boundedValues.SequenceEqual(other.boundedValues) && 
            this.boundedText.Equals(other.boundedText) && 
            Equals(this.optionalState, other.optionalState);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusUnboundedReceiveLifecycle
