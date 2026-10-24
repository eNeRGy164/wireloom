
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-multiple.idl
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

public class WireloomWireCompatibilityMultipleTypes :  IEquatable<WireloomWireCompatibilityMultipleTypes>
{
    public global::First first { get; set; }
    public global::Second second { get; set; }

    public WireloomWireCompatibilityMultipleTypes()
    {
        first = new global::First();
        second = new global::Second();
    }

    public WireloomWireCompatibilityMultipleTypes(global::First  first, global::Second  second)
    {
        this.first = first;
        this.second = second;
    }

    public WireloomWireCompatibilityMultipleTypes(WireloomWireCompatibilityMultipleTypes other)
    {
        if (other == null)
        {
            return;
        }

        this.first = new global::First(other.first);
        this.second = new global::Second(other.second);

    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(this.first);
        hash.Add(this.second);

        return hash.ToHashCode();
    }

    public bool Equals(WireloomWireCompatibilityMultipleTypes other)
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

    public override bool Equals(object obj) => this.Equals(obj as WireloomWireCompatibilityMultipleTypes);

    public override string ToString() => WireloomWireCompatibilityMultipleTypesSupport.Instance.ToString(this);
}

