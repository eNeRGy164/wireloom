cmake_minimum_required(VERSION 3.20)
project(WireloomCppPreprocessingSample LANGUAGES CXX)

enable_testing()
list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/11-preprocessing")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/11-preprocessing"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_preprocessing_peer target source_directory)
    set(sources peer.cpp
        "${source_directory}/11-preprocessing.cxx"
        "${source_directory}/11-preprocessingPlugin.cxx")
    if(EXISTS "${source_directory}/includes/common.cxx")
        list(APPEND sources
            "${source_directory}/includes/common.cxx"
            "${source_directory}/includes/commonPlugin.cxx")
    endif()
    add_executable(${target} ${sources})
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_preprocessing_peer(rti-preprocessing-sample "${oracle}")
add_preprocessing_peer(wireloom-preprocessing-sample "${WIRELOOM_CPP_GENERATED_DIR}")

foreach(peer IN ITEMS rti wireloom)
    add_test(NAME preprocessing-sample-${peer}-modern-cpp-binding
        COMMAND ${peer}-preprocessing-sample --api)
    add_test(NAME preprocessing-sample-${peer}-dynamic-type
        COMMAND ${peer}-preprocessing-sample --dynamic-type)
    add_test(NAME preprocessing-sample-${peer}-registration-datawriter
        COMMAND ${peer}-preprocessing-sample --registration)
    foreach(representation IN ITEMS xcdr1 xcdr2)
        add_test(NAME preprocessing-sample-${peer}-${representation}-cdr
            COMMAND ${peer}-preprocessing-sample --cdr ${representation}
                "${CMAKE_CURRENT_BINARY_DIR}/${peer}-${representation}.bin")
    endforeach()
endforeach()

foreach(representation IN ITEMS xcdr1 xcdr2)
    add_test(NAME preprocessing-sample-${representation}-rti-wireloom-byte-equality
        COMMAND "${CMAKE_COMMAND}"
            "-DRTI_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/rti-${representation}.bin"
            "-DWIRELOOM_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/wireloom-${representation}.bin"
            -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
    set_tests_properties(preprocessing-sample-${representation}-rti-wireloom-byte-equality PROPERTIES
        DEPENDS "preprocessing-sample-rti-${representation}-cdr;preprocessing-sample-wireloom-${representation}-cdr")
endforeach()

if(UNIX)
    add_test(NAME preprocessing-sample-live-dds-cross-peer-matrix
        COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
            "$<TARGET_FILE:rti-preprocessing-sample>"
            "$<TARGET_FILE:wireloom-preprocessing-sample>"
            "${CMAKE_CURRENT_BINARY_DIR}/live")
endif()
