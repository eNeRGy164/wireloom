cmake_minimum_required(VERSION 3.20)
project(WireloomCppScopes LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/02-scopes")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/02-scopes"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_scopes_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/02-scopes.cxx"
        "${source_directory}/02-scopesPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_scopes_peer(rti-cpp-scopes "${oracle}")
add_scopes_peer(wireloom-cpp-scopes "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
add_test(NAME wireloom-scopes-registration COMMAND wireloom-cpp-scopes --register)
foreach(sequence_size IN ITEMS 0 1 4)
    set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-scopes-${sequence_size}.cdr")
    set(wireloom_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-scopes-${sequence_size}.cdr")
    add_test(NAME rti-scopes-cdr-${sequence_size}
        COMMAND rti-cpp-scopes "${rti_cdr}" "${sequence_size}")
    add_test(NAME wireloom-scopes-cdr-${sequence_size}
        COMMAND wireloom-cpp-scopes "${wireloom_cdr}" "${sequence_size}")
    add_test(NAME scopes-cdr-equality-${sequence_size}
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${wireloom_cdr}")
    set_tests_properties(scopes-cdr-equality-${sequence_size} PROPERTIES
        DEPENDS "rti-scopes-cdr-${sequence_size};wireloom-scopes-cdr-${sequence_size}")
endforeach()
