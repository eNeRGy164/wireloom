
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-names-constants.idl
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

namespace CorpusNames
{

    public static class Base
    {
        public const int Value = 4;
    }

    public static class Derived
    {
        public const int Value = ((CorpusNames.Base.Value)<<2)|1;
    }

    public enum State
    {
        Idle,
        Running,
        Complete
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

    } // namespace Nested

    public class KeywordRecord :  IEquatable<KeywordRecord>
    {
        public int @event { get; set; }
        public global::CorpusNames.State state { get; set; }
        public global::CorpusNames.Nested.Point point { get; set; }

        public KeywordRecord()
        {
            state = (global::CorpusNames.State) (0);
            point = new global::CorpusNames.Nested.Point();
        }

        public KeywordRecord(int  @event, global::CorpusNames.State  state, global::CorpusNames.Nested.Point  point)
        {
            this.@event = @event;
            this.state = state;
            this.point = point;
        }

        public KeywordRecord(KeywordRecord other)
        {
            if (other == null)
            {
                return;
            }

            this.@event = other.@event;
            this.state = other.state;
            this.point = new global::CorpusNames.Nested.Point(other.point);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.@event);
            hash.Add(this.state);
            hash.Add(this.point);

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
            this.state.Equals(other.state) && 
            this.point.Equals(other.point);
        }

        public override bool Equals(object obj) => this.Equals(obj as KeywordRecord);

        public override string ToString() => KeywordRecordSupport.Instance.ToString(this);
    }

} // namespace CorpusNames
