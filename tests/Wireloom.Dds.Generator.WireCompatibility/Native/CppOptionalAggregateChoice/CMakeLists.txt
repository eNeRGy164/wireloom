cmake_minimum_required(VERSION 3.20)
project(WireloomCppOptionalAggregateChoice LANGUAGES CXX)

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
add_peer(rti-cpp-optional-aggregate-choice "${oracle}")
add_peer(wireloom-cpp-optional-aggregate-choice "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
foreach(peer IN ITEMS rti wireloom)
    add_test(NAME ${peer}-optional-aggregate-api COMMAND ${peer}-cpp-optional-aggregate-choice --api)
    add_test(NAME ${peer}-optional-aggregate-dynamic-type COMMAND ${peer}-cpp-optional-aggregate-choice --dynamic-type)
    add_test(NAME ${peer}-optional-aggregate-registration COMMAND ${peer}-cpp-optional-aggregate-choice --registration)
endforeach()
set_tests_properties(
    rti-optional-aggregate-api rti-optional-aggregate-dynamic-type rti-optional-aggregate-registration
    wireloom-optional-aggregate-api wireloom-optional-aggregate-dynamic-type wireloom-optional-aggregate-registration
    PROPERTIES TIMEOUT 15)
foreach(shape IN ITEMS defaults payload number)
    add_test(NAME rti-optional-aggregate-cdr-${shape} COMMAND rti-cpp-optional-aggregate-choice --cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-${shape}.cdr" ${shape})
    add_test(NAME wireloom-optional-aggregate-cdr-${shape} COMMAND wireloom-cpp-optional-aggregate-choice --cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-${shape}.cdr" ${shape})
    add_test(NAME optional-aggregate-cdr-equality-${shape} COMMAND "${CMAKE_COMMAND}" -E compare_files "${CMAKE_CURRENT_BINARY_DIR}/rti-${shape}.cdr" "${CMAKE_CURRENT_BINARY_DIR}/wireloom-${shape}.cdr")
    set_tests_properties(optional-aggregate-cdr-equality-${shape} PROPERTIES DEPENDS "rti-optional-aggregate-cdr-${shape};wireloom-optional-aggregate-cdr-${shape}")
endforeach()
foreach(shape IN ITEMS defaults payload number)
    set_tests_properties(
        rti-optional-aggregate-cdr-${shape}
        wireloom-optional-aggregate-cdr-${shape}
        optional-aggregate-cdr-equality-${shape}
        PROPERTIES TIMEOUT 15)
endforeach()
