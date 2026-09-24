
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-formatting.idl
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

namespace CorpusFormatting
{

    public class Value :  IEquatable<Value>
    {
        [Bound(16)]
        public string text { get; set; } = string.Empty;

        public Value()
        {
        }

        public Value(string  text)
        {
            this.text = text;
        }

        public Value(Value other)
        {
            if (other == null)
            {
                return;
            }

            this.text = other.text;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.text);

            return hash.ToHashCode();
        }

        public bool Equals(Value other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.text.Equals(other.text);
        }

        public override bool Equals(object obj) => this.Equals(obj as Value);

        public override string ToString() => ValueSupport.Instance.ToString(this);
    }

} // namespace CorpusFormatting
