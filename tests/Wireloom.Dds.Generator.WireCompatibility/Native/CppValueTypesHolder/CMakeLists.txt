cmake_minimum_required(VERSION 3.20)
project(WireloomCppValueTypesHolder LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/06-valuetypes")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/06-valuetypes"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_value_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/06-valuetypes.cxx"
        "${source_directory}/06-valuetypesPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}" ..)
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    if(target STREQUAL "rti-cpp-valuetypes-holder")
        target_compile_definitions(${target} PRIVATE RTI_ORACLE=1)
    endif()
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_value_peer(rti-cpp-valuetypes-holder "${oracle}")
add_value_peer(wireloom-cpp-valuetypes-holder "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
set(rti_contract "${CMAKE_CURRENT_BINARY_DIR}/contract/rti")
set(wireloom_contract "${CMAKE_CURRENT_BINARY_DIR}/contract/wireloom")
file(MAKE_DIRECTORY "${rti_contract}" "${wireloom_contract}")
add_test(NAME rti-valuetypes-holder-api-dynamic-type-cdr
    COMMAND rti-cpp-valuetypes-holder contract "${rti_contract}")
add_test(NAME wireloom-valuetypes-holder-api-dynamic-type-cdr
    COMMAND wireloom-cpp-valuetypes-holder contract "${wireloom_contract}")
add_test(NAME valuetypes-holder-api-dynamic-type-equality
    COMMAND "${CMAKE_COMMAND}"
        "-DRTI_CONTRACT=${rti_contract}"
        "-DWIRELOOM_CONTRACT=${wireloom_contract}"
        -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-contract.cmake")
set_tests_properties(valuetypes-holder-api-dynamic-type-equality PROPERTIES
    DEPENDS "rti-valuetypes-holder-api-dynamic-type-cdr;wireloom-valuetypes-holder-api-dynamic-type-cdr")

foreach(representation IN ITEMS xcdr1 xcdr2)
    foreach(state IN ITEMS default alternate)
        add_test(NAME valuetypes-holder-${representation}-${state}-cdr-equality
            COMMAND "${CMAKE_COMMAND}" -E compare_files
                "${rti_contract}/${representation}-${state}.cdr"
                "${wireloom_contract}/${representation}-${state}.cdr")
        set_tests_properties(valuetypes-holder-${representation}-${state}-cdr-equality PROPERTIES
            DEPENDS "rti-valuetypes-holder-api-dynamic-type-cdr;wireloom-valuetypes-holder-api-dynamic-type-cdr")
    endforeach()
endforeach()

add_test(NAME rti-valuetypes-holder-registration-datawriter
    COMMAND rti-cpp-valuetypes-holder register)
add_test(NAME wireloom-valuetypes-holder-registration-datawriter
    COMMAND wireloom-cpp-valuetypes-holder register)
add_test(NAME valuetypes-holder-live-dds-cross-peer-matrix
    COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
        "$<TARGET_FILE:rti-cpp-valuetypes-holder>"
        "$<TARGET_FILE:wireloom-cpp-valuetypes-holder>"
        "${CMAKE_CURRENT_BINARY_DIR}/live")
