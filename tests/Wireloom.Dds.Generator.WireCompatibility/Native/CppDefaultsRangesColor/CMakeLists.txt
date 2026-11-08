cmake_minimum_required(VERSION 3.20)
project(WireloomCppDefaultsRangesColor LANGUAGES CXX)

enable_testing()
list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/09-defaults-ranges")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/09-defaults-ranges"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_color_peer name source_directory)
    add_executable(${name} peer.cpp
        "${source_directory}/09-defaults-ranges.cxx"
        "${source_directory}/09-defaults-rangesPlugin.cxx")
    target_include_directories(${name} PRIVATE "${source_directory}")
    target_link_libraries(${name} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${name} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_color_peer(rti-defaults-ranges-color "${oracle}")
add_color_peer(wireloom-defaults-ranges-color "${WIRELOOM_CPP_GENERATED_DIR}")

foreach(peer IN ITEMS rti wireloom)
    add_test(NAME c040-defaults-ranges-color-${peer}-modern-cpp-api
        COMMAND ${peer}-defaults-ranges-color --api "${CMAKE_CURRENT_BINARY_DIR}/${peer}-api.txt" "${CMAKE_CURRENT_BINARY_DIR}/${peer}-stream.txt")
    add_test(NAME c040-defaults-ranges-color-${peer}-dynamic-type
        COMMAND ${peer}-defaults-ranges-color --dynamic-type "${CMAKE_CURRENT_BINARY_DIR}/${peer}-metadata.txt")
endforeach()

foreach(kind IN ITEMS api metadata stream)
    add_test(NAME c040-defaults-ranges-color-${kind}-rti-wireloom-comparison
        COMMAND "${CMAKE_COMMAND}"
            "-DRTI_FILE=${CMAKE_CURRENT_BINARY_DIR}/rti-${kind}.txt"
            "-DWIRELOOM_FILE=${CMAKE_CURRENT_BINARY_DIR}/wireloom-${kind}.txt"
            "-DREASON=${kind}"
            -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-file.cmake")
    if(kind STREQUAL "api")
        set_tests_properties(c040-defaults-ranges-color-${kind}-rti-wireloom-comparison PROPERTIES
            DEPENDS "c040-defaults-ranges-color-rti-modern-cpp-api;c040-defaults-ranges-color-wireloom-modern-cpp-api")
    elseif(kind STREQUAL "metadata")
        set_tests_properties(c040-defaults-ranges-color-${kind}-rti-wireloom-comparison PROPERTIES
            DEPENDS "c040-defaults-ranges-color-rti-dynamic-type;c040-defaults-ranges-color-wireloom-dynamic-type")
    else()
        set_tests_properties(c040-defaults-ranges-color-${kind}-rti-wireloom-comparison PROPERTIES
            DEPENDS "c040-defaults-ranges-color-rti-modern-cpp-api;c040-defaults-ranges-color-wireloom-modern-cpp-api")
    endif()
endforeach()
