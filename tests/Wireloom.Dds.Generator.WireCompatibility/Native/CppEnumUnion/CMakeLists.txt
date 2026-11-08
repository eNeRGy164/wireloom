cmake_minimum_required(VERSION 3.20)
project(WireloomCppEnumUnion LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/07-union-enum")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/07-union-enum"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_enum_union_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/07-union-enum.cxx"
        "${source_directory}/07-union-enumPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_enum_union_peer(rti-cpp-enum-union "${oracle}")
add_enum_union_peer(wireloom-cpp-enum-union "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
add_test(NAME wireloom-enum-union-registration
    COMMAND wireloom-cpp-enum-union --register)
add_test(NAME rti-enum-union-metadata
    COMMAND rti-cpp-enum-union --metadata)
add_test(NAME wireloom-enum-union-metadata
    COMMAND wireloom-cpp-enum-union --metadata)

foreach(shape IN ITEMS defaults number text payload)
    set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-enum-union-${shape}.cdr")
    set(wireloom_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-enum-union-${shape}.cdr")
    add_test(NAME rti-enum-union-cdr-${shape}
        COMMAND rti-cpp-enum-union "${rti_cdr}" "${shape}")
    add_test(NAME wireloom-enum-union-cdr-${shape}
        COMMAND wireloom-cpp-enum-union "${wireloom_cdr}" "${shape}")
    add_test(NAME enum-union-cdr-equality-${shape}
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${wireloom_cdr}")
    set_tests_properties(enum-union-cdr-equality-${shape} PROPERTIES
        DEPENDS "rti-enum-union-cdr-${shape};wireloom-enum-union-cdr-${shape}")
endforeach()
