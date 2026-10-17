
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-autoid-hash.idl
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

namespace CorpusAutoId
{

    public class Sample :  IEquatable<Sample>
    {
        public int id { get; set; }
        [Bound(16)]
        public string text { get; set; } = string.Empty;

        public Sample()
        {
        }

        public Sample(int  id, string  text)
        {
            this.id = id;
            this.text = text;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.id = other.id;
            this.text = other.text;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.id);
            hash.Add(this.text);

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
            this.text.Equals(other.text);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusAutoId
