
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 11-preprocessing.idl
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

public static class BranchValue
{
    public const int Value = (((IncludedConstant.Value))+1);
}

namespace CorpusPreprocessing
{

    public class Sample :  IEquatable<Sample>
    {
        public global::IncludedType item { get; set; }
        public int value { get; set; }

        public Sample()
        {
            item = new global::IncludedType();
        }

        public Sample(global::IncludedType  item, int  value)
        {
            this.item = item;
            this.value = value;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.item = new global::IncludedType(other.item);
            this.value = other.value;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.item);
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

            return this.item.Equals(other.item) && 
            this.value.Equals(other.value);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusPreprocessing
