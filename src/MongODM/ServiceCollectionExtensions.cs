﻿//   Copyright 2020-present Etherna Sagl
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using Etherna.ExecContext;
using Etherna.MongODM.AspNetCore;
using Etherna.MongODM.Core.Models;
using Etherna.MongODM.Core.ProxyModels;
using Etherna.MongODM.HF.Tasks;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        // Methods.
        public static MongODMConfiguration UseMongODMWithHangfire<TModelBase>(
            this IServiceCollection services,
            IEnumerable<IExecutionContext>? executionContexts = null,
            string? hangfireConnectionString = null,
            MongoStorageOptions? hangfireMongoStorageOptions = null)
            where TModelBase : class, IModel
        {
            // Configure MongODM.
            var conf = services.UseMongODM<HangfireTaskRunner, TModelBase>(executionContexts);

            // Configure Hangfire.
            AddHangfire(services, hangfireConnectionString, hangfireMongoStorageOptions);

            return conf;
        }

        public static MongODMConfiguration UseMongODMWithHangfire<TProxyGenerator, TModelBase>(
            this IServiceCollection services,
            IEnumerable<IExecutionContext>? executionContexts = null,
            string? hangfireConnectionString = null,
            MongoStorageOptions? hangfireMongoStorageOptions = null)
            where TProxyGenerator : class, IProxyGenerator
            where TModelBase : class, IModel
        {
            if (hangfireConnectionString is null)
                throw new ArgumentNullException(nameof(hangfireConnectionString));

            // Configure MongODM.
            var conf = services.UseMongODM<TProxyGenerator, HangfireTaskRunner, TModelBase>(executionContexts);

            // Configure Hangfire.
            AddHangfire(services, hangfireConnectionString, hangfireMongoStorageOptions);

            return conf;
        }

        // Helpers.
        private static void AddHangfire(
            IServiceCollection services,
            string? hangfireConnectionString,
            MongoStorageOptions? hangfireMongoStorageOptions)
        {
            // Set default options.
            hangfireConnectionString ??= "mongodb://localhost/hangfire";
            hangfireMongoStorageOptions ??= new MongoStorageOptions
            {
                MigrationOptions = new MongoMigrationOptions
                {
                    MigrationStrategy = new MigrateMongoMigrationStrategy(),
                    BackupStrategy = new CollectionMongoBackupStrategy()
                }
            };

            // Configure Hangfire.
            services.AddHangfire(options =>
            {
                options.UseMongoStorage(hangfireConnectionString, hangfireMongoStorageOptions);
            });
        }
    }
}
