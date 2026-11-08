cmake_minimum_required(VERSION 3.20)
project(WireloomCppFormatting LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/02-formatting")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/02-formatting"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_formatting_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/02-formatting.cxx"
        "${source_directory}/02-formattingPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_formatting_peer(rti-cpp-formatting "${oracle}")
add_formatting_peer(wireloom-cpp-formatting "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
set(rti_contract "${CMAKE_CURRENT_BINARY_DIR}/contract/rti")
set(wireloom_contract "${CMAKE_CURRENT_BINARY_DIR}/contract/wireloom")
file(MAKE_DIRECTORY "${rti_contract}" "${wireloom_contract}")

add_test(NAME rti-formatting-api-metadata-cdr
    COMMAND rti-cpp-formatting contract "${rti_contract}")
add_test(NAME wireloom-formatting-api-metadata-cdr
    COMMAND wireloom-cpp-formatting contract "${wireloom_contract}")
add_test(NAME formatting-rti-wireloom-cdr-equality
    COMMAND "${CMAKE_COMMAND}"
        "-DRTI_CONTRACT=${rti_contract}"
        "-DWIRELOOM_CONTRACT=${wireloom_contract}"
        -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
set_tests_properties(formatting-rti-wireloom-cdr-equality PROPERTIES
    DEPENDS "rti-formatting-api-metadata-cdr;wireloom-formatting-api-metadata-cdr")

add_test(NAME rti-formatting-registration-datawriter COMMAND rti-cpp-formatting register)
add_test(NAME wireloom-formatting-registration-datawriter COMMAND wireloom-cpp-formatting register)
add_test(NAME formatting-live-dds-cross-peer-matrix
    COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
        "$<TARGET_FILE:rti-cpp-formatting>"
        "$<TARGET_FILE:wireloom-cpp-formatting>"
        "${CMAKE_CURRENT_BINARY_DIR}/live")
