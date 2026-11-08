execute_process(
    COMMAND "${CMAKE_COMMAND}" -E compare_files
        "${RTI_CONTRACT}/stream.txt" "${WIRELOOM_CONTRACT}/stream.txt"
    RESULT_VARIABLE comparison)
if(NOT comparison EQUAL 0)
    message(FATAL_ERROR "RTI and Wireloom enum stream output differs")
endif()
