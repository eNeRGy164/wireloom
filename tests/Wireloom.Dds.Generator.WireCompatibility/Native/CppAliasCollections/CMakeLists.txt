cmake_minimum_required(VERSION 3.20)
project(WireloomCppAliasCollections LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/03-alias-collections")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/03-alias-collections"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_alias_collections_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/03-alias-collections.cxx"
        "${source_directory}/03-alias-collectionsPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_alias_collections_peer(rti-cpp-alias-collections "${oracle}")
add_alias_collections_peer(wireloom-cpp-alias-collections "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
add_test(NAME wireloom-alias-collections-registration COMMAND wireloom-cpp-alias-collections --register)
foreach(shape IN ITEMS empty outer-one-inner-empty outer-one-inner-partial outer-full-inner-full)
    if(shape STREQUAL "empty")
        set(outer_size 0)
        set(inner_size 0)
    elseif(shape STREQUAL "outer-one-inner-empty")
        set(outer_size 1)
        set(inner_size 0)
    elseif(shape STREQUAL "outer-one-inner-partial")
        set(outer_size 1)
        set(inner_size 1)
    else()
        set(outer_size 2)
        set(inner_size 3)
    endif()
    set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-alias-collections-${shape}.cdr")
    set(wireloom_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-alias-collections-${shape}.cdr")
    add_test(NAME rti-alias-collections-cdr-${shape}
        COMMAND rti-cpp-alias-collections "${rti_cdr}" "${outer_size}" "${inner_size}")
    add_test(NAME wireloom-alias-collections-cdr-${shape}
        COMMAND wireloom-cpp-alias-collections "${wireloom_cdr}" "${outer_size}" "${inner_size}")
    add_test(NAME alias-collections-cdr-equality-${shape}
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${wireloom_cdr}")
    set_tests_properties(alias-collections-cdr-equality-${shape} PROPERTIES
        DEPENDS "rti-alias-collections-cdr-${shape};wireloom-alias-collections-cdr-${shape}")
endforeach()
