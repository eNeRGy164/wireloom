
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-name-collisions.idl
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

namespace CorpusNameCollisions
{

    public class Item :  IEquatable<Item>
    {
        public int value { get; set; }

        public Item()
        {
        }

        public Item(int  value)
        {
            this.value = value;
        }

        public Item(Item other)
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

            hash.Add(this.value);

            return hash.ToHashCode();
        }

        public bool Equals(Item other)
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

        public override bool Equals(object obj) => this.Equals(obj as Item);

        public override string ToString() => ItemSupport.Instance.ToString(this);
    }

    public class LifecycleParameters :  IEquatable<LifecycleParameters>
    {
        [Bound(16)]
        public string optionalsOnly { get; set; } = string.Empty;
        public global::CorpusNameCollisions.Item allocatePointers { get; set; }
        [Bound(2)]
        public ISequence<int> allocateMemory { get; }

        public LifecycleParameters()
        {
            allocatePointers = new global::CorpusNameCollisions.Item();
            allocateMemory = new Rti.Types.Sequence<int>();
        }

        public LifecycleParameters(string  optionalsOnly, global::CorpusNameCollisions.Item  allocatePointers, ISequence<int>allocateMemory)
        {
            this.optionalsOnly = optionalsOnly;
            this.allocatePointers = allocatePointers;
            this.allocateMemory = allocateMemory;
        }

        public LifecycleParameters(LifecycleParameters other)
        {
            if (other == null)
            {
                return;
            }

            this.optionalsOnly = other.optionalsOnly;
            this.allocatePointers = new global::CorpusNameCollisions.Item(other.allocatePointers);
            this.allocateMemory = new Rti.Types.Sequence<int>(other.allocateMemory);

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.optionalsOnly);
            hash.Add(this.allocatePointers);
            hash.Add(this.allocateMemory.Count);

            return hash.ToHashCode();
        }

        public bool Equals(LifecycleParameters other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.optionalsOnly.Equals(other.optionalsOnly) && 
            this.allocatePointers.Equals(other.allocatePointers) && 
            this.allocateMemory.SequenceEqual(other.allocateMemory);
        }

        public override bool Equals(object obj) => this.Equals(obj as LifecycleParameters);

        public override string ToString() => LifecycleParametersSupport.Instance.ToString(this);
    }

    public class ManagedMethodParameters :  IEquatable<ManagedMethodParameters>
    {
        public int other { get; set; }
        public int hash { get; set; }
        public int sample { get; set; }
        public int keysOnly { get; set; }

        public ManagedMethodParameters()
        {
        }

        public ManagedMethodParameters(int  other, int  hash, int  sample, int  keysOnly)
        {
            this.other = other;
            this.hash = hash;
            this.sample = sample;
            this.keysOnly = keysOnly;
        }

        public ManagedMethodParameters(ManagedMethodParameters other)
        {
            if (other == null)
            {
                return;
            }

            this.other = other.other;
            this.hash = other.hash;
            this.sample = other.sample;
            this.keysOnly = other.keysOnly;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.other);
            hash.Add(this.hash);
            hash.Add(this.sample);
            hash.Add(this.keysOnly);

            return hash.ToHashCode();
        }

        public bool Equals(ManagedMethodParameters other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.other.Equals(other.other) && 
            this.hash.Equals(other.hash) && 
            this.sample.Equals(other.sample) && 
            this.keysOnly.Equals(other.keysOnly);
        }

        public override bool Equals(object obj) => this.Equals(obj as ManagedMethodParameters);

        public override string ToString() => ManagedMethodParametersSupport.Instance.ToString(this);
    }

    public class ArrayInitializationLocals :  IEquatable<ArrayInitializationLocals>
    {
        public global::CorpusNameCollisions.Item[] dimension0 { get; set; }

        public ArrayInitializationLocals()
        {
            dimension0 = new global::CorpusNameCollisions.Item[2];
            for( int i1 = 0; i1 < 2; i1++)
            {
                dimension0[i1] = new global::CorpusNameCollisions.Item();
            }
            ;
        }

        public ArrayInitializationLocals(global::CorpusNameCollisions.Item [] dimension0)
        {
            this.dimension0 = dimension0;
        }

        public ArrayInitializationLocals(ArrayInitializationLocals other)
        {
            if (other == null)
            {
                return;
            }

            this.dimension0 = new global::CorpusNameCollisions.Item[2];
            for( int i1 = 0; i1 < 2; i1++)
            {
                dimension0[i1] = new global::CorpusNameCollisions.Item(other.dimension0[i1]);
            }

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.dimension0[0]);

            return hash.ToHashCode();
        }

        public bool Equals(ArrayInitializationLocals other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.dimension0.SequenceEqual(other.dimension0);
        }

        public override bool Equals(object obj) => this.Equals(obj as ArrayInitializationLocals);

        public override string ToString() => ArrayInitializationLocalsSupport.Instance.ToString(this);
    }

    public class NativeInheritanceStorage :  IEquatable<NativeInheritanceStorage>
    {
        public int baseValue { get; set; }

        public NativeInheritanceStorage()
        {
        }

        public NativeInheritanceStorage(int  baseValue)
        {
            this.baseValue = baseValue;
        }

        public NativeInheritanceStorage(NativeInheritanceStorage other)
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

        public bool Equals(NativeInheritanceStorage other)
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

        public override bool Equals(object obj) => this.Equals(obj as NativeInheritanceStorage);

        public override string ToString() => NativeInheritanceStorageSupport.Instance.ToString(this);
    }

    public class Derived :  global::CorpusNameCollisions.NativeInheritanceStorage, IEquatable<Derived>
    {
        public int parent { get; set; }
        public int value { get; set; }

        public Derived()
        {
        }

        public Derived(int  baseValue, int  parent, int  value) : base(baseValue)
        {
            this.parent = parent;
            this.value = value;
        }

        public Derived(Derived other) : base(other)
        {
            if (other == null)
            {
                return;
            }

            this.parent = other.parent;
            this.value = other.value;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(base.GetHashCode());

            hash.Add(this.parent);
            hash.Add(this.value);

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

            return this.parent.Equals(other.parent) && 
            this.value.Equals(other.value);
        }

        public override bool Equals(object obj) => this.Equals(obj as Derived);

        public override string ToString() => DerivedSupport.Instance.ToString(this);
    }

    public class NativeLifecycleParameters :  IEquatable<NativeLifecycleParameters>
    {

        private string _optionalsOnly = string.Empty;

        private global::CorpusNameCollisions.Item _allocatePointers;

        private ISequence<int> _allocateMemory;

        private bool _flag;

        public int Discriminator { get; private set; }

        public const int DefaultDiscriminator = 0;

        [Bound(16)]
        public string optionalsOnly
        {
            get
            {
                if (Discriminator != 1) 
                {
                    throw new InvalidOperationException("optionalsOnly not selected");
                }
                return _optionalsOnly;
            }

            set
            {
                _optionalsOnly = value;
                Discriminator = 1;
            }
        }

        public global::CorpusNameCollisions.Item allocatePointers
        {
            get
            {
                if (Discriminator != 2) 
                {
                    throw new InvalidOperationException("allocatePointers not selected");
                }
                return _allocatePointers;
            }

            set
            {
                _allocatePointers = value;
                Discriminator = 2;
            }
        }

        [Bound(2)]
        public ISequence<int> allocateMemory
        {
            get
            {
                if (Discriminator != 3) 
                {
                    throw new InvalidOperationException("allocateMemory not selected");
                }
                return _allocateMemory;
            }

            set
            {
                _allocateMemory = value;
                Discriminator = 3;
            }
        }

        public bool flag
        {
            get
            {
                if (Discriminator == 1 || Discriminator == 2 || Discriminator == 3)

                {
                    throw new InvalidOperationException("flag not selected");
                }
                return _flag;
            }

            set
            {
                _flag = value;
                Discriminator = 0;
            }
        }

        public NativeLifecycleParameters()
        {
            Discriminator = DefaultDiscriminator;
        }

        public NativeLifecycleParameters(NativeLifecycleParameters other)
        {
            if (other == null)
            {
                return;
            }

            this.Discriminator = other.Discriminator;
            switch (Discriminator)
            {
                case 1:
                this._optionalsOnly = other.optionalsOnly;
                break;
                case 2:
                this._allocatePointers = new global::CorpusNameCollisions.Item(other.allocatePointers);
                break;
                case 3:
                this._allocateMemory = new Rti.Types.Sequence<int>(other.allocateMemory);
                break;
                default:
                this._flag = other.flag;
                break;
            }
        }

        public void Setflag(bool  flag, int discriminator)
        {
            if (discriminator == 1 || discriminator == 2 || discriminator == 3)

            {
                throw new ArgumentException("Invalid discriminator value for flag", paramName: nameof(discriminator));
            }
            this._flag = flag;
            Discriminator = discriminator;
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case 1:
                return optionalsOnly;
                case 2:
                return allocatePointers;
                case 3:
                return allocateMemory;
                default:
                return flag;

            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case 1:
                return HashCode.Combine(Discriminator, this.optionalsOnly);
                case 2:
                return HashCode.Combine(Discriminator, this.allocatePointers);
                case 3:
                return HashCode.Combine(Discriminator, this.allocateMemory.Count);
                default:
                return HashCode.Combine(Discriminator, this.flag);
            }
        }

        public bool Equals(NativeLifecycleParameters other)
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
                case 1:
                return this.optionalsOnly.Equals(other.optionalsOnly);
                case 2:
                return this.allocatePointers.Equals(other.allocatePointers);
                case 3:
                return this.allocateMemory.SequenceEqual(other.allocateMemory);
                default:
                return this.flag.Equals(other.flag);
            }
        }

        public override bool Equals(object obj) => this.Equals(obj as NativeLifecycleParameters);

        public override string ToString() => NativeLifecycleParametersSupport.Instance.ToString(this);
    }

    public class NativeDiscriminatorStorage :  IEquatable<NativeDiscriminatorStorage>
    {

        private int __discriminator;

        private bool _flag;

        public int Discriminator { get; private set; }

        public const int DefaultDiscriminator = 0;

        public int _discriminator
        {
            get
            {
                if (Discriminator != 1) 
                {
                    throw new InvalidOperationException("_discriminator not selected");
                }
                return __discriminator;
            }

            set
            {
                __discriminator = value;
                Discriminator = 1;
            }
        }

        public bool flag
        {
            get
            {
                if (Discriminator == 1)

                {
                    throw new InvalidOperationException("flag not selected");
                }
                return _flag;
            }

            set
            {
                _flag = value;
                Discriminator = 0;
            }
        }

        public NativeDiscriminatorStorage()
        {
            Discriminator = DefaultDiscriminator;
        }

        public NativeDiscriminatorStorage(NativeDiscriminatorStorage other)
        {
            if (other == null)
            {
                return;
            }

            this.Discriminator = other.Discriminator;
            switch (Discriminator)
            {
                case 1:
                this.__discriminator = other._discriminator;
                break;
                default:
                this._flag = other.flag;
                break;
            }
        }

        public void Setflag(bool  flag, int discriminator)
        {
            if (discriminator == 1)

            {
                throw new ArgumentException("Invalid discriminator value for flag", paramName: nameof(discriminator));
            }
            this._flag = flag;
            Discriminator = discriminator;
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case 1:
                return _discriminator;
                default:
                return flag;

            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case 1:
                return HashCode.Combine(Discriminator, this._discriminator);
                default:
                return HashCode.Combine(Discriminator, this.flag);
            }
        }

        public bool Equals(NativeDiscriminatorStorage other)
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
                case 1:
                return this._discriminator.Equals(other._discriminator);
                default:
                return this.flag.Equals(other.flag);
            }
        }

        public override bool Equals(object obj) => this.Equals(obj as NativeDiscriminatorStorage);

        public override string ToString() => NativeDiscriminatorStorageSupport.Instance.ToString(this);
    }

    public class ManagedDiscriminatorStorage :  IEquatable<ManagedDiscriminatorStorage>
    {

        private int _Discriminator;

        private bool _flag;

        public int Discriminator { get; private set; }

        public const int DefaultDiscriminator = 0;

        public int Discriminator
        {
            get
            {
                if (Discriminator != 1) 
                {
                    throw new InvalidOperationException("Discriminator not selected");
                }
                return _Discriminator;
            }

            set
            {
                _Discriminator = value;
                Discriminator = 1;
            }
        }

        public bool flag
        {
            get
            {
                if (Discriminator == 1)

                {
                    throw new InvalidOperationException("flag not selected");
                }
                return _flag;
            }

            set
            {
                _flag = value;
                Discriminator = 0;
            }
        }

        public ManagedDiscriminatorStorage()
        {
            Discriminator = DefaultDiscriminator;
        }

        public ManagedDiscriminatorStorage(ManagedDiscriminatorStorage other)
        {
            if (other == null)
            {
                return;
            }

            this.Discriminator = other.Discriminator;
            switch (Discriminator)
            {
                case 1:
                this._Discriminator = other.Discriminator;
                break;
                default:
                this._flag = other.flag;
                break;
            }
        }

        public void Setflag(bool  flag, int discriminator)
        {
            if (discriminator == 1)

            {
                throw new ArgumentException("Invalid discriminator value for flag", paramName: nameof(discriminator));
            }
            this._flag = flag;
            Discriminator = discriminator;
        }

        public object Get()
        {
            switch (Discriminator)
            {
                case 1:
                return Discriminator;
                default:
                return flag;

            }
        }

        public override int GetHashCode()
        {
            switch (Discriminator)
            {
                case 1:
                return HashCode.Combine(Discriminator, this.Discriminator);
                default:
                return HashCode.Combine(Discriminator, this.flag);
            }
        }

        public bool Equals(ManagedDiscriminatorStorage other)
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
                case 1:
                return this.Discriminator.Equals(other.Discriminator);
                default:
                return this.flag.Equals(other.flag);
            }
        }

        public override bool Equals(object obj) => this.Equals(obj as ManagedDiscriminatorStorage);

        public override string ToString() => ManagedDiscriminatorStorageSupport.Instance.ToString(this);
    }

} // namespace CorpusNameCollisions
