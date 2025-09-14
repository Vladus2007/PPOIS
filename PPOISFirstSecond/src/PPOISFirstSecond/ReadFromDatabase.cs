using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore.Storage;

namespace PPOISFirstSecond
{
    /// <summary>
    /// Provides functionality for reading word pairs from a database using Entity Framework Core.
    /// Implements the IReadFromDatabase interface for database read operations.
    /// </summary>
    public class ReadFromDatabase : IReadFromDatabase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the ReadFromDatabase class with the specified database context.
        /// </summary>
        /// <param name="context">The Entity Framework database context used for data access operations.</param>
        public ReadFromDatabase(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Reads all word pairs from the database.
        /// </summary>
        /// <returns>An enumerable collection of WordPair objects containing all word translations from the database.</returns>
        public IEnumerable<WordPair> Read()
        {
            return _context.WordPairs.ToList();
        }
    }
}