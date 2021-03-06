﻿using System.Data.SqlClient;
using System;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Sql.Xml;

namespace NServiceBus
{

    /// <summary>
    /// Shared session extensions for NHibernate persistence.
    /// </summary>
    public static class SqlPersistenceStorageSessionExtensions
    {
        /// <summary>
        /// Gets the current context SqlPersistence <see cref="SqlTransaction"/>.
        /// </summary>
        public static ISqlStorageSession SqlPersistenceSession(this SynchronizedStorageSession session)
        {
            var storageSession = session as StorageSession;
            if (storageSession == null)
            {
                throw new InvalidOperationException("Shared session has not been configured for SqlPersistence.");
            }
            return storageSession;
        }
    }
}