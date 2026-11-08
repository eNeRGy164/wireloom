cmake_minimum_required(VERSION 3.20)
project(WireloomCppPrimitiveBoundaries LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/01-boundaries")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/01-boundaries"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_boundaries_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/01-boundaries.cxx"
        "${source_directory}/01-boundariesPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_boundaries_peer(rti-cpp-boundaries "${oracle}")
add_boundaries_peer(wireloom-cpp-boundaries "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
set(rti_contract "${CMAKE_CURRENT_BINARY_DIR}/contract/rti")
set(wireloom_contract "${CMAKE_CURRENT_BINARY_DIR}/contract/wireloom")
file(MAKE_DIRECTORY "${rti_contract}" "${wireloom_contract}")

add_test(NAME rti-boundaries-api-metadata-cdr
    COMMAND rti-cpp-boundaries contract "${rti_contract}")
add_test(NAME wireloom-boundaries-api-metadata-cdr
    COMMAND wireloom-cpp-boundaries contract "${wireloom_contract}")
add_test(NAME boundaries-rti-wireloom-cdr-equality
    COMMAND "${CMAKE_COMMAND}"
        "-DRTI_CONTRACT=${rti_contract}"
        "-DWIRELOOM_CONTRACT=${wireloom_contract}"
        -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
set_tests_properties(boundaries-rti-wireloom-cdr-equality PROPERTIES
    DEPENDS "rti-boundaries-api-metadata-cdr;wireloom-boundaries-api-metadata-cdr")

add_test(NAME rti-boundaries-registration-datawriter COMMAND rti-cpp-boundaries register)
add_test(NAME wireloom-boundaries-registration-datawriter COMMAND wireloom-cpp-boundaries register)
add_test(NAME boundaries-live-dds-cross-peer-matrix
    COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
        "$<TARGET_FILE:rti-cpp-boundaries>"
        "$<TARGET_FILE:wireloom-cpp-boundaries>"
        "${CMAKE_CURRENT_BINARY_DIR}/live")
