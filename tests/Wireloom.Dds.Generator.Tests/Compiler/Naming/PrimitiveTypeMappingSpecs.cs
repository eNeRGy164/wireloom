using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Naming.Tests;

/// <summary>Verifies that primitive target mappings remain internally consistent.</summary>
public sealed class PrimitiveTypeMappingSpecs
{
    /// <summary>Checks canonical and alias spellings for representative primitive mappings.</summary>
    [Fact]
    public void ResolvesCanonicalAndAliasSpellingsToTheSameMapping()
    {
        // Arrange
        var canonical = PrimitiveTypeMapping.Resolve("long");
        var alias = PrimitiveTypeMapping.Resolve(" int32 ");

        // Act
        var canonicalShape = (
            canonical.ManagedType,
            canonical.NativeStorageType,
            canonical.DynamicType,
            canonical.NativeDefaultLiteral,
            canonical.AnnotationTypeKind,
            canonical.AnnotationValueProperty,
            canonical.MinimumLiteral,
            canonical.MaximumLiteral,
            canonical.UncastNativeDefaultLiteral,
            canonical.AnnotationDefaultLiteral,
            canonical.NativeValueRequiresCast);
        var aliasShape = (
            alias.ManagedType,
            alias.NativeStorageType,
            alias.DynamicType,
            alias.NativeDefaultLiteral,
            alias.AnnotationTypeKind,
            alias.AnnotationValueProperty,
            alias.MinimumLiteral,
            alias.MaximumLiteral,
            alias.UncastNativeDefaultLiteral,
            alias.AnnotationDefaultLiteral,
            alias.NativeValueRequiresCast);

        // Assert
        aliasShape.ShouldBe(canonicalShape);
    }

    /// <summary>Checks native defaults for representative primitive storage shapes.</summary>
    [Theory]
    [InlineData("boolean", "byte", "0")]
    [InlineData("char", "byte", "(byte)0")]
    [InlineData("wchar", "short", "(short)0")]
    [InlineData("octet", "byte", "(byte)0")]
    public void ResolvesNativeDefaultsForStorageShapes(string idlType, string expectedStorageType, string expectedDefault)
    {
        // Arrange
        var mapping = PrimitiveTypeMapping.Resolve(idlType);

        // Act
        var nativeStorageType = mapping.NativeStorageType;
        var nativeDefault = mapping.NativeDefaultLiteral;

        // Assert
        nativeStorageType.ShouldBe(expectedStorageType);
        nativeDefault.ShouldBe(expectedDefault);
    }

    /// <summary>Checks the special managed-to-native expressions for primitive representations.</summary>
    [Fact]
    public void BuildsSpecialNativeConversionExpressions()
    {
        // Arrange
        var boolean = PrimitiveTypeMapping.Resolve("boolean");
        var character = PrimitiveTypeMapping.Resolve("char");
        var wideCharacter = PrimitiveTypeMapping.Resolve("wchar");

        // Act
        var booleanFromNative = boolean.FromNativeExpression("value");
        var characterToNative = character.ToNativeExpression("value");
        var wideCharacterToNative = wideCharacter.ToNativeExpression("value");

        // Assert
        booleanFromNative.ShouldBe("global::System.Convert.ToBoolean(value)");
        characterToNative.ShouldBe("NativeChar.ToUtf8(value)");
        wideCharacterToNative.ShouldBe("(short)value");
    }

    /// <summary>Checks annotation defaults remain expressed in managed primitive types.</summary>
    [Fact]
    public void UsesManagedLiteralsForStorageSpecificAnnotations()
    {
        // Arrange
        var boolean = PrimitiveTypeMapping.Resolve("boolean");
        var character = PrimitiveTypeMapping.Resolve("char");
        var wideCharacter = PrimitiveTypeMapping.Resolve("wchar");

        // Act
        var defaults = (
            boolean.AnnotationDefaultLiteral,
            character.AnnotationDefaultLiteral,
            wideCharacter.AnnotationDefaultLiteral);

        // Assert
        defaults.ShouldBe(("false", "'\\0'", "'\\0'"));
    }

    /// <summary>Checks annotation metadata and the primitive predicate.</summary>
    [Fact]
    public void ExposesAnnotationMetadataAndRecognizesSupportedPrimitives()
    {
        // Arrange
        var signed = PrimitiveTypeMapping.Resolve("short");
        var floating = PrimitiveTypeMapping.Resolve("double");

        // Act
        var signedIsPrimitive = PrimitiveTypeMapping.IsPrimitive("int16");
        var unknownIsPrimitive = PrimitiveTypeMapping.IsPrimitive("not-a-primitive");
        var userDefinedIsPrimitive = PrimitiveTypeMapping.IsPrimitive("Example.Sample");

        // Assert
        signed.AnnotationTypeKind.ShouldBe("Int16");
        signed.AnnotationValueProperty.ShouldBe("Int16Value");
        signed.MinimumLiteral.ShouldBe("short.MinValue");
        signed.MaximumLiteral.ShouldBe("short.MaxValue");
        floating.AnnotationTypeKind.ShouldBe("Float64");
        floating.MinimumLiteral.ShouldBe("double.MinValue");
        floating.MaximumLiteral.ShouldBe("double.MaxValue");
        signedIsPrimitive.ShouldBeTrue();
        unknownIsPrimitive.ShouldBeFalse();
        userDefinedIsPrimitive.ShouldBeFalse();
    }
}
