cmake_minimum_required(VERSION 3.20)
project(WireloomCppAliasComposition LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/06-alias-composition")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/06-alias-composition"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_alias_composition_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/06-alias-composition.cxx"
        "${source_directory}/06-alias-compositionPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_alias_composition_peer(rti-cpp-alias-composition "${oracle}")
add_alias_composition_peer(wireloom-cpp-alias-composition "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
add_test(NAME wireloom-alias-composition-registration
    COMMAND wireloom-cpp-alias-composition --register)

foreach(shape IN ITEMS defaults non-default)
    set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-alias-composition-${shape}.cdr")
    set(wireloom_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-alias-composition-${shape}.cdr")
    add_test(NAME rti-alias-composition-cdr-${shape}
        COMMAND rti-cpp-alias-composition "${rti_cdr}" "${shape}")
    add_test(NAME wireloom-alias-composition-cdr-${shape}
        COMMAND wireloom-cpp-alias-composition "${wireloom_cdr}" "${shape}")
    add_test(NAME alias-composition-cdr-equality-${shape}
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${wireloom_cdr}")
    set_tests_properties(alias-composition-cdr-equality-${shape} PROPERTIES
        DEPENDS "rti-alias-composition-cdr-${shape};wireloom-alias-composition-cdr-${shape}")
endforeach()
