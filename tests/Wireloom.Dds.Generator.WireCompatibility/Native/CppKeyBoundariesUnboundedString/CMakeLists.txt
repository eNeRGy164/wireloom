cmake_minimum_required(VERSION 3.20)
project(WireloomCppKeyBoundariesUnboundedString LANGUAGES CXX)

enable_testing()
list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/08-key-boundaries")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/08-key-boundaries"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_unbounded_string_peer name source_directory)
    add_executable(${name} peer.cpp
        "${source_directory}/08-key-boundaries.cxx"
        "${source_directory}/08-key-boundariesPlugin.cxx")
    target_include_directories(${name} PRIVATE "${source_directory}")
    target_link_libraries(${name} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${name} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_unbounded_string_peer(rti-key-boundaries-unbounded-string "${oracle}")
add_unbounded_string_peer(wireloom-key-boundaries-unbounded-string "${WIRELOOM_CPP_GENERATED_DIR}")

foreach(peer IN ITEMS rti wireloom)
    add_test(NAME c008-unbounded-string-key-${peer}-modern-cpp-binding COMMAND ${peer}-key-boundaries-unbounded-string --api)
    add_test(NAME c008-unbounded-string-key-${peer}-dynamic-type COMMAND ${peer}-key-boundaries-unbounded-string --dynamic-type)
    add_test(NAME c008-unbounded-string-key-${peer}-registration-datawriter COMMAND ${peer}-key-boundaries-unbounded-string --registration)
    foreach(representation IN ITEMS xcdr1 xcdr2)
        add_test(NAME c008-unbounded-string-key-${peer}-${representation}-cdr
            COMMAND ${peer}-key-boundaries-unbounded-string --cdr ${representation}
                "${CMAKE_CURRENT_BINARY_DIR}/${peer}-${representation}.bin")
    endforeach()
    foreach(length IN ITEMS 255 256)
        add_test(NAME c008-unbounded-string-key-${peer}-length-${length}-probe
            COMMAND ${peer}-key-boundaries-unbounded-string --probe ${length}
                "${CMAKE_CURRENT_BINARY_DIR}/${peer}-length-${length}.txt")
    endforeach()
endforeach()

foreach(representation IN ITEMS xcdr1 xcdr2)
    add_test(NAME c008-unbounded-string-key-${representation}-rti-wireloom-byte-equality
        COMMAND "${CMAKE_COMMAND}"
            "-DRTI_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/rti-${representation}.bin"
            "-DWIRELOOM_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/wireloom-${representation}.bin"
            -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
    set_tests_properties(c008-unbounded-string-key-${representation}-rti-wireloom-byte-equality PROPERTIES
        DEPENDS "c008-unbounded-string-key-rti-${representation}-cdr;c008-unbounded-string-key-wireloom-${representation}-cdr")
endforeach()

foreach(length IN ITEMS 255 256)
    add_test(NAME c008-unbounded-string-key-length-${length}-rti-wireloom-boundary-equality
        COMMAND "${CMAKE_COMMAND}"
            "-DRTI_STATUS=${CMAKE_CURRENT_BINARY_DIR}/rti-length-${length}.txt"
            "-DWIRELOOM_STATUS=${CMAKE_CURRENT_BINARY_DIR}/wireloom-length-${length}.txt"
            -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-boundary.cmake")
    set_tests_properties(c008-unbounded-string-key-length-${length}-rti-wireloom-boundary-equality PROPERTIES
        DEPENDS "c008-unbounded-string-key-rti-length-${length}-probe;c008-unbounded-string-key-wireloom-length-${length}-probe")
endforeach()

if(UNIX)
    add_test(NAME c008-unbounded-string-key-live-dds-cross-peer-matrix
        COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
            "$<TARGET_FILE:rti-key-boundaries-unbounded-string>"
            "$<TARGET_FILE:wireloom-key-boundaries-unbounded-string>"
            "${CMAKE_CURRENT_BINARY_DIR}/live")
endif()
