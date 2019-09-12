using IPA;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomUI.Utilities;
using CustomUI.Settings;
using Harmony;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace ScoreboardStylist
{
    public sealed class Plugin : IBeatSaberPlugin
    {
        public static IPA.Logging.Logger logger;
        public static BS_Utils.Utilities.Config config;
        
        public static Dictionary<string, string> hexMap;
        public static string rankColor;
        public static string scoreColor;

        private static Dictionary<string, Color> colorMap;

        public void Init(object thisWillBeNull, IPA.Logging.Logger logger)
        {
            Plugin.logger = logger;
        }

        public void OnApplicationStart()
        {
            BSEvents.menuSceneLoadedFresh += OnMenuSceneLoadedFresh;
            config = new BS_Utils.Utilities.Config("ScoreboardStylist");
            hexMap = new Dictionary<string, string>();
            
            hexMap.Add("#FFFFFF", config.GetString("Colors", "default", "#FFFFFF", true));
            hexMap.Add("#F96854", config.GetString("Colors", "patron", "#F96854", true));
            hexMap.Add("#1ABC9C", config.GetString("Colors", "rankingTeam", "#1ABC9C", true));
            hexMap.Add("#FF03E3", config.GetString("Colors", "staff", "#FF03E3", true));
            hexMap.Add("#FFD42A", config.GetString("Colors", "percent", "#FFD42A", true));
            hexMap.Add("#6772E5", config.GetString("Colors", "pp", "#6772E5", true));

            rankColor = config.GetString("Colors", "rank", "#FFFFFF", true);
            scoreColor = config.GetString("Colors", "score", "#FFFFFF", true);

            var harmony = HarmonyInstance.Create("com.rolo.BeatSaber.ScoreboardStylist");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private void OnMenuSceneLoadedFresh()
        {
            CreateSettingsUI();
        }
        
        private static void CreateSettingsUI()
        {
            var subMenu = SettingsUI.CreateSubMenu("Scoreboard Style");
            colorMap = new Dictionary<string, Color>();
            
            MakePicker(subMenu, "default", "Default", "Color for normal users", "#FFFFFF");
            MakePicker(subMenu, "patron", "Patron", "Color for scoresaber patreon supporters", "#F96854");
            MakePicker(subMenu, "rankingTeam", "Ranking Team", "Color for scoresaber ranking team members", "#1ABC9C");
            MakePicker(subMenu, "staff", "Staff", "Color for scoresaber staff", "#FF03E3");
            MakePicker(subMenu, "percent", "Percentage", "Color for score percentage", "#FFD42A");
            MakePicker(subMenu, "pp", "PP", "Color for pp earned", "#6772E5");

            var rankPicker = MakePicker(subMenu, "rank", "Rank", "Color for leaderboard rank", "#FFFFFF", false);
            rankPicker.SetValue += delegate (Color value)
            {
                string hex = "#" + ColorUtility.ToHtmlStringRGB(value);
                rankColor = hex;
                config.SetString("Colors", "rank", hex);
            };

            var scorePicker = MakePicker(subMenu, "score", "Score", "Color for score", "#FFFFFF", false);
            scorePicker.SetValue += delegate (Color value)
            {
                string hex = "#" + ColorUtility.ToHtmlStringRGB(value);
                scoreColor = hex;
                config.SetString("Colors", "score", hex);
            };
        }

        private static ColorPickerViewController MakePicker(SubMenu subMenu, string id, string name, string description, string hex, bool setter = true)
        {
            Color col = Color.red;
            ColorUtility.TryParseHtmlString(config.GetString("Colors", id, hex, true), out col);
            colorMap[id] = col;

            ColorPickerViewController newPicker = subMenu.AddColorPicker(name, description, colorMap[id]);
            newPicker.GetValue += delegate
            {
                return colorMap[id];
            };
            if (setter)
            {
                newPicker.SetValue += delegate (Color value)
                {
                    string newHex = "#" + ColorUtility.ToHtmlStringRGB(value);
                    hexMap[hex] = newHex;
                    config.SetString("Colors", id, newHex);
                };
            }

            return newPicker;
        }

        public void OnApplicationQuit()
        {
            BSEvents.menuSceneLoadedFresh -= OnMenuSceneLoadedFresh;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode) { }

        public void OnSceneUnloaded(Scene scene) { }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene) { }

        public void OnUpdate() { }

        public void OnFixedUpdate() { }
    }
}