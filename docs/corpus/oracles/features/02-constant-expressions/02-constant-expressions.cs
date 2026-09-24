
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-constant-expressions.idl
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

namespace CorpusConstantExpressions
{

    public static class Add
    {
        public const int Value = 2+3;
    }

    public static class Shifted
    {
        public const int Value = ((CorpusConstantExpressions.Add.Value)<<2)|1;
    }

    public static class Masked
    {
        public const int Value = ((CorpusConstantExpressions.Shifted.Value)&15)^3;
    }

    public static class Bound
    {
        public const int Value = (CorpusConstantExpressions.Masked.Value)-1;
    }

    public class Sample :  IEquatable<Sample>
    {
        [Bound((CorpusConstantExpressions.Bound.Value))]
        public ISequence<int> values { get; }
        public int[] array { get; set; }

        public Sample()
        {
            values = new Rti.Types.Sequence<int>();
            array = new int[(CorpusConstantExpressions.Add.Value)];
            for( int i1 = 0; i1 < (CorpusConstantExpressions.Add.Value); i1++)
            {
                array[i1] = (0);
            }
            ;
        }

        public Sample(ISequence<int>values, int [] array)
        {
            this.values = values;
            this.array = array;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.values = new Rti.Types.Sequence<int>(other.values);
            this.array = new int[(CorpusConstantExpressions.Add.Value)];
            for( int i1 = 0; i1 < (CorpusConstantExpressions.Add.Value); i1++)
            {
                array[i1] = other.array[i1];
            }

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.values.Count);
            hash.Add(this.array[0]);

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

            return this.values.SequenceEqual(other.values) && 
            this.array.SequenceEqual(other.array);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusConstantExpressions
