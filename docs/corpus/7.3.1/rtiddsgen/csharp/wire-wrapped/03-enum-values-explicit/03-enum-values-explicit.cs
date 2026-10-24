
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-enum-values-explicit.idl
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

namespace CorpusEnumValues
{

    public class EnumTopic :  IEquatable<EnumTopic>
    {
        public global::CorpusEnumValues.Explicit value { get; set; }

        public EnumTopic()
        {
            value = (global::CorpusEnumValues.Explicit) (-2);
        }

        public EnumTopic(global::CorpusEnumValues.Explicit  value)
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

} // namespace CorpusEnumValues
