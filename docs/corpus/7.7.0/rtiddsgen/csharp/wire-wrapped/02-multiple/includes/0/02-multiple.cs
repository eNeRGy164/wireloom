
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

public class First :  IEquatable<First>
{
    public int value { get; set; }

    public First()
    {
    }

    public First(int  value)
    {
        this.value = value;
    }

    public First(First other)
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

    public bool Equals(First other)
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

    public override bool Equals(object obj) => this.Equals(obj as First);

    public override string ToString() => FirstSupport.Instance.ToString(this);
}

public class Second :  IEquatable<Second>
{
    [Bound(255)]
    public string name { get; set; } = string.Empty;

    public Second()
    {
    }

    public Second(string  name)
    {
        this.name = name;
    }

    public Second(Second other)
    {
        if (other == null)
        {
            return;
        }

        this.name = other.name;

    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(this.name);

        return hash.ToHashCode();
    }

    public bool Equals(Second other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.name.Equals(other.name);
    }

    public override bool Equals(object obj) => this.Equals(obj as Second);

    public override string ToString() => SecondSupport.Instance.ToString(this);
}

