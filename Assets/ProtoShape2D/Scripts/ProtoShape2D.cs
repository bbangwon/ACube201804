using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer)),ExecuteInEditMode]
public class ProtoShape2D:MonoBehaviour{
	//Type of the object
	public PS2DType type=PS2DType.Simple;
	//Points which are used to construct the shape
	public List<PS2DPoint> points=new List<PS2DPoint>(200);
	//Actual points used to draw the shape, generated automatically
	public List<Vector3> pointsFinal=new List<Vector3>(500);
	//Points for a collider, generated automatically
	public List<PS2DColliderPoint> cpoints=new List<PS2DColliderPoint>(500);
	//Actual points to draw a collider, also used to draw collider in scene view
	public Vector2[] cpointsFinal;
	//Just for displaying the number
	public int triangleCount=0;
	//Fill parameters
	public PS2DFillType fillType=PS2DFillType.Color;
	public float textureScale=1f;
	public float textureRotation=0f;
	public Vector2 textureOffset;
	public Color color1=Color.red;
	public Color color2=Color.red;
	//Gradient parameters
	public float gradientScale=1f;
	public float gradientRotation=0;
	public float gradientOffset=0;
	//
	public Material material=null;
	public Material spriteMaterial=null;
	public Texture2D texture=null;
	public string uniqueName="";
	public int curveIterations=10;
	//For "antialiasing" algorythm
	public bool antialias=false;
	public float aaridge=0.02f;
	public List<Vector2> outline=new List<Vector2>(500);
	//For sorting layers
	public int sortingLayer=0;
	private int _sortingLayer;
	public int orderInLayer=0;
	private int _orderInLayer;
	//Snap settings
	public PS2DSnapType snapType=PS2DSnapType.Points;
	public float gridSize=1f;
	//For pivot adjustment
	public PS2DPivotPositions PivotPosition=PS2DPivotPositions.Disabled;
	//For collider type
	public PS2DColliderType colliderType=PS2DColliderType.None;
	public float colliderTopAngle=90f;
	public float colliderOffsetTop=0f;
	public bool showNormals=false;
	//A mesh for mesh collider
	public Mesh cMesh;
	//A width (depth) of mesh for the mesh collider
	public float cMeshDepth=3f;
	//For calculating normals
	private float edgeSum=0;
	public bool clockwise=true;
	//Mesh management
	private MeshRenderer mr;
	private MeshFilter mf;
	private List<Vector3> vertices=new List<Vector3>(200);
	private List<Color> colors=new List<Color>(200);
	private List<Vector2> uvs=new List<Vector2>(200);
	private int[] tris;
	private int[] trisRidge;
	//To track the position of the object
	private Vector2 lastPos;
	//Min and max point of the object
	public Vector2 minPoint;
	public Vector2 maxPoint;
	//For foldouts in inspector
	public bool showFillSettings=true;
	public bool showMeshSetting=true;
	public bool showSnapSetting=true;
	public bool showColliderSettings=true;
	public bool showTools=true;

	private void Awake(){
		mr=GetComponent<MeshRenderer>();
		mf=GetComponent<MeshFilter>();
		vertices=new List<Vector3>();
		colors=new List<Color>();
		uvs=new List<Vector2>();
		//If it's new object
		if(uniqueName==""){
			uniqueName=Random.Range(1000,999999).ToString();
			mr.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;
			mr.receiveShadows=false;
			mr.lightProbeUsage=UnityEngine.Rendering.LightProbeUsage.Off;
			mr.reflectionProbeUsage=UnityEngine.Rendering.ReflectionProbeUsage.Off;
			SetSpriteMaterial();
		//It already has unique name, but is it really unique?
		//Happens when you clone an object or instantiate a prefab
		}else{
			ProtoShape2D[] arr=GameObject.FindObjectsOfType<ProtoShape2D>();
			for(int i=0;i<arr.Length;i++){
				if(arr[i].uniqueName==uniqueName){
					uniqueName=Random.Range(1000,999999).ToString();
					if(fillType==PS2DFillType.Color) SetSpriteMaterial();
					else if(fillType==PS2DFillType.CustomMaterial) SetCustomMaterial();
					else SetDefaultMaterial();
				}
			}
		}
		Mesh mesh=new Mesh();
		mesh.name=uniqueName;
		mf.sharedMesh=mesh;
		lastPos=transform.position;
		UpdateMaterialSettings();
		UpdateMesh();
	}

