execute_process(COMMAND "${CMAKE_COMMAND}" -E compare_files
    "${RTI_CONTRACT}/metadata.txt" "${WIRELOOM_CONTRACT}/metadata.txt"
    RESULT_VARIABLE comparison)
if(NOT comparison EQUAL 0)
    message(FATAL_ERROR "RTI and Wireloom DerivedValue DynamicType metadata differ")
endif()
