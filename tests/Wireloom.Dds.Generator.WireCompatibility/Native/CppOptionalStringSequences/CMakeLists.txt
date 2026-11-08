cmake_minimum_required(VERSION 3.20)
project(WireloomCppOptionalStringSequences LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/09-optional-string-sequences")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/09-optional-string-sequences"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_optional_string_sequences_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/09-optional-string-sequences.cxx"
        "${source_directory}/09-optional-string-sequencesPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_optional_string_sequences_peer(rti-cpp-optional-string-sequences "${oracle}")
add_optional_string_sequences_peer(wireloom-cpp-optional-string-sequences "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
add_test(NAME wireloom-optional-string-sequences-registration
    COMMAND wireloom-cpp-optional-string-sequences --register)

foreach(shape IN ITEMS absent empty narrow-only wide-only both-partial both-full)
    if(shape STREQUAL "absent")
        set(narrow_state 0)
        set(wide_state 0)
    elseif(shape STREQUAL "empty")
        set(narrow_state 1)
        set(wide_state 1)
    elseif(shape STREQUAL "narrow-only")
        set(narrow_state 2)
        set(wide_state 0)
    elseif(shape STREQUAL "wide-only")
        set(narrow_state 0)
        set(wide_state 2)
    elseif(shape STREQUAL "both-partial")
        set(narrow_state 2)
        set(wide_state 2)
    else()
        set(narrow_state 4)
        set(wide_state 4)
    endif()
    set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-optional-string-sequences-${shape}.cdr")
    set(wireloom_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-optional-string-sequences-${shape}.cdr")
    add_test(NAME rti-optional-string-sequences-cdr-${shape}
        COMMAND rti-cpp-optional-string-sequences "${rti_cdr}" "${narrow_state}" "${wide_state}")
    add_test(NAME wireloom-optional-string-sequences-cdr-${shape}
        COMMAND wireloom-cpp-optional-string-sequences "${wireloom_cdr}" "${narrow_state}" "${wide_state}")
    add_test(NAME optional-string-sequences-cdr-equality-${shape}
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${wireloom_cdr}")
    set_tests_properties(optional-string-sequences-cdr-equality-${shape} PROPERTIES
        DEPENDS "rti-optional-string-sequences-cdr-${shape};wireloom-optional-string-sequences-cdr-${shape}")
endforeach()
