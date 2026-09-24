
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-zero-bound.idl
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

public class ZeroBound :  IEquatable<ZeroBound>
{
    [Bound(0)]
    public string value { get; set; } = string.Empty;

    public ZeroBound()
    {
    }

    public ZeroBound(string  value)
    {
        this.value = value;
    }

    public ZeroBound(ZeroBound other)
    {
        if (other == null)
        {
            return;
        }

        this.value = other.value;

    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(this.value);

        return hash.ToHashCode();
    }

    public bool Equals(ZeroBound other)
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

    public override bool Equals(object obj) => this.Equals(obj as ZeroBound);

    public override string ToString() => ZeroBoundSupport.Instance.ToString(this);
}

