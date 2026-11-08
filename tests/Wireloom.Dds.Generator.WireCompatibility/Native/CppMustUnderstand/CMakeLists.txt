cmake_minimum_required(VERSION 3.20)
project(WireloomCppMustUnderstand LANGUAGES CXX)

enable_testing()
list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/09-must-understand")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/09-must-understand"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_sample_peer name source_directory)
    add_executable(${name} peer.cpp
        "${source_directory}/09-must-understand.cxx"
        "${source_directory}/09-must-understandPlugin.cxx")
    target_include_directories(${name} PRIVATE "${source_directory}")
    target_link_libraries(${name} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${name} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_sample_peer(rti-must-understand-sample "${oracle}")
add_sample_peer(wireloom-must-understand-sample "${WIRELOOM_CPP_GENERATED_DIR}")

foreach(peer IN ITEMS rti wireloom)
    add_test(NAME c142-must-understand-sample-${peer}-modern-cpp-binding
        COMMAND ${peer}-must-understand-sample --api)
    add_test(NAME c142-must-understand-sample-${peer}-dynamic-type
        COMMAND ${peer}-must-understand-sample --dynamic-type)
    add_test(NAME c142-must-understand-sample-${peer}-registration-datawriter
        COMMAND ${peer}-must-understand-sample --registration)
    foreach(representation IN ITEMS xcdr1 xcdr2)
        add_test(NAME c142-must-understand-sample-${peer}-${representation}-cdr
            COMMAND ${peer}-must-understand-sample --cdr ${representation}
                "${CMAKE_CURRENT_BINARY_DIR}/${peer}-${representation}.bin")
    endforeach()
endforeach()

foreach(representation IN ITEMS xcdr1 xcdr2)
    add_test(NAME c142-must-understand-sample-${representation}-rti-wireloom-byte-equality
        COMMAND "${CMAKE_COMMAND}"
            "-DRTI_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/rti-${representation}.bin"
            "-DWIRELOOM_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/wireloom-${representation}.bin"
            -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
    set_tests_properties(c142-must-understand-sample-${representation}-rti-wireloom-byte-equality PROPERTIES
        DEPENDS "c142-must-understand-sample-rti-${representation}-cdr;c142-must-understand-sample-wireloom-${representation}-cdr")
endforeach()

if(UNIX)
    add_test(NAME c142-must-understand-sample-live-dds-cross-peer-matrix
        COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
            "$<TARGET_FILE:rti-must-understand-sample>"
            "$<TARGET_FILE:wireloom-must-understand-sample>"
            "${CMAKE_CURRENT_BINARY_DIR}/live")
endif()
