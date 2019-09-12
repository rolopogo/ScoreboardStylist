using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using BS_Utils.Utilities;
using TMPro;

namespace ScoreboardStylist.HarmonyPatches
{
    [HarmonyPatch(typeof(LeaderboardTableCell))]
    [HarmonyPatch("playerName", MethodType.Setter)]
    class LeaderboardTableCellplayerNameSetter
    {
        static void Postfix(LeaderboardTableCell __instance)
        {
            TextMeshProUGUI playerNameText = __instance.GetPrivateField<TextMeshProUGUI>("_playerNameText");
            string name = "<color=#FFFFFF>" + playerNameText.text + "</color>"; // to allow replacing default colours
            foreach (string key in Plugin.hexMap.Keys)
            {
                name = name.Replace(key, Plugin.hexMap[key]);
            }
            playerNameText.text = name;
        }
    }

    [HarmonyPatch(typeof(LeaderboardTableCell))]
    [HarmonyPatch("rank", MethodType.Setter)]
    class LeaderboardTableCellrankSetter
    {
        static void Postfix(LeaderboardTableCell __instance)
        {
            TextMeshProUGUI rankText = __instance.GetPrivateField<TextMeshProUGUI>("_rankText");
            string name = "<color=" + Plugin.rankColor + ">" + rankText.text + "</color>"; // to allow replacing default colours
            rankText.text = name;
        }
    }

    [HarmonyPatch(typeof(LeaderboardTableCell))]
    [HarmonyPatch("score", MethodType.Setter)]
    class LeaderboardTableCellscoreSetter
    {
        static void Postfix(LeaderboardTableCell __instance)
        {
            TextMeshProUGUI scoreText = __instance.GetPrivateField<TextMeshProUGUI>("_scoreText");
            string name = "<color=" + Plugin.scoreColor + ">" + scoreText.text + "</color>"; // to allow replacing default colours
            scoreText.text = name;
        }
    }
}
