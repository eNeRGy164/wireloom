
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-modules.idl
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

namespace CorpusIntegration
{

    namespace Contracts
    {

        public class NamespacedMessage :  IEquatable<NamespacedMessage>
        {
            public bool enabled { get; set; }

            public NamespacedMessage()
            {
            }

            public NamespacedMessage(bool  enabled)
            {
                this.enabled = enabled;
            }

            public NamespacedMessage(NamespacedMessage other)
            {
                if (other == null)
                {
                    return;
                }

                this.enabled = other.enabled;

            }

            public override int GetHashCode()
            {
                var hash = new HashCode();

                hash.Add(this.enabled);

                return hash.ToHashCode();
            }

            public bool Equals(NamespacedMessage other)
            {
                if (other == null)
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return this.enabled.Equals(other.enabled);
            }

            public override bool Equals(object obj) => this.Equals(obj as NamespacedMessage);

            public override string ToString() => NamespacedMessageSupport.Instance.ToString(this);
        }

    } // namespace Contracts

} // namespace CorpusIntegration
