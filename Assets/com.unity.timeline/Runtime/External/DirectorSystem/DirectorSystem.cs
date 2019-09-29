﻿using System;
using System.IO;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{


    public interface IDirectorIO
    {
        void Load(BinaryReader reader);


        void Write(BinaryWriter writer);

    }

    public class DirectorSystem
    {

        public static TrackType UtilTrackType(TrackAsset track)
        {
            if (track is AudioTrack)
            {
                return TrackType.AUDIO;
            }
            if (track is AnimationTrack)
            {
                return TrackType.ANIMTION;
            }
            if (track is ControlTrack)
            {
                return TrackType.CONTROL;
            }
            if (track is ActivationTrack)
            {
                return TrackType.ACTIVE;
            }
            if (track is AnchorTrack)
            {
                return TrackType.ANCHOR;
            }
            if (track is BoneFxTrack)
            {
                return TrackType.BONEFX;
            }
            if (track is MarkerTrack)
            {
                return TrackType.MARKER;
            }
            return TrackType.NONE;
        }


        public static Type UtilTrackType(TrackType type)
        {
            Type tp = null;
            switch (type)
            {
                case TrackType.ACTIVE:
                case TrackType.MARKER:
                    tp = typeof(GameObject);
                    break;
                case TrackType.ANIMTION:
                    tp = typeof(Animator);
                    break;
                case TrackType.AUDIO:
                    tp = typeof(AudioSource);
                    break;
            }
            return tp;
        }


        public static PlayableAsset CreateClipAsset(TrackType type)
        {
            PlayableAsset clip = null;
            switch (type)
            {
                case TrackType.ANIMTION:
                    clip = ScriptableObject.CreateInstance<AnimationPlayableAsset>();
                    break;
                case TrackType.ACTIVE:
                    clip = ScriptableObject.CreateInstance<ActivationPlayableAsset>();
                    break;
                case TrackType.CONTROL:
                    clip = ScriptableObject.CreateInstance<ControlPlayableAsset>();
                    break;
                case TrackType.AUDIO:
                    clip = ScriptableObject.CreateInstance<AudioPlayableAsset>();
                    break;
                case TrackType.BONEFX:
                    clip = ScriptableObject.CreateInstance<BoneFxAsset>();
                    break;
                case TrackType.ANCHOR:
                    clip = ScriptableObject.CreateInstance<AnchorAsset>();
                    break;
            }
            return clip;
        }


        public static Marker CreateMarker(MarkType type)
        {
            Marker marker = null;
            switch (type)
            {
                case MarkType.ACTIVE:
                    marker = ScriptableObject.CreateInstance<ActiveSignalEmmiter>();
                    break;
                case MarkType.ANCHOR:
                    marker = ScriptableObject.CreateInstance<AnchorSignalEmitter>();
                    break;
                case MarkType.JUMP:
                    marker = ScriptableObject.CreateInstance<JumpSignalEmmiter>();
                    break;
                case MarkType.SLOW:
                    marker = ScriptableObject.CreateInstance<SlowSignalEmitter>();
                    break;
            }
            return marker;
        }

    }

}