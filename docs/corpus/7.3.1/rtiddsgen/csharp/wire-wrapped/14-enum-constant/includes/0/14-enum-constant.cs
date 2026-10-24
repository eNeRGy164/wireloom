
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-enum-constant.idl
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

public enum Color
{
    RED,
    GREEN
}
public static class Selected
{
    public const global::Color Value = Color.GREEN;
}

public class EnumConstant :  IEquatable<EnumConstant>
{
    public global::Color color { get; set; }

    public EnumConstant()
    {
        color = (global::Color) (0);
    }

    public EnumConstant(global::Color  color)
    {
        this.color = color;
    }

    public EnumConstant(EnumConstant other)
    {
        if (other == null)
        {
            return;
        }

        this.color = other.color;

    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(this.color);

        return hash.ToHashCode();
    }

    public bool Equals(EnumConstant other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.color.Equals(other.color);
    }

    public override bool Equals(object obj) => this.Equals(obj as EnumConstant);

    public override string ToString() => EnumConstantSupport.Instance.ToString(this);
}

public class EnumUnion :  IEquatable<EnumUnion>
{

    private int _selected;

    private int _red;

    public global::Color Discriminator { get; private set; }

    public const global::Color DefaultDiscriminator = (Selected.Value);

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

    public int red
    {
        get
        {
            if (Discriminator != Color.RED) 
            {
                throw new InvalidOperationException("red not selected");
            }
            return _red;
        }

        set
        {
            _red = value;
            Discriminator = Color.RED;
        }
    }

    public EnumUnion()
    {
        Discriminator = DefaultDiscriminator;
    }

    public EnumUnion(EnumUnion other)
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
            case Color.RED:
            this._red = other.red;
            break;
        }
    }

    public object Get()
    {
        switch (Discriminator)
        {
            case (Selected.Value):
                return selected;
            case Color.RED:
            return red;

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
            case Color.RED:
            return HashCode.Combine(Discriminator, this.red);
        }
        return HashCode.Combine(Discriminator);
    }

    public bool Equals(EnumUnion other)
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
            case Color.RED:
            return this.red.Equals(other.red);
        }
        return true;
    }

    public override bool Equals(object obj) => this.Equals(obj as EnumUnion);

    public override string ToString() => EnumUnionSupport.Instance.ToString(this);
}

