foreach(name api.txt metadata.txt)
    execute_process(COMMAND "${CMAKE_COMMAND}" -E compare_files
        "${RTI_CONTRACT}/${name}" "${WIRELOOM_CONTRACT}/${name}"
        RESULT_VARIABLE comparison)
    if(NOT comparison EQUAL 0)
        message(FATAL_ERROR "RTI and Wireloom Holder ${name} differ")
    endif()
endforeach()
