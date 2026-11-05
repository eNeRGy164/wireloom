
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

namespace RuntimeUnionStringSequences
{

    public class StringSequenceUnionsSample :  IEquatable<StringSequenceUnionsSample>
    {
        public RuntimeUnionStringSequences.DirectStringSequences direct { get; set; }

        public RuntimeUnionStringSequences.StringSequenceAliases aliases { get; set; }

        public StringSequenceUnionsSample()
        {
            direct = new RuntimeUnionStringSequences.DirectStringSequences();
            aliases = new RuntimeUnionStringSequences.StringSequenceAliases();
        }

        public StringSequenceUnionsSample(RuntimeUnionStringSequences.DirectStringSequences  direct, RuntimeUnionStringSequences.StringSequenceAliases  aliases)
        {
            this.direct = direct;
            this.aliases = aliases;
        }

        public StringSequenceUnionsSample(StringSequenceUnionsSample other)
        {
            if (other == null)
            {
                return;
            }

            this.direct = new RuntimeUnionStringSequences.DirectStringSequences(other.direct);
            this.aliases = new RuntimeUnionStringSequences.StringSequenceAliases(other.aliases);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(direct);
            hash.Add(aliases);

            return hash.ToHashCode();
        }

        public bool Equals(StringSequenceUnionsSample other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.direct.Equals(other.direct) &&
            this.aliases.Equals(other.aliases);
        }

        public override bool Equals(object obj) => this.Equals(obj as StringSequenceUnionsSample);

        public override string ToString() => StringSequenceUnionsSampleSupport.Instance.ToString(this);
    }

} // namespace RuntimeUnionStringSequences
