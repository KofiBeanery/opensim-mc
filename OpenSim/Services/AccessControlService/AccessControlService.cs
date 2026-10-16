/*
 * Copyright (c) Contributors, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 */

using System;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Nini.Config;
using OpenSim.Framework;
using OpenSim.Services.Interfaces;

namespace OpenSim.Services.AccessControlService
{
    public class AccessControlService : AccessControlServiceBase, IAccessControlService
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static bool m_commandsRegistered = false;
        private static readonly Regex m_hash32 = new(@"\A[0-9a-fA-F]{32}\z", RegexOptions.Compiled);

        public AccessControlService(IConfigSource config) : base(config)
        {
            if (!m_commandsRegistered)
            {
                m_commandsRegistered = true;
                RegisterCommands();
            }

            m_log.Info("[ACCESS CONTROL]: Access control service started");
        }

        public bool IsIPBanned(string ip)
        {
            return m_Database.IsIPBanned(Normalize(ip));
        }

        public bool IsHardwareBanned(string mac, string id0)
        {
            return m_Database.IsHardwareBanned(mac, id0);
        }

        private static string Normalize(string ip)
        {
            return IPAddress.TryParse(ip, out IPAddress addr) ? addr.ToString() : ip;
        }

        private void RegisterCommands()
        {
            if (MainConsole.Instance is null)
                return;

            MainConsole.Instance.Commands.AddCommand("Access", false, "ban mac", "ban mac <mac_hash>",
                "Add a viewer MAC hash (32 hex characters) to the banned list.", (m, c) => Change(c, "mac", true));
            MainConsole.Instance.Commands.AddCommand("Access", false, "unban mac", "unban mac <mac_hash>",
                "Remove a viewer MAC hash from the banned list.", (m, c) => Change(c, "mac", false));
            MainConsole.Instance.Commands.AddCommand("Access", false, "ban id0", "ban id0 <id0_hash>",
                "Add a viewer id0 hash (32 hex characters) to the banned list.", (m, c) => Change(c, "id0", true));
            MainConsole.Instance.Commands.AddCommand("Access", false, "unban id0", "unban id0 <id0_hash>",
                "Remove a viewer id0 hash from the banned list.", (m, c) => Change(c, "id0", false));
            MainConsole.Instance.Commands.AddCommand("Access", false, "ban ip", "ban ip <ip_address>",
                "Add an IP address (IPv4 or IPv6) to the banned list.", (m, c) => Change(c, "ip", true));
            MainConsole.Instance.Commands.AddCommand("Access", false, "unban ip", "unban ip <ip_address>",
                "Remove an IP address from the banned list.", (m, c) => Change(c, "ip", false));
        }

        private void Change(string[] cmd, string kind, bool ban)
        {
            if (cmd.Length != 3)
            {
                m_log.Info($"[ACCESS CONTROL]: usage: {(ban ? "ban" : "unban")} {kind} <{(kind == "ip" ? "ip_address" : kind + "_hash")}>");
                return;
            }

            string value = cmd[2];
            if (kind == "ip")
            {
                if (!IPAddress.TryParse(value, out IPAddress addr))
                {
                    m_log.Info("[ACCESS CONTROL]: invalid IP address");
                    return;
                }
                value = addr.ToString();
            }
            else if (!m_hash32.IsMatch(value))
            {
                m_log.Info($"[ACCESS CONTROL]: invalid {kind} hash (expected 32 hex characters)");
                return;
            }

            m_log.Info($"[ACCESS CONTROL]: {(ban ? "banning" : "unbanning")} {kind} {value}");
            switch (kind)
            {
                case "ip": _ = ban ? m_Database.BanIPAddress(value) : m_Database.UnbanIPAddress(value); break;
                case "mac": _ = ban ? m_Database.BanMacAddress(value) : m_Database.UnbanMacAddress(value); break;
                default: _ = ban ? m_Database.BanID0(value) : m_Database.UnbanID0(value); break;
            }
        }
    }
}
