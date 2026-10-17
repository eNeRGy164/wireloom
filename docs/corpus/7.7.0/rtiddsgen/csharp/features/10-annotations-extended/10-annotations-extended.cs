
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-annotations-extended.idl
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

namespace CorpusAnnotationVariants
{

    public class Sample :  IEquatable<Sample>
    {
        [Key]
        public int id { get; set; }
        public int value { get; set; }

        public Sample()
        {
            value = (int) (3);
        }

        public Sample(int  id, int  value)
        {
            this.id = id;
            this.value = value;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.id = other.id;
            this.value = other.value;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.id);
            hash.Add(this.value);

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
            this.value.Equals(other.value);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

    public class NestedValue :  IEquatable<NestedValue>
    {
        public int value { get; set; }

        public NestedValue()
        {
        }

        public NestedValue(int  value)
        {
            this.value = value;
        }

        public NestedValue(NestedValue other)
        {
            if (other == null)
            {
                return;
            }

            this.value = other.value;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.value);

            return hash.ToHashCode();
        }

        public bool Equals(NestedValue other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.value.Equals(other.value);
        }

        public override bool Equals(object obj) => this.Equals(obj as NestedValue);

        public override string ToString() => NestedValueSupport.Instance.ToString(this);
    }

} // namespace CorpusAnnotationVariants
