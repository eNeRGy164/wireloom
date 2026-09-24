
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 06-aggregates.idl
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

namespace CorpusAggregates
{

    public class Base :  IEquatable<Base>
    {
        public int @base { get; set; }

        public Base()
        {
        }

        public Base(int  @base)
        {
            this.@base = @base;
        }

        public Base(Base other)
        {
            if (other == null)
            {
                return;
            }

            this.@base = other.@base;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.@base);

            return hash.ToHashCode();
        }

        public bool Equals(Base other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.@base.Equals(other.@base);
        }

        public override bool Equals(object obj) => this.Equals(obj as Base);

        public override string ToString() => BaseSupport.Instance.ToString(this);
    }

    public class Derived :  global::CorpusAggregates.Base, IEquatable<Derived>
    {
        [Bound(16)]
        public string derived { get; set; } = string.Empty;

        public Derived()
        {
        }

        public Derived(int  @base, string  derived) : base(@base)
        {
            this.derived = derived;
        }

        public Derived(Derived other) : base(other)
        {
            if (other == null)
            {
                return;
            }

            this.derived = other.derived;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(base.GetHashCode());

            hash.Add(this.derived);

            return hash.ToHashCode();
        }

        public bool Equals(Derived other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if(!base.Equals(other))
            {
                return false;
            }

            return this.derived.Equals(other.derived);
        }

        public override bool Equals(object obj) => this.Equals(obj as Derived);

        public override string ToString() => DerivedSupport.Instance.ToString(this);
    }

    public class Recursive :  IEquatable<Recursive>
    {
        public int value { get; set; }
        [Bound(100)]
        public ISequence<global::CorpusAggregates.Recursive> children { get; }

        public Recursive()
        {
            children = new Rti.Types.Sequence<global::CorpusAggregates.Recursive>();
        }

        public Recursive(int  value, ISequence<global::CorpusAggregates.Recursive>children)
        {
            this.value = value;
            this.children = children;
        }

        public Recursive(Recursive other)
        {
            if (other == null)
            {
                return;
            }

            this.value = other.value;
            this.children = new Rti.Types.Sequence<global::CorpusAggregates.Recursive>(other.children.Select(element => new global::CorpusAggregates.Recursive(element)));

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.value);
            hash.Add(this.children.Count);

            return hash.ToHashCode();
        }

        public bool Equals(Recursive other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.value.Equals(other.value) && 
            this.children.SequenceEqual(other.children);
        }

        public override bool Equals(object obj) => this.Equals(obj as Recursive);

        public override string ToString() => RecursiveSupport.Instance.ToString(this);
    }

    public class Composed :  IEquatable<Composed>
    {
        public global::CorpusAggregates.Base @base { get; set; }
        public global::CorpusAggregates.Derived derived { get; set; }

        public Composed()
        {
            @base = new global::CorpusAggregates.Base();
            derived = new global::CorpusAggregates.Derived();
        }

        public Composed(global::CorpusAggregates.Base  @base, global::CorpusAggregates.Derived  derived)
        {
            this.@base = @base;
            this.derived = derived;
        }

        public Composed(Composed other)
        {
            if (other == null)
            {
                return;
            }

            this.@base = new global::CorpusAggregates.Base(other.@base);
            this.derived = new global::CorpusAggregates.Derived(other.derived);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.@base);
            hash.Add(this.derived);

            return hash.ToHashCode();
        }

        public bool Equals(Composed other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.@base.Equals(other.@base) && 
            this.derived.Equals(other.derived);
        }

        public override bool Equals(object obj) => this.Equals(obj as Composed);

        public override string ToString() => ComposedSupport.Instance.ToString(this);
    }

} // namespace CorpusAggregates
