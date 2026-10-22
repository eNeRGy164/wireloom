
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-union-array-branch.idl
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

public class ArrayBranch :  IEquatable<ArrayBranch>
{

    private int[,] _values;

    private int _scalar;

    public int Discriminator { get; private set; }

    public const int DefaultDiscriminator = 0;

    public int[,] values
    {
        get
        {
            if (Discriminator != 0) 
            {
                throw new InvalidOperationException("values not selected");
            }
            return _values;
        }

        set
        {
            _values = value;
            Discriminator = 0;
        }
    }

    public int scalar
    {
        get
        {
            if (Discriminator != 1) 
            {
                throw new InvalidOperationException("scalar not selected");
            }
            return _scalar;
        }

        set
        {
            _scalar = value;
            Discriminator = 1;
        }
    }

    public ArrayBranch()
    {
        Discriminator = DefaultDiscriminator;

        _values = new int[2,3];
        for( int i1 = 0; i1 < 2; i1++)
        {
            for( int i2 = 0; i2 < 3; i2++)
            {
                values[i1, i2] = (0);
            }}
        ;
    }

    public ArrayBranch(ArrayBranch other)
    {
        if (other == null)
        {
            return;
        }

        this.Discriminator = other.Discriminator;
        switch (Discriminator)
        {
            case 0:
            this._values = new int[2,3];
            for( int i1 = 0; i1 < 2; i1++)
            {
                for( int i2 = 0; i2 < 3; i2++)
                {
                    values[i1, i2] = other.values[i1, i2];
                }}
            break;
            case 1:
            this._scalar = other.scalar;
            break;
        }
    }

    public object Get()
    {
        switch (Discriminator)
        {
            case 0:
            return values;
            case 1:
            return scalar;

            default:
            return null;
        }
    }

    public override int GetHashCode()
    {
        switch (Discriminator)
        {
            case 0:
            return HashCode.Combine(Discriminator, this.values[0, 0]);
            case 1:
            return HashCode.Combine(Discriminator, this.scalar);
        }
        return HashCode.Combine(Discriminator);
    }

    public bool Equals(ArrayBranch other)
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
            case 0:
            return this.values.Rank == other.values.Rank && Enumerable.Range(0,this.values.Rank).All(dimension => this.values.GetLength(dimension) == other.values.GetLength(dimension)) && this.values.Cast<int>().SequenceEqual(other.values.Cast<int>());
            case 1:
            return this.scalar.Equals(other.scalar);
        }
        return true;
    }

    public override bool Equals(object obj) => this.Equals(obj as ArrayBranch);

    public override string ToString() => ArrayBranchSupport.Instance.ToString(this);
}

