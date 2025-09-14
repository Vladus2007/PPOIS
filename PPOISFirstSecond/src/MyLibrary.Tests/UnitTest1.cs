using Microsoft.EntityFrameworkCore;
using Moq;
using PPOISFirstSecond;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit;

namespace Test
{
    public class EnglishRussianDictionaryTests
    {
        private readonly Mock<IReadFromDatabase> _readMock;
        private readonly Mock<IWriteToDatabase> _writeMock;
        private readonly EnglishRussianDictionary _dictionary;

        public EnglishRussianDictionaryTests()
        {
            _readMock = new Mock<IReadFromDatabase>();
            _writeMock = new Mock<IWriteToDatabase>();
            _dictionary = new EnglishRussianDictionary(_readMock.Object, _writeMock.Object);
        }

        [Fact]
        public void Insert_FirstNode_SetsRoot()
        {
            bool result = _dictionary.Insert("Значение", "Ключ");
            Assert.True(result);
            Assert.NotNull(_dictionary.Root);
            Assert.Equal("Ключ", _dictionary.Root.Key);
            Assert.Equal("Значение", _dictionary.Root.Value);
        }

        [Fact]
        public void Insert_UpdateExistingNode_UpdatesValue()
        {
            _dictionary.Insert("Значение1", "Ключ");
            bool result = _dictionary.Insert("Значение2", "Ключ");

            Assert.True(result);
            Assert.Equal("Значение2", _dictionary.Root.Value);
        }

        [Fact]
        public void Insert_AddLeftAndRightNodes_CheckStructure()
        {
            // Root key
            _dictionary.Insert("RootVal", "M");

            // Insert left child (key < Root.Key)
            _dictionary.Insert("LeftVal", "A");

            // Insert right child (key > Root.Key)
            _dictionary.Insert("RightVal", "Z");

            Assert.NotNull(_dictionary.Root.left);
            Assert.NotNull(_dictionary.Root.right);
            Assert.Equal("A", _dictionary.Root.left.Key);
            Assert.Equal("Z", _dictionary.Root.right.Key);
        }

        [Fact]
        public void Find_KeyExists_ReturnsValue()
        {
            _dictionary.Insert("Value", "Key");
            var result = _dictionary.Find("Key");
            Assert.Equal("Value", result);
        }

        [Fact]
        public void Find_KeyNotExists_ReturnsNull()
        {
            _dictionary.Insert("Value", "Key");
            var result = _dictionary.Find("Absent");
            Assert.Null(result);
        }

        [Fact]
        public void Indexer_Get_ReturnsValue()
        {
            _dictionary.Insert("Value", "Key");
            Assert.Equal("Value", _dictionary["Key"]);
        }

        [Fact]
        public void Indexer_Set_InsertsOrUpdatesValue()
        {
            _dictionary["Key"] = "Value1";
            Assert.Equal("Value1", _dictionary["Key"]);

            _dictionary["Key"] = "Value2";
            Assert.Equal("Value2", _dictionary["Key"]);
        }

        [Fact]
        public void Delete_LeafNode_RemovesNode()
        {
            _dictionary.Insert("Root", "M");
            _dictionary.Insert("Left", "A");
            var deleted = _dictionary.Delete("A");

            Assert.True(deleted);
            Assert.Null(_dictionary.Root.left);
        }

        [Fact]
        public void Delete_NodeWithOneChild_RemovesNodeAndReconnects()
        {
            _dictionary.Insert("Root", "M");
            _dictionary.Insert("Left", "A");
            _dictionary.Insert("LeftRight", "B");

            bool deleted = _dictionary.Delete("A");

            Assert.True(deleted);
            Assert.NotNull(_dictionary.Root.left);
            Assert.Equal("B", _dictionary.Root.left.Key);
        }

