
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-extensibility-full.idl
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

public class FullExtensibility :  IEquatable<FullExtensibility>
{
    public int value { get; set; }

    public FullExtensibility()
    {
    }

    public FullExtensibility(int  value)
    {
        this.value = value;
    }

    public FullExtensibility(FullExtensibility other)
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

    public bool Equals(FullExtensibility other)
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

    public override bool Equals(object obj) => this.Equals(obj as FullExtensibility);

    public override string ToString() => FullExtensibilitySupport.Instance.ToString(this);
}

