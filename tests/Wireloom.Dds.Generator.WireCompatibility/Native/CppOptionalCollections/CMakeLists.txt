cmake_minimum_required(VERSION 3.20)
project(WireloomCppOptionalCollections LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/09-optional-collections")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/09-optional-collections"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_optional_collections_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/09-optional-collections.cxx"
        "${source_directory}/09-optional-collectionsPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_optional_collections_peer(rti-cpp-optional-collections "${oracle}")
add_optional_collections_peer(wireloom-cpp-optional-collections "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
add_test(NAME wireloom-optional-collections-registration
    COMMAND wireloom-cpp-optional-collections --register)

foreach(shape IN ITEMS
        sequence-absent-array-absent
        sequence-empty-array-absent
        sequence-absent-array-present
        sequence-empty-array-present
        sequence-partial-array-present
        sequence-full-array-present)
    if(shape STREQUAL "sequence-absent-array-absent")
        set(sequence_state 0)
        set(array_present 0)
    elseif(shape STREQUAL "sequence-empty-array-absent")
        set(sequence_state 1)
        set(array_present 0)
    elseif(shape STREQUAL "sequence-absent-array-present")
        set(sequence_state 0)
        set(array_present 1)
    elseif(shape STREQUAL "sequence-empty-array-present")
        set(sequence_state 1)
        set(array_present 1)
    elseif(shape STREQUAL "sequence-partial-array-present")
        set(sequence_state 2)
        set(array_present 1)
    else()
        set(sequence_state 4)
        set(array_present 1)
    endif()
    set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-optional-collections-${shape}.cdr")
    set(wireloom_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-optional-collections-${shape}.cdr")
    add_test(NAME rti-optional-collections-cdr-${shape}
        COMMAND rti-cpp-optional-collections "${rti_cdr}" "${sequence_state}" "${array_present}")
    add_test(NAME wireloom-optional-collections-cdr-${shape}
        COMMAND wireloom-cpp-optional-collections "${wireloom_cdr}" "${sequence_state}" "${array_present}")
    add_test(NAME optional-collections-cdr-equality-${shape}
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${wireloom_cdr}")
    set_tests_properties(optional-collections-cdr-equality-${shape} PROPERTIES
        DEPENDS "rti-optional-collections-cdr-${shape};wireloom-optional-collections-cdr-${shape}")
endforeach()
