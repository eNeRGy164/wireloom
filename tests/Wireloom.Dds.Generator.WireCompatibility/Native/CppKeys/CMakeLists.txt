cmake_minimum_required(VERSION 3.20)
project(WireloomCppKeys LANGUAGES CXX)

enable_testing()

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/08-keys")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/08-keys"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_keys_peer name)
    add_executable(${name} peer.cpp)
    set_target_properties(${name} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
    target_link_libraries(${name} PRIVATE RTIConnextDDS::cpp2_api)
endfunction()

add_keys_peer(rti-cpp-keys)
target_sources(rti-cpp-keys PRIVATE
    "${oracle}/08-keys.cxx"
    "${oracle}/08-keysPlugin.cxx")
target_include_directories(rti-cpp-keys PRIVATE "${oracle}")

add_keys_peer(wireloom-cpp-keys)
target_sources(wireloom-cpp-keys PRIVATE
    "${WIRELOOM_CPP_GENERATED_DIR}/08-keys.cxx"
    "${WIRELOOM_CPP_GENERATED_DIR}/08-keysPlugin.cxx")
target_include_directories(wireloom-cpp-keys PRIVATE "${WIRELOOM_CPP_GENERATED_DIR}")

foreach(peer IN ITEMS rti wireloom)
    add_test(NAME c008-${peer}-keys-api-and-registration
        COMMAND ${peer}-cpp-keys --api)
    add_test(NAME c008-${peer}-keys-cdr
        COMMAND ${peer}-cpp-keys
            "${CMAKE_CURRENT_BINARY_DIR}/${peer}-keys-cdr.bin")
endforeach()

add_test(NAME c008-compare-keys-cdr
    COMMAND "${CMAKE_COMMAND}"
        -DRTI_CDR=${CMAKE_CURRENT_BINARY_DIR}/rti-keys-cdr.bin
        -DWIRELOOM_CDR=${CMAKE_CURRENT_BINARY_DIR}/wireloom-keys-cdr.bin
        -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
set_tests_properties(c008-compare-keys-cdr PROPERTIES
    DEPENDS "c008-rti-keys-cdr;c008-wireloom-keys-cdr")
