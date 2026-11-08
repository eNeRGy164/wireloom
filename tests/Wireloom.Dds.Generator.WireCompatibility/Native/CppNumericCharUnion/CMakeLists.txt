cmake_minimum_required(VERSION 3.20)
project(WireloomCppNumericCharUnion LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/07-union-char-numeric")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/07-union-char-numeric"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_numeric_char_union_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/07-union-char-numeric.cxx"
        "${source_directory}/07-union-char-numericPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_numeric_char_union_peer(rti-cpp-numeric-char-union "${oracle}")
add_numeric_char_union_peer(wireloom-cpp-numeric-char-union "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
add_test(NAME rti-numeric-char-union-api COMMAND rti-cpp-numeric-char-union --api)
add_test(NAME wireloom-numeric-char-union-api COMMAND wireloom-cpp-numeric-char-union --api)
add_test(NAME wireloom-numeric-char-union-registration
    COMMAND wireloom-cpp-numeric-char-union --register)
add_test(NAME rti-numeric-char-union-metadata COMMAND rti-cpp-numeric-char-union --metadata)
add_test(NAME wireloom-numeric-char-union-metadata COMMAND wireloom-cpp-numeric-char-union --metadata)

foreach(shape IN ITEMS defaults explicit fallback)
    set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-numeric-char-union-${shape}.cdr")
    set(wireloom_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-numeric-char-union-${shape}.cdr")
    add_test(NAME rti-numeric-char-union-cdr-${shape}
        COMMAND rti-cpp-numeric-char-union "${rti_cdr}" "${shape}")
    add_test(NAME wireloom-numeric-char-union-cdr-${shape}
        COMMAND wireloom-cpp-numeric-char-union "${wireloom_cdr}" "${shape}")
    add_test(NAME numeric-char-union-cdr-equality-${shape}
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${wireloom_cdr}")
    set_tests_properties(numeric-char-union-cdr-equality-${shape} PROPERTIES
        DEPENDS "rti-numeric-char-union-cdr-${shape};wireloom-numeric-char-union-cdr-${shape}")
endforeach()
