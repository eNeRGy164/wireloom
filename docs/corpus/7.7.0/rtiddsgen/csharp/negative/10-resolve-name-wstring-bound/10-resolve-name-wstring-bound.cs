
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-resolve-name-wstring-bound.idl
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

namespace CorpusResolveNameWstringBound
{

    public class Sample :  IEquatable<Sample>
    {
        [Bound((WSTRING_BOUND.Value))]
        public string wideText { get; set; } = string.Empty;

        public Sample()
        {
        }

        public Sample(string  wideText)
        {
            this.wideText = wideText;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.wideText = other.wideText;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.wideText);

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

            return this.wideText.Equals(other.wideText);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusResolveNameWstringBound
