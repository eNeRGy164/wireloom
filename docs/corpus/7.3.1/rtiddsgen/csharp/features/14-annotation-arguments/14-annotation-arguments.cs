
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-annotation-arguments.idl
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

public static class Selected
{
    public const int Value = 0x2a;
}

public class AnnotationArguments :  IEquatable<AnnotationArguments>
{
    public int plain { get; set; }
    public int selected { get; set; }

    public AnnotationArguments()
    {
        selected = (int) ((Selected.Value));
    }

    public AnnotationArguments(int  plain, int  selected)
    {
        this.plain = plain;
        this.selected = selected;
    }

    public AnnotationArguments(AnnotationArguments other)
    {
        if (other == null)
        {
            return;
        }

        this.plain = other.plain;
        this.selected = other.selected;

    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(this.plain);
        hash.Add(this.selected);

        return hash.ToHashCode();
    }

    public bool Equals(AnnotationArguments other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.plain.Equals(other.plain) && 
        this.selected.Equals(other.selected);
    }

    public override bool Equals(object obj) => this.Equals(obj as AnnotationArguments);

    public override string ToString() => AnnotationArgumentsSupport.Instance.ToString(this);
}

