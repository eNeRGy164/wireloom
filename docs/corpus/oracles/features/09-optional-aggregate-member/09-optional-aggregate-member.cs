
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-optional-aggregate-member.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.1.
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

namespace CorpusOptionalAggregate
{

    public class Payload :  IEquatable<Payload>
    {
        public int value { get; set; }

        public Payload()
        {
        }

        public Payload(int  value)
        {
            this.value = value;
        }

        public Payload(Payload other)
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

        public bool Equals(Payload other)
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

        public override bool Equals(object obj) => this.Equals(obj as Payload);

        public override string ToString() => PayloadSupport.Instance.ToString(this);
    }

    public class Holder :  IEquatable<Holder>
    {
        [Optional]
        public global::CorpusOptionalAggregate.Payload payload { get; set; }

        public Holder()
        {
        }

        public Holder(global::CorpusOptionalAggregate.Payload  payload)
        {
            this.payload = payload;
        }

        public Holder(Holder other)
        {
            if (other == null)
            {
                return;
            }

            if(other.payload != null)
            {
                this.payload = new global::CorpusOptionalAggregate.Payload(other.payload);
            }

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.payload);

            return hash.ToHashCode();
        }

        public bool Equals(Holder other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.payload, other.payload);
        }

        public override bool Equals(object obj) => this.Equals(obj as Holder);

        public override string ToString() => HolderSupport.Instance.ToString(this);
    }

} // namespace CorpusOptionalAggregate
