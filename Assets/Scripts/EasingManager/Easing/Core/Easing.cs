using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EasingLoopType
{
	None,
	Loop,
	PingPong
}

public class Easing
{
	#region enums
	public enum TargetType
	{
		floatValue,
		position,
		rotation,
		color,
		vector3, 
		path
	}
	
	public enum StartingOptions
	{
		none,
		fetchStartingPosition,
		fetchStartingRotation
	}
	#endregion
	
	#region events
    public delegate void onEasingCompleteDelegate(Easing es);
	public delegate void onEasingStartDelegate();
	public delegate void onEasingRemovedDelegate( Easing es);
	public delegate void onEasingProgressFloatDelegate( Easing es, float val );
	public delegate void onEasingProgressVector3Delegate( Easing es, Vector3 val );
	public delegate void onEasingProgressColorDelegate( Easing es, Color val );

	public event onEasingCompleteDelegate onEasingComplete;
	public event onEasingStartDelegate onEasingStart;
	public event onEasingRemovedDelegate onEasingRemoved;
	public event onEasingProgressFloatDelegate onEasingProgressFloat;
	public event onEasingProgressVector3Delegate onEasingProgressVector3;
	public event onEasingProgressColorDelegate onEasingProgressColor;
	
	public Easing setOnEasingremoved( onEasingRemovedDelegate del )
	{
		onEasingRemoved += del;
		return this;
	}
	
	public Easing setOnEasingComplete( onEasingCompleteDelegate del )
	{
		onEasingComplete += del;
		return this;
	}
   

    public Easing setOnEasingStart( onEasingStartDelegate del )
	{
		onEasingStart += del;
		return this;
	}
	
	public Easing setOnEasingProgressFloat( onEasingProgressFloatDelegate del )
	{
		onEasingProgressFloat += del;
		return this;
	}
	
	public Easing setOnEasingProgressVector3( onEasingProgressVector3Delegate del )
	{
		onEasingProgressVector3 += del;
		return this;
	}
	
	public Easing setOnEasingProgressColor( onEasingProgressColorDelegate del )
	{
		onEasingProgressColor += del;
		return this;
	}
	#endregion
	
	#region proprties
	TargetType target;
	
	public float inValue;
	public float outValue;
	
	public Transform transform;
	public Space space=Space.World;
	public Vector3 inVector;
	public Vector3 outVector;
	
	public Color inColor;
	public Color outColor;	
	
	public float easeTime;
	public float elapsed;
	public float delay;
	public EasingManager.EasingFunction fn;
	public bool completed= false;
	
	public EasingType easingType;
	public string animationCurveName;
	
	public EasingLoopType looping;
	public float normalizedTime;
	
	public PathScript path;
	public Vector3[] pathPositions;
	public Vector3[] pathRotations;
	public float	 pathStartNode;
	public float	 pathEndNode;
	
	public CRSpline positionSpline;
	public CRSpline rotationSpline;
	
	public bool pathAlignWithSpeed;
	public string easingGroup = "default";
	
	public Transform lookAtTransform;
	public Vector3 vectorUp;
	
	public List<Easing> chainedEasings;
	public bool chained=false;
	public bool paused;
	public bool toBeRemoved;
	public StartingOptions startingOptions = StartingOptions.none;

    public object param;
	#endregion
	
	#region helpers
	
	public Easing( )
	{
		looping = EasingLoopType.None;
		elapsed = 0f;
		delay = 0f;
		easingGroup = "default";
		normalizedTime = 0f;
	}
	
	public static Easing[] easeController( Transform pc, float time )
	{
		return easeController( pc, time, "default" );
	}
	
	public static Easing[] easeController( Transform pc, float time , string easingGroup )
	{
		return Easing.easeController( pc.GetComponent<PathController>(), time, easingGroup );
	}
	
	public static Easing[] easeController( PathController pc, float time )
	{
		return easeController( pc, time, "default" );
	}
	
	public static Easing[] easeController( PathController pc, float time, string easingGroup )
	{
		int i = 0;
		Easing[] easings = new Easing[ pc.paths.Length ];
		foreach( PathScript scr in pc.paths )
		{
			Easing es = Easing.easePath( scr.easingTarget, scr, time, easingGroup ); 
			es.alignPathWithSpeed( scr.alignWithSpeed );
			easings[ i++ ] = es;
		}
		
		return easings;
	}
	
