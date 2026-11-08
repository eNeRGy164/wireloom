if(NOT DEFINED RTI_CONTRACT OR NOT DEFINED WIRELOOM_CONTRACT)
    message(FATAL_ERROR "RTI_CONTRACT and WIRELOOM_CONTRACT are required")
endif()

set(expected_files
    metadata.txt
    consumer-state.txt
    xcdr1-phase1.cdr
    xcdr1-phase2.cdr
    xcdr1-phase3.cdr
    xcdr1-phase4.cdr
    xcdr1-phase5.cdr
    xcdr2-phase1.cdr
    xcdr2-phase2.cdr
    xcdr2-phase3.cdr
    xcdr2-phase4.cdr
    xcdr2-phase5.cdr)

foreach(file IN LISTS expected_files)
    set(rti_file "${RTI_CONTRACT}/${file}")
    set(wireloom_file "${WIRELOOM_CONTRACT}/${file}")
    if(NOT EXISTS "${rti_file}" OR NOT EXISTS "${wireloom_file}")
        message(FATAL_ERROR "Missing contract output: ${file}")
    endif()

    execute_process(
        COMMAND "${CMAKE_COMMAND}" -E compare_files "${rti_file}" "${wireloom_file}"
        RESULT_VARIABLE result)
    if(NOT result EQUAL 0)
        message(FATAL_ERROR "RTI and Wireloom contract output differs: ${file}")
    endif()
endforeach()

message(STATUS "RTI and Wireloom match on metadata, consumer state, and 10 XCDR captures")
