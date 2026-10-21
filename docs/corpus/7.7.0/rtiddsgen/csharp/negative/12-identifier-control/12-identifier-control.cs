
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-identifier-control.idl
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

namespace CorpusIdentifierControl
{

    public class _Struct :  IEquatable<_Struct>
    {
        public int @event { get; set; }

        public _Struct()
        {
        }

        public _Struct(int  @event)
        {
            this.@event = @event;
        }

        public _Struct(_Struct other)
        {
            if (other == null)
            {
                return;
            }

            this.@event = other.@event;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.@event);

            return hash.ToHashCode();
        }

        public bool Equals(_Struct other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.@event.Equals(other.@event);
        }

        public override bool Equals(object obj) => this.Equals(obj as _Struct);

        public override string ToString() => _StructSupport.Instance.ToString(this);
    }

} // namespace CorpusIdentifierControl
