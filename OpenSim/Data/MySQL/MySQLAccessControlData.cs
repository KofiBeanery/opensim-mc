/*
 * Copyright (c) Contributors, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 */

using System.Reflection;
using MySql.Data.MySqlClient;

namespace OpenSim.Data.MySQL
{
    public class MySqlAccessControlData : MySqlFramework, IAccessControlData
    {
        protected virtual Assembly Assembly
        {
            get { return GetType().Assembly; }
        }

        public MySqlAccessControlData(string connectionString)
                : base(connectionString)
        {
            m_connectionString = connectionString;

            using (MySqlConnection dbcon = new MySqlConnection(m_connectionString))
            {
                dbcon.Open();
                Migration m = new Migration(dbcon, Assembly, "AccessControl");
                m.Update();
            }
        }

        public bool IsHardwareBanned(string mac, string id0)
        {
            using (MySqlConnection dbcon = new MySqlConnection(m_connectionString))
            {
                dbcon.Open();

                using (MySqlCommand cmd = new MySqlCommand(
                    "select mac as id from `banned_macs` where mac = ?mac UNION ALL select id0 as id from `banned_id0s` where id0 = ?id0 LIMIT 1", dbcon))
                {
                    cmd.Parameters.AddWithValue("?mac", mac ?? string.Empty);
                    cmd.Parameters.AddWithValue("?id0", id0 ?? string.Empty);

                    using (MySqlDataReader result = cmd.ExecuteReader())
                        return result.Read();
                }
            }
        }

        public bool IsIPBanned(string ip)
        {
            using (MySqlConnection dbcon = new MySqlConnection(m_connectionString))
            {
                dbcon.Open();

                using (MySqlCommand cmd = new MySqlCommand("select ip from `banned_ips` where ip = ?ip LIMIT 1", dbcon))
                {
                    cmd.Parameters.AddWithValue("?ip", ip ?? string.Empty);

                    using (MySqlDataReader result = cmd.ExecuteReader())
                        return result.Read();
                }
            }
        }

        public bool BanIPAddress(string ip)
        {
            return Execute("INSERT IGNORE INTO `banned_ips` (`ip`) VALUES(?v)", ip);
        }

        public bool UnbanIPAddress(string ip)
        {
            return Execute("DELETE FROM `banned_ips` WHERE ip = ?v LIMIT 1", ip);
        }

        public bool BanMacAddress(string mac)
        {
            return Execute("INSERT IGNORE INTO `banned_macs` (`mac`) VALUES(?v)", mac);
        }

        public bool UnbanMacAddress(string mac)
        {
            return Execute("DELETE FROM `banned_macs` WHERE mac = ?v LIMIT 1", mac);
        }

        public bool BanID0(string id0)
        {
            return Execute("INSERT IGNORE INTO `banned_id0s` (`id0`) VALUES(?v)", id0);
        }

        public bool UnbanID0(string id0)
        {
            return Execute("DELETE FROM `banned_id0s` WHERE id0 = ?v LIMIT 1", id0);
        }

        private bool Execute(string sql, string value)
        {
            using (MySqlConnection dbcon = new MySqlConnection(m_connectionString))
            {
                dbcon.Open();

                using (MySqlCommand cmd = new MySqlCommand(sql, dbcon))
                {
                    cmd.Parameters.AddWithValue("?v", value);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
        }
    }
}
