
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-interface-keyword-default.idl
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

namespace CorpusInterfaceKeyword
{

    public class Service_Context_In :  IEquatable<Service_Context_In>
    {
        public byte dummy { get; set; }

        public Service_Context_In()
        {
        }

        public Service_Context_In(byte  dummy)
        {
            this.dummy = dummy;
        }

        public Service_Context_In(Service_Context_In other)
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

        public bool Equals(Service_Context_In other)
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

        public override bool Equals(object obj) => this.Equals(obj as Service_Context_In);

        public override string ToString() => Service_Context_InSupport.Instance.ToString(this);
    }

    public class Service_Context_Out :  IEquatable<Service_Context_Out>
    {
        public byte dummy { get; set; }

        public Service_Context_Out()
        {
        }

        public Service_Context_Out(byte  dummy)
        {
            this.dummy = dummy;
        }

        public Service_Context_Out(Service_Context_Out other)
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

        public bool Equals(Service_Context_Out other)
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

        public override bool Equals(object obj) => this.Equals(obj as Service_Context_Out);

        public override string ToString() => Service_Context_OutSupport.Instance.ToString(this);
    }

    public class Service_Context_Result :  IEquatable<Service_Context_Result>
    {

        private global::CorpusInterfaceKeyword.Service_Context_Out _result;

        public int Discriminator { get; private set; }

        public const int DefaultDiscriminator = 0;

        public global::CorpusInterfaceKeyword.Service_Context_Out result
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

        public Service_Context_Result()
        {
            Discriminator = DefaultDiscriminator;

            _result = new global::CorpusInterfaceKeyword.Service_Context_Out();
        }

        public Service_Context_Result(Service_Context_Result other)
        {
            if (other == null)
            {
                return;
            }

            this.Discriminator = other.Discriminator;
            switch (Discriminator)
            {
                case 0:
                this._result = new global::CorpusInterfaceKeyword.Service_Context_Out(other.result);
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

        public bool Equals(Service_Context_Result other)
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

        public override bool Equals(object obj) => this.Equals(obj as Service_Context_Result);

        public override string ToString() => Service_Context_ResultSupport.Instance.ToString(this);
    }

    public static class Service_Context_Hash
    {
        public const int Value = 1679838893;
    }

    public class Service_Call :  IEquatable<Service_Call>
    {

        private global::CorpusInterfaceKeyword.Service_Context_In _Context;

        public int Discriminator { get; private set; }

        public const int DefaultDiscriminator = (CorpusInterfaceKeyword.Service_Context_Hash.Value);

        public global::CorpusInterfaceKeyword.Service_Context_In Context
        {
            get
            {
                if (Discriminator != (CorpusInterfaceKeyword.Service_Context_Hash.Value)) 
                {
                    throw new InvalidOperationException("Context not selected");
                }
                return _Context;
            }

            set
            {
                _Context = value;
                Discriminator = (CorpusInterfaceKeyword.Service_Context_Hash.Value);
            }
        }

        public Service_Call()
        {
            Discriminator = DefaultDiscriminator;

            _Context = new global::CorpusInterfaceKeyword.Service_Context_In();
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
                case (CorpusInterfaceKeyword.Service_Context_Hash.Value):
                    this._Context = new global::CorpusInterfaceKeyword.Service_Context_In(other.Context);
                    break;
            }
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case (CorpusInterfaceKeyword.Service_Context_Hash.Value):
                    return Context;

                    default:
                    return null;
            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case (CorpusInterfaceKeyword.Service_Context_Hash.Value):
                    return HashCode.Combine(Discriminator, this.Context);
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
                case (CorpusInterfaceKeyword.Service_Context_Hash.Value):
                    return this.Context.Equals(other.Context);
            }
            return true;
        }

        public override bool Equals(object obj) => this.Equals(obj as Service_Call);

        public override string ToString() => Service_CallSupport.Instance.ToString(this);
    }

    public class Service_Return :  IEquatable<Service_Return>
    {

        private global::CorpusInterfaceKeyword.Service_Context_Result _Context;

        public int Discriminator { get; private set; }

        public const int DefaultDiscriminator = (CorpusInterfaceKeyword.Service_Context_Hash.Value);

        public global::CorpusInterfaceKeyword.Service_Context_Result Context
        {
            get
            {
                if (Discriminator != (CorpusInterfaceKeyword.Service_Context_Hash.Value)) 
                {
                    throw new InvalidOperationException("Context not selected");
                }
                return _Context;
            }

            set
            {
                _Context = value;
                Discriminator = (CorpusInterfaceKeyword.Service_Context_Hash.Value);
            }
        }

        public Service_Return()
        {
            Discriminator = DefaultDiscriminator;

            _Context = new global::CorpusInterfaceKeyword.Service_Context_Result();
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
                case (CorpusInterfaceKeyword.Service_Context_Hash.Value):
                    this._Context = new global::CorpusInterfaceKeyword.Service_Context_Result(other.Context);
                    break;
            }
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case (CorpusInterfaceKeyword.Service_Context_Hash.Value):
                    return Context;

                    default:
                    return null;
            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case (CorpusInterfaceKeyword.Service_Context_Hash.Value):
                    return HashCode.Combine(Discriminator, this.Context);
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
                case (CorpusInterfaceKeyword.Service_Context_Hash.Value):
                    return this.Context.Equals(other.Context);
            }
            return true;
        }

        public override bool Equals(object obj) => this.Equals(obj as Service_Return);

        public override string ToString() => Service_ReturnSupport.Instance.ToString(this);
    }

    public class Service :  IEquatable<Service>
    {
        public Non_basic Context { get; set; }

        public Service()
        {
        }

        public Service(Non_basic  Context)
        {
            this.Context = Context;
        }

        public Service(Service other)
        {
            if (other == null)
            {
                return;
            }

            this.Context = other.Context;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.Context);

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

            return this.Context.Equals(other.Context);
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

} // namespace CorpusInterfaceKeyword
