using UnityEngine;
using System.Collections;
using System.Linq;
using Rewired;

public class MousePhysicsTest : MonoBehaviour
{
	private TargetJoint2D currentyActiveJoint;

	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			var body = Physics2D.OverlapPointAll(point, SRLayerMask.All).FirstOrDefault(c => c.GetComponent<Rigidbody2D>());
			if (body != null)
			{
				Debug.Log("grabbed body" + body.name);
                currentyActiveJoint = body.gameObject.AddComponent<TargetJoint2D>();
				currentyActiveJoint.autoConfigureTarget = false;
				currentyActiveJoint.anchor = body.transform.InverseTransformPoint(point);
			}
			else Debug.Log("didnt grab body");
		}

		if (currentyActiveJoint != null)
		{
			if (Input.GetMouseButton(0))
			{
				currentyActiveJoint.target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			}
			else
			{
				Destroy(currentyActiveJoint);
			}
		}
	}
}
