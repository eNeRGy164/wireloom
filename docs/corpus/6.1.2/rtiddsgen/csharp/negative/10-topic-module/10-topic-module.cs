
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-topic-module.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
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

namespace CorpusTopicModule
{

    public class Identifier :  IEquatable<Identifier>
    {

        public int Value { get; set; }

        public Identifier()
        {
        }

        public Identifier(int  Value)
        {
            this.Value = Value;
        }

        public Identifier(Identifier other)
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

            hash.Add(Value);

            return hash.ToHashCode();
        }

        public bool Equals(Identifier other)
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

        public override bool Equals(object obj) => this.Equals(obj as Identifier);

        public override string ToString() => IdentifierSupport.Instance.ToString(this);
    }

    public static class Greeting
    {
        public const string Value = "hi";
    }

    public class Message :  IEquatable<Message>
    {
        [Key]
        public int id { get; set; }

        public Message()
        {
        }

        public Message(int  id)
        {
            this.id = id;
        }

        public Message(Message other)
        {
            if (other == null)
            {
                return;
            }

            this.id = other.id;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(id);

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

            return this.id.Equals(other.id);
        }

        public override bool Equals(object obj) => this.Equals(obj as Message);

        public override string ToString() => MessageSupport.Instance.ToString(this);
    }

} // namespace CorpusTopicModule
