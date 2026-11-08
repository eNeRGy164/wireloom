cmake_minimum_required(VERSION 3.20)
project(WireloomCppPrimitives LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(first_case_peer "${repository}/tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppFirstCase/peer.cpp")
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/01-primitives")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/01-primitives"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_primitives_peer target source_directory)
    add_executable(${target}
        "${first_case_peer}"
        "${source_directory}/01-primitives.cxx"
        "${source_directory}/01-primitivesPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_primitives_peer(rti-cpp-primitives "${oracle}")
add_primitives_peer(wireloom-cpp-primitives "${WIRELOOM_CPP_GENERATED_DIR}")

add_executable(rti-primitives-registration registration.cpp "${oracle}/01-primitives.cxx" "${oracle}/01-primitivesPlugin.cxx")
target_include_directories(rti-primitives-registration PRIVATE "${oracle}")
target_link_libraries(rti-primitives-registration PRIVATE RTIConnextDDS::cpp2_api)

add_executable(wireloom-primitives-registration registration.cpp
    "${WIRELOOM_CPP_GENERATED_DIR}/01-primitives.cxx"
    "${WIRELOOM_CPP_GENERATED_DIR}/01-primitivesPlugin.cxx")
target_include_directories(wireloom-primitives-registration PRIVATE "${WIRELOOM_CPP_GENERATED_DIR}")
target_link_libraries(wireloom-primitives-registration PRIVATE RTIConnextDDS::cpp2_api)

foreach(target IN ITEMS rti-primitives-registration wireloom-primitives-registration)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endforeach()

enable_testing()
set(rti_contract "${CMAKE_CURRENT_BINARY_DIR}/contract/rti")
set(wireloom_contract "${CMAKE_CURRENT_BINARY_DIR}/contract/wireloom")
file(MAKE_DIRECTORY "${rti_contract}" "${wireloom_contract}")

add_test(NAME rti-primitives-api-metadata-cdr
    COMMAND rti-cpp-primitives contract "${rti_contract}")
add_test(NAME wireloom-primitives-api-metadata-cdr
    COMMAND wireloom-cpp-primitives contract "${wireloom_contract}")
add_test(NAME primitives-rti-wireloom-cdr-equality
    COMMAND "${CMAKE_COMMAND}"
        "-DRTI_CONTRACT=${rti_contract}"
        "-DWIRELOOM_CONTRACT=${wireloom_contract}"
        -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
set_tests_properties(primitives-rti-wireloom-cdr-equality PROPERTIES
    DEPENDS "rti-primitives-api-metadata-cdr;wireloom-primitives-api-metadata-cdr")

add_test(NAME rti-primitives-registration-datawriter COMMAND rti-primitives-registration)
add_test(NAME wireloom-primitives-registration-datawriter COMMAND wireloom-primitives-registration)

if(UNIX)
    add_test(NAME primitives-live-dds-cross-peer-matrix
        COMMAND bash "${CMAKE_CURRENT_SOURCE_DIR}/run-live-matrix.sh"
            "$<TARGET_FILE:rti-cpp-primitives>"
            "$<TARGET_FILE:wireloom-cpp-primitives>"
            "${CMAKE_CURRENT_BINARY_DIR}/live")
endif()
