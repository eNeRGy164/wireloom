
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-string-alias-chains.idl
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

namespace CorpusStringAliasChains
{

    public class NarrowText :  IEquatable<NarrowText>
    {

        [Bound(255)]
        public string Value { get; set; } = string.Empty;

        public NarrowText()
        {
        }

        public NarrowText(string  Value)
        {
            this.Value = Value;
        }

        public NarrowText(NarrowText other)
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

        public bool Equals(NarrowText other)
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

        public override bool Equals(object obj) => this.Equals(obj as NarrowText);

        public override string ToString() => NarrowTextSupport.Instance.ToString(this);
    }

    public class NarrowTextChain :  IEquatable<NarrowTextChain>
    {

        [Bound(255)]
        public string Value { get; set; } = string.Empty;

        public NarrowTextChain()
        {
        }

        public NarrowTextChain(string  Value)
        {
            this.Value = Value;
        }

        public NarrowTextChain(NarrowTextChain other)
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

        public bool Equals(NarrowTextChain other)
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

        public override bool Equals(object obj) => this.Equals(obj as NarrowTextChain);

        public override string ToString() => NarrowTextChainSupport.Instance.ToString(this);
    }

    public class BoundedNarrowText :  IEquatable<BoundedNarrowText>
    {

        [Bound(16)]
        public string Value { get; set; } = string.Empty;

        public BoundedNarrowText()
        {
        }

        public BoundedNarrowText(string  Value)
        {
            this.Value = Value;
        }

        public BoundedNarrowText(BoundedNarrowText other)
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

        public bool Equals(BoundedNarrowText other)
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

        public override bool Equals(object obj) => this.Equals(obj as BoundedNarrowText);

        public override string ToString() => BoundedNarrowTextSupport.Instance.ToString(this);
    }

    public class BoundedNarrowTextChain :  IEquatable<BoundedNarrowTextChain>
    {

        [Bound(16)]
        public string Value { get; set; } = string.Empty;

        public BoundedNarrowTextChain()
        {
        }

        public BoundedNarrowTextChain(string  Value)
        {
            this.Value = Value;
        }

        public BoundedNarrowTextChain(BoundedNarrowTextChain other)
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

        public bool Equals(BoundedNarrowTextChain other)
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

        public override bool Equals(object obj) => this.Equals(obj as BoundedNarrowTextChain);

        public override string ToString() => BoundedNarrowTextChainSupport.Instance.ToString(this);
    }

    public class BoundedNarrowTextChain2 :  IEquatable<BoundedNarrowTextChain2>
    {

        [Bound(16)]
        public string Value { get; set; } = string.Empty;

        public BoundedNarrowTextChain2()
        {
        }

        public BoundedNarrowTextChain2(string  Value)
        {
            this.Value = Value;
        }

        public BoundedNarrowTextChain2(BoundedNarrowTextChain2 other)
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

        public bool Equals(BoundedNarrowTextChain2 other)
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

        public override bool Equals(object obj) => this.Equals(obj as BoundedNarrowTextChain2);

        public override string ToString() => BoundedNarrowTextChain2Support.Instance.ToString(this);
    }

    public class WideText :  IEquatable<WideText>
    {

        [Bound(255)]
        public string Value { get; set; } = string.Empty;

        public WideText()
        {
        }

        public WideText(string  Value)
        {
            this.Value = Value;
        }

        public WideText(WideText other)
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

        public bool Equals(WideText other)
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

        public override bool Equals(object obj) => this.Equals(obj as WideText);

        public override string ToString() => WideTextSupport.Instance.ToString(this);
    }

    public class WideTextChain :  IEquatable<WideTextChain>
    {

        [Bound(255)]
        public string Value { get; set; } = string.Empty;

        public WideTextChain()
        {
        }

        public WideTextChain(string  Value)
        {
            this.Value = Value;
        }

        public WideTextChain(WideTextChain other)
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

        public bool Equals(WideTextChain other)
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

        public override bool Equals(object obj) => this.Equals(obj as WideTextChain);

        public override string ToString() => WideTextChainSupport.Instance.ToString(this);
    }

    public class BoundedWideText :  IEquatable<BoundedWideText>
    {

        [Bound(8)]
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

    public class BoundedWideTextChain :  IEquatable<BoundedWideTextChain>
    {

        [Bound(8)]
        public string Value { get; set; } = string.Empty;

        public BoundedWideTextChain()
        {
        }

        public BoundedWideTextChain(string  Value)
        {
            this.Value = Value;
        }

        public BoundedWideTextChain(BoundedWideTextChain other)
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

        public bool Equals(BoundedWideTextChain other)
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

        public override bool Equals(object obj) => this.Equals(obj as BoundedWideTextChain);

        public override string ToString() => BoundedWideTextChainSupport.Instance.ToString(this);
    }

    public class BoundedWideTextChain2 :  IEquatable<BoundedWideTextChain2>
    {

        [Bound(8)]
        public string Value { get; set; } = string.Empty;

        public BoundedWideTextChain2()
        {
        }

        public BoundedWideTextChain2(string  Value)
        {
            this.Value = Value;
        }

        public BoundedWideTextChain2(BoundedWideTextChain2 other)
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

        public bool Equals(BoundedWideTextChain2 other)
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

        public override bool Equals(object obj) => this.Equals(obj as BoundedWideTextChain2);

        public override string ToString() => BoundedWideTextChain2Support.Instance.ToString(this);
    }

} // namespace CorpusStringAliasChains
