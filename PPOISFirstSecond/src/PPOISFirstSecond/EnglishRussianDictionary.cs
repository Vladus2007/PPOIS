using Microsoft.VisualBasic;
using PPOISFirstSecond;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents an English-Russian dictionary implemented as a binary search tree.
/// Provides functionality for inserting, finding, and deleting word translations,
/// as well as reading from and writing to a database.
/// </summary>
public class EnglishRussianDictionary : IInsert
{
    /// <summary>
    /// Gets the root node of the binary search tree.
    /// </summary>
    public Node? Root { get; private set; }

    private readonly IReadFromDatabase _read;
    private readonly IWriteToDatabase _write;

    /// <summary>
    /// Initializes a new instance of the EnglishRussianDictionary class with specified database readers and writers.
    /// </summary>
    /// <param name="read">The database reader implementation for loading dictionary data.</param>
    /// <param name="write">The database writer implementation for saving dictionary data.</param>
    public EnglishRussianDictionary(IReadFromDatabase read, IWriteToDatabase write)
    {
        _read = read;
        _write = write;
    }

    /// <summary>
    /// Reads all word pairs from the database and inserts them into the dictionary.
    /// </summary>
    public void Read()
    {
        var inf = _read.Read();
        foreach (var i in inf)
        {
            Insert(i.Values, i.Key);
        }
    }

    /// <summary>
    /// Writes a collection of word pairs to the database.
    /// </summary>
    /// <param name="wordPairs">The collection of word pairs to write to the database.</param>
    public void Write(IEnumerable<WordPair> wordPairs)
    {
        foreach (var wordpair in wordPairs)
        {
            _write.WriteDatabase(wordpair);
        }
    }

    /// <summary>
    /// Writes a single word pair to the database.
    /// </summary>
    /// <param name="wordPair">The word pair to write to the database.</param>
    public void Write(WordPair wordPair)
    {
        _write.WriteDatabase(wordPair);
    }

    /// <summary>
    /// Inserts or updates a word translation in the dictionary.
    /// </summary>
    /// <param name="value">The Russian translation of the word.</param>
    /// <param name="key">The English word to insert or update.</param>
    /// <returns>True if the operation was successful.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either value or key is null.</exception>
    public bool Insert(string value, string key)
    {
        if (value == null && key == null) throw new ArgumentNullException();
        if (Root == null)
        {
            Root = new Node { Key = key, Value = value };
            return true;
        }

        Node? parent;
        Node? current = FindNodeAndParent(key, out parent);

        if (current != null) // Ключ найден - обновляем значение
        {
            current.Value = value;
            return true;
        }

        var newNode = new Node { Key = key, Value = value };
        int cmp = string.CompareOrdinal(key, parent!.Key);

        if (cmp < 0)
            parent.left = newNode;
        else
            parent.right = newNode;

        return true;
    }

    /// <summary>
    /// Finds the Russian translation for the specified English word.
    /// </summary>
    /// <param name="key">The English word to search for.</param>
    /// <returns>The Russian translation if found; otherwise, null.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    public string? Find(string key)
    {
        if (key == null) throw new ArgumentNullException();
        Node? current = Root;
        while (current != null)
        {
            int cmp = string.CompareOrdinal(key, current.Key);
            if (cmp == 0)
                return current.Value;
            else if (cmp < 0)
                current = current.left;
            else
                current = current.right;
        }
        return null;
    }

    /// <summary>
    /// Gets or sets the Russian translation for the specified English word using indexer syntax.
    /// </summary>
    /// <param name="key">The English word to access.</param>
    /// <returns>The Russian translation if found during get operation; null otherwise.</returns>
    public string? this[string key]
    {
        get
        {
            return Find(key);
        }
        set
        {
            Insert(value, key);
        }
    }

    /// <summary>
    /// Deletes the specified English word and its translation from the dictionary.
    /// </summary>
    /// <param name="key">The English word to delete.</param>
    /// <returns>True if the word was found and deleted; false otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    public bool Delete(string key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (Root == null) return false;

        Node? parent;
        Node? node = FindNodeAndParent(key, out parent);
        if (node == null) return false;

        if (node.left == null && node.right == null)
            RemoveLeaf(node, parent);
        else if (node.left == null || node.right == null)
            RemoveNodeWithOneChild(node, parent);
        else
            RemoveNodeWithTwoChildren(node);

        return true;
    }

    private Node? FindNodeAndParent(string key, out Node? parent)
    {
        parent = null;
        Node? current = Root;
        while (current != null && current.Key != key)
        {
            parent = current;
            int cmp = string.CompareOrdinal(key, current.Key);
            current = (cmp < 0) ? current.left : current.right;
        }
        return current;
    }

    private void RemoveLeaf(Node node, Node? parent)
    {
        if (parent == null)
            Root = null;
        else if (parent.left == node)
            parent.left = null;
        else
            parent.right = null;
    }

    private void RemoveNodeWithOneChild(Node node, Node? parent)
    {
        Node? child = node.left ?? node.right;
        if (parent == null)
            Root = child;
        else if (parent.left == node)
            parent.left = child;
        else
            parent.right = child;
    }

    private void RemoveNodeWithTwoChildren(Node node)
    {
        Node? succParent = node;
        Node? succ = node.right;
        while (succ.left != null)
        {
            succParent = succ;
            succ = succ.left;
        }
        node.Key = succ.Key;
        node.Value = succ.Value;
        if (succParent.left == succ)
            succParent.left = succ.right;
        else
            succParent.right = succ.right;
    }

    /// <summary>
    /// Deletes the specified English word from the dictionary using the subtraction operator.
    /// </summary>
    /// <param name="dic">The dictionary from which to delete the word.</param>
    /// <param name="key">The English word to delete.</param>
    /// <returns>Always returns true.</returns>
    public static bool operator -(EnglishRussianDictionary dic, string key)
    {
        dic.Delete(key);
        return true;
    }

    /// <summary>
    /// Gets the total number of word pairs in the dictionary.
    /// </summary>
    /// <returns>The number of word pairs in the dictionary.</returns>
    public int Length()
    {
        int length = 0;
        return Run(Root, ref length);
    }

    /// <summary>
    /// Recursively traverses the binary tree and counts the number of nodes.
    /// </summary>
    /// <param name="node">The current node being processed.</param>
    /// <param name="Length">Reference to the length counter.</param>
    /// <returns>The total number of nodes in the subtree.</returns>
    public int Run(Node node, ref int Length)
    {
        Length++;
        if (node == null) return 0;
        Run(node.left, ref Length);
        Run(node.right, ref Length);
        return Length;
    }
}

/// <summary>
/// Represents a node in the binary search tree used by EnglishRussianDictionary.
/// Each node contains an English word (Key) and its Russian translation (Value).
/// </summary>
public class Node
{
    /// <summary>
    /// Gets or sets the right child node in the binary tree.
    /// </summary>
    public Node? right { get; set; }

    /// <summary>
    /// Gets or sets the left child node in the binary tree.
    /// </summary>
    public Node? left { get; set; }

    /// <summary>
    /// Gets or sets the English word (key) stored in this node.
    /// </summary>
    public string? Key { get; set; }

    /// <summary>
    /// Gets or sets the Russian translation (value) stored in this node.
    /// </summary>
    public string? Value { get; set; }
}