        [Fact]
        public void Delete_NodeWithTwoChildren_RemovesNodeCorrectly()
        {
            _dictionary.Insert("Root", "M");
            _dictionary.Insert("Left", "A");
            _dictionary.Insert("Right", "Z");
            _dictionary.Insert("LeftRight", "B");
            _dictionary.Insert("RightLeft", "N");

            // delete node "M" (root) which has 2 children
            bool deleted = _dictionary.Delete("M");

            Assert.True(deleted);
            Assert.NotEqual("M", _dictionary.Root.Key);
            // Successor could be either "N" or "B" depending on implementation
        }

        [Fact]
        public void Delete_NonExistingKey_ReturnsFalse()
        {
            _dictionary.Insert("Value", "Key");
            bool deleted = _dictionary.Delete("NonExist");
            Assert.False(deleted);
        }

        [Fact]
        public void Delete_NullKey_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _dictionary.Delete(null!));
        }

        [Fact]
        public void OperatorMinus_DeletesKey()
        {
            _dictionary.Insert("V", "Key");
            var result = _dictionary - "Key";
            Assert.True(result);
            Assert.Null(_dictionary.Find("Key"));
        }

        [Fact]
        public void Length_EmptyDictionary_ReturnsZero()
        {
            int length = _dictionary.Length();
            Assert.Equal(0, length);
        }

        
       

        [Fact]
       
        public void Read_ReadsFromDatabaseAndInserts()
        {
            var data = new List<WordPair>
    {
        new WordPair { Key = "Key1", Values = "Value1" },
        new WordPair { Key = "Key2", Values = "Value2" }
    };

            // Setup mock to return data
            _readMock.Setup(r => r.Read()).Returns(data);

            _dictionary.Read();

            Assert.Equal("Value1", _dictionary.Find("Key1"));
            Assert.Equal("Value2", _dictionary.Find("Key2"));
        }

        [Fact]
        public void WriteEnumerable_CallsWriteDatabaseForEachItem()
        {
            var wordPairs = new List<WordPair>
            {
                new WordPair { Key = "Key1", Values = "Val1" },
                new WordPair { Key = "Key2", Values = "Val2" }
            };

            _dictionary.Write(wordPairs);

            _writeMock.Verify(w => w.WriteDatabase(It.IsAny<WordPair>()), Times.Exactly(2));
        }

        [Fact]
        public void WriteSingle_CallsWriteDatabaseOnce()
        {
            var wp = new WordPair { Key = "Key", Values = "Val" };
            _dictionary.Write(wp);
            _writeMock.Verify(w => w.WriteDatabase(wp), Times.Once);
        }

        [Fact]
        public void Insert_WithNullRoot_AddsRoot()
        {
            var dict = new EnglishRussianDictionary(_readMock.Object, _writeMock.Object);
            bool inserted = dict.Insert("Value", "Key");
            Assert.True(inserted);
            Assert.NotNull(dict.Root);
        }

        [Fact]
        public void Insert_MaintainsBST()
        {
            _dictionary.Insert("M", "M");
            _dictionary.Insert("A", "A");
            _dictionary.Insert("Z", "Z");
            _dictionary.Insert("B", "B");

            Assert.Equal("A", _dictionary.Root.left.Key);
            Assert.Equal("B", _dictionary.Root.left.right.Key);
        }

        [Fact]
        public void Insert_DuplicateKey_UpdatesValue()
        {
            _dictionary.Insert("Value1", "Key");
            _dictionary.Insert("Value2", "Key");

            Assert.Equal("Value2", _dictionary.Find("Key"));
        }

        [Fact]
        public void Find_NullKey_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _dictionary.Find(null!));
        }

        [Fact]
        public void Indexer_GetNonExistingKey_ReturnsNull()
        {
            Assert.Null(_dictionary["NonExistingKey"]);
        }

       
       
        private int CountNodes(Node node)
        {
            if (node == null)
                return 0;

            return 1 + CountNodes(node.left) + CountNodes(node.right);
        }

        private Node? CallFindNodeAndParent(EnglishRussianDictionary dic, string key, out Node? parent)
        {
            var method = typeof(EnglishRussianDictionary).GetMethod("FindNodeAndParent",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            object?[] parameters = { key, null };
            var result = method!.Invoke(dic, parameters);

            parent = (Node?)parameters[1];
            return (Node?)result;
        }

        private void CallRemoveLeaf(EnglishRussianDictionary dic, Node node, Node parent)
        {
            var method = typeof(EnglishRussianDictionary).GetMethod("RemoveLeaf",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            method!.Invoke(dic, new object[] { node, parent });
        }

        private void CallRemoveNodeWithOneChild(EnglishRussianDictionary dic, Node node, Node parent)
        {
            var method = typeof(EnglishRussianDictionary).GetMethod("RemoveNodeWithOneChild",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            method!.Invoke(dic, new object[] { node, parent });
        }

        private void CallRemoveNodeWithTwoChildren(EnglishRussianDictionary dic, Node node)
        {
            var method = typeof(EnglishRussianDictionary).GetMethod("RemoveNodeWithTwoChildren",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            method!.Invoke(dic, new object[] { node });
        }

        private int CallRun(EnglishRussianDictionary dic, Node node, ref int length)
        {
            var method = typeof(EnglishRussianDictionary).GetMethod("Run",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            object?[] parameters = { node, length };
            var result = method!.Invoke(dic, parameters);

            length = (int)parameters[1];
            return (int)result;
        }
        private Mock<DbSet<WordPair>> CreateMockDbSet(List<WordPair> data)
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<WordPair>>();

            mockSet.As<IQueryable<WordPair>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<WordPair>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<WordPair>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<WordPair>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());

            return mockSet;
        }

        private (Mock<IReadFromDatabase>, Mock<IWriteToDatabase>) CreateMockServices()
        {
            var mockRead = new Mock<IReadFromDatabase>();
            var mockWrite = new Mock<IWriteToDatabase>();
            return (mockRead, mockWrite);
        }

     

        

        [Fact]
        public void EnglishRussianDictionary_Read_LoadsDataFromDatabase()
        {
            // Arrange
            var (mockRead, mockWrite) = CreateMockServices();

            var dictionaryData = new List<WordPair>
        {
            new WordPair { Key = "hello", Values = "привет" },
            new WordPair { Key = "world", Values = "мир" }
        };

            mockRead.Setup(r => r.Read()).Returns(dictionaryData);
            var dictionary = new EnglishRussianDictionary(mockRead.Object, mockWrite.Object);

            // Act
            dictionary.Read();

            // Assert
            Assert.NotNull(dictionary.Root);
            Assert.Equal("привет", dictionary.Find("hello"));
            Assert.Equal("мир", dictionary.Find("world"));
        }

        [Fact]
        public void EnglishRussianDictionary_Write_SavesWordPairsToDatabase()
        {
            // Arrange
            var (mockRead, mockWrite) = CreateMockServices();
            var dictionary = new EnglishRussianDictionary(mockRead.Object, mockWrite.Object);

            var wordPairs = new List<WordPair>
        {
            new WordPair { Key = "test", Values = "тест" },
            new WordPair { Key = "data", Values = "данные" }
        };

            // Act
            dictionary.Write(wordPairs);

            // Assert
            mockWrite.Verify(w => w.WriteDatabase(It.IsAny<WordPair>()), Times.Exactly(2));
        }

        [Fact]
        public void EnglishRussianDictionary_WriteSingleWordPair_SavesToDatabase()
        {
            // Arrange
            var (mockRead, mockWrite) = CreateMockServices();
            var dictionary = new EnglishRussianDictionary(mockRead.Object, mockWrite.Object);
            var wordPair = new WordPair { Key = "single", Values = "одиночный" };

            // Act
            dictionary.Write(wordPair);

            // Assert
            mockWrite.Verify(w => w.WriteDatabase(It.IsAny<WordPair>()), Times.Once());
        }

        [Fact]
        public void EnglishRussianDictionary_Insert_AddsNewWord()
        {
            // Arrange
            var (mockRead, mockWrite) = CreateMockServices();
            var dictionary = new EnglishRussianDictionary(mockRead.Object, mockWrite.Object);

            // Act
            var result = dictionary.Insert("дом", "house");

            // Assert
            Assert.True(result);
            Assert.Equal("дом", dictionary.Find("house"));
        }

        [Fact]
        public void EnglishRussianDictionary_Insert_DuplicateKeyUpdatesValue()
        {
            // Arrange
            var (mockRead, mockWrite) = CreateMockServices();
            var dictionary = new EnglishRussianDictionary(mockRead.Object, mockWrite.Object);
            dictionary.Insert("first", "первый");

            // Act
            var result = dictionary.Insert("updated", "первый"); // Same key, different value

            // Assert
            Assert.True(result);
            Assert.Equal("updated", dictionary.Find("первый"));
        }

        [Fact]
        public void EnglishRussianDictionary_Delete_RemovesWord()
        {
            // Arrange
            var (mockRead, mockWrite) = CreateMockServices();
            var dictionary = new EnglishRussianDictionary(mockRead.Object, mockWrite.Object);
            dictionary.Insert("дом", "house");

            // Act
            var result = dictionary.Delete("house");

            // Assert
            Assert.True(result);
            Assert.Null(dictionary.Find("house"));
        }

        [Fact]
        public void EnglishRussianDictionary_Delete_NonExistentKeyReturnsFalse()
        {
            // Arrange
            var (mockRead, mockWrite) = CreateMockServices();
            var dictionary = new EnglishRussianDictionary(mockRead.Object, mockWrite.Object);

            // Act
            var result = dictionary.Delete("nonexistent");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EnglishRussianDictionary_Indexer_GetSetWorks()
        {
            // Arrange
            var (mockRead, mockWrite) = CreateMockServices();
            var dictionary = new EnglishRussianDictionary(mockRead.Object, mockWrite.Object);

            // Act - Set using indexer
            dictionary["computer"] = "компьютер";

            // Assert - Get using indexer
            Assert.Equal("компьютер", dictionary["computer"]);
        }

        [Fact]
        public void EnglishRussianDictionary_Indexer_GetNonExistentKeyReturnsNull()
        {
            // Arrange
            var (mockRead, mockWrite) = CreateMockServices();
            var dictionary = new EnglishRussianDictionary(mockRead.Object, mockWrite.Object);

            // Act & Assert
            Assert.Null(dictionary["nonexistent"]);
        }

       
        [Fact]
        public void EnglishRussianDictionary_Length_EmptyDictionaryReturnsZero()
        {
            // Arrange
            var (mockRead, mockWrite) = CreateMockServices();
            var dictionary = new EnglishRussianDictionary(mockRead.Object, mockWrite.Object);

            // Act
            var length = dictionary.Length();

            // Assert
            Assert.Equal(0, length);
        }

        [Fact]
        public void EnglishRussianDictionary_SubtractionOperator_RemovesWord()
        {
            // Arrange
            var (mockRead, mockWrite) = CreateMockServices();
            var dictionary = new EnglishRussianDictionary(mockRead.Object, mockWrite.Object);
            dictionary.Insert("test", "тест");

            // Act
            var result = dictionary - "тест";

            // Assert
            Assert.True(result);
            Assert.Null(dictionary.Find("тест"));
        }

        [Fact]
        public void EnglishRussianDictionary_Find_NonExistentKeyReturnsNull()
        {
            // Arrange
            var (mockRead, mockWrite) = CreateMockServices();
            var dictionary = new EnglishRussianDictionary(mockRead.Object, mockWrite.Object);

            // Act
            var result = dictionary.Find("nonexistent");

            // Assert
            Assert.Null(result);
        }

    
    }
}


