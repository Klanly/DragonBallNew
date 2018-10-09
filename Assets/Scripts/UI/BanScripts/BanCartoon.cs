using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 战斗中，漫画展示的部分。现在已经不在使用。
/// </summary>
public class BanCartoon : MonoBehaviour {

	#region Static Function
	
	public static void DoUseNum(int num){
		Instantiate(Resources.Load("CartoonPieces/"+num),new Vector3(3000,0,0),Quaternion.identity);
	}
	
	#endregion

	#region Common Function

	public string textureName;

	public Texture2D texture{
		get{
			return Resources.Load("CartoonPieces/10134_3") as Texture2D;
		}
	}

	private Mesh mesh;

	private Material _material = null;

	public Material material{
		get{
			if(_material == null ){
				_material = Instantiate(Resources.Load("CartoonPieces/CartoonMaterial")) as Material;
			}
			return _material;
		}
	}

	public float widthByHeight = 4f/3;

	public Camera targetCamera;

	public bool b_EditMode = false;

	public bool b_FitBounds = false;

	public BanCartoonPiece [] banCartoonPieces;

	public float f_TotalTime = 2;

	public float f_HoldTime = 0.2f;

	public float f_PieceFlyTime = 0.5f;

	public float f_DisappearTime = 0.5f;

	private List<GameObject> list_Piece = new List<GameObject>();

	IEnumerator Start(){
		float aPieceTime = (f_TotalTime - f_HoldTime - f_DisappearTime)/banCartoonPieces.Length;
		for(int i = 0;i<banCartoonPieces.Length;i++){
			GeneratePieceMesh(banCartoonPieces[i],i);
			yield return new WaitForSeconds(aPieceTime);
		}
		yield return new WaitForSeconds(f_HoldTime);

		foreach(GameObject go in list_Piece){
			MiniItween.VertexColorTo(go,new V4(0,0,0,0),f_DisappearTime,MiniItween.EasingType.Normal);
		}
		Destroy(this.gameObject,f_DisappearTime);
	}

	void GeneratePieceMesh(BanCartoonPiece aPiece,int index){
		GameObject temp = new GameObject("CartoonPiece:"+index);

		float size = targetCamera.orthographicSize * 2;

		switch(aPiece.type){
		case BanCartoonPiece.Type.None:
			temp.transform.position = this.transform.position;
			break;
		case BanCartoonPiece.Type.Left:
			temp.transform.position = this.transform.position + Vector3.left * size * widthByHeight;
			break;
		case BanCartoonPiece.Type.Right:
			temp.transform.position = this.transform.position + Vector3.right * size * widthByHeight;
			break;
		case BanCartoonPiece.Type.Up:
			temp.transform.position = this.transform.position + Vector3.up * size;
			break;
		case BanCartoonPiece.Type.Down:
			temp.transform.position = this.transform.position + Vector3.down * size;
			break;
		}

		MiniItween.MoveTo(temp,this.transform.position,f_PieceFlyTime,MiniItween.EasingType.EaseOutQuart,true);

		Mesh tempMesh = new Mesh();
		tempMesh.vertices = new Vector3[]{
			new Vector3(aPiece.pos_1.x * widthByHeight,aPiece.pos_1.y,0),
			new Vector3(aPiece.pos_2.x * widthByHeight,aPiece.pos_2.y,0),
			new Vector3(aPiece.pos_3.x * widthByHeight,aPiece.pos_3.y,0),
			new Vector3(aPiece.pos_4.x * widthByHeight,aPiece.pos_4.y,0)
		};
		tempMesh.triangles = new int[]{0,1,2,0,2,3};
		tempMesh.uv = new Vector2[]{
			aPiece.uv_1,
			aPiece.uv_2,
			aPiece.uv_3,
			aPiece.uv_4
		};
		temp.AddComponent<MeshFilter>().mesh = tempMesh;
		temp.AddComponent<MeshRenderer>().material = material;
		material.mainTexture = texture;
		temp.transform.localScale = targetCamera.orthographicSize * 2 * Vector3.one;
		temp.transform.parent = this.transform;

		list_Piece.Add(temp);
	}

