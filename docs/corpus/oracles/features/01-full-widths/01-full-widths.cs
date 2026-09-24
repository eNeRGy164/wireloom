
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 01-full-widths.idl
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

namespace CorpusFullWidths
{

    public class Sample :  IEquatable<Sample>
    {
        public sbyte i8 { get; set; }
        public short i16 { get; set; }
        public int i32 { get; set; }
        public long i64 { get; set; }
        public byte u8 { get; set; }
        public ushort u16 { get; set; }
        public uint u32 { get; set; }
        public ulong u64 { get; set; }

        public Sample()
        {
        }

        public Sample(sbyte  i8, short  i16, int  i32, long  i64, byte  u8, ushort  u16, uint  u32, ulong  u64)
        {
            this.i8 = i8;
            this.i16 = i16;
            this.i32 = i32;
            this.i64 = i64;
            this.u8 = u8;
            this.u16 = u16;
            this.u32 = u32;
            this.u64 = u64;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.i8 = other.i8;
            this.i16 = other.i16;
            this.i32 = other.i32;
            this.i64 = other.i64;
            this.u8 = other.u8;
            this.u16 = other.u16;
            this.u32 = other.u32;
            this.u64 = other.u64;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.i8);
            hash.Add(this.i16);
            hash.Add(this.i32);
            hash.Add(this.i64);
            hash.Add(this.u8);
            hash.Add(this.u16);
            hash.Add(this.u32);
            hash.Add(this.u64);

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

            return this.i8.Equals(other.i8) && 
            this.i16.Equals(other.i16) && 
            this.i32.Equals(other.i32) && 
            this.i64.Equals(other.i64) && 
            this.u8.Equals(other.u8) && 
            this.u16.Equals(other.u16) && 
            this.u32.Equals(other.u32) && 
            this.u64.Equals(other.u64);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusFullWidths
