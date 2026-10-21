
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-interface-body-control.idl
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

namespace CorpusInterfaceControl
{

    public class Sample :  IEquatable<Sample>
    {
        public int number { get; set; }

        public Sample()
        {
        }

        public Sample(int  number)
        {
            this.number = number;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.number = other.number;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.number);

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

            return this.number.Equals(other.number);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusInterfaceControl
