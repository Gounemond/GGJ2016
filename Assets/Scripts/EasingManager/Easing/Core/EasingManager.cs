using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EasingType {
	Custom,
	CustomAnimationCurve,
	SplinePath,
	Linear,
	InOutCubic,
	InOutQuintic,
	InQuintic,
	InQuartic,
	InCubic,
	InQuadratic,
	OutQuintic,
	OutQuartic,
	OutCubic,
	OutInCubic,
	OutInQuartic,
	BackInCubic,
	BackInQuartic,
	OutBackCubic,
	OutBackQuartic,
	OutElasticSmall,
	OutElasticBig,
	InElasticSmall,
	InElasticBig
}

[System.Serializable]
public class EasingCurve
{
	public string 			name;
	public AnimationCurve 	curve;
}

public class EasingManager : MonoBehaviour {
	
	#region properties
	public delegate float EasingFunction(float _elapsed, float _total, float _start, float _end, string reference);
	
	protected static Dictionary<string,List<Easing>> activeEasings;
	protected static Dictionary<EasingType,EasingFunction> easings = new Dictionary<EasingType, EasingFunction>();
	
	public static string[] easingLabels= {
								"Custom", 			"CustomAnimationCurve", 	"SplinePath",		"Linear", 
								"InOutCubic", 		"InOutQuintic",				"InQuintic",		"InQuartic",
								"InCubic",			"InQuadratic",				"OutQuintic",		"OutQuartic",
								"OutCubic",			"OutInCubic",				"OutInQuatic",		"BackInCubic",
								"BackInQuartic",	"OutBackCubic",				"OutBackQuartic",	"OutElasticSmall",
								"OutElasticBig",	"InElasticSmall",			"InElasticBig"							}; 
	
	public List<EasingCurve> customCurves;
	public Dictionary<string, AnimationCurve> customCurvesDict;
	
	protected static bool 						paused 		= false;
	protected static Dictionary<string, bool> 	groupsPause;
	public float speedFactor = 1f;
	
	protected 	static 		EasingManager		_self;
	public		static 		EasingManager		Self
	{
		get
		{
			if( _self == null ) _self = FindObjectOfType( typeof ( EasingManager ) ) as EasingManager;
			return _self;
		}
	}
	#endregion
	
	#region init
	void Awake () 
	{
		groupsPause 	= new Dictionary<string, bool>();
		activeEasings 	= new Dictionary<string, List<Easing>>();
		
		easings[EasingType.CustomAnimationCurve] 	= new EasingFunction( easeCustomAnimationCurve );
		easings[EasingType.Custom] 					= new EasingFunction( easeCustom );
		easings[EasingType.Linear] 					= new EasingFunction( easeLinear );
		easings[EasingType.InOutCubic] 				= new EasingFunction( easeInOutCubic );
		easings[EasingType.InQuintic] 				= new EasingFunction( easeInQuintic );
		easings[EasingType.InOutQuintic] 			= new EasingFunction( easeInOutQuintic );
		easings[EasingType.InQuartic] 				= new EasingFunction( easeInQuartic );
		easings[EasingType.InCubic] 				= new EasingFunction( easeInCubic );
		easings[EasingType.InQuadratic] 			= new EasingFunction( easeInQuadratic );
		easings[EasingType.OutQuintic] 				= new EasingFunction( easeOutQuintic );
		easings[EasingType.OutQuartic] 				= new EasingFunction( easeOutQuartic );
		easings[EasingType.OutCubic] 				= new EasingFunction( easeOutCubic );
		easings[EasingType.OutInCubic] 				= new EasingFunction( easeOutInCubic );
		easings[EasingType.OutInQuartic] 			= new EasingFunction( easeOutInQuartic );
		easings[EasingType.BackInCubic] 			= new EasingFunction( easeBackInCubic );
		easings[EasingType.BackInQuartic] 			= new EasingFunction( easeBackInQuartic );
		easings[EasingType.OutBackCubic] 			= new EasingFunction( easeOutBackCubic );
		easings[EasingType.OutBackQuartic] 			= new EasingFunction( easeOutBackQuartic );
		easings[EasingType.OutElasticSmall] 		= new EasingFunction( easeOutElasticSmall );
		easings[EasingType.OutElasticBig] 			= new EasingFunction( easeOutElasticBig );
		easings[EasingType.InElasticSmall] 			= new EasingFunction( easeInElasticSmall );
		easings[EasingType.InElasticBig] 			= new EasingFunction( easeInElasticBig );
		
		customCurvesDict = new Dictionary<string, AnimationCurve>();
		foreach( EasingCurve crv in customCurves )
			customCurvesDict[ crv.name ] = crv.curve;
	}
	#endregion
	
