cmake_minimum_required(VERSION 3.20)
project(WireloomCppMultiLabelUnion LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/07-union-multilabel")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/07-union-multilabel"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_multilabel_union_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/07-union-multilabel.cxx"
        "${source_directory}/07-union-multilabelPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_multilabel_union_peer(rti-cpp-multilabel-union "${oracle}")
add_multilabel_union_peer(wireloom-cpp-multilabel-union "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
add_test(NAME wireloom-multilabel-union-registration
    COMMAND wireloom-cpp-multilabel-union --register)
add_test(NAME rti-multilabel-union-metadata
    COMMAND rti-cpp-multilabel-union --metadata)
add_test(NAME wireloom-multilabel-union-metadata
    COMMAND wireloom-cpp-multilabel-union --metadata)

foreach(shape IN ITEMS defaults number-label-1 number-label-5 text)
    set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-multilabel-union-${shape}.cdr")
    set(wireloom_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-multilabel-union-${shape}.cdr")
    add_test(NAME rti-multilabel-union-cdr-${shape}
        COMMAND rti-cpp-multilabel-union "${rti_cdr}" "${shape}")
    add_test(NAME wireloom-multilabel-union-cdr-${shape}
        COMMAND wireloom-cpp-multilabel-union "${wireloom_cdr}" "${shape}")
    add_test(NAME multilabel-union-cdr-equality-${shape}
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${wireloom_cdr}")
    set_tests_properties(multilabel-union-cdr-equality-${shape} PROPERTIES
        DEPENDS "rti-multilabel-union-cdr-${shape};wireloom-multilabel-union-cdr-${shape}")
endforeach()
