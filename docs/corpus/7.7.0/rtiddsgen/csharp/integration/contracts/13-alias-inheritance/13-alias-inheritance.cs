
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-alias-inheritance.idl
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

namespace CorpusIntegrationTrace
{

    public class Producer :  IEquatable<Producer>
    {

        [Bound(16)]
        public string Value { get; set; } = string.Empty;

        public Producer()
        {
        }

        public Producer(string  Value)
        {
            this.Value = Value;
        }

        public Producer(Producer other)
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

        public bool Equals(Producer other)
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

        public override bool Equals(object obj) => this.Equals(obj as Producer);

        public override string ToString() => ProducerSupport.Instance.ToString(this);
    }

    public class Context :  IEquatable<Context>
    {
        [Bound(32)]
        public string correlation { get; set; } = string.Empty;
        [Optional]
        [Bound(16)]
        public string producer { get; set; } = null;

        public Context()
        {
        }

        public Context(string  correlation, string  producer)
        {
            this.correlation = correlation;
            this.producer = producer;
        }

        public Context(Context other)
        {
            if (other == null)
            {
                return;
            }

            this.correlation = other.correlation;
            this.producer = other.producer;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.correlation);
            hash.Add(this.producer);

            return hash.ToHashCode();
        }

        public bool Equals(Context other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.correlation.Equals(other.correlation) && 
            Equals(this.producer, other.producer);
        }

        public override bool Equals(object obj) => this.Equals(obj as Context);

        public override string ToString() => ContextSupport.Instance.ToString(this);
    }

    public class Base :  IEquatable<Base>
    {
        public int baseValue { get; set; }

        public Base()
        {
        }

        public Base(int  baseValue)
        {
            this.baseValue = baseValue;
        }

        public Base(Base other)
        {
            if (other == null)
            {
                return;
            }

            this.baseValue = other.baseValue;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.baseValue);

            return hash.ToHashCode();
        }

        public bool Equals(Base other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.baseValue.Equals(other.baseValue);
        }

        public override bool Equals(object obj) => this.Equals(obj as Base);

        public override string ToString() => BaseSupport.Instance.ToString(this);
    }

    public class Derived :  global::CorpusIntegrationTrace.Base, IEquatable<Derived>
    {
        [Bound(16)]
        public string state { get; set; } = string.Empty;
        public global::CorpusIntegrationTrace.Context traceContext { get; set; }

        public Derived()
        {
            traceContext = new global::CorpusIntegrationTrace.Context();
        }

        public Derived(int  baseValue, string  state, global::CorpusIntegrationTrace.Context  traceContext) : base(baseValue)
        {
            this.state = state;
            this.traceContext = traceContext;
        }

        public Derived(Derived other) : base(other)
        {
            if (other == null)
            {
                return;
            }

            this.state = other.state;
            this.traceContext = new global::CorpusIntegrationTrace.Context(other.traceContext);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(base.GetHashCode());

            hash.Add(this.state);
            hash.Add(this.traceContext);

            return hash.ToHashCode();
        }

        public bool Equals(Derived other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if(!base.Equals(other))
            {
                return false;
            }

            return this.state.Equals(other.state) && 
            this.traceContext.Equals(other.traceContext);
        }

        public override bool Equals(object obj) => this.Equals(obj as Derived);

        public override string ToString() => DerivedSupport.Instance.ToString(this);
    }

} // namespace CorpusIntegrationTrace
