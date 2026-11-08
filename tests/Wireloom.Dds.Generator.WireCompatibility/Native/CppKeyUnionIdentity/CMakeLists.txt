cmake_minimum_required(VERSION 3.20)
project(WireloomCppKeyUnionIdentity LANGUAGES CXX)

enable_testing()
list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/08-key-union")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/08-key-union"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_identity_peer name source_directory)
    add_executable(${name} peer.cpp
        "${source_directory}/08-key-union.cxx"
        "${source_directory}/08-key-unionPlugin.cxx")
    target_include_directories(${name} PRIVATE "${source_directory}")
    target_link_libraries(${name} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${name} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_identity_peer(rti-key-union-identity "${oracle}")
add_identity_peer(wireloom-key-union-identity "${WIRELOOM_CPP_GENERATED_DIR}")

foreach(peer IN ITEMS rti wireloom)
    add_test(NAME c008-key-union-identity-${peer}-modern-cpp-binding
        COMMAND ${peer}-key-union-identity --api)
    add_test(NAME c008-key-union-identity-${peer}-dynamic-type
        COMMAND ${peer}-key-union-identity --dynamic-type)
    add_test(NAME c008-key-union-identity-${peer}-registration-datawriter
        COMMAND ${peer}-key-union-identity --registration)
    foreach(representation IN ITEMS xcdr1 xcdr2)
        add_test(NAME c008-key-union-identity-${peer}-${representation}-cdr
            COMMAND ${peer}-key-union-identity --cdr ${representation}
                "${CMAKE_CURRENT_BINARY_DIR}/${peer}-${representation}.bin")
    endforeach()
endforeach()

foreach(representation IN ITEMS xcdr1 xcdr2)
    add_test(NAME c008-key-union-identity-${representation}-rti-wireloom-byte-equality
        COMMAND "${CMAKE_COMMAND}"
            "-DRTI_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/rti-${representation}.bin"
            "-DWIRELOOM_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/wireloom-${representation}.bin"
            -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
    set_tests_properties(c008-key-union-identity-${representation}-rti-wireloom-byte-equality PROPERTIES
        DEPENDS "c008-key-union-identity-rti-${representation}-cdr;c008-key-union-identity-wireloom-${representation}-cdr")
endforeach()

if(UNIX)
    add_test(NAME c008-key-union-identity-live-dds-cross-peer-matrix
        COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
            "$<TARGET_FILE:rti-key-union-identity>"
            "$<TARGET_FILE:wireloom-key-union-identity>"
            "${CMAKE_CURRENT_BINARY_DIR}/live")
endif()
