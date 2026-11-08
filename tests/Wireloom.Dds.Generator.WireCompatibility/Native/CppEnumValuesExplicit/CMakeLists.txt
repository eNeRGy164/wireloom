cmake_minimum_required(VERSION 3.20)
project(WireloomCppEnumValuesExplicit LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/03-enum-values-explicit")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/03-enum-values-explicit"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_enum_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/03-enum-values-explicit.cxx"
        "${source_directory}/03-enum-values-explicitPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_enum_peer(rti-cpp-enum-values-explicit "${oracle}")
add_enum_peer(wireloom-cpp-enum-values-explicit "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
set(rti_contract "${CMAKE_CURRENT_BINARY_DIR}/contract/rti")
set(wireloom_contract "${CMAKE_CURRENT_BINARY_DIR}/contract/wireloom")
file(MAKE_DIRECTORY "${rti_contract}" "${wireloom_contract}")
add_test(NAME rti-enum-values-explicit-binding-dynamic-type
    COMMAND rti-cpp-enum-values-explicit contract "${rti_contract}")
add_test(NAME wireloom-enum-values-explicit-binding-dynamic-type
    COMMAND wireloom-cpp-enum-values-explicit contract "${wireloom_contract}")
add_test(NAME enum-values-explicit-rti-wireloom-api-dynamic-type-equality
    COMMAND "${CMAKE_COMMAND}"
        "-DRTI_CONTRACT=${rti_contract}"
        "-DWIRELOOM_CONTRACT=${wireloom_contract}"
        -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-metadata.cmake")
set_tests_properties(enum-values-explicit-rti-wireloom-api-dynamic-type-equality PROPERTIES
    DEPENDS "rti-enum-values-explicit-binding-dynamic-type;wireloom-enum-values-explicit-binding-dynamic-type")
add_test(NAME enum-values-explicit-rti-wireloom-stream-parity
    COMMAND "${CMAKE_COMMAND}"
        "-DRTI_CONTRACT=${rti_contract}"
        "-DWIRELOOM_CONTRACT=${wireloom_contract}"
        -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-stream.cmake")
set_tests_properties(enum-values-explicit-rti-wireloom-stream-parity PROPERTIES
    DEPENDS "rti-enum-values-explicit-binding-dynamic-type;wireloom-enum-values-explicit-binding-dynamic-type")
