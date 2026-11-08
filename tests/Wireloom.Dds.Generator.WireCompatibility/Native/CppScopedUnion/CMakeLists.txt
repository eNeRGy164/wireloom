cmake_minimum_required(VERSION 3.20)
project(WireloomCppScopedUnion LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/07-union-scoped")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/07-union-scoped"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_scoped_union_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/07-union-scoped.cxx"
        "${source_directory}/07-union-scopedPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_scoped_union_peer(rti-cpp-scoped-union "${oracle}")
add_scoped_union_peer(wireloom-cpp-scoped-union "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
add_test(NAME wireloom-scoped-union-registration
    COMMAND wireloom-cpp-scoped-union --register)
add_test(NAME rti-scoped-union-metadata
    COMMAND rti-cpp-scoped-union --metadata)
add_test(NAME wireloom-scoped-union-metadata
    COMMAND wireloom-cpp-scoped-union --metadata)

foreach(shape IN ITEMS defaults payload sequence-empty sequence-partial sequence-full)
    set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-scoped-union-${shape}.cdr")
    set(wireloom_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-scoped-union-${shape}.cdr")
    add_test(NAME rti-scoped-union-cdr-${shape}
        COMMAND rti-cpp-scoped-union "${rti_cdr}" "${shape}")
    add_test(NAME wireloom-scoped-union-cdr-${shape}
        COMMAND wireloom-cpp-scoped-union "${wireloom_cdr}" "${shape}")
    add_test(NAME scoped-union-cdr-equality-${shape}
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${wireloom_cdr}")
    set_tests_properties(scoped-union-cdr-equality-${shape} PROPERTIES
        DEPENDS "rti-scoped-union-cdr-${shape};wireloom-scoped-union-cdr-${shape}")
endforeach()
