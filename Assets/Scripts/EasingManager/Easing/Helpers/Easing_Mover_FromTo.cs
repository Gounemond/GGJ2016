using UnityEngine;
using System.Collections;

public class Easing_Mover_FromTo : Easing_Helper 
{	
	public Vector3 from;
	public Vector3 to;
	
	public override void EaseMe ()
	{
		Easing.easeToPosition( 	this.transform,
								from, to,
								duration,
								easingFunction ).setDelay( startDelay );
								
	}
}
