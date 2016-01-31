using UnityEngine;

public class ArmControlTest : MonoBehaviour
{
	public SpiderMain SpiderRef;
	public Rigidbody2D LeftHandBody;
	public float DefaultDistance;
	public Vector2 ArmStartingPoint;
	public Vector2 BaseDirection = new Vector2(-1,-1);
	public float AllowedMaxAngleFromBaseDirection;
	public Side side;
	private SpringJoint2D joint;
	public float SpringFrequency;
	public float SpringDampingRatio;
	public Vector2 AnchorOffset;
	void Start()
	{
		joint = LeftHandBody.gameObject.AddComponent<SpringJoint2D>();
		joint.connectedBody = GetComponent<Rigidbody2D>();
		joint.autoConfigureDistance = false;
		joint.distance = 0;
		joint.dampingRatio = SpringDampingRatio;
		joint.frequency = SpringFrequency;
		joint.anchor = AnchorOffset;
		/*foreach (var c in Rewired.ReInput.controllers.Joysticks)
		{
			Rewired.ReInput.players.GetPlayer(0).controllers.AddController(c, false);
			Debug.Log("Added controller "+c.name);
		}*/
	}

	float mod(float b, float m)
	{
		var r = b % m;
		return r < 0 ? r + m : r;
	}

	Vector2 rotate(Vector2 v, float degrees)
	{
		var cos = Mathf.Cos(degrees*Mathf.Deg2Rad);
		var sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
		Vector2 r;
		r.x = v.x*cos - v.y*sin;
		r.y = v.x*sin + v.y*cos;
		return r;
	}

	void Update()
	{
		var player = SpiderRef.RwPlayer;
		var leftArmControl = player.GetAxis2D(side+" Arm X", side+" Arm Y");
		leftArmControl = Vector2.ClampMagnitude(leftArmControl, 1f);
		leftArmControl = transform.InverseTransformDirection(leftArmControl);
		var angle = Mathf.Atan2(leftArmControl.x, leftArmControl.y) * Mathf.Rad2Deg;
		var angleBase = Mathf.Atan2(BaseDirection.x, BaseDirection.y) * Mathf.Rad2Deg;
		var angleDiff = mod(angle - angleBase, 360) - 180;

		if (Vector2.Angle(BaseDirection, leftArmControl) > AllowedMaxAngleFromBaseDirection)
		{
			leftArmControl = rotate(BaseDirection.normalized*leftArmControl.magnitude,
				AllowedMaxAngleFromBaseDirection*Mathf.Sign(angleDiff));
		}

        joint.enabled = leftArmControl.magnitude > 0.2f;
        joint.connectedAnchor = (Vector2) ArmStartingPoint + leftArmControl * DefaultDistance;
		if (joint.enabled)
		{
			Debug.DrawLine(transform.TransformPoint(ArmStartingPoint), transform.TransformPoint(joint.connectedAnchor));
			Debug.DrawLine(LeftHandBody.transform.TransformPoint(AnchorOffset), transform.TransformPoint(joint.connectedAnchor));
		}
	}
}
