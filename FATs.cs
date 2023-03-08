using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AutomatedTesting;

//                               //
//  FACTS, ASSERTIONS, THEORIES  //
//                               //

public class FactTest
{
    [Fact]
    public void Should_be_equal()
    {
        var expectedValue = 2;
        var actualValue = 2;
        Assert.Equal(expectedValue, actualValue);
    }
}

public class AsyncFactTest
{
    [Fact]
    public async Task Should_be_equal()
    {
        var expectedValue = 2;
        var actualValue = 2;
        await Task.Yield();
        Assert.Equal(expectedValue, actualValue);
    }
}

public class AssertionTest
{
    [Fact]
    public void xUnit_Assertions()
    {
        object o1 = new { Name = "Object 1" };
        object o2 = new { Name = "Object 1" };
        object o3 = o1;
        object? o4 = o1;

        // Verify objects are same instance
        Assert.Same(expected: o1, actual: o3);
        Assert.Same(expected: o4, actual: o1);

        Assert.NotNull(o4);

        var e = Assert.Throws<CustomException>(
            testCode: () => OperationThatThrows("Test Exception")
        );

        Assert.Equal(expected: "Test Exception", actual: e.Name);
    }

    static void OperationThatThrows(string name)
    {
        throw new CustomException() { Name = name };
    }

    private class CustomException : Exception
    {
        public string? Name { get; set; }
    }
}

public class TheoryTest
{
    [Theory]
    [InlineData(1,1)]
    [InlineData(2,2)]
    [InlineData(5,5)]
    public void Should_be_equal(int val1, int val2) // Runs 3 tests
    {
        Assert.Equal(val1, val2);
    }

    [Theory]
    [MemberData(nameof(NumsWithEqualOrNot))]
    public void Should_be_equal_complex(int val1, int val2, bool shouldBeEqual)
    {
        if(shouldBeEqual) Assert.Equal(val1, val2);
        else Assert.NotEqual(val1, val2);
    }

    public static IEnumerable<object[]> NumsWithEqualOrNot => new[]
    {
        // Each array element in this object array is mapped to test method params
        new object[] { 1, 2, false },
        new object[] { 2, 2, true },
        new object[] { 3, 3, true }
    };

    [Theory]
    [ClassData(typeof(TheoryDataClass))]
    public void Should_be_equal_class_data(int val1, int val2, bool shouldBeEqual)
    {
        if(shouldBeEqual) Assert.Equal(val1, val2);
        else Assert.NotEqual(val1, val2);
    }

    public class TheoryDataClass : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1, 2, false };
            yield return new object[] { 2, 2, true };
            yield return new object[] { 3, 3, true };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}