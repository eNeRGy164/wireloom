
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from autoid-hash-include.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
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

public class Included :  IEquatable<Included>
{
    public int first { get; set; }
    public int second { get; set; }

    public Included()
    {
    }

    public Included(int  first, int  second)
    {
        this.first = first;
        this.second = second;
    }

    public Included(Included other)
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

    public bool Equals(Included other)
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

    public override bool Equals(object obj) => this.Equals(obj as Included);

    public override string ToString() => IncludedSupport.Instance.ToString(this);
}

