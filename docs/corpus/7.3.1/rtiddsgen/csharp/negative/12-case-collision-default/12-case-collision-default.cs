
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-case-collision-default.idl
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

namespace CorpusStrictCase
{

    public class Sample :  IEquatable<Sample>
    {
        public int number { get; set; }
        public int Number { get; set; }

        public Sample()
        {
        }

        public Sample(int  number, int  Number)
        {
            this.number = number;
            this.Number = Number;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.number = other.number;
            this.Number = other.Number;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.number);
            hash.Add(this.Number);

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

            return this.number.Equals(other.number) && 
            this.Number.Equals(other.Number);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusStrictCase
