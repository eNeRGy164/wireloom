
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-annotations-rti.idl
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

namespace CorpusRtiAnnotations
{

    public class Sample :  IEquatable<Sample>
    {
        [Key]
        public int id { get; set; }
        public int value{
            get
            {
                return this._value;
            }

            set
            {
                if(value < -32)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{value} is lower than the minimum (-32)");
                }
                if(value > 31)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{value} is higher than the maximum (31)");
                }
                this._value = value;
            }
        }

        private int _value;

        [Bound(16)]
        public string text { get; set; } = string.Empty;
        [Bound(16)]
        public string externalText { get; set; } = string.Empty;

        public Sample()
        {
        }

        public Sample(int  id, int  value, string  text, string  externalText)
        {
            this.id = id;
            this._value = value;
            this.text = text;
            this.externalText = externalText;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.id = other.id;
            this._value = other.value;
            this.text = other.text;
            this.externalText = other.externalText;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.id);
            hash.Add(this.value);
            hash.Add(this.text);
            hash.Add(this.externalText);

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

            return this.id.Equals(other.id) && 
            this.value.Equals(other.value) && 
            this.text.Equals(other.text) && 
            this.externalText.Equals(other.externalText);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusRtiAnnotations
