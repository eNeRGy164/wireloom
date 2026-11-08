cmake_minimum_required(VERSION 3.20)
project(WireloomCppFinal LANGUAGES CXX)

enable_testing()
list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/09-extensibility")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/09-extensibility"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_final_peer name source_directory)
    add_executable(${name} peer.cpp
        "${source_directory}/09-extensibility.cxx"
        "${source_directory}/09-extensibilityPlugin.cxx")
    target_include_directories(${name} PRIVATE "${source_directory}")
    target_link_libraries(${name} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${name} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_final_peer(rti-final "${oracle}")
add_final_peer(wireloom-final "${WIRELOOM_CPP_GENERATED_DIR}")

foreach(peer IN ITEMS rti wireloom)
    add_test(NAME c009-final-${peer}-modern-cpp-binding COMMAND ${peer}-final --api)
    add_test(NAME c009-final-${peer}-dynamic-type COMMAND ${peer}-final --dynamic-type)
    add_test(NAME c009-final-${peer}-registration-datawriter COMMAND ${peer}-final --registration)
    foreach(representation IN ITEMS xcdr1 xcdr2)
        add_test(NAME c009-final-${peer}-${representation}-cdr
            COMMAND ${peer}-final --cdr ${representation}
                "${CMAKE_CURRENT_BINARY_DIR}/${peer}-${representation}.bin")
    endforeach()
endforeach()

foreach(representation IN ITEMS xcdr1 xcdr2)
    add_test(NAME c009-final-${representation}-rti-wireloom-byte-equality
        COMMAND "${CMAKE_COMMAND}"
            "-DRTI_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/rti-${representation}.bin"
            "-DWIRELOOM_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/wireloom-${representation}.bin"
            -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
    set_tests_properties(c009-final-${representation}-rti-wireloom-byte-equality PROPERTIES
        DEPENDS "c009-final-rti-${representation}-cdr;c009-final-wireloom-${representation}-cdr")
endforeach()

if(UNIX)
    add_test(NAME c009-final-live-dds-cross-peer-matrix
        COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
            "$<TARGET_FILE:rti-final>"
            "$<TARGET_FILE:wireloom-final>"
            "${CMAKE_CURRENT_BINARY_DIR}/live")
endif()
