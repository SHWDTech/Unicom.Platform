﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using Unicom.Platform.Model;

namespace Unicom.Platform.SQLite
{
    public class UnicomContext
    {
        public UnicomContext(string connstring)
        {
            _connectionString = connstring;
        }

        private readonly string _connectionString;

        public List<EmsDevice> Devices 
            => GetDbSet<EmsDevice>();

        public List<EmsProject> Projects
            => GetDbSet<EmsProject>();

        public List<EmsPrjCategory> PrjCategories
            => GetDbSet<EmsPrjCategory>();

        public List<EmsPrjType> PrjTypes
            => GetDbSet<EmsPrjType>();

        public List<EmsPrjPeriod> PrjPeriods
            => GetDbSet<EmsPrjPeriod>();


        private List<T> GetDbSet<T>() where T : class, new()
        {
            var tableName = typeof(T).Name;
            using (var conn = new SQLiteConnection(_connectionString))
            {
                var cmd = new SQLiteCommand($"SELECT * FROM {tableName}", conn);
                var adapter = new SQLiteDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);
                return table.ToListOf<T>();
            }
        }

        public int AddOrUpdate<T>(T model) where T : class, new()
        {
            return IsExist(model) ? Update(model) : Add(model);
        }

        public bool IsExist<T>(T model) where T : class, new()
        {
            var id = (long)model.GetType().GetProperty("Id").GetValue(model, null);

            var tableName = typeof(T).Name;
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SQLiteCommand($"SELECT Count(*) FROM {tableName} where Id = {id}", conn);

                return (long) cmd.ExecuteScalar() != 0;
            }
        }

        private int Add<T>(T model) where T : class, new()
        {
            var stametParams = InsertParams(model);
            var sql = $"INSERT INTO {typeof(T).Name} ({stametParams[0]}) VALUES ({stametParams[1]})";

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SQLiteCommand(sql, conn);

                return cmd.ExecuteNonQuery();
            }
        }

        private int Update<T>(T model) where T : class, new()
        {
            var stametParams = UpdateParams(model);
            var sql = $"UPDATE {typeof(T).Name} SET {stametParams}";

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SQLiteCommand(sql, conn);

                return cmd.ExecuteNonQuery();
            }
        }

        private string[] InsertParams<T>(T model) where T : class, new()
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var objectProperties = typeof(T).GetProperties(flags);
            var columns = new List<string>();
            var values = new List<string>();

            foreach (var objectProperty in objectProperties)
            {
                columns.Add(objectProperty.Name);
                var value = objectProperty.GetValue(model, null);
                if (value is string)
                {
                    value = $"'{value}'";
                }
                if (value is bool)
                {
                    value = Convert.ToInt32(value);
                }
                values.Add(value.ToString());
            }

            var result = new string[2];
            result[0] = string.Join(",", columns);
            result[1] = string.Join(",", values);

            return result;
        }

        private string UpdateParams<T>(T model) where T : class, new()
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var objectProperties = typeof(T).GetProperties(flags);
            var result = new List<string>();
            foreach (var objectProperty in objectProperties)
            {
                var value = objectProperty.GetValue(model, null);
                if (value is string)
                {
                    value = $"'{value}'";
                }
                if (value is bool)
                {
                    value = Convert.ToInt32(value);
                }
                result.Add($"{objectProperty.Name} = {value}");
            }
            return $"{string.Join(",", result)} WHERE ID = {(long)model.GetType().GetProperty("Id").GetValue(model, null)}";
        }

    }
}