
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-defaults-ranges.idl
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

namespace CorpusDefaultsAndRanges
{

    public enum Color
    {
        GREEN,
        RED,
        BLUE
    }

    public class Sample :  IEquatable<Sample>
    {
        public int value{
            get
            {
                return this._value;
            }

            set
            {
                if(value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{value} is lower than the minimum (0)");
                }
                if(value > 100)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{value} is higher than the maximum (100)");
                }
                this._value = value;
            }
        }

        private int _value;

        public int ranged{
            get
            {
                return this._ranged;
            }

            set
            {
                if(value < -32)
                {
                    throw new ArgumentOutOfRangeException(nameof(ranged), $"{value} is lower than the minimum (-32)");
                }
                if(value > 31)
                {
                    throw new ArgumentOutOfRangeException(nameof(ranged), $"{value} is higher than the maximum (31)");
                }
                this._ranged = value;
            }
        }

        private int _ranged;

        public global::CorpusDefaultsAndRanges.Color color { get; set; }

        public Sample()
        {
            value = (int) (50);
            color = (global::CorpusDefaultsAndRanges.Color) (2);
        }

        public Sample(int  value, int  ranged, global::CorpusDefaultsAndRanges.Color  color)
        {
            this._value = value;
            this._ranged = ranged;
            this.color = color;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this._value = other.value;
            this._ranged = other.ranged;
            this.color = other.color;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.value);
            hash.Add(this.ranged);
            hash.Add(this.color);

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

            return this.value.Equals(other.value) && 
            this.ranged.Equals(other.ranged) && 
            this.color.Equals(other.color);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusDefaultsAndRanges
