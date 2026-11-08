foreach(peer IN ITEMS RTI_CONTRACT WIRELOOM_CONTRACT)
    if(NOT EXISTS "${${peer}}/metadata.txt")
        message(FATAL_ERROR "Missing ${peer} metadata output")
    endif()
endforeach()

execute_process(
    COMMAND "${CMAKE_COMMAND}" -E compare_files
        "${RTI_CONTRACT}/metadata.txt"
        "${WIRELOOM_CONTRACT}/metadata.txt"
    RESULT_VARIABLE result)
if(NOT result EQUAL 0)
    message(FATAL_ERROR "RTI and Wireloom enum DynamicType metadata or public enum values differ")
endif()
