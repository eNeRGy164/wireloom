
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-enums-aliases.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
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

namespace CorpusEnums
{

    public enum Color
    {
        RED,
        GREEN,
        BLUE
    }

    public class Scalar :  IEquatable<Scalar>
    {

        public int Value { get; set; }

        public Scalar()
        {
        }

        public Scalar(int  Value)
        {
            this.Value = Value;
        }

        public Scalar(Scalar other)
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

            hash.Add(Value);

            return hash.ToHashCode();
        }

        public bool Equals(Scalar other)
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

        public override bool Equals(object obj) => this.Equals(obj as Scalar);

        public override string ToString() => ScalarSupport.Instance.ToString(this);
    }

    public class ColorSequence :  IEquatable<ColorSequence>
    {

        [Bound(3)]
        public ISequence<CorpusEnums.Color> Value { get; }

        public ColorSequence()
        {
            Value = new Rti.Types.Sequence<CorpusEnums.Color>();
        }

        public ColorSequence(ISequence<CorpusEnums.Color>Value)
        {
            this.Value = Value;
        }

        public ColorSequence(ColorSequence other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new Rti.Types.Sequence<CorpusEnums.Color>(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(Value.Count);

            return hash.ToHashCode();
        }

        public bool Equals(ColorSequence other)
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

        public override bool Equals(object obj) => this.Equals(obj as ColorSequence);

        public override string ToString() => ColorSequenceSupport.Instance.ToString(this);
    }

    public class ScalarAlias :  IEquatable<ScalarAlias>
    {

        public int Value { get; set; }

        public ScalarAlias()
        {
        }

        public ScalarAlias(int  Value)
        {
            this.Value = Value;
        }

        public ScalarAlias(ScalarAlias other)
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

            hash.Add(Value);

            return hash.ToHashCode();
        }

        public bool Equals(ScalarAlias other)
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

        public override bool Equals(object obj) => this.Equals(obj as ScalarAlias);

        public override string ToString() => ScalarAliasSupport.Instance.ToString(this);
    }

    public class ColorAlias :  IEquatable<ColorAlias>
    {

        public CorpusEnums.Color Value { get; set; }

        public ColorAlias()
        {
            Value = (CorpusEnums.Color) 0;
        }

        public ColorAlias(CorpusEnums.Color  Value)
        {
            this.Value = Value;
        }

        public ColorAlias(ColorAlias other)
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

            hash.Add(Value);

            return hash.ToHashCode();
        }

        public bool Equals(ColorAlias other)
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

        public override bool Equals(object obj) => this.Equals(obj as ColorAlias);

        public override string ToString() => ColorAliasSupport.Instance.ToString(this);
    }

    public class ColorAlias2 :  IEquatable<ColorAlias2>
    {

        public CorpusEnums.Color Value { get; set; }

        public ColorAlias2()
        {
            Value = (CorpusEnums.Color) 0;
        }

        public ColorAlias2(CorpusEnums.Color  Value)
        {
            this.Value = Value;
        }

        public ColorAlias2(ColorAlias2 other)
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

            hash.Add(Value);

            return hash.ToHashCode();
        }

        public bool Equals(ColorAlias2 other)
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

        public override bool Equals(object obj) => this.Equals(obj as ColorAlias2);

        public override string ToString() => ColorAlias2Support.Instance.ToString(this);
    }

    public class Record :  IEquatable<Record>
    {
        public int value { get; set; }

        public int aliasValue { get; set; }

        public CorpusEnums.Color color { get; set; }

        public CorpusEnums.Color aliasColor { get; set; }

        public CorpusEnums.ColorSequence colors { get; set; }

        public Record()
        {
            color = (CorpusEnums.Color) 0;
            aliasColor = (CorpusEnums.Color) 0;
            colors = new CorpusEnums.ColorSequence();
        }

        public Record(int  value, int  aliasValue, CorpusEnums.Color  color, CorpusEnums.Color  aliasColor, CorpusEnums.ColorSequence  colors)
        {
            this.value = value;
            this.aliasValue = aliasValue;
            this.color = color;
            this.aliasColor = aliasColor;
            this.colors = colors;
        }

        public Record(Record other)
        {
            if (other == null)
            {
                return;
            }

            this.value = other.value;
            this.aliasValue = other.aliasValue;
            this.color = other.color;
            this.aliasColor = other.aliasColor;
            this.colors = other.colors;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(value);
            hash.Add(aliasValue);
            hash.Add(color);
            hash.Add(aliasColor);
            hash.Add(colors);

            return hash.ToHashCode();
        }

        public bool Equals(Record other)
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
            this.aliasValue.Equals(other.aliasValue) &&
            this.color.Equals(other.color) &&
            this.aliasColor.Equals(other.aliasColor) &&
            this.colors.Equals(other.colors);
        }

        public override bool Equals(object obj) => this.Equals(obj as Record);

        public override string ToString() => RecordSupport.Instance.ToString(this);
    }

} // namespace CorpusEnums
