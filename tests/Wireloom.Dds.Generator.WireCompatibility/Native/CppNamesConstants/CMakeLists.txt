cmake_minimum_required(VERSION 3.20)
project(WireloomCppNamesConstants LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/02-names-constants")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/02-names-constants"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_names_constants_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/02-names-constants.cxx"
        "${source_directory}/02-names-constantsPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_names_constants_peer(rti-cpp-names-constants "${oracle}")
add_names_constants_peer(wireloom-cpp-names-constants "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-names-constants.cdr")
set(wireloom_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-names-constants.cdr")
add_test(NAME rti-names-constants-cdr
    COMMAND rti-cpp-names-constants "${rti_cdr}")
add_test(NAME wireloom-names-constants-cdr
    COMMAND wireloom-cpp-names-constants "${wireloom_cdr}")
set_tests_properties(rti-names-constants-cdr PROPERTIES FIXTURES_SETUP rti_cdr)
set_tests_properties(wireloom-names-constants-cdr PROPERTIES FIXTURES_SETUP wireloom_cdr)
add_test(NAME names-constants-cdr-equality
    COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${wireloom_cdr}")
set_tests_properties(names-constants-cdr-equality PROPERTIES
    FIXTURES_REQUIRED "rti_cdr;wireloom_cdr")
