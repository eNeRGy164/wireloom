cmake_minimum_required(VERSION 3.20)
project(WireloomCppSpike LANGUAGES CXX)

find_package(WireloomCpp CONFIG REQUIRED)
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(idl "${repository}/docs/corpus/idl/features/10-annotations.idl")
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/10-annotations")

function(add_peer name)
    add_executable(${name} peer.cpp)
    set_target_properties(${name} PROPERTIES CXX_STANDARD 11 CXX_STANDARD_REQUIRED YES CXX_EXTENSIONS NO)
    target_link_libraries(${name} PRIVATE RTIConnextDDS::cpp2_api)
endfunction()

add_peer(rti-cpp-peer)
target_sources(rti-cpp-peer PRIVATE "${oracle}/10-annotations.cxx" "${oracle}/10-annotationsPlugin.cxx")
target_include_directories(rti-cpp-peer PRIVATE "${oracle}")

add_peer(wireloom-cpp-peer)
wireloom_add_idl(wireloom-cpp-peer IDL "${idl}")
get_target_property(generated wireloom-cpp-peer WIRELOOM_GENERATED_DIR)
file(WRITE "${CMAKE_CURRENT_BINARY_DIR}/wireloom-generated-dir.txt" "${generated}")
