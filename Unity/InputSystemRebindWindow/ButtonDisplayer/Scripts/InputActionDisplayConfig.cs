using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    [CreateAssetMenu(menuName = "Config/" + nameof(InputActionDisplayConfig))]
    public class InputActionDisplayConfig : ScriptableObject
    {
        [SerializeField]
        private DeviceButtons[] deviceConfigs = new DeviceButtons[]
        {
            new() { name = "Keyboard", devicePrefixes = new string[] { "<Keyboard>", "Keyboard" } },
            new() { name = "Mouse", devicePrefixes = new string[] { "<Mouse>", "Mouse" } }
        };

        [System.Serializable]
        private class DeviceButtons
        {
            public string name;
            public string[] devicePrefixes;
            public PathToConfig[] pathToConfigs = new PathToConfig[0];
        }

        [System.Serializable]
        private class PathToConfig
        {
            public string path;
            public InputButtonDisplayConfig buttonConfig;
        }

        public InputButtonDisplayConfig GetDisplayConfig(InputBinding inputBinding)
        {
            var inputPath = inputBinding.effectivePath;
            if (inputPath == string.Empty)
            {
                return null;
            }
            //Debug.Log("GetDisplayConfig path: " + inputPath);
            var pathSplited = inputPath.Split(InputControlPath.Separator);
            if (string.IsNullOrEmpty(pathSplited[0])) pathSplited = pathSplited[1..]; //this is in format /device/ (probably)

            foreach (var item in deviceConfigs)
            {
                var remainPath = string.Join(InputControlPath.Separator, pathSplited[1..]);
                var config = item.pathToConfigs.FirstOrDefault(x => x.path == remainPath);

                if (config is not null) return config.buttonConfig;
            }

            return new InputButtonDisplayConfig() { text = inputBinding.ToDisplayString() };
        }

    }
}
