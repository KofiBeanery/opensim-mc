/*
 * Copyright (c) Contributors, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 */

using System;
using Nini.Config;
using OpenSim.Data;
using OpenSim.Services.Base;

namespace OpenSim.Services.AccessControlService
{
    public class AccessControlServiceBase : ServiceBase
    {
        protected IAccessControlData m_Database;

        public AccessControlServiceBase(IConfigSource config) : base(config)
        {
            string dllName = string.Empty;
            string connString = string.Empty;

            // [AccessControlService] first, then [DatabaseService]
            IConfig acConfig = config.Configs["AccessControlService"];
            if (acConfig is not null)
            {
                dllName = acConfig.GetString("StorageProvider", dllName);
                connString = acConfig.GetString("ConnectionString", connString);
            }

            IConfig dbConfig = config.Configs["DatabaseService"];
            if (dbConfig is not null)
            {
                if (dllName.Length == 0)
                    dllName = dbConfig.GetString("StorageProvider", string.Empty);
                if (connString.Length == 0)
                    connString = dbConfig.GetString("ConnectionString", string.Empty);
            }

            if (dllName.Length == 0)
                throw new Exception("No StorageProvider configured");

            m_Database = LoadPlugin<IAccessControlData>(dllName, [connString]);
            if (m_Database is null)
                throw new Exception($"Could not find a storage interface in module {dllName}");
        }
    }
}