	#region helpers
	public static EasingFunction getFunction( EasingType type )
	{
		return easings[ type ];
	}
	
	static List<Easing> toAdd = new List<Easing>();
	public static void startEasing(Easing ease)
	{
		toAdd.Add( ease );
	}
	
	public static void _startEasing(Easing ease)
	{
		List<Easing> eGroup;
		string easingGroup = ease.easingGroup;
		if( activeEasings.ContainsKey( easingGroup ) )
			eGroup = activeEasings[ easingGroup ];
		else
			eGroup = new List<Easing>();
		
		ease.starting();
		
		eGroup.Add( ease );
		activeEasings[easingGroup] = eGroup;
	}
	
	static List<Easing> toRemove = new List<Easing>();
	public static void removeEasing( Easing es )
	{
		toRemove.Add( es );
		es.toBeRemoved = true;
	}
	
	public static void _removeEasing( Easing es )
	{
		if( activeEasings.ContainsKey( es.easingGroup ) )
		{
			es.removeMe();
			activeEasings[ es.easingGroup].Remove( es );
		}
	}
	
	public static void removeEasingGroup( string easingGroup )
	{
		if (activeEasings==null)
			return;
		if( activeEasings.ContainsKey( easingGroup ) )
		{
			
			List<Easing> ease = activeEasings[ easingGroup ];
			Easing es;
			for( int i = ease.Count-1; i>=0; i-- )
			{
				es = ease[ i ];
				es.removeMe();
				ease.RemoveAt( i );
			}
			ease.Clear();
		}
	}
	
	public static void pauseGroup( string easingGroup )
	{
		if (activeEasings==null)
			return;
		if( activeEasings.ContainsKey( easingGroup ) )
		{	
			List<Easing> ease = activeEasings[ easingGroup ];
			
			for( int i = ease.Count-1; i>=0; i-- )
			{
				ease[ i ].pause();
			}
		}
	}
	public static void playGroup( string easingGroup )
	{
		if (activeEasings==null)
			return;
		if( activeEasings.ContainsKey( easingGroup ) )
		{	
			List<Easing> ease = activeEasings[ easingGroup ];
			
			for( int i = ease.Count-1; i>=0; i-- )
			{
				ease[ i ].play();
			}
		}
	}
	
	public static void removeAllEasings( )
	{
	/*
		Easing es;
		foreach( KeyValuePair<string,List<Easing>> ease in activeEasings )			
			for( int i = ease.Value.Count-1; i>=0; i-- )
			{
				es = ease.Value[ i ];
				es.removeMe();
				ease.Value.RemoveAt( i );
			}
		activeEasings.Clear();
	*/
		foreach( KeyValuePair<string,List<Easing>> ease in activeEasings)
			for( int i = ease.Value.Count-1; i>=0; i-- )
				ease.Value[i].clear();
		toAdd.Clear();
	}
	
	public static bool isPaused( )
	{
		return paused;
	}
	
	public static bool isPaused( string group )
	{
		if( paused )
			return true;
		if ( groupsPause.ContainsKey( group ) && groupsPause[ group] == true )
			return true;
		return false;
	}
	
	public static void pause()
	{
		paused = true;
	}
	
	public static void play()
	{
		paused = false;
	}
	
	public static void togglePause()
	{
		paused = !paused;
	}
	#endregion	
	
	#region internals
	// Update is called once per frame
	void Update () 
	{
		if( paused )
			return;
		
		foreach( Easing ease in toAdd )
			if( !ease.chained )
				_startEasing( ease );
		toAdd.Clear();
		
		if (activeEasings==null) 
			return;
		foreach( KeyValuePair<string,List<Easing>> ease in activeEasings )
		{
			for( int i = ease.Value.Count-1; i>=0; i-- )
			{
				Easing es = ease.Value[ i ];
				if(!es.toBeRemoved && !es.paused && !isPaused( es.easingGroup ) )
				{
					if( es.ease( Time.deltaTime * speedFactor) )
					{
						es.clear();
						//if (i<ease.Value.Count)
						//	ease.Value.RemoveAt( i );
					}
				}
			}
		}
		
		foreach( Easing ease in toRemove )
			_removeEasing( ease );
		toRemove.Clear();
	}
	#endregion
	
