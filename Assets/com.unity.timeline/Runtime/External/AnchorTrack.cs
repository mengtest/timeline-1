﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{

    [TrackClipType(typeof(AnchorAsset))]
    [TrackColor(0.76f, 0.44f, 0.24f)]
    [TrackAttribute(true)]
    public class AnchorTrack : TrackAsset
    {

        List<AnchorSignalEmitter> signals;
        AnimationCurve[] m_curves_pos = null;
        AnimationCurve[] m_curves_rot = null;
        

        protected override void OnAfterTrackDeserialize()
        {
            base.OnAfterTrackDeserialize();
            OutputTrackinfo();
        }

#if UNITY_EDITOR
        public void AddOrUpdateMarker(PlayableDirector director, GameObject go)
        {
            Transform tf = DirectorSystem.FetchAttachOfTrack(this);
            if (tf && tf.gameObject == go)
            {
                double dtime = director.time;
                var marks = GetMarkers();
                foreach (var mark in marks)
                {
                    if (mark.time == director.time)
                    {
                        var anchor = mark as AnchorSignalEmitter;
                        anchor.position = tf.localPosition;
                        anchor.rotation = tf.localEulerAngles;
                        break;
                    }
                }
            }
        }

        public void CreateAnchor()
        {
            var marker = ScriptableObject.CreateInstance<AnchorSignalEmitter>();
            marker.time = DirectorSystem.Director.time;
            AddMarker(marker);
            (marker as IMarker).Initialize(this);
        }
#endif

        public void OutputTrackinfo()
        {
            if (signals == null)
            {
                signals = new List<AnchorSignalEmitter>();
            }
            else signals.Clear();

            var marks = GetMarkers().GetEnumerator();
            while (marks.MoveNext())
            {
                IMarker mark = marks.Current;
                if (mark is AnchorSignalEmitter)
                {
                    AnchorSignalEmitter anchor = mark as AnchorSignalEmitter;
                    signals.Add(anchor);
                }
            }
            marks.Dispose();
            CreateClips();
        }

        private void ClearCurves(ref AnimationCurve[] curves)
        {
            if (curves == null || curves.Length == 0)
            {
                curves = new AnimationCurve[3];
                for (int i = 0; i < 3; i++)
                {
                    curves[i] = new AnimationCurve();
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    curves[i].keys = null;
                }
            }
        }


        private void CreateClips()
        {
            if (signals != null && signals.Count > 0)
            {
                ClearCurves(ref m_curves_pos);
                ClearCurves(ref m_curves_rot);
                for (int i = 0; i < signals.Count; i++)
                {
                    AnchorSignalEmitter sign = signals[i];
                    float time = (float)sign.time;
                    m_curves_pos[0].AddKey(time, sign.position.x);
                    m_curves_pos[1].AddKey(time, sign.position.y);
                    m_curves_pos[2].AddKey(time, sign.position.z);

                    m_curves_rot[0].AddKey(time, sign.rotation.x);
                    m_curves_rot[1].AddKey(time, sign.rotation.y);
                    m_curves_rot[2].AddKey(time, sign.rotation.z);
                }
                var clips = GetClips();
                if (clips != null)
                {
                    TimelineClip xclip = clips.FirstOrDefault();
                    if (xclip == null)
                    {
                        Debug.LogWarning("transform clip con't be null");
                    }
                    else
                    {
                        AnchorAsset asset = xclip.asset as AnchorAsset;
                        asset.clip_pos = m_curves_pos;
                        asset.clip_rot = m_curves_rot;
                        var bind = DirectorSystem.FetchAttachOfTrack(this);
                        asset.SetBind(bind);
                    }
                }
            }
        }
        

        public AnchorAsset GetAsset()
        {
            var clips = GetClips();
            if (clips.Count() > 0)
            {
                TimelineClip xclip = clips.FirstOrDefault();
                if (xclip != null)
                {
                    return xclip.asset as AnchorAsset;
                }
            }
            return null;
        }


        public void RebuildClip()
        {
            CreateClips();
        }

    }
}