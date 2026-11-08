if(NOT DEFINED RTI_CONTRACT OR NOT DEFINED WIRELOOM_CONTRACT)
    message(FATAL_ERROR "RTI_CONTRACT and WIRELOOM_CONTRACT are required")
endif()

set(expected_files metadata.txt)
foreach(representation IN ITEMS xcdr1 xcdr2)
    foreach(state IN ITEMS default minimum maximum alternate)
        list(APPEND expected_files "${representation}-${state}.cdr")
    endforeach()
endforeach()

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

message(STATUS "RTI and Wireloom match on DynamicType metadata and eight XCDR captures")