	public static Easing easePath( Transform transform, PathScript path, float time, string easingGroup )
	{
		Easing es = new Easing();
		es.transform = transform;
		es.path=path;
		es.easeTime = time;
		es.pathStartNode=1;
		es.pathEndNode=path.path.Count-1;
		es.easingType = EasingType.SplinePath;
		es.target = TargetType.path;
		es.easingGroup = easingGroup;
		EasingManager.startEasing( es );
		
		es.setDelay( path.delay );
		return es;
	}
	
//	public static Easing easeNextFrames(int framesGap, string easingLabel, onEasingCompleteDelegate onComplete)
//	{
//		if(framesGap < 1)
//		{
//			Debug.LogError("Frames gap is " + framesGap + " but should be > 1");
//			return;
//		}
//		
//	}

	public static Easing easeNextFrame(string easingGroup,onEasingCompleteDelegate onComplete)
	{
		Easing es = new Easing();
		es.inValue = 0;
		es.outValue = 1;
		es.easeTime = 0.1f;
		es.easingType = EasingType.Linear;
		es.fn = EasingManager.getFunction( es.easingType );
		es.target = TargetType.floatValue;
		es.easingGroup = easingGroup;
		EasingManager.startEasing( es );
		es.onEasingComplete=onComplete;
		return es;
	}	
	
	public static Easing easeFloat( float start, float end, float time, EasingType type )
	{
		return easeFloat( start, end, time, type, "default" );
	}
	
	public static Easing easeFloat( float start, float end, float time, EasingType type, string easingGroup )
	{
		Easing es = new Easing();
		es.inValue = start;
		es.outValue = end;
		es.easeTime = time;
		es.easingType = type;
		es.fn = EasingManager.getFunction( type );
		es.target = TargetType.floatValue;
		es.easingGroup = easingGroup;
		EasingManager.startEasing( es );
		return es;
	}	
	
	public static Easing easeVector3( Vector3 start, Vector3 end, float time, EasingType type )
	{
		return easeVector3( start, end, time, type, "default" );
	}
	
	public static Easing easeVector3( Vector3 start, Vector3 end, float time, EasingType type, string easingGroup )
	{
		Easing es = new Easing();
		es.inVector = start;
		es.outVector = end;
		es.easeTime = time;
		es.easingType = type;
		es.fn = EasingManager.getFunction( type );
		es.target = TargetType.vector3;
		es.easingGroup = easingGroup;
		EasingManager.startEasing( es );
		return es;
	}	
	
	public static Easing easeColor( Color start, Color end, float time, EasingType type )
	{
		return easeColor( start, end, time, type, "default" );
	}
	
	public static Easing easeColor( Color start, Color end, float time, EasingType type, string easingGroup )
	{
		Easing es = new Easing();
		es.inColor = start;
		es.outColor = end;
		es.easeTime = time;
		es.easingType = type;
		es.fn = EasingManager.getFunction( type );
		es.target = TargetType.color;
		es.easingGroup = easingGroup;
		EasingManager.startEasing( es );
		return es;
	}
	
	public static Easing easeToPosition( Transform transform, Vector3 start, Vector3 end, float time, EasingType type, Space space=Space.World )
	{
		return easeToPosition( transform,  start,  end,  time,  type,  "default", space );
	}
	
	public static Easing easeToPosition( Transform transform, Vector3 start, Vector3 end, float time, EasingType type, string easingGroup, Space space=Space.World )
	{
		Easing es = new Easing();
		es.transform = transform;
		es.space = space;
		es.inVector = start;
		es.outVector = end;
		es.easeTime = time;
		es.easingType = type;
		es.fn = EasingManager.getFunction( type );
		es.target = TargetType.position;
		es.easingGroup = easingGroup;
		EasingManager.startEasing( es );
		return es;
	}	
	
	public static Easing moveTo( Transform transform, Vector3 end, float time, EasingType type, string easingGroup, Space space=Space.World )
	{
		Vector3 position;
		if(space==Space.World)
			position=transform.position;
		else
			position=transform.localPosition;
		
		return Easing.easeToPosition( transform, position, end, time, type, easingGroup, space )
				.setStartingOptions( StartingOptions.fetchStartingPosition );
	}
	
	public static Easing moveTo( Transform transform, Vector3 end, float time, EasingType type, Space space=Space.World )
	{
		Vector3 position;
		if(space==Space.World)
			position=transform.position;
		else
			position=transform.localPosition;
		
		return Easing.easeToPosition( transform, position, end, time, type, "default", space )
				.setStartingOptions( StartingOptions.fetchStartingPosition );
	}
	
