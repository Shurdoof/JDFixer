﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;


[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]

namespace JDFixer
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }

        public virtual bool enabled { get; set; } = false;
        //public bool enabledInPractice;
        public virtual float jumpDistance { get; set; } = 24f;
        public virtual int minJumpDistance { get; set; } = 15;
        public virtual int maxJumpDistance { get; set; } = 35;
        public virtual bool usePreferredJumpDistanceValues { get; set; } = false;

        [UseConverter(typeof(ListConverter<JDPref>))]
        [NonNullable]
        public virtual List<JDPref> preferredValues { get; set; } = new List<JDPref>();

        public virtual float upper_threshold { get; set; } = 100f;
        public virtual float lower_threshold { get; set; } = 1f;
        public virtual bool use_heuristic { get; set; } = false;

        // Reaction Time Mode
        public virtual bool rt_enabled { get; set; } = false;

        [UseConverter(typeof(ListConverter<RTPref>))]
        [NonNullable]
        public virtual List<RTPref> rt_preferredValues { get; set; } = new List<RTPref>();
        public virtual int minReactionTime { get; set; } = 200;
        public virtual int maxReactionTime { get; set; } = 2000;
        public virtual bool rt_display_enabled { get; set; } = true;
        public bool disableForNoodleMaps { get; set; } = true;

        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed()
        {
            // Do stuff when the config is changed.
        }

        /// <summary>
        /// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
        /// </summary>
        public virtual void CopyFrom(PluginConfig other)
        {
            // This instance's members populated from other
        }
    }

    public class JDPref
    {
        public float njs = 16f;
        public float jumpDistance = 24f;

        public JDPref()
        {

        }

        public JDPref(float njs, float jumpDistance)
        {
            this.njs = njs;
            this.jumpDistance = jumpDistance;
        }
    }

    // Reaction Time Mode
    public class RTPref
    {
        public float njs = 16f;
        public float reactionTime = 800f;

        public RTPref()
        {

        }

        public RTPref(float njs, float reactionTime)
        {
            this.njs = njs;
            this.reactionTime = reactionTime;
        }
    }
}
