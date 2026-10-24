
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-typed-defaults.idl
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

public class Seven :  IEquatable<Seven>
{

    public int Value { get; set; }

    public Seven()
    {
        Value = (int) (7);
    }

    public Seven(int  Value)
    {
        this.Value = Value;
    }

    public Seven(Seven other)
    {
        if (other == null)
        {
            return;
        }

        this.Value = other.Value;

    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(this.Value);

        return hash.ToHashCode();
    }

    public bool Equals(Seven other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.Value.Equals(other.Value);
    }

    public override bool Equals(object obj) => this.Equals(obj as Seven);

    public override string ToString() => SevenSupport.Instance.ToString(this);
}

public class TextAlias :  IEquatable<TextAlias>
{

    [Bound(8)]
    public string Value { get; set; } = string.Empty;

    public TextAlias()
    {
        Value = (string) ("alias");
    }

    public TextAlias(string  Value)
    {
        this.Value = Value;
    }

    public TextAlias(TextAlias other)
    {
        if (other == null)
        {
            return;
        }

        this.Value = other.Value;

    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(this.Value);

        return hash.ToHashCode();
    }

    public bool Equals(TextAlias other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.Value.Equals(other.Value);
    }

    public override bool Equals(object obj) => this.Equals(obj as TextAlias);

    public override string ToString() => TextAliasSupport.Instance.ToString(this);
}

public class Ratio :  IEquatable<Ratio>
{

    public double Value { get; set; }

    public Ratio()
    {
        Value = (double) (1.5);
    }

    public Ratio(double  Value)
    {
        this.Value = Value;
    }

    public Ratio(Ratio other)
    {
        if (other == null)
        {
            return;
        }

        this.Value = other.Value;

    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(this.Value);

        return hash.ToHashCode();
    }

    public bool Equals(Ratio other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.Value.Equals(other.Value);
    }

    public override bool Equals(object obj) => this.Equals(obj as Ratio);

    public override string ToString() => RatioSupport.Instance.ToString(this);
}

public class TypedDefaults :  IEquatable<TypedDefaults>
{
    public int inherited { get; set; }
    public int overridden { get; set; }
    [Bound(8)]
    public string text { get; set; } = string.Empty;
    public double ratio { get; set; }
    public float single { get; set; }

    public TypedDefaults()
    {
        inherited = (int) (7);
        overridden = (int) (9);
        text = (string) ("alias");
        ratio = (double) (1.5);
        single = (float) (1.25f);
    }

    public TypedDefaults(int  inherited, int  overridden, string  text, double  ratio, float  single)
    {
        this.inherited = inherited;
        this.overridden = overridden;
        this.text = text;
        this.ratio = ratio;
        this.single = single;
    }

    public TypedDefaults(TypedDefaults other)
    {
        if (other == null)
        {
            return;
        }

        this.inherited = other.inherited;
        this.overridden = other.overridden;
        this.text = other.text;
        this.ratio = other.ratio;
        this.single = other.single;

    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(this.inherited);
        hash.Add(this.overridden);
        hash.Add(this.text);
        hash.Add(this.ratio);
        hash.Add(this.single);

        return hash.ToHashCode();
    }

    public bool Equals(TypedDefaults other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.inherited.Equals(other.inherited) && 
        this.overridden.Equals(other.overridden) && 
        this.text.Equals(other.text) && 
        this.ratio.Equals(other.ratio) && 
        this.single.Equals(other.single);
    }

    public override bool Equals(object obj) => this.Equals(obj as TypedDefaults);

    public override string ToString() => TypedDefaultsSupport.Instance.ToString(this);
}

