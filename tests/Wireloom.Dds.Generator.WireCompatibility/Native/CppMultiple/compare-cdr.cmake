foreach(peer IN ITEMS RTI_CONTRACT WIRELOOM_CONTRACT)
    if(NOT EXISTS "${${peer}}/metadata.txt")
        message(FATAL_ERROR "Missing ${peer} metadata output")
    endif()
endforeach()

execute_process(
    COMMAND "${CMAKE_COMMAND}" -E compare_files
        "${RTI_CONTRACT}/metadata.txt"
        "${WIRELOOM_CONTRACT}/metadata.txt"
    RESULT_VARIABLE metadata_result)
if(NOT metadata_result EQUAL 0)
    message(FATAL_ERROR "RTI and Wireloom DynamicType metadata differs")
endif()

foreach(representation IN ITEMS xcdr1 xcdr2)
    foreach(state IN ITEMS default minimum maximum alternate)
        execute_process(
            COMMAND "${CMAKE_COMMAND}" -E compare_files
                "${RTI_CONTRACT}/${representation}-${state}.cdr"
                "${WIRELOOM_CONTRACT}/${representation}-${state}.cdr"
            RESULT_VARIABLE result)
        if(NOT result EQUAL 0)
            message(FATAL_ERROR "RTI and Wireloom ${representation} ${state} captures differ")
        endif()
    endforeach()
endforeach()
