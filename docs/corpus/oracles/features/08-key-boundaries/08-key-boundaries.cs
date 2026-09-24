
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-boundaries.idl
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

namespace CorpusKeyBoundaries
{

    public class ArrayKey :  IEquatable<ArrayKey>
    {
        [Key]
        public int[] coordinates { get; set; }
        public int payload { get; set; }

        public ArrayKey()
        {
            coordinates = new int[2];
            for( int i1 = 0; i1 < 2; i1++)
            {
                coordinates[i1] = (0);
            }
            ;
        }

        public ArrayKey(int [] coordinates, int  payload)
        {
            this.coordinates = coordinates;
            this.payload = payload;
        }

        public ArrayKey(ArrayKey other)
        {
            if (other == null)
            {
                return;
            }

            this.coordinates = new int[2];
            for( int i1 = 0; i1 < 2; i1++)
            {
                coordinates[i1] = other.coordinates[i1];
            }
            this.payload = other.payload;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.coordinates[0]);
            hash.Add(this.payload);

            return hash.ToHashCode();
        }

        public bool Equals(ArrayKey other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.coordinates.SequenceEqual(other.coordinates) && 
            this.payload.Equals(other.payload);
        }

        public override bool Equals(object obj) => this.Equals(obj as ArrayKey);

        public override string ToString() => ArrayKeySupport.Instance.ToString(this);
    }

    public class BoundedStringKey :  IEquatable<BoundedStringKey>
    {
        [Key]
        [Bound(12)]
        public string name { get; set; } = string.Empty;
        public int payload { get; set; }

        public BoundedStringKey()
        {
        }

        public BoundedStringKey(string  name, int  payload)
        {
            this.name = name;
            this.payload = payload;
        }

        public BoundedStringKey(BoundedStringKey other)
        {
            if (other == null)
            {
                return;
            }

            this.name = other.name;
            this.payload = other.payload;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.name);
            hash.Add(this.payload);

            return hash.ToHashCode();
        }

        public bool Equals(BoundedStringKey other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.name.Equals(other.name) && 
            this.payload.Equals(other.payload);
        }

        public override bool Equals(object obj) => this.Equals(obj as BoundedStringKey);

        public override string ToString() => BoundedStringKeySupport.Instance.ToString(this);
    }

    public class UnboundedStringKey :  IEquatable<UnboundedStringKey>
    {
        [Key]
        [Bound(255)]
        public string name { get; set; } = string.Empty;
        public int payload { get; set; }

        public UnboundedStringKey()
        {
        }

        public UnboundedStringKey(string  name, int  payload)
        {
            this.name = name;
            this.payload = payload;
        }

        public UnboundedStringKey(UnboundedStringKey other)
        {
            if (other == null)
            {
                return;
            }

            this.name = other.name;
            this.payload = other.payload;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.name);
            hash.Add(this.payload);

            return hash.ToHashCode();
        }

        public bool Equals(UnboundedStringKey other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.name.Equals(other.name) && 
            this.payload.Equals(other.payload);
        }

        public override bool Equals(object obj) => this.Equals(obj as UnboundedStringKey);

        public override string ToString() => UnboundedStringKeySupport.Instance.ToString(this);
    }

} // namespace CorpusKeyBoundaries
