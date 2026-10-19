cmake_minimum_required(VERSION 3.16)
# Build the substituted C++ peer together with all files emitted by rtiddsgen.
# The RTI toolchain is supplied by the caller's pinned container environment.
project(WireloomWirePeer LANGUAGES CXX)

if(WIRELOOM_TYPED_OPTIONAL_RECURSIVE_ARRAY_FIXTURE)
    add_compile_definitions(WIRELOOM_TYPED_OPTIONAL_RECURSIVE_ARRAY_FIXTURE)
endif()
if(WIRELOOM_RTI_7_3_1)
    add_compile_definitions(WIRELOOM_RTI_7_3_1)
endif()

find_package(RTIConnextDDS REQUIRED)

file(GLOB_RECURSE RTI_GENERATED_SOURCES CONFIGURE_DEPENDS "${CMAKE_CURRENT_SOURCE_DIR}/*.cxx")
add_executable(wire-peer main.cpp ${RTI_GENERATED_SOURCES})
target_compile_features(wire-peer PRIVATE cxx_std_11)
target_link_libraries(wire-peer PRIVATE RTIConnextDDS::cpp2_api)
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
endif()
if(WIRELOOM_TYPED_STRING_SEQUENCE_FIXTURE)
    target_compile_definitions(wire-peer PRIVATE WIRELOOM_TYPED_STRING_SEQUENCE_FIXTURE)
endif()
foreach(fixture_macro IN ITEMS WIRELOOM_TYPED_UNBOUNDED_STRING_ALIASES WIRELOOM_TYPED_RESOLVE_NAME_WSTRING_ALIASES WIRELOOM_TYPED_NUMERIC_CHAR_UNION WIRELOOM_TYPED_STRING_ALIAS_CHAINS WIRELOOM_TYPED_UNBOUNDED_SEQUENCE_ALIASES WIRELOOM_TYPED_UNION_STRING_SEQUENCE_ALIASES WIRELOOM_TYPED_UNION_STRING_SEQUENCE_BOUNDED)
    if(${fixture_macro})
        target_compile_definitions(wire-peer PRIVATE ${fixture_macro})
    endif()
endforeach()
if(WIRELOOM_TYPED_WIDE_STRINGS_FIXTURE)
    target_compile_definitions(wire-peer PRIVATE WIRELOOM_TYPED_WIDE_STRINGS_FIXTURE)
endif()
if(WIRELOOM_TYPED_STRING_BOUNDARIES_FIXTURE)
    target_compile_definitions(wire-peer PRIVATE WIRELOOM_TYPED_STRING_BOUNDARIES_FIXTURE)
endif()
if(WIRELOOM_TYPED_OPTIONAL_AGGREGATE_FIXTURE)
    target_compile_definitions(wire-peer PRIVATE WIRELOOM_TYPED_OPTIONAL_AGGREGATE_FIXTURE)
endif()
