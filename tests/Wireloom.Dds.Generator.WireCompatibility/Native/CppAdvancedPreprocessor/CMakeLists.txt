cmake_minimum_required(VERSION 3.20)
project(WireloomCppAdvancedPreprocessor LANGUAGES CXX)

enable_testing()
list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/11-preprocessor-advanced")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/11-preprocessor-advanced"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_advanced_preprocessor_peer target source_directory)
    add_executable(${target} peer.cpp
        "${source_directory}/11-preprocessor-advanced.cxx"
        "${source_directory}/11-preprocessor-advancedPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_advanced_preprocessor_peer(rti-advanced-preprocessor "${oracle}")
add_advanced_preprocessor_peer(wireloom-advanced-preprocessor "${WIRELOOM_CPP_GENERATED_DIR}")

foreach(peer IN ITEMS rti wireloom)
    add_test(NAME advanced-preprocessor-${peer}-modern-cpp-binding
        COMMAND ${peer}-advanced-preprocessor --api)
    add_test(NAME advanced-preprocessor-${peer}-dynamic-type
        COMMAND ${peer}-advanced-preprocessor --dynamic-type)
    add_test(NAME advanced-preprocessor-${peer}-registration-datawriter
        COMMAND ${peer}-advanced-preprocessor --registration)
    foreach(representation IN ITEMS xcdr1 xcdr2)
        add_test(NAME advanced-preprocessor-${peer}-${representation}-cdr
            COMMAND ${peer}-advanced-preprocessor --cdr ${representation}
                "${CMAKE_CURRENT_BINARY_DIR}/${peer}-${representation}.bin")
    endforeach()
endforeach()

foreach(representation IN ITEMS xcdr1 xcdr2)
    add_test(NAME advanced-preprocessor-${representation}-rti-wireloom-byte-equality
        COMMAND "${CMAKE_COMMAND}"
            "-DRTI_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/rti-${representation}.bin"
            "-DWIRELOOM_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/wireloom-${representation}.bin"
            -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
    set_tests_properties(advanced-preprocessor-${representation}-rti-wireloom-byte-equality PROPERTIES
        DEPENDS "advanced-preprocessor-rti-${representation}-cdr;advanced-preprocessor-wireloom-${representation}-cdr")
endforeach()

if(UNIX)
    add_test(NAME advanced-preprocessor-live-dds-cross-peer-matrix
        COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
            "$<TARGET_FILE:rti-advanced-preprocessor>"
            "$<TARGET_FILE:wireloom-advanced-preprocessor>"
            "${CMAKE_CURRENT_BINARY_DIR}/live")
endif()
