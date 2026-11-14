# Published 1.1.0 C++ walkthrough

Fresh applications used the signed NuGet.org package and the three source blocks
from [Hello World](../../../../guides/hello-world.md#modern-c-walkthrough).
The [source and generated files](sources-and-generated.zip), build logs, and
both peer logs are retained here.
[provenance.json](provenance.json) records exact package/file hashes and toolchains.

| Check                                          | Windows/MSVC                                         | Linux/GCC                           |
| ---------------------------------------------- | ---------------------------------------------------- | ----------------------------------- |
| Clean Release/shared build                     | Passed                                               | Passed                              |
| Unchanged build skips generation               | Passed                                               | Passed                              |
| Invalid arguments                              | Exit 2                                               | Exit 2                              |
| Ten ordered payloads and both completion lines | Both roles exit 0                                    | Both roles exit 0                   |
| Publisher/subscriber missing-peer timeout      | Both exit 1 after 30 seconds                         | Both exit 1 after 30 seconds        |
| Graceful interruption                          | Both exit 0 with `Stopped.`; isolated console Ctrl+C | Both exit 0 with `Stopped.`; SIGINT |

Machine-readable [Windows](windows/result.json) and [Linux](linux/result.json)
results retain six runtime checks each. Configuration/build commands follow
the guide, with the recorded Windows generator/toolset selected explicitly.
Both hosts emitted byte-identical generated files. Linux used a container
with networking disabled; these processes do not establish UDP transport.
The [tagged-source wire reports](../../../../testing/wire-compatibility/evidence/1.1.0/README.md)
separately establish their named UDPv4 mixed-peer exchanges.

No RTI libraries or license file are retained. Consumers supply their own SDK
and license. This qualifies the named Hello World shape and toolchains only.
