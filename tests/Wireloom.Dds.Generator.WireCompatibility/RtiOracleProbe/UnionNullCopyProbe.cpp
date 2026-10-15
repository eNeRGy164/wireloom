#include "unions.hpp"
#include "union-null-copy.hpp"
#include <cstddef>
#include <iostream>
#include <type_traits>

// A C++ reference copy has no null-source equivalent. Never dereference a null pointer.
template<typename T>
void observe(const char* shape)
{
    // Arrange
    const T normal;

    // Act
    const T copy(normal);

    // Assert/report: expose the actual defaults and constructibility, without imposing C# behavior.
    std::cout << shape << " | default=" << static_cast<int>(T::default_discriminator())
              << " | discriminator=" << static_cast<int>(normal._d())
              << " | copy-discriminator=" << static_cast<int>(copy._d())
              << " | equal=" << std::boolalpha << (copy == normal)
              << " | nullptr-constructible=" << std::is_constructible<T, std::nullptr_t>::value << '\n';
}

int main()
{
    using namespace RuntimeUnions;
    using namespace RuntimeUnionNullCopy;

    observe<Choice>("choice");
    observe<Sparse>("sparse");
    observe<ZeroScalar>("zero-scalar");
    observe<ZeroString>("zero-string");
    observe<ZeroAggregate>("zero-aggregate");
    observe<ZeroSequence>("zero-sequence");
    observe<EnumNoDefault>("enum-no-default");
    observe<EnumWithDefault>("enum-with-default");
    observe<NegativeLabels>("negative-labels");
    observe<NegativeAndZero>("negative-and-zero");
    observe<NegativeEnum>("negative-enum");
    observe<MixedEnum>("mixed-enum");

    const ZeroAggregate aggregate;
    const ZeroSequence sequence;
    std::cout << "default-branches | child.id=" << aggregate.child().id
              << " | numbers.size=" << sequence.numbers().size() << '\n';

    Choice source;
    source.text("copied", 2);
    const Choice copy(source);
    std::cout << "populated-copy-control | discriminator=" << copy._d()
              << " | text=" << copy.text() << " | equal=" << (copy == source) << '\n';

    NegativeLabels negative;
    negative.number(23, -1);
    const NegativeLabels negative_copy(negative);
    std::cout << "negative-copy-control | discriminator=" << negative_copy._d()
              << " | number=" << negative_copy.number() << " | equal=" << (negative_copy == negative) << '\n';
}
