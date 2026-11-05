
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-optional-aggregate-member.idl
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

namespace CorpusOptionalAggregate
{

    public class Payload :  IEquatable<Payload>
    {
        public int value { get; set; }

        public Payload()
        {
        }

        public Payload(int  value)
        {
            this.value = value;
        }

        public Payload(Payload other)
        {
            if (other == null)
            {
                return;
            }

            this.value = other.value;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(value);

            return hash.ToHashCode();
        }

        public bool Equals(Payload other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.value.Equals(other.value);
        }

        public override bool Equals(object obj) => this.Equals(obj as Payload);

        public override string ToString() => PayloadSupport.Instance.ToString(this);
    }

    public class PayloadAlias :  IEquatable<PayloadAlias>
    {

        public CorpusOptionalAggregate.Payload Value { get; set; }

        public PayloadAlias()
        {
            Value = new CorpusOptionalAggregate.Payload();
        }

        public PayloadAlias(CorpusOptionalAggregate.Payload  Value)
        {
            this.Value = Value;
        }

        public PayloadAlias(PayloadAlias other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new CorpusOptionalAggregate.Payload(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(Value);

            return hash.ToHashCode();
        }

        public bool Equals(PayloadAlias other)
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

        public override bool Equals(object obj) => this.Equals(obj as PayloadAlias);

        public override string ToString() => PayloadAliasSupport.Instance.ToString(this);
    }

    public class Choice :  IEquatable<Choice>
    {

        private CorpusOptionalAggregate.Payload _payload;

        private int _number;

        public int Discriminator { get; private set; }

        public const int DefaultDiscriminator = 0;

        public CorpusOptionalAggregate.Payload payload
        {
            get
            {
                if (Discriminator != 0) 
                {
                    throw new InvalidOperationException("payload not selected");
                }
                return _payload;
            }

            set
            {
                _payload = value;
                Discriminator = 0;
            }
        }

        public int number
        {
            get
            {
                if (Discriminator != 1) 
                {
                    throw new InvalidOperationException("number not selected");
                }
                return _number;
            }

            set
            {
                _number = value;
                Discriminator = 1;
            }
        }

        public Choice()
        {
            Discriminator = DefaultDiscriminator;
            _payload = new CorpusOptionalAggregate.Payload();
        }

        public Choice(Choice other)
        {
            if (other == null)
            {
                return;
            }

            this.Discriminator = other.Discriminator;
            switch (Discriminator)
            {
                case 0:
                this._payload = new CorpusOptionalAggregate.Payload(other.payload);
                break;
                case 1:
                this._number = other.number;
                break;
            }
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case 0:
                return payload;
                case 1:
                return number;

                default:
                return null;
            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case 0:
                return HashCode.Combine(Discriminator, payload);
                case 1:
                return HashCode.Combine(Discriminator, number);
            }

            throw new InvalidOperationException("Invalid discriminator");
        }

        public bool Equals(Choice other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.Discriminator != other.Discriminator)
            {
                return false;
            }

            switch (Discriminator)
            {
                case 0:
                return this.payload.Equals(other.payload);
                case 1:
                return this.number.Equals(other.number);
            }
            throw new InvalidOperationException("Invalid discriminator");
        }

        public override bool Equals(object obj) => this.Equals(obj as Choice);

        public override string ToString() => ChoiceSupport.Instance.ToString(this);
    }

    public class ChoiceAlias :  IEquatable<ChoiceAlias>
    {

        public CorpusOptionalAggregate.Choice Value { get; set; }

        public ChoiceAlias()
        {
            Value = new CorpusOptionalAggregate.Choice();
        }

        public ChoiceAlias(CorpusOptionalAggregate.Choice  Value)
        {
            this.Value = Value;
        }

        public ChoiceAlias(ChoiceAlias other)
        {
            if (other == null)
            {
                return;
            }

            this.Value = new CorpusOptionalAggregate.Choice(other.Value);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(Value);

            return hash.ToHashCode();
        }

        public bool Equals(ChoiceAlias other)
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

        public override bool Equals(object obj) => this.Equals(obj as ChoiceAlias);

        public override string ToString() => ChoiceAliasSupport.Instance.ToString(this);
    }

    public class Holder :  IEquatable<Holder>
    {
        [Optional]
        public CorpusOptionalAggregate.Payload payload { get; set; }

        [Optional]
        public CorpusOptionalAggregate.Payload payloadAlias { get; set; }

        [Optional]
        public CorpusOptionalAggregate.Choice choice { get; set; }

        [Optional]
        public CorpusOptionalAggregate.Choice choiceAlias { get; set; }

        public Holder()
        {
        }

        public Holder(CorpusOptionalAggregate.Payload  payload, CorpusOptionalAggregate.Payload  payloadAlias, CorpusOptionalAggregate.Choice  choice, CorpusOptionalAggregate.Choice  choiceAlias)
        {
            this.payload = payload;
            this.payloadAlias = payloadAlias;
            this.choice = choice;
            this.choiceAlias = choiceAlias;
        }

        public Holder(Holder other)
        {
            if (other == null)
            {
                return;
            }

            if(other.payload != null)
            {
                this.payload = new CorpusOptionalAggregate.Payload(other.payload);
            }
            if(other.payloadAlias != null)
            {
                this.payloadAlias = new CorpusOptionalAggregate.Payload(other.payloadAlias);
            }
            if(other.choice != null)
            {
                this.choice = new CorpusOptionalAggregate.Choice(other.choice);
            }
            if(other.choiceAlias != null)
            {
                this.choiceAlias = new CorpusOptionalAggregate.Choice(other.choiceAlias);
            }

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(payload);
            hash.Add(payloadAlias);
            hash.Add(choice);
            hash.Add(choiceAlias);

            return hash.ToHashCode();
        }

        public bool Equals(Holder other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.payload, other.payload) &&
            Equals(this.payloadAlias, other.payloadAlias) &&
            Equals(this.choice, other.choice) &&
            Equals(this.choiceAlias, other.choiceAlias);
        }

        public override bool Equals(object obj) => this.Equals(obj as Holder);

        public override string ToString() => HolderSupport.Instance.ToString(this);
    }

} // namespace CorpusOptionalAggregate
