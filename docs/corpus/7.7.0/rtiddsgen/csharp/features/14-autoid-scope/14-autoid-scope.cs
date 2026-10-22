
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-autoid-scope.idl
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

namespace AutoIds
{

    public class Hashed :  IEquatable<Hashed>
    {
        public int first { get; set; }
        public int second { get; set; }

        public Hashed()
        {
        }

        public Hashed(int  first, int  second)
        {
            this.first = first;
            this.second = second;
        }

        public Hashed(Hashed other)
        {
            if (other == null)
            {
                return;
            }

            this.first = other.first;
            this.second = other.second;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.first);
            hash.Add(this.second);

            return hash.ToHashCode();
        }

        public bool Equals(Hashed other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.first.Equals(other.first) && 
            this.second.Equals(other.second);
        }

        public override bool Equals(object obj) => this.Equals(obj as Hashed);

        public override string ToString() => HashedSupport.Instance.ToString(this);
    }

} // namespace AutoIds

namespace AutoIds
{

    public class Reopened :  IEquatable<Reopened>
    {
        public int first { get; set; }
        public int second { get; set; }

        public Reopened()
        {
        }

        public Reopened(int  first, int  second)
        {
            this.first = first;
            this.second = second;
        }

        public Reopened(Reopened other)
        {
            if (other == null)
            {
                return;
            }

            this.first = other.first;
            this.second = other.second;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.first);
            hash.Add(this.second);

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

            return this.first.Equals(other.first) && 
            this.second.Equals(other.second);
        }

        public override bool Equals(object obj) => this.Equals(obj as Reopened);

        public override string ToString() => ReopenedSupport.Instance.ToString(this);
    }

} // namespace AutoIds

public class Sequential :  IEquatable<Sequential>
{
    public int first { get; set; }
    public int second { get; set; }

    public Sequential()
    {
    }

    public Sequential(int  first, int  second)
    {
        this.first = first;
        this.second = second;
    }

    public Sequential(Sequential other)
    {
        if (other == null)
        {
            return;
        }

        this.first = other.first;
        this.second = other.second;

    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(this.first);
        hash.Add(this.second);

        return hash.ToHashCode();
    }

    public bool Equals(Sequential other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.first.Equals(other.first) && 
        this.second.Equals(other.second);
    }

    public override bool Equals(object obj) => this.Equals(obj as Sequential);

    public override string ToString() => SequentialSupport.Instance.ToString(this);
}

public class UsesInclude :  IEquatable<UsesInclude>
{
    public global::Included included { get; set; }

    public UsesInclude()
    {
        included = new global::Included();
    }

    public UsesInclude(global::Included  included)
    {
        this.included = included;
    }

    public UsesInclude(UsesInclude other)
    {
        if (other == null)
        {
            return;
        }

        this.included = new global::Included(other.included);

    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(this.included);

        return hash.ToHashCode();
    }

    public bool Equals(UsesInclude other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.included.Equals(other.included);
    }

    public override bool Equals(object obj) => this.Equals(obj as UsesInclude);

    public override string ToString() => UsesIncludeSupport.Instance.ToString(this);
}

