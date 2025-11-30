using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphLibrary
{
    public interface IGraphTraits<T>
    {
        bool Equals(T x, T y);
        int GetHashCode(T obj);
    }

    public class DefaultGraphTraits<T> : IGraphTraits<T>
    {
        public bool Equals(T x, T y) => EqualityComparer<T>.Default.Equals(x, y);
        public int GetHashCode(T obj) => EqualityComparer<T>.Default.GetHashCode(obj);
    }

    public class DirectedWirthGraph<T, TTraits> where TTraits : IGraphTraits<T>, new()
    {
        private readonly List<Vertex> _vertices = new();
        private readonly TTraits _traits = new();

        public class Vertex
        {
            public T Value { get; }
            public List<Edge> OutgoingEdges { get; } = new();

            public Vertex(T value) => Value = value;
        }

        public class Edge
        {
            public Vertex From { get; }
            public Vertex To { get; }

            public Edge(Vertex from, Vertex to)
            {
                From = from;
                To = to;
            }
        }

        // Основные методы графа
        public Vertex AddVertex(T value)
        {
            var vertex = new Vertex(value);
            _vertices.Add(vertex);
            return vertex;
        }

        public void RemoveVertex(Vertex vertex)
        {
            foreach (var v in _vertices)
                v.OutgoingEdges.RemoveAll(e => e.To == vertex);

            _vertices.Remove(vertex);
        }

        public Edge AddEdge(Vertex from, Vertex to)
        {
            if (!_vertices.Contains(from) || !_vertices.Contains(to))
                throw new ArgumentException("Vertices must be in graph");

            var edge = new Edge(from, to);
            from.OutgoingEdges.Add(edge);
            return edge;
        }

        public void RemoveEdge(Edge edge) => edge.From.OutgoingEdges.Remove(edge);

        public bool ContainsVertex(Vertex vertex) => _vertices.Contains(vertex);

        public bool ContainsEdge(Vertex from, Vertex to) =>
            from.OutgoingEdges.Any(e => e.To == to);

        public int VertexCount => _vertices.Count;
        public int EdgeCount => _vertices.Sum(v => v.OutgoingEdges.Count);

        // Итераторы
        public IEnumerable<Vertex> Vertices => _vertices;

        public IEnumerable<Edge> Edges => _vertices.SelectMany(v => v.OutgoingEdges);

        public IEnumerable<Vertex> AdjacentVertices(Vertex vertex) =>
            vertex.OutgoingEdges.Select(e => e.To);

        public IEnumerable<Edge> IncidentEdges(Vertex vertex) =>
            vertex.OutgoingEdges;

        
        public void Clear() => _vertices.Clear();
        public bool Empty => _vertices.Count == 0;
    }
}