# Generated C# Qodana report

Measured on October 25, 2026. This report describes the current Qodana scan of
Wireloom-generated C# and the retained RTI C# corpus. It is a snapshot, not a
change log. The scans assess static inspections; they do not measure runtime
serialization or interoperability.

## Scan scope and method

Community Qodana 2026.2.691 completed with the `qodana.recommended` profile,
C# 12, .NET 10, and `Rti.ConnextDds` 7.7.0. The Wireloom scan covers all 109
exported corpus cases (867 C# files). The RTI scan covers 105 cases with
retained C# output (214 C# files: 212 case files and two common include files).
Wireloom has no retained RTI C# oracle for four cases: `05-array-of-sequences-idl`,
`07-union-wchar-label`, `12-macro-arity-linux`, and `12-union-wchar-rti-limit`.

The sources were staged in separate scan roots, with generated headers removed
and `.g.cs` files renamed to `.cs`; source code, XML documentation, and nullable
directives were preserved. Each case was compiled in an isolated project.
The Wireloom host projects disabled nullable analysis so generated files could
establish their own nullable context. The RTI host projects enabled it. RTI was
scanned with `--no-build` because the unmodified retained sources contain
compiler errors. These scope and configuration differences mean the raw totals
are not a percentage comparison or a like-for-like ranking.

Each minimal corpus project is only a wrapper around an isolated code snippet;
it has no application consumers. For both scans, the Qodana `.editorconfig`
disables the four project-wide inspections surfaced by this scan. File-level
and compiler inspections remain enabled:

```editorconfig
resharper_auto_property_can_be_made_get_only_global_highlighting = none
resharper_member_can_be_protected_global_highlighting = none
resharper_unused_member_global_highlighting = none
resharper_unused_type_global_highlighting = none
```

## Errors

| Qodana category  | Wireloom | RTI C# |
| :--------------- | -------: | -----: |
| `CSharpErrors`   |        0 |     27 |
| **Total errors** |    **0** | **27** |

## Warnings

| Qodana category                                                 |  Wireloom |     RTI C# |
| :-------------------------------------------------------------- | --------: | ---------: |
| `CheckNamespace`                                                |       867 |        192 |
| `ConditionalAccessQualifierIsNonNullableAccordingToAPIContract` |         1 |          0 |
| `ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract`    |        26 |        414 |
| `CSharpWarnings::CS1591`                                        |         0 |      3,671 |
| `CSharpWarnings::CS8603`                                        |         0 |         13 |
| `CSharpWarnings::CS8604`                                        |         0 |        193 |
| `CSharpWarnings::CS8618`                                        |         0 |         79 |
| `CSharpWarnings::CS8625`                                        |         0 |          4 |
| `CSharpWarnings::CS8765`                                        |         0 |        189 |
| `CSharpWarnings::CS8767`                                        |         0 |        193 |
| `EmptyNamespace`                                                |         0 |          2 |
| `EmptyStatement`                                                |         0 |         14 |
| `InconsistentNaming`                                            |       761 |        924 |
| `NonReadonlyMemberInGetHashCode`                                |       386 |        377 |
| `NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract`  |        18 |          0 |
| `ParameterHidesMember`                                          |         8 |         22 |
| `PartialTypeWithSinglePart`                                     |       200 |          0 |
| `RedundantBoolCompare`                                          |         0 |          1 |
| `RedundantCast`                                                 |         0 |      1,221 |
| `RedundantDefaultMemberInitializer`                             |         0 |          6 |
| `RedundantExplicitArrayCreation`                                |         0 |        221 |
| `RedundantNameQualifier`                                        |     1,159 |      2,377 |
| `RedundantTypeArgumentsOfMethod`                                |         0 |         14 |
| `RedundantUsingDirective`                                       |         0 |        741 |
| **Total warnings**                                              | **3,426** | **10,868** |

## Notes

| Qodana category                                | Wireloom |    RTI C# |
| :--------------------------------------------- | -------: | --------: |
| `ArrangeAccessorOwnerBody`                     |        0 |         3 |
| `ArrangeObjectCreationWhenTypeEvident`         |        0 |       311 |
| `ConvertIfStatementToNullCoalescingAssignment` |        0 |         2 |
| `ConvertToPrimaryConstructor`                  |        0 |       217 |
| `UseCollectionExpression`                      |        0 |        98 |
| `UseObjectOrCollectionInitializer`             |        0 |       644 |
| **Total notes**                                |    **0** | **1,275** |

## Preventable-warning review

The Qodana warning categories below are explicitly excluded from the
preventable-warning count. Each category has evidence that the finding is an
intentional generated-code choice, preserves IDL/RTI compatibility, or comes
from the scan setup. The RTI column shows the count in the corresponding
category; it does not mean RTI's findings received the same review.

| Excluded category                                               |  Wireloom |    RTI C# | Why excluded                                                                                                                                                                                                                                                                                                                                                                                                                  |
| :-------------------------------------------------------------- | --------: | --------: | :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `CheckNamespace`                                                |       867 |       192 | The temporary scan layout groups files by corpus case ID rather than generated namespace. This is a staging-location artifact.                                                                                                                                                                                                                                                                                                |
| `ConditionalAccessQualifierIsNonNullableAccordingToAPIContract` |         1 |         0 | Defensive runtime null handling is retained. Nullable annotations cannot guarantee that mutable generated values or caller-supplied values are non-null at runtime.                                                                                                                                                                                                                                                           |
| `ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract`    |        26 |       414 | Retain defensive guards: generated collection aliases can have a null `Value` after a null copy-constructor argument, mutable string values can be set to null, and recursive support initialization can re-enter before its singleton assignment completes.                                                                                                                                                                  |
| `InconsistentNaming`                                            |       761 |       924 | Names preserve IDL spelling and match retained RTI C# identifiers wherever corresponding source is available. Same-case comparison matched all enum members, locals, parameters, static fields, and type names, plus 311/312 properties and 371/372 private fields. The unmatched property and field findings are on the Wireloom-only `CorpusIntegrationStatus.active` fixture; 26 findings are in cases without RTI source. |
| `NonReadonlyMemberInGetHashCode`                                |       386 |       377 | Generated mutable-model hashing follows the established RTI pattern and keeps Wireloom behavior aligned with the oracle.                                                                                                                                                                                                                                                                                                      |
| `NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract`  |        18 |         0 | Retain null fallback behavior for runtime values. Annotations describe intended use but do not prevent null from reaching mutable generated APIs or caller-provided data.                                                                                                                                                                                                                                                     |
| `ParameterHidesMember`                                          |         8 |        22 | Generated parameter names are preserved for RTI parity. Where a parameter shadows a member, `this.` qualifies the member; RTI union equality code also uses this qualification.                                                                                                                                                                                                                                               |
| `PartialTypeWithSinglePart`                                     |       200 |         0 | Partial generated data types are extension points for application-authored partial declarations. The isolated corpus scan cannot see those application parts.                                                                                                                                                                                                                                                                 |
| `RedundantNameQualifier`                                        |     1,159 |     2,377 | The SARIF-highlighted qualifiers use `global::`. Full qualification avoids collisions when IDL names shadow framework or generated types.                                                                                                                                                                                                                                                                                     |
| **Total explicitly excluded warnings**                          | **3,426** | **4,306** | RTI total is the sum of the same categories for comparison only; RTI's other findings have not been classified as preventable or non-preventable.                                                                                                                                                                                                                                                                             |

### Remaining findings

| Measure                                              | Wireloom |      RTI C# |
| :--------------------------------------------------- | -------: | ----------: |
| Warnings after the explicit Wireloom exclusions      |        0 |     6,562\* |
| Notes, reported separately from warnings             |        0 |       1,275 |
| Errors                                               |        0 |          27 |
| **Findings remaining after the Wireloom exclusions** |    **0** | **7,864\*** |

Wireloom has **0 preventable warnings** after the documented exclusions.
The starred RTI figures subtract only the matching excluded warning categories
above from the raw RTI scan.
They are not an RTI preventable-warning total because the remaining RTI
warnings have not been individually reviewed.

## Category-specific decisions

- **Naming:** Generated identifiers retain IDL names and agree with RTI C# in
  the retained matching cases. Style normalization would change public API names
  and break parity.
- **Nullable-condition checks:** A nullable annotation does not enforce a
  runtime non-null invariant. The runtime contract test
  [`MatrixChainPreservesItsValueContract`](../../tests/Wireloom.Dds.Generator.RuntimeContracts/Aliases/CollectionAliasSpecs.cs)
  asserts the null collection-alias state; the associated guards remain useful.
- **Parameter hides member:** The generated `this.` qualification is explicit
  and consistent with RTI's union equality implementation. It also avoids
  accidentally binding to a same-named parameter.

Raw SARIF warning counts are retained; the exclusions above are used only to
calculate the preventable-warning figure.
