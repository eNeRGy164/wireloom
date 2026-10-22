
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-union-label-expressions.idl
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

public static class Selected
{
    public const int Value = 0x2a;
}

public class LabelExpressions :  IEquatable<LabelExpressions>
{

    private int _symbolic;

    private int _arithmetic;

    private int _hexadecimal;

    public int Discriminator { get; private set; }

    public const int DefaultDiscriminator = 0;

    public int symbolic
    {
        get
        {
            if (Discriminator != (Selected.Value)) 
            {
                throw new InvalidOperationException("symbolic not selected");
            }
            return _symbolic;
        }

        set
        {
            _symbolic = value;
            Discriminator = (Selected.Value);
        }
    }

    public int arithmetic
    {
        get
        {
            if (Discriminator != (3+1)) 
            {
                throw new InvalidOperationException("arithmetic not selected");
            }
            return _arithmetic;
        }

        set
        {
            _arithmetic = value;
            Discriminator = (3+1);
        }
    }

    public int hexadecimal
    {
        get
        {
            if (Discriminator != 0x10) 
            {
                throw new InvalidOperationException("hexadecimal not selected");
            }
            return _hexadecimal;
        }

        set
        {
            _hexadecimal = value;
            Discriminator = 0x10;
        }
    }

    public LabelExpressions()
    {
        Discriminator = DefaultDiscriminator;
    }

    public LabelExpressions(LabelExpressions other)
    {
        if (other == null)
        {
            return;
        }

        this.Discriminator = other.Discriminator;
        switch (Discriminator)
        {
            case (Selected.Value):
                this._symbolic = other.symbolic;
                break;
            case (3+1):
                this._arithmetic = other.arithmetic;
                break;
            case 0x10:
            this._hexadecimal = other.hexadecimal;
            break;
        }
    }

    public object Get()
    {
        switch (Discriminator)
        {
            case (Selected.Value):
                return symbolic;
            case (3+1):
                return arithmetic;
            case 0x10:
            return hexadecimal;

            default:
            return null;
        }
    }

    public override int GetHashCode()
    {
        switch (Discriminator)
        {
            case (Selected.Value):
                return HashCode.Combine(Discriminator, this.symbolic);
            case (3+1):
                return HashCode.Combine(Discriminator, this.arithmetic);
            case 0x10:
            return HashCode.Combine(Discriminator, this.hexadecimal);
        }
        return HashCode.Combine(Discriminator);
    }

    public bool Equals(LabelExpressions other)
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
                return this.symbolic.Equals(other.symbolic);
            case (3+1):
                return this.arithmetic.Equals(other.arithmetic);
            case 0x10:
            return this.hexadecimal.Equals(other.hexadecimal);
        }
        return true;
    }

    public override bool Equals(object obj) => this.Equals(obj as LabelExpressions);

    public override string ToString() => LabelExpressionsSupport.Instance.ToString(this);
}

