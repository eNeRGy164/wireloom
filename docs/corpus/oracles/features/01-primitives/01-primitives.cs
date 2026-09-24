
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 01-primitives.idl
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

namespace CorpusPrimitives
{

    public class Sample :  IEquatable<Sample>
    {
        public short signed16 { get; set; }
        public int signed32 { get; set; }
        public long signed64 { get; set; }
        public ushort unsigned16 { get; set; }
        public uint unsigned32 { get; set; }
        public ulong unsigned64 { get; set; }
        public sbyte fixedInt8 { get; set; }
        public long fixedInt64 { get; set; }
        public byte fixedUint8 { get; set; }
        public ulong fixedUint64 { get; set; }
        public byte octetValue { get; set; }
        public bool boolValue { get; set; }
        public char charValue { get; set; }
        public char wideCharValue { get; set; }
        public float floatValue { get; set; }
        public double doubleValue { get; set; }
        public LongDouble longDoubleValue { get; set; }

        public Sample()
        {
        }

        public Sample(short  signed16, int  signed32, long  signed64, ushort  unsigned16, uint  unsigned32, ulong  unsigned64, sbyte  fixedInt8, long  fixedInt64, byte  fixedUint8, ulong  fixedUint64, byte  octetValue, bool  boolValue, char  charValue, char  wideCharValue, float  floatValue, double  doubleValue, LongDouble  longDoubleValue)
        {
            this.signed16 = signed16;
            this.signed32 = signed32;
            this.signed64 = signed64;
            this.unsigned16 = unsigned16;
            this.unsigned32 = unsigned32;
            this.unsigned64 = unsigned64;
            this.fixedInt8 = fixedInt8;
            this.fixedInt64 = fixedInt64;
            this.fixedUint8 = fixedUint8;
            this.fixedUint64 = fixedUint64;
            this.octetValue = octetValue;
            this.boolValue = boolValue;
            this.charValue = charValue;
            this.wideCharValue = wideCharValue;
            this.floatValue = floatValue;
            this.doubleValue = doubleValue;
            this.longDoubleValue = longDoubleValue;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.signed16 = other.signed16;
            this.signed32 = other.signed32;
            this.signed64 = other.signed64;
            this.unsigned16 = other.unsigned16;
            this.unsigned32 = other.unsigned32;
            this.unsigned64 = other.unsigned64;
            this.fixedInt8 = other.fixedInt8;
            this.fixedInt64 = other.fixedInt64;
            this.fixedUint8 = other.fixedUint8;
            this.fixedUint64 = other.fixedUint64;
            this.octetValue = other.octetValue;
            this.boolValue = other.boolValue;
            this.charValue = other.charValue;
            this.wideCharValue = other.wideCharValue;
            this.floatValue = other.floatValue;
            this.doubleValue = other.doubleValue;
            this.longDoubleValue = other.longDoubleValue;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.signed16);
            hash.Add(this.signed32);
            hash.Add(this.signed64);
            hash.Add(this.unsigned16);
            hash.Add(this.unsigned32);
            hash.Add(this.unsigned64);
            hash.Add(this.fixedInt8);
            hash.Add(this.fixedInt64);
            hash.Add(this.fixedUint8);
            hash.Add(this.fixedUint64);
            hash.Add(this.octetValue);
            hash.Add(this.boolValue);
            hash.Add(this.charValue);
            hash.Add(this.wideCharValue);
            hash.Add(this.floatValue);
            hash.Add(this.doubleValue);
            hash.Add(this.longDoubleValue);

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

            return this.signed16.Equals(other.signed16) && 
            this.signed32.Equals(other.signed32) && 
            this.signed64.Equals(other.signed64) && 
            this.unsigned16.Equals(other.unsigned16) && 
            this.unsigned32.Equals(other.unsigned32) && 
            this.unsigned64.Equals(other.unsigned64) && 
            this.fixedInt8.Equals(other.fixedInt8) && 
            this.fixedInt64.Equals(other.fixedInt64) && 
            this.fixedUint8.Equals(other.fixedUint8) && 
            this.fixedUint64.Equals(other.fixedUint64) && 
            this.octetValue.Equals(other.octetValue) && 
            this.boolValue.Equals(other.boolValue) && 
            this.charValue.Equals(other.charValue) && 
            this.wideCharValue.Equals(other.wideCharValue) && 
            this.floatValue.Equals(other.floatValue) && 
            this.doubleValue.Equals(other.doubleValue) && 
            this.longDoubleValue.Equals(other.longDoubleValue);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusPrimitives
