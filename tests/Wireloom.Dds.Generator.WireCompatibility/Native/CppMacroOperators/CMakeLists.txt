cmake_minimum_required(VERSION 3.20)
project(WireloomCppMacroOperators LANGUAGES CXX)

enable_testing()
list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/11-macro-operators")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/11-macro-operators"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_macro_operators_peer target source_directory name_type)
    add_executable(${target} peer.cpp
        "${source_directory}/11-macro-operators.cxx"
        "${source_directory}/11-macro-operatorsPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_compile_definitions(${target} PRIVATE ${name_type})
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_macro_operators_peer(rti-macro-operators "${oracle}" EXPECT_RTI_NAME_STRING_VIEW)
add_macro_operators_peer(wireloom-macro-operators "${WIRELOOM_CPP_GENERATED_DIR}" EXPECT_WIRELOOM_NAME_CSTRING)

foreach(peer IN ITEMS rti wireloom)
    add_test(NAME macro-operators-${peer}-modern-cpp-binding
        COMMAND ${peer}-macro-operators --api)
    add_test(NAME macro-operators-${peer}-dynamic-type
        COMMAND ${peer}-macro-operators --dynamic-type)
    add_test(NAME macro-operators-${peer}-registration-datawriter
        COMMAND ${peer}-macro-operators --registration)
    foreach(representation IN ITEMS xcdr1 xcdr2)
        add_test(NAME macro-operators-${peer}-${representation}-cdr
            COMMAND ${peer}-macro-operators --cdr ${representation}
                "${CMAKE_CURRENT_BINARY_DIR}/${peer}-${representation}.bin")
    endforeach()
endforeach()

foreach(representation IN ITEMS xcdr1 xcdr2)
    add_test(NAME macro-operators-${representation}-rti-wireloom-byte-equality
        COMMAND "${CMAKE_COMMAND}"
            "-DRTI_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/rti-${representation}.bin"
            "-DWIRELOOM_CAPTURE=${CMAKE_CURRENT_BINARY_DIR}/wireloom-${representation}.bin"
            -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
    set_tests_properties(macro-operators-${representation}-rti-wireloom-byte-equality PROPERTIES
        DEPENDS "macro-operators-rti-${representation}-cdr;macro-operators-wireloom-${representation}-cdr")
endforeach()

if(UNIX)
    add_test(NAME macro-operators-live-dds-cross-peer-matrix
        COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
            "$<TARGET_FILE:rti-macro-operators>"
            "$<TARGET_FILE:wireloom-macro-operators>"
            "${CMAKE_CURRENT_BINARY_DIR}/live")
endif()
