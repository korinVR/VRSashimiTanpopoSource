using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Oculus.Platform;
using UnityEngine;
using UnityEngine.Networking;

namespace VRSashimiTanpopo.Debug
{
    public class AchievementsTest : MonoBehaviour
    {
#if PLATFORM_OCULUS
        async void Start()
        {
            try
            {
                if (!Core.IsInitialized())
                {
                    var initialized = await Core.AsyncInitialize().Gen(); // ここをTask化
                    if (initialized.IsError)
                    {
                        UnityEngine.Debug.Log($"failed initialize: {initialized.GetError().Message}");
                        return;
                    }
                }

                var entitlements = await Entitlements.IsUserEntitledToApplication().Gen(); // ここをTask化
                if (entitlements.IsError)
                {
                    UnityEngine.Debug.Log($"failed entitlement: {entitlements.GetError().Message}");
                    return;
                }

                var user = await Users.GetLoggedInUser().Gen(); // ここをTask化
                if (user.IsError)
                {
                    UnityEngine.Debug.Log($"failed get user: {user.GetError().Message}");
                    return;
                }
                UnityEngine.Debug.Log($"{user.Data.ID}, {user.Data.OculusID}");
            }
            catch (UnityException e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }

#endif
    }
}