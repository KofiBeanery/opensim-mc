/*
 * Copyright (c) Contributors, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 */

namespace OpenSim.Services.Interfaces
{
    /// <summary>
    /// Optional database backed ban list (IP address, viewer MAC hash, viewer id0 hash), checked at login
    /// and at Hypergrid gatekeeper entry. Not configured = not used. Complements IBansService and the
    /// DeniedMacs / DeniedID0s settings.
    /// </summary>
    public interface IAccessControlService
    {
        bool IsIPBanned(string ip);
        bool IsHardwareBanned(string mac, string id0);
    }
}
