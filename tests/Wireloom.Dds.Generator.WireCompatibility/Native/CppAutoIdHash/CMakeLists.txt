cmake_minimum_required(VERSION 3.20)
project(WireloomCppAutoIdHash LANGUAGES CXX)

enable_testing()
list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/09-autoid-hash")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/09-autoid-hash"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_autoid_peer name source_directory)
    add_executable(${name} peer.cpp
        "${source_directory}/09-autoid-hash.cxx"
        "${source_directory}/09-autoid-hashPlugin.cxx")
    target_include_directories(${name} PRIVATE "${source_directory}")
    target_link_libraries(${name} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${name} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_autoid_peer(rti-autoid-hash-sample "${oracle}")
add_autoid_peer(wireloom-autoid-hash-sample "${WIRELOOM_CPP_GENERATED_DIR}")

foreach(peer IN ITEMS rti wireloom)
    add_test(NAME c039-autoid-hash-sample-${peer}-modern-cpp-binding
        COMMAND ${peer}-autoid-hash-sample --api)
    add_test(NAME c039-autoid-hash-sample-${peer}-dynamic-type
        COMMAND ${peer}-autoid-hash-sample --dynamic-type)
    add_test(NAME c039-autoid-hash-sample-${peer}-registration-datawriter
        COMMAND ${peer}-autoid-hash-sample --registration)
    foreach(representation IN ITEMS xcdr1 xcdr2)
        add_test(NAME c039-autoid-hash-sample-${peer}-${representation}-cdr
            COMMAND ${peer}-autoid-hash-sample --cdr ${representation}
                "${CMAKE_CURRENT_BINARY_DIR}/${peer}-${representation}.bin")
    endforeach()
endforeach()

foreach(representation IN ITEMS xcdr1 xcdr2)
    add_test(NAME c039-autoid-hash-sample-${representation}-rti-wireloom-byte-equality
        COMMAND "${CMAKE_COMMAND}"
            "-DRTI_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/rti-${representation}.bin"
            "-DWIRELOOM_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/wireloom-${representation}.bin"
            -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
    set_tests_properties(c039-autoid-hash-sample-${representation}-rti-wireloom-byte-equality PROPERTIES
        DEPENDS "c039-autoid-hash-sample-rti-${representation}-cdr;c039-autoid-hash-sample-wireloom-${representation}-cdr")
endforeach()

if(UNIX)
    add_test(NAME c039-autoid-hash-sample-live-dds-cross-peer-matrix
        COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
            "$<TARGET_FILE:rti-autoid-hash-sample>"
            "$<TARGET_FILE:wireloom-autoid-hash-sample>"
            "${CMAKE_CURRENT_BINARY_DIR}/live")
endif()
