﻿using Microsoft.Data.Sqlite;
using NUnit.Framework;
using SimpleMigrations;
using SimpleMigrations.VersionProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Simple.Migrations.IntegrationTests.SQLite
{
    [TestFixture]
    public class SQLiteTests
    {
        private SqliteConnection connection;
        private SimpleMigrator migrator;

        [SetUp]
        public void SetUp()
        {
            this.connection = new SqliteConnection(ConnectionStrings.SQLite);
            var migrationProvider = new CustomMigrationProvider(typeof(AddTable));
            this.migrator = new SimpleMigrator(migrationProvider, this.connection, new SQLiteVersionProvider(), new NUnitLogger());

            this.migrator.Load();
        }

        [TearDown]
        public void TearDown()
        {
            this.migrator.MigrateTo(0);
            new SqliteCommand(@"DROP TABLE VersionInfo", this.connection).ExecuteNonQuery();
        }

        [Test]
        public void RunMigration()
        {
            this.migrator.MigrateToLatest();
        }
    }
}
