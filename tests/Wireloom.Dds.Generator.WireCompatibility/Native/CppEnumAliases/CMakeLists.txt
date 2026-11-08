cmake_minimum_required(VERSION 3.20)
project(WireloomCppEnumAliases LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/03-enums-aliases")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/03-enums-aliases"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_enum_aliases_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/03-enums-aliases.cxx"
        "${source_directory}/03-enums-aliasesPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_enum_aliases_peer(rti-cpp-enum-aliases "${oracle}")
add_enum_aliases_peer(wireloom-cpp-enum-aliases "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
add_test(NAME wireloom-enum-aliases-registration COMMAND wireloom-cpp-enum-aliases --register)
foreach(sequence_size IN ITEMS 0 1 3)
    set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-enum-aliases-${sequence_size}.cdr")
    set(wireloom_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-enum-aliases-${sequence_size}.cdr")
    add_test(NAME rti-enum-aliases-cdr-${sequence_size}
        COMMAND rti-cpp-enum-aliases "${rti_cdr}" "${sequence_size}")
    add_test(NAME wireloom-enum-aliases-cdr-${sequence_size}
        COMMAND wireloom-cpp-enum-aliases "${wireloom_cdr}" "${sequence_size}")
    add_test(NAME enum-aliases-cdr-equality-${sequence_size}
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${wireloom_cdr}")
    set_tests_properties(enum-aliases-cdr-equality-${sequence_size} PROPERTIES
        DEPENDS "rti-enum-aliases-cdr-${sequence_size};wireloom-enum-aliases-cdr-${sequence_size}")
endforeach()