	void Update(){
		if(mr.sharedMaterial!=null && fillType!=PS2DFillType.CustomMaterial && fillType!=PS2DFillType.Color){
			if(!lastPos.Equals(transform.position)){
				lastPos=transform.position;
				mr.sharedMaterial.SetVector("_WPos",transform.position);
				mr.sharedMaterial.SetVector("_MinWPos",mr.bounds.min);
				mr.sharedMaterial.SetVector("_MaxWPos",mr.bounds.max);
			}
		}
		if(sortingLayer!=_sortingLayer || orderInLayer!=_orderInLayer){
			mr.sortingLayerID=sortingLayer;
			mr.sortingOrder=orderInLayer;
			_sortingLayer=sortingLayer;
			_orderInLayer=orderInLayer;
		}
	}


	public void SetSpriteMaterial(Material mat){
		this.spriteMaterial=mat;
		SetSpriteMaterial();
	}

	public void SetSpriteMaterial(){
		#if UNITY_EDITOR
		if(this.spriteMaterial==null){
			//this.spriteMaterial=AssetDatabase.GetBuiltinExtraResource<Material>("Sprites-Default.mat");
			this.spriteMaterial=(Material)Resources.Load("PS2DSpritesDefault");
		}
		#endif
		mr.sharedMaterial=spriteMaterial;
	}

	public void SetDefaultMaterial(){
		this.material=new Material(Shader.Find("ProtoShape2D/TextureAndColors"));
		material.name="PS2DTextureAndColors";
		mr.sharedMaterial=material;
	}

	public void SetCustomMaterial(){
		SetCustomMaterial(this.material);
	}

	public void SetCustomMaterial(Material mat){
		this.material=mat;
		mr.sharedMaterial=this.material;
	}

	public void UpdateMaterialSettings(){
		if(mf!=null && mr!=null){
			if(mr.sharedMaterial!=null && fillType!=PS2DFillType.CustomMaterial && fillType!=PS2DFillType.Color){
				if(fillType==PS2DFillType.Texture){
					mr.sharedMaterial.SetVector("_Color1",Color.white);
					mr.sharedMaterial.SetVector("_Color2",Color.white);
				}
				if(fillType==PS2DFillType.Color || fillType==PS2DFillType.TextureWithColor){
					mr.sharedMaterial.SetVector("_Color1",color1);
					mr.sharedMaterial.SetVector("_Color2",color1);
				}
				if(fillType==PS2DFillType.Gradient || fillType==PS2DFillType.TextureWithGradient){
					mr.sharedMaterial.SetVector("_Color1",color1);
					mr.sharedMaterial.SetVector("_Color2",color2);
				}
				mr.sharedMaterial.SetFloat("_GradientAngle",gradientRotation);
				mr.sharedMaterial.SetFloat("_GradientScale",gradientScale);
				mr.sharedMaterial.SetFloat("_GradientOffset",gradientOffset);
				if(fillType==PS2DFillType.Texture || fillType==PS2DFillType.TextureWithColor || fillType==PS2DFillType.TextureWithGradient){
					mr.sharedMaterial.SetTexture("_Texture",texture);
					//Set texture size
					if(texture!=null){
						mr.sharedMaterial.SetTextureScale("_Texture",new Vector2(texture.width,texture.height)/100f*textureScale);
						mr.sharedMaterial.SetFloat("_TextureAngle",textureRotation);
						mr.sharedMaterial.SetVector("_TextureOffset",textureOffset);
					}
				}else{
					mr.sharedMaterial.SetTexture("_Texture",null);
				}
			}
		}
	}

