using Stride.Engine;
using Stride.Core.Serialization;
using qASIC;
using Stride.Core;

namespace LudumDare54
{
    public class SceneManager : StartupScript
    {
        public UrlReference<Scene> defaultScene;

        [DataMemberIgnore] public static SceneManager Instance { get; private set; }

        [DataMemberIgnore] public Scene CurrentScene { get; set; }
        [DataMemberIgnore] public UrlReference<Scene> CurrentSceneUrl { get; set; }

        public override void Start()
        {
            Instance = this;

            LoadScene(defaultScene);
        }

        public void LoadScene(UrlReference<Scene> scene)
        {
            qDebug.Log($"Attempting to load scene...", "magenta");
            LoadSceneBasic(scene);
        }

        private void LoadSceneBasic(UrlReference<Scene> sceneUrl)
        {
            if (!Content.Exists(sceneUrl))
            {
                qDebug.LogError("Cannot load a scene that doesn't exist!");
                return;
            }

            if (CurrentScene != null)
            {
                Entity.Scene.Children.Remove(CurrentScene);
                Content.Unload(CurrentScene);
            }

            var scene = Content.Load(sceneUrl);
            Entity.Scene.Children.Add(scene);
            CurrentScene = scene;
            CurrentSceneUrl = sceneUrl;

            qDebug.Log($"Scene {scene.Name} has been loaded", "magenta");
        }

        public void ReloadScene()
        {
            qDebug.Log($"Attempting to reload scene", "magenta");
            LoadSceneBasic(CurrentSceneUrl);
        }
    }
}
