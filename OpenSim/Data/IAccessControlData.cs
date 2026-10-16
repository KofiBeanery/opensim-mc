/*
 * Copyright (c) Contributors, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 */

namespace OpenSim.Data
{
    /// <summary>
    /// Storage for the optional access control (hardware and IP ban) service.
    /// Originally written by StolenRuby for the Consortium fork; re-written for Core.
    /// </summary>
    public interface IAccessControlData
    {
        bool IsHardwareBanned(string mac, string id0);
        bool IsIPBanned(string ip);

        bool BanIPAddress(string ip);
        bool UnbanIPAddress(string ip);

        bool BanMacAddress(string mac);
        bool UnbanMacAddress(string mac);

        bool BanID0(string id0);
        bool UnbanID0(string id0);
    }
}
