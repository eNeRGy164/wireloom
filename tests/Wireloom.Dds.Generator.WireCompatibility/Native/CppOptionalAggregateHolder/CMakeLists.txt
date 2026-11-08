cmake_minimum_required(VERSION 3.20)
project(WireloomCppOptionalAggregateHolder LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)
get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/09-optional-aggregate-member")
set(WIRELOOM_CPP_GENERATED_DIR "${repository}/docs/corpus/7.7.0/generator/cpp/features/09-optional-aggregate-member"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_peer target source)
    add_executable(${target} peer.cpp "${source}/09-optional-aggregate-member.cxx" "${source}/09-optional-aggregate-memberPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES CXX_STANDARD 11 CXX_STANDARD_REQUIRED YES CXX_EXTENSIONS NO)
endfunction()
add_peer(rti-cpp-optional-aggregate-holder "${oracle}")
add_peer(wireloom-cpp-optional-aggregate-holder "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
foreach(peer IN ITEMS rti wireloom)
    add_test(NAME ${peer}-optional-holder-api COMMAND ${peer}-cpp-optional-aggregate-holder --api)
    add_test(NAME ${peer}-optional-holder-dynamic-type COMMAND ${peer}-cpp-optional-aggregate-holder --dynamic-type)
    add_test(NAME ${peer}-optional-holder-registration COMMAND ${peer}-cpp-optional-aggregate-holder --registration)
endforeach()
foreach(shape IN ITEMS absent payload payload-alias choice-payload choice-number choice-alias)
    add_test(NAME rti-optional-holder-cdr-${shape} COMMAND rti-cpp-optional-aggregate-holder --cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-${shape}.cdr" ${shape})
    add_test(NAME wireloom-optional-holder-cdr-${shape} COMMAND wireloom-cpp-optional-aggregate-holder --cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-${shape}.cdr" ${shape})
    add_test(NAME optional-holder-cdr-equality-${shape} COMMAND "${CMAKE_COMMAND}" -E compare_files "${CMAKE_CURRENT_BINARY_DIR}/rti-${shape}.cdr" "${CMAKE_CURRENT_BINARY_DIR}/wireloom-${shape}.cdr")
    set_tests_properties(optional-holder-cdr-equality-${shape} PROPERTIES DEPENDS "rti-optional-holder-cdr-${shape};wireloom-optional-holder-cdr-${shape}")
endforeach()
set_tests_properties(
    rti-optional-holder-api rti-optional-holder-dynamic-type rti-optional-holder-registration
    wireloom-optional-holder-api wireloom-optional-holder-dynamic-type wireloom-optional-holder-registration
    PROPERTIES TIMEOUT 15)
foreach(shape IN ITEMS absent payload payload-alias choice-payload choice-number choice-alias)
    set_tests_properties(rti-optional-holder-cdr-${shape} wireloom-optional-holder-cdr-${shape} optional-holder-cdr-equality-${shape} PROPERTIES TIMEOUT 15)
endforeach()