	public void UpdateMesh(){
		if(mf!=null && mr!=null){
			if(mr.sharedMaterial!=null){
				//Find if polygon is drawn clockwise or counterclockwise
				edgeSum=0;
				for(int i=0;i<points.Count;i++){
					edgeSum+=(points[i].position.x-points.Loop(i+1).position.x)*(points[i].position.y+points.Loop(i+1).position.y);
				}
				clockwise=edgeSum<=0;
				//Generate bezier handles
				if(type==PS2DType.Simple) GenerateHandles();
				//Clear mesh properties
				vertices.Clear();
				colors.Clear();
				uvs.Clear();
				minPoint=Vector2.one*9999f;
				maxPoint=-Vector2.one*9999f;
				//Decide vertex color
				Color vcolor=(fillType==PS2DFillType.Color?color1:Color.white);
				//Generate vertices and colors, get bounds
				for(int i=0;i<points.Count;i++){
					if(points[i].curve>0f || points.Loop(i+1).curve>0f){
						for(int j=0;j<curveIterations;j++){
							vertices.Add((Vector2)CalculateBezierPoint(
								(float)j/(float)curveIterations,
								points[i].position,
								points[i].handleN,
								points.Loop(i+1).handleP,
								points.Loop(i+1).position
							));
							colors.Add(color1);
							if(vertices[vertices.Count-1].x<minPoint.x) minPoint.x=vertices[vertices.Count-1].x;
							if(vertices[vertices.Count-1].y<minPoint.y) minPoint.y=vertices[vertices.Count-1].y;
							if(vertices[vertices.Count-1].x>maxPoint.x) maxPoint.x=vertices[vertices.Count-1].x;
							if(vertices[vertices.Count-1].y>maxPoint.y) maxPoint.y=vertices[vertices.Count-1].y;
						}
					}else{
						vertices.Add((Vector3)points[i].position);
						colors.Add(vcolor);
						//Get 
						if(points[i].position.x<minPoint.x) minPoint.x=points[i].position.x;
						if(points[i].position.y<minPoint.y) minPoint.y=points[i].position.y;
						if(points[i].position.x>maxPoint.x) maxPoint.x=points[i].position.x;
						if(points[i].position.y>maxPoint.y) maxPoint.y=points[i].position.y;
					}
				}
				//Generate UVs based on bounds
				for(int i=0;i<vertices.Count;i++){
					uvs.Add(new Vector2(
						Mathf.InverseLerp(minPoint.x,maxPoint.x,vertices[i].x),
						Mathf.InverseLerp(minPoint.y,maxPoint.y,vertices[i].y)
					));
				}
				//Save shape's outline
				pointsFinal=new List<Vector3>(vertices);
				//Triangulate
				Triangulator triangulator=new Triangulator(vertices);
				tris=triangulator.Triangulate();
				//Anti-aliasing
				outline.Clear();
				if(antialias){
					aaridge=0.002f*(Camera.main!=null?Camera.main.orthographicSize*2:10f);
					int vertCount=vertices.Count;
					Vector2 normal1;
					Vector2 normal2;
					for(int i=0;i<vertices.Count;i++){
						normal1=(vertices[i]-vertices.Loop(i+1)).normalized;
						normal1=new Vector2(normal1.y,-normal1.x)*aaridge;
						normal2=(vertices.Loop(i+1)-vertices.Loop(i+2)).normalized;
						normal2=new Vector2(normal2.y,-normal2.x)*aaridge;
						if(!clockwise){
							normal1*=-1;
							normal2*=-1;
						}
						outline.Add(LineIntersectionPoint(
							(Vector2)vertices[i]+normal1,
							(Vector2)vertices.Loop(i+1)+normal1,
							(Vector2)vertices.Loop(i+1)+normal2,
							(Vector2)vertices.Loop(i+2)+normal2
						));
					}
					Color clear=new Color(vcolor.r,vcolor.g,vcolor.b,0.0f);
					for(int i=0;i<outline.Count;i++){
						vertices.Add(outline[i]);
						colors.Add(clear);
						uvs.Add(Vector2.zero);
					}
					trisRidge=new int[tris.Length+(outline.Count*2*3)];
					for(int i=0;i<tris.Length;i++){
						trisRidge[i]=tris[i];
					}
					for(int i=0;i<outline.Count;i++){
						trisRidge[tris.Length+(i*6)+0]=i;
						trisRidge[tris.Length+(i*6)+1]=(vertCount+i-1)<vertCount?vertCount+outline.Count-1:vertCount+i-1;
						trisRidge[tris.Length+(i*6)+2]=vertCount+i;
						trisRidge[tris.Length+(i*6)+3]=i;
						trisRidge[tris.Length+(i*6)+4]=vertCount+i;
						trisRidge[tris.Length+(i*6)+5]=(i+1>vertCount-1)?0:i+1;
					}
					tris=trisRidge;
				}
				//Set mesh
				mf.sharedMesh.Clear();
				mf.sharedMesh.SetVertices(vertices);
				mf.sharedMesh.SetColors(colors);
				mf.sharedMesh.SetUVs(0,uvs);
				mf.sharedMesh.RecalculateBounds();
				mf.sharedMesh.SetTriangles(tris,0);
				//Update trieangle count
				triangleCount=(mf.sharedMesh.triangles.Length/3);
				//Pass min and max positions to shader
				mr.sharedMaterial.SetVector("_WPos",transform.position);
				mr.sharedMaterial.SetVector("_MinWPos",mr.bounds.min);
				mr.sharedMaterial.SetVector("_MaxWPos",mr.bounds.max);
			}
		}
		UpdateCollider();
	}

