cmake_minimum_required(VERSION 3.20)
project(WireloomCppMultiple LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/02-multiple")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/02-multiple"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_multiple_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/02-multiple.cxx"
        "${source_directory}/02-multiplePlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_multiple_peer(rti-cpp-multiple "${oracle}")
add_multiple_peer(wireloom-cpp-multiple "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
set(rti_contract "${CMAKE_CURRENT_BINARY_DIR}/contract/rti")
set(wireloom_contract "${CMAKE_CURRENT_BINARY_DIR}/contract/wireloom")
file(MAKE_DIRECTORY "${rti_contract}" "${wireloom_contract}")

add_test(NAME rti-multiple-api-metadata-cdr COMMAND rti-cpp-multiple contract "${rti_contract}")
add_test(NAME wireloom-multiple-api-metadata-cdr COMMAND wireloom-cpp-multiple contract "${wireloom_contract}")
add_test(NAME multiple-rti-wireloom-cdr-equality
    COMMAND "${CMAKE_COMMAND}"
        "-DRTI_CONTRACT=${rti_contract}"
        "-DWIRELOOM_CONTRACT=${wireloom_contract}"
        -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
set_tests_properties(multiple-rti-wireloom-cdr-equality PROPERTIES
    DEPENDS "rti-multiple-api-metadata-cdr;wireloom-multiple-api-metadata-cdr")

add_test(NAME rti-multiple-registration-datawriter COMMAND rti-cpp-multiple register)
add_test(NAME wireloom-multiple-registration-datawriter COMMAND wireloom-cpp-multiple register)
add_test(NAME multiple-live-dds-cross-peer-matrix
    COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
        "$<TARGET_FILE:rti-cpp-multiple>"
        "$<TARGET_FILE:wireloom-cpp-multiple>"
        "${CMAKE_CURRENT_BINARY_DIR}/live")

