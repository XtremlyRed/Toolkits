using System.Collections;
using Toolkits.Core;

namespace Toolkits.Core.Tests;

[TestClass]
public class EnumerableExtensionsTests
{
    [Test]
    public void IsNullOrEmpty_Should_Return_True_When_Source_Is_Null()
    {
        IEnumerable<int> source = null!;
        bool result = source.IsNullOrEmpty();
        Assert.IsTrue(result);
    }

    [Test]
    public void IsNullOrEmpty_Should_Return_True_When_Source_Is_Empty()
    {
        IEnumerable<int> source = Enumerable.Empty<int>();
        bool result = source.IsNullOrEmpty();
        Assert.IsTrue(result);
    }

    [Test]
    public void IsNullOrEmpty_Should_Return_False_When_Source_Is_Not_Null_Or_Empty()
    {
        IEnumerable<int> source = new List<int> { 1, 2, 3 };
        bool result = source.IsNullOrEmpty();
        Assert.IsFalse(result);
    }

    [Test]
    public void IsNotNullOrEmpty_Should_Return_False_When_Source_Is_Null()
    {
        IEnumerable<int> source = null!;
        bool result = source.IsNotNullOrEmpty();
        Assert.IsFalse(result);
    }

    [Test]
    public void IsNotNullOrEmpty_Should_Return_False_When_Source_Is_Empty()
    {
        IEnumerable<int> source = Enumerable.Empty<int>();
        bool result = source.IsNotNullOrEmpty();
        Assert.IsFalse(result);
    }

    [Test]
    public void IsNotNullOrEmpty_Should_Return_True_When_Source_Is_Not_Null_Or_Empty()
    {
        IEnumerable<int> source = new List<int> { 1, 2, 3 };
        bool result = source.IsNotNullOrEmpty();
        Assert.IsTrue(result);
    }

    [Test]
    public void IsNullOrEmpty_Generic_Should_Return_True_When_Source_Is_Null()
    {
        IEnumerable<int> source = null!;
        bool result = source.IsNullOrEmpty<int>();
        Assert.IsTrue(result);
    }

    [Test]
    public void IsNullOrEmpty_Generic_Should_Return_True_When_Source_Is_Empty()
    {
        IEnumerable<int> source = Enumerable.Empty<int>();
        bool result = source.IsNullOrEmpty<int>();
        Assert.IsTrue(result);
    }

    [Test]
    public void IsNullOrEmpty_Generic_Should_Return_False_When_Source_Is_Not_Null_Or_Empty()
    {
        IEnumerable<int> source = new List<int> { 1, 2, 3 };
        bool result = source.IsNullOrEmpty<int>();
        Assert.IsFalse(result);
    }

    [Test]
    public void IsNotNullOrEmpty_Generic_Should_Return_False_When_Source_Is_Null()
    {
        IEnumerable<int> source = null!;
        bool result = source.IsNotNullOrEmpty<int>();
        Assert.IsFalse(result);
    }

    [Test]
    public void IsNotNullOrEmpty_Generic_Should_Return_False_When_Source_Is_Empty()
    {
        IEnumerable<int> source = Enumerable.Empty<int>();
        bool result = source.IsNotNullOrEmpty<int>();
        Assert.IsFalse(result);
    }

    [Test]
    public void IsNotNullOrEmpty_Generic_Should_Return_True_When_Source_Is_Not_Null_Or_Empty()
    {
        IEnumerable<int> source = new List<int> { 1, 2, 3 };
        bool result = source.IsNotNullOrEmpty<int>();
        Assert.IsTrue(result);
    }

