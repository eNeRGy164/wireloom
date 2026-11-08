cmake_minimum_required(VERSION 3.20)
project(WireloomCppInt8Boundaries LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/01-int8-boundaries")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/01-int8-boundaries"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_int8_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/01-int8-boundaries.cxx"
        "${source_directory}/01-int8-boundariesPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_int8_peer(rti-cpp-int8-boundaries "${oracle}")
add_int8_peer(wireloom-cpp-int8-boundaries "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
set(rti_contract "${CMAKE_CURRENT_BINARY_DIR}/contract/rti")
set(wireloom_contract "${CMAKE_CURRENT_BINARY_DIR}/contract/wireloom")
file(MAKE_DIRECTORY "${rti_contract}" "${wireloom_contract}")

add_test(NAME rti-int8-api-metadata-cdr
    COMMAND rti-cpp-int8-boundaries contract "${rti_contract}")
add_test(NAME wireloom-int8-api-metadata-cdr
    COMMAND wireloom-cpp-int8-boundaries contract "${wireloom_contract}")
add_test(NAME int8-rti-wireloom-cdr-equality
    COMMAND "${CMAKE_COMMAND}"
        "-DRTI_CONTRACT=${rti_contract}"
        "-DWIRELOOM_CONTRACT=${wireloom_contract}"
        -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
set_tests_properties(int8-rti-wireloom-cdr-equality PROPERTIES
    DEPENDS "rti-int8-api-metadata-cdr;wireloom-int8-api-metadata-cdr")

add_test(NAME rti-int8-registration-datawriter COMMAND rti-cpp-int8-boundaries register)
add_test(NAME wireloom-int8-registration-datawriter COMMAND wireloom-cpp-int8-boundaries register)
add_test(NAME int8-live-dds-cross-peer-matrix
    COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
        "$<TARGET_FILE:rti-cpp-int8-boundaries>"
        "$<TARGET_FILE:wireloom-cpp-int8-boundaries>"
        "${CMAKE_CURRENT_BINARY_DIR}/live")
