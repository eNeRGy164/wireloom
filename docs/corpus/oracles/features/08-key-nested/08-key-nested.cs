
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-nested.idl
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

namespace CorpusNestedKeys
{

    public class Inner :  IEquatable<Inner>
    {
        [Key]
        public int code { get; set; }
        public int revision { get; set; }

        public Inner()
        {
        }

        public Inner(int  code, int  revision)
        {
            this.code = code;
            this.revision = revision;
        }

        public Inner(Inner other)
        {
            if (other == null)
            {
                return;
            }

            this.code = other.code;
            this.revision = other.revision;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.code);
            hash.Add(this.revision);

            return hash.ToHashCode();
        }

        public bool Equals(Inner other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.code.Equals(other.code) && 
            this.revision.Equals(other.revision);
        }

        public override bool Equals(object obj) => this.Equals(obj as Inner);

        public override string ToString() => InnerSupport.Instance.ToString(this);
    }

    public class Outer :  IEquatable<Outer>
    {
        [Key]
        public global::CorpusNestedKeys.Inner identity { get; set; }
        public int payload { get; set; }

        public Outer()
        {
            identity = new global::CorpusNestedKeys.Inner();
        }

        public Outer(global::CorpusNestedKeys.Inner  identity, int  payload)
        {
            this.identity = identity;
            this.payload = payload;
        }

        public Outer(Outer other)
        {
            if (other == null)
            {
                return;
            }

            this.identity = new global::CorpusNestedKeys.Inner(other.identity);
            this.payload = other.payload;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.identity);
            hash.Add(this.payload);

            return hash.ToHashCode();
        }

        public bool Equals(Outer other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.identity.Equals(other.identity) && 
            this.payload.Equals(other.payload);
        }

        public override bool Equals(object obj) => this.Equals(obj as Outer);

        public override string ToString() => OuterSupport.Instance.ToString(this);
    }

} // namespace CorpusNestedKeys
