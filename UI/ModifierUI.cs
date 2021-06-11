using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using HMUI;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Attributes;
using UnityEngine;
using BeatSaberMarkupLanguage.MenuButtons;
using BeatSaberMarkupLanguage.Components.Settings;

namespace JDFixer.UI
{
    public class ModifierUI : NotifiableSingleton<ModifierUI>
    {
        private PreferencesFlowCoordinator _prefFlow;
        [UIValue("minJump")]
        private int minJump => Config.UserConfig.minJumpDistance;
        [UIValue("maxJump")]
        private int maxJump => Config.UserConfig.maxJumpDistance;


        [UIValue("enabled")]
        public bool modEnabled
        {
            get => Config.UserConfig.enabled;
            set
            {
                Config.UserConfig.enabled = value;
            }
        }
        [UIAction("setEnabled")]
        void SetEnabled(bool value)
        {
            modEnabled = value;
        }

        [UIValue("practiceEnabled")]
        public bool practiceEnabled
        {
            get => Config.UserConfig.enabledInPractice;
            set
            {
                Config.UserConfig.enabledInPractice = value;
            }
        }
        [UIAction("setPracticeEnabled")]
        void SetPracticeEnabled(bool value)
        {
            practiceEnabled = value;
        }

        private float _currentJd = 0f;
        public void UpdateJumpDistance(float value)
        {
            _currentJd = value;
            NotifyPropertyChanged(nameof(CurrentJDText));
        }

        [UIValue("current-jd")]
        public string CurrentJDText => "<#ebbd52>" + _currentJd.ToString();




        [UIComponent("jumpDisSlider")]
        private SliderSetting jumpDisSlider;
        [UIValue("jumpDisValue")]
        public float jumpDisValue
        {
            get => Config.UserConfig.jumpDistance;
            set
            {
                Config.UserConfig.jumpDistance = value;
            }
        }
        [UIAction("setJumpDis")]
        void SetJumpDis(float value)
        {
            jumpDisValue = value;
        }

        [UIValue("usePrefJumpValues")]
        public bool usePrefJumpValues
        {
            get => Config.UserConfig.usePreferredJumpDistanceValues;
            set
            {
                Config.UserConfig.usePreferredJumpDistanceValues = value;
            }
        }
        [UIAction("setUsePrefJumpValues")]
        void SetUsePrefJumpValues(bool value)
        {
            usePrefJumpValues = value;
        }
        [UIAction("prefButtonClicked")]
        void PrefButtonClicked()
        {
            if (_prefFlow == null)
                _prefFlow = BeatSaberUI.CreateFlowCoordinator<PreferencesFlowCoordinator>();
            var ActiveFlowCoordinator = DeepestChildFlowCoordinator(BeatSaberUI.MainFlowCoordinator);
            _prefFlow.ParentFlow = ActiveFlowCoordinator;
            ActiveFlowCoordinator.PresentFlowCoordinator(_prefFlow, null, ViewController.AnimationDirection.Horizontal, true);
        }


        //###################################
        // Thresholds Display
        [UIValue("upperthreshold")]
        public float upperthreshold
        {
            get => Config.UserConfig.upper_threshold;
        }

        [UIValue("lowerthreshold")]
        public float lowerthreshold
        {
            get => Config.UserConfig.lower_threshold;
        }
        //###################################


        [UIComponent("leftButton")]
        private RectTransform leftButton;
        [UIComponent("rightButton")]
        private RectTransform rightButton;

        [UIAction("#post-parse")]
        void PostParse() // Arrow buttons on the slider
        {
            SliderButton.Register(GameObject.Instantiate(leftButton), GameObject.Instantiate(rightButton), jumpDisSlider, 0.1f);
            GameObject.Destroy(leftButton.gameObject);
            GameObject.Destroy(rightButton.gameObject);
        }

        public static FlowCoordinator DeepestChildFlowCoordinator(FlowCoordinator root)
        {
            var flow = root.childFlowCoordinator;
            if (flow == null) return root;
            if (flow.childFlowCoordinator == null || flow.childFlowCoordinator == flow)
            {
                return flow;
            }
            return DeepestChildFlowCoordinator(flow);
        }

    }
}