	void GenerateMainMesh(){
		if(this.mesh == null && GetComponent<MeshFilter>() == null){
			this.mesh = new Mesh();
			this.mesh.vertices = new Vector3[]{
				new Vector3(-0.5f * widthByHeight,0.5f,0),
				new Vector3(0.5f * widthByHeight,0.5f,0),
				new Vector3(0.5f * widthByHeight,-0.5f,0),
				new Vector3(-0.5f * widthByHeight,-0.5f,0)
			};
			this.mesh.triangles = new int[]{0,1,2,0,2,3};
			this.mesh.uv = new Vector2[]{
				new Vector2(0,1),
				new Vector2(1,1),
				new Vector2(1,0),
				new Vector2(0,0)
			};
			this.gameObject.AddComponent<MeshFilter>().mesh = this.mesh;
			this.gameObject.AddComponent<MeshRenderer>().material = material;
			material.mainTexture = texture;
			this.transform.localScale = targetCamera.orthographicSize * 2 * Vector3.one;
		}
	}

	void DestroyMainMesh(){
		if(this.mesh != null || GetComponent<MeshFilter>() != null){
			this.mesh = null;
			DestroyImmediate(this.gameObject.GetComponent<MeshFilter>());
			DestroyImmediate(this.gameObject.GetComponent<MeshRenderer>());
		}
	}

