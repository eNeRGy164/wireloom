# Generated C# Qodana report

The independent scans compare Wireloom-generated C# and the matching retained
RTI C# captures with generated-code classification removed. Both exact RTI
7.3.1 and 7.7.0 profiles are covered. Normal
[consumer Qodana and SonarQube checks](../testing/consumer-analysis.md) retain
generated classification and establish a different result. Static analysis
does not measure DDS serialization or interoperability.

## Attribution and reproduction

The current result uses fresh Wireloom corpus exports and retained RTI captures,
with source/input/output hashes, staging manifests, project settings, commands,
exit status, and raw SARIF
retained under `artifacts/consumer-quality/`. These identify the measured source
and tool configuration; the result does not retroactively qualify a published
package. Use the [release records](../releases/README.md) for package scope.

To reproduce:

1. Follow the [LTS export procedure](LTS-PROFILES.md#corpus-tests-and-source-exports)
   with a new output directory. Record both profiles' successful corpus results
   and source/input/output hashes.
2. Stage every Wireloom export and its corresponding retained RTI case in
   separate projects/scan roots. Resolve RTI source paths through the shared
   manifest, including oracle-directory overrides and required include sources.
   Preserve source code, XML documentation, nullable directives, and attributes;
   remove the leading
   generated provenance comments and rename `.g.cs` files to `.cs`.
3. Target `net10.0`, C# 12, and the case's exact RTI package using central
   package versions. Wireloom files establish their own nullable context; their
   host disables it. RTI hosts enable nullable analysis. Build both solutions
   and retain failures in unmodified RTI output.
4. Apply only the four wrapper-project exclusions listed below. Keep the
   repository's `global.json` visible and stage fresh Linux build intermediates.
   Run Community Qodana with `qodana.recommended` against each staged solution.
   Use `--no-build` for RTI so raw-source compiler failures do not prevent the
   inspection. Separately build both with `-p:GenerateDocumentationFile=true`
   to measure XML-documentation diagnostics.
   See the [Docker procedure](../testing/quality.md) for mounting and report checks.
5. Confirm normal completion and review every reported category against its
   actual source locations. Retain raw findings and classify intentional output
   separately; do not turn these classifications into consumer-wide suppressions.
   Update this report when the measured source or configuration changes.

## Scan scope and method

Community Qodana 2026.2.691 / InspectCode 2026.2.1 runs under Debian 13 in
Docker, using SDK 10.0.400 and the `qodana.recommended` profile. Both Wireloom exports
pass all 379 corpus checks.

| Output              | RTI version/profile | Isolated cases | C# files |
| :------------------ | :------------------ | -------------: | -------: |
| Wireloom            | 7.3.1               | 109            | 883      |
| RTI rtiddsgen 4.3.1 | 7.3.1               | 104            | 212      |
| Wireloom            | 7.7.0               | 110            | 887      |
| RTI rtiddsgen 4.7.0 | 7.7.0               | 106            | 216      |

RTI counts include required shared include files once per physical source.
Only captures corresponding to accepted Wireloom exports are included; unrelated
negative probes and wire-adjusted/wrapped sources are excluded. Four Wireloom
cases have no corresponding retained RTI C# capture on either profile:
`05-array-of-sequences-idl`, `07-union-wchar-label`,
`12-macro-arity-linux`, and `12-union-wchar-rti-limit`. The 7.3.1 profile
also lacks the Windows macro-arity capture.

Wireloom's staged solution builds without warnings or errors. RTI's unmodified
output contains compiler failures and is inspected with `--no-build`. RTI
hosts enable nullable analysis, while Wireloom files contain their own nullable
directives. Differences in input coverage, file organization, host settings,
and build mode mean raw totals are not a percentage comparison or a normalized
quality ranking.

Each project wraps an isolated generated contract without application callers.
The staging `.editorconfig` disables four project-wide inspections whose
assumptions do not fit those wrappers. File-level and compiler inspections stay
enabled:

```editorconfig
resharper_auto_property_can_be_made_get_only_global_highlighting = none
resharper_member_can_be_protected_global_highlighting = none
resharper_unused_member_global_highlighting = none
resharper_unused_type_global_highlighting = none
```

The [RTI defect catalog](RTI-CODEGEN-DEFECTS.md) links the affected cases and
exact-profile compilation failures. Inspection findings and compiler diagnostics
are reported separately below; one tool may report more locations for a syntax
failure than another.

## Errors

| Qodana category  | Wireloom 7.3.1 | RTI 7.3.1 | Wireloom 7.7.0 | RTI 7.7.0 |
| :--------------- | -------------: | --------: | -------------: | --------: |
| .CSharpErrors    | 0              | 56        | 0              | 27        |
| **Total errors** | **0**          | **56**    | **0**          | **27**    |

The separate compiler build reports 39 diagnostics across eight RTI 7.3.1 cases
and 24 across seven RTI 7.7.0 cases. Wireloom has zero compiler errors. RTI
failures include shadowed native-conversion receivers, numeric character labels,
unsigned union-label metadata, octet bindings, wstring-alias namespace references,
and malformed 7.3.1 RPC output. No source workarounds are applied to these scans.

## Warnings

| Qodana category                                               | Wireloom 7.3.1 | RTI 7.3.1 | Wireloom 7.7.0 | RTI 7.7.0 |
| :------------------------------------------------------------ | -------------: | --------: | -------------: | --------: |
| CSharpWarnings::CS8603                                        | 0              | 20        | 0              | 13        |
| CSharpWarnings::CS8604                                        | 0              | 212       | 0              | 198       |
| CSharpWarnings::CS8618                                        | 0              | 94        | 0              | 85        |
| CSharpWarnings::CS8625                                        | 0              | 5         | 0              | 5         |
| CSharpWarnings::CS8765                                        | 0              | 208       | 0              | 194       |
| CSharpWarnings::CS8767                                        | 0              | 212       | 0              | 198       |
| CheckNamespace                                                | 883            | 190       | 887            | 193       |
| ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract    | 28             | 453       | 28             | 425       |
| ConditionalAccessQualifierIsNonNullableAccordingToAPIContract | 1              | 0         | 1              | 0         |
| EmptyStatement                                                | 0              | 17        | 0              | 14        |
| InconsistentNaming                                            | 801            | 1,073     | 802            | 970       |
| NonReadonlyMemberInGetHashCode                                | 397            | 420       | 398            | 389       |
| NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract  | 18             | 0         | 18             | 0         |
| ParameterHidesMember                                          | 8              | 22        | 8              | 22        |
| PartialTypeWithSinglePart                                     | 204            | 0         | 205            | 0         |
| RedundantBoolCompare                                          | 0              | 1         | 0              | 1         |
| RedundantCast                                                 | 0              | 618       | 0              | 1,203     |
| RedundantDefaultMemberInitializer                             | 0              | 7         | 0              | 7         |
| RedundantExplicitArrayCreation                                | 0              | 247       | 0              | 224       |
| RedundantNameQualifier                                        | 1,194          | 2,715     | 1,197          | 2,487     |
| RedundantTypeArgumentsOfMethod                                | 0              | 14        | 0              | 14        |
| RedundantUsingDirective                                       | 0              | 1,045     | 0              | 970       |
| **Total warnings**                                            | **3,534**      | **7,573** | **3,544**      | **7,612** |

### XML-documentation compiler check

Both staged solutions are also built with `GenerateDocumentationFile=true`.
These compiler `CS1591` counts are separate from the Qodana totals above;
duplicate build-summary lines are removed, while distinct project contexts
remain distinct. RTI compiler failures can prevent further diagnostics.

| Diagnostic                                    | Wireloom 7.3.1 | RTI 7.3.1 | Wireloom 7.7.0 | RTI 7.7.0 |
| :-------------------------------------------- | -------------: | --------: | -------------: | --------: |
| Missing public API XML documentation (CS1591) | 0              | 3,537     | 0              | 3,568     |

## Notes

| Qodana category                              | Wireloom 7.3.1 | RTI 7.3.1 | Wireloom 7.7.0 | RTI 7.7.0 |
| :------------------------------------------- | -------------: | --------: | -------------: | --------: |
| ArrangeAccessorOwnerBody                     | 0              | 3         | 0              | 3         |
| ArrangeObjectCreationWhenTypeEvident         | 0              | 347       | 0              | 330       |
| BadControlBracesIndent                       | 0              | 12        | 0              | 0         |
| ConvertIfStatementToNullCoalescingAssignment | 0              | 2         | 0              | 2         |
| ConvertToPrimaryConstructor                  | 0              | 236       | 0              | 222       |
| InvokeAsExtensionMember                      | 96             | 0         | 96             | 0         |
| UseCollectionExpression                      | 0              | 107       | 0              | 98        |
| UseObjectOrCollectionInitializer             | 0              | 0         | 0              | 656       |
| **Total notes**                              | **96**         | **707**   | **96**         | **1,311** |

## Preventable-warning review

Every Wireloom category is accounted for below. Raw warnings and notes from
both generators remain in the retained SARIF. RTI findings have not received
the same preventable/intentional review; they are raw inspection counts. The classifications describe the inspected output, not
blanket exemptions for arbitrary code with the same inspection ID.

| Category                                                        | Why intentional or attributable to staging                                                                                             |
| :-------------------------------------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------- |
| `CheckNamespace`                                                | Staging groups files by corpus case rather than generated namespace; a scan-layout artifact.                                           |
| `ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract`    | Defensive runtime null guards remain necessary for mutable values, null copy-constructor states, and recursive support initialization. |
| `ConditionalAccessQualifierIsNonNullableAccordingToAPIContract` | Nullable annotations do not prevent a collection alias Value from being null at runtime.                                               |
| `InconsistentNaming`                                            | Generated names retain IDL spelling and the corresponding RTI API instead of applying application naming conventions.                  |
| `InvokeAsExtensionMember`                                       | Explicit global::System.Linq.Enumerable calls fix the target without relying on extension lookup or using directives.                  |
| `NonReadonlyMemberInGetHashCode`                                | Mutable-model hashing preserves the documented generated-value contract and RTI behavior.                                              |
| `NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract`  | Retain null formatting fallbacks and reentrant support initialization; annotations do not enforce runtime invariants.                  |
| `ParameterHidesMember`                                          | Parameter and member names follow the RTI/IDL API; explicit this. qualification resolves shadowing.                                    |
| `PartialTypeWithSinglePart`                                     | Generated data types are partial extension points; isolated cases contain no handwritten application part.                             |
| `RedundantNameQualifier`                                        | Explicit global:: qualification fixes binding when IDL names shadow framework or generated names.                                      |

The Wireloom review finds **zero preventable warnings or notes** after
these explicit classifications, with **zero errors**. No generator change or
additional normal-consumer exclusion is required.

Defensive null handling is supported by
[`MatrixChainPreservesItsValueContract`](../../tests/Wireloom.Dds.Generator.RuntimeContracts/Aliases/CollectionAliasSpecs.cs)
and the [generated-value contracts](../guides/generated-value-semantics.md).
All inspected redundant qualifiers are `global::` qualifications, and all
extension-call suggestions target explicitly qualified
`global::System.Linq.Enumerable` calls. Keeping those forms makes binding
independent of imported extension methods and IDL naming collisions.
