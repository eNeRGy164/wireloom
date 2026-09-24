
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-alias-aggregate.idl
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

namespace CorpusAggregateAliases
{

    public class Point :  IEquatable<Point>
    {
        public int x { get; set; }
        public int y { get; set; }

        public Point()
        {
        }

        public Point(int  x, int  y)
        {
            this.x = x;
            this.y = y;
        }

        public Point(Point other)
        {
            if (other == null)
            {
                return;
            }

            this.x = other.x;
            this.y = other.y;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.x);
            hash.Add(this.y);

            return hash.ToHashCode();
        }

        public bool Equals(Point other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.x.Equals(other.x) && 
            this.y.Equals(other.y);
        }

        public override bool Equals(object obj) => this.Equals(obj as Point);

        public override string ToString() => PointSupport.Instance.ToString(this);
    }

    public class PointAlias :  IEquatable<PointAlias>
    {

        public global::CorpusAggregateAliases.Point Value { get; set; }

        public PointAlias()
        {
            Value = new global::CorpusAggregateAliases.Point();
        }

        public PointAlias(global::CorpusAggregateAliases.Point  Value)
        {
            this.Value = Value;
        }

        public PointAlias(PointAlias other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new global::CorpusAggregateAliases.Point(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.Value);

            return hash.ToHashCode();
        }

        public bool Equals(PointAlias other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Value.Equals(other.Value);
        }

        public override bool Equals(object obj) => this.Equals(obj as PointAlias);

        public override string ToString() => PointAliasSupport.Instance.ToString(this);
    }

    public class PointAlias2 :  IEquatable<PointAlias2>
    {

        public global::CorpusAggregateAliases.Point Value { get; set; }

        public PointAlias2()
        {
            Value = new global::CorpusAggregateAliases.Point();
        }

        public PointAlias2(global::CorpusAggregateAliases.Point  Value)
        {
            this.Value = Value;
        }

        public PointAlias2(PointAlias2 other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new global::CorpusAggregateAliases.Point(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.Value);

            return hash.ToHashCode();
        }

        public bool Equals(PointAlias2 other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Value.Equals(other.Value);
        }

        public override bool Equals(object obj) => this.Equals(obj as PointAlias2);

        public override string ToString() => PointAlias2Support.Instance.ToString(this);
    }

    public class Sample :  IEquatable<Sample>
    {
        public global::CorpusAggregateAliases.Point point { get; set; }

        public Sample()
        {
            point = new global::CorpusAggregateAliases.Point();
        }

        public Sample(global::CorpusAggregateAliases.Point  point)
        {
            this.point = point;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.point = new global::CorpusAggregateAliases.Point(other.point);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.point);

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

            return this.point.Equals(other.point);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusAggregateAliases
