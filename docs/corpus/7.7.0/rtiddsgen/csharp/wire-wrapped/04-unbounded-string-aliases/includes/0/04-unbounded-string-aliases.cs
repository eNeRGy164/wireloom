
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-unbounded-string-aliases.idl
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

namespace CorpusUnboundedStrings
{

    public class Text :  IEquatable<Text>
    {

        [Bound(-1)]
        public string Value { get; set; } = string.Empty;

        public Text()
        {
        }

        public Text(string  Value)
        {
            this.Value = Value;
        }

        public Text(Text other)
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

        public bool Equals(Text other)
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

        public override bool Equals(object obj) => this.Equals(obj as Text);

        public override string ToString() => TextSupport.Instance.ToString(this);
    }

    public class WideText :  IEquatable<WideText>
    {

        [Bound(-1)]
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

} // namespace CorpusUnboundedStrings
