using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Internal.Commands;
using ServerApp.Data;
using ServerApp.Mapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace ServerApp.Tests
{
    public class BaseTest : IDisposable
    {
        private DbConnection connection;

        protected AppDbContext context;

        protected IMapper mapper;        

        public virtual void SetUp()
        {
            connection = CreateInMemoryDatabase();
            context = new AppDbContext(
                new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options);
            mapper = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>()).CreateMapper();
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

        public virtual void Dispose()
        {
            context.Dispose();
            connection.Dispose();
        }
    }
}
