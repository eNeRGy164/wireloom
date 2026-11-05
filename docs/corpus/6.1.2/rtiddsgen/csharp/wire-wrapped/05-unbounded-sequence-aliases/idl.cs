
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
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

namespace CorpusUnboundedSequences
{

    public class SequenceAliasesSample :  IEquatable<SequenceAliasesSample>
    {
        public CorpusUnboundedSequences.Numbers numbers { get; set; }

        public CorpusUnboundedSequences.States states { get; set; }

        public CorpusUnboundedSequences.Children children { get; set; }

        public CorpusUnboundedSequences.Scalars scalars { get; set; }

        public CorpusUnboundedSequences.Texts texts { get; set; }

        public CorpusUnboundedSequences.WideTexts wideTexts { get; set; }

        public CorpusUnboundedSequences.BoundedTexts boundedTexts { get; set; }

        public CorpusUnboundedSequences.BoundedWideTexts boundedWideTexts { get; set; }

        public SequenceAliasesSample()
        {
            numbers = new CorpusUnboundedSequences.Numbers();
            states = new CorpusUnboundedSequences.States();
            children = new CorpusUnboundedSequences.Children();
            scalars = new CorpusUnboundedSequences.Scalars();
            texts = new CorpusUnboundedSequences.Texts();
            wideTexts = new CorpusUnboundedSequences.WideTexts();
            boundedTexts = new CorpusUnboundedSequences.BoundedTexts();
            boundedWideTexts = new CorpusUnboundedSequences.BoundedWideTexts();
        }

        public SequenceAliasesSample(CorpusUnboundedSequences.Numbers  numbers, CorpusUnboundedSequences.States  states, CorpusUnboundedSequences.Children  children, CorpusUnboundedSequences.Scalars  scalars, CorpusUnboundedSequences.Texts  texts, CorpusUnboundedSequences.WideTexts  wideTexts, CorpusUnboundedSequences.BoundedTexts  boundedTexts, CorpusUnboundedSequences.BoundedWideTexts  boundedWideTexts)
        {
            this.numbers = numbers;
            this.states = states;
            this.children = children;
            this.scalars = scalars;
            this.texts = texts;
            this.wideTexts = wideTexts;
            this.boundedTexts = boundedTexts;
            this.boundedWideTexts = boundedWideTexts;
        }

        public SequenceAliasesSample(SequenceAliasesSample other)
        {
            if (other == null)
            {
                return;
            }

            this.numbers = other.numbers;
            this.states = other.states;
            this.children = other.children;
            this.scalars = other.scalars;
            this.texts = other.texts;
            this.wideTexts = other.wideTexts;
            this.boundedTexts = other.boundedTexts;
            this.boundedWideTexts = other.boundedWideTexts;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(numbers);
            hash.Add(states);
            hash.Add(children);
            hash.Add(scalars);
            hash.Add(texts);
            hash.Add(wideTexts);
            hash.Add(boundedTexts);
            hash.Add(boundedWideTexts);

            return hash.ToHashCode();
        }

        public bool Equals(SequenceAliasesSample other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.numbers.Equals(other.numbers) &&
            this.states.Equals(other.states) &&
            this.children.Equals(other.children) &&
            this.scalars.Equals(other.scalars) &&
            this.texts.Equals(other.texts) &&
            this.wideTexts.Equals(other.wideTexts) &&
            this.boundedTexts.Equals(other.boundedTexts) &&
            this.boundedWideTexts.Equals(other.boundedWideTexts);
        }

        public override bool Equals(object obj) => this.Equals(obj as SequenceAliasesSample);

        public override string ToString() => SequenceAliasesSampleSupport.Instance.ToString(this);
    }

} // namespace CorpusUnboundedSequences
