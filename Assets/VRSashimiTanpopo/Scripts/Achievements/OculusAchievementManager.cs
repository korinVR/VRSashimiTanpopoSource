using Cysharp.Threading.Tasks;
using FrameSynthesis.XR;
using UnityEngine;
using UnityEngine.Networking;
#if PLATFORM_OCULUS
using Oculus.Platform;
#endif

namespace VRSashimiTanpopo.Achievements
{
#if PLATFORM_OCULUS
    public class OculusAchievementManager : IAchievementManager
    {
        readonly DebugCode debugCode = new DebugCode();

        public async void Start()
        {
            try
            {
                if (!Core.IsInitialized())
                {
                    var initialized = await Core.AsyncInitialize().Gen();
                    if (initialized.IsError)
                    {
                        UnityEngine.Debug.Log($"failed initialize: {initialized.GetError().Message}");
                        return;
                    }
                }

                var entitlements = await Entitlements.IsUserEntitledToApplication().Gen();
                if (entitlements.IsError)
                {
                    UnityEngine.Debug.Log($"failed entitlement: {entitlements.GetError().Message}");
                    return;
                }

                var user = await Users.GetLoggedInUser().Gen();
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

        public void Tick()
        {
            if (debugCode.HasCompleted())
            {
                ResetAchievementsAsync().Forget();
            }
        }
        
        public void UnlockFirstSashimiTanpopo()
        {
            Oculus.Platform.Achievements.Unlock("FirstSashimiTanpopo");
        }

        public void UnlockFirstInfiniteTanpopo()
        {
            Oculus.Platform.Achievements.Unlock("FirstInfiniteTanpopo");
        }

        public void UnlockSashimiTanpopoPerfect()
        {
            Oculus.Platform.Achievements.Unlock("SashimiTanpopoPerfect");
        }

        public void UnlockInfiniteTanpopoAchievement(int index)
        {
            // if (index < 1 || index > 8) return;
            // Oculus.Platform.Achievements.Unlock("InfiniteTanpopo" + index);
        }

        static async UniTask ResetAchievementsAsync()
        {
            UnityEngine.Debug.Log("Reset Achievements");
            
            var user = await Oculus.Platform.Users.GetLoggedInUser().Gen();
            UnityEngine.Debug.Log($"ID: {user.Data.ID} OculusID: {user.Data.OculusID}");

            var form = new WWWForm();
            form.AddField("access_token", Secret.Oculus.AppCredentials);
            form.AddField("user_id", user.Data.ID.ToString());

            using var request = UnityWebRequest.Post("https://graph.oculus.com/achievement_remove_all", form);
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                UnityEngine.Debug.Log("Error");
            }
            else
            {
                UnityEngine.Debug.Log("Completed");
            }
        }
    }
#endif
}
