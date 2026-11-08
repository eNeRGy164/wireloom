cmake_minimum_required(VERSION 3.20)
project(WireloomCppNestedKeys LANGUAGES CXX)

enable_testing()

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/08-key-nested")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/08-key-nested"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_nested_keys_peer name)
    add_executable(${name} peer.cpp)
    set_target_properties(${name} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
    target_link_libraries(${name} PRIVATE RTIConnextDDS::cpp2_api)
endfunction()

add_nested_keys_peer(rti-cpp-nested-keys)
target_sources(rti-cpp-nested-keys PRIVATE
    "${oracle}/08-key-nested.cxx"
    "${oracle}/08-key-nestedPlugin.cxx")
target_include_directories(rti-cpp-nested-keys PRIVATE "${oracle}")

add_nested_keys_peer(wireloom-cpp-nested-keys)
target_sources(wireloom-cpp-nested-keys PRIVATE
    "${WIRELOOM_CPP_GENERATED_DIR}/08-key-nested.cxx"
    "${WIRELOOM_CPP_GENERATED_DIR}/08-key-nestedPlugin.cxx")
target_include_directories(wireloom-cpp-nested-keys PRIVATE "${WIRELOOM_CPP_GENERATED_DIR}")

foreach(peer IN ITEMS rti wireloom)
    add_test(NAME cpp-nested-keys-${peer}-api-and-registration
        COMMAND ${peer}-cpp-nested-keys --api)
    add_test(NAME cpp-nested-keys-${peer}-cdr
        COMMAND ${peer}-cpp-nested-keys
            "${CMAKE_CURRENT_BINARY_DIR}/${peer}-nested-keys-cdr.bin")
endforeach()

add_test(NAME cpp-nested-keys-compare-cdr
    COMMAND "${CMAKE_COMMAND}"
        -DRTI_CDR=${CMAKE_CURRENT_BINARY_DIR}/rti-nested-keys-cdr.bin
        -DWIRELOOM_CDR=${CMAKE_CURRENT_BINARY_DIR}/wireloom-nested-keys-cdr.bin
        -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
set_tests_properties(cpp-nested-keys-compare-cdr PROPERTIES
    DEPENDS "cpp-nested-keys-rti-cdr;cpp-nested-keys-wireloom-cdr")
