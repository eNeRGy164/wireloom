
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-boolean.idl
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

namespace CorpusBooleanUnion
{

    public class Choice :  IEquatable<Choice>
    {

        private int _enabled;

        private string _disabled = string.Empty;

        public bool Discriminator { get; private set; }

        public const bool DefaultDiscriminator = false;

        public int enabled
        {
            get
            {
                if (Discriminator != true) 
                {
                    throw new InvalidOperationException("enabled not selected");
                }
                return _enabled;
            }

            set
            {
                _enabled = value;
                Discriminator = true;
            }
        }

        [Bound(255)]
        public string disabled
        {
            get
            {
                if (Discriminator != false) 
                {
                    throw new InvalidOperationException("disabled not selected");
                }
                return _disabled;
            }

            set
            {
                _disabled = value;
                Discriminator = false;
            }
        }

        public Choice()
        {
            Discriminator = DefaultDiscriminator;
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
                case true:
                this._enabled = other.enabled;
                break;
                case false:
                this._disabled = other.disabled;
                break;
            }
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case true:
                return enabled;
                case false:
                return disabled;

            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case true:
                return HashCode.Combine(Discriminator, this.enabled);
                case false:
                return HashCode.Combine(Discriminator, this.disabled);
            }
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
                case true:
                return this.enabled.Equals(other.enabled);
                case false:
                return this.disabled.Equals(other.disabled);
            }
        }

        public override bool Equals(object obj) => this.Equals(obj as Choice);

        public override string ToString() => ChoiceSupport.Instance.ToString(this);
    }

} // namespace CorpusBooleanUnion
