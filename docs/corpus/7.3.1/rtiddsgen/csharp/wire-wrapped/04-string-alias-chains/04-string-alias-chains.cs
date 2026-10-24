
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

    public class StringAliasChainsSample :  IEquatable<StringAliasChainsSample>
    {
        [Bound(255)]
        public string narrow { get; set; } = string.Empty;
        [Bound(255)]
        public string narrowChain { get; set; } = string.Empty;
        [Bound(16)]
        public string boundedNarrow { get; set; } = string.Empty;
        [Bound(16)]
        public string boundedNarrowChain { get; set; } = string.Empty;
        [Bound(16)]
        public string boundedNarrowChain2 { get; set; } = string.Empty;
        [Bound(255)]
        public string wide { get; set; } = string.Empty;
        [Bound(255)]
        public string wideChain { get; set; } = string.Empty;
        [Bound(8)]
        public string boundedWide { get; set; } = string.Empty;
        [Bound(8)]
        public string boundedWideChain { get; set; } = string.Empty;
        [Bound(8)]
        public string boundedWideChain2 { get; set; } = string.Empty;

        public StringAliasChainsSample()
        {
        }

        public StringAliasChainsSample(string  narrow, string  narrowChain, string  boundedNarrow, string  boundedNarrowChain, string  boundedNarrowChain2, string  wide, string  wideChain, string  boundedWide, string  boundedWideChain, string  boundedWideChain2)
        {
            this.narrow = narrow;
            this.narrowChain = narrowChain;
            this.boundedNarrow = boundedNarrow;
            this.boundedNarrowChain = boundedNarrowChain;
            this.boundedNarrowChain2 = boundedNarrowChain2;
            this.wide = wide;
            this.wideChain = wideChain;
            this.boundedWide = boundedWide;
            this.boundedWideChain = boundedWideChain;
            this.boundedWideChain2 = boundedWideChain2;
        }

        public StringAliasChainsSample(StringAliasChainsSample other)
        {
            if (other == null)
            {
                return;
            }

            this.narrow = other.narrow;
            this.narrowChain = other.narrowChain;
            this.boundedNarrow = other.boundedNarrow;
            this.boundedNarrowChain = other.boundedNarrowChain;
            this.boundedNarrowChain2 = other.boundedNarrowChain2;
            this.wide = other.wide;
            this.wideChain = other.wideChain;
            this.boundedWide = other.boundedWide;
            this.boundedWideChain = other.boundedWideChain;
            this.boundedWideChain2 = other.boundedWideChain2;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.narrow);
            hash.Add(this.narrowChain);
            hash.Add(this.boundedNarrow);
            hash.Add(this.boundedNarrowChain);
            hash.Add(this.boundedNarrowChain2);
            hash.Add(this.wide);
            hash.Add(this.wideChain);
            hash.Add(this.boundedWide);
            hash.Add(this.boundedWideChain);
            hash.Add(this.boundedWideChain2);

            return hash.ToHashCode();
        }

        public bool Equals(StringAliasChainsSample other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.narrow.Equals(other.narrow) && 
            this.narrowChain.Equals(other.narrowChain) && 
            this.boundedNarrow.Equals(other.boundedNarrow) && 
            this.boundedNarrowChain.Equals(other.boundedNarrowChain) && 
            this.boundedNarrowChain2.Equals(other.boundedNarrowChain2) && 
            this.wide.Equals(other.wide) && 
            this.wideChain.Equals(other.wideChain) && 
            this.boundedWide.Equals(other.boundedWide) && 
            this.boundedWideChain.Equals(other.boundedWideChain) && 
            this.boundedWideChain2.Equals(other.boundedWideChain2);
        }

        public override bool Equals(object obj) => this.Equals(obj as StringAliasChainsSample);

        public override string ToString() => StringAliasChainsSampleSupport.Instance.ToString(this);
    }

} // namespace CorpusStringAliasChains
