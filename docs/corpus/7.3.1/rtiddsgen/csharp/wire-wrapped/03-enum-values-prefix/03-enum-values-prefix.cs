
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-enum-values-prefix.idl
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

namespace CorpusEnumValuePrefix
{

    public class EnumTopic :  IEquatable<EnumTopic>
    {
        public global::CorpusEnumValuePrefix.Annotated value { get; set; }

        public EnumTopic()
        {
            value = (global::CorpusEnumValuePrefix.Annotated) (7);
        }

        public EnumTopic(global::CorpusEnumValuePrefix.Annotated  value)
        {
            this.value = value;
        }

        public EnumTopic(EnumTopic other)
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

        public bool Equals(EnumTopic other)
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

        public override bool Equals(object obj) => this.Equals(obj as EnumTopic);

        public override string ToString() => EnumTopicSupport.Instance.ToString(this);
    }

} // namespace CorpusEnumValuePrefix
