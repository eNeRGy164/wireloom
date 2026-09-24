
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-id-gaps.idl
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

namespace CorpusIdGaps
{

    public class Message :  IEquatable<Message>
    {
        public int first { get; set; }
        public int seventh { get; set; }

        public Message()
        {
        }

        public Message(int  first, int  seventh)
        {
            this.first = first;
            this.seventh = seventh;
        }

        public Message(Message other)
        {
            if (other == null)
            {
                return;
            }

            this.first = other.first;
            this.seventh = other.seventh;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.first);
            hash.Add(this.seventh);

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

            return this.first.Equals(other.first) && 
            this.seventh.Equals(other.seventh);
        }

        public override bool Equals(object obj) => this.Equals(obj as Message);

        public override string ToString() => MessageSupport.Instance.ToString(this);
    }

} // namespace CorpusIdGaps
