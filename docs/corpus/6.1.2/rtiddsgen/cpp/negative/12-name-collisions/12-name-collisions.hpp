

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-name-collisions.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _12_name_collisions_687327134_hpp
#define _12_name_collisions_687327134_hpp

#include <iosfwd>

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, start exporting symbols.
*/
#undef RTIUSERDllExport
#define RTIUSERDllExport __declspec(dllexport)
#endif

#include "dds/domain/DomainParticipant.hpp"
#include "dds/topic/TopicTraits.hpp"
#include "dds/core/SafeEnumeration.hpp"
#include "dds/core/String.hpp"
#include "dds/core/array.hpp"
#include "dds/core/vector.hpp"
#include "dds/core/Optional.hpp"
#include "dds/core/xtypes/DynamicType.hpp"
#include "dds/core/xtypes/StructType.hpp"
#include "dds/core/xtypes/UnionType.hpp"
#include "dds/core/xtypes/EnumType.hpp"
#include "dds/core/xtypes/AliasType.hpp"
#include "rti/core/array.hpp"
#include "rti/core/BoundedSequence.hpp"
#include "rti/util/StreamFlagSaver.hpp"
#include "rti/domain/PluginSupport.hpp"
#include "rti/core/LongDouble.hpp"
#include "dds/core/External.hpp"
#include "rti/core/Pointer.hpp"
#include "rti/topic/TopicTraits.hpp"

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef RTIUSERDllExport
#define RTIUSERDllExport
#endif

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, start exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

namespace CorpusNameCollisions {

    class NDDSUSERDllExport Item {
      public:
        Item();

