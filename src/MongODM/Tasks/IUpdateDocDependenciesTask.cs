﻿using Digicando.MongODM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Digicando.MongODM.Tasks
{
    public interface IUpdateDocDependenciesTask
    {
        Task RunAsync<TDbContext, TModel, TKey>(
            IEnumerable<string> idPaths,
            TKey modelId)
            where TModel : class, IEntityModel<TKey>
            where TDbContext : class, IDbContext;
    }
}