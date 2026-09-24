
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-macro-recursion.idl
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

namespace CorpusNegativeMacroRecursion
{

    public class Sample :  IEquatable<Sample>
    {
        public int CORPUS_A { get; set; }

        public Sample()
        {
        }

        public Sample(int  CORPUS_A)
        {
            this.CORPUS_A = CORPUS_A;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.CORPUS_A = other.CORPUS_A;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.CORPUS_A);

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

            return this.CORPUS_A.Equals(other.CORPUS_A);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusNegativeMacroRecursion
