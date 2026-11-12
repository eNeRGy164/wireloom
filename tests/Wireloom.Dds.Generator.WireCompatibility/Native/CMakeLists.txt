cmake_minimum_required(VERSION 3.16)
# Build the substituted C++ peer together with all files emitted by rtiddsgen.
# The RTI toolchain is supplied by the caller's pinned container environment.
project(WireloomWirePeer LANGUAGES CXX)

if(WIRELOOM_TYPED_OPTIONAL_RECURSIVE_ARRAY_FIXTURE)
    add_compile_definitions(WIRELOOM_TYPED_OPTIONAL_RECURSIVE_ARRAY_FIXTURE)
endif()
if(WIRELOOM_RTI_LEGACY_API)
    add_compile_definitions(WIRELOOM_RTI_LEGACY_API)
endif()
if(WIRELOOM_RTI_6_1_2)
    add_compile_definitions(WIRELOOM_RTI_6_1_2)
endif()
foreach(fixture_define IN ITEMS
    WIRELOOM_TYPED_C163_EXTENSIBILITY
    WIRELOOM_TYPED_C164_AUTOID_SCOPE
    WIRELOOM_TYPED_C165_ANNOTATION_ARGUMENTS
    WIRELOOM_TYPED_C166_MEMBER_DECLARATORS
    WIRELOOM_TYPED_C167_UNION_LABELS
    WIRELOOM_TYPED_C168_SCOPED_ENUM
    WIRELOOM_TYPED_C169_UNION_ARRAY
    WIRELOOM_TYPED_C170_STRING_BOUND
    WIRELOOM_TYPED_C171_DEFAULTS
    WIRELOOM_TYPED_C172_ENUM_CONSTANT)
    if(${fixture_define})
        add_compile_definitions(${fixture_define})
        add_compile_definitions(WIRELOOM_TYPED_IDL_GAPS_FIXTURE)
    endif()
endforeach()
if(WIRELOOM_TYPED_C173_UNBOUNDED_SUPPORT)
    add_compile_definitions(WIRELOOM_TYPED_C173_UNBOUNDED_SUPPORT)
endif()
if(WIRELOOM_TYPED_C174_UNBOUNDED_RECEIVE_LIFECYCLE)
    add_compile_definitions(WIRELOOM_TYPED_C174_UNBOUNDED_RECEIVE_LIFECYCLE)
endif()

find_package(RTIConnextDDS REQUIRED)

set(WIRELOOM_CPP_GENERATED_DIR "" CACHE PATH "Directory containing generated Wireloom C++ output")

file(GLOB_RECURSE RTI_GENERATED_SOURCES CONFIGURE_DEPENDS "${CMAKE_CURRENT_SOURCE_DIR}/*.cxx")
add_executable(wire-peer main.cpp ${RTI_GENERATED_SOURCES})
target_include_directories(wire-peer PRIVATE "${CMAKE_CURRENT_SOURCE_DIR}")
target_compile_features(wire-peer PRIVATE cxx_std_11)
target_link_libraries(wire-peer PRIVATE RTIConnextDDS::cpp2_api)

if(WIRELOOM_CPP_GENERATED_DIR)
    file(GLOB_RECURSE WIRELOOM_CPP_GENERATED_SOURCES CONFIGURE_DEPENDS "${WIRELOOM_CPP_GENERATED_DIR}/*.cxx")
    add_executable(wire-peer-wireloom-cpp wireloom-cpp-main.cpp ${WIRELOOM_CPP_GENERATED_SOURCES})
    target_include_directories(wire-peer-wireloom-cpp BEFORE PRIVATE "${WIRELOOM_CPP_GENERATED_DIR}")
    target_include_directories(wire-peer-wireloom-cpp PRIVATE "${CMAKE_CURRENT_SOURCE_DIR}")
    target_compile_definitions(wire-peer-wireloom-cpp PRIVATE WIRELOOM_CPP_GENERATED_PEER)
    target_compile_features(wire-peer-wireloom-cpp PRIVATE cxx_std_11)
    target_link_libraries(wire-peer-wireloom-cpp PRIVATE RTIConnextDDS::cpp2_api)
endif()
if(EXISTS "${CMAKE_CURRENT_SOURCE_DIR}/recursive-sequence-peer.cpp")
    add_executable(wire-peer-recursive-sequence recursive-sequence-peer.cpp ${RTI_GENERATED_SOURCES})
    target_include_directories(wire-peer-recursive-sequence PRIVATE "${CMAKE_CURRENT_SOURCE_DIR}")
    target_compile_features(wire-peer-recursive-sequence PRIVATE cxx_std_11)
    target_link_libraries(wire-peer-recursive-sequence PRIVATE RTIConnextDDS::cpp2_api)
endif()
if(WIRELOOM_CHAR8_LITERAL_FIXTURE)
    # RTI emits the high-bit char label literally. Compile it as one char8 byte,
    # not as a UTF-8 multicharacter integer truncated into the discriminator.
    target_compile_options(wire-peer PRIVATE -fexec-charset=ISO-8859-1)
endif()
if(WIRELOOM_UNSIGNED_CHAR_NUMERIC_UNION)
    # RTI's generated char-discriminator union stores IDL char in C++ char.
    # On Linux's default signed-char ABI, label 255 becomes -1 and the
    # generated branch guard rejects its assigned discriminator.
    target_compile_options(wire-peer PRIVATE -funsigned-char)
endif()
if(WIRELOOM_DYNAMIC_WCHAR_UNION)
    target_compile_definitions(wire-peer PRIVATE WIRELOOM_DYNAMIC_WCHAR_UNION)
    if(TARGET wire-peer-wireloom-cpp)
        target_compile_definitions(wire-peer-wireloom-cpp PRIVATE WIRELOOM_DYNAMIC_WCHAR_UNION)
    endif()
endif()
if(WIRELOOM_TYPED_STRING_SEQUENCE_FIXTURE)
    target_compile_definitions(wire-peer PRIVATE WIRELOOM_TYPED_STRING_SEQUENCE_FIXTURE)
    if(TARGET wire-peer-wireloom-cpp)
        target_compile_definitions(wire-peer-wireloom-cpp PRIVATE WIRELOOM_TYPED_STRING_SEQUENCE_FIXTURE)
    endif()
endif()
foreach(fixture_macro IN ITEMS WIRELOOM_TYPED_UNBOUNDED_STRING_ALIASES WIRELOOM_TYPED_RESOLVE_NAME_WSTRING_ALIASES WIRELOOM_TYPED_DEFAULT_LITERAL_ENUM_ARRAY WIRELOOM_TYPED_NUMERIC_CHAR_UNION WIRELOOM_TYPED_STRING_ALIAS_CHAINS WIRELOOM_TYPED_UNBOUNDED_SEQUENCE_ALIASES WIRELOOM_TYPED_UNION_STRING_SEQUENCE_ALIASES WIRELOOM_TYPED_UNION_STRING_SEQUENCE_BOUNDED WIRELOOM_TYPED_SEQUENCE_ARRAYS WIRELOOM_TYPED_OPTIONAL_COLLECTIONS_FIXTURE)
    if(${fixture_macro})
        target_compile_definitions(wire-peer PRIVATE ${fixture_macro})
        if(TARGET wire-peer-wireloom-cpp)
            target_compile_definitions(wire-peer-wireloom-cpp PRIVATE ${fixture_macro})
        endif()
    endif()
endforeach()
if(WIRELOOM_TYPED_WIDE_STRINGS_FIXTURE)
    target_compile_definitions(wire-peer PRIVATE WIRELOOM_TYPED_WIDE_STRINGS_FIXTURE)
    if(TARGET wire-peer-wireloom-cpp)
        target_compile_definitions(wire-peer-wireloom-cpp PRIVATE WIRELOOM_TYPED_WIDE_STRINGS_FIXTURE)
    endif()
endif()
if(WIRELOOM_TYPED_STRING_BOUNDARIES_FIXTURE)
    target_compile_definitions(wire-peer PRIVATE WIRELOOM_TYPED_STRING_BOUNDARIES_FIXTURE)
    if(TARGET wire-peer-wireloom-cpp)
        target_compile_definitions(wire-peer-wireloom-cpp PRIVATE WIRELOOM_TYPED_STRING_BOUNDARIES_FIXTURE)
    endif()
endif()
if(WIRELOOM_TYPED_OPTIONAL_AGGREGATE_FIXTURE)
    target_compile_definitions(wire-peer PRIVATE WIRELOOM_TYPED_OPTIONAL_AGGREGATE_FIXTURE)
    if(TARGET wire-peer-wireloom-cpp)
        target_compile_definitions(wire-peer-wireloom-cpp PRIVATE WIRELOOM_TYPED_OPTIONAL_AGGREGATE_FIXTURE)
    endif()
endif()
