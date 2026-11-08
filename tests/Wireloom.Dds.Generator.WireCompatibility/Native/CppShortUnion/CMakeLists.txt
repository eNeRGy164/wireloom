cmake_minimum_required(VERSION 3.20)
project(WireloomCppShortUnion LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/07-union-short")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/07-union-short"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_short_union_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/07-union-short.cxx"
        "${source_directory}/07-union-shortPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_short_union_peer(rti-cpp-short-union "${oracle}")
add_short_union_peer(wireloom-cpp-short-union "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
add_test(NAME wireloom-short-union-registration
    COMMAND wireloom-cpp-short-union --register)
add_test(NAME rti-short-union-metadata
    COMMAND rti-cpp-short-union --metadata)
add_test(NAME wireloom-short-union-metadata
    COMMAND wireloom-cpp-short-union --metadata)

foreach(shape IN ITEMS defaults negative text default-branch)
    set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-short-union-${shape}.cdr")
    set(wireloom_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-short-union-${shape}.cdr")
    add_test(NAME rti-short-union-cdr-${shape}
        COMMAND rti-cpp-short-union "${rti_cdr}" "${shape}")
    add_test(NAME wireloom-short-union-cdr-${shape}
        COMMAND wireloom-cpp-short-union "${wireloom_cdr}" "${shape}")
    add_test(NAME short-union-cdr-equality-${shape}
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${wireloom_cdr}")
    set_tests_properties(short-union-cdr-equality-${shape} PROPERTIES
        DEPENDS "rti-short-union-cdr-${shape};wireloom-short-union-cdr-${shape}")
endforeach()
