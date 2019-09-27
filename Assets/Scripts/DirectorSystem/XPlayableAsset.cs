﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class XPlayableAsset : PlayableAsset
{

    private double _duration;

    public override double duration
    {
        get { return _duration; }
    }

    public override IEnumerable<PlayableBinding> outputs
    {
        get { return null; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        throw new System.NotImplementedException();
    }

    public void SetDuration(double duration)
    {
        _duration = duration;
    }

}