	private void UpdateCollider(){
		Collider2D col=GetComponent<Collider2D>();
		MeshCollider mcol=GetComponent<MeshCollider>();
		if(col!=null || mcol!=null){
			//Create points for collider
			cpoints.Clear();
			for(int i=0;i<points.Count;i++){
				if(points[i].curve>0f || points.Loop(i+1).curve>0f){
					for(int j=0;j<curveIterations;j++){
						cpoints.Add(new PS2DColliderPoint((Vector2)CalculateBezierPoint(
							(float)j/(float)curveIterations,
							points[i].position,
							points[i].handleN,
							points.Loop(i+1).handleP,
							points.Loop(i+1).position
						)));
						cpoints[cpoints.Count-1].wPosition=transform.TransformPoint(cpoints[cpoints.Count-1].position);
					}
				}else{
					cpoints.Add(new PS2DColliderPoint((Vector2)points[i].position));
					cpoints[cpoints.Count-1].wPosition=transform.TransformPoint(cpoints[cpoints.Count-1].position);
				}
			}
			//Create normals and directions for every point
			for(int i=0;i<cpoints.Count;i++){
				//Setting normal
				cpoints[i].normal=cpoints[i].wPosition-cpoints.Loop(i+1).wPosition;
				cpoints[i].normal=new Vector2(cpoints[i].normal.y,-cpoints[i].normal.x).normalized;
				if(!clockwise) cpoints[i].normal*=-1;
				//Deciding direction
				cpoints[i].signedAngle=Vector2Extension.SignedAngle(Vector2.up,cpoints[i].normal);
				if(Mathf.Abs(cpoints[i].signedAngle)<=colliderTopAngle/2) cpoints[i].direction=PS2DDirection.Up;
				else if(cpoints[i].signedAngle>(colliderTopAngle/2) && cpoints[i].signedAngle<135) cpoints[i].direction=PS2DDirection.Left;
				else if(cpoints[i].signedAngle<-(colliderTopAngle/2) && cpoints[i].signedAngle>-135) cpoints[i].direction=PS2DDirection.Right;
				else if(Mathf.Abs(cpoints[i].signedAngle)>=135) cpoints[i].direction=PS2DDirection.Down;
			}
			//Create array of points for collider
			if(col!=null){
				//Polygon collider
				if(col.GetType()==typeof(UnityEngine.PolygonCollider2D)){
					cpointsFinal=new Vector2[cpoints.Count];
					for(int i=0;i<cpoints.Count;i++){
						cpointsFinal[i]=cpoints[i].position;
						if(cpoints[i].direction==PS2DDirection.Up || cpoints.Loop(i-1).direction==PS2DDirection.Up) cpointsFinal[i]+=(Vector2.up*colliderOffsetTop);
					}
					GetComponent<PolygonCollider2D>().points=cpointsFinal;
				//Full edge collider
				}else if(col.GetType()==typeof(UnityEngine.EdgeCollider2D) && colliderType==PS2DColliderType.Edge){
					cpointsFinal=new Vector2[cpoints.Count];
					for(int i=0;i<cpoints.Count;i++){
						cpointsFinal[i]=cpoints[i].position;
						if(cpoints[i].direction==PS2DDirection.Up || cpoints.Loop(i-1).direction==PS2DDirection.Up) cpointsFinal[i]+=(Vector2.up*colliderOffsetTop);
					}
					GetComponent<EdgeCollider2D>().points=cpointsFinal;
				//Top edge collider
				}else if(col.GetType()==typeof(UnityEngine.EdgeCollider2D) && colliderType==PS2DColliderType.TopEdge){
					int lowestWPoint=0;
					for(int i=0;i<cpoints.Count;i++){
						if(i==0 || cpoints[i].wPosition.y<cpoints[lowestWPoint].wPosition.y){
							lowestWPoint=i;			
						}
					}
					int edgeStartPoint=-1;
					for(int i=lowestWPoint;i<lowestWPoint+cpoints.Count;i++){
						if(cpoints.Loop(i).direction==PS2DDirection.Up){
							edgeStartPoint=cpoints.LoopID(i);
							break;
						}
					}
					int edgeEndPoint=-1;
					for(int i=lowestWPoint;i>lowestWPoint-cpoints.Count;i--){
						if(cpoints.Loop(i).direction==PS2DDirection.Up){
							edgeEndPoint=cpoints.LoopID(i+1);
							break;
						}
					}
					if(edgeStartPoint>=0 && edgeEndPoint>=0){
						//Find number of collider points
						int countPoints=1;
						for(int i=edgeStartPoint;i!=edgeEndPoint;i=cpoints.LoopID(i+1)){
							countPoints++;
						}
						if(countPoints>1){
							//Create collider points
							cpointsFinal=new Vector2[countPoints];
							for(int i=0;i<countPoints;i++){
								cpointsFinal[i]=cpoints.Loop(edgeStartPoint+i).position+(Vector2.up*colliderOffsetTop);
							}
							//Set collider points
							GetComponent<EdgeCollider2D>().enabled=true;
							GetComponent<EdgeCollider2D>().points=cpointsFinal;
						}else{
							GetComponent<EdgeCollider2D>().enabled=false;
						}
					}else{
						GetComponent<EdgeCollider2D>().enabled=false;
					}
				}
			}
			if(mcol!=null){
				//Create two sets of vertices for the mesh collier
				Vector3[] mVertices=new Vector3[mf.sharedMesh.vertices.Length*2];
				for(int i=0;i<mf.sharedMesh.vertices.Length;i++){
					mVertices[i]=mf.sharedMesh.vertices[i];
					mVertices[i].z-=cMeshDepth/2f;
				}
				for(int i=mf.sharedMesh.vertices.Length;i<mf.sharedMesh.vertices.Length*2;i++){
					mVertices[i]=mf.sharedMesh.vertices[i-mf.sharedMesh.vertices.Length];
					mVertices[i].z+=cMeshDepth/2f;
				}
				//Create triangles for mesh collider
				int[] mTriangles=new int[mf.sharedMesh.triangles.Length*2+((mf.sharedMesh.vertices.Length*2)*3)];
				for(int i=0;i<mf.sharedMesh.triangles.Length;i++){
					mTriangles[i]=mf.sharedMesh.triangles[i];
				}
				for(int i=mf.sharedMesh.triangles.Length*2-1;i>=mf.sharedMesh.triangles.Length;i--){
					mTriangles[(mf.sharedMesh.triangles.Length*2)+mf.sharedMesh.triangles.Length-1-i]=mf.sharedMesh.triangles[i-mf.sharedMesh.triangles.Length]+mf.sharedMesh.vertices.Length;
				}
				//Stich the two sides together
				for(int i=0;i<mf.sharedMesh.vertices.Length;i++){
					mTriangles[mf.sharedMesh.triangles.Length*2+(i*6)+0]=i; 
					mTriangles[mf.sharedMesh.triangles.Length*2+(i*6)+1]=mf.sharedMesh.vertices.Length+i;
					mTriangles[mf.sharedMesh.triangles.Length*2+(i*6)+2]=mf.sharedMesh.vertices.Length+i+1-(i==mf.sharedMesh.vertices.Length-1?mf.sharedMesh.vertices.Length:0);
					mTriangles[mf.sharedMesh.triangles.Length*2+(i*6)+3]=i;
					mTriangles[mf.sharedMesh.triangles.Length*2+(i*6)+4]=mf.sharedMesh.vertices.Length+i+1-(i==mf.sharedMesh.vertices.Length-1?mf.sharedMesh.vertices.Length:0);
					mTriangles[mf.sharedMesh.triangles.Length*2+(i*6)+5]=i+1-(i==mf.sharedMesh.vertices.Length-1?mf.sharedMesh.vertices.Length:0);
				}
				cMesh=new Mesh();
				cMesh.SetVertices(new List<Vector3>(mVertices));
				cMesh.SetTriangles(mTriangles,0);
				//cMesh.RecalculateBounds();
				//cMesh.RecalculateNormals();
				cMesh.name=transform.name;
				mcol.sharedMesh=null;
				mcol.sharedMesh=cMesh;
			}
		}
	}

	public Mesh GetMesh(){
		return mf.sharedMesh;
	}

	private void GenerateHandles(){
		for(int i=0;i<points.Count;i++){
			GenerateHandles(i);
		}
	}

	public void GenerateHandles(int i){
		//Find a median angle
		float angle=Vector2Extension.SignedAngle(
			(points.Loop(i+1).position-points[i].position).normalized,
			(points.Loop(i-1).position-points[i].position).normalized
		);
		if(angle>0) angle=-(360f-angle);
		Vector2 median=((points.Loop(i+1).position-points[i].position).normalized).Rotate(angle/2);
		if(!clockwise) median*=-1;
		//Check for sudden angle inversions when clockwise order didn't change
		if(points[i].clockwise==clockwise && Mathf.Abs(Vector2Extension.SignedAngle(points[i].median,median))>135){
			median*=-1;
		}
		//Calculate bezier handles
		points[i].handleP=median.Rotate(90*(clockwise?-1:1))+points[i].position;
		points[i].handleN=median.Rotate(90*(clockwise?1:-1))+points[i].position;
		//Multiply points by half of the distance to neighboring point
		points[i].handleP=((points[i].handleP-points[i].position)*(Vector2.Distance(points.Loop(i-1).position,points[i].position)*points[i].curve))+points[i].position;
		points[i].handleN=((points[i].handleN-points[i].position)*(Vector2.Distance(points.Loop(i+1).position,points[i].position)*points[i].curve))+points[i].position;
		//Store the cofiguration
		points[i].median=median;
		points[i].clockwise=clockwise;
	}

