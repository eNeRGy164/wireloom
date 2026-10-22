
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-string-expression-bound.idl
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

public static class StringBound
{
    public const int Value = 2+3;
}

public class StringBounded :  IEquatable<StringBounded>
{
    [Bound((StringBound.Value))]
    public string text { get; set; } = string.Empty;
    [Bound(2*3)]
    public string wideText { get; set; } = string.Empty;

    public StringBounded()
    {
    }

    public StringBounded(string  text, string  wideText)
    {
        this.text = text;
        this.wideText = wideText;
    }

    public StringBounded(StringBounded other)
    {
        if (other == null)
        {
            return;
        }

        this.text = other.text;
        this.wideText = other.wideText;

    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(this.text);
        hash.Add(this.wideText);

        return hash.ToHashCode();
    }

    public bool Equals(StringBounded other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.text.Equals(other.text) && 
        this.wideText.Equals(other.wideText);
    }

    public override bool Equals(object obj) => this.Equals(obj as StringBounded);

    public override string ToString() => StringBoundedSupport.Instance.ToString(this);
}

