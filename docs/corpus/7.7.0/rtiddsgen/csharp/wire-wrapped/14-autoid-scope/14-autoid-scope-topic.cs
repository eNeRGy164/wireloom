
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-autoid-scope-topic.idl
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

public class AutoIdScopeSample :  IEquatable<AutoIdScopeSample>
{
    public global::AutoIds.Hashed hashed { get; set; }
    public global::AutoIds.Reopened reopened { get; set; }
    public global::Sequential sequential { get; set; }
    public global::UsesInclude usesInclude { get; set; }

    public AutoIdScopeSample()
    {
        hashed = new global::AutoIds.Hashed();
        reopened = new global::AutoIds.Reopened();
        sequential = new global::Sequential();
        usesInclude = new global::UsesInclude();
    }

    public AutoIdScopeSample(global::AutoIds.Hashed  hashed, global::AutoIds.Reopened  reopened, global::Sequential  sequential, global::UsesInclude  usesInclude)
    {
        this.hashed = hashed;
        this.reopened = reopened;
        this.sequential = sequential;
        this.usesInclude = usesInclude;
    }

    public AutoIdScopeSample(AutoIdScopeSample other)
    {
        if (other == null)
        {
            return;
        }

        this.hashed = new global::AutoIds.Hashed(other.hashed);
        this.reopened = new global::AutoIds.Reopened(other.reopened);
        this.sequential = new global::Sequential(other.sequential);
        this.usesInclude = new global::UsesInclude(other.usesInclude);

    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(this.hashed);
        hash.Add(this.reopened);
        hash.Add(this.sequential);
        hash.Add(this.usesInclude);

        return hash.ToHashCode();
    }

    public bool Equals(AutoIdScopeSample other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.hashed.Equals(other.hashed) && 
        this.reopened.Equals(other.reopened) && 
        this.sequential.Equals(other.sequential) && 
        this.usesInclude.Equals(other.usesInclude);
    }

    public override bool Equals(object obj) => this.Equals(obj as AutoIdScopeSample);

    public override string ToString() => AutoIdScopeSampleSupport.Instance.ToString(this);
}

