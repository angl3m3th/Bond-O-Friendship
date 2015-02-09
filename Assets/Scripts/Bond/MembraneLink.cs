﻿using UnityEngine;
using System.Collections;

public class MembraneLink : BondLink {
	public Membrane membrane;
	public BondAttachable bondAttachable;
	public SpringJoint[] jointsShaping;

	void Update()
	{
		if (bondAttachable && bondAttachable.bonds.Count > 0)
		{
			for (int i = 0; i < bondAttachable.bonds.Count; i++)
			{
				Bond bond = bondAttachable.bonds[i];
				MembraneLink nearestLink;
				Vector3 halfWidthOffset = transform.right * 0.5f;
				Vector3 currentPos = (bond.attachment1.position - halfWidthOffset);
				Vector3 newPos = membrane.NearestNeighboredPoint(bond.links[1].transform.position, out nearestLink);
				bond.attachment1.position = newPos;
				if (nearestLink != this)
				{
					BondAttachable newAttachee = nearestLink.bondAttachable;
					if (nearestLink == nearestLink.membrane.links[0])
					{
						newAttachee = nearestLink.membrane.attachment1FauxLink.bondAttachable;
					}
					else if (nearestLink == nearestLink.membrane.links[nearestLink.membrane.links.Count - 1])
					{
						newAttachee = nearestLink.membrane.attachment2FauxLink.bondAttachable;
					}
					bond.ReplacePartner(bondAttachable, newAttachee);
				}
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Character")
		{
			if (membrane != null && membrane.extraStats.bondOnContact)
			{
				AttempBond(collision.collider.GetComponent<BondAttachable>(), collision.contacts[0].point);
			}
		}
	}

	void AttachFluff(Fluff fluff)
	{
		if (fluff != null && fluff.moving && (fluff.attachee == null || fluff.attachee.gameObject == gameObject))
		{
			if (membrane != null && membrane.extraStats.bondOnFluff)
			{
				AttempBond(fluff.creator, fluff.transform.position);
			}
			fluff.PopFluff();
		}
	}

	private void AttempBond(BondAttachable partner, Vector3 contactPosition)
	{
		if (membrane != null && partner != null && (membrane.preferNewBonds || !membrane.IsBondMade(partner)))
		{
			// Use the attachable provided by the membrane, to allow endpoint links to be handled differently.
			BondAttachable linkAttachable = membrane.FindLinkAttachable(this);
			if (linkAttachable != null)
			{
				if (membrane.preferNewBonds)
				{
					membrane.BreakInnerBond(partner);
				}
				bool bonded = linkAttachable.AttemptBond(partner, membrane.NearestPoint(contactPosition)/*contactPosition*/, true);
				if (bonded)
				{
					membrane.forceFullDetail = true;
					bondAttachable.bonds[bondAttachable.bonds.Count - 1].stats.manualAttachment1 = true;
				}
			}
		}
	}

	private void BondBroken()
	{
		if (membrane != null && !membrane.IsBondMade())
		{
			membrane.forceFullDetail = false;
		}
	}
}