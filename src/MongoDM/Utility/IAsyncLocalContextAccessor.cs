﻿namespace Digicando.MongoDM.Utility
{
    public interface IAsyncLocalContextAccessor
    {
        AsyncLocalContext Context { get; }

        IAsyncLocalContext GetNewLocalContext();

        void OnCreated(AsyncLocalContext context);

        void OnDisposed(AsyncLocalContext context);
    }
}