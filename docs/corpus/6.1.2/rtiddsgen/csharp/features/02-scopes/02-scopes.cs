
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-scopes.idl
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

namespace CorpusScopes
{
    public static class Bound
    {
        public const int Value = 4;
    }
    namespace Nested
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

                hash.Add(x);
                hash.Add(y);

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

    } // namespace Nested

    public class KeywordRecord :  IEquatable<KeywordRecord>
    {
        public int @event { get; set; }

        public CorpusScopes.Nested.Point relative { get; set; }

        public CorpusScopes.Nested.Point absolute { get; set; }

        [Bound((CorpusScopes.Bound.Value))]
        public ISequence<int> values { get; }

        public KeywordRecord()
        {
            relative = new CorpusScopes.Nested.Point();
            absolute = new CorpusScopes.Nested.Point();
            values = new Rti.Types.Sequence<int>();
        }

        public KeywordRecord(int  @event, CorpusScopes.Nested.Point  relative, CorpusScopes.Nested.Point  absolute, ISequence<int>values)
        {
            this.@event = @event;
            this.relative = relative;
            this.absolute = absolute;
            this.values = values;
        }

        public KeywordRecord(KeywordRecord other)
        {
            if (other == null)
            {
                return;
            }

            this.@event = other.@event;
            this.relative = new CorpusScopes.Nested.Point(other.relative);
            this.absolute = new CorpusScopes.Nested.Point(other.absolute);
            this.values = new Rti.Types.Sequence<int>(other.values);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(@event);
            hash.Add(relative);
            hash.Add(absolute);
            hash.Add(values.Count);

            return hash.ToHashCode();
        }

        public bool Equals(KeywordRecord other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.@event.Equals(other.@event) &&
            this.relative.Equals(other.relative) &&
            this.absolute.Equals(other.absolute) &&
            this.values.SequenceEqual(other.values);
        }

        public override bool Equals(object obj) => this.Equals(obj as KeywordRecord);

        public override string ToString() => KeywordRecordSupport.Instance.ToString(this);
    }

} // namespace CorpusScopes
namespace CorpusScopes
{

    public class Reopened :  IEquatable<Reopened>
    {
        public CorpusScopes.KeywordRecord value { get; set; }

        public Reopened()
        {
            value = new CorpusScopes.KeywordRecord();
        }

        public Reopened(CorpusScopes.KeywordRecord  value)
        {
            this.value = value;
        }

        public Reopened(Reopened other)
        {
            if (other == null)
            {
                return;
            }

            this.value = new CorpusScopes.KeywordRecord(other.value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(value);

            return hash.ToHashCode();
        }

        public bool Equals(Reopened other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.value.Equals(other.value);
        }

        public override bool Equals(object obj) => this.Equals(obj as Reopened);

        public override string ToString() => ReopenedSupport.Instance.ToString(this);
    }

} // namespace CorpusScopes
