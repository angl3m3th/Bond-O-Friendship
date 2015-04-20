﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class StreamReaction : MonoBehaviour {

	public float reactionProgress = 0;
	public float reactionRate = 1;
	public float decayRate = -1;
	public bool reactable = true;
	[SerializeField]
	public List<StreamReaction> superiors;
	protected bool reacting = false;

	virtual protected void Start()
	{
		if (superiors.Count > 0)
		{
			reactable = false;
		}
	}

	virtual protected void Update()
	{
		if (decayRate < 0)
		{
			decayRate = reactionRate;
		}

		if (!reacting && reactionProgress > 0)
		{
			//React(-decayRate * Time.deltaTime);
		}
		reacting = false;
	}

	public virtual bool React(float actionRate)
	{
		if (superiors.Count > 0)
		{
			reactable = true;
			for (int i = 0; i < superiors.Count; i++)
			{
				if (superiors[i] != null && superiors[i].reactionProgress < 1)
				{
					reactable = false;
				}
			}
		}

		if (reactable)
		{
			bool reacted = (actionRate >= 0 && reactionProgress < 1) || (actionRate <= 0 && reactionProgress > 0);
			reactionProgress = CalculateReaction(actionRate);
			return reacted;
		}
		return false;
	}

	public float CalculateReaction(float actionRate)
	{
		return Mathf.Clamp01(reactionProgress + actionRate * reactionRate);
	}
}
