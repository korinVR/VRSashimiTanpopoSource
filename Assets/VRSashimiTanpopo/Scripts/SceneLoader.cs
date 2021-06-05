using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace VRSashimiTanpopo
{
    public enum SceneName
    {
        Unloaded,
        Title,
        SashimiTanpopoFactory,
        InfiniteTanpopoFactory,
    }
    
    public class SceneLoader
    {
        SceneName loadedSceneName;
        
        public async UniTask LoadAdditiveSceneAsync(SceneName sceneName)
        {
            var alreadyLoaded = false;
            
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == sceneName.ToString())
                {
                    alreadyLoaded = true;
                }
            }

            if (!alreadyLoaded)
            {
                await SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Additive);
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName.ToString()));

            loadedSceneName = sceneName;
        }

        public async UniTask UnloadSceneAsync()
        {
            if (loadedSceneName == SceneName.Unloaded) return;
            
            await SceneManager.UnloadSceneAsync(loadedSceneName.ToString());
        }
    }
}