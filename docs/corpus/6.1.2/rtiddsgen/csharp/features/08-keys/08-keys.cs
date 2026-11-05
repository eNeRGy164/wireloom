
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-keys.idl
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

namespace CorpusKeys
{

    public class Simple :  IEquatable<Simple>
    {
        [Key]
        public int id { get; set; }

        [Bound(16)]
        public string name { get; set; } = string.Empty;

        public Simple()
        {
        }

        public Simple(int  id, string  name)
        {
            this.id = id;
            this.name = name;
        }

        public Simple(Simple other)
        {
            if (other == null)
            {
                return;
            }

            this.id = other.id;
            this.name = other.name;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(id);
            hash.Add(name);

            return hash.ToHashCode();
        }

        public bool Equals(Simple other)
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
            this.name.Equals(other.name);
        }

        public override bool Equals(object obj) => this.Equals(obj as Simple);

        public override string ToString() => SimpleSupport.Instance.ToString(this);
    }

    public class Identity :  IEquatable<Identity>
    {
        [Key]
        public int tenant { get; set; }

        [Key]
        [Bound(16)]
        public string name { get; set; } = string.Empty;

        public Identity()
        {
        }

        public Identity(int  tenant, string  name)
        {
            this.tenant = tenant;
            this.name = name;
        }

        public Identity(Identity other)
        {
            if (other == null)
            {
                return;
            }

            this.tenant = other.tenant;
            this.name = other.name;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(tenant);
            hash.Add(name);

            return hash.ToHashCode();
        }

        public bool Equals(Identity other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.tenant.Equals(other.tenant) &&
            this.name.Equals(other.name);
        }

        public override bool Equals(object obj) => this.Equals(obj as Identity);

        public override string ToString() => IdentitySupport.Instance.ToString(this);
    }

    public class Sample :  IEquatable<Sample>
    {
        [Key]
        public CorpusKeys.Identity identity { get; set; }

        public int payload { get; set; }

        public Sample()
        {
            identity = new CorpusKeys.Identity();
        }

        public Sample(CorpusKeys.Identity  identity, int  payload)
        {
            this.identity = identity;
            this.payload = payload;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.identity = new CorpusKeys.Identity(other.identity);
            this.payload = other.payload;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(identity);
            hash.Add(payload);

            return hash.ToHashCode();
        }

        public bool Equals(Sample other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.identity.Equals(other.identity) &&
            this.payload.Equals(other.payload);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusKeys
