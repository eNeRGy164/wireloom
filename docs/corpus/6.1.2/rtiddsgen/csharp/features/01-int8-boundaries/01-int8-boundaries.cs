
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 01-int8-boundaries.idl
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

namespace CorpusInt8Boundaries
{
    public static class int8Min
    {
        public const byte Value = -128;
    }
    public static class int8Max
    {
        public const byte Value = 127;
    }

    public class Sample :  IEquatable<Sample>
    {
        public byte minimum { get; set; }

        public byte maximum { get; set; }

        public Sample()
        {
        }

        public Sample(byte  minimum, byte  maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.minimum = other.minimum;
            this.maximum = other.maximum;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(minimum);
            hash.Add(maximum);

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

            return this.minimum.Equals(other.minimum) &&
            this.maximum.Equals(other.maximum);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusInt8Boundaries
