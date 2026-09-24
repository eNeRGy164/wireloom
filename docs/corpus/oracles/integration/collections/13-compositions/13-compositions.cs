
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-compositions.idl
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

namespace CorpusIntegrationCollections
{

    public class Item :  IEquatable<Item>
    {
        public int id { get; set; }
        [Bound(16)]
        public string label { get; set; } = string.Empty;

        public Item()
        {
        }

        public Item(int  id, string  label)
        {
            this.id = id;
            this.label = label;
        }

        public Item(Item other)
        {
            if (other == null)
            {
                return;
            }

            this.id = other.id;
            this.label = other.label;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.id);
            hash.Add(this.label);

            return hash.ToHashCode();
        }

        public bool Equals(Item other)
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
            this.label.Equals(other.label);
        }

        public override bool Equals(object obj) => this.Equals(obj as Item);

        public override string ToString() => ItemSupport.Instance.ToString(this);
    }

    public class Row :  IEquatable<Row>
    {

        public int[] Value { get; set; }

        public Row()
        {
            Value = new int[2];
            for( int i1 = 0; i1 < 2; i1++)
            {
                Value[i1] = (0);
            }
            ;
        }

        public Row(int [] Value)
        {
            this.Value = Value;
        }

        public Row(Row other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new int[2];
            for( int i1 = 0; i1 < 2; i1++)
            {
                Value[i1] = other.Value[i1];
            }

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.Value[0]);

            return hash.ToHashCode();
        }

        public bool Equals(Row other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Value.SequenceEqual(other.Value);
        }

        public override bool Equals(object obj) => this.Equals(obj as Row);

        public override string ToString() => RowSupport.Instance.ToString(this);
    }

    public class Sample :  IEquatable<Sample>
    {
        [Bound(100)]
        public ISequence<global::CorpusIntegrationCollections.Item> unboundedItems { get; }
        [Bound(2)]
        public ISequence<global::CorpusIntegrationCollections.Item> boundedItems { get; }
        public global::CorpusIntegrationCollections.Item[] aggregateArray { get; set; }
        public global::CorpusIntegrationCollections.Row[] rows { get; set; }
        [Bound(2)]
        public ISequence<global::CorpusIntegrationCollections.Row> sequenceOfArrays { get; }

        public Sample()
        {
            unboundedItems = new Rti.Types.Sequence<global::CorpusIntegrationCollections.Item>();
            boundedItems = new Rti.Types.Sequence<global::CorpusIntegrationCollections.Item>();
            aggregateArray = new global::CorpusIntegrationCollections.Item[2];
            for( int i1 = 0; i1 < 2; i1++)
            {
                aggregateArray[i1] = new global::CorpusIntegrationCollections.Item();
            }
            ;
            rows = new global::CorpusIntegrationCollections.Row[2];
            for( int i1 = 0; i1 < 2; i1++)
            {
                rows[i1] = new global::CorpusIntegrationCollections.Row();
            }
            ;
            sequenceOfArrays = new Rti.Types.Sequence<global::CorpusIntegrationCollections.Row>();
        }

        public Sample(ISequence<global::CorpusIntegrationCollections.Item>unboundedItems, ISequence<global::CorpusIntegrationCollections.Item>boundedItems, global::CorpusIntegrationCollections.Item [] aggregateArray, global::CorpusIntegrationCollections.Row [] rows, ISequence<global::CorpusIntegrationCollections.Row>sequenceOfArrays)
        {
            this.unboundedItems = unboundedItems;
            this.boundedItems = boundedItems;
            this.aggregateArray = aggregateArray;
            this.rows = rows;
            this.sequenceOfArrays = sequenceOfArrays;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.unboundedItems = new Rti.Types.Sequence<global::CorpusIntegrationCollections.Item>(other.unboundedItems.Select(element => new global::CorpusIntegrationCollections.Item(element)));
            this.boundedItems = new Rti.Types.Sequence<global::CorpusIntegrationCollections.Item>(other.boundedItems.Select(element => new global::CorpusIntegrationCollections.Item(element)));
            this.aggregateArray = new global::CorpusIntegrationCollections.Item[2];
            for( int i1 = 0; i1 < 2; i1++)
            {
                aggregateArray[i1] = new global::CorpusIntegrationCollections.Item(other.aggregateArray[i1]);
            }
            this.rows = new global::CorpusIntegrationCollections.Row[2];
            for( int i1 = 0; i1 < 2; i1++)
            {
                rows[i1] = new global::CorpusIntegrationCollections.Row(other.rows[i1]);
            }
            this.sequenceOfArrays = new Rti.Types.Sequence<global::CorpusIntegrationCollections.Row>(other.sequenceOfArrays.Select(element => new global::CorpusIntegrationCollections.Row(element)));

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.unboundedItems.Count);
            hash.Add(this.boundedItems.Count);
            hash.Add(this.aggregateArray[0]);
            hash.Add(this.rows[0]);
            hash.Add(this.sequenceOfArrays.Count);

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

            return this.unboundedItems.SequenceEqual(other.unboundedItems) && 
            this.boundedItems.SequenceEqual(other.boundedItems) && 
            this.aggregateArray.SequenceEqual(other.aggregateArray) && 
            this.rows.SequenceEqual(other.rows) && 
            this.sequenceOfArrays.SequenceEqual(other.sequenceOfArrays);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusIntegrationCollections
