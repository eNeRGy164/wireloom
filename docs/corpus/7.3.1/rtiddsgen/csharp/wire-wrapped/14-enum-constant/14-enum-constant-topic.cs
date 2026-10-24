
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-enum-constant-topic.idl
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

public class EnumConstantSample :  IEquatable<EnumConstantSample>
{
    public global::EnumConstant value { get; set; }
    public global::EnumUnion choice { get; set; }

    public EnumConstantSample()
    {
        value = new global::EnumConstant();
        choice = new global::EnumUnion();
    }

    public EnumConstantSample(global::EnumConstant  value, global::EnumUnion  choice)
    {
        this.value = value;
        this.choice = choice;
    }

    public EnumConstantSample(EnumConstantSample other)
    {
        if (other == null)
        {
            return;
        }

        this.value = new global::EnumConstant(other.value);
        this.choice = new global::EnumUnion(other.choice);

    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(this.value);
        hash.Add(this.choice);

        return hash.ToHashCode();
    }

    public bool Equals(EnumConstantSample other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.value.Equals(other.value) && 
        this.choice.Equals(other.choice);
    }

    public override bool Equals(object obj) => this.Equals(obj as EnumConstantSample);

    public override string ToString() => EnumConstantSampleSupport.Instance.ToString(this);
}