	#region easing_functions
	public float easeCustomAnimationCurve(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		if(customCurvesDict.ContainsKey(reference)==false)
		{
			Debug.LogError("Custom animation curve not found: '"+reference+"'");
			return 0;
		}
		float val = customCurvesDict[ reference ].Evaluate( t );
		return _start+(_end-_start)*val;
	}
	
	public float easeCustom(float _elapsed, float _total, float _start, float _end, string reference)
	{
		Debug.Log( "custom function not set" );
		return _start;
	}
	
	public float easeLinear(float _elapsed, float _total, float _start, float _end, string reference)
	{
		return _start+(_end-_start)*(_elapsed/_total);
	}
	
	public float easeInOutCubic(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float ts = t*t;
		float tc = ts*t;
		return _start+(_end-_start)*(-2*tc+3*ts);
	}
	
	public float easeInQuintic(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float ts = t*t;
		float tc = ts*t;
		return _start+(_end-_start)*(tc*ts);
	}
	
	public float easeInOutQuintic(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float ts = t*t;
		float tc = ts*t;
		return _start+(_end-_start)*(6*tc*ts+ -15*ts*ts+10*tc);
	}
	
	public float easeInQuartic(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float ts = t*t;
		return _start+(_end-_start)*(ts*ts);
	}
	
	public float easeInCubic(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float tc = t*t*t;
		return _start+(_end-_start)*(tc);
	}
	
	public float easeInQuadratic(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float ts = t*t;
		return _start+(_end-_start)*(ts);
	}
	
	public float easeOutQuintic(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float ts = t*t;
		float tc = ts*t;
		return _start+(_end-_start)*(tc*ts + -5*ts*ts + 10*tc + -10*ts + 5*t);
	}
	
	public float easeOutQuartic(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float ts = t*t;
		float tc = ts*t;
		return _start+(_end-_start)*(-1*ts*ts + 4*tc + -6*ts +4*t);
	}
	
	public float easeOutCubic(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float ts = t*t;
		float tc = ts*t;
		return _start+(_end-_start)*( tc + -3*ts +3*t);
	}
	
	public float easeOutInCubic(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float ts = t*t;
		float tc = ts*t;
		return _start+(_end-_start)*( 4*tc + -6*ts +3*t);
	}
	
	public float easeOutInQuartic(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float ts = t*t;
		float tc = ts*t;
		return _start+(_end-_start)*(6*tc + -9*ts + 4*t);
	}
	
	public float easeBackInCubic(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float ts = t*t;
		float tc = ts*t;
		return _start+(_end-_start)*(4*tc + -3*ts);
	}
	
	public float easeBackInQuartic(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float ts = t*t;
		float tc = ts*t;
		return _start+(_end-_start)*(1*ts*ts + 2*tc + -3*ts);
	}
	
	public float easeOutBackCubic(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float ts = t*t;
		float tc = ts*t;
		return _start+(_end-_start)*(4*tc+ -9*ts + 6*t);
	}
	
	public float easeOutBackQuartic(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float ts = t*t;
		float tc = ts*t;
		return _start+(_end-_start)*( -2*ts*ts + 10*tc+ -15*ts + 8*t);
	}
	
	public float easeOutElasticSmall(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float ts = t*t;
		float tc = ts*t;
		return _start+(_end-_start)*( 33*tc*ts + -106*ts*ts + 126*tc + -67*ts +15*t);
	}
	
	public float easeOutElasticBig(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float ts = t*t;
		float tc = ts*t;
		return _start+(_end-_start)*( 56*tc*ts + -175*ts*ts + 200*tc + -100*ts +20*t);
	}
	
	public float easeInElasticSmall(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float ts = t*t;
		float tc = ts*t;
		return _start+(_end-_start)*( 33*tc*ts + -59*ts*ts + 32*tc + -5*ts );
	}
	
	public float easeInElasticBig(float _elapsed, float _total, float _start, float _end, string reference)
	{
		float t = _elapsed/_total;
		float ts = t*t;
		float tc = ts*t;
		return _start+(_end-_start)*( 56*tc*ts + -105*ts*ts + 60*tc + -10*ts );
	}
	#endregion
}
