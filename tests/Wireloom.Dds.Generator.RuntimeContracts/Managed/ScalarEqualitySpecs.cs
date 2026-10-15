using RuntimeManaged;

namespace Wireloom.Dds.Generator.RuntimeContracts.Managed;

/// <summary>Checks that every scalar member participates in generated equality.</summary>
public sealed class ScalarEqualitySpecs
{
    /// <summary>Checks each distinct scalar comparison branch independently.</summary>
    [Theory]
    [InlineData("flag")]
    [InlineData("byte")]
    [InlineData("int8")]
    [InlineData("uint8")]
    [InlineData("short")]
    [InlineData("ushort")]
    [InlineData("int")]
    [InlineData("uint")]
    [InlineData("long")]
    [InlineData("ulong")]
    [InlineData("float")]
    [InlineData("double")]
    [InlineData("long-double")]
    [InlineData("char")]
    [InlineData("wchar")]
    [InlineData("enum")]
    [InlineData("string")]
    [InlineData("wstring")]
    [InlineData("default")]
    public void DetectsEachChangedMember(string member)
    {
        // Arrange
        var sample = new Scalars();
        var copy = new Scalars(sample);

        // Act
        switch (member)
        {
            case "flag":
                sample.flag = true;
                break;
            case "byte":
                sample.byteValue = 1;
                break;
            case "int8":
                sample.signedByte = -1;
                break;
            case "uint8":
                sample.unsignedByte = 1;
                break;
            case "short":
                sample.shortValue = -1;
                break;
            case "ushort":
                sample.unsignedShort = 1;
                break;
            case "int":
                sample.intValue = -1;
                break;
            case "uint":
                sample.unsignedInt = 1;
                break;
            case "long":
                sample.longValue = -1;
                break;
            case "ulong":
                sample.unsignedLong = 1;
                break;
            case "float":
                sample.singleValue = 1;
                break;
            case "double":
                sample.doubleValue = 1;
                break;
            case "long-double":
                sample.extendedValue = (Rti.Types.LongDouble)1;
                break;
            case "char":
                sample.character = 'x';
                break;
            case "wchar":
                sample.wideCharacter = 'Ω';
                break;
            case "enum":
                sample.state = State.Ready;
                break;
            case "string":
                sample.text = "changed";
                break;
            case "wstring":
                sample.wideText = "世界";
                break;
            case "default":
                sample.defaultNumber = 99;
                break;
            default:
                throw new ArgumentException("Unknown member", nameof(member));
        }

        // Assert
        sample.Equals(copy).ShouldBeFalse(member);
        copy.Equals(sample).ShouldBeFalse(member);
    }
}
