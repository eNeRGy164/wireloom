cmake_minimum_required(VERSION 3.20)
project(WireloomCppCollections LANGUAGES CXX)

list(APPEND CMAKE_MODULE_PATH "$ENV{NDDSHOME}/resource/cmake")
find_package(RTIConnextDDS 7.7.0 EXACT REQUIRED)

get_filename_component(repository "${CMAKE_CURRENT_SOURCE_DIR}/../../../.." ABSOLUTE)
set(oracle "${repository}/docs/corpus/7.7.0/rtiddsgen/cpp/features/05-collections")
set(WIRELOOM_CPP_GENERATED_DIR
    "${repository}/docs/corpus/7.7.0/generator/cpp/features/05-collections"
    CACHE PATH "Directory containing retained Wireloom C++ output")

function(add_collections_peer target source_directory)
    add_executable(${target}
        peer.cpp
        "${source_directory}/05-collections.cxx"
        "${source_directory}/05-collectionsPlugin.cxx")
    target_include_directories(${target} PRIVATE "${source_directory}")
    target_link_libraries(${target} PRIVATE RTIConnextDDS::cpp2_api)
    set_target_properties(${target} PROPERTIES
        CXX_STANDARD 11
        CXX_STANDARD_REQUIRED YES
        CXX_EXTENSIONS NO)
endfunction()

add_collections_peer(rti-cpp-collections "${oracle}")
add_collections_peer(wireloom-cpp-collections "${WIRELOOM_CPP_GENERATED_DIR}")

enable_testing()
add_test(NAME wireloom-collections-registration COMMAND wireloom-cpp-collections --register)
foreach(shape IN ITEMS empty single full)
    set(rti_cdr "${CMAKE_CURRENT_BINARY_DIR}/rti-collections-${shape}.cdr")
    set(wireloom_cdr "${CMAKE_CURRENT_BINARY_DIR}/wireloom-collections-${shape}.cdr")
    add_test(NAME rti-collections-cdr-${shape}
        COMMAND rti-cpp-collections "${rti_cdr}" "${shape}")
    add_test(NAME wireloom-collections-cdr-${shape}
        COMMAND wireloom-cpp-collections "${wireloom_cdr}" "${shape}")
    add_test(NAME collections-cdr-equality-${shape}
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_cdr}" "${wireloom_cdr}")
    set_tests_properties(collections-cdr-equality-${shape} PROPERTIES
        DEPENDS "rti-collections-cdr-${shape};wireloom-collections-cdr-${shape}")
endforeach()
