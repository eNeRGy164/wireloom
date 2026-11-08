cmake_minimum_required(VERSION 3.20)
project(WireloomCppInheritedKeysDerived LANGUAGES CXX)

enable_testing()
list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/08-key-inherited")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/08-key-inherited"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_inherited_keys_peer name source_directory)
    add_executable(${name} peer.cpp
        "${source_directory}/08-key-inherited.cxx"
        "${source_directory}/08-key-inheritedPlugin.cxx")
    target_include_directories(${name} PRIVATE "${source_directory}")
    target_link_libraries(${name} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${name} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_inherited_keys_peer(rti-inherited-keys-derived "${oracle}")
target_compile_definitions(rti-inherited-keys-derived PRIVATE RTI_ORACLE=1)
add_inherited_keys_peer(wireloom-inherited-keys-derived "${WIRELOOM_CPP_GENERATED_DIR}")

foreach(peer IN ITEMS rti wireloom)
    add_test(NAME c008-derived-${peer}-modern-cpp-api COMMAND ${peer}-inherited-keys-derived --api)
    add_test(NAME c008-derived-${peer}-dynamic-type COMMAND ${peer}-inherited-keys-derived --dynamic-type)
    add_test(NAME c008-derived-${peer}-registration-datawriter COMMAND ${peer}-inherited-keys-derived --registration)
    foreach(representation IN ITEMS xcdr1 xcdr2)
        add_test(NAME c008-derived-${peer}-${representation}-cdr
            COMMAND ${peer}-inherited-keys-derived --cdr ${representation}
                "${CMAKE_CURRENT_BINARY_DIR}/${peer}-${representation}.bin")
    endforeach()
endforeach()

foreach(representation IN ITEMS xcdr1 xcdr2)
    add_test(NAME c008-derived-${representation}-rti-wireloom-byte-equality
        COMMAND "${CMAKE_COMMAND}"
            "-DRTI_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/rti-${representation}.bin"
            "-DWIRELOOM_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/wireloom-${representation}.bin"
            -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
    set_tests_properties(c008-derived-${representation}-rti-wireloom-byte-equality PROPERTIES
        DEPENDS "c008-derived-rti-${representation}-cdr;c008-derived-wireloom-${representation}-cdr")
endforeach()

if(UNIX)
    add_test(NAME c008-derived-live-dds-cross-peer-matrix
        COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
            "$<TARGET_FILE:rti-inherited-keys-derived>"
            "$<TARGET_FILE:wireloom-inherited-keys-derived>"
            "${CMAKE_CURRENT_BINARY_DIR}/live")
endif()