        explicit Item(int32_t value);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Item (Item&&) = default;
        Item& operator=(Item&&) = default;
        Item& operator=(const Item&) = default;
        Item(const Item&) = default;
        #else
        Item(Item&& other_) OMG_NOEXCEPT;  
        Item& operator=(Item&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        int32_t& value() OMG_NOEXCEPT {
            return m_value_;
        }

        const int32_t& value() const OMG_NOEXCEPT {
            return m_value_;
        }

        void value(int32_t value) {
            m_value_ = value;
        }

        bool operator == (const Item& other_) const;
        bool operator != (const Item& other_) const;

        void swap(Item& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_value_;

    };

    inline void swap(Item& a, Item& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Item& sample);

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< int32_t >;
    template class NDDSUSERDllExport std::vector< int32_t >;
    #endif
    class NDDSUSERDllExport LifecycleParameters {
      public:
        LifecycleParameters();

        LifecycleParameters(const std::string& optionalsOnly,const CorpusNameCollisions::Item& allocatePointers,const ::rti::core::bounded_sequence< int32_t, 2L >& allocateMemory);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        LifecycleParameters (LifecycleParameters&&) = default;
        LifecycleParameters& operator=(LifecycleParameters&&) = default;
        LifecycleParameters& operator=(const LifecycleParameters&) = default;
        LifecycleParameters(const LifecycleParameters&) = default;
        #else
        LifecycleParameters(LifecycleParameters&& other_) OMG_NOEXCEPT;  
        LifecycleParameters& operator=(LifecycleParameters&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        std::string& optionalsOnly() OMG_NOEXCEPT {
            return m_optionalsOnly_;
        }

        const std::string& optionalsOnly() const OMG_NOEXCEPT {
            return m_optionalsOnly_;
        }

        void optionalsOnly(const std::string& value) {
            m_optionalsOnly_ = value;
        }

        void optionalsOnly(std::string&& value) {
            m_optionalsOnly_ = std::move(value);
        }
        CorpusNameCollisions::Item& allocatePointers() OMG_NOEXCEPT {
            return m_allocatePointers_;
        }

        const CorpusNameCollisions::Item& allocatePointers() const OMG_NOEXCEPT {
            return m_allocatePointers_;
        }

        void allocatePointers(const CorpusNameCollisions::Item& value) {
            m_allocatePointers_ = value;
        }

        void allocatePointers(CorpusNameCollisions::Item&& value) {
            m_allocatePointers_ = std::move(value);
        }
        ::rti::core::bounded_sequence< int32_t, 2L >& allocateMemory() OMG_NOEXCEPT {
            return m_allocateMemory_;
        }

        const ::rti::core::bounded_sequence< int32_t, 2L >& allocateMemory() const OMG_NOEXCEPT {
            return m_allocateMemory_;
        }

        void allocateMemory(const ::rti::core::bounded_sequence< int32_t, 2L >& value) {
            m_allocateMemory_ = value;
        }

        void allocateMemory(::rti::core::bounded_sequence< int32_t, 2L >&& value) {
            m_allocateMemory_ = std::move(value);
        }

        bool operator == (const LifecycleParameters& other_) const;
        bool operator != (const LifecycleParameters& other_) const;

        void swap(LifecycleParameters& other_) OMG_NOEXCEPT ;

      private:

        std::string m_optionalsOnly_;
        CorpusNameCollisions::Item m_allocatePointers_;
        ::rti::core::bounded_sequence< int32_t, 2L > m_allocateMemory_;

    };

    inline void swap(LifecycleParameters& a, LifecycleParameters& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const LifecycleParameters& sample);

    class NDDSUSERDllExport ManagedMethodParameters {
      public:
        ManagedMethodParameters();

        ManagedMethodParameters(int32_t other,int32_t hash,int32_t sample,int32_t keysOnly);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        ManagedMethodParameters (ManagedMethodParameters&&) = default;
        ManagedMethodParameters& operator=(ManagedMethodParameters&&) = default;
        ManagedMethodParameters& operator=(const ManagedMethodParameters&) = default;
        ManagedMethodParameters(const ManagedMethodParameters&) = default;
        #else
        ManagedMethodParameters(ManagedMethodParameters&& other_) OMG_NOEXCEPT;  
        ManagedMethodParameters& operator=(ManagedMethodParameters&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        int32_t& other() OMG_NOEXCEPT {
            return m_other_;
        }

        const int32_t& other() const OMG_NOEXCEPT {
            return m_other_;
        }

        void other(int32_t value) {
            m_other_ = value;
        }

        int32_t& hash() OMG_NOEXCEPT {
            return m_hash_;
        }

        const int32_t& hash() const OMG_NOEXCEPT {
            return m_hash_;
        }

        void hash(int32_t value) {
            m_hash_ = value;
        }

        int32_t& sample() OMG_NOEXCEPT {
            return m_sample_;
        }

        const int32_t& sample() const OMG_NOEXCEPT {
            return m_sample_;
        }

        void sample(int32_t value) {
            m_sample_ = value;
        }

        int32_t& keysOnly() OMG_NOEXCEPT {
            return m_keysOnly_;
        }

        const int32_t& keysOnly() const OMG_NOEXCEPT {
            return m_keysOnly_;
        }

        void keysOnly(int32_t value) {
            m_keysOnly_ = value;
        }

        bool operator == (const ManagedMethodParameters& other_) const;
        bool operator != (const ManagedMethodParameters& other_) const;

        void swap(ManagedMethodParameters& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_other_;
        int32_t m_hash_;
        int32_t m_sample_;
        int32_t m_keysOnly_;

    };

    inline void swap(ManagedMethodParameters& a, ManagedMethodParameters& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const ManagedMethodParameters& sample);

    class NDDSUSERDllExport ArrayInitializationLocals {
      public:
        ArrayInitializationLocals();

        explicit ArrayInitializationLocals(const ::dds::core::array< CorpusNameCollisions::Item, 2L>& dimension0);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        ArrayInitializationLocals (ArrayInitializationLocals&&) = default;
        ArrayInitializationLocals& operator=(ArrayInitializationLocals&&) = default;
        ArrayInitializationLocals& operator=(const ArrayInitializationLocals&) = default;
        ArrayInitializationLocals(const ArrayInitializationLocals&) = default;
        #else
        ArrayInitializationLocals(ArrayInitializationLocals&& other_) OMG_NOEXCEPT;  
        ArrayInitializationLocals& operator=(ArrayInitializationLocals&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        ::dds::core::array< CorpusNameCollisions::Item, 2L>& dimension0() OMG_NOEXCEPT {
            return m_dimension0_;
        }

        const ::dds::core::array< CorpusNameCollisions::Item, 2L>& dimension0() const OMG_NOEXCEPT {
            return m_dimension0_;
        }

        void dimension0(const ::dds::core::array< CorpusNameCollisions::Item, 2L>& value) {
            m_dimension0_ = value;
        }

        void dimension0(::dds::core::array< CorpusNameCollisions::Item, 2L>&& value) {
            m_dimension0_ = std::move(value);
        }

        bool operator == (const ArrayInitializationLocals& other_) const;
        bool operator != (const ArrayInitializationLocals& other_) const;

        void swap(ArrayInitializationLocals& other_) OMG_NOEXCEPT ;

      private:

        ::dds::core::array< CorpusNameCollisions::Item, 2L> m_dimension0_;

    };

    inline void swap(ArrayInitializationLocals& a, ArrayInitializationLocals& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const ArrayInitializationLocals& sample);

    class NDDSUSERDllExport NativeInheritanceStorage {
      public:
        NativeInheritanceStorage();

        explicit NativeInheritanceStorage(int32_t baseValue);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        NativeInheritanceStorage (NativeInheritanceStorage&&) = default;
        NativeInheritanceStorage& operator=(NativeInheritanceStorage&&) = default;
        NativeInheritanceStorage& operator=(const NativeInheritanceStorage&) = default;
        NativeInheritanceStorage(const NativeInheritanceStorage&) = default;
        #else
        NativeInheritanceStorage(NativeInheritanceStorage&& other_) OMG_NOEXCEPT;  
        NativeInheritanceStorage& operator=(NativeInheritanceStorage&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        int32_t& baseValue() OMG_NOEXCEPT {
            return m_baseValue_;
        }

        const int32_t& baseValue() const OMG_NOEXCEPT {
            return m_baseValue_;
        }

        void baseValue(int32_t value) {
            m_baseValue_ = value;
        }

        bool operator == (const NativeInheritanceStorage& other_) const;
        bool operator != (const NativeInheritanceStorage& other_) const;

        void swap(NativeInheritanceStorage& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_baseValue_;

    };

    inline void swap(NativeInheritanceStorage& a, NativeInheritanceStorage& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const NativeInheritanceStorage& sample);

    class NDDSUSERDllExport Derived
    : public CorpusNameCollisions::NativeInheritanceStorage {
      public:
        Derived();

        Derived(int32_t baseValue,int32_t parent,int32_t value);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Derived (Derived&&) = default;
        Derived& operator=(Derived&&) = default;
        Derived& operator=(const Derived&) = default;
        Derived(const Derived&) = default;
        #else
        Derived(Derived&& other_) OMG_NOEXCEPT;  
        Derived& operator=(Derived&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        int32_t& parent() OMG_NOEXCEPT {
            return m_parent_;
        }

        const int32_t& parent() const OMG_NOEXCEPT {
            return m_parent_;
        }

        void parent(int32_t value) {
            m_parent_ = value;
        }

        int32_t& value() OMG_NOEXCEPT {
            return m_value_;
        }

        const int32_t& value() const OMG_NOEXCEPT {
            return m_value_;
        }

        void value(int32_t value) {
            m_value_ = value;
        }

        bool operator == (const Derived& other_) const;
        bool operator != (const Derived& other_) const;

        void swap(Derived& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_parent_;
        int32_t m_value_;

    };

    inline void swap(Derived& a, Derived& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Derived& sample);

    class NDDSUSERDllExport NativeLifecycleParameters {
      public:
        NativeLifecycleParameters();

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        NativeLifecycleParameters (NativeLifecycleParameters&&) = default;
        NativeLifecycleParameters& operator=(NativeLifecycleParameters&&) = default;
        NativeLifecycleParameters& operator=(const NativeLifecycleParameters&) = default;
        NativeLifecycleParameters(const NativeLifecycleParameters&) = default;
        #else
        NativeLifecycleParameters(NativeLifecycleParameters&& other_) OMG_NOEXCEPT;  
        NativeLifecycleParameters& operator=(NativeLifecycleParameters&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        int32_t& _d()  {
            return m_d_;
        }

        const int32_t& _d() const  {
            return m_d_;
        }

        void _d(int32_t value) {
            m_d_ = value;
        }

        std::string& optionalsOnly()  {
            if ( _d() != 1) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::NativeLifecycleParameters::optionalsOnly not selected by the discriminator" );
            }
            return m_u_.m_optionalsOnly_;
        }

        const std::string& optionalsOnly() const  {
            if ( _d() != 1) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::NativeLifecycleParameters::optionalsOnly not selected by the discriminator" );
            }
            return m_u_.m_optionalsOnly_;
        }

        void optionalsOnly(const std::string& value) {
            m_u_.m_optionalsOnly_ = value;
            m_d_= 1;
        }

        void optionalsOnly(std::string&& value) {
            m_u_.m_optionalsOnly_ = std::move(value);
            m_d_= 1;
        }
        CorpusNameCollisions::Item& allocatePointers()  {
            if ( _d() != 2) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::NativeLifecycleParameters::allocatePointers not selected by the discriminator" );
            }
            return m_u_.m_allocatePointers_;
        }

        const CorpusNameCollisions::Item& allocatePointers() const  {
            if ( _d() != 2) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::NativeLifecycleParameters::allocatePointers not selected by the discriminator" );
            }
            return m_u_.m_allocatePointers_;
        }

        void allocatePointers(const CorpusNameCollisions::Item& value) {
            m_u_.m_allocatePointers_ = value;
            m_d_= 2;
        }

        void allocatePointers(CorpusNameCollisions::Item&& value) {
            m_u_.m_allocatePointers_ = std::move(value);
            m_d_= 2;
        }
        ::rti::core::bounded_sequence< int32_t, 2L >& allocateMemory()  {
            if ( _d() != 3) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::NativeLifecycleParameters::allocateMemory not selected by the discriminator" );
            }
            return m_u_.m_allocateMemory_;
        }

        const ::rti::core::bounded_sequence< int32_t, 2L >& allocateMemory() const  {
            if ( _d() != 3) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::NativeLifecycleParameters::allocateMemory not selected by the discriminator" );
            }
            return m_u_.m_allocateMemory_;
        }

        void allocateMemory(const ::rti::core::bounded_sequence< int32_t, 2L >& value) {
            m_u_.m_allocateMemory_ = value;
            m_d_= 3;
        }

        void allocateMemory(::rti::core::bounded_sequence< int32_t, 2L >&& value) {
            m_u_.m_allocateMemory_ = std::move(value);
            m_d_= 3;
        }
        bool& flag()  {
            if (!(_d() != 1 &&
            _d() != 2 &&
            _d() != 3)) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::NativeLifecycleParameters::flag not selected by the discriminator" );
            }
            return m_u_.m_flag_;
        }

        const bool& flag() const  {
            if (!(_d() != 1 &&
            _d() != 2 &&
            _d() != 3)) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::NativeLifecycleParameters::flag not selected by the discriminator" );
            }
            return m_u_.m_flag_;
        }

        void flag(bool value) {
            if (!(_d() != 1 &&
            _d() != 2 &&
            _d() != 3)) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::NativeLifecycleParameters::flag not selected by the discriminator" );
            }
            m_u_.m_flag_ = value;
        }

        bool operator == (const NativeLifecycleParameters& other_) const;
        bool operator != (const NativeLifecycleParameters& other_) const;

        static int32_t default_discriminator();  

        void swap(NativeLifecycleParameters& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_d_;
        struct NDDSUSERDllExport Union_ {
            std::string m_optionalsOnly_;
            CorpusNameCollisions::Item m_allocatePointers_;
            ::rti::core::bounded_sequence< int32_t, 2L > m_allocateMemory_;
            bool m_flag_;
            Union_();
            Union_(const std::string& optionalsOnly,const CorpusNameCollisions::Item& allocatePointers,const ::rti::core::bounded_sequence< int32_t, 2L >& allocateMemory,bool flag);
        };
        Union_ m_u_;

    };

    inline void swap(NativeLifecycleParameters& a, NativeLifecycleParameters& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const NativeLifecycleParameters& sample);

    class NDDSUSERDllExport NativeDiscriminatorStorage {
      public:
        NativeDiscriminatorStorage();

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        NativeDiscriminatorStorage (NativeDiscriminatorStorage&&) = default;
        NativeDiscriminatorStorage& operator=(NativeDiscriminatorStorage&&) = default;
        NativeDiscriminatorStorage& operator=(const NativeDiscriminatorStorage&) = default;
        NativeDiscriminatorStorage(const NativeDiscriminatorStorage&) = default;
        #else
        NativeDiscriminatorStorage(NativeDiscriminatorStorage&& other_) OMG_NOEXCEPT;  
        NativeDiscriminatorStorage& operator=(NativeDiscriminatorStorage&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        int32_t& _d()  {
            return m_d_;
        }

        const int32_t& _d() const  {
            return m_d_;
        }

        void _d(int32_t value) {
            m_d_ = value;
        }

        int32_t& _discriminator()  {
            if ( _d() != 1) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::NativeDiscriminatorStorage::_discriminator not selected by the discriminator" );
            }
            return m_u_.m__discriminator_;
        }

        const int32_t& _discriminator() const  {
            if ( _d() != 1) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::NativeDiscriminatorStorage::_discriminator not selected by the discriminator" );
            }
            return m_u_.m__discriminator_;
        }

        void _discriminator(int32_t value) {
            m_u_.m__discriminator_ = value;
            m_d_= 1;
        }

        bool& flag()  {
            if (!(_d() != 1)) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::NativeDiscriminatorStorage::flag not selected by the discriminator" );
            }
            return m_u_.m_flag_;
        }

        const bool& flag() const  {
            if (!(_d() != 1)) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::NativeDiscriminatorStorage::flag not selected by the discriminator" );
            }
            return m_u_.m_flag_;
        }

