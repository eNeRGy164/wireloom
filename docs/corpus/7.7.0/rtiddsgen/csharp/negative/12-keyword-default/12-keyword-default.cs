
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-keyword-default.idl
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

namespace CorpusStrictKeyword
{

    public class Struct :  IEquatable<Struct>
    {
        public int number { get; set; }

        public Struct()
        {
        }

        public Struct(int  number)
        {
            this.number = number;
        }

        public Struct(Struct other)
        {
            if (other == null)
            {
                return;
            }

            this.number = other.number;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.number);

            return hash.ToHashCode();
        }

        public bool Equals(Struct other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.number.Equals(other.number);
        }

        public override bool Equals(object obj) => this.Equals(obj as Struct);

        public override string ToString() => StructSupport.Instance.ToString(this);
    }

} // namespace CorpusStrictKeyword
