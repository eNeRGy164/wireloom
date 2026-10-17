
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-boundaries.idl
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

namespace CorpusStringBoundaries
{

    public class Sample :  IEquatable<Sample>
    {
        [Bound(8)]
        public string narrow { get; set; } = string.Empty;
        [Bound(8)]
        public string wide { get; set; } = string.Empty;

        public Sample()
        {
        }

        public Sample(string  narrow, string  wide)
        {
            this.narrow = narrow;
            this.wide = wide;
        }

        public Sample(Sample other)
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

            return this.narrow.Equals(other.narrow) && 
            this.wide.Equals(other.wide);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusStringBoundaries
