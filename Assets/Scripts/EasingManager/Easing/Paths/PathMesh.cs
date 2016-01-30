using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PathMesh : MonoBehaviour
{
	PathScript path;
	
	public bool refresh=false;
	
	public Transform meshPrefab;
	public string prefabName;
	
	void Start ()
	{
		path=GetComponent<PathScript>();
		
		if(GetComponent<MeshFilter>()==null)
			gameObject.AddComponent<MeshFilter>();
			
		if(GetComponent<MeshRenderer>()==null)
			gameObject.AddComponent<MeshRenderer>();
		
		try{
		meshPrefab=((GameObject)Resources.Load(prefabName)).transform;
		}catch{}
	}
	
	void Update ()
	{
		if(!refresh)
			return;
			
		generateMesh();
		
		//refresh=false;
	}
	
	void generateMesh()
	{		
		if(meshPrefab==null)
			return;
		List<Vector3> vertices=new List<Vector3>();
		List<int>[] triangles=new List<int>[3];
		triangles[0]=new List<int>();
		triangles[1]=new List<int>();
		triangles[2]=new List<int>();
		List<Vector2> uv=new List<Vector2>();
		List<Vector3> normals=new List<Vector3>();
		List<Vector4> tangents=new List<Vector4>();
		
		CRSpline posSpline = new CRSpline( path.getPositions() );
		//CRSpline rotSpline = new CRSpline( path.getRotations() );
		
		Mesh[] mesh=new Mesh[3];
		mesh[0]=meshPrefab.FindChild("start").GetComponent<MeshFilter>().sharedMesh;
		mesh[1]=meshPrefab.FindChild("middle").GetComponent<MeshFilter>().sharedMesh;
		mesh[2]=meshPrefab.FindChild("end").GetComponent<MeshFilter>().sharedMesh;
		
		Vector3[][] meshVertices=new Vector3[3][];
		int[][] meshTriangles=new int[3][];
		Vector2[][] meshUv=new Vector2[3][];
		Vector3[][] meshNormals=new Vector3[3][];
		Vector4[][] meshTangents=new Vector4[3][];
		
		float[] max=new float[3];
		
		for(int i=0;i<3;i++)
		{
			meshVertices[i]=mesh[i].vertices;
			meshTriangles[i]=mesh[i].GetTriangles(0);
			meshUv[i]=mesh[i].uv;
			meshNormals[i]=mesh[i].normals;
			meshTangents[i]=mesh[i].tangents;
			
			for(int vi=0;vi<meshTriangles[i].Length;vi++)
			{
				if(meshVertices[i][meshTriangles[i][vi]].x>max[i])
					max[i]=meshVertices[i][meshTriangles[i][vi]].x;
			}
		}
		
		float cursor=0;
		int vertI=0;
		
		int meshI=0;
		if(path.round)
			meshI=1;
		bool last=false;
		//float scale=1;
		while(true)
		{
			if(meshI==2)
				break;
			if(getT(cursor+max[meshI])<1.0f || last)
			{
				if(last)
				{
					//scale=1.0f/(getT(cursor+max[meshI])-getT(cursor))*(1.0f-getT(cursor));
					
				}
				if(getT(cursor+max[meshI]+max[2])>1.0f && !path.round)
					meshI=2;
					
				for(int i=0;i<meshTriangles[meshI].Length;i++)
				{
					triangles[meshI].Add(vertI+meshTriangles[meshI][i]);
				}
				vertI+=mesh[meshI].vertexCount;
				
				for(int i=0;i<meshVertices[meshI].Length;i++)
				{
					float vertexCursor=cursor+Mathf.Max(meshVertices[meshI][i].x,0);
					float t=getT(vertexCursor);
					Vector3 vPos=posSpline.Interp(t);
					Vector3 look=posSpline.Velocity(t).normalized;
					
					Quaternion r=Quaternion.LookRotation(look);
					r=Quaternion.Euler(0,-90,0)*Quaternion.Euler(r.eulerAngles.z,(r.eulerAngles.y),-r.eulerAngles.x);
				
					Vector3 localVert=meshVertices[meshI][i];
					localVert.x=0;
					Vector3 vert=(r*localVert)+(vPos-transform.position);
					//vert.x-=middleVertices[i].x;
					
					//Debug.Log(t);
					vertices.Add(vert);
					
					uv.Add(meshUv[meshI][i]);
					normals.Add(r*meshNormals[meshI][i]);
					tangents.Add(r*meshTangents[meshI][i]);
				}
				//Debug.Log(max);
				cursor+=max[meshI];
				
				if(meshI==0)
					meshI=1;
					
				if(last)
					break;
			}
			else
			{
				if(!last)
					last=true;
				else
					break;
			}
			
		}
		
		Mesh finalMesh=new Mesh();
		finalMesh.vertices=vertices.ToArray();
		finalMesh.subMeshCount=3;
		finalMesh.SetTriangles(triangles[0].ToArray(),0);
		finalMesh.SetTriangles(triangles[1].ToArray(),1);
		finalMesh.SetTriangles(triangles[2].ToArray(),2);
		finalMesh.uv=uv.ToArray();
		finalMesh.normals=normals.ToArray();
		finalMesh.tangents=tangents.ToArray();
		GetComponent<MeshFilter>().sharedMesh=finalMesh;
		
		Material[] sharedMaterials=new Material[3];
		sharedMaterials[0]=meshPrefab.FindChild("start").GetComponent<MeshRenderer>().sharedMaterial;
		sharedMaterials[1]=meshPrefab.FindChild("middle").GetComponent<MeshRenderer>().sharedMaterial;
		sharedMaterials[2]=meshPrefab.FindChild("end").GetComponent<MeshRenderer>().sharedMaterial;
		
		GetComponent<Renderer>().sharedMaterials=sharedMaterials;
	}

	float getT(float vertexCursor)
	{
		//vertexCursor=processsCursor(vertexCursor);
		//int numSection=Mathf.Min(Mathf.FloorToInt((vertexCursor/path.splineLenght)*(path.path.Count-3)),path.path.Count-3);
		int numSection=0;
		while( numSection+1<=path.path.Count-3 && vertexCursor>cumulativeLen(numSection+1))
			numSection++;
		
		
		float t=1.1f;
		if(numSection<path.path.Count-2)
			t=((1.0f/(path.path.Count-3))*numSection) +((vertexCursor-cumulativeLen(numSection))/path.splineLengths[numSection]*(1.0f/(path.path.Count-3)) );
		//float t=vertexCursor/path.splineLenght;
		return t;
	} 
	
	float cumulativeLen(int section)
	{
		float ret=0;
		for(int i=0;i<section;i++)
			ret+=path.splineLengths[i];
		return ret;
	}
	float sectionWeight(int section)
	{
		return ((path.splineLengths[section]/path.splineLenght));
	}
	float processsCursor(float cursor)
	{
		float ret=0;
		for(int i=0;i<path.path.Count-3;i++)
		{
			float oldCursor=cursor;
			if(cursor-path.splineLengths[i]>0)
				cursor-=path.splineLengths[i];
			else
				cursor=0;
			
			ret+=(oldCursor-cursor)*sectionWeight(i);
		}
		return ret;
	}
}
