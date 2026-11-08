cmake_minimum_required(VERSION 3.20)
project(WireloomCppConstantExpressions LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/02-constant-expressions")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/02-constant-expressions"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_constant_expressions_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/02-constant-expressions.cxx"
        "${source_directory}/02-constant-expressionsPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_constant_expressions_peer(rti-cpp-constant-expressions "${oracle}")
add_constant_expressions_peer(wireloom-cpp-constant-expressions "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
add_test(NAME rti-constant-expressions-api COMMAND rti-cpp-constant-expressions --metadata)
add_test(NAME wireloom-constant-expressions-api COMMAND wireloom-cpp-constant-expressions --metadata)
add_test(NAME wireloom-constant-expressions-registration
    COMMAND wireloom-cpp-constant-expressions --register)
foreach(sequence_size IN ITEMS 0 1 5)
    set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-constant-expressions-${sequence_size}.cdr")
    set(wireloom_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-constant-expressions-${sequence_size}.cdr")
    add_test(NAME rti-constant-expressions-cdr-${sequence_size}
        COMMAND rti-cpp-constant-expressions "${rti_cdr}" "${sequence_size}")
    add_test(NAME wireloom-constant-expressions-cdr-${sequence_size}
        COMMAND wireloom-cpp-constant-expressions "${wireloom_cdr}" "${sequence_size}")
    add_test(NAME constant-expressions-cdr-equality-${sequence_size}
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${wireloom_cdr}")
    set_tests_properties(constant-expressions-cdr-equality-${sequence_size} PROPERTIES
        DEPENDS "rti-constant-expressions-cdr-${sequence_size};wireloom-constant-expressions-cdr-${sequence_size}")
endforeach()
