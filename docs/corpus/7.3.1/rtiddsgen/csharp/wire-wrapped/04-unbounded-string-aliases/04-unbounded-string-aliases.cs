
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-unbounded-string-aliases.idl
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

namespace CorpusUnboundedStrings
{

    public class StringAliasesSample :  IEquatable<StringAliasesSample>
    {
        [Bound(255)]
        public string narrow { get; set; } = string.Empty;
        [Bound(255)]
        public string wide { get; set; } = string.Empty;

        public StringAliasesSample()
        {
        }

        public StringAliasesSample(string  narrow, string  wide)
        {
            this.narrow = narrow;
            this.wide = wide;
        }

        public StringAliasesSample(StringAliasesSample other)
        {
            if (other == null)
            {
                return;
            }

            this.narrow = other.narrow;
            this.wide = other.wide;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.narrow);
            hash.Add(this.wide);

            return hash.ToHashCode();
        }

        public bool Equals(StringAliasesSample other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.narrow.Equals(other.narrow) && 
            this.wide.Equals(other.wide);
        }

        public override bool Equals(object obj) => this.Equals(obj as StringAliasesSample);

        public override string ToString() => StringAliasesSampleSupport.Instance.ToString(this);
    }

} // namespace CorpusUnboundedStrings
