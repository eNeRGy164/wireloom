cmake_minimum_required(VERSION 3.20)
project(WireloomCppIdGaps LANGUAGES CXX)

enable_testing()
list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/09-id-gaps")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/09-id-gaps"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_message_peer name source_directory)
    add_executable(${name} peer.cpp
        "${source_directory}/09-id-gaps.cxx"
        "${source_directory}/09-id-gapsPlugin.cxx")
    target_include_directories(${name} PRIVATE "${source_directory}")
    target_link_libraries(${name} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${name} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_message_peer(rti-id-gaps-message "${oracle}")
add_message_peer(wireloom-id-gaps-message "${WIRELOOM_CPP_GENERATED_DIR}")

foreach(peer IN ITEMS rti wireloom)
    add_test(NAME c037-id-gaps-message-${peer}-modern-cpp-binding
        COMMAND ${peer}-id-gaps-message --api)
    add_test(NAME c037-id-gaps-message-${peer}-dynamic-type
        COMMAND ${peer}-id-gaps-message --dynamic-type)
    add_test(NAME c037-id-gaps-message-${peer}-registration-datawriter
        COMMAND ${peer}-id-gaps-message --registration)
    foreach(representation IN ITEMS xcdr1 xcdr2)
        add_test(NAME c037-id-gaps-message-${peer}-${representation}-cdr
            COMMAND ${peer}-id-gaps-message --cdr ${representation}
                "${CMAKE_CURRENT_BINARY_DIR}/${peer}-${representation}.bin")
    endforeach()
endforeach()

foreach(representation IN ITEMS xcdr1 xcdr2)
    add_test(NAME c037-id-gaps-message-${representation}-rti-wireloom-byte-equality
        COMMAND "${CMAKE_COMMAND}"
            "-DRTI_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/rti-${representation}.bin"
            "-DWIRELOOM_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/wireloom-${representation}.bin"
            -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
    set_tests_properties(c037-id-gaps-message-${representation}-rti-wireloom-byte-equality PROPERTIES
        DEPENDS "c037-id-gaps-message-rti-${representation}-cdr;c037-id-gaps-message-wireloom-${representation}-cdr")
endforeach()

if(UNIX)
    add_test(NAME c037-id-gaps-message-live-dds-cross-peer-matrix
        COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
            "$<TARGET_FILE:rti-id-gaps-message>"
            "$<TARGET_FILE:wireloom-id-gaps-message>"
            "${CMAKE_CURRENT_BINARY_DIR}/live")
endif()
