cmake_minimum_required(VERSION 3.20)
project(WireloomCppDefaultExtensibility LANGUAGES CXX)

enable_testing()
list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/09-default")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/09-default"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_message_peer name source_directory)
    add_executable(${name} peer.cpp
        "${source_directory}/09-default.cxx"
        "${source_directory}/09-defaultPlugin.cxx")
    target_include_directories(${name} PRIVATE "${source_directory}")
    target_link_libraries(${name} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${name} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_message_peer(rti-default-message "${oracle}")
add_message_peer(wireloom-default-message "${WIRELOOM_CPP_GENERATED_DIR}")

foreach(peer IN ITEMS rti wireloom)
    add_test(NAME c009-default-message-${peer}-modern-cpp-binding
        COMMAND ${peer}-default-message --api)
    add_test(NAME c009-default-message-${peer}-dynamic-type
        COMMAND ${peer}-default-message --dynamic-type)
    add_test(NAME c009-default-message-${peer}-registration-datawriter
        COMMAND ${peer}-default-message --registration)
    foreach(representation IN ITEMS xcdr1 xcdr2)
        add_test(NAME c009-default-message-${peer}-${representation}-cdr
            COMMAND ${peer}-default-message --cdr ${representation}
                "${CMAKE_CURRENT_BINARY_DIR}/${peer}-${representation}.bin")
    endforeach()
endforeach()

foreach(representation IN ITEMS xcdr1 xcdr2)
    add_test(NAME c009-default-message-${representation}-rti-wireloom-byte-equality
        COMMAND "${CMAKE_COMMAND}"
            "-DRTI_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/rti-${representation}.bin"
            "-DWIRELOOM_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/wireloom-${representation}.bin"
            -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
    set_tests_properties(c009-default-message-${representation}-rti-wireloom-byte-equality PROPERTIES
        DEPENDS "c009-default-message-rti-${representation}-cdr;c009-default-message-wireloom-${representation}-cdr")
endforeach()

if(UNIX)
    add_test(NAME c009-default-message-live-dds-cross-peer-matrix
        COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
            "$<TARGET_FILE:rti-default-message>"
            "$<TARGET_FILE:wireloom-default-message>"
            "${CMAKE_CURRENT_BINARY_DIR}/live")
endif()
