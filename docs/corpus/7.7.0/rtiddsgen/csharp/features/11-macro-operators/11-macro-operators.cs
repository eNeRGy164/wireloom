
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 11-macro-operators.idl
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

namespace CorpusMacroOperators
{

    public static class Name
    {
        public const string Value = "sample";
    }

    public class Sample :  IEquatable<Sample>
    {
        public int field { get; set; }

        public Sample()
        {
        }

        public Sample(int  field)
        {
            this.field = field;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.field = other.field;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.field);

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

            return this.field.Equals(other.field);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusMacroOperators
