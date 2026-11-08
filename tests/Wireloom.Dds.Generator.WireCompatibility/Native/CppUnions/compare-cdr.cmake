if(NOT EXISTS "${RTI_CDR}" OR NOT EXISTS "${WIRELOOM_CDR}")
    message(FATAL_ERROR "Both RTI and Wireloom CDR captures must exist.")
endif()

file(READ "${RTI_CDR}" rti_bytes HEX)
file(READ "${WIRELOOM_CDR}" wireloom_bytes HEX)
if(NOT rti_bytes STREQUAL wireloom_bytes)
    message(FATAL_ERROR "RTI and Wireloom union CDR captures differ.")
endif()

message(STATUS "RTI and Wireloom union CDR captures match byte for byte.")
