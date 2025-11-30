using System.Collections.Generic;
using System.Linq;
using SortingAlgorithms;
using GraphLibrary;
using Xunit;

namespace Lab14Tests
{
    public class SortingTests
    {
        [Fact]
        public void HeapSort_IntArray_SortedCorrectly()
        {
            var array = new[] { 3, 1, 4, 1, 5, 9, 2, 6 };
            var expected = new[] { 1, 1, 2, 3, 4, 5, 6, 9 };

            HeapSort.Sort(array);

            Assert.Equal(expected, array);
        }

        [Fact]
        public void HeapSort_StringList_SortedCorrectly()
        {
            var list = new List<string> { "banana", "apple", "cherry" };
            var expected = new List<string> { "apple", "banana", "cherry" };

            HeapSort.Sort(list);

            Assert.Equal(expected, list);
        }

        [Fact]
        public void StoogeSort_IntArray_SortedCorrectly()
        {
            var array = new[] { 3, 1, 4, 1, 5, 9, 2, 6 };
            var expected = new[] { 1, 1, 2, 3, 4, 5, 6, 9 };

            StoogeSort.Sort(array);

            Assert.Equal(expected, array);
        }

        [Fact]
        public void StoogeSort_EmptyArray_NoError()
        {
            var array = new int[0];

            StoogeSort.Sort(array);

            Assert.Empty(array);
        }

        [Fact]
        public void CustomClass_SortingWorks()
        {
            var people = new[]
            {
                new Person("John", 25),
                new Person("Alice", 30),
                new Person("Bob", 20)
            };

            HeapSort.Sort(people);

            Assert.Equal("Bob", people[0].Name);
            Assert.Equal("John", people[1].Name);
            Assert.Equal("Alice", people[2].Name);
        }

        private class Person : System.IComparable<Person>
        {
            public string Name { get; }
            public int Age { get; }

            public Person(string name, int age) => (Name, Age) = (name, age);

            public int CompareTo(Person other) => Age.CompareTo(other.Age);
        }
    }

    public class GraphTests
    {
        [Fact]
        public void AddRemoveVertex_WorksCorrectly()
        {
            var graph = new DirectedWirthGraph<int, DefaultGraphTraits<int>>();

            var vertex = graph.AddVertex(42);
            Assert.True(graph.ContainsVertex(vertex));
            Assert.Equal(1, graph.VertexCount);

            graph.RemoveVertex(vertex);
            Assert.False(graph.ContainsVertex(vertex));
            Assert.Equal(0, graph.VertexCount);
        }

        [Fact]
        public void AddRemoveEdge_WorksCorrectly()
        {
            var graph = new DirectedWirthGraph<int, DefaultGraphTraits<int>>();

            var v1 = graph.AddVertex(1);
            var v2 = graph.AddVertex(2);

            var edge = graph.AddEdge(v1, v2);
            Assert.True(graph.ContainsEdge(v1, v2));
            Assert.Equal(1, graph.EdgeCount);

            graph.RemoveEdge(edge);
            Assert.False(graph.ContainsEdge(v1, v2));
            Assert.Equal(0, graph.EdgeCount);
        }

        [Fact]
        public void VertexIterator_WorksCorrectly()
        {
            var graph = new DirectedWirthGraph<string, DefaultGraphTraits<string>>();

            graph.AddVertex("A");
            graph.AddVertex("B");
            graph.AddVertex("C");

            Assert.Equal(3, graph.Vertices.Count());
        }

        [Fact]
        public void AdjacentVertices_WorksCorrectly()
        {
            var graph = new DirectedWirthGraph<int, DefaultGraphTraits<int>>();

            var v1 = graph.AddVertex(1);
            var v2 = graph.AddVertex(2);
            var v3 = graph.AddVertex(3);

            graph.AddEdge(v1, v2);
            graph.AddEdge(v1, v3);

            var adjacent = graph.AdjacentVertices(v1).ToList();
            Assert.Equal(2, adjacent.Count);
            Assert.Contains(v2, adjacent);
            Assert.Contains(v3, adjacent);
        }

        [Fact]
        public void Clear_WorksCorrectly()
        {
            var graph = new DirectedWirthGraph<int, DefaultGraphTraits<int>>();

            var v1 = graph.AddVertex(1);
            var v2 = graph.AddVertex(2);
            graph.AddEdge(v1, v2);

            graph.Clear();

            Assert.True(graph.Empty);
            Assert.Equal(0, graph.VertexCount);
            Assert.Equal(0, graph.EdgeCount);
        }
    }
}