	public void StraightenHandles(int i){
		Vector2 middle=((points[i].handleP-points[i].position).normalized+(points[i].handleN-points[i].position).normalized).normalized;
		if(middle!=Vector2.zero){
			Vector2 newHandleP=middle.Rotate(-90f);
			Vector2 newHandleN=middle.Rotate(+90f);
			if(Vector2.Distance(points[i].handleP,newHandleP+points[i].position)<Vector2.Distance(points[i].handleP,newHandleN+points[i].position)){
				points[i].handleP=(newHandleP*(points[i].handleP-points[i].position).magnitude)+points[i].position;
				points[i].handleN=(newHandleN*(points[i].handleN-points[i].position).magnitude)+points[i].position;
			}else{
				points[i].handleP=(newHandleN*(points[i].handleP-points[i].position).magnitude)+points[i].position;
				points[i].handleN=(newHandleP*(points[i].handleN-points[i].position).magnitude)+points[i].position;
			}
		}
	}

	private Vector2 LineIntersectionPoint(Vector2 l1s, Vector2 l1e, Vector2 l2s, Vector2 l2e) {
		//Get A,B,C of first line
		float A1=l1e.y-l1s.y;
		float B1=l1s.x-l1e.x;
		float C1=A1*l1s.x+B1*l1s.y;
		//Get A,B,C of second line
		float A2=l2e.y-l2s.y;
		float B2=l2s.x-l2e.x;
		float C2=A2*l2s.x+B2*l2s.y;
		//Get delta and check if the lines are parallel
		float delta=A1*B2-A2*B1;
		//if(delta==0) throw new System.Exception("Lines are parallel");
		//Special case where the angle is too small
		if(delta<0.01f && delta>-0.01f && l1e==l2s) return l1e;
		// now return the Vector2 intersection point
		return new Vector2(
			(B2*C1-B1*C2)/delta,
			(A1*C2-A2*C1)/delta
		);
	}

	private Vector3 CalculateBezierPoint(float t,Vector3 p0,Vector3 p1,Vector3 p2,Vector3 p3){
		float u=1-t;
		float tt=t*t;
		float uu=u*u;
		float uuu=uu*u;
		float ttt=tt*t;
		Vector3 p=uuu*p0;
		p+=3*uu*t*p1;
		p+=3*u*tt*p2;
		p+=ttt*p3;
		return p;
	}

}
