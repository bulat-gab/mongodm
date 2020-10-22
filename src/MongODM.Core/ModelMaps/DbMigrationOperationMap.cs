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

using Etherna.MongODM.Core.Models;
using Etherna.MongODM.Core.Models.DbMigrationOpAgg;
using Etherna.MongODM.Core.Serialization;

namespace Etherna.MongODM.Core.ModelMaps
{
    class DbMigrationOperationMap : IModelMapsCollector
    {
        public void Register(IDbContext dbContext)
        {
            dbContext.DocumentSchemaRegister.RegisterModelSchema<DbMigrationOperation>(
                "0.20.0");

            dbContext.DocumentSchemaRegister.RegisterModelSchema<MigrationLogBase>(
                "0.20.0");

            dbContext.DocumentSchemaRegister.RegisterModelSchema<DocumentMigrationLog>(
                "0.20.0");

            dbContext.DocumentSchemaRegister.RegisterModelSchema<IndexMigrationLog>(
                "0.20.0");
        }
    }
}