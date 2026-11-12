cmake_minimum_required(VERSION 3.20)
project(WireloomCppPackageCheck LANGUAGES CXX)

find_package(WireloomCpp CONFIG REQUIRED)
add_library(contract OBJECT EXCLUDE_FROM_ALL)
wireloom_add_idl(contract IDL "${WIRELOOM_IDL}"
    DEFINES WIRELOOM_PACKAGE_TEST=1
    UNDEFINES WIRELOOM_PACKAGE_TEST
    INCLUDE_DIRECTORIES "${CMAKE_CURRENT_SOURCE_DIR}"
    SEQUENCE_SIZE 12
    STRING_SIZE 42
    FILE_SEQUENCE_SIZES "${WIRELOOM_IDL}" 24
    FILE_STRING_SIZES "${WIRELOOM_IDL}" 48
    FILE_UNBOUNDED_SUPPORT "${WIRELOOM_IDL}" false)
get_target_property(generated contract WIRELOOM_GENERATED_DIR)
get_filename_component(idl_name "${WIRELOOM_IDL}" NAME_WLE)
add_custom_target(verify ALL DEPENDS
    "${generated}/${idl_name}.hpp"
    "${generated}/${idl_name}.cxx"
    "${generated}/${idl_name}Plugin.hpp"
    "${generated}/${idl_name}Plugin.cxx")
