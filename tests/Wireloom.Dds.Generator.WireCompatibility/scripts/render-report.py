"""Render sanitized scenario TSV data as structured JSON and a Markdown matrix."""

import json
import pathlib
import sys

version, expected_count, source_path, output_path, markdown_path, logs_path, exclusions_path = sys.argv[1:]
exclusions = json.loads(pathlib.Path(exclusions_path).read_text(encoding="utf-8"))["cases"]
scenarios = []
for line in pathlib.Path(source_path).read_text(encoding="utf-8").splitlines():
    case_id, csharp_type, cpp_type, writer, writer_version, reader, reader_version, status, fixture = line.split("\t")
    scenario = {
        "case": case_id,
        "writer": {"language": writer, "rtiVersion": writer_version},
        "reader": {"language": reader, "rtiVersion": reader_version},
        "fixture": fixture,
        "status": status,
    }
    if csharp_type or cpp_type:
        scenario["types"] = {"C#": csharp_type, "C++": cpp_type}
    if case_id == "10-flat-data-binding":
        scenario["cppIdlAdaptation"] = "Removed @language_binding(FLAT_DATA) for standard C++ type generation; original IDL remains the C# input."
    elif case_id == "07-union-wchar-label":
        scenario["cppIdlAdaptation"] = "Used an RTI DynamicType with a wchar discriminator because rtiddsgen cannot parse wchar union labels."
    elif case_id == "05-array-of-sequences":
        scenario["knownLimitation"] = "DDSG0105 is a warning. Generated C# flattens each array-of-sequences member into one sequence while the RTI C++ type preserves the IDL array; the default fixture passes within each language, but both cross-language pairings fail endpoint discovery with different member type kinds."
    elif case_id == "09-optional-string-sequences":
        scenario["knownLimitation"] = "Absent and empty optional states are controls. Present wide-string sequence fixtures expose failures involving the RTI-generated C++ representation; inspect the sanitized endpoint log for the individual pairing."
    scenarios.append(scenario)
pathlib.Path(output_path).write_text(
    json.dumps({
        "rtiVersion": version,
        "expectedScenarioCount": int(expected_count),
        "scenarioCount": len(scenarios),
        "fixtureCount": len({(scenario["case"], scenario["fixture"]) for scenario in scenarios}),
        "excludedCases": exclusions,
        "scenarios": scenarios,
    }, indent=2) + "\n",
    encoding="utf-8",
)

# Build a compact matrix for humans while retaining every individual endpoint
# result and version in JSON. Logs contain only the runner's sanitized lines.
pairings = (("C#", "C#"), ("C++", "C++"), ("C#", "C++"), ("C++", "C#"))
grouped = {}
for scenario in scenarios:
    row = grouped.setdefault((scenario["case"], scenario["fixture"]), {})
    pairing = (scenario["writer"]["language"], scenario["reader"]["language"])
    row[pairing] = scenario

def problem_for(case_id, fixture, pairing):
    writer, reader = pairing
    writer_id = "cs" if writer == "C#" else "cpp"
    reader_id = "cs" if reader == "C#" else "cpp"
    log_path = pathlib.Path(logs_path) / f"{case_id}-{fixture}-{writer_id}-{reader_id}.log"
    if not log_path.exists():
        return "scenario did not run"

    lines = log_path.read_text(encoding="utf-8").splitlines()
    details = [line.removeprefix("FAIL_DETAIL ") for line in lines if line.startswith("FAIL_DETAIL ")]
    if details:
        return details[0].rstrip(".")
    failures = [line.removeprefix("FAIL ") for line in lines if line.startswith("FAIL ")]
    if failures:
        return failures[0].rstrip(".")
    diagnostics = [line for line in lines if line.startswith("RTI_DIAGNOSTIC ")]
    if any("different type kinds" in line for line in diagnostics):
        return "RTI discovery rejected the type: different member type kinds"
    return "endpoint failed; see sanitized scenario log"

def cell_for(case_id, fixture, pairing, row):
    scenario = row.get(pairing)
    if scenario is None:
        return "❔"
    if scenario["status"] == "PASS":
        return "✅"
    if scenario["status"] == "FAIL":
        return "⛔"
    return "❔"

failed = [scenario for scenario in scenarios if scenario["status"] == "FAIL"]
not_run = [scenario for scenario in scenarios if scenario["status"] == "NOT_RUN"]
passed = len(scenarios) - len(failed) - len(not_run)
lines = [
    "# Wire compatibility results",
    "",
    f"RTI Connext **{version}** · **{len(grouped)}** case/fixture rows · **{passed}** passed · **{len(failed)}** failed · **{len(not_run)}** not run",
    "",
    "Each cell is an independent producer/consumer exchange. ✅ passed, ⛔ failed, ❔ not run. Failure details are taken from sanitized endpoint logs.",
    "",
    "| IDL scenario | Variation | C# → C# | C++ → C++ | C# → C++ | C++ → C# | Problem / defect |",
    "| :-- | :-- | :--: | :--: | :--: | :--: | :-- |",
]
for (case_id, fixture), row in sorted(grouped.items()):
    status_cells = [cell_for(case_id, fixture, pairing, row) for pairing in pairings]
    problems = []
    for pairing, status in zip(pairings, status_cells):
        if status in {"⛔", "❔"}:
            pairing_name = f"{pairing[0]} → {pairing[1]}"
            detail = problem_for(case_id, fixture, pairing).replace("|", "\\|")
            problems.append(f"**{pairing_name}:** {detail}")
    problem_cell = "<br>".join(problems) if problems else "—"
    lines.append(
        f"| `{case_id}.idl` | `{fixture}` | {' | '.join(status_cells)} | {problem_cell} |"
    )
pathlib.Path(markdown_path).write_text("\n".join(lines) + "\n", encoding="utf-8")
