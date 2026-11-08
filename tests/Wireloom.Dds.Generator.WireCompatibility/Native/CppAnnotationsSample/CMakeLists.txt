cmake_minimum_required(VERSION 3.20)
project(WireloomCppAnnotationsSample LANGUAGES CXX)

enable_testing()
list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/10-annotations")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/10-annotations"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_annotations_peer target source_directory)
    add_executable(${target} peer.cpp
        "${source_directory}/10-annotations.cxx"
        "${source_directory}/10-annotationsPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_annotations_peer(rti-annotations-sample "${oracle}")
add_annotations_peer(wireloom-annotations-sample "${WIRELOOM_CPP_GENERATED_DIR}")

foreach(peer IN ITEMS rti wireloom)
    add_test(NAME annotations-sample-${peer}-modern-cpp-binding
        COMMAND ${peer}-annotations-sample --api)
    add_test(NAME annotations-sample-${peer}-dynamic-type
        COMMAND ${peer}-annotations-sample --dynamic-type)
    add_test(NAME annotations-sample-${peer}-registration-datawriter
        COMMAND ${peer}-annotations-sample --registration)
    foreach(representation IN ITEMS xcdr1 xcdr2)
        add_test(NAME annotations-sample-${peer}-${representation}-cdr
            COMMAND ${peer}-annotations-sample --cdr ${representation}
                "${CMAKE_CURRENT_BINARY_DIR}/${peer}-${representation}.bin")
    endforeach()
endforeach()

foreach(representation IN ITEMS xcdr1 xcdr2)
    add_test(NAME annotations-sample-${representation}-rti-wireloom-byte-equality
        COMMAND "${CMAKE_COMMAND}"
            "-DRTI_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/rti-${representation}.bin"
            "-DWIRELOOM_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/wireloom-${representation}.bin"
            -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
    set_tests_properties(annotations-sample-${representation}-rti-wireloom-byte-equality PROPERTIES
        DEPENDS "annotations-sample-rti-${representation}-cdr;annotations-sample-wireloom-${representation}-cdr")
endforeach()

if(UNIX)
    add_test(NAME annotations-sample-live-dds-cross-peer-matrix
        COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
            "$<TARGET_FILE:rti-annotations-sample>"
            "$<TARGET_FILE:wireloom-annotations-sample>"
            "${CMAKE_CURRENT_BINARY_DIR}/live")
endif()
