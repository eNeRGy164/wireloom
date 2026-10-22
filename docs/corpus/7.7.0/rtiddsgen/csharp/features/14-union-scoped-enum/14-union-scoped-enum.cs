
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-union-scoped-enum.idl
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

public enum Choice
{
    FIRST,
    SECOND
}

public static class Selected
{
    public const global::Choice Value = Choice.SECOND;
}

public class ScopedEnum :  IEquatable<ScopedEnum>
{

    private int _selected;

    private int _first;

    public global::Choice Discriminator { get; private set; }

    public const global::Choice DefaultDiscriminator = (global::Choice) Choice.FIRST;

    public int selected
    {
        get
        {
            if (Discriminator != (Selected.Value)) 
            {
                throw new InvalidOperationException("selected not selected");
            }
            return _selected;
        }

        set
        {
            _selected = value;
            Discriminator = (Selected.Value);
        }
    }

    public int first
    {
        get
        {
            if (Discriminator != Choice.FIRST) 
            {
                throw new InvalidOperationException("first not selected");
            }
            return _first;
        }

        set
        {
            _first = value;
            Discriminator = Choice.FIRST;
        }
    }

    public ScopedEnum()
    {
        Discriminator = DefaultDiscriminator;
    }

    public ScopedEnum(ScopedEnum other)
    {
        if (other == null)
        {
            return;
        }

        this.Discriminator = other.Discriminator;
        switch (Discriminator)
        {
            case (Selected.Value):
                this._selected = other.selected;
                break;
            case Choice.FIRST:
            this._first = other.first;
            break;
        }
    }

    public object Get()
    {
        switch (Discriminator)
        {
            case (Selected.Value):
                return selected;
            case Choice.FIRST:
            return first;

            default:
            return null;
        }
    }

    public override int GetHashCode()
    {
        switch (Discriminator)
        {
            case (Selected.Value):
                return HashCode.Combine(Discriminator, this.selected);
            case Choice.FIRST:
            return HashCode.Combine(Discriminator, this.first);
        }
        return HashCode.Combine(Discriminator);
    }

    public bool Equals(ScopedEnum other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (this.Discriminator != other.Discriminator)
        {
            return false;
        }

        switch (Discriminator)
        {
            case (Selected.Value):
                return this.selected.Equals(other.selected);
            case Choice.FIRST:
            return this.first.Equals(other.first);
        }
        return true;
    }

    public override bool Equals(object obj) => this.Equals(obj as ScopedEnum);

    public override string ToString() => ScopedEnumSupport.Instance.ToString(this);
}

