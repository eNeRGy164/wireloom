cmake_minimum_required(VERSION 3.20)
project(WireloomCppAggregates LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/06-aggregates")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/06-aggregates"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_aggregates_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/06-aggregates.cxx"
        "${source_directory}/06-aggregatesPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_aggregates_peer(rti-cpp-aggregates "${oracle}")
add_aggregates_peer(wireloom-cpp-aggregates "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
add_test(NAME wireloom-aggregates-nonrecursive-registration
    COMMAND wireloom-cpp-aggregates --register-nonrecursive)
add_test(NAME rti-aggregates-recursive-registration-unsupported
    COMMAND rti-cpp-aggregates --expect-recursive-registration-unsupported)
add_test(NAME wireloom-aggregates-recursive-registration-unsupported
    COMMAND wireloom-cpp-aggregates --expect-recursive-registration-unsupported)

foreach(shape IN ITEMS base-default base-value derived-empty derived-value derived-boundary recursive-empty recursive-depth composed-default composed-value)
    set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-aggregates-${shape}.cdr")
    set(wireloom_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-aggregates-${shape}.cdr")
    add_test(NAME rti-aggregates-cdr-${shape}
        COMMAND rti-cpp-aggregates "${rti_cdr}" "${shape}")
    add_test(NAME wireloom-aggregates-cdr-${shape}
        COMMAND wireloom-cpp-aggregates "${wireloom_cdr}" "${shape}")
    add_test(NAME aggregates-cdr-equality-${shape}
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${wireloom_cdr}")
    set_tests_properties(aggregates-cdr-equality-${shape} PROPERTIES
        DEPENDS "rti-aggregates-cdr-${shape};wireloom-aggregates-cdr-${shape}")
endforeach()
