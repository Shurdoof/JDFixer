using IPA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BeatSaberMarkupLanguage;
using TMPro;
using HarmonyLib;

namespace JDFixer
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        [OnStart]
        public void OnApplicationStart()
        {
            Config.Read();
            var harmony = new Harmony("com.zephyr.BeatSaber.JDFixer");
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());

            BS_Utils.Utilities.BSEvents.lateMenuSceneLoadedFresh += BSEvents_lateMenuSceneLoadedFresh;
            BS_Utils.Utilities.BSEvents.difficultySelected += BSEvents_difficultySelected;
            //BS_Utils.Utilities.BSEvents.gameSceneLoaded += BSEvents_gameSceneLoaded;

            BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup.instance.AddTab("JDFixer", "JDFixer.UI.BSML.modifierUI.bsml", UI.ModifierUI.instance, BeatSaberMarkupLanguage.GameplaySetup.MenuType.Solo);
            BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup.instance.AddTab("JDFixerOnline", "JDFixer.UI.BSML.modifierOnlineUI.bsml", UI.ModifierUI.instance, BeatSaberMarkupLanguage.GameplaySetup.MenuType.Online);
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        private void SceneManager_activeSceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
        {
            Config.Write();
        }

        [Init]
        public void Init(IPA.Logging.Logger logger)
        {
            Logger.log = logger;
        }

        // For when user selects a map with only 1 difficulty or selects a map but does not click a difficulty
        private void BSEvents_lateMenuSceneLoadedFresh(ScenesTransitionSetupDataSO obj)
        {
            var leveldetail = Resources.FindObjectsOfTypeAll<StandardLevelDetailViewController>().FirstOrDefault();

            if (leveldetail != null)
            {
                leveldetail.didChangeContentEvent += Leveldetail_didChangeContentEvent;
            }
        }


        float CalculateJumpDistance(float bpm, float njs, float offset)
        {
            float halfjump = 4f;

            //Logger.log.Debug($"Difficulty Selected. BPM: {bpm} | NJS: {njs} | Offset: {offset}");

            float num = 60f / bpm;
            while (njs * num * halfjump > 18)
                halfjump /= 2;

            halfjump += offset;
            if (halfjump < 1) halfjump = 1f;

            return njs * num * halfjump * 2;
        }

        // For when user explicitly clicks on a difficulty (Doesn't work if map has only 1 diff)
        private void BSEvents_difficultySelected(StandardLevelDetailViewController arg1, IDifficultyBeatmap level)
        {
            float bpm = arg1.beatmapLevel.beatsPerMinute;
            float njs = level.noteJumpMovementSpeed;
            float offset = level.noteJumpStartBeatOffset;

            CurrentMapInfo.JumpDistance = CalculateJumpDistance(bpm, njs, offset);
        }


        // For when user selects a map with only 1 difficulty or selects a map but does not click a difficulty
        private void Leveldetail_didChangeContentEvent(StandardLevelDetailViewController arg1, StandardLevelDetailViewController.ContentType arg2)
        {
            if (arg1 != null && arg1.selectedDifficultyBeatmap != null)
            {
                float bpm = arg1.selectedDifficultyBeatmap.level.beatsPerMinute;
                float njs = arg1.selectedDifficultyBeatmap.noteJumpMovementSpeed;
                float offset = arg1.selectedDifficultyBeatmap.noteJumpStartBeatOffset;

                // NOTE THESE DONT WORK: SONG LOADS FOREVER
                //float bpm = arg1.beatmapLevel.beatsPerMinute;
                //float offset = arg1.beatmapLevel.songTimeOffset;
                //String songname = arg1.beatmapLevel.songName;


                CurrentMapInfo.JumpDistance = CalculateJumpDistance(bpm, njs, offset);
                //Logger.log.Debug($"UI: BPM: {map_bpm} | NJS: {map_njs} | Offset: {map_offset} | Jump Distance: { map_jumpdistance}");
            }
        }


        // Think this is not necessary anymore:
        /*private void BSEvents_gameSceneLoaded()
        {
            bool WillOverride = BS_Utils.Plugin.LevelData.IsSet && !BS_Utils.Gameplay.Gamemode.IsIsolatedLevel
                && Config.UserConfig.enabled && BS_Utils.Plugin.LevelData.Mode == BS_Utils.Gameplay.Mode.Standard && BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.practiceSettings == null;
            if(WillOverride && false) // false is from "!Config.User.Config.dontForceNJS"
                BS_Utils.Gameplay.ScoreSubmission.DisableSubmission("JDFixer");

        }*/

        [OnExit]
        public void OnApplicationQuit()
        {
            Config.Write();
        }

    }
}
