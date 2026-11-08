if(NOT EXISTS "${RTI_CAPTURE}" OR NOT EXISTS "${WIRELOOM_CAPTURE}")
    message(FATAL_ERROR "Both RTI and Wireloom CDR captures must exist")
endif()
execute_process(COMMAND "${CMAKE_COMMAND}" -E compare_files "${RTI_CAPTURE}" "${WIRELOOM_CAPTURE}"
    RESULT_VARIABLE result)
if(NOT result EQUAL 0)
    message(FATAL_ERROR "RTI and Wireloom CDR capture bytes differ")
endif()
message(STATUS "RTI and Wireloom CDR capture bytes match")
