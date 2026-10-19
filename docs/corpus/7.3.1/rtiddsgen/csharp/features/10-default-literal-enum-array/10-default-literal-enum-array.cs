
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-default-literal-enum-array.idl
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

namespace CorpusDefaultLiteralEnumArray
{

    public enum Color
    {
        GREEN,
        RED,
        BLUE
    }

    public class ColorArray :  IEquatable<ColorArray>
    {

        public global::CorpusDefaultLiteralEnumArray.Color[] Value { get; set; }

        public ColorArray()
        {
            Value = new global::CorpusDefaultLiteralEnumArray.Color[5];
            for( int i1 = 0; i1 < 5; i1++)
            {
                Value[i1] = (0);
            }
            ;
        }

        public ColorArray(global::CorpusDefaultLiteralEnumArray.Color [] Value)
        {
            this.Value = Value;
        }

        public ColorArray(ColorArray other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new global::CorpusDefaultLiteralEnumArray.Color[5];
            for( int i1 = 0; i1 < 5; i1++)
            {
                Value[i1] = other.Value[i1];
            }

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.Value[0]);

            return hash.ToHashCode();
        }

        public bool Equals(ColorArray other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Value.SequenceEqual(other.Value);
        }

        public override bool Equals(object obj) => this.Equals(obj as ColorArray);

        public override string ToString() => ColorArraySupport.Instance.ToString(this);
    }

    public class Shape :  IEquatable<Shape>
    {
        public global::CorpusDefaultLiteralEnumArray.Color color { get; set; }
        public global::CorpusDefaultLiteralEnumArray.ColorArray colorArray { get; set; }

        public Shape()
        {
            color = (global::CorpusDefaultLiteralEnumArray.Color) (1);
            colorArray = new global::CorpusDefaultLiteralEnumArray.ColorArray();
        }

        public Shape(global::CorpusDefaultLiteralEnumArray.Color  color, global::CorpusDefaultLiteralEnumArray.ColorArray  colorArray)
        {
            this.color = color;
            this.colorArray = colorArray;
        }

        public Shape(Shape other)
        {
            if (other == null)
            {
                return;
            }

            this.color = other.color;
            this.colorArray = new global::CorpusDefaultLiteralEnumArray.ColorArray(other.colorArray);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.color);
            hash.Add(this.colorArray);

            return hash.ToHashCode();
        }

        public bool Equals(Shape other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.color.Equals(other.color) && 
            this.colorArray.Equals(other.colorArray);
        }

        public override bool Equals(object obj) => this.Equals(obj as Shape);

        public override string ToString() => ShapeSupport.Instance.ToString(this);
    }

} // namespace CorpusDefaultLiteralEnumArray
