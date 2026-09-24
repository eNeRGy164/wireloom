
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-integration.idl
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

public class CorpusIntegrationMessage :  IEquatable<CorpusIntegrationMessage>
{
    [Bound(255)]
    public string text { get; set; } = string.Empty;

    public CorpusIntegrationMessage()
    {
    }

    public CorpusIntegrationMessage(string  text)
    {
        this.text = text;
    }

    public CorpusIntegrationMessage(CorpusIntegrationMessage other)
    {
        if (other == null)
        {
            return;
        }

        this.text = other.text;

    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(this.text);

        return hash.ToHashCode();
    }

    public bool Equals(CorpusIntegrationMessage other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.text.Equals(other.text);
    }

    public override bool Equals(object obj) => this.Equals(obj as CorpusIntegrationMessage);

    public override string ToString() => CorpusIntegrationMessageSupport.Instance.ToString(this);
}

