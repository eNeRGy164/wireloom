cmake_minimum_required(VERSION 3.20)
project(WireloomCppKeyBoundariesArray LANGUAGES CXX)

enable_testing()
list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/08-key-boundaries")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/08-key-boundaries"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_array_key_peer name source_directory)
    add_executable(${name} peer.cpp
        "${source_directory}/08-key-boundaries.cxx"
        "${source_directory}/08-key-boundariesPlugin.cxx")
    target_include_directories(${name} PRIVATE "${source_directory}")
    target_link_libraries(${name} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${name} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_array_key_peer(rti-key-boundaries-array "${oracle}")
add_array_key_peer(wireloom-key-boundaries-array "${WIRELOOM_CPP_GENERATED_DIR}")

foreach(peer IN ITEMS rti wireloom)
    add_test(NAME c008-array-key-${peer}-modern-cpp-binding COMMAND ${peer}-key-boundaries-array --api)
    add_test(NAME c008-array-key-${peer}-dynamic-type COMMAND ${peer}-key-boundaries-array --dynamic-type)
    add_test(NAME c008-array-key-${peer}-registration-datawriter COMMAND ${peer}-key-boundaries-array --registration)
    foreach(representation IN ITEMS xcdr1 xcdr2)
        add_test(NAME c008-array-key-${peer}-${representation}-cdr
            COMMAND ${peer}-key-boundaries-array --cdr ${representation}
                "${CMAKE_CURRENT_BINARY_DIR}/${peer}-${representation}.bin")
    endforeach()
endforeach()

foreach(representation IN ITEMS xcdr1 xcdr2)
    add_test(NAME c008-array-key-${representation}-rti-wireloom-byte-equality
        COMMAND "${CMAKE_COMMAND}"
            "-DRTI_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/rti-${representation}.bin"
            "-DWIRELOOM_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/wireloom-${representation}.bin"
            -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
    set_tests_properties(c008-array-key-${representation}-rti-wireloom-byte-equality PROPERTIES
        DEPENDS "c008-array-key-rti-${representation}-cdr;c008-array-key-wireloom-${representation}-cdr")
endforeach()

if(UNIX)
    add_test(NAME c008-array-key-live-dds-cross-peer-matrix
        COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
            "$<TARGET_FILE:rti-key-boundaries-array>"
            "$<TARGET_FILE:wireloom-key-boundaries-array>"
            "${CMAKE_CURRENT_BINARY_DIR}/live")
endif()
