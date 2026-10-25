using TypedDefaults;
using System.Reflection;
using Omg.Types;
using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.RuntimeContracts.Defaults;

/// <summary>Verifies type defaults supplied by DynamicType metadata when DynamicData members are absent.</summary>
public sealed class TypedDefaultSpecs
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void EnumExtensibilityReachesRuntimeMetadata(bool isPublic)
    {
        // Arrange
        var fixedType = TypedDefaults.Implementation.FixedKindPlugin.CreateDynamicType(isPublic).ShouldBeOfType<EnumType>();

        // Act
        var growingType = TypedDefaults.Implementation.GrowingKindPlugin.CreateDynamicType(isPublic).ShouldBeOfType<EnumType>();

        // Assert
        fixedType.ExtensibilityKind.ShouldBe(ExtensibilityKind.Final);
        growingType.ExtensibilityKind.ShouldBe(ExtensibilityKind.Extensible);
        fixedType.MemberCount.ShouldBe(2U);
        growingType.MemberCount.ShouldBe(2U);
    }

    [Fact]
    public void AdjacentStringDefaultsUseDecodedValuesForManagedAndNativeInitialization()
    {
        // Arrange
        var sample = new Sample();
        var alias = new JoinedText();
        var native = new TypedDefaults.Implementation.SampleUnmanaged();

        try
        {
            // Act
            native.Initialize();
            native.FromNative(sample);

            // Assert
            Joined.Value.ShouldBe("AB");
            alias.Value.ShouldBe("AB");
            sample.joinedText.ShouldBe("AB");
            sample.tokenBoundary.ShouldBe("\u00041");
        }
        finally
        {
            native.Destroy(optionalsOnly: false);
        }
    }

    [Fact]
    public void WideStringTypedefDefaultsUseWideTextForManagedAndNativeInitialization()
    {
        // Arrange
        var sample = new Sample();
        var alias = new WideText((WideText?)null);
        var native = new TypedDefaults.Implementation.SampleUnmanaged();

        try
        {
            // Act
            native.Initialize();
            native.FromNative(sample);

            // Assert
            alias.Value.ShouldBe("世界");
            sample.wideText.ShouldBe("世界");
        }
        finally
        {
            native.Destroy(optionalsOnly: false);
        }
    }

    [Fact]
    public void FullWidthCharacterDefaultsSurviveNativeInitializationAndCheckedRoundTrips()
    {
        // Arrange
        var native = new TypedDefaults.Implementation.SampleUnmanaged();
        var aliasNative = new TypedDefaults.Implementation.InheritedCharacterUnmanaged();
        var sample = new Sample();
        var alias = new InheritedCharacter();

        try
        {
            // Act
            checked
            {
                native.Initialize();
                native.FromNative(sample);
                aliasNative.Initialize();
                aliasNative.FromNative(alias);
                native.ToNative(sample);
                aliasNative.ToNative(alias);
                native.FromNative(sample);
                aliasNative.FromNative(alias);
            }

            // Assert
            sample.highCharacter.ShouldBe('\u8000');
            sample.inheritedCharacter.ShouldBe('\uFFFF');
            alias.Value.ShouldBe('\uFFFF');
        }
        finally
        {
            native.Destroy(optionalsOnly: false);
            aliasNative.Destroy(optionalsOnly: false);
        }
    }

    [Fact]
    public void NullCopyConstructionPreservesTypedDefaultsForAliasesAndMembers()
    {
        // Arrange
        var seven = new Seven(null);
        var nine = new Nine(null);
        var text = new Text((Text?)null);
        var ratio = new Ratio(null);

        // Act
        var sample = new Sample(null);
        var derived = new Derived(null);

        // Assert
        seven.Value.ShouldBe(7);
        nine.Value.ShouldBe(9);
        text.Value.ShouldBe("alias");
        ratio.Value.ShouldBe(1.5);

        sample.inherited.ShouldBe(7);
        sample.overridden.ShouldBe(10);
        sample.nested.ShouldBe(9);
        sample.text.ShouldBe("alias");
        sample.localRatio.ShouldBe(2.5);
        sample.localSingle.ShouldBe(1.25F);
        sample.wideText.ShouldBe("世界");

        derived.inherited.ShouldBe(7);
        derived.localValue.ShouldBe(11);
    }

    [Fact]
    public void FloatingDefaultsKeepTheNegativeZeroSignBit()
    {
        // Arrange
        var sample = new Sample();
        var alias = new NegativeZero(null);

        // Act
        var doubleBits = BitConverter.DoubleToInt64Bits(sample.negativeZero);
        var singleBits = BitConverter.SingleToInt32Bits(sample.negativeFloatZero);

        // Assert
        doubleBits.ShouldBe(long.MinValue);
        singleBits.ShouldBe(int.MinValue);
        BitConverter.DoubleToInt64Bits(alias.Value).ShouldBe(long.MinValue);
    }

    [Fact]
    public void NativeFloatingDefaultsKeepTheNegativeZeroSignBit()
    {
        // Arrange
        var native = new TypedDefaults.Implementation.SampleUnmanaged();
        var sample = new Sample();

        try
        {
            // Act
            native.Initialize();
            native.FromNative(sample);

            // Assert
            BitConverter.DoubleToInt64Bits(sample.negativeZero).ShouldBe(long.MinValue);
            BitConverter.SingleToInt32Bits(sample.negativeFloatZero).ShouldBe(int.MinValue);
            sample.localSingle.ShouldBe(1.25f);
        }
        finally
        {
            native.Destroy(optionalsOnly: false);
        }
    }

    [Fact]
    public void DynamicTypeMetadataCarriesFractionalAndWideStringDefaultsWhenSupported()
    {
        // Arrange
        var type = TypedDefaults.Implementation.SamplePlugin.CreateDynamicType().ShouldBeOfType<StructType>();
        var single = type.Members.Single(member => member.Name == "localSingle");
        var ratio = type.Members.Single(member => member.Name == "ratio");
        var wideText = type.Members.Single(member => member.Name == "wideText");

        // Act
        var singleDefault = GetMemberAnnotationValue(single, "Float32Value");
        var ratioDefault = GetMemberAnnotationValue(ratio, "Float64Value");
        var wideTextDefault = GetMemberAnnotationValue(wideText, "WideStringValue");

        // Assert
        if (typeof(Rti.Dds.Topics.TypeSupport<>).Assembly.GetName().Version < new Version(7, 7))
        {
            singleDefault.ShouldBeNull();
            ratioDefault.ShouldBeNull();
            wideTextDefault.ShouldBeNull();
            return;
        }

        singleDefault.ShouldBeOfType<float>().ShouldBe(1.25f);
        ratioDefault.ShouldBeOfType<double>().ShouldBe(1.5);
        wideTextDefault.ShouldBeOfType<string>().ShouldBe("世界");
    }

    [Fact]
    public void AppliesMemberAndTypedefDefaultsWhenDynamicDataMembersAreCleared()
    {
        // Arrange
        using var serializer = SampleSupport.Instance.CreateSerializer();
        var source = new Sample { ratio = 0, localRatio = 0, localSingle = 0 };
        using var data = serializer.ToDynamicData(source);
        data.ClearMember("inherited");
        data.ClearMember("overridden");
        data.ClearMember("nested");
        data.ClearMember("text");
        data.ClearMember("localText");
        data.ClearMember("negativeZero");
        data.ClearMember("negativeFloatZero");

        // Act
        var result = serializer.FromDynamicData(data);

        // Assert
        if (typeof(Rti.Dds.Topics.TypeSupport<>).Assembly.GetName().Version < new Version(7, 7))
        {
            // RTI 7.3.1 has no dynamic-type member annotation API, so a cleared member has no wire-level default.
            result.inherited.ShouldBe(0);
            return;
        }

        result.inherited.ShouldBe(7);
        result.overridden.ShouldBe(10);
        result.nested.ShouldBe(9);
        result.text.ShouldBe("alias");
        result.localText.ShouldBe("local");
        BitConverter.DoubleToInt64Bits(result.negativeZero).ShouldBe(long.MinValue);
        BitConverter.SingleToInt32Bits(result.negativeFloatZero).ShouldBe(int.MinValue);
    }

    private static object? GetMemberAnnotationValue(StructMember member, string valueProperty)
    {
        var annotations = member.GetType().GetProperty("Annotations", BindingFlags.Instance | BindingFlags.Public)
            ?.GetValue(member);
        var defaultValue = annotations?.GetType().GetProperty("Default", BindingFlags.Instance | BindingFlags.Public)
            ?.GetValue(annotations);
        return defaultValue?.GetType().GetProperty(valueProperty, BindingFlags.Instance | BindingFlags.Public)
            ?.GetValue(defaultValue);
    }
}
