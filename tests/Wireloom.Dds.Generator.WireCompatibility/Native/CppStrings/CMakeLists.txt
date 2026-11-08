cmake_minimum_required(VERSION 3.20)
project(WireloomCppStrings LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/04-strings")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/04-strings"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_strings_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/04-strings.cxx"
        "${source_directory}/04-stringsPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_strings_peer(rti-cpp-strings "${oracle}")
add_strings_peer(wireloom-cpp-strings "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
add_test(NAME wireloom-strings-registration COMMAND wireloom-cpp-strings --register)

foreach(shape IN ITEMS empty partial boundary)
    set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-strings-${shape}.cdr")
    set(wireloom_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-strings-${shape}.cdr")
    add_test(NAME rti-strings-cdr-${shape}
        COMMAND rti-cpp-strings "${rti_cdr}" "${shape}")
    add_test(NAME wireloom-strings-cdr-${shape}
        COMMAND wireloom-cpp-strings "${wireloom_cdr}" "${shape}")
    add_test(NAME strings-cdr-equality-${shape}
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${wireloom_cdr}")
    set_tests_properties(strings-cdr-equality-${shape} PROPERTIES
        DEPENDS "rti-strings-cdr-${shape};wireloom-strings-cdr-${shape}")
endforeach()
