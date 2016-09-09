// LoginService.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.IO;
using System.Threading.Tasks;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using Steamworks;

namespace dEngine.Services
{
    /// <summary>
    /// A service for logging into a user account.
    /// </summary>
    [TypeId(48)]
    [ExplorerOrder(-1)]
    public class LoginService : Service
    {
        internal static LoginService Service;

        /// <summary />
        public LoginService()
        {
            Service = this;

            LoginSucceeded = new Signal<string, uint>(this);
            LoginFailed = new Signal<string>(this);
        }

        internal static bool LoggedIn { get; set; }
        internal static uint UserId { get; set; }
        internal static CSteamID SteamId { get; set; }
        internal static string ProfileName { get; set; }

        internal static LoginService GetExisting()
        {
            return DataModel.GetService<LoginService>();
        }

        /// <summary>
        /// Determines if the client is currently logged in.
        /// </summary>
        public bool IsLoggedIn()
        {
            return LoggedIn;
        }

        /// <summary>
        /// Used to get the user's ID when the <see cref="Player" /> object is not available.
        /// </summary>
        /// <returns></returns>
        public uint GetUserId()
        {
            return UserId;
        }

        private bool FailLogin(string msg)
        {
            Logger.Warn(msg);
            LoginFailed.Fire(msg);
            return false;
        }

        private bool SteamLogin()
        {
            if (!File.Exists("steam_appid.txt"))
                try
                {
                    File.WriteAllText("steam_appid.txt", Engine.AppId.ToString());
                }
                catch (Exception e)
                {
                    return FailLogin(e.Message);
                }

            if (!SteamAPI.Init())
                return FailLogin(
                    "Could initialize SteamAPI - Make sure Steam is running and the AppID is valid.");

            SteamAPI.RunCallbacks();
            SteamId = SteamUser.GetSteamID();
            UserId = SteamId.GetAccountID().m_AccountID;
            ProfileName = SteamFriends.GetPersonaName();

            LoginSucceeded.Fire(ProfileName);

            return true;
        }

        private void SteamLogout()
        {
            SteamAPI.Shutdown();
        }

        /// <summary>
        /// Tries to login to the user's account.
        /// </summary>
        /// <returns>A boolean determining if the login was successful.</returns>
        public bool TryLogin()
        {
            if (LoggedIn)
                throw new InvalidOperationException("Cannot login: already logged in.");
            LoggedIn = SteamLogin();

            if (LoggedIn)
            {
                Task.Run(() =>
                {
                    AnalyticsService.BeginSession();
                });
            }

            return LoggedIn;
        }

        /// <summary>
        /// Logs out of the current account.
        /// </summary>
        public void Logout()
        {
            SteamLogout();
            AnalyticsService.EndSession();
            //LoggedIn = false;
        }

        /// <summary>
        /// Fired when a login attempt is successful.
        /// </summary>
        /// <eventParam name="username" />
        /// <eventParam name="userId" />
        public readonly Signal<string, uint> LoginSucceeded;

        /// <summary>
        /// Fired when a login attempt fails.
        /// </summary>
        public readonly Signal<string> LoginFailed;
    }
}