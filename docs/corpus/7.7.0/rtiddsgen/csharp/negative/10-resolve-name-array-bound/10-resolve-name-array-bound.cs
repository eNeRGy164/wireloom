
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-resolve-name-array-bound.idl
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

namespace CorpusResolveNameArrayBound
{

    public class Sample :  IEquatable<Sample>
    {
        public byte[] bytes { get; set; }

        public Sample()
        {
            bytes = new byte[(ARRAY_BOUND.Value)];
            for( int i1 = 0; i1 < (ARRAY_BOUND.Value); i1++)
            {
                bytes[i1] = (0);
            }
            ;
        }

        public Sample(byte [] bytes)
        {
            this.bytes = bytes;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.bytes = new byte[(ARRAY_BOUND.Value)];
            for( int i1 = 0; i1 < (ARRAY_BOUND.Value); i1++)
            {
                bytes[i1] = other.bytes[i1];
            }

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.bytes[0]);

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

            return this.bytes.SequenceEqual(other.bytes);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusResolveNameArrayBound
