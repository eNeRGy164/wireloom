
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-evolution-optional.idl
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

namespace CorpusOptionalEvolution
{

    public class Message :  IEquatable<Message>
    {
        public int requiredValue { get; set; }
        [Optional]
        public int? optionalValue { get; set; }
        [Optional]
        [Bound(16)]
        public string optionalText { get; set; } = null;

        public Message()
        {
        }

        public Message(int  requiredValue, int ?  optionalValue, string  optionalText)
        {
            this.requiredValue = requiredValue;
            this.optionalValue = optionalValue;
            this.optionalText = optionalText;
        }

        public Message(Message other)
        {
            if (other == null)
            {
                return;
            }

            this.requiredValue = other.requiredValue;
            this.optionalValue = other.optionalValue;
            this.optionalText = other.optionalText;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.requiredValue);
            hash.Add(this.optionalValue);
            hash.Add(this.optionalText);

            return hash.ToHashCode();
        }

        public bool Equals(Message other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.requiredValue.Equals(other.requiredValue) && 
            Equals(this.optionalValue, other.optionalValue) && 
            Equals(this.optionalText, other.optionalText);
        }

        public override bool Equals(object obj) => this.Equals(obj as Message);

        public override string ToString() => MessageSupport.Instance.ToString(this);
    }

} // namespace CorpusOptionalEvolution
