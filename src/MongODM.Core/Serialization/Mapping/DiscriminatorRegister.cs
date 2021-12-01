﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Etherna.MongODM.Core.Serialization.Mapping
{
    public class DiscriminatorRegister : IDiscriminatorRegister
    {
        // Fields.
        private readonly ReaderWriterLockSlim configLock = new(LockRecursionPolicy.SupportsRecursion);
        private readonly Dictionary<Type, IDiscriminatorConvention> discriminatorConventions = new();
        private readonly Dictionary<BsonValue, HashSet<Type>> discriminators = new();
        private readonly HashSet<Type> discriminatedTypes = new();

        // Methods.
        public void AddDiscriminator(Type type, BsonValue discriminator)
        {
            // Checks.
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (discriminator is null)
                throw new ArgumentNullException(nameof(discriminator));

            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsInterface)
                throw new BsonSerializationException($"Discriminators can only be registered for classes, not for interface {type.FullName}.");

            // Add discriminator.
            configLock.EnterWriteLock();
            try
            {
                if (!discriminators.TryGetValue(discriminator, out HashSet<Type> hashSet))
                {
                    hashSet = new HashSet<Type>();
                    discriminators.Add(discriminator, hashSet);
                }

                if (!hashSet.Contains(type))
                {
                    hashSet.Add(type);

                    //mark all base types as discriminated (so we know that it's worth reading a discriminator)
                    for (var baseType = typeInfo.BaseType; baseType != null; baseType = baseType.GetTypeInfo().BaseType)
                        discriminatedTypes.Add(baseType);
                }
            }
            finally
            {
                configLock.ExitWriteLock();
            }
        }

        public void AddDiscriminatorConvention(Type type, IDiscriminatorConvention convention)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (convention is null)
                throw new ArgumentNullException(nameof(convention));

            configLock.EnterWriteLock();
            try
            {
                if (!discriminatorConventions.ContainsKey(type))
                    discriminatorConventions.Add(type, convention);
                else
                    throw new BsonSerializationException($"There is already a discriminator convention registered for type {type.FullName}.");
            }
            finally
            {
                configLock.ExitWriteLock();
            }
        }
    }
}
