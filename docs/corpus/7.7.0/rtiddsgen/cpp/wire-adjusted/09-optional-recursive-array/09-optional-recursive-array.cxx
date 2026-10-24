

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-optional-recursive-array.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#include <iosfwd>
#include <iomanip>
#include <atomic>

#ifndef NDDS_STANDALONE_TYPE
#include "rti/topic/cdr/Serialization.hpp"
#include "09-optional-recursive-arrayPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "09-optional-recursive-array.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusOptionalRecursiveArray {

    // ---- Node:

    Node::Node() :
        value (0)  {

    }

    Node::Node (int32_t value_,const ::omg::types::optional< ::dds::core::array< ::CorpusOptionalRecursiveArray::Node, 2L> >& children_) {
        value = value_;
        children = children_;
    }

    bool operator == (const Node& a, const Node& b) {

        if (a.value != b.value) {
            return false;
        }
        if (a.children != b.children) {
            return false;
        }

        return true;
    }

    bool operator != (const Node& a, const Node& b) {
        return !operator ==(a, b);
    }

    void swap(Node& a, Node& b) noexcept
    {
        using std::swap;

        swap(a.value, b.value);
        swap(a.children, b.children);

    }  
    std::ostream& operator << (std::ostream& o,const Node& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "value: " << sample.value<<", ";
        o << "children: " << sample.children;
        o <<"]";
        return o;
    }

} // namespace CorpusOptionalRecursiveArray  

#ifdef NDDS_STANDALONE_TYPE
namespace rti {
    namespace topic {
    }
}

#else
// --- Type traits: -------------------------------------------------

namespace rti { 
    namespace topic {

        template<>
        struct native_type_code< ::CorpusOptionalRecursiveArray::Node > {
            static DDS_TypeCode * get()
            {
                RTIXCdrResourceNode node = RTIXCdrResourceNode_INITIALIZER;
                return (DDS_TypeCode *) &::rti::topic::dynamic_type< ::CorpusOptionalRecursiveArray::Node>::get(&node).native();
            }

            static DDS_TypeCode * get(RTIXCdrResourceNode *parentVisitedNode)
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};
                RTIXCdrResourceNode node = RTIXCdrResourceNode_INITIALIZER;

                static DDS_TypeCode Node_g_tc_children_array =DDS_INITIALIZE_ARRAY_TYPECODE(1,2L, NULL,NULL);

                static DDS_TypeCode_Member Node_g_tc_members[2]=
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
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"children",/* Member name */
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
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Node_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusOptionalRecursiveArray::Node", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Node_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Node*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Node_g_tc;
                }

                /* Return if the type has already been visited stopping recursion */
                node.prev = parentVisitedNode;
                node.resource = &Node_g_tc;
                if (RTIXCdrTypeCode_isResourceVisited(node.resource, parentVisitedNode, NULL)) {
                    return &Node_g_tc;
                }

                Node_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Node_g_tc_children_array._data._typeCode =(RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusOptionalRecursiveArray::Node>::get(&node).native();
                Node_g_tc_children_array._data._sampleAccessInfo = interpreter::get_opt_array_sample_access_info< ::CorpusOptionalRecursiveArray::Node>();
                Node_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                Node_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)& Node_g_tc_children_array;

                /* Initialize the values for member annotations. */
                Node_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Node_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Node_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Node_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Node_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Node_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Node_g_tc._data._sampleAccessInfo = sample_access_info();
                Node_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Node_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusOptionalRecursiveArray::Node *sample;

                static RTIXCdrMemberAccessInfo Node_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Node_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Node_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusOptionalRecursiveArray::Node);
                if (sample == NULL) {
                    return NULL;
                }

                Node_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->value - (char *)sample);

                Node_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->children - (char *)sample);

                Node_g_sampleAccessInfo.memberAccessInfos =
                Node_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusOptionalRecursiveArray::Node);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Node_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Node_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Node_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Node_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusOptionalRecursiveArray::Node >;

                Node_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Node_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Node_g_typePlugin =
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
                    NULL,
                    NULL
                };

                return &Node_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusOptionalRecursiveArray::Node >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusOptionalRecursiveArray::Node >::get())));
        }
        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusOptionalRecursiveArray::Node >::get(RTIXCdrResourceNode *parentVisitedNode)
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusOptionalRecursiveArray::Node >::get(parentVisitedNode))));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusOptionalRecursiveArray::Node >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusOptionalRecursiveArray::NodePlugin_new,
                ::CorpusOptionalRecursiveArray::NodePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusOptionalRecursiveArray::Node >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusOptionalRecursiveArray::Node& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = NodePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = NodePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusOptionalRecursiveArray::Node >::from_cdr_buffer(::CorpusOptionalRecursiveArray::Node& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = NodePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusOptionalRecursiveArray::Node from cdr buffer");
        }

        void topic_type_support< ::CorpusOptionalRecursiveArray::Node >::reset_sample(::CorpusOptionalRecursiveArray::Node& sample) 
        {
            sample.value = 0;
            ::rti::topic::reset_sample(sample.children);
        }

        void topic_type_support< ::CorpusOptionalRecursiveArray::Node >::allocate_sample(::CorpusOptionalRecursiveArray::Node& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
