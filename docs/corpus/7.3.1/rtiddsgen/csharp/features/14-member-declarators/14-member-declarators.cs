
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-member-declarators.idl
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

public class Declarators :  IEquatable<Declarators>
{
    public int first { get; set; }
    [Key]
    public int second { get; set; }
    public int[] numbers { get; set; }
    public int[,] matrix { get; set; }

    public Declarators()
    {
        numbers = new int[2];
        for( int i1 = 0; i1 < 2; i1++)
        {
            numbers[i1] = (0);
        }
        ;
        matrix = new int[2,3];
        for( int i1 = 0; i1 < 2; i1++)
        {
            for( int i2 = 0; i2 < 3; i2++)
            {
                matrix[i1, i2] = (0);
            }}
        ;
    }

    public Declarators(int  first, int  second, int [] numbers, int [,] matrix)
    {
        this.first = first;
        this.second = second;
        this.numbers = numbers;
        this.matrix = matrix;
    }

    public Declarators(Declarators other)
    {
        if (other == null)
        {
            return;
        }

        this.first = other.first;
        this.second = other.second;
        this.numbers = new int[2];
        for( int i1 = 0; i1 < 2; i1++)
        {
            numbers[i1] = other.numbers[i1];
        }
        this.matrix = new int[2,3];
        for( int i1 = 0; i1 < 2; i1++)
        {
            for( int i2 = 0; i2 < 3; i2++)
            {
                matrix[i1, i2] = other.matrix[i1, i2];
            }}

    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(this.first);
        hash.Add(this.second);
        hash.Add(this.numbers[0]);
        hash.Add(this.matrix[0, 0]);

        return hash.ToHashCode();
    }

    public bool Equals(Declarators other)
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
        this.second.Equals(other.second) && 
        this.numbers.SequenceEqual(other.numbers) && 
        this.matrix.Rank == other.matrix.Rank && Enumerable.Range(0,this.matrix.Rank).All(dimension => this.matrix.GetLength(dimension) == other.matrix.GetLength(dimension)) && this.matrix.Cast<int>().SequenceEqual(other.matrix.Cast<int>());
    }

    public override bool Equals(object obj) => this.Equals(obj as Declarators);

    public override string ToString() => DeclaratorsSupport.Instance.ToString(this);
}

