
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-resolve-name-wstring-aliases.idl
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

namespace CorpusResolveNameWstringAliases
{
    public static class WSTRING_BOUND
    {
        public const int Value = 8;
    }

    public class BoundedWideText :  IEquatable<BoundedWideText>
    {

        [Bound((CorpusResolveNameWstringAliases.WSTRING_BOUND.Value))]
        public string Value { get; set; } = string.Empty;

        public BoundedWideText()
        {
        }

        public BoundedWideText(string  Value)
        {
            this.Value = Value;
        }

        public BoundedWideText(BoundedWideText other)
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

        public bool Equals(BoundedWideText other)
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

        public override bool Equals(object obj) => this.Equals(obj as BoundedWideText);

        public override string ToString() => BoundedWideTextSupport.Instance.ToString(this);
    }

    public class BoundedWideTextAlias :  IEquatable<BoundedWideTextAlias>
    {

        [Bound((CorpusResolveNameWstringAliases.WSTRING_BOUND.Value))]
        public string Value { get; set; } = string.Empty;

        public BoundedWideTextAlias()
        {
        }

        public BoundedWideTextAlias(string  Value)
        {
            this.Value = Value;
        }

        public BoundedWideTextAlias(BoundedWideTextAlias other)
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

        public bool Equals(BoundedWideTextAlias other)
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

        public override bool Equals(object obj) => this.Equals(obj as BoundedWideTextAlias);

        public override string ToString() => BoundedWideTextAliasSupport.Instance.ToString(this);
    }

    public class Sample :  IEquatable<Sample>
    {
        public BoundedWideText firstLevel { get; set; }
        public BoundedWideTextAlias secondLevel { get; set; }

        public Sample()
        {
            firstLevel = new BoundedWideText();
            secondLevel = new BoundedWideTextAlias();
        }

        public Sample(BoundedWideText  firstLevel, BoundedWideTextAlias  secondLevel)
        {
            this.firstLevel = firstLevel;
            this.secondLevel = secondLevel;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.firstLevel = new BoundedWideText(other.firstLevel);
            this.secondLevel = new BoundedWideTextAlias(other.secondLevel);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.firstLevel);
            hash.Add(this.secondLevel);

            return hash.ToHashCode();
        }

        public bool Equals(Sample other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.firstLevel.Equals(other.firstLevel) && 
            this.secondLevel.Equals(other.secondLevel);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusResolveNameWstringAliases
