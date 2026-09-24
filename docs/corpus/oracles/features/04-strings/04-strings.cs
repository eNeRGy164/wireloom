
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-strings.idl
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

namespace CorpusStrings
{

    public class Sample :  IEquatable<Sample>
    {
        [Bound(255)]
        public string unbounded { get; set; } = string.Empty;
        [Bound(8)]
        public string bounded { get; set; } = string.Empty;
        [Bound(255)]
        public string wideUnbounded { get; set; } = string.Empty;
        [Bound(8)]
        public string wideBounded { get; set; } = string.Empty;

        public Sample()
        {
        }

        public Sample(string  unbounded, string  bounded, string  wideUnbounded, string  wideBounded)
        {
            this.unbounded = unbounded;
            this.bounded = bounded;
            this.wideUnbounded = wideUnbounded;
            this.wideBounded = wideBounded;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.unbounded = other.unbounded;
            this.bounded = other.bounded;
            this.wideUnbounded = other.wideUnbounded;
            this.wideBounded = other.wideBounded;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.unbounded);
            hash.Add(this.bounded);
            hash.Add(this.wideUnbounded);
            hash.Add(this.wideBounded);

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

            return this.unbounded.Equals(other.unbounded) && 
            this.bounded.Equals(other.bounded) && 
            this.wideUnbounded.Equals(other.wideUnbounded) && 
            this.wideBounded.Equals(other.wideBounded);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusStrings
