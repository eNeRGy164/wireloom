cmake_minimum_required(VERSION 3.20)
project(WireloomCppFullWidths LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/01-full-widths")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/01-full-widths"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_full_widths_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/01-full-widths.cxx"
        "${source_directory}/01-full-widthsPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_full_widths_peer(rti-cpp-full-widths "${oracle}")
add_full_widths_peer(wireloom-cpp-full-widths "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-full-widths.cdr")
set(wireloom_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-full-widths.cdr")
add_test(NAME rti-full-widths-cdr
    COMMAND rti-cpp-full-widths "${rti_cdr}")
add_test(NAME wireloom-full-widths-cdr
    COMMAND wireloom-cpp-full-widths "${wireloom_cdr}")
set_tests_properties(rti-full-widths-cdr PROPERTIES FIXTURES_SETUP rti_cdr)
set_tests_properties(wireloom-full-widths-cdr PROPERTIES FIXTURES_SETUP wireloom_cdr)
add_test(NAME full-widths-cdr-equality
    COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${wireloom_cdr}")
set_tests_properties(full-widths-cdr-equality PROPERTIES
    FIXTURES_REQUIRED "rti_cdr;wireloom_cdr")