	public static Easing easeToRotation( Transform transform, Quaternion start, Quaternion end, float time, EasingType type, Space space=Space.World )
	{
		return easeToRotation( transform,  start,  end,  time,  type,  "default", space );
	}
	
	public static Easing easeToRotation( Transform transform, Quaternion start, Quaternion end, float time, EasingType type, string easingGroup, Space space=Space.World )
	{
		Easing es = new Easing();
		es.transform = transform;
		es.space=space;
		
		es.inVector = start.eulerAngles;
		Vector3 endRotation=end.eulerAngles;
		
		for(int i=0;i<3;i++)
		{
			if(es.inVector[i]<180.0f && endRotation[i] > es.inVector[i]+180.0f)
				endRotation[i]-=360.0f;
			if(es.inVector[i]>=180.0f && endRotation[i] < es.inVector[i]-180.0f)
				endRotation[i]+=360.0f;
		}
		
		es.outVector = endRotation;
		
		es.easeTime = time;
		es.easingType = type;
		es.fn = EasingManager.getFunction( type );
		es.target = TargetType.rotation;
		es.easingGroup = easingGroup;
		EasingManager.startEasing( es );
		return es;
	}
	
	public static Easing rotateTo( Transform transform, Quaternion end, float time, EasingType type, string easingGroup, Space space=Space.World )
	{
		Quaternion rotation;
		if(space==Space.World)
			rotation=transform.rotation;
		else
			rotation=transform.localRotation;
		
		return Easing.easeToRotation( transform, rotation, end, time, type, easingGroup, space )
			.setStartingOptions( StartingOptions.fetchStartingRotation );
	}
	
	public static Easing rotateTo( Transform transform, Quaternion end, float time, EasingType type, Space space=Space.World )
	{
		Quaternion rotation;
		if(space==Space.World)
			rotation=transform.rotation;
		else
			rotation=transform.localRotation;
		
		return Easing.easeToRotation( transform, rotation, end, time, type, "default", space )
			.setStartingOptions( StartingOptions.fetchStartingRotation );
	}
	
	public Easing pause( )
	{
		paused = true;
		return this;
	}
	
	public Easing play( )
	{
		paused = false;
		return this;
	}
	
	public Easing setDelay( float delay )
	{
		this.delay = delay;
		return this;
	}
	
	public Easing setStartingOptions( StartingOptions opt )
	{
		startingOptions = opt;
		return this;
	}
	
	public Easing queueEasing( Easing es )
	{
		if( chainedEasings == null )
			chainedEasings = new List<Easing>();
		es.chained = true;
		chainedEasings.Add( es );
		return this;
	}
	
	public Easing setEaseTime( float easeTime )
	{
		this.easeTime = easeTime;
		return this;
	}
	
	public Easing loopMe( EasingLoopType loopType )
	{
		looping = loopType;
		return this;
	}
	
	public Easing setCustomFunction( EasingManager.EasingFunction func )
	{
		fn = func;
		return this;
	}
	
	public Easing setAnimationCurveName( string name )
	{
		animationCurveName = name;
		return this;
	}
	
	public Easing alignPathWithSpeed( bool align )
	{
		pathAlignWithSpeed = align;
		return this;
	}
	
	public Easing pathStartEndNodes(float start,float end)
	{
		pathStartNode=start;
		pathEndNode=end;
		return this;
	}
	
	public Easing lookAt( Transform target, Vector3 _vectorUP )
	{
		lookAtTransform = target;
		vectorUp = _vectorUP;
		return this;
	}

	#endregion
	
	#region internals
	public bool started;
	public bool ease( float updateTime )
	{
		bool result;
		if( delay > 0 )
			delay -= updateTime;
		
		if( delay > 0 )
			return false;
		else
		{
			elapsed += (-delay);
			delay = 0;
		}
		
		if( elapsed <= easeTime || !completed ) 
		{			
			if( !started )
			{
				started = true;
				if( onEasingStart != null )
					onEasingStart();
			}
			elapsed = Mathf.Clamp( elapsed, 0f, easeTime );
			normalizedTime = elapsed/easeTime;
			
			switch( target )
			{
				case TargetType.floatValue:		easeFloat	(  ); 		break;
				case TargetType.position: 		easePosition(  ); 		break;
				case TargetType.rotation: 		easeRotation(  ); 		break;
				case TargetType.path:			easePath	(  ); 		break;
				case TargetType.vector3:		easeVector3 (  );		break;
				case TargetType.color:			easeColor	(  );		break;
			}
			
			if( elapsed == easeTime)
					completed = true;
				
			elapsed += updateTime;
			return false;
		}
		else
		{
			switch( looping )
			{
				case EasingLoopType.Loop: complete(); elapsed =0; completed = false; result = false; break;
				default:	result =  complete(); break;
			}
			return result;
		}
	}
	
