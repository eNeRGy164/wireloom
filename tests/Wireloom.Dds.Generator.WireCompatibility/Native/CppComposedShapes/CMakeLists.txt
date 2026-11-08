cmake_minimum_required(VERSION 3.20)
project(WireloomCppComposedShapes LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/05-shapes")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/05-shapes"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_composed_shapes_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/05-shapes.cxx"
        "${source_directory}/05-shapesPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_composed_shapes_peer(rti-cpp-composed-shapes "${oracle}")
add_composed_shapes_peer(wireloom-cpp-composed-shapes "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
add_test(NAME wireloom-composed-shapes-registration
    COMMAND wireloom-cpp-composed-shapes --register)

foreach(shape IN ITEMS sequence-empty sequence-unequal sequence-mixed-empty sequence-boundary)
    set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-composed-shapes-${shape}.cdr")
    set(wireloom_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-composed-shapes-${shape}.cdr")
    add_test(NAME rti-composed-shapes-cdr-${shape}
        COMMAND rti-cpp-composed-shapes "${rti_cdr}" "${shape}")
    add_test(NAME wireloom-composed-shapes-cdr-${shape}
        COMMAND wireloom-cpp-composed-shapes "${wireloom_cdr}" "${shape}")
    add_test(NAME composed-shapes-cdr-equality-${shape}
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${wireloom_cdr}")
    set_tests_properties(composed-shapes-cdr-equality-${shape} PROPERTIES
        DEPENDS "rti-composed-shapes-cdr-${shape};wireloom-composed-shapes-cdr-${shape}")
endforeach()