    [Test]
    public void WhereIf_Should_Return_Filtered_Source_When_Condition_Is_True()
    {
        IEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5 };
        bool condition = true;
        IEnumerable<int> result = source.WhereIf(condition, x => x % 2 == 0);
        Assert.AreEqual(2, result.Count());
        Assert.IsTrue(result.Contains(2));
        Assert.IsTrue(result.Contains(4));
    }

    [Test]
    public void WhereIf_Should_Return_Source_When_Condition_Is_False()
    {
        IEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5 };
        bool condition = false;
        IEnumerable<int> result = source.WhereIf(condition, x => x % 2 == 0);
        Assert.AreEqual(5, result.Count());
        Assert.IsTrue(result.Contains(1));
        Assert.IsTrue(result.Contains(2));
        Assert.IsTrue(result.Contains(3));
        Assert.IsTrue(result.Contains(4));
        Assert.IsTrue(result.Contains(5));
    }

    [Test]
    public void IndexOf_Should_Return_Index_Of_First_Matching_Element()
    {
        IEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5 };
        int index = source.IndexOf(x => x == 3);
        Assert.AreEqual(2, index);
    }

    [Test]
    public void IndexOf_Should_Return_Minus_One_When_No_Matching_Element_Found()
    {
        IEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5 };
        int index = source.IndexOf(x => x == 6);
        Assert.AreEqual(-1, index);
    }

    [Test]
    public void IndexOfMany_Should_Return_Indices_Of_All_Matching_Elements()
    {
        IEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5, 3 };
        IEnumerable<int> indices = source.IndexOfMany(x => x == 3);
        Assert.AreEqual(2, indices.Count());
        Assert.IsTrue(indices.Contains(2));
        Assert.IsTrue(indices.Contains(5));
    }

    [Test]
    public void IndexOfMany_Should_Return_Empty_Collection_When_No_Matching_Element_Found()
    {
        IEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5 };
        IEnumerable<int> indices = source.IndexOfMany(x => x == 6);
        Assert.IsTrue(indices.IsNullOrEmpty());
    }

    [Test]
    public void Paginate_Should_Return_Correct_Page()
    {
        IEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        int pageIndex = 2;
        int pageSize = 3;
        IEnumerable<int> result = source.Paginate(pageIndex, pageSize);
        Assert.AreEqual(3, result.Count());
        Assert.IsTrue(result.Contains(4));
        Assert.IsTrue(result.Contains(5));
        Assert.IsTrue(result.Contains(6));
    }

    [Test]
    public void ForEach_Should_Execute_Action_For_Each_Element()
    {
        IEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5 };
        List<int> result = new List<int>();
        source.ForEach(x => result.Add(x * 2));
        Assert.AreEqual(5, result.Count);
        Assert.IsTrue(result.Contains(2));
        Assert.IsTrue(result.Contains(4));
        Assert.IsTrue(result.Contains(6));
        Assert.IsTrue(result.Contains(8));
        Assert.IsTrue(result.Contains(10));
    }

    [Test]
    public async Task ForEachAsync_Should_Execute_Action_For_Each_Element()
    {
        IEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5 };
        List<int> result = new List<int>();
        await source.ForEachAsync(async x =>
        {
            await Task.Delay(100);
            result.Add(x * 2);
        });
        Assert.AreEqual(5, result.Count);
        Assert.IsTrue(result.Contains(2));
        Assert.IsTrue(result.Contains(4));
        Assert.IsTrue(result.Contains(6));
        Assert.IsTrue(result.Contains(8));
        Assert.IsTrue(result.Contains(10));
    }

    [Test]
    public async Task ForEachAsync_With_Index_Should_Execute_Action_For_Each_Element_With_Index()
    {
        IEnumerable<int> source = new List<int> { 1, 2, 3, 4, 5 };
        List<int> result = new List<int>();
        await source.ForEachAsync(
            async (x, index) =>
            {
                await Task.Delay(100);
                result.Add(x * index);
            }
        );
        Assert.AreEqual(5, result.Count);
        Assert.IsTrue(result.Contains(0));
        Assert.IsTrue(result.Contains(2));
        Assert.IsTrue(result.Contains(6));
        Assert.IsTrue(result.Contains(12));
        Assert.IsTrue(result.Contains(20));
    }

    [Test]
    public void ForEach_Non_Generic_Should_Execute_Action_For_Each_Element()
    {
        IEnumerable source = new List<int> { 1, 2, 3, 4, 5 };
        List<int> result = new List<int>();
        source.ForEach<int>(x => result.Add(x * 2));
        Assert.AreEqual(5, result.Count);
        Assert.IsTrue(result.Contains(2));
        Assert.IsTrue(result.Contains(4));
        Assert.IsTrue(result.Contains(6));
        Assert.IsTrue(result.Contains(8));
        Assert.IsTrue(result.Contains(10));
    }

    [Test]
    public void Sort_Should_Sort_List_In_Ascending_Order()
    {
        List<int> source = new List<int> { 5, 3, 1, 4, 2 };
        source.Sort(x => x);
        Assert.AreEqual(1, source[0]);
        Assert.AreEqual(2, source[1]);
        Assert.AreEqual(3, source[2]);
        Assert.AreEqual(4, source[3]);
        Assert.AreEqual(5, source[4]);
    }

    [Test]
    public void Sort_Should_Sort_List_In_Descending_Order()
    {
        List<int> source = new List<int> { 5, 3, 1, 4, 2 };
        source.Sort(x => x, true);
        Assert.AreEqual(5, source[0]);
        Assert.AreEqual(4, source[1]);
        Assert.AreEqual(3, source[2]);
        Assert.AreEqual(2, source[3]);
        Assert.AreEqual(1, source[4]);
    }

    [Test]
    public void Sort_Should_Sort_Array_In_Ascending_Order()
    {
        int[] source = new int[] { 5, 3, 1, 4, 2 };
        source.Sort(x => x);
        Assert.AreEqual(1, source[0]);
        Assert.AreEqual(2, source[1]);
        Assert.AreEqual(3, source[2]);
        Assert.AreEqual(4, source[3]);
        Assert.AreEqual(5, source[4]);
    }

    [Test]
    public void Sort_Should_Sort_Array_In_Descending_Order()
    {
        int[] source = new int[] { 5, 3, 1, 4, 2 };
        source.Sort(x => x, true);
        Assert.AreEqual(5, source[0]);
        Assert.AreEqual(4, source[1]);
        Assert.AreEqual(3, source[2]);
        Assert.AreEqual(2, source[3]);
        Assert.AreEqual(1, source[4]);
    }
}