	protected void easePath(  )
	{
		positionSpline = new CRSpline( path.getPositions() );
		rotationSpline = new CRSpline( path.getRotations() );
		
		float startI=(1.0f/(positionSpline.pts.Length-3))*(pathStartNode-1);
		float endI=(1.0f/(positionSpline.pts.Length-3))*(pathEndNode-1);
		
		float i=startI+ ((endI-startI)*normalizedTime);
		
		Vector3 newPos = positionSpline.Interp( i );
		if (transform==null)
		{
			Debug.Log("EasePath error: "+easingGroup + " " + animationCurveName);
			return;
		}
		if(space==Space.World)
			transform.position = newPos;
		else
			transform.localPosition = newPos;
		
		if( lookAtTransform!= null )
		{
			transform.LookAt( lookAtTransform, vectorUp );
		}
		else if( pathAlignWithSpeed )
		{
			Vector3 velVector = positionSpline.Velocity( i);
			if(space==Space.World)
				transform.rotation = Quaternion.LookRotation( velVector );
			else
				transform.localRotation = Quaternion.LookRotation( velVector );
		}
		else
		{
			Vector3 newRot = rotationSpline.Interp( i );
			if(space==Space.World)
				transform.rotation = Quaternion.Euler( newRot );
			else
				transform.localRotation = Quaternion.Euler( newRot );
		}	
		
	}
	
	protected void easeFloat(  )
	{
		float val = fn( elapsed,  easeTime,  inValue,  outValue, animationCurveName );
		if( onEasingProgressFloat != null )
			onEasingProgressFloat( this, val );			
	}
	
	protected Vector3 easeVector3(  )
	{
		float xp = fn( elapsed,  easeTime,  inVector.x,  outVector.x, animationCurveName );
		float yp = fn( elapsed,  easeTime,  inVector.y,  outVector.y, animationCurveName );
		float zp = fn( elapsed,  easeTime,  inVector.z,  outVector.z, animationCurveName );
		Vector3 vec = new Vector3( xp, yp, zp );
		if( onEasingProgressVector3 != null )
			onEasingProgressVector3( this, vec );	
		return vec;
	}
	
	protected Color easeColor(  )
	{
		float red 	= fn( elapsed,  easeTime,  inColor.r,  outColor.r, animationCurveName );
		float green = fn( elapsed,  easeTime,  inColor.g,  outColor.g, animationCurveName );
		float blue 	= fn( elapsed,  easeTime,  inColor.b,  outColor.b, animationCurveName );
		float alpha = fn( elapsed,  easeTime,  inColor.a,  outColor.a, animationCurveName );
		Color col = new Color( red, green, blue, alpha );
		if( onEasingProgressColor != null )
			onEasingProgressColor( this, col );	
		return col;
	}
	
	protected void easePosition(  )
	{
		Vector3 vec = easeVector3( );
		
		if(space==Space.World)
			transform.position = new Vector3( vec.x, vec.y, vec.z );
		else
			transform.localPosition = new Vector3( vec.x, vec.y, vec.z );
	}
	
	protected void easeRotation(  )
	{
		Vector3 vec = easeVector3( );
		if(space==Space.World)
			transform.rotation = Quaternion.Euler( new Vector3( vec.x, vec.y, vec.z ) );
		else
			transform.localRotation = Quaternion.Euler( new Vector3( vec.x, vec.y, vec.z ) );
	}
	
	public void starting( )
	{
		if( startingOptions == StartingOptions.fetchStartingPosition )
			if(space==Space.World)
				inVector = transform.position;
			else
				inVector = transform.localPosition;
		
		if( startingOptions == StartingOptions.fetchStartingRotation )
			if(space==Space.World)
				inVector = transform.rotation.eulerAngles;
			else
				inVector = transform.localRotation.eulerAngles;
		
	}
	
	protected bool complete( )
	{
		if( onEasingComplete != null )
			onEasingComplete(this);
     
        if ( chainedEasings != null )
			foreach( Easing es in chainedEasings )
			{
				es.chained = false;
				EasingManager.startEasing( es );
			}
		return true;
	}
	
	public void clear()
	{
		EasingManager.removeEasing( this );
	}
	
	public void removeMe()
	{
		if( onEasingRemoved != null )
			onEasingRemoved( this );
	}
	#endregion
}