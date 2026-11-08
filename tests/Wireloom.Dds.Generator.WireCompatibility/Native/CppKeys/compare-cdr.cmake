if(NOT EXISTS "${RTI_CDR}")
    message(FATAL_ERROR "RTI CDR output is missing: ${RTI_CDR}")
endif()
if(NOT EXISTS "${WIRELOOM_CDR}")
    message(FATAL_ERROR "Wireloom CDR output is missing: ${WIRELOOM_CDR}")
endif()
execute_process(
    COMMAND "${CMAKE_COMMAND}" -E compare_files "${RTI_CDR}" "${WIRELOOM_CDR}"
    RESULT_VARIABLE result)
if(NOT result EQUAL 0)
    message(FATAL_ERROR "RTI and Wireloom CDR capture files differ")
endif()
