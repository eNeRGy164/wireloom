cmake_minimum_required(VERSION 3.20)
project(WireloomCppArraySequences LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/05-array-of-sequences")
set(wireloom_c018 "${repository}/docs/corpus/7.7.0/generator/cpp/features/05-array-of-sequences")
set(wireloom_c149 "${repository}/docs/corpus/7.7.0/generator/cpp/features/05-array-of-sequences-idl")

function(add_array_sequences_peer target source_directory file_stem)
    add_executable(${target}
        peer.cpp
        "${source_directory}/${file_stem}.cxx"
        "${source_directory}/${file_stem}Plugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_compile_definitions(${target} PRIVATE "ARRAY_SEQUENCE_HEADER=\"${file_stem}.hpp\"")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_array_sequences_peer(rti-cpp-array-sequences "${oracle}" "05-array-of-sequences")
add_array_sequences_peer(wireloom-cpp-array-sequences-c018 "${wireloom_c018}" "05-array-of-sequences")
add_array_sequences_peer(wireloom-cpp-array-sequences-c149 "${wireloom_c149}" "05-array-of-sequences-idl")

enable_testing()
add_test(NAME wireloom-array-sequences-c018-registration
    COMMAND wireloom-cpp-array-sequences-c018 --register)
add_test(NAME wireloom-array-sequences-c149-registration
    COMMAND wireloom-cpp-array-sequences-c149 --register)

foreach(shape IN ITEMS sequence-empty sequence-unequal sequence-mixed-empty sequence-boundary)
    set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-array-sequences-${shape}.cdr")
    set(c018_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-c018-array-sequences-${shape}.cdr")
    set(c149_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-c149-array-sequences-${shape}.cdr")
    add_test(NAME rti-array-sequences-cdr-${shape}
        COMMAND rti-cpp-array-sequences "${rti_cdr}" "${shape}")
    add_test(NAME wireloom-c018-array-sequences-cdr-${shape}
        COMMAND wireloom-cpp-array-sequences-c018 "${c018_cdr}" "${shape}")
    add_test(NAME wireloom-c149-array-sequences-cdr-${shape}
        COMMAND wireloom-cpp-array-sequences-c149 "${c149_cdr}" "${shape}")
    add_test(NAME array-sequences-c018-cdr-equality-${shape}
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${c018_cdr}")
    add_test(NAME array-sequences-c149-cdr-equality-${shape}
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${c149_cdr}")
    set_tests_properties(array-sequences-c018-cdr-equality-${shape} PROPERTIES
        DEPENDS "rti-array-sequences-cdr-${shape};wireloom-c018-array-sequences-cdr-${shape}")
    set_tests_properties(array-sequences-c149-cdr-equality-${shape} PROPERTIES
        DEPENDS "rti-array-sequences-cdr-${shape};wireloom-c149-array-sequences-cdr-${shape}")
endforeach()
