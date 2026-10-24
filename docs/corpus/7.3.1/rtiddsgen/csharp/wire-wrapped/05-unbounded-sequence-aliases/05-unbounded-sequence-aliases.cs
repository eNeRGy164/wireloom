
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 05-unbounded-sequence-aliases.idl
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

namespace CorpusUnboundedSequences
{

    public class SequenceAliasesSample :  IEquatable<SequenceAliasesSample>
    {
        public global::CorpusUnboundedSequences.Numbers numbers { get; set; }
        public global::CorpusUnboundedSequences.States states { get; set; }
        public global::CorpusUnboundedSequences.Children children { get; set; }
        public global::CorpusUnboundedSequences.Scalars scalars { get; set; }
        public global::CorpusUnboundedSequences.Texts texts { get; set; }
        public global::CorpusUnboundedSequences.WideTexts wideTexts { get; set; }
        public global::CorpusUnboundedSequences.BoundedTexts boundedTexts { get; set; }
        public global::CorpusUnboundedSequences.BoundedWideTexts boundedWideTexts { get; set; }

        public SequenceAliasesSample()
        {
            numbers = new global::CorpusUnboundedSequences.Numbers();
            states = new global::CorpusUnboundedSequences.States();
            children = new global::CorpusUnboundedSequences.Children();
            scalars = new global::CorpusUnboundedSequences.Scalars();
            texts = new global::CorpusUnboundedSequences.Texts();
            wideTexts = new global::CorpusUnboundedSequences.WideTexts();
            boundedTexts = new global::CorpusUnboundedSequences.BoundedTexts();
            boundedWideTexts = new global::CorpusUnboundedSequences.BoundedWideTexts();
        }

        public SequenceAliasesSample(global::CorpusUnboundedSequences.Numbers  numbers, global::CorpusUnboundedSequences.States  states, global::CorpusUnboundedSequences.Children  children, global::CorpusUnboundedSequences.Scalars  scalars, global::CorpusUnboundedSequences.Texts  texts, global::CorpusUnboundedSequences.WideTexts  wideTexts, global::CorpusUnboundedSequences.BoundedTexts  boundedTexts, global::CorpusUnboundedSequences.BoundedWideTexts  boundedWideTexts)
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

            this.numbers = new global::CorpusUnboundedSequences.Numbers(other.numbers);
            this.states = new global::CorpusUnboundedSequences.States(other.states);
            this.children = new global::CorpusUnboundedSequences.Children(other.children);
            this.scalars = new global::CorpusUnboundedSequences.Scalars(other.scalars);
            this.texts = new global::CorpusUnboundedSequences.Texts(other.texts);
            this.wideTexts = new global::CorpusUnboundedSequences.WideTexts(other.wideTexts);
            this.boundedTexts = new global::CorpusUnboundedSequences.BoundedTexts(other.boundedTexts);
            this.boundedWideTexts = new global::CorpusUnboundedSequences.BoundedWideTexts(other.boundedWideTexts);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.numbers);
            hash.Add(this.states);
            hash.Add(this.children);
            hash.Add(this.scalars);
            hash.Add(this.texts);
            hash.Add(this.wideTexts);
            hash.Add(this.boundedTexts);
            hash.Add(this.boundedWideTexts);

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
