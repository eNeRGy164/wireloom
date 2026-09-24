
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 11-conditionals.idl
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

namespace CorpusConditionals
{

    public class Enabled :  IEquatable<Enabled>
    {
        public int value { get; set; }

        public Enabled()
        {
        }

        public Enabled(int  value)
        {
            this.value = value;
        }

        public Enabled(Enabled other)
        {
            if (other == null)
            {
                return;
            }

            this.value = other.value;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.value);

            return hash.ToHashCode();
        }

        public bool Equals(Enabled other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.value.Equals(other.value);
        }

        public override bool Equals(object obj) => this.Equals(obj as Enabled);

        public override string ToString() => EnabledSupport.Instance.ToString(this);
    }

} // namespace CorpusConditionals

namespace CorpusConditionals
{

    public class UndefBranch :  IEquatable<UndefBranch>
    {
        public int value { get; set; }

        public UndefBranch()
        {
        }

        public UndefBranch(int  value)
        {
            this.value = value;
        }

        public UndefBranch(UndefBranch other)
        {
            if (other == null)
            {
                return;
            }

            this.value = other.value;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.value);

            return hash.ToHashCode();
        }

        public bool Equals(UndefBranch other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.value.Equals(other.value);
        }

        public override bool Equals(object obj) => this.Equals(obj as UndefBranch);

        public override string ToString() => UndefBranchSupport.Instance.ToString(this);
    }

} // namespace CorpusConditionals
