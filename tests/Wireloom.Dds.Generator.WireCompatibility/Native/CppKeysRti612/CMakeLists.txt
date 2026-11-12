cmake_minimum_required(VERSION 3.20)
project(WireloomCppKeysRti612 LANGUAGES CXX)

enable_testing()
list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 6.1.2 EXACT REQUIRED)

set(WIRELOOM_CPP_GENERATED_DIR "" CACHE PATH "Directory containing generated RTI 6.1.2 Wireloom C++ output")
if(NOT WIRELOOM_CPP_GENERATED_DIR)
    message(FATAL_ERROR "Set WIRELOOM_CPP_GENERATED_DIR to the output of Wireloom C++ generation for RTI 6.1.2.")
endif()

foreach(required_file IN ITEMS
    keys.hpp keys.cxx keysPlugin.cxx
    collections.hpp collections.cxx collectionsPlugin.cxx
    optional-collections.hpp optional-collections.cxx optional-collectionsPlugin.cxx
    unions.hpp unions.cxx unionsPlugin.cxx)
    if(NOT EXISTS "${WIRELOOM_CPP_GENERATED_DIR}/${required_file}")
        message(FATAL_ERROR "Missing generated RTI 6.1.2 key fixture file: ${WIRELOOM_CPP_GENERATED_DIR}/${required_file}")
    endif()
endforeach()

add_executable(wireloom-cpp-rti612-keys
    peer.cpp
    "${WIRELOOM_CPP_GENERATED_DIR}/keys.cxx"
    "${WIRELOOM_CPP_GENERATED_DIR}/keysPlugin.cxx")
target_include_directories(wireloom-cpp-rti612-keys PRIVATE "${WIRELOOM_CPP_GENERATED_DIR}")
target_compile_features(wireloom-cpp-rti612-keys PRIVATE cxx_std_11)
target_link_libraries(wireloom-cpp-rti612-keys PRIVATE RTIConnextDDS::cpp2_api)

add_executable(wireloom-cpp-rti612-collections
    collections-peer.cpp
    "${WIRELOOM_CPP_GENERATED_DIR}/collections.cxx"
    "${WIRELOOM_CPP_GENERATED_DIR}/collectionsPlugin.cxx"
    "${WIRELOOM_CPP_GENERATED_DIR}/optional-collections.cxx"
    "${WIRELOOM_CPP_GENERATED_DIR}/optional-collectionsPlugin.cxx")
target_include_directories(wireloom-cpp-rti612-collections PRIVATE "${WIRELOOM_CPP_GENERATED_DIR}")
target_compile_features(wireloom-cpp-rti612-collections PRIVATE cxx_std_11)
target_link_libraries(wireloom-cpp-rti612-collections PRIVATE RTIConnextDDS::cpp2_api)

add_library(wireloom-cpp-rti612-union-compile EXCLUDE_FROM_ALL STATIC
    "${WIRELOOM_CPP_GENERATED_DIR}/unions.cxx"
    "${WIRELOOM_CPP_GENERATED_DIR}/unionsPlugin.cxx")
target_include_directories(wireloom-cpp-rti612-union-compile PRIVATE "${WIRELOOM_CPP_GENERATED_DIR}")
target_compile_features(wireloom-cpp-rti612-union-compile PRIVATE cxx_std_11)
target_link_libraries(wireloom-cpp-rti612-union-compile PRIVATE RTIConnextDDS::cpp2_api)

add_test(NAME wireloom-cpp-rti612-keys-instance-registration
    COMMAND wireloom-cpp-rti612-keys)
add_test(NAME wireloom-cpp-rti612-collection-cdr-roundtrip
    COMMAND wireloom-cpp-rti612-collections)