        void flag(bool value) {
            if (!(_d() != 1)) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::NativeDiscriminatorStorage::flag not selected by the discriminator" );
            }
            m_u_.m_flag_ = value;
        }

        bool operator == (const NativeDiscriminatorStorage& other_) const;
        bool operator != (const NativeDiscriminatorStorage& other_) const;

        static int32_t default_discriminator();  

        void swap(NativeDiscriminatorStorage& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_d_;
        struct NDDSUSERDllExport Union_ {
            int32_t m__discriminator_;
            bool m_flag_;
            Union_();
            Union_(int32_t _discriminator,bool flag);
        };
        Union_ m_u_;

    };

    inline void swap(NativeDiscriminatorStorage& a, NativeDiscriminatorStorage& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const NativeDiscriminatorStorage& sample);

    class NDDSUSERDllExport ManagedDiscriminatorStorage {
      public:
        ManagedDiscriminatorStorage();

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        ManagedDiscriminatorStorage (ManagedDiscriminatorStorage&&) = default;
        ManagedDiscriminatorStorage& operator=(ManagedDiscriminatorStorage&&) = default;
        ManagedDiscriminatorStorage& operator=(const ManagedDiscriminatorStorage&) = default;
        ManagedDiscriminatorStorage(const ManagedDiscriminatorStorage&) = default;
        #else
        ManagedDiscriminatorStorage(ManagedDiscriminatorStorage&& other_) OMG_NOEXCEPT;  
        ManagedDiscriminatorStorage& operator=(ManagedDiscriminatorStorage&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        int32_t& _d()  {
            return m_d_;
        }

        const int32_t& _d() const  {
            return m_d_;
        }

        void _d(int32_t value) {
            m_d_ = value;
        }

        int32_t& Discriminator()  {
            if ( _d() != 1) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::ManagedDiscriminatorStorage::Discriminator not selected by the discriminator" );
            }
            return m_u_.m_Discriminator_;
        }

        const int32_t& Discriminator() const  {
            if ( _d() != 1) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::ManagedDiscriminatorStorage::Discriminator not selected by the discriminator" );
            }
            return m_u_.m_Discriminator_;
        }

        void Discriminator(int32_t value) {
            m_u_.m_Discriminator_ = value;
            m_d_= 1;
        }

        bool& flag()  {
            if (!(_d() != 1)) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::ManagedDiscriminatorStorage::flag not selected by the discriminator" );
            }
            return m_u_.m_flag_;
        }

        const bool& flag() const  {
            if (!(_d() != 1)) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::ManagedDiscriminatorStorage::flag not selected by the discriminator" );
            }
            return m_u_.m_flag_;
        }

        void flag(bool value) {
            if (!(_d() != 1)) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusNameCollisions::ManagedDiscriminatorStorage::flag not selected by the discriminator" );
            }
            m_u_.m_flag_ = value;
        }

        bool operator == (const ManagedDiscriminatorStorage& other_) const;
        bool operator != (const ManagedDiscriminatorStorage& other_) const;

        static int32_t default_discriminator();  

        void swap(ManagedDiscriminatorStorage& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_d_;
        struct NDDSUSERDllExport Union_ {
            int32_t m_Discriminator_;
            bool m_flag_;
            Union_();
            Union_(int32_t Discriminator,bool flag);
        };
        Union_ m_u_;

    };

    inline void swap(ManagedDiscriminatorStorage& a, ManagedDiscriminatorStorage& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const ManagedDiscriminatorStorage& sample);

} // namespace CorpusNameCollisions  

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< CorpusNameCollisions::Item > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusNameCollisions::Item";
            }
        };

        template<>
        struct is_topic_type< CorpusNameCollisions::Item > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusNameCollisions::Item > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusNameCollisions::Item& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusNameCollisions::Item& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusNameCollisions::Item& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusNameCollisions::Item& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusNameCollisions::LifecycleParameters > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusNameCollisions::LifecycleParameters";
            }
        };

        template<>
        struct is_topic_type< CorpusNameCollisions::LifecycleParameters > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusNameCollisions::LifecycleParameters > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusNameCollisions::LifecycleParameters& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusNameCollisions::LifecycleParameters& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusNameCollisions::LifecycleParameters& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusNameCollisions::LifecycleParameters& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusNameCollisions::ManagedMethodParameters > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusNameCollisions::ManagedMethodParameters";
            }
        };

        template<>
        struct is_topic_type< CorpusNameCollisions::ManagedMethodParameters > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusNameCollisions::ManagedMethodParameters > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusNameCollisions::ManagedMethodParameters& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusNameCollisions::ManagedMethodParameters& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusNameCollisions::ManagedMethodParameters& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusNameCollisions::ManagedMethodParameters& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusNameCollisions::ArrayInitializationLocals > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusNameCollisions::ArrayInitializationLocals";
            }
        };

        template<>
        struct is_topic_type< CorpusNameCollisions::ArrayInitializationLocals > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusNameCollisions::ArrayInitializationLocals > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusNameCollisions::ArrayInitializationLocals& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusNameCollisions::ArrayInitializationLocals& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusNameCollisions::ArrayInitializationLocals& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusNameCollisions::ArrayInitializationLocals& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusNameCollisions::NativeInheritanceStorage > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusNameCollisions::NativeInheritanceStorage";
            }
        };

        template<>
        struct is_topic_type< CorpusNameCollisions::NativeInheritanceStorage > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusNameCollisions::NativeInheritanceStorage > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusNameCollisions::NativeInheritanceStorage& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusNameCollisions::NativeInheritanceStorage& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusNameCollisions::NativeInheritanceStorage& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusNameCollisions::NativeInheritanceStorage& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusNameCollisions::Derived > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusNameCollisions::Derived";
            }
        };

        template<>
        struct is_topic_type< CorpusNameCollisions::Derived > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusNameCollisions::Derived > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusNameCollisions::Derived& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusNameCollisions::Derived& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusNameCollisions::Derived& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusNameCollisions::Derived& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusNameCollisions::NativeLifecycleParameters > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusNameCollisions::NativeLifecycleParameters";
            }
        };

        template<>
        struct is_topic_type< CorpusNameCollisions::NativeLifecycleParameters > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusNameCollisions::NativeLifecycleParameters > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusNameCollisions::NativeLifecycleParameters& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusNameCollisions::NativeLifecycleParameters& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusNameCollisions::NativeLifecycleParameters& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusNameCollisions::NativeLifecycleParameters& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusNameCollisions::NativeDiscriminatorStorage > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusNameCollisions::NativeDiscriminatorStorage";
            }
        };

        template<>
        struct is_topic_type< CorpusNameCollisions::NativeDiscriminatorStorage > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusNameCollisions::NativeDiscriminatorStorage > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusNameCollisions::NativeDiscriminatorStorage& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusNameCollisions::NativeDiscriminatorStorage& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusNameCollisions::NativeDiscriminatorStorage& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusNameCollisions::NativeDiscriminatorStorage& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusNameCollisions::ManagedDiscriminatorStorage > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusNameCollisions::ManagedDiscriminatorStorage";
            }
        };

        template<>
        struct is_topic_type< CorpusNameCollisions::ManagedDiscriminatorStorage > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusNameCollisions::ManagedDiscriminatorStorage > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusNameCollisions::ManagedDiscriminatorStorage& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusNameCollisions::ManagedDiscriminatorStorage& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusNameCollisions::ManagedDiscriminatorStorage& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusNameCollisions::ManagedDiscriminatorStorage& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusNameCollisions::Item > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusNameCollisions::Item > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusNameCollisions::LifecycleParameters > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusNameCollisions::LifecycleParameters > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusNameCollisions::ManagedMethodParameters > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusNameCollisions::ManagedMethodParameters > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusNameCollisions::ArrayInitializationLocals > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusNameCollisions::ArrayInitializationLocals > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusNameCollisions::NativeInheritanceStorage > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusNameCollisions::NativeInheritanceStorage > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusNameCollisions::Derived > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusNameCollisions::Derived > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusNameCollisions::NativeLifecycleParameters > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusNameCollisions::NativeLifecycleParameters > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusNameCollisions::NativeDiscriminatorStorage > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusNameCollisions::NativeDiscriminatorStorage > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusNameCollisions::ManagedDiscriminatorStorage > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusNameCollisions::ManagedDiscriminatorStorage > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

    }
}

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _12_name_collisions_687327134_hpp

