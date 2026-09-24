
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 05-collections.idl
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

namespace CorpusCollections
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

            hash.Add(this.Value.Count);

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

    public class CoordinateGrid :  IEquatable<CoordinateGrid>
    {

        public int[,] Value { get; set; }

        public CoordinateGrid()
        {
            Value = new int[2,3];
            for( int i1 = 0; i1 < 2; i1++)
            {
                for( int i2 = 0; i2 < 3; i2++)
                {
                    Value[i1, i2] = (0);
                }}
            ;
        }

        public CoordinateGrid(int [,] Value)
        {
            this.Value = Value;
        }

        public CoordinateGrid(CoordinateGrid other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new int[2,3];
            for( int i1 = 0; i1 < 2; i1++)
            {
                for( int i2 = 0; i2 < 3; i2++)
                {
                    Value[i1, i2] = other.Value[i1, i2];
                }}

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.Value[0, 0]);

            return hash.ToHashCode();
        }

        public bool Equals(CoordinateGrid other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Value.Rank == other.Value.Rank && Enumerable.Range(0,this.Value.Rank).All(dimension => this.Value.GetLength(dimension) == other.Value.GetLength(dimension)) && this.Value.Cast<int>().SequenceEqual(other.Value.Cast<int>());
        }

        public override bool Equals(object obj) => this.Equals(obj as CoordinateGrid);

        public override string ToString() => CoordinateGridSupport.Instance.ToString(this);
    }

    public class Sample :  IEquatable<Sample>
    {
        public int[,] values { get; set; }
        [Bound(100)]
        public ISequence<int> unbounded { get; }
        public global::CorpusCollections.BoundedLongs bounded { get; set; }
        [Bound(2)]
        public ISequence<global::CorpusCollections.Item> items { get; }
        public global::CorpusCollections.CoordinateGrid grid { get; set; }

        public Sample()
        {
            values = new int[2,3];
            for( int i1 = 0; i1 < 2; i1++)
            {
                for( int i2 = 0; i2 < 3; i2++)
                {
                    values[i1, i2] = (0);
                }}
            ;
            unbounded = new Rti.Types.Sequence<int>();
            bounded = new global::CorpusCollections.BoundedLongs();
            items = new Rti.Types.Sequence<global::CorpusCollections.Item>();
            grid = new global::CorpusCollections.CoordinateGrid();
        }

        public Sample(int [,] values, ISequence<int>unbounded, global::CorpusCollections.BoundedLongs  bounded, ISequence<global::CorpusCollections.Item>items, global::CorpusCollections.CoordinateGrid  grid)
        {
            this.values = values;
            this.unbounded = unbounded;
            this.bounded = bounded;
            this.items = items;
            this.grid = grid;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.values = new int[2,3];
            for( int i1 = 0; i1 < 2; i1++)
            {
                for( int i2 = 0; i2 < 3; i2++)
                {
                    values[i1, i2] = other.values[i1, i2];
                }}
            this.unbounded = new Rti.Types.Sequence<int>(other.unbounded);
            this.bounded = new global::CorpusCollections.BoundedLongs(other.bounded);
            this.items = new Rti.Types.Sequence<global::CorpusCollections.Item>(other.items.Select(element => new global::CorpusCollections.Item(element)));
            this.grid = new global::CorpusCollections.CoordinateGrid(other.grid);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.values[0, 0]);
            hash.Add(this.unbounded.Count);
            hash.Add(this.bounded);
            hash.Add(this.items.Count);
            hash.Add(this.grid);

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

            return this.values.Rank == other.values.Rank && Enumerable.Range(0,this.values.Rank).All(dimension => this.values.GetLength(dimension) == other.values.GetLength(dimension)) && this.values.Cast<int>().SequenceEqual(other.values.Cast<int>()) && 
            this.unbounded.SequenceEqual(other.unbounded) && 
            this.bounded.Equals(other.bounded) && 
            this.items.SequenceEqual(other.items) && 
            this.grid.Equals(other.grid);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusCollections
