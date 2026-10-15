#include "aliases.hpp"
#include <rti/topic/to_string.hpp>
#include <rti/util/ostream_operators.hpp>
#include <iostream>
#include <sstream>

template<typename T>
void print(const char* shape, const T& sample)
{
    std::ostringstream text;
    text << sample;
    std::cout << shape << " | topic-type=" << std::boolalpha
              << dds::topic::is_topic_type<T>::value << " | " << text.str() << "\n";
}

int main()
{
    using namespace RuntimeAliases;
    const Child child(17, "hello");
    const Numbers numbers {17};
    const Matrix matrix {{{17, 18, 19}, {20, 21, 22}}};

    print("scalar", Scalar(17));
    print("scalar-chain", ScalarChain(17));
    print("enum", EnumAlias(State::Ready));
    print("enum-chain", EnumChain(State::Ready));
    print("text", Text("hello"));
    print("wide-text", WideText(L"\u4e16\u754c"));
    print("numbers", numbers);
    print("numbers-chain", NumbersChain(numbers));
    print("children", Children {child});
    print("texts", Texts {"hello"});
    print("wide-texts", WideTexts {L"\u4e16\u754c"});
    print("vector", Vector {{17, 18}});
    print("matrix", matrix);
    print("matrix-chain", MatrixChain(matrix));
    print("points", Points {{child, Child(18, "other")}});
    print("aggregate-control", Aggregate(child));
    std::cout << "child-topic-to_string:\n" << rti::topic::to_string(child);
}
