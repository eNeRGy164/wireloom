cmake_minimum_required(VERSION 3.20)
project(WireloomCppFirstCase LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

option(WIRELOOM_CPP_FIRST_CASE_RTI_ONLY "Build only the retained RTI control peer" OFF)
if(NOT WIRELOOM_CPP_FIRST_CASE_RTI_ONLY)
    find_package(WireloomCpp CONFIG REQUIRED)
endif()

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(idl "${repository}/docs/corpus/idl/features/01-primitives.idl")
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/01-primitives")

function(add_first_case_peer name)
    add_executable(${name} peer.cpp)
    set_target_properties(${name} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
    target_link_libraries(${name} PRIVATE RTIConnextDDS::cpp2_api)
endfunction()

add_first_case_peer(rti-cpp-first-case)
target_sources(rti-cpp-first-case PRIVATE
    "${oracle}/01-primitives.cxx"
    "${oracle}/01-primitivesPlugin.cxx")
target_include_directories(rti-cpp-first-case PRIVATE "${oracle}")

if(NOT WIRELOOM_CPP_FIRST_CASE_RTI_ONLY)
    add_first_case_peer(wireloom-cpp-first-case)
    wireloom_add_idl(wireloom-cpp-first-case IDL "${idl}")
    get_target_property(generated wireloom-cpp-first-case WIRELOOM_GENERATED_DIR)
    file(WRITE "${CMAKE_CURRENT_BINARY_DIR}/wireloom-generated-dir.txt" "${generated}")
endif()
