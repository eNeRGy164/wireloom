
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-interface-case-default.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
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

namespace CorpusInterfaceCase
{

    public class Service_ping_In :  IEquatable<Service_ping_In>
    {
        public byte dummy { get; set; }

        public Service_ping_In()
        {
        }

        public Service_ping_In(byte  dummy)
        {
            this.dummy = dummy;
        }

        public Service_ping_In(Service_ping_In other)
        {
            if (other == null)
            {
                return;
            }

            this.dummy = other.dummy;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.dummy);

            return hash.ToHashCode();
        }

        public bool Equals(Service_ping_In other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.dummy.Equals(other.dummy);
        }

        public override bool Equals(object obj) => this.Equals(obj as Service_ping_In);

        public override string ToString() => Service_ping_InSupport.Instance.ToString(this);
    }

    public class Service_ping_Out :  IEquatable<Service_ping_Out>
    {
        public byte dummy { get; set; }

        public Service_ping_Out()
        {
        }

        public Service_ping_Out(byte  dummy)
        {
            this.dummy = dummy;
        }

        public Service_ping_Out(Service_ping_Out other)
        {
            if (other == null)
            {
                return;
            }

            this.dummy = other.dummy;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.dummy);

            return hash.ToHashCode();
        }

        public bool Equals(Service_ping_Out other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.dummy.Equals(other.dummy);
        }

        public override bool Equals(object obj) => this.Equals(obj as Service_ping_Out);

        public override string ToString() => Service_ping_OutSupport.Instance.ToString(this);
    }

    public class Service_ping_Result :  IEquatable<Service_ping_Result>
    {

        private global::CorpusInterfaceCase.Service_ping_Out _result;

        public int Discriminator { get; private set; }

        public const int DefaultDiscriminator = 0;

        public global::CorpusInterfaceCase.Service_ping_Out result
        {
            get
            {
                if (Discriminator != 0) 
                {
                    throw new InvalidOperationException("result not selected");
                }
                return _result;
            }

            set
            {
                _result = value;
                Discriminator = 0;
            }
        }

        public Service_ping_Result()
        {
            Discriminator = DefaultDiscriminator;

            _result = new global::CorpusInterfaceCase.Service_ping_Out();
        }

        public Service_ping_Result(Service_ping_Result other)
        {
            if (other == null)
            {
                return;
            }

            this.Discriminator = other.Discriminator;
            switch (Discriminator)
            {
                case 0:
                this._result = new global::CorpusInterfaceCase.Service_ping_Out(other.result);
                break;
            }
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case 0:
                return result;

                default:
                return null;
            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case 0:
                return HashCode.Combine(Discriminator, this.result);
            }
            return HashCode.Combine(Discriminator);
        }

        public bool Equals(Service_ping_Result other)
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
                return this.result.Equals(other.result);
            }
            return true;
        }

        public override bool Equals(object obj) => this.Equals(obj as Service_ping_Result);

        public override string ToString() => Service_ping_ResultSupport.Instance.ToString(this);
    }

    public static class Service_ping_Hash
    {
        public const int Value = 18846175;
    }

    public class Service_Ping_In :  IEquatable<Service_Ping_In>
    {
        public byte dummy { get; set; }

        public Service_Ping_In()
        {
        }

        public Service_Ping_In(byte  dummy)
        {
            this.dummy = dummy;
        }

        public Service_Ping_In(Service_Ping_In other)
        {
            if (other == null)
            {
                return;
            }

            this.dummy = other.dummy;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.dummy);

            return hash.ToHashCode();
        }

        public bool Equals(Service_Ping_In other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.dummy.Equals(other.dummy);
        }

        public override bool Equals(object obj) => this.Equals(obj as Service_Ping_In);

        public override string ToString() => Service_Ping_InSupport.Instance.ToString(this);
    }

    public class Service_Ping_Out :  IEquatable<Service_Ping_Out>
    {
        public byte dummy { get; set; }

        public Service_Ping_Out()
        {
        }

        public Service_Ping_Out(byte  dummy)
        {
            this.dummy = dummy;
        }

        public Service_Ping_Out(Service_Ping_Out other)
        {
            if (other == null)
            {
                return;
            }

            this.dummy = other.dummy;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.dummy);

            return hash.ToHashCode();
        }

        public bool Equals(Service_Ping_Out other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.dummy.Equals(other.dummy);
        }

        public override bool Equals(object obj) => this.Equals(obj as Service_Ping_Out);

        public override string ToString() => Service_Ping_OutSupport.Instance.ToString(this);
    }

    public class Service_Ping_Result :  IEquatable<Service_Ping_Result>
    {

        private global::CorpusInterfaceCase.Service_Ping_Out _result;

        public int Discriminator { get; private set; }

        public const int DefaultDiscriminator = 0;

        public global::CorpusInterfaceCase.Service_Ping_Out result
        {
            get
            {
                if (Discriminator != 0) 
                {
                    throw new InvalidOperationException("result not selected");
                }
                return _result;
            }

            set
            {
                _result = value;
                Discriminator = 0;
            }
        }

        public Service_Ping_Result()
        {
            Discriminator = DefaultDiscriminator;

            _result = new global::CorpusInterfaceCase.Service_Ping_Out();
        }

        public Service_Ping_Result(Service_Ping_Result other)
        {
            if (other == null)
            {
                return;
            }

            this.Discriminator = other.Discriminator;
            switch (Discriminator)
            {
                case 0:
                this._result = new global::CorpusInterfaceCase.Service_Ping_Out(other.result);
                break;
            }
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case 0:
                return result;

                default:
                return null;
            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case 0:
                return HashCode.Combine(Discriminator, this.result);
            }
            return HashCode.Combine(Discriminator);
        }

        public bool Equals(Service_Ping_Result other)
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
                return this.result.Equals(other.result);
            }
            return true;
        }

        public override bool Equals(object obj) => this.Equals(obj as Service_Ping_Result);

        public override string ToString() => Service_Ping_ResultSupport.Instance.ToString(this);
    }

    public static class Service_Ping_Hash
    {
        public const int Value = -803907400;
    }

    public class Service_Call :  IEquatable<Service_Call>
    {

        private global::CorpusInterfaceCase.Service_ping_In _ping;

        private global::CorpusInterfaceCase.Service_Ping_In _Ping;

        public int Discriminator { get; private set; }

        public const int DefaultDiscriminator = (CorpusInterfaceCase.Service_ping_Hash.Value);

        public global::CorpusInterfaceCase.Service_ping_In ping
        {
            get
            {
                if (Discriminator != (CorpusInterfaceCase.Service_ping_Hash.Value)) 
                {
                    throw new InvalidOperationException("ping not selected");
                }
                return _ping;
            }

            set
            {
                _ping = value;
                Discriminator = (CorpusInterfaceCase.Service_ping_Hash.Value);
            }
        }

        public global::CorpusInterfaceCase.Service_Ping_In Ping
        {
            get
            {
                if (Discriminator != (CorpusInterfaceCase.Service_Ping_Hash.Value)) 
                {
                    throw new InvalidOperationException("Ping not selected");
                }
                return _Ping;
            }

            set
            {
                _Ping = value;
                Discriminator = (CorpusInterfaceCase.Service_Ping_Hash.Value);
            }
        }

        public Service_Call()
        {
            Discriminator = DefaultDiscriminator;

            _ping = new global::CorpusInterfaceCase.Service_ping_In();
        }

        public Service_Call(Service_Call other)
        {
            if (other == null)
            {
                return;
            }

            this.Discriminator = other.Discriminator;
            switch (Discriminator)
            {
                case (CorpusInterfaceCase.Service_ping_Hash.Value):
                    this._ping = new global::CorpusInterfaceCase.Service_ping_In(other.ping);
                    break;
                case (CorpusInterfaceCase.Service_Ping_Hash.Value):
                    this._Ping = new global::CorpusInterfaceCase.Service_Ping_In(other.Ping);
                    break;
            }
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case (CorpusInterfaceCase.Service_ping_Hash.Value):
                    return ping;
                case (CorpusInterfaceCase.Service_Ping_Hash.Value):
                    return Ping;

                    default:
                    return null;
            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case (CorpusInterfaceCase.Service_ping_Hash.Value):
                    return HashCode.Combine(Discriminator, this.ping);
                case (CorpusInterfaceCase.Service_Ping_Hash.Value):
                    return HashCode.Combine(Discriminator, this.Ping);
            }
            return HashCode.Combine(Discriminator);
        }

        public bool Equals(Service_Call other)
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
                case (CorpusInterfaceCase.Service_ping_Hash.Value):
                    return this.ping.Equals(other.ping);
                case (CorpusInterfaceCase.Service_Ping_Hash.Value):
                    return this.Ping.Equals(other.Ping);
            }
            return true;
        }

        public override bool Equals(object obj) => this.Equals(obj as Service_Call);

        public override string ToString() => Service_CallSupport.Instance.ToString(this);
    }

    public class Service_Return :  IEquatable<Service_Return>
    {

        private global::CorpusInterfaceCase.Service_ping_Result _ping;

        private global::CorpusInterfaceCase.Service_Ping_Result _Ping;

        public int Discriminator { get; private set; }

        public const int DefaultDiscriminator = (CorpusInterfaceCase.Service_ping_Hash.Value);

        public global::CorpusInterfaceCase.Service_ping_Result ping
        {
            get
            {
                if (Discriminator != (CorpusInterfaceCase.Service_ping_Hash.Value)) 
                {
                    throw new InvalidOperationException("ping not selected");
                }
                return _ping;
            }

            set
            {
                _ping = value;
                Discriminator = (CorpusInterfaceCase.Service_ping_Hash.Value);
            }
        }

        public global::CorpusInterfaceCase.Service_Ping_Result Ping
        {
            get
            {
                if (Discriminator != (CorpusInterfaceCase.Service_Ping_Hash.Value)) 
                {
                    throw new InvalidOperationException("Ping not selected");
                }
                return _Ping;
            }

            set
            {
                _Ping = value;
                Discriminator = (CorpusInterfaceCase.Service_Ping_Hash.Value);
            }
        }

        public Service_Return()
        {
            Discriminator = DefaultDiscriminator;

            _ping = new global::CorpusInterfaceCase.Service_ping_Result();
        }

        public Service_Return(Service_Return other)
        {
            if (other == null)
            {
                return;
            }

            this.Discriminator = other.Discriminator;
            switch (Discriminator)
            {
                case (CorpusInterfaceCase.Service_ping_Hash.Value):
                    this._ping = new global::CorpusInterfaceCase.Service_ping_Result(other.ping);
                    break;
                case (CorpusInterfaceCase.Service_Ping_Hash.Value):
                    this._Ping = new global::CorpusInterfaceCase.Service_Ping_Result(other.Ping);
                    break;
            }
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case (CorpusInterfaceCase.Service_ping_Hash.Value):
                    return ping;
                case (CorpusInterfaceCase.Service_Ping_Hash.Value):
                    return Ping;

                    default:
                    return null;
            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case (CorpusInterfaceCase.Service_ping_Hash.Value):
                    return HashCode.Combine(Discriminator, this.ping);
                case (CorpusInterfaceCase.Service_Ping_Hash.Value):
                    return HashCode.Combine(Discriminator, this.Ping);
            }
            return HashCode.Combine(Discriminator);
        }

        public bool Equals(Service_Return other)
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
                case (CorpusInterfaceCase.Service_ping_Hash.Value):
                    return this.ping.Equals(other.ping);
                case (CorpusInterfaceCase.Service_Ping_Hash.Value):
                    return this.Ping.Equals(other.Ping);
            }
            return true;
        }

        public override bool Equals(object obj) => this.Equals(obj as Service_Return);

        public override string ToString() => Service_ReturnSupport.Instance.ToString(this);
    }

    public class Service :  IEquatable<Service>
    {
        public Non_basic ping { get; set; }
        public Non_basic Ping { get; set; }

        public Service()
        {
        }

        public Service(Non_basic  ping, Non_basic  Ping)
        {
            this.ping = ping;
            this.Ping = Ping;
        }

        public Service(Service other)
        {
            if (other == null)
            {
                return;
            }

            this.ping = other.ping;
            this.Ping = other.Ping;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.ping);
            hash.Add(this.Ping);

            return hash.ToHashCode();
        }

        public bool Equals(Service other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.ping.Equals(other.ping) && 
            this.Ping.Equals(other.Ping);
        }

        public override bool Equals(object obj) => this.Equals(obj as Service);

        public override string ToString() => ServiceSupport.Instance.ToString(this);
    }

    public class Sample :  IEquatable<Sample>
    {
        public int number { get; set; }

        public Sample()
        {
        }

        public Sample(int  number)
        {
            this.number = number;
        }

        public Sample(Sample other)
        {
            if (other == null)
            {
                return;
            }

            this.number = other.number;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.number);

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

            return this.number.Equals(other.number);
        }

        public override bool Equals(object obj) => this.Equals(obj as Sample);

        public override string ToString() => SampleSupport.Instance.ToString(this);
    }

} // namespace CorpusInterfaceCase
