
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from common.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.1.
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

public static class IncludedConstant
{
    public const int Value = 7;
}

public class IncludedType :  IEquatable<IncludedType>
{
    public int value { get; set; }

    public IncludedType()
    {
    }

    public IncludedType(int  value)
    {
        this.value = value;
    }

    public IncludedType(IncludedType other)
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

    public bool Equals(IncludedType other)
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

    public override bool Equals(object obj) => this.Equals(obj as IncludedType);

    public override string ToString() => IncludedTypeSupport.Instance.ToString(this);
}

