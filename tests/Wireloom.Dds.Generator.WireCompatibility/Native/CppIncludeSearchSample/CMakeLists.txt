cmake_minimum_required(VERSION 3.20)
project(WireloomCppIncludeSearchSample LANGUAGES CXX)

enable_testing()
list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/11-include-search")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/11-include-search"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(configure_include_search_peer target source_directory)
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_executable(rti-include-search-sample peer.cpp
    "${oracle}/11-include-search.cxx"
    "${oracle}/11-include-searchPlugin.cxx"
    "${oracle}/includes/common.cxx"
    "${oracle}/includes/commonPlugin.cxx")
configure_include_search_peer(rti-include-search-sample "${oracle}")
target_include_directories(rti-include-search-sample PRIVATE "${oracle}/includes")

add_executable(wireloom-include-search-sample peer.cpp
    "${WIRELOOM_CPP_GENERATED_DIR}/11-include-search.cxx"
    "${WIRELOOM_CPP_GENERATED_DIR}/11-include-searchPlugin.cxx")
configure_include_search_peer(wireloom-include-search-sample "${WIRELOOM_CPP_GENERATED_DIR}")

foreach(peer IN ITEMS rti wireloom)
    add_test(NAME include-search-sample-${peer}-modern-cpp-binding
        COMMAND ${peer}-include-search-sample --api)
    add_test(NAME include-search-sample-${peer}-dynamic-type
        COMMAND ${peer}-include-search-sample --dynamic-type)
    add_test(NAME include-search-sample-${peer}-registration-datawriter
        COMMAND ${peer}-include-search-sample --registration)
    foreach(representation IN ITEMS xcdr1 xcdr2)
        add_test(NAME include-search-sample-${peer}-${representation}-cdr
            COMMAND ${peer}-include-search-sample --cdr ${representation}
                "${CMAKE_CURRENT_BINARY_DIR}/${peer}-${representation}.bin")
    endforeach()
endforeach()

foreach(representation IN ITEMS xcdr1 xcdr2)
    add_test(NAME include-search-sample-${representation}-rti-wireloom-byte-equality
        COMMAND "${CMAKE_COMMAND}"
            "-DRTI_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/rti-${representation}.bin"
            "-DWIRELOOM_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/wireloom-${representation}.bin"
            -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
    set_tests_properties(include-search-sample-${representation}-rti-wireloom-byte-equality PROPERTIES
        DEPENDS "include-search-sample-rti-${representation}-cdr;include-search-sample-wireloom-${representation}-cdr")
endforeach()

if(UNIX)
    add_test(NAME include-search-sample-live-dds-cross-peer-matrix
        COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
            "$<TARGET_FILE:rti-include-search-sample>"
            "$<TARGET_FILE:wireloom-include-search-sample>"
            "${CMAKE_CURRENT_BINARY_DIR}/live")
endif()
