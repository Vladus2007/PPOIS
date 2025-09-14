using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISFirstSecond
{
    /// <summary>
    /// Provides functionality for writing word pairs to a database using Entity Framework Core.
    /// Implements the IWriteToDatabase interface for database write operations.
    /// </summary>
    public class WriteToDatabase : IWriteToDatabase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the WriteToDatabase class with the specified database context.
        /// </summary>
        /// <param name="context">The Entity Framework database context used for data persistence operations.</param>
        public WriteToDatabase(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Writes a single word pair to the database and saves changes immediately.
        /// </summary>
        /// <param name="pair">The WordPair object to be written to the database.</param>
        public void WriteDatabase(WordPair pair)
        {
            _context.WordPairs.Add(pair);
            _context.SaveChanges();
        }
    }
}