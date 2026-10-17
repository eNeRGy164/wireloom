
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-extensibility.idl
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

namespace CorpusExtensibility
{

    public class Appendable :  IEquatable<Appendable>
    {
        public int id { get; set; }
        [Bound(16)]
        public string text { get; set; } = string.Empty;

        public Appendable()
        {
        }

        public Appendable(int  id, string  text)
        {
            this.id = id;
            this.text = text;
        }

        public Appendable(Appendable other)
        {
            if (other == null)
            {
                return;
            }

            this.id = other.id;
            this.text = other.text;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.id);
            hash.Add(this.text);

            return hash.ToHashCode();
        }

        public bool Equals(Appendable other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.id.Equals(other.id) && 
            this.text.Equals(other.text);
        }

        public override bool Equals(object obj) => this.Equals(obj as Appendable);

        public override string ToString() => AppendableSupport.Instance.ToString(this);
    }

    public class Final :  IEquatable<Final>
    {
        public int id { get; set; }
        [Bound(16)]
        public string text { get; set; } = string.Empty;

        public Final()
        {
        }

        public Final(int  id, string  text)
        {
            this.id = id;
            this.text = text;
        }

        public Final(Final other)
        {
            if (other == null)
            {
                return;
            }

            this.id = other.id;
            this.text = other.text;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.id);
            hash.Add(this.text);

            return hash.ToHashCode();
        }

        public bool Equals(Final other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.id.Equals(other.id) && 
            this.text.Equals(other.text);
        }

        public override bool Equals(object obj) => this.Equals(obj as Final);

        public override string ToString() => FinalSupport.Instance.ToString(this);
    }

    public class Mutable :  IEquatable<Mutable>
    {
        public int id { get; set; }
        [Optional]
        public int? optionalValue { get; set; }

        public Mutable()
        {
        }

        public Mutable(int  id, int ?  optionalValue)
        {
            this.id = id;
            this.optionalValue = optionalValue;
        }

        public Mutable(Mutable other)
        {
            if (other == null)
            {
                return;
            }

            this.id = other.id;
            this.optionalValue = other.optionalValue;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.id);
            hash.Add(this.optionalValue);

            return hash.ToHashCode();
        }

        public bool Equals(Mutable other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.id.Equals(other.id) && 
            Equals(this.optionalValue, other.optionalValue);
        }

        public override bool Equals(object obj) => this.Equals(obj as Mutable);

        public override string ToString() => MutableSupport.Instance.ToString(this);
    }

} // namespace CorpusExtensibility
