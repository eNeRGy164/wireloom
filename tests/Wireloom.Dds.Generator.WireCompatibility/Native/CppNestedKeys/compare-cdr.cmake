if(NOT EXISTS "${RTI_CDR}" OR NOT EXISTS "${WIRELOOM_CDR}")
    message(FATAL_ERROR "Both RTI and Wireloom CDR captures must exist")
endif()

execute_process(
    COMMAND "${CMAKE_COMMAND}" -E compare_files "${RTI_CDR}" "${WIRELOOM_CDR}"
    RESULT_VARIABLE comparison)

if(NOT comparison EQUAL 0)
    message(FATAL_ERROR "RTI and Wireloom nested-key CDR captures differ")
endif()
