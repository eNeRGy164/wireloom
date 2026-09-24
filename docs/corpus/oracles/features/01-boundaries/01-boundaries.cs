
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 01-boundaries.idl
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

namespace CorpusPrimitiveBoundaries
{

    public static class shortMin
    {
        public const short Value = -32768;
    }

    public static class shortMax
    {
        public const short Value = 32767;
    }

    public static class longMin
    {
        public const int Value = -2147483648;
    }

    public static class longMax
    {
        public const int Value = 2147483647;
    }

    public static class longLongMin
    {
        public const int Value = -9223372036854775807-1;
    }

    public static class longLongMax
    {
        public const int Value = 9223372036854775807;
    }

    public static class int8Min
    {
        public const sbyte Value = -128;
    }

    public static class int8Max
    {
        public const sbyte Value = 127;
    }

    public static class int64Min
    {
        public const long Value = -9223372036854775807-1;
    }

    public static class int64Max
    {
        public const long Value = 9223372036854775807;
    }

    public class Sample :  IEquatable<Sample>
    {
        public short shortValue { get; set; }
        public int longValue { get; set; }
        public long longLongValue { get; set; }
        public sbyte int8Value { get; set; }
        public long int64Value { get; set; }
        public float floatValue { get; set; }
        public double doubleValue { get; set; }

        public Sample()
        {
        }

        public Sample(short  shortValue, int  longValue, long  longLongValue, sbyte  int8Value, long  int64Value, float  floatValue, double  doubleValue)
        {
            this.shortValue = shortValue;
            this.longValue = longValue;
            this.longLongValue = longLongValue;
            this.int8Value = int8Value;
            this.int64Value = int64Value;
            this.floatValue = floatValue;
            this.doubleValue = doubleValue;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.shortValue = other.shortValue;
            this.longValue = other.longValue;
            this.longLongValue = other.longLongValue;
            this.int8Value = other.int8Value;
            this.int64Value = other.int64Value;
            this.floatValue = other.floatValue;
            this.doubleValue = other.doubleValue;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.shortValue);
            hash.Add(this.longValue);
            hash.Add(this.longLongValue);
            hash.Add(this.int8Value);
            hash.Add(this.int64Value);
            hash.Add(this.floatValue);
            hash.Add(this.doubleValue);

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

            return this.shortValue.Equals(other.shortValue) && 
            this.longValue.Equals(other.longValue) && 
            this.longLongValue.Equals(other.longLongValue) && 
            this.int8Value.Equals(other.int8Value) && 
            this.int64Value.Equals(other.int64Value) && 
            this.floatValue.Equals(other.floatValue) && 
            this.doubleValue.Equals(other.doubleValue);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusPrimitiveBoundaries
