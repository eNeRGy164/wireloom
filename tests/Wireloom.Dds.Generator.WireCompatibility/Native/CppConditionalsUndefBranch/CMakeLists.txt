cmake_minimum_required(VERSION 3.20)
project(WireloomCppConditionalsUndefBranch LANGUAGES CXX)

enable_testing()
list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/11-conditionals")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/11-conditionals"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_conditionals_peer target source_directory)
    add_executable(${target} peer.cpp
        "${source_directory}/11-conditionals.cxx"
        "${source_directory}/11-conditionalsPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_conditionals_peer(rti-conditionals-undef-branch "${oracle}")
add_conditionals_peer(wireloom-conditionals-undef-branch "${WIRELOOM_CPP_GENERATED_DIR}")

foreach(peer IN ITEMS rti wireloom)
    add_test(NAME conditionals-undef-branch-${peer}-modern-cpp-binding
        COMMAND ${peer}-conditionals-undef-branch --api)
    add_test(NAME conditionals-undef-branch-${peer}-dynamic-type
        COMMAND ${peer}-conditionals-undef-branch --dynamic-type)
    add_test(NAME conditionals-undef-branch-${peer}-registration-datawriter
        COMMAND ${peer}-conditionals-undef-branch --registration)
    foreach(representation IN ITEMS xcdr1 xcdr2)
        add_test(NAME conditionals-undef-branch-${peer}-${representation}-cdr
            COMMAND ${peer}-conditionals-undef-branch --cdr ${representation}
                "${CMAKE_CURRENT_BINARY_DIR}/${peer}-${representation}.bin")
    endforeach()
endforeach()

foreach(representation IN ITEMS xcdr1 xcdr2)
    add_test(NAME conditionals-undef-branch-${representation}-rti-wireloom-byte-equality
        COMMAND "${CMAKE_COMMAND}"
            "-DRTI_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/rti-${representation}.bin"
            "-DWIRELOOM_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/wireloom-${representation}.bin"
            -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
    set_tests_properties(conditionals-undef-branch-${representation}-rti-wireloom-byte-equality PROPERTIES
        DEPENDS "conditionals-undef-branch-rti-${representation}-cdr;conditionals-undef-branch-wireloom-${representation}-cdr")
endforeach()

if(UNIX)
    add_test(NAME conditionals-undef-branch-live-dds-cross-peer-matrix
        COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
            "$<TARGET_FILE:rti-conditionals-undef-branch>"
            "$<TARGET_FILE:wireloom-conditionals-undef-branch>"
            "${CMAKE_CURRENT_BINARY_DIR}/live")
endif()
