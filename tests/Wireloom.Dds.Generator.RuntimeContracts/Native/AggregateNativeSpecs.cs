using RuntimeManaged;
using RuntimeManaged.Implementation;
using Rti.Types;

namespace Wireloom.Dds.Generator.RuntimeContracts.Native;

/// <summary>Exercises native defaults, conversions, key filtering, and cleanup.</summary>
public sealed class AggregateNativeSpecs
{
    /// <summary>Exercises every initialization allocation flag combination.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void ConvertsEveryPrimitiveWidth(bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = new Scalars
        {
            flag = true, byteValue = 255, signedByte = -128, unsignedByte = 254,
            shortValue = short.MinValue, unsignedShort = ushort.MaxValue,
            intValue = int.MinValue, unsignedInt = uint.MaxValue,
            longValue = long.MinValue, unsignedLong = ulong.MaxValue,
            singleValue = 1.25F, doubleValue = -2.5D, extendedValue = (LongDouble)3.75M,
            character = '\u00fe', wideCharacter = '\u03a9', state = State.Ready,
            text = "narrow", wideText = "世界", defaultNumber = 99
        };

        // Act
        var result = NativeSample.RoundTrip<Scalars, ScalarsUnmanaged>(source, allocatePointers, allocateMemory);

        // Assert
        result.flag.ShouldBeTrue();
        result.byteValue.ShouldBe((byte)255);
        result.signedByte.ShouldBe(sbyte.MinValue);
        result.unsignedByte.ShouldBe((byte)254);
        result.shortValue.ShouldBe(short.MinValue);
        result.unsignedShort.ShouldBe(ushort.MaxValue);
        result.intValue.ShouldBe(int.MinValue);
        result.unsignedInt.ShouldBe(uint.MaxValue);
        result.longValue.ShouldBe(long.MinValue);
        result.unsignedLong.ShouldBe(ulong.MaxValue);
        result.singleValue.ShouldBe(1.25F);
        result.doubleValue.ShouldBe(-2.5D);
        result.extendedValue.ShouldBe((LongDouble)3.75M);
        result.character.ShouldBe('\u00fe');
        result.wideCharacter.ShouldBe('\u03a9');
        result.state.ShouldBe(State.Ready);
        result.text.ShouldBe("narrow");
        result.wideText.ShouldBe("世界");
        result.defaultNumber.ShouldBe(99);
    }

    /// <summary>Checks initialized defaults independently of ToNative.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void InitializesNativeDefaults(bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var native = new ScalarsUnmanaged();
        var destination = new Scalars { defaultNumber = -1, intValue = 99 };

        try
        {
            // Act
            native.Initialize(allocatePointers, allocateMemory);
            native.FromNative(destination);

            // Assert
            destination.defaultNumber.ShouldBe(42);
            destination.intValue.ShouldBe(0);
            destination.flag.ShouldBeFalse();
            destination.state.ShouldBe(State.Idle);
        }
        finally
        {
            native.Destroy(optionalsOnly: false);
        }
    }

    /// <summary>Exercises aggregate and collection conversion with both optional states.</summary>
    [Theory]
    [InlineData(false, false, false)]
    [InlineData(false, false, true)]
    [InlineData(false, true, false)]
    [InlineData(false, true, true)]
    [InlineData(true, false, false)]
    [InlineData(true, false, true)]
    [InlineData(true, true, false)]
    [InlineData(true, true, true)]
    public void ConvertsCollectionsAndOptionalPresence(bool present, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = CollectionSamples.Create(present);

        // Act
        var result = NativeSample.RoundTrip<Collections, CollectionsUnmanaged>(source, allocatePointers, allocateMemory);

        // Assert
        result.child.id.ShouldBe(11);
        result.child.text.ShouldBe("nested");
        result.numbers.ShouldBe([1, 2, 3]);
        result.children[0].id.ShouldBe(21);
        result.children[0].text.ShouldBe("child");
        result.vector.ShouldBe([8, 9]);
        result.matrix[1, 2].ShouldBe(6);
        result.points[0].id.ShouldBe(41);
        result.points[1].text.ShouldBe("second");
        result.texts.ShouldBe(["alpha", "beta"]);
        result.wideTexts.ShouldBe(["世界", "Ω"]);
        if (present)
        {
            result.optionalNumber.ShouldBe(7);
            result.optionalText.ShouldBe("optional");
            result.optionalChild!.id.ShouldBe(31);
            result.optionalNumbers.ShouldBe([5, 6]);
            result.optionalPoints![1].id.ShouldBe(52);
            result.optionalState.ShouldBe(State.Ready);
            result.optionalWideText.ShouldBe("世界");
            result.optionalWideTexts.ShouldBe(["世界", "Ω"]);
            result.optionalMatrix![1, 2].ShouldBe(6);
        }
        else
        {
            result.optionalNumber.ShouldBeNull();
            result.optionalText.ShouldBeNull();
            result.optionalChild.ShouldBeNull();
            result.optionalNumbers.ShouldBeNull();
            result.optionalPoints.ShouldBeNull();
            result.optionalState.ShouldBeNull();
            result.optionalWideText.ShouldBeNull();
            result.optionalWideTexts.ShouldBeNull();
            result.optionalMatrix.ShouldBeNull();
        }
    }

    /// <summary>Checks each key-only conversion direction against sentinel non-key values.</summary>
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void FiltersKeysInEachDirection(bool toKeysOnly, bool fromKeysOnly)
    {
        // Arrange
        var native = new ChildUnmanaged();
        var destination = new Child(99, "destination");

        try
        {
            native.Initialize();
            native.ToNative(new Child(1, "retained"));

            // Act
            native.ToNative(new Child(17, "updated"), keysOnly: toKeysOnly);
            native.FromNative(destination, keysOnly: fromKeysOnly);

            // Assert
            destination.id.ShouldBe(17);
            if (fromKeysOnly)
            {
                destination.text.ShouldBe("destination");
            }
            else if (toKeysOnly)
            {
                destination.text.ShouldBe("retained");
            }
            else
            {
                destination.text.ShouldBe("updated");
            }
        }
        finally
        {
            native.Destroy(optionalsOnly: false);
        }
    }

    /// <summary>Checks inherited optional cleanup without destroying required strings.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void CleansOnlyInheritedOptionalsBeforeFullCleanup(bool keysOnly)
    {
        // Arrange
        var native = new DerivedUnmanaged();
        var source = new Derived { id = 17, note = "optional", text = "required", extra = 23 };

        try
        {
            native.Initialize();
            native.ToNative(source);

            // Act
            native.Destroy(optionalsOnly: true);
            var result = new Derived { text = "sentinel" };
            native.FromNative(result, keysOnly);

            // Assert
            result.id.ShouldBe(17);
            result.note.ShouldBeNull();
            result.extra.ShouldBeNull();
            if (keysOnly)
            {
                result.text.ShouldBe("sentinel");
            }
            else
            {
                result.text.ShouldBe("required");
            }
        }
        finally
        {
            native.Destroy(optionalsOnly: false);
        }
    }

    /// <summary>Checks optional-only cleanup on nested collections and optional arrays.</summary>
    [Fact]
    public void CleansOptionalCollectionsAndKeepsRequiredValues()
    {
        // Arrange
        var native = new CollectionsUnmanaged();
        try
        {
            native.Initialize();
            native.ToNative(CollectionSamples.Create(present: true));

            // Act
            native.Destroy(optionalsOnly: true);
            var result = new Collections();
            native.FromNative(result);

            // Assert
            result.child.text.ShouldBe("nested");
            result.numbers.ShouldBe([1, 2, 3]);
            result.points[1].text.ShouldBe("second");
            result.optionalNumber.ShouldBeNull();
            result.optionalText.ShouldBeNull();
            result.optionalChild.ShouldBeNull();
            result.optionalNumbers.ShouldBeNull();
            result.optionalPoints.ShouldBeNull();
            result.optionalState.ShouldBeNull();
            result.optionalWideText.ShouldBeNull();
            result.optionalWideTexts.ShouldBeNull();
            result.optionalMatrix.ShouldBeNull();
        }
        finally
        {
            native.Destroy(optionalsOnly: false);
        }
    }

    /// <summary>Exercises native valuetype inheritance.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ConvertsInheritedValueTypes(bool keysOnly)
    {
        // Arrange
        var source = new DerivedValue { id = 17, text = "value" };

        // Act
        var result = NativeSample.RoundTrip<DerivedValue, DerivedValueUnmanaged>(source, keysOnly: keysOnly);

        // Assert
        result.id.ShouldBe(17);
        result.text.ShouldBe("value");
    }
}
