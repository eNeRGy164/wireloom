
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-optional-collections.idl
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

namespace CorpusOptionalCollections
{

    public class Sample :  IEquatable<Sample>
    {
        [Optional]
        [Bound(4)]
        public ISequence<int> values { get; set; }
        [Optional]
        public int[] items { get; set; }

        public Sample()
        {
        }

        public Sample(ISequence<int>values, int [] items)
        {
            this.values = values;
            this.items = items;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            if(other.values != null)
            {
                this.values = new Rti.Types.Sequence<int>(other.values);
            }
            if(other.items != null)
            {
                this.items = new int[2];
                for( int i1 = 0; i1 < 2; i1++)
                {
                    items[i1] = other.items[i1];
                }
            }

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            if (this.values != null)
            {
                hash.Add(this.values.Count);
            }
            else
            {
                hash.Add(-1);
            }
            if (this.items != null)
            {
                hash.Add(this.items[0]);
            }
            else
            {
                hash.Add(-1);
            }

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

            return (ReferenceEquals(this.values, other.values) || (this.values != null && other.values != null && this.values.SequenceEqual(other.values))) && 
            (ReferenceEquals(this.items, other.items) || (this.items != null && other.items != null && this.items.SequenceEqual(other.items)));
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusOptionalCollections
