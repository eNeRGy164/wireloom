

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-name-collisions.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#include <iosfwd>
#include <iomanip>

#include "rti/topic/cdr/Serialization.hpp"

#include "12-name-collisions.hpp"
#include "12-name-collisionsPlugin.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusNameCollisions {

    // ---- Item: 

    Item::Item() :
        m_value_ (0)  {
    }   

    Item::Item (int32_t value):
        m_value_( value ) {
    }

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifdef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    Item::Item(Item&& other_) OMG_NOEXCEPT  :m_value_ (std::move(other_.m_value_))
    {
    } 

    Item& Item::operator=(Item&&  other_) OMG_NOEXCEPT {
        Item tmp(std::move(other_));
        swap(tmp); 
        return *this;
    }
    #endif
    #endif   

    void Item::swap(Item& other_)  OMG_NOEXCEPT 
    {
        using std::swap;
        swap(m_value_, other_.m_value_);
    }  

    bool Item::operator == (const Item& other_) const {
        if (m_value_ != other_.m_value_) {
            return false;
        }
        return true;
    }
    bool Item::operator != (const Item& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Item& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "value: " << sample.value() ;
        o <<"]";
        return o;
    }

    // ---- LifecycleParameters: 

    LifecycleParameters::LifecycleParameters() :
        m_optionalsOnly_ ("")  {
    }   

    LifecycleParameters::LifecycleParameters (const std::string& optionalsOnly,const CorpusNameCollisions::Item& allocatePointers,const ::rti::core::bounded_sequence< int32_t, 2L >& allocateMemory):
        m_optionalsOnly_( optionalsOnly ),
        m_allocatePointers_( allocatePointers ),
        m_allocateMemory_( allocateMemory ) {
    }

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifdef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    LifecycleParameters::LifecycleParameters(LifecycleParameters&& other_) OMG_NOEXCEPT  :m_optionalsOnly_ (std::move(other_.m_optionalsOnly_))
    ,
    m_allocatePointers_ (std::move(other_.m_allocatePointers_))
    ,
    m_allocateMemory_ (std::move(other_.m_allocateMemory_))
    {
    } 

    LifecycleParameters& LifecycleParameters::operator=(LifecycleParameters&&  other_) OMG_NOEXCEPT {
        LifecycleParameters tmp(std::move(other_));
        swap(tmp); 
        return *this;
    }
    #endif
    #endif   

    void LifecycleParameters::swap(LifecycleParameters& other_)  OMG_NOEXCEPT 
    {
        using std::swap;
        swap(m_optionalsOnly_, other_.m_optionalsOnly_);
        swap(m_allocatePointers_, other_.m_allocatePointers_);
        swap(m_allocateMemory_, other_.m_allocateMemory_);
    }  

    bool LifecycleParameters::operator == (const LifecycleParameters& other_) const {
        if (m_optionalsOnly_ != other_.m_optionalsOnly_) {
            return false;
        }
        if (m_allocatePointers_ != other_.m_allocatePointers_) {
            return false;
        }
        if (m_allocateMemory_ != other_.m_allocateMemory_) {
            return false;
        }
        return true;
    }
    bool LifecycleParameters::operator != (const LifecycleParameters& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const LifecycleParameters& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "optionalsOnly: " << sample.optionalsOnly()<<", ";
        o << "allocatePointers: " << sample.allocatePointers()<<", ";
        o << "allocateMemory: " << sample.allocateMemory() ;
        o <<"]";
        return o;
    }

    // ---- ManagedMethodParameters: 

    ManagedMethodParameters::ManagedMethodParameters() :
        m_other_ (0) ,
        m_hash_ (0) ,
        m_sample_ (0) ,
        m_keysOnly_ (0)  {
    }   

    ManagedMethodParameters::ManagedMethodParameters (int32_t other,int32_t hash,int32_t sample,int32_t keysOnly):
        m_other_( other ),
        m_hash_( hash ),
        m_sample_( sample ),
        m_keysOnly_( keysOnly ) {
    }

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifdef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    ManagedMethodParameters::ManagedMethodParameters(ManagedMethodParameters&& other_) OMG_NOEXCEPT  :m_other_ (std::move(other_.m_other_))
    ,
    m_hash_ (std::move(other_.m_hash_))
    ,
    m_sample_ (std::move(other_.m_sample_))
    ,
    m_keysOnly_ (std::move(other_.m_keysOnly_))
    {
    } 

    ManagedMethodParameters& ManagedMethodParameters::operator=(ManagedMethodParameters&&  other_) OMG_NOEXCEPT {
        ManagedMethodParameters tmp(std::move(other_));
        swap(tmp); 
        return *this;
    }
    #endif
    #endif   

    void ManagedMethodParameters::swap(ManagedMethodParameters& other_)  OMG_NOEXCEPT 
    {
        using std::swap;
        swap(m_other_, other_.m_other_);
        swap(m_hash_, other_.m_hash_);
        swap(m_sample_, other_.m_sample_);
        swap(m_keysOnly_, other_.m_keysOnly_);
    }  

    bool ManagedMethodParameters::operator == (const ManagedMethodParameters& other_) const {
        if (m_other_ != other_.m_other_) {
            return false;
        }
        if (m_hash_ != other_.m_hash_) {
            return false;
        }
        if (m_sample_ != other_.m_sample_) {
            return false;
        }
        if (m_keysOnly_ != other_.m_keysOnly_) {
            return false;
        }
        return true;
    }
    bool ManagedMethodParameters::operator != (const ManagedMethodParameters& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const ManagedMethodParameters& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "other: " << sample.other()<<", ";
        o << "hash: " << sample.hash()<<", ";
        o << "sample: " << sample.sample()<<", ";
        o << "keysOnly: " << sample.keysOnly() ;
        o <<"]";
        return o;
    }

    // ---- ArrayInitializationLocals: 

    ArrayInitializationLocals::ArrayInitializationLocals()  {
    }   

    ArrayInitializationLocals::ArrayInitializationLocals (const ::dds::core::array< CorpusNameCollisions::Item, 2L>& dimension0):
        m_dimension0_( dimension0 ) {
    }

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifdef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    ArrayInitializationLocals::ArrayInitializationLocals(ArrayInitializationLocals&& other_) OMG_NOEXCEPT  :m_dimension0_ (std::move(other_.m_dimension0_))
    {
    } 

    ArrayInitializationLocals& ArrayInitializationLocals::operator=(ArrayInitializationLocals&&  other_) OMG_NOEXCEPT {
        ArrayInitializationLocals tmp(std::move(other_));
        swap(tmp); 
        return *this;
    }
    #endif
    #endif   

    void ArrayInitializationLocals::swap(ArrayInitializationLocals& other_)  OMG_NOEXCEPT 
    {
        using std::swap;
        swap(m_dimension0_, other_.m_dimension0_);
    }  

    bool ArrayInitializationLocals::operator == (const ArrayInitializationLocals& other_) const {
        if (m_dimension0_ != other_.m_dimension0_) {
            return false;
        }
        return true;
    }
    bool ArrayInitializationLocals::operator != (const ArrayInitializationLocals& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const ArrayInitializationLocals& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "dimension0: " << sample.dimension0() ;
        o <<"]";
        return o;
    }

    // ---- NativeInheritanceStorage: 

    NativeInheritanceStorage::NativeInheritanceStorage() :
        m_baseValue_ (0)  {
    }   

    NativeInheritanceStorage::NativeInheritanceStorage (int32_t baseValue):
        m_baseValue_( baseValue ) {
    }

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifdef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    NativeInheritanceStorage::NativeInheritanceStorage(NativeInheritanceStorage&& other_) OMG_NOEXCEPT  :m_baseValue_ (std::move(other_.m_baseValue_))
    {
    } 

    NativeInheritanceStorage& NativeInheritanceStorage::operator=(NativeInheritanceStorage&&  other_) OMG_NOEXCEPT {
        NativeInheritanceStorage tmp(std::move(other_));
        swap(tmp); 
        return *this;
    }
    #endif
    #endif   

    void NativeInheritanceStorage::swap(NativeInheritanceStorage& other_)  OMG_NOEXCEPT 
    {
        using std::swap;
        swap(m_baseValue_, other_.m_baseValue_);
    }  

    bool NativeInheritanceStorage::operator == (const NativeInheritanceStorage& other_) const {
        if (m_baseValue_ != other_.m_baseValue_) {
            return false;
        }
        return true;
    }
    bool NativeInheritanceStorage::operator != (const NativeInheritanceStorage& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const NativeInheritanceStorage& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "baseValue: " << sample.baseValue() ;
        o <<"]";
        return o;
    }

    // ---- Derived: 

    Derived::Derived() :
        m_parent_ (0) ,
        m_value_ (0)  {
    }   

    Derived::Derived (int32_t baseValue,int32_t parent,int32_t value) :
        CorpusNameCollisions::NativeInheritanceStorage(baseValue ),
        m_parent_( parent ),
        m_value_( value ) {
    }

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifdef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    Derived::Derived(Derived&& other_) OMG_NOEXCEPT  : 
    CorpusNameCollisions::NativeInheritanceStorage(std::move(other_)),
    m_parent_ (std::move(other_.m_parent_))
    ,
    m_value_ (std::move(other_.m_value_))
    {
    } 

    Derived& Derived::operator=(Derived&&  other_) OMG_NOEXCEPT {
        Derived tmp(std::move(other_));
        swap(tmp); 
        return *this;
    }
    #endif
    #endif   

    void Derived::swap(Derived& other_)  OMG_NOEXCEPT 
    {
        using std::swap;
        CorpusNameCollisions::NativeInheritanceStorage::swap(other_);
        swap(m_parent_, other_.m_parent_);
        swap(m_value_, other_.m_value_);
    }  

    bool Derived::operator == (const Derived& other_) const {
        if (!CorpusNameCollisions::NativeInheritanceStorage::operator == (other_)){
            return false;
        }
        if (m_parent_ != other_.m_parent_) {
            return false;
        }
        if (m_value_ != other_.m_value_) {
            return false;
        }
        return true;
    }
    bool Derived::operator != (const Derived& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Derived& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << static_cast<const CorpusNameCollisions::NativeInheritanceStorage &>(sample);
        o << "parent: " << sample.parent()<<", ";
        o << "value: " << sample.value() ;
        o <<"]";
        return o;
    }

    // ---- NativeLifecycleParameters: 

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifdef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    NativeLifecycleParameters::NativeLifecycleParameters(NativeLifecycleParameters&& other_) OMG_NOEXCEPT 
    {
        _d(std::move(other_._d()));
        switch(::rti::topic::cdr::integer_case(_d())){
            case 1:
            {  
                optionalsOnly( std::move(other_.optionalsOnly()));
            } break;
            case 2:
            {  
                allocatePointers( std::move(other_.allocatePointers()));
            } break;
            case 3:
            {  
                allocateMemory( std::move(other_.allocateMemory()));
            } break;
            default:
            {  
                flag( std::move(other_.flag()));
            } ;

        }
    }

    NativeLifecycleParameters& NativeLifecycleParameters::operator=(NativeLifecycleParameters&&  other_) OMG_NOEXCEPT {
        NativeLifecycleParameters tmp(std::move(other_));
        swap(tmp); 
        return *this;
    }
    #endif
    #endif 

    NativeLifecycleParameters::Union_::Union_() :
        m_optionalsOnly_ ("") ,
        m_flag_ (0)  {
    }

    NativeLifecycleParameters::Union_::Union_(const std::string& optionalsOnly,const CorpusNameCollisions::Item& allocatePointers,const ::rti::core::bounded_sequence< int32_t, 2L >& allocateMemory,bool flag):
        m_optionalsOnly_( optionalsOnly ),
        m_allocatePointers_( allocatePointers ),
        m_allocateMemory_( allocateMemory ),
        m_flag_( flag ) {
    }

    NativeLifecycleParameters::NativeLifecycleParameters() :m_d_(default_discriminator())
    {
    }

    void NativeLifecycleParameters::swap(NativeLifecycleParameters& other_)  OMG_NOEXCEPT 
    {
        using std::swap;
        swap (m_d_,other_.m_d_);
        switch(::rti::topic::cdr::integer_case(_d())){
            case 1:
            {  
                swap(m_u_.m_optionalsOnly_, other_.m_u_.m_optionalsOnly_);
            } break;
            case 2:
            {  
                swap(m_u_.m_allocatePointers_, other_.m_u_.m_allocatePointers_);
            } break;
            case 3:
            {  
                swap(m_u_.m_allocateMemory_, other_.m_u_.m_allocateMemory_);
            } break;
            default:
            {  
                swap(m_u_.m_flag_, other_.m_u_.m_flag_);
            } ;

        }
        if (_d() != other_._d()){
            switch(::rti::topic::cdr::integer_case(other_._d())){
                case 1:
                {  
                    swap(m_u_.m_optionalsOnly_, other_.m_u_.m_optionalsOnly_);
                } break;
                case 2:
                {  
                    swap(m_u_.m_allocatePointers_, other_.m_u_.m_allocatePointers_);
                } break;
                case 3:
                {  
                    swap(m_u_.m_allocateMemory_, other_.m_u_.m_allocateMemory_);
                } break;
                default:
                {  
                    swap(m_u_.m_flag_, other_.m_u_.m_flag_);
                } ;

            }
        }
    }  

    bool NativeLifecycleParameters::operator == (const NativeLifecycleParameters& other_) const {
        if (_d() != other_._d()){
            return false;
        }
        switch(::rti::topic::cdr::integer_case(_d())){
            case 1:
            {  
                if ( m_u_.m_optionalsOnly_ != other_.m_u_.m_optionalsOnly_) {
                    return false;
                }
            } break ;
            case 2:
            {  
                if ( m_u_.m_allocatePointers_ != other_.m_u_.m_allocatePointers_) {
                    return false;
                }
            } break ;
            case 3:
            {  
                if ( m_u_.m_allocateMemory_ != other_.m_u_.m_allocateMemory_) {
                    return false;
                }
            } break ;
            default:
            {  
                if ( m_u_.m_flag_ != other_.m_u_.m_flag_) {
                    return false;
                }
            } ;
        }
        return true;
    }
    bool NativeLifecycleParameters::operator != (const NativeLifecycleParameters& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const NativeLifecycleParameters& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "_d: " << sample._d() <<", ";
        switch(::rti::topic::cdr::integer_case(sample._d())){
            case 1:
            {  
                o << "optionalsOnly: " << sample.optionalsOnly()<<", ";
            } break ;
            case 2:
            {  
                o << "allocatePointers: " << sample.allocatePointers()<<", ";
            } break ;
            case 3:
            {  
                o << "allocateMemory: " << sample.allocateMemory()<<", ";
            } break ;
            default:
            {  
                o << "flag: " << sample.flag() ;        } ;

        }
        o <<"]";
        return o;
    }

    int32_t NativeLifecycleParameters::default_discriminator() {
        return 0;
    }  

    // ---- NativeDiscriminatorStorage: 

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifdef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    NativeDiscriminatorStorage::NativeDiscriminatorStorage(NativeDiscriminatorStorage&& other_) OMG_NOEXCEPT 
    {
        _d(std::move(other_._d()));
        switch(::rti::topic::cdr::integer_case(_d())){
            case 1:
            {  
                _discriminator( std::move(other_._discriminator()));
            } break;
            default:
            {  
                flag( std::move(other_.flag()));
            } ;

        }
    }

    NativeDiscriminatorStorage& NativeDiscriminatorStorage::operator=(NativeDiscriminatorStorage&&  other_) OMG_NOEXCEPT {
        NativeDiscriminatorStorage tmp(std::move(other_));
        swap(tmp); 
        return *this;
    }
    #endif
    #endif 

    NativeDiscriminatorStorage::Union_::Union_() :
        m__discriminator_ (0) ,
        m_flag_ (0)  {
    }

    NativeDiscriminatorStorage::Union_::Union_(int32_t _discriminator,bool flag):
        m__discriminator_( _discriminator ),
        m_flag_( flag ) {
    }

    NativeDiscriminatorStorage::NativeDiscriminatorStorage() :m_d_(default_discriminator())
    {
    }

    void NativeDiscriminatorStorage::swap(NativeDiscriminatorStorage& other_)  OMG_NOEXCEPT 
    {
        using std::swap;
        swap (m_d_,other_.m_d_);
        switch(::rti::topic::cdr::integer_case(_d())){
            case 1:
            {  
                swap(m_u_.m__discriminator_, other_.m_u_.m__discriminator_);
            } break;
            default:
            {  
                swap(m_u_.m_flag_, other_.m_u_.m_flag_);
            } ;

        }
        if (_d() != other_._d()){
            switch(::rti::topic::cdr::integer_case(other_._d())){
                case 1:
                {  
                    swap(m_u_.m__discriminator_, other_.m_u_.m__discriminator_);
                } break;
                default:
                {  
                    swap(m_u_.m_flag_, other_.m_u_.m_flag_);
                } ;

            }
        }
    }  

    bool NativeDiscriminatorStorage::operator == (const NativeDiscriminatorStorage& other_) const {
        if (_d() != other_._d()){
            return false;
        }
        switch(::rti::topic::cdr::integer_case(_d())){
            case 1:
            {  
                if ( m_u_.m__discriminator_ != other_.m_u_.m__discriminator_) {
                    return false;
                }
            } break ;
            default:
            {  
                if ( m_u_.m_flag_ != other_.m_u_.m_flag_) {
                    return false;
                }
            } ;
        }
        return true;
    }
    bool NativeDiscriminatorStorage::operator != (const NativeDiscriminatorStorage& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const NativeDiscriminatorStorage& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "_d: " << sample._d() <<", ";
        switch(::rti::topic::cdr::integer_case(sample._d())){
            case 1:
            {  
                o << "_discriminator: " << sample._discriminator()<<", ";
            } break ;
            default:
            {  
                o << "flag: " << sample.flag() ;        } ;

        }
        o <<"]";
        return o;
    }

    int32_t NativeDiscriminatorStorage::default_discriminator() {
        return 0;
    }  

    // ---- ManagedDiscriminatorStorage: 

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifdef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    ManagedDiscriminatorStorage::ManagedDiscriminatorStorage(ManagedDiscriminatorStorage&& other_) OMG_NOEXCEPT 
    {
        _d(std::move(other_._d()));
        switch(::rti::topic::cdr::integer_case(_d())){
            case 1:
            {  
                Discriminator( std::move(other_.Discriminator()));
            } break;
            default:
            {  
                flag( std::move(other_.flag()));
            } ;

        }
    }

    ManagedDiscriminatorStorage& ManagedDiscriminatorStorage::operator=(ManagedDiscriminatorStorage&&  other_) OMG_NOEXCEPT {
        ManagedDiscriminatorStorage tmp(std::move(other_));
        swap(tmp); 
        return *this;
    }
    #endif
    #endif 

    ManagedDiscriminatorStorage::Union_::Union_() :
        m_Discriminator_ (0) ,
        m_flag_ (0)  {
    }

    ManagedDiscriminatorStorage::Union_::Union_(int32_t Discriminator,bool flag):
        m_Discriminator_( Discriminator ),
        m_flag_( flag ) {
    }

    ManagedDiscriminatorStorage::ManagedDiscriminatorStorage() :m_d_(default_discriminator())
    {
    }

    void ManagedDiscriminatorStorage::swap(ManagedDiscriminatorStorage& other_)  OMG_NOEXCEPT 
    {
        using std::swap;
        swap (m_d_,other_.m_d_);
        switch(::rti::topic::cdr::integer_case(_d())){
            case 1:
            {  
                swap(m_u_.m_Discriminator_, other_.m_u_.m_Discriminator_);
            } break;
            default:
            {  
                swap(m_u_.m_flag_, other_.m_u_.m_flag_);
            } ;

        }
        if (_d() != other_._d()){
            switch(::rti::topic::cdr::integer_case(other_._d())){
                case 1:
                {  
                    swap(m_u_.m_Discriminator_, other_.m_u_.m_Discriminator_);
                } break;
                default:
                {  
                    swap(m_u_.m_flag_, other_.m_u_.m_flag_);
                } ;

            }
        }
    }  

    bool ManagedDiscriminatorStorage::operator == (const ManagedDiscriminatorStorage& other_) const {
        if (_d() != other_._d()){
            return false;
        }
        switch(::rti::topic::cdr::integer_case(_d())){
            case 1:
            {  
                if ( m_u_.m_Discriminator_ != other_.m_u_.m_Discriminator_) {
                    return false;
                }
            } break ;
            default:
            {  
                if ( m_u_.m_flag_ != other_.m_u_.m_flag_) {
                    return false;
                }
            } ;
        }
        return true;
    }
    bool ManagedDiscriminatorStorage::operator != (const ManagedDiscriminatorStorage& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const ManagedDiscriminatorStorage& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "_d: " << sample._d() <<", ";
        switch(::rti::topic::cdr::integer_case(sample._d())){
            case 1:
            {  
                o << "Discriminator: " << sample.Discriminator()<<", ";
            } break ;
            default:
            {  
                o << "flag: " << sample.flag() ;        } ;

        }
        o <<"]";
        return o;
    }

    int32_t ManagedDiscriminatorStorage::default_discriminator() {
        return 0;
    }  

} // namespace CorpusNameCollisions  

