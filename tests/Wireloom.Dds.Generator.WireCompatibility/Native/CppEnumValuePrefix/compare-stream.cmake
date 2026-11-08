file(READ "${RTI_CONTRACT}/stream.txt" rti_stream)
file(READ "${WIRELOOM_CONTRACT}/stream.txt" wireloom_stream)
if(NOT rti_stream STREQUAL wireloom_stream)
    message(FATAL_ERROR
        "Confirmed stream operator parity failure. RTI output: [${rti_stream}], Wireloom output: [${wireloom_stream}]")
endif()
