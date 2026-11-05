
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
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

public class AutoIdScopeSample :  IEquatable<AutoIdScopeSample>
{
    public AutoIds.Hashed hashed { get; set; }

    public AutoIds.Reopened reopened { get; set; }

    public Sequential sequential { get; set; }

    public UsesInclude usesInclude { get; set; }

    public AutoIdScopeSample()
    {
        hashed = new AutoIds.Hashed();
        reopened = new AutoIds.Reopened();
        sequential = new Sequential();
        usesInclude = new UsesInclude();
    }

    public AutoIdScopeSample(AutoIds.Hashed  hashed, AutoIds.Reopened  reopened, Sequential  sequential, UsesInclude  usesInclude)
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

        this.hashed = new AutoIds.Hashed(other.hashed);
        this.reopened = new AutoIds.Reopened(other.reopened);
        this.sequential = new Sequential(other.sequential);
        this.usesInclude = new UsesInclude(other.usesInclude);

    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(hashed);
        hash.Add(reopened);
        hash.Add(sequential);
        hash.Add(usesInclude);

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