// --- Type traits: -------------------------------------------------

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct native_type_code< CorpusNameCollisions::Item > {
            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static RTIBool is_initialized = RTI_FALSE;

                static DDS_TypeCode_Member Item_g_tc_members[1]=
                {

                    {
                        (char *)"value",/* Member name */
                        {
                            0,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Item_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusNameCollisions::Item", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        Item_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Item*/

                if (is_initialized) {
                    return &Item_g_tc;
                }

                Item_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Item_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Item_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Item_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Item_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Item_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Item_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Item_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Item_g_tc._data._sampleAccessInfo = sample_access_info();
                Item_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized = RTI_TRUE;

                return &Item_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static RTIBool is_initialized = RTI_FALSE;

                CorpusNameCollisions::Item *sample;

                static RTIXCdrMemberAccessInfo Item_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Item_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized) {
                    return (RTIXCdrSampleAccessInfo*) &Item_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    CorpusNameCollisions::Item);
                if (sample == NULL) {
                    return NULL;
                }

                Item_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->value() - (char *)sample);

                Item_g_sampleAccessInfo.memberAccessInfos = 
                Item_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(CorpusNameCollisions::Item);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Item_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Item_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Item_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Item_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< CorpusNameCollisions::Item >;

                Item_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized = RTI_TRUE;
                return (RTIXCdrSampleAccessInfo*) &Item_g_sampleAccessInfo;
            }

            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Item_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &Item_g_typePlugin;
            }
        }; // native_type_code
        #endif

        const ::dds::core::xtypes::StructType& dynamic_type< CorpusNameCollisions::Item >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< CorpusNameCollisions::Item >::get())));
        }

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct native_type_code< CorpusNameCollisions::LifecycleParameters > {
            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static RTIBool is_initialized = RTI_FALSE;

                static DDS_TypeCode LifecycleParameters_g_tc_optionalsOnly_string;
                static DDS_TypeCode LifecycleParameters_g_tc_allocateMemory_sequence;

                static DDS_TypeCode_Member LifecycleParameters_g_tc_members[3]=
                {

                    {
                        (char *)"optionalsOnly",/* Member name */
                        {
                            0,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"allocatePointers",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"allocateMemory",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode LifecycleParameters_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusNameCollisions::LifecycleParameters", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        3, /* Number of members */
                        LifecycleParameters_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for LifecycleParameters*/

                if (is_initialized) {
                    return &LifecycleParameters_g_tc;
                }

                LifecycleParameters_g_tc_optionalsOnly_string = initialize_string_typecode((16L));
                LifecycleParameters_g_tc_allocateMemory_sequence = initialize_sequence_typecode< ::rti::core::bounded_sequence< int32_t, 2L > >((2L));

                LifecycleParameters_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                LifecycleParameters_g_tc_allocateMemory_sequence._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                LifecycleParameters_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&LifecycleParameters_g_tc_optionalsOnly_string;
                LifecycleParameters_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< CorpusNameCollisions::Item>::get().native();
                LifecycleParameters_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)& LifecycleParameters_g_tc_allocateMemory_sequence;

                /* Initialize the values for member annotations. */
                LifecycleParameters_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                LifecycleParameters_g_tc_members[0]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                LifecycleParameters_g_tc._data._sampleAccessInfo = sample_access_info();
                LifecycleParameters_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized = RTI_TRUE;

                return &LifecycleParameters_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static RTIBool is_initialized = RTI_FALSE;

                CorpusNameCollisions::LifecycleParameters *sample;

                static RTIXCdrMemberAccessInfo LifecycleParameters_g_memberAccessInfos[3] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo LifecycleParameters_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized) {
                    return (RTIXCdrSampleAccessInfo*) &LifecycleParameters_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    CorpusNameCollisions::LifecycleParameters);
                if (sample == NULL) {
                    return NULL;
                }

                LifecycleParameters_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->optionalsOnly() - (char *)sample);

                LifecycleParameters_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->allocatePointers() - (char *)sample);

                LifecycleParameters_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->allocateMemory() - (char *)sample);

                LifecycleParameters_g_sampleAccessInfo.memberAccessInfos = 
                LifecycleParameters_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(CorpusNameCollisions::LifecycleParameters);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        LifecycleParameters_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        LifecycleParameters_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                LifecycleParameters_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                LifecycleParameters_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< CorpusNameCollisions::LifecycleParameters >;

                LifecycleParameters_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized = RTI_TRUE;
                return (RTIXCdrSampleAccessInfo*) &LifecycleParameters_g_sampleAccessInfo;
            }

            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin LifecycleParameters_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &LifecycleParameters_g_typePlugin;
            }
        }; // native_type_code
        #endif

        const ::dds::core::xtypes::StructType& dynamic_type< CorpusNameCollisions::LifecycleParameters >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< CorpusNameCollisions::LifecycleParameters >::get())));
        }

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct native_type_code< CorpusNameCollisions::ManagedMethodParameters > {
            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static RTIBool is_initialized = RTI_FALSE;

                static DDS_TypeCode_Member ManagedMethodParameters_g_tc_members[4]=
                {

                    {
                        (char *)"other",/* Member name */
                        {
                            0,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"hash",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"sample",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"keysOnly",/* Member name */
                        {
                            3,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode ManagedMethodParameters_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusNameCollisions::ManagedMethodParameters", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        4, /* Number of members */
                        ManagedMethodParameters_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for ManagedMethodParameters*/

                if (is_initialized) {
                    return &ManagedMethodParameters_g_tc;
                }

                ManagedMethodParameters_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                ManagedMethodParameters_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                ManagedMethodParameters_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                ManagedMethodParameters_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                ManagedMethodParameters_g_tc_members[3]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                ManagedMethodParameters_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                ManagedMethodParameters_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                ManagedMethodParameters_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                ManagedMethodParameters_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                ManagedMethodParameters_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                ManagedMethodParameters_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                ManagedMethodParameters_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                ManagedMethodParameters_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                ManagedMethodParameters_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                ManagedMethodParameters_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                ManagedMethodParameters_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                ManagedMethodParameters_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                ManagedMethodParameters_g_tc_members[2]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                ManagedMethodParameters_g_tc_members[2]._annotations._defaultValue._u.long_value = 0;
                ManagedMethodParameters_g_tc_members[2]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                ManagedMethodParameters_g_tc_members[2]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                ManagedMethodParameters_g_tc_members[2]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                ManagedMethodParameters_g_tc_members[2]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                ManagedMethodParameters_g_tc_members[3]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                ManagedMethodParameters_g_tc_members[3]._annotations._defaultValue._u.long_value = 0;
                ManagedMethodParameters_g_tc_members[3]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                ManagedMethodParameters_g_tc_members[3]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                ManagedMethodParameters_g_tc_members[3]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                ManagedMethodParameters_g_tc_members[3]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                ManagedMethodParameters_g_tc._data._sampleAccessInfo = sample_access_info();
                ManagedMethodParameters_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized = RTI_TRUE;

                return &ManagedMethodParameters_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static RTIBool is_initialized = RTI_FALSE;

                CorpusNameCollisions::ManagedMethodParameters *sample;

                static RTIXCdrMemberAccessInfo ManagedMethodParameters_g_memberAccessInfos[4] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo ManagedMethodParameters_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized) {
                    return (RTIXCdrSampleAccessInfo*) &ManagedMethodParameters_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    CorpusNameCollisions::ManagedMethodParameters);
                if (sample == NULL) {
                    return NULL;
                }

                ManagedMethodParameters_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->other() - (char *)sample);

                ManagedMethodParameters_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->hash() - (char *)sample);

                ManagedMethodParameters_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->sample() - (char *)sample);

                ManagedMethodParameters_g_memberAccessInfos[3].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->keysOnly() - (char *)sample);

                ManagedMethodParameters_g_sampleAccessInfo.memberAccessInfos = 
                ManagedMethodParameters_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(CorpusNameCollisions::ManagedMethodParameters);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        ManagedMethodParameters_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        ManagedMethodParameters_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                ManagedMethodParameters_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                ManagedMethodParameters_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< CorpusNameCollisions::ManagedMethodParameters >;

                ManagedMethodParameters_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized = RTI_TRUE;
                return (RTIXCdrSampleAccessInfo*) &ManagedMethodParameters_g_sampleAccessInfo;
            }

            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin ManagedMethodParameters_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &ManagedMethodParameters_g_typePlugin;
            }
        }; // native_type_code
        #endif

        const ::dds::core::xtypes::StructType& dynamic_type< CorpusNameCollisions::ManagedMethodParameters >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< CorpusNameCollisions::ManagedMethodParameters >::get())));
        }

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct native_type_code< CorpusNameCollisions::ArrayInitializationLocals > {
            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static RTIBool is_initialized = RTI_FALSE;

                static DDS_TypeCode ArrayInitializationLocals_g_tc_dimension0_array =DDS_INITIALIZE_ARRAY_TYPECODE(1,2L, NULL,NULL);

                static DDS_TypeCode_Member ArrayInitializationLocals_g_tc_members[1]=
                {

                    {
                        (char *)"dimension0",/* Member name */
                        {
                            0,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode ArrayInitializationLocals_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusNameCollisions::ArrayInitializationLocals", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        ArrayInitializationLocals_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for ArrayInitializationLocals*/

                if (is_initialized) {
                    return &ArrayInitializationLocals_g_tc;
                }

                ArrayInitializationLocals_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                ArrayInitializationLocals_g_tc_dimension0_array._data._typeCode =(RTICdrTypeCode *)&::rti::topic::dynamic_type< CorpusNameCollisions::Item>::get().native();
                ArrayInitializationLocals_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)& ArrayInitializationLocals_g_tc_dimension0_array;

                /* Initialize the values for member annotations. */

                ArrayInitializationLocals_g_tc._data._sampleAccessInfo = sample_access_info();
                ArrayInitializationLocals_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized = RTI_TRUE;

                return &ArrayInitializationLocals_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static RTIBool is_initialized = RTI_FALSE;

                CorpusNameCollisions::ArrayInitializationLocals *sample;

                static RTIXCdrMemberAccessInfo ArrayInitializationLocals_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo ArrayInitializationLocals_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized) {
                    return (RTIXCdrSampleAccessInfo*) &ArrayInitializationLocals_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    CorpusNameCollisions::ArrayInitializationLocals);
                if (sample == NULL) {
                    return NULL;
                }

                ArrayInitializationLocals_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->dimension0() - (char *)sample);

                ArrayInitializationLocals_g_sampleAccessInfo.memberAccessInfos = 
                ArrayInitializationLocals_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(CorpusNameCollisions::ArrayInitializationLocals);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        ArrayInitializationLocals_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        ArrayInitializationLocals_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                ArrayInitializationLocals_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                ArrayInitializationLocals_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< CorpusNameCollisions::ArrayInitializationLocals >;

                ArrayInitializationLocals_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized = RTI_TRUE;
                return (RTIXCdrSampleAccessInfo*) &ArrayInitializationLocals_g_sampleAccessInfo;
            }

            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin ArrayInitializationLocals_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &ArrayInitializationLocals_g_typePlugin;
            }
        }; // native_type_code
        #endif

        const ::dds::core::xtypes::StructType& dynamic_type< CorpusNameCollisions::ArrayInitializationLocals >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< CorpusNameCollisions::ArrayInitializationLocals >::get())));
        }

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct native_type_code< CorpusNameCollisions::NativeInheritanceStorage > {
            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static RTIBool is_initialized = RTI_FALSE;

                static DDS_TypeCode_Member NativeInheritanceStorage_g_tc_members[1]=
                {

                    {
                        (char *)"baseValue",/* Member name */
                        {
                            0,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode NativeInheritanceStorage_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusNameCollisions::NativeInheritanceStorage", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        NativeInheritanceStorage_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for NativeInheritanceStorage*/

                if (is_initialized) {
                    return &NativeInheritanceStorage_g_tc;
                }

                NativeInheritanceStorage_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                NativeInheritanceStorage_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                NativeInheritanceStorage_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                NativeInheritanceStorage_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                NativeInheritanceStorage_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                NativeInheritanceStorage_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                NativeInheritanceStorage_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                NativeInheritanceStorage_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                NativeInheritanceStorage_g_tc._data._sampleAccessInfo = sample_access_info();
                NativeInheritanceStorage_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized = RTI_TRUE;

                return &NativeInheritanceStorage_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static RTIBool is_initialized = RTI_FALSE;

                CorpusNameCollisions::NativeInheritanceStorage *sample;

                static RTIXCdrMemberAccessInfo NativeInheritanceStorage_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo NativeInheritanceStorage_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized) {
                    return (RTIXCdrSampleAccessInfo*) &NativeInheritanceStorage_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    CorpusNameCollisions::NativeInheritanceStorage);
                if (sample == NULL) {
                    return NULL;
                }

                NativeInheritanceStorage_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->baseValue() - (char *)sample);

                NativeInheritanceStorage_g_sampleAccessInfo.memberAccessInfos = 
                NativeInheritanceStorage_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(CorpusNameCollisions::NativeInheritanceStorage);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        NativeInheritanceStorage_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        NativeInheritanceStorage_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                NativeInheritanceStorage_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                NativeInheritanceStorage_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< CorpusNameCollisions::NativeInheritanceStorage >;

                NativeInheritanceStorage_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized = RTI_TRUE;
                return (RTIXCdrSampleAccessInfo*) &NativeInheritanceStorage_g_sampleAccessInfo;
            }

            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin NativeInheritanceStorage_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &NativeInheritanceStorage_g_typePlugin;
            }
        }; // native_type_code
        #endif

        const ::dds::core::xtypes::StructType& dynamic_type< CorpusNameCollisions::NativeInheritanceStorage >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< CorpusNameCollisions::NativeInheritanceStorage >::get())));
        }

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct native_type_code< CorpusNameCollisions::Derived > {
            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static RTIBool is_initialized = RTI_FALSE;

                static DDS_TypeCode_Member Derived_g_tc_members[2]=
                {

                    {
                        (char *)"parent",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"value",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Derived_g_tc =
                {{
                        DDS_TK_VALUE, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusNameCollisions::Derived", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Derived_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Derived*/

                if (is_initialized) {
                    return &Derived_g_tc;
                }

                Derived_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Derived_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Derived_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Derived_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Derived_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Derived_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Derived_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Derived_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Derived_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Derived_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Derived_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                Derived_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Derived_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Derived_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Derived_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Derived_g_tc._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< CorpusNameCollisions::NativeInheritanceStorage >::get().native(); /* Base class */

                Derived_g_tc._data._sampleAccessInfo = sample_access_info();
                Derived_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized = RTI_TRUE;

                return &Derived_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static RTIBool is_initialized = RTI_FALSE;

                CorpusNameCollisions::Derived *sample;

                static RTIXCdrMemberAccessInfo Derived_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Derived_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized) {
                    return (RTIXCdrSampleAccessInfo*) &Derived_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    CorpusNameCollisions::Derived);
                if (sample == NULL) {
                    return NULL;
                }

                Derived_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->parent() - (char *)sample);

                Derived_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->value() - (char *)sample);

                Derived_g_sampleAccessInfo.memberAccessInfos = 
                Derived_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(CorpusNameCollisions::Derived);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Derived_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Derived_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Derived_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Derived_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< CorpusNameCollisions::Derived >;

                Derived_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized = RTI_TRUE;
                return (RTIXCdrSampleAccessInfo*) &Derived_g_sampleAccessInfo;
            }

            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Derived_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &Derived_g_typePlugin;
            }
        }; // native_type_code
        #endif

        const ::dds::core::xtypes::StructType& dynamic_type< CorpusNameCollisions::Derived >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< CorpusNameCollisions::Derived >::get())));
        }

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct native_type_code< CorpusNameCollisions::NativeLifecycleParameters > {
            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static RTIBool is_initialized = RTI_FALSE;

                static DDS_TypeCode NativeLifecycleParameters_g_tc_optionalsOnly_string;
                static DDS_TypeCode NativeLifecycleParameters_g_tc_allocateMemory_sequence;

                static DDS_TypeCode_Member NativeLifecycleParameters_g_tc_members[4]=
                {

                    {
                        (char *)"optionalsOnly",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(1),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"allocatePointers",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(2),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"allocateMemory",/* Member name */
                        {
                            3,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(3),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"flag",/* Member name */
                        {
                            4,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        RTI_CDR_TYPE_CODE_UNION_DEFAULT_LABEL, /* First label */
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode NativeLifecycleParameters_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        3, /*Ignored*/
                        (char *)"CorpusNameCollisions::NativeLifecycleParameters", /* Name */
                        NULL,     /* Base class type code is assigned later */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        4, /* Number of members */
                        NativeLifecycleParameters_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for NativeLifecycleParameters*/

                if (is_initialized) {
                    return &NativeLifecycleParameters_g_tc;
                }

                NativeLifecycleParameters_g_tc_optionalsOnly_string = initialize_string_typecode((16L));
                NativeLifecycleParameters_g_tc_allocateMemory_sequence = initialize_sequence_typecode< ::rti::core::bounded_sequence< int32_t, 2L > >((2L));

                NativeLifecycleParameters_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                NativeLifecycleParameters_g_tc_allocateMemory_sequence._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                NativeLifecycleParameters_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&NativeLifecycleParameters_g_tc_optionalsOnly_string;
                NativeLifecycleParameters_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< CorpusNameCollisions::Item>::get().native();
                NativeLifecycleParameters_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)& NativeLifecycleParameters_g_tc_allocateMemory_sequence;
                NativeLifecycleParameters_g_tc_members[3]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::interpreter::initialize_bool_typecode();

                /* Initialize the values for member annotations. */
                NativeLifecycleParameters_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                NativeLifecycleParameters_g_tc_members[0]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                NativeLifecycleParameters_g_tc_members[3]._annotations._defaultValue._d = RTI_XCDR_TK_BOOLEAN;
                NativeLifecycleParameters_g_tc_members[3]._annotations._defaultValue._u.boolean_value = 0;

                /* Discriminator type code */
                NativeLifecycleParameters_g_tc._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                NativeLifecycleParameters_g_tc._data._sampleAccessInfo = sample_access_info();
                NativeLifecycleParameters_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized = RTI_TRUE;

                return &NativeLifecycleParameters_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static RTIBool is_initialized = RTI_FALSE;

                CorpusNameCollisions::NativeLifecycleParameters *sample;

                static RTIXCdrMemberAccessInfo NativeLifecycleParameters_g_memberAccessInfos[5] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo NativeLifecycleParameters_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized) {
                    return (RTIXCdrSampleAccessInfo*) &NativeLifecycleParameters_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    CorpusNameCollisions::NativeLifecycleParameters);
                if (sample == NULL) {
                    return NULL;
                }

                NativeLifecycleParameters_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d() = 1;
                NativeLifecycleParameters_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->optionalsOnly() - (char *)sample);

                sample->_d() = 2;
                NativeLifecycleParameters_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->allocatePointers() - (char *)sample);

                sample->_d() = 3;
                NativeLifecycleParameters_g_memberAccessInfos[3].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->allocateMemory() - (char *)sample);

                sample->_d() = CorpusNameCollisions::NativeLifecycleParameters::default_discriminator();
                NativeLifecycleParameters_g_memberAccessInfos[4].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->flag() - (char *)sample);

                NativeLifecycleParameters_g_sampleAccessInfo.memberAccessInfos = 
                NativeLifecycleParameters_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(CorpusNameCollisions::NativeLifecycleParameters);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        NativeLifecycleParameters_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        NativeLifecycleParameters_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                NativeLifecycleParameters_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                NativeLifecycleParameters_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< CorpusNameCollisions::NativeLifecycleParameters >;

                NativeLifecycleParameters_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized = RTI_TRUE;
                return (RTIXCdrSampleAccessInfo*) &NativeLifecycleParameters_g_sampleAccessInfo;
            }

            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin NativeLifecycleParameters_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &NativeLifecycleParameters_g_typePlugin;
            }
        }; // native_type_code
        #endif

        const ::dds::core::xtypes::UnionType& dynamic_type< CorpusNameCollisions::NativeLifecycleParameters >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< CorpusNameCollisions::NativeLifecycleParameters >::get())));
        }

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct native_type_code< CorpusNameCollisions::NativeDiscriminatorStorage > {
            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static RTIBool is_initialized = RTI_FALSE;

                static DDS_TypeCode_Member NativeDiscriminatorStorage_g_tc_members[2]=
                {

                    {
                        (char *)"_discriminator",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(1),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"flag",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        RTI_CDR_TYPE_CODE_UNION_DEFAULT_LABEL, /* First label */
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode NativeDiscriminatorStorage_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        1, /*Ignored*/
                        (char *)"CorpusNameCollisions::NativeDiscriminatorStorage", /* Name */
                        NULL,     /* Base class type code is assigned later */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        NativeDiscriminatorStorage_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for NativeDiscriminatorStorage*/

                if (is_initialized) {
                    return &NativeDiscriminatorStorage_g_tc;
                }

                NativeDiscriminatorStorage_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                NativeDiscriminatorStorage_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                NativeDiscriminatorStorage_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::interpreter::initialize_bool_typecode();

                /* Initialize the values for member annotations. */
                NativeDiscriminatorStorage_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                NativeDiscriminatorStorage_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                NativeDiscriminatorStorage_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                NativeDiscriminatorStorage_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                NativeDiscriminatorStorage_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                NativeDiscriminatorStorage_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                NativeDiscriminatorStorage_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_BOOLEAN;
                NativeDiscriminatorStorage_g_tc_members[1]._annotations._defaultValue._u.boolean_value = 0;

                /* Discriminator type code */
                NativeDiscriminatorStorage_g_tc._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                NativeDiscriminatorStorage_g_tc._data._sampleAccessInfo = sample_access_info();
                NativeDiscriminatorStorage_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized = RTI_TRUE;

                return &NativeDiscriminatorStorage_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static RTIBool is_initialized = RTI_FALSE;

                CorpusNameCollisions::NativeDiscriminatorStorage *sample;

                static RTIXCdrMemberAccessInfo NativeDiscriminatorStorage_g_memberAccessInfos[3] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo NativeDiscriminatorStorage_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized) {
                    return (RTIXCdrSampleAccessInfo*) &NativeDiscriminatorStorage_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    CorpusNameCollisions::NativeDiscriminatorStorage);
                if (sample == NULL) {
                    return NULL;
                }

                NativeDiscriminatorStorage_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d() = 1;
                NativeDiscriminatorStorage_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->_discriminator() - (char *)sample);

                sample->_d() = CorpusNameCollisions::NativeDiscriminatorStorage::default_discriminator();
                NativeDiscriminatorStorage_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->flag() - (char *)sample);

                NativeDiscriminatorStorage_g_sampleAccessInfo.memberAccessInfos = 
                NativeDiscriminatorStorage_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(CorpusNameCollisions::NativeDiscriminatorStorage);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        NativeDiscriminatorStorage_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        NativeDiscriminatorStorage_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                NativeDiscriminatorStorage_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                NativeDiscriminatorStorage_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< CorpusNameCollisions::NativeDiscriminatorStorage >;

                NativeDiscriminatorStorage_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized = RTI_TRUE;
                return (RTIXCdrSampleAccessInfo*) &NativeDiscriminatorStorage_g_sampleAccessInfo;
            }

            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin NativeDiscriminatorStorage_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &NativeDiscriminatorStorage_g_typePlugin;
            }
        }; // native_type_code
        #endif

        const ::dds::core::xtypes::UnionType& dynamic_type< CorpusNameCollisions::NativeDiscriminatorStorage >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< CorpusNameCollisions::NativeDiscriminatorStorage >::get())));
        }

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct native_type_code< CorpusNameCollisions::ManagedDiscriminatorStorage > {
            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static RTIBool is_initialized = RTI_FALSE;

                static DDS_TypeCode_Member ManagedDiscriminatorStorage_g_tc_members[2]=
                {

                    {
                        (char *)"Discriminator",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(1),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"flag",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        RTI_CDR_TYPE_CODE_UNION_DEFAULT_LABEL, /* First label */
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode ManagedDiscriminatorStorage_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        1, /*Ignored*/
                        (char *)"CorpusNameCollisions::ManagedDiscriminatorStorage", /* Name */
                        NULL,     /* Base class type code is assigned later */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        ManagedDiscriminatorStorage_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for ManagedDiscriminatorStorage*/

                if (is_initialized) {
                    return &ManagedDiscriminatorStorage_g_tc;
                }

                ManagedDiscriminatorStorage_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                ManagedDiscriminatorStorage_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                ManagedDiscriminatorStorage_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::interpreter::initialize_bool_typecode();

                /* Initialize the values for member annotations. */
                ManagedDiscriminatorStorage_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                ManagedDiscriminatorStorage_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                ManagedDiscriminatorStorage_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                ManagedDiscriminatorStorage_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                ManagedDiscriminatorStorage_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                ManagedDiscriminatorStorage_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                ManagedDiscriminatorStorage_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_BOOLEAN;
                ManagedDiscriminatorStorage_g_tc_members[1]._annotations._defaultValue._u.boolean_value = 0;

                /* Discriminator type code */
                ManagedDiscriminatorStorage_g_tc._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                ManagedDiscriminatorStorage_g_tc._data._sampleAccessInfo = sample_access_info();
                ManagedDiscriminatorStorage_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized = RTI_TRUE;

                return &ManagedDiscriminatorStorage_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static RTIBool is_initialized = RTI_FALSE;

                CorpusNameCollisions::ManagedDiscriminatorStorage *sample;

                static RTIXCdrMemberAccessInfo ManagedDiscriminatorStorage_g_memberAccessInfos[3] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo ManagedDiscriminatorStorage_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized) {
                    return (RTIXCdrSampleAccessInfo*) &ManagedDiscriminatorStorage_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    CorpusNameCollisions::ManagedDiscriminatorStorage);
                if (sample == NULL) {
                    return NULL;
                }

                ManagedDiscriminatorStorage_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d() = 1;
                ManagedDiscriminatorStorage_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->Discriminator() - (char *)sample);

                sample->_d() = CorpusNameCollisions::ManagedDiscriminatorStorage::default_discriminator();
                ManagedDiscriminatorStorage_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->flag() - (char *)sample);

                ManagedDiscriminatorStorage_g_sampleAccessInfo.memberAccessInfos = 
                ManagedDiscriminatorStorage_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(CorpusNameCollisions::ManagedDiscriminatorStorage);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        ManagedDiscriminatorStorage_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        ManagedDiscriminatorStorage_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                ManagedDiscriminatorStorage_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                ManagedDiscriminatorStorage_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< CorpusNameCollisions::ManagedDiscriminatorStorage >;

                ManagedDiscriminatorStorage_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized = RTI_TRUE;
                return (RTIXCdrSampleAccessInfo*) &ManagedDiscriminatorStorage_g_sampleAccessInfo;
            }

            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin ManagedDiscriminatorStorage_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &ManagedDiscriminatorStorage_g_typePlugin;
            }
        }; // native_type_code
        #endif

        const ::dds::core::xtypes::UnionType& dynamic_type< CorpusNameCollisions::ManagedDiscriminatorStorage >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< CorpusNameCollisions::ManagedDiscriminatorStorage >::get())));
        }

    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< CorpusNameCollisions::Item >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                CorpusNameCollisions::ItemPlugin_new,
                CorpusNameCollisions::ItemPlugin_delete);
        }

        std::vector<char>& topic_type_support< CorpusNameCollisions::Item >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const CorpusNameCollisions::Item& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = ItemPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = ItemPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< CorpusNameCollisions::Item >::from_cdr_buffer(CorpusNameCollisions::Item& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ItemPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create CorpusNameCollisions::Item from cdr buffer");
        }

        void topic_type_support< CorpusNameCollisions::Item >::reset_sample(CorpusNameCollisions::Item& sample) 
        {
            sample.value(0);
        }

        void topic_type_support< CorpusNameCollisions::Item >::allocate_sample(CorpusNameCollisions::Item& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }

        void topic_type_support< CorpusNameCollisions::LifecycleParameters >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                CorpusNameCollisions::LifecycleParametersPlugin_new,
                CorpusNameCollisions::LifecycleParametersPlugin_delete);
        }

        std::vector<char>& topic_type_support< CorpusNameCollisions::LifecycleParameters >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const CorpusNameCollisions::LifecycleParameters& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = LifecycleParametersPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = LifecycleParametersPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< CorpusNameCollisions::LifecycleParameters >::from_cdr_buffer(CorpusNameCollisions::LifecycleParameters& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = LifecycleParametersPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create CorpusNameCollisions::LifecycleParameters from cdr buffer");
        }

        void topic_type_support< CorpusNameCollisions::LifecycleParameters >::reset_sample(CorpusNameCollisions::LifecycleParameters& sample) 
        {
            sample.optionalsOnly("");
            ::rti::topic::reset_sample(sample.allocatePointers());
            ::rti::topic::reset_sample(sample.allocateMemory());
        }

        void topic_type_support< CorpusNameCollisions::LifecycleParameters >::allocate_sample(CorpusNameCollisions::LifecycleParameters& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.optionalsOnly(),  -1, 16L);
            ::rti::topic::allocate_sample(sample.allocatePointers(),  -1, -1);
            ::rti::topic::allocate_sample(sample.allocateMemory(),  2L, -1);
        }

        void topic_type_support< CorpusNameCollisions::ManagedMethodParameters >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                CorpusNameCollisions::ManagedMethodParametersPlugin_new,
                CorpusNameCollisions::ManagedMethodParametersPlugin_delete);
        }

        std::vector<char>& topic_type_support< CorpusNameCollisions::ManagedMethodParameters >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const CorpusNameCollisions::ManagedMethodParameters& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = ManagedMethodParametersPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = ManagedMethodParametersPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< CorpusNameCollisions::ManagedMethodParameters >::from_cdr_buffer(CorpusNameCollisions::ManagedMethodParameters& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ManagedMethodParametersPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create CorpusNameCollisions::ManagedMethodParameters from cdr buffer");
        }

        void topic_type_support< CorpusNameCollisions::ManagedMethodParameters >::reset_sample(CorpusNameCollisions::ManagedMethodParameters& sample) 
        {
            sample.other(0);
            sample.hash(0);
            sample.sample(0);
            sample.keysOnly(0);
        }

        void topic_type_support< CorpusNameCollisions::ManagedMethodParameters >::allocate_sample(CorpusNameCollisions::ManagedMethodParameters& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }

        void topic_type_support< CorpusNameCollisions::ArrayInitializationLocals >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                CorpusNameCollisions::ArrayInitializationLocalsPlugin_new,
                CorpusNameCollisions::ArrayInitializationLocalsPlugin_delete);
        }

        std::vector<char>& topic_type_support< CorpusNameCollisions::ArrayInitializationLocals >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const CorpusNameCollisions::ArrayInitializationLocals& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = ArrayInitializationLocalsPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = ArrayInitializationLocalsPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< CorpusNameCollisions::ArrayInitializationLocals >::from_cdr_buffer(CorpusNameCollisions::ArrayInitializationLocals& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ArrayInitializationLocalsPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create CorpusNameCollisions::ArrayInitializationLocals from cdr buffer");
        }

        void topic_type_support< CorpusNameCollisions::ArrayInitializationLocals >::reset_sample(CorpusNameCollisions::ArrayInitializationLocals& sample) 
        {
            ::rti::topic::reset_sample(sample.dimension0());
        }

        void topic_type_support< CorpusNameCollisions::ArrayInitializationLocals >::allocate_sample(CorpusNameCollisions::ArrayInitializationLocals& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.dimension0(),  -1, -1);
        }

        void topic_type_support< CorpusNameCollisions::NativeInheritanceStorage >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                CorpusNameCollisions::NativeInheritanceStoragePlugin_new,
                CorpusNameCollisions::NativeInheritanceStoragePlugin_delete);
        }

        std::vector<char>& topic_type_support< CorpusNameCollisions::NativeInheritanceStorage >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const CorpusNameCollisions::NativeInheritanceStorage& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = NativeInheritanceStoragePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = NativeInheritanceStoragePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< CorpusNameCollisions::NativeInheritanceStorage >::from_cdr_buffer(CorpusNameCollisions::NativeInheritanceStorage& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = NativeInheritanceStoragePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create CorpusNameCollisions::NativeInheritanceStorage from cdr buffer");
        }

        void topic_type_support< CorpusNameCollisions::NativeInheritanceStorage >::reset_sample(CorpusNameCollisions::NativeInheritanceStorage& sample) 
        {
            sample.baseValue(0);
        }

        void topic_type_support< CorpusNameCollisions::NativeInheritanceStorage >::allocate_sample(CorpusNameCollisions::NativeInheritanceStorage& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }

        void topic_type_support< CorpusNameCollisions::Derived >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                CorpusNameCollisions::DerivedPlugin_new,
                CorpusNameCollisions::DerivedPlugin_delete);
        }

        std::vector<char>& topic_type_support< CorpusNameCollisions::Derived >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const CorpusNameCollisions::Derived& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = DerivedPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = DerivedPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< CorpusNameCollisions::Derived >::from_cdr_buffer(CorpusNameCollisions::Derived& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = DerivedPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create CorpusNameCollisions::Derived from cdr buffer");
        }

        void topic_type_support< CorpusNameCollisions::Derived >::reset_sample(CorpusNameCollisions::Derived& sample) 
        {
            // Initialize base
            topic_type_support< CorpusNameCollisions::NativeInheritanceStorage >::reset_sample(sample);

            sample.parent(0);
            sample.value(0);
        }

        void topic_type_support< CorpusNameCollisions::Derived >::allocate_sample(CorpusNameCollisions::Derived& sample, int, int) 
        {
            // Initialize base
            topic_type_support< CorpusNameCollisions::NativeInheritanceStorage >::allocate_sample(sample, -1, -1);

        }

        void topic_type_support< CorpusNameCollisions::NativeLifecycleParameters >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                CorpusNameCollisions::NativeLifecycleParametersPlugin_new,
                CorpusNameCollisions::NativeLifecycleParametersPlugin_delete);
        }

        std::vector<char>& topic_type_support< CorpusNameCollisions::NativeLifecycleParameters >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const CorpusNameCollisions::NativeLifecycleParameters& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = NativeLifecycleParametersPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = NativeLifecycleParametersPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< CorpusNameCollisions::NativeLifecycleParameters >::from_cdr_buffer(CorpusNameCollisions::NativeLifecycleParameters& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = NativeLifecycleParametersPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create CorpusNameCollisions::NativeLifecycleParameters from cdr buffer");
        }

        void topic_type_support< CorpusNameCollisions::NativeLifecycleParameters >::reset_sample(CorpusNameCollisions::NativeLifecycleParameters& sample) 
        {
            sample._d() = 1;
            sample.optionalsOnly("");
            sample._d() = 2;
            ::rti::topic::reset_sample(sample.allocatePointers());
            sample._d() = 3;
            ::rti::topic::reset_sample(sample.allocateMemory());
            sample._d() = CorpusNameCollisions::NativeLifecycleParameters::default_discriminator();
            sample.flag(0);

            sample._d() = CorpusNameCollisions::NativeLifecycleParameters::default_discriminator();
        }

        void topic_type_support< CorpusNameCollisions::NativeLifecycleParameters >::allocate_sample(CorpusNameCollisions::NativeLifecycleParameters& sample, int, int) 
        {
            sample._d() = 1;
            ::rti::topic::allocate_sample(sample.optionalsOnly(),  -1, 16L);
            sample._d() = 2;
            ::rti::topic::allocate_sample(sample.allocatePointers(),  -1, -1);
            sample._d() = 3;
            ::rti::topic::allocate_sample(sample.allocateMemory(),  2L, -1);

            sample._d() = CorpusNameCollisions::NativeLifecycleParameters::default_discriminator();
        }

        void topic_type_support< CorpusNameCollisions::NativeDiscriminatorStorage >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                CorpusNameCollisions::NativeDiscriminatorStoragePlugin_new,
                CorpusNameCollisions::NativeDiscriminatorStoragePlugin_delete);
        }

        std::vector<char>& topic_type_support< CorpusNameCollisions::NativeDiscriminatorStorage >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const CorpusNameCollisions::NativeDiscriminatorStorage& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = NativeDiscriminatorStoragePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = NativeDiscriminatorStoragePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< CorpusNameCollisions::NativeDiscriminatorStorage >::from_cdr_buffer(CorpusNameCollisions::NativeDiscriminatorStorage& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = NativeDiscriminatorStoragePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create CorpusNameCollisions::NativeDiscriminatorStorage from cdr buffer");
        }

        void topic_type_support< CorpusNameCollisions::NativeDiscriminatorStorage >::reset_sample(CorpusNameCollisions::NativeDiscriminatorStorage& sample) 
        {
            sample._d() = 1;
            sample._discriminator(0);
            sample._d() = CorpusNameCollisions::NativeDiscriminatorStorage::default_discriminator();
            sample.flag(0);

            sample._d() = CorpusNameCollisions::NativeDiscriminatorStorage::default_discriminator();
        }

        void topic_type_support< CorpusNameCollisions::NativeDiscriminatorStorage >::allocate_sample(CorpusNameCollisions::NativeDiscriminatorStorage& sample, int, int) 
        {

            sample._d() = CorpusNameCollisions::NativeDiscriminatorStorage::default_discriminator();
        }

        void topic_type_support< CorpusNameCollisions::ManagedDiscriminatorStorage >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                CorpusNameCollisions::ManagedDiscriminatorStoragePlugin_new,
                CorpusNameCollisions::ManagedDiscriminatorStoragePlugin_delete);
        }

        std::vector<char>& topic_type_support< CorpusNameCollisions::ManagedDiscriminatorStorage >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const CorpusNameCollisions::ManagedDiscriminatorStorage& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = ManagedDiscriminatorStoragePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = ManagedDiscriminatorStoragePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< CorpusNameCollisions::ManagedDiscriminatorStorage >::from_cdr_buffer(CorpusNameCollisions::ManagedDiscriminatorStorage& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ManagedDiscriminatorStoragePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create CorpusNameCollisions::ManagedDiscriminatorStorage from cdr buffer");
        }

        void topic_type_support< CorpusNameCollisions::ManagedDiscriminatorStorage >::reset_sample(CorpusNameCollisions::ManagedDiscriminatorStorage& sample) 
        {
            sample._d() = 1;
            sample.Discriminator(0);
            sample._d() = CorpusNameCollisions::ManagedDiscriminatorStorage::default_discriminator();
            sample.flag(0);

            sample._d() = CorpusNameCollisions::ManagedDiscriminatorStorage::default_discriminator();
        }

        void topic_type_support< CorpusNameCollisions::ManagedDiscriminatorStorage >::allocate_sample(CorpusNameCollisions::ManagedDiscriminatorStorage& sample, int, int) 
        {

            sample._d() = CorpusNameCollisions::ManagedDiscriminatorStorage::default_discriminator();
        }

    }
}  

