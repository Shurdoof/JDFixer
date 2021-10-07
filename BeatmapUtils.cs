using BS_Utils.Utilities;
using SongCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JDFixer
{
    static class BeatmapUtils
    {
        public static float CalculateJumpDistance(float bpm, float njs, float offset)
        {
            float jumpdistance = 0f; // In case
            float halfjump = 4f;
            float num = 60f / bpm;

            // Need to repeat this here even tho it's in BeatmapInfo because sometimes we call this function directly
            if (njs <= 0.01) // Is it ok to == a 0f?
                njs = 10f;

            while (njs * num * halfjump > 18)
                halfjump /= 2;

            halfjump += offset;
            if (halfjump < 0.25)
                halfjump = 0.25f;

            jumpdistance = njs * num * halfjump * 2;

            return jumpdistance;
        }

        public static bool IsNoodleMap(IDifficultyBeatmap diff)
        {
            return SongCore.Collections.RetrieveDifficultyData(diff).additionalDifficultyData._requirements.Contains("Noodle Extensions");
        }

        const string CustomCampaignStubSongLevelId = "custom_level_stub";
        public static async Task<IDifficultyBeatmap> GetDifficultyBeatMapAsync(this MissionNode missionNode)
        {
            if (missionNode == null || missionNode.missionData == null)
            {
                return null;
            }

            var beatmapCharacteristic = missionNode.missionData.beatmapCharacteristic;
            var beatmapDifficulty = missionNode.missionData.beatmapDifficulty;

            var customLevelProp = missionNode.missionData.GetType().GetFields().FirstOrDefault(x => x.Name == "customLevel" && x.FieldType == typeof(CustomPreviewBeatmapLevel));
            if (customLevelProp != null) //Custom campaigns
            {
                var customLevel = (CustomPreviewBeatmapLevel)customLevelProp.GetValue(missionNode.missionData);
                if (customLevel == null) //Song not downloaded
                {
                    return null;
                }

                var level = await Loader.BeatmapLevelsModelSO.GetBeatmapLevelAsync(customLevel.levelID, CancellationToken.None);
                return level.beatmapLevel.beatmapLevelData.GetDifficultyBeatmap(beatmapCharacteristic, beatmapDifficulty);
            }

            if (missionNode.missionData.level != null && missionNode.missionData.level.levelID != CustomCampaignStubSongLevelId) //Official campaign
            {
                return missionNode.missionData.level.GetDifficultyBeatmap(beatmapCharacteristic, beatmapDifficulty);
            }

            return null;
        }
    }
}