	void OnDrawGizmos(){

		if(Application.isPlaying){
			return;
		}

		if(b_EditMode){
			GenerateMainMesh();
		}else{
			DestroyMainMesh();
		}

		if(b_FitBounds){

			if(this.transform.childCount == 4){
				GameObject go_1 = this.transform.GetChild(0).gameObject;
				GameObject go_2 = this.transform.GetChild(1).gameObject;
				GameObject go_3 = this.transform.GetChild(2).gameObject;
				GameObject go_4 = this.transform.GetChild(3).gameObject;

				Vector3 center = this.renderer.bounds.center;
				Vector3 extents = this.renderer.bounds.extents;

				go_1.transform.position = center + Vector3.left * extents.x + Vector3.up * extents.y;
				go_2.transform.position = center + Vector3.right * extents.x + Vector3.up * extents.y;
				go_3.transform.position = center + Vector3.right * extents.x + Vector3.down * extents.y;
				go_4.transform.position = center + Vector3.left * extents.x + Vector3.down * extents.y;
			}

			b_FitBounds = false;
		}

		if(banCartoonPieces != null && banCartoonPieces.Length > 0){
			Gizmos.color = Color.green;
			float size = targetCamera.orthographicSize * 2;
			foreach(BanCartoonPiece aPiece in banCartoonPieces){

				if(aPiece.go != null && aPiece.go.transform.childCount == 4 ){
					if(aPiece.b_ReverseSet){
						aPiece.go.transform.GetChild(0).transform.position = (aPiece.pos_1.x * Vector3.right * widthByHeight + aPiece.pos_1.y * Vector3.up) * size + this.transform.position;
						aPiece.go.transform.GetChild(1).transform.position = (aPiece.pos_2.x * Vector3.right * widthByHeight + aPiece.pos_2.y * Vector3.up) * size + this.transform.position;
						aPiece.go.transform.GetChild(2).transform.position = (aPiece.pos_3.x * Vector3.right * widthByHeight + aPiece.pos_3.y * Vector3.up) * size + this.transform.position;
						aPiece.go.transform.GetChild(3).transform.position = (aPiece.pos_4.x * Vector3.right * widthByHeight + aPiece.pos_4.y * Vector3.up) * size + this.transform.position;
						aPiece.b_ReverseSet = false;
					}else{
						Vector3 v3_1 = aPiece.go.transform.GetChild(0).transform.position;
						Vector3 v3_2 = aPiece.go.transform.GetChild(1).transform.position;
						Vector3 v3_3 = aPiece.go.transform.GetChild(2).transform.position;
						Vector3 v3_4 = aPiece.go.transform.GetChild(3).transform.position;
						
						aPiece.pos_1 = new Vector2((v3_1.x - this.transform.position.x)/size/widthByHeight,
						                           (v3_1.y - this.transform.position.y)/size);
						aPiece.pos_2 = new Vector2((v3_2.x - this.transform.position.x)/size/widthByHeight,
						                           (v3_2.y - this.transform.position.y)/size);
						aPiece.pos_3 = new Vector2((v3_3.x - this.transform.position.x)/size/widthByHeight,
						                           (v3_3.y - this.transform.position.y)/size);
						aPiece.pos_4 = new Vector2((v3_4.x - this.transform.position.x)/size/widthByHeight,
						                           (v3_4.y - this.transform.position.y)/size);

						aPiece.uv_1 = new Vector2(aPiece.pos_1.x + 0.5f,aPiece.pos_1.y + 0.5f);
						aPiece.uv_2 = new Vector2(aPiece.pos_2.x + 0.5f,aPiece.pos_2.y + 0.5f);
						aPiece.uv_3 = new Vector2(aPiece.pos_3.x + 0.5f,aPiece.pos_3.y + 0.5f);
						aPiece.uv_4 = new Vector2(aPiece.pos_4.x + 0.5f,aPiece.pos_4.y + 0.5f);

					}
					aPiece.go = null;
				}

				if(aPiece != null && aPiece.b_Draw ){
					Gizmos.DrawLine(this.transform.position + Vector3.right * aPiece.pos_1.x * size * widthByHeight + Vector3.up * aPiece.pos_1.y * size,
					                this.transform.position + Vector3.right * aPiece.pos_2.x * size * widthByHeight + Vector3.up * aPiece.pos_2.y * size);
					
					Gizmos.DrawLine(this.transform.position + Vector3.right * aPiece.pos_2.x * size * widthByHeight + Vector3.up * aPiece.pos_2.y * size,
					                this.transform.position + Vector3.right * aPiece.pos_3.x * size * widthByHeight + Vector3.up * aPiece.pos_3.y * size);
					
					Gizmos.DrawLine(this.transform.position + Vector3.right * aPiece.pos_3.x * size * widthByHeight + Vector3.up * aPiece.pos_3.y * size,
					                this.transform.position + Vector3.right * aPiece.pos_4.x * size * widthByHeight + Vector3.up * aPiece.pos_4.y * size);
					
					Gizmos.DrawLine(this.transform.position + Vector3.right * aPiece.pos_4.x * size * widthByHeight + Vector3.up * aPiece.pos_4.y * size,
					                this.transform.position + Vector3.right * aPiece.pos_1.x * size * widthByHeight + Vector3.up * aPiece.pos_1.y * size);
				}
				//aPiece
			}

		}

		if(this.transform.childCount == 4){
			Gizmos.color = Color.red;
	
			Vector3 v3_1 = this.transform.GetChild(0).transform.position;
			Vector3 v3_2 = this.transform.GetChild(1).transform.position;
			Vector3 v3_3 = this.transform.GetChild(2).transform.position;
			Vector3 v3_4 = this.transform.GetChild(3).transform.position;

			Gizmos.DrawLine( v3_1,v3_2 );
			Gizmos.DrawLine( v3_2,v3_3 );
			Gizmos.DrawLine( v3_3,v3_4 );
			Gizmos.DrawLine( v3_4,v3_1 );
		}

	}

	void Update () {
		//banCartoonPiece
	}

}

[System.Serializable]
public class BanCartoonPiece{
	public Vector2 pos_1;
	public Vector2 pos_2;
	public Vector2 pos_3;
	public Vector2 pos_4;

	public Vector2 uv_1;
	public Vector2 uv_2;
	public Vector2 uv_3;
	public Vector2 uv_4;

	public GameObject go;
	public bool b_ReverseSet = false;

	public enum Type{None,Left,Right,Up,Down};

	public Type type;

	public bool b_Draw = true;

}


#endregion



















