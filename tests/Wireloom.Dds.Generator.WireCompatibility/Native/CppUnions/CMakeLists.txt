cmake_minimum_required(VERSION 3.20)
project(WireloomCppUnions LANGUAGES CXX)

enable_testing()

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

option(WIRELOOM_CPP_UNIONS_RTI_ONLY "Build only the retained RTI control peer" OFF)
if(NOT WIRELOOM_CPP_UNIONS_RTI_ONLY)
    find_package(WireloomCpp CONFIG REQUIRED)
endif()

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(idl "${repository}/docs/corpus/idl/features/07-unions.idl")
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/07-unions")

function(add_unions_peer name)
    add_executable(${name} peer.cpp)
    set_target_properties(${name} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
    target_link_libraries(${name} PRIVATE RTIConnextDDS::cpp2_api)
endfunction()

add_unions_peer(rti-cpp-unions)
target_sources(rti-cpp-unions PRIVATE
    "${oracle}/07-unions.cxx"
    "${oracle}/07-unionsPlugin.cxx")
target_include_directories(rti-cpp-unions PRIVATE "${oracle}")

if(NOT WIRELOOM_CPP_UNIONS_RTI_ONLY)
    add_unions_peer(wireloom-cpp-unions)
    wireloom_add_idl(wireloom-cpp-unions IDL "${idl}")
    get_target_property(generated wireloom-cpp-unions WIRELOOM_GENERATED_DIR)
    file(WRITE "${CMAKE_CURRENT_BINARY_DIR}/wireloom-generated-dir.txt" "${generated}")

    add_test(NAME c022-rti-union-cdr
        COMMAND rti-cpp-unions "${CMAKE_CURRENT_BINARY_DIR}/rti-cdr.bin")
    add_test(NAME c022-wireloom-union-cdr
        COMMAND wireloom-cpp-unions "${CMAKE_CURRENT_BINARY_DIR}/wireloom-cdr.bin")
    add_test(NAME c022-compare-union-cdr
        COMMAND "${CMAKE_COMMAND}"
            -DRTI_CDR=${CMAKE_CURRENT_BINARY_DIR}/rti-cdr.bin
            -DWIRELOOM_CDR=${CMAKE_CURRENT_BINARY_DIR}/wireloom-cdr.bin
            -P "${CMAKE_CURRENT_SOURCE_DIR}/compare-cdr.cmake")
    set_tests_properties(c022-compare-union-cdr PROPERTIES
        DEPENDS "c022-rti-union-cdr;c022-wireloom-union-cdr")
endif()
