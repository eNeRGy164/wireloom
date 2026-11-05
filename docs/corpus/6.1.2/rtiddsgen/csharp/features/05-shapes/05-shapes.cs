
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 05-shapes.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
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

namespace CorpusCollectionShapes
{

    public class Item :  IEquatable<Item>
    {
        public int id { get; set; }

        [Bound(12)]
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

            hash.Add(id);
            hash.Add(label);

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
            Value = new int[3];
            for( int i1 = 0; i1 < 3; i1++)
            {
                Value[i1] = 0;
            };
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

            this.Value = new int[3];
            for( int i1 = 0; i1 < 3; i1++)
            {
                Value[i1] = other.Value[i1];
            }
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(Value[0]);

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

    public class BoundedLongs :  IEquatable<BoundedLongs>
    {

        [Bound(4)]
        public ISequence<int> Value { get; }

        public BoundedLongs()
        {
            Value = new Rti.Types.Sequence<int>();
        }

        public BoundedLongs(ISequence<int>Value)
        {
            this.Value = Value;
        }

        public BoundedLongs(BoundedLongs other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new Rti.Types.Sequence<int>(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(Value.Count);

            return hash.ToHashCode();
        }

        public bool Equals(BoundedLongs other)
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

        public override bool Equals(object obj) => this.Equals(obj as BoundedLongs);

        public override string ToString() => BoundedLongsSupport.Instance.ToString(this);
    }

    public class Rows :  IEquatable<Rows>
    {

        [Bound(2)]
        public ISequence<CorpusCollectionShapes.Row> Value { get; }

        public Rows()
        {
            Value = new Rti.Types.Sequence<CorpusCollectionShapes.Row>();
        }

        public Rows(ISequence<CorpusCollectionShapes.Row>Value)
        {
            this.Value = Value;
        }

        public Rows(Rows other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new Rti.Types.Sequence<CorpusCollectionShapes.Row>(other.Value.Select(element => new CorpusCollectionShapes.Row(element)));

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(Value.Count);

            return hash.ToHashCode();
        }

        public bool Equals(Rows other)
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

        public override bool Equals(object obj) => this.Equals(obj as Rows);

        public override string ToString() => RowsSupport.Instance.ToString(this);
    }

    public class Items :  IEquatable<Items>
    {

        [Bound(3)]
        public ISequence<CorpusCollectionShapes.Item> Value { get; }

        public Items()
        {
            Value = new Rti.Types.Sequence<CorpusCollectionShapes.Item>();
        }

        public Items(ISequence<CorpusCollectionShapes.Item>Value)
        {
            this.Value = Value;
        }

        public Items(Items other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new Rti.Types.Sequence<CorpusCollectionShapes.Item>(other.Value.Select(element => new CorpusCollectionShapes.Item(element)));

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(Value.Count);

            return hash.ToHashCode();
        }

        public bool Equals(Items other)
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

        public override bool Equals(object obj) => this.Equals(obj as Items);

        public override string ToString() => ItemsSupport.Instance.ToString(this);
    }

    public class Names :  IEquatable<Names>
    {

        [Bound(4)]
        public ISequence<string> Value { get; }

        public Names()
        {
            Value = new Rti.Types.Sequence<string>();
        }

        public Names(ISequence<string>Value)
        {
            this.Value = Value;
        }

        public Names(Names other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new Rti.Types.Sequence<string>(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(Value.Count);

            return hash.ToHashCode();
        }

        public bool Equals(Names other)
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

        public override bool Equals(object obj) => this.Equals(obj as Names);

        public override string ToString() => NamesSupport.Instance.ToString(this);
    }

    public class Sample :  IEquatable<Sample>
    {
        [Bound(100)]
        public ISequence<CorpusCollectionShapes.Item> unboundedItems { get; }

        public CorpusCollectionShapes.Items boundedItems { get; set; }

        [Bound(2)]
        public ISequence<CorpusCollectionShapes.BoundedLongs> nestedSequences { get; }

        public CorpusCollectionShapes.Row[] rows { get; set; }

        [Bound(2)]
        public ISequence<CorpusCollectionShapes.Row> sequenceOfArrays { get; }

        [Bound(2)]
        public ISequence<CorpusCollectionShapes.Rows> sequenceOfArrayAliases { get; }

        public CorpusCollectionShapes.Names names { get; set; }

        public Sample()
        {
            unboundedItems = new Rti.Types.Sequence<CorpusCollectionShapes.Item>();
            boundedItems = new CorpusCollectionShapes.Items();
            nestedSequences = new Rti.Types.Sequence<CorpusCollectionShapes.BoundedLongs>();
            rows = new CorpusCollectionShapes.Row[2];
            for( int i1 = 0; i1 < 2; i1++)
            {
                rows[i1] = new CorpusCollectionShapes.Row();
            };
            sequenceOfArrays = new Rti.Types.Sequence<CorpusCollectionShapes.Row>();
            sequenceOfArrayAliases = new Rti.Types.Sequence<CorpusCollectionShapes.Rows>();
            names = new CorpusCollectionShapes.Names();
        }

        public Sample(ISequence<CorpusCollectionShapes.Item>unboundedItems, CorpusCollectionShapes.Items  boundedItems, ISequence<CorpusCollectionShapes.BoundedLongs>nestedSequences, CorpusCollectionShapes.Row [] rows, ISequence<CorpusCollectionShapes.Row>sequenceOfArrays, ISequence<CorpusCollectionShapes.Rows>sequenceOfArrayAliases, CorpusCollectionShapes.Names  names)
        {
            this.unboundedItems = unboundedItems;
            this.boundedItems = boundedItems;
            this.nestedSequences = nestedSequences;
            this.rows = rows;
            this.sequenceOfArrays = sequenceOfArrays;
            this.sequenceOfArrayAliases = sequenceOfArrayAliases;
            this.names = names;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.unboundedItems = new Rti.Types.Sequence<CorpusCollectionShapes.Item>(other.unboundedItems.Select(element => new CorpusCollectionShapes.Item(element)));
            this.boundedItems = other.boundedItems;
            this.nestedSequences = new Rti.Types.Sequence<CorpusCollectionShapes.BoundedLongs>(other.nestedSequences.Select(element => new CorpusCollectionShapes.BoundedLongs(element)));
            this.rows = new CorpusCollectionShapes.Row[2];
            for( int i1 = 0; i1 < 2; i1++)
            {
                rows[i1] = new CorpusCollectionShapes.Row(other.rows[i1]);
            }        this.sequenceOfArrays = new Rti.Types.Sequence<CorpusCollectionShapes.Row>(other.sequenceOfArrays.Select(element => new CorpusCollectionShapes.Row(element)));
            this.sequenceOfArrayAliases = new Rti.Types.Sequence<CorpusCollectionShapes.Rows>(other.sequenceOfArrayAliases.Select(element => new CorpusCollectionShapes.Rows(element)));
            this.names = other.names;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(unboundedItems.Count);
            hash.Add(boundedItems);
            hash.Add(nestedSequences.Count);
            hash.Add(rows[0]);
            hash.Add(sequenceOfArrays.Count);
            hash.Add(sequenceOfArrayAliases.Count);
            hash.Add(names);

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
            this.boundedItems.Equals(other.boundedItems) &&
            this.nestedSequences.SequenceEqual(other.nestedSequences) &&
            this.rows.SequenceEqual(other.rows) &&
            this.sequenceOfArrays.SequenceEqual(other.sequenceOfArrays) &&
            this.sequenceOfArrayAliases.SequenceEqual(other.sequenceOfArrayAliases) &&
            this.names.Equals(other.names);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusCollectionShapes
