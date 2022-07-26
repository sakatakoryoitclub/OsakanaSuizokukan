using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class PassScan : MonoBehaviour {

	SpriteRenderer MainSpriteRenderer;
	string Timed,Timing;
	public string Path;
	string Pathh ;
	public int Frame;
	public bool Keybord=false;
	System.IO.FileInfo fi;
	Texture2D KDM;
	int count=0;
	Sprite FJI;
	// Use this for initialization
	void Start () {
		Pathh = "P:\\おさかな画像\\sakana"+Path+".png";
		//MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		//Texture2D KDM = Texture2DFromFile (Pathh);
		//Sprite FJI = SpriteFromTexture2D (KDM);
		//MainSpriteRenderer.sprite = FJI;
	}

	// Update is called once per frame
	void Update () {
		count++;
		if (Keybord == false) {
			if (count == Frame) {
				count=0;
				Debug.Log(Pathh);
				fi = new System.IO.FileInfo (Pathh);
				Timing = fi.LastWriteTime.ToString ();
				if (Timed == null) {
					Timed = Timing;
				}	
				//作成日時の取得
				//Debug.Log(fi.CreationTime);
				//更新日時の取得
				if (!(Timing.Equals (Timed))) {
					Debug.Log ("変ったよ");
					KDM = Texture2DFromFile (Pathh);
					FJI = SpriteFromTexture2D (KDM);
					MainSpriteRenderer.sprite = FJI;
					Timed = Timing;
				}
			}	
		} else {
			if (Input.GetKey(KeyCode.LeftControl)) {
				if(Input.GetKey(KeyCode.R)){
					fi = new System.IO.FileInfo (Pathh);
					Timing = fi.LastWriteTime.ToString ();
					if (Timed == null) {
						Timed = Timing;
					}	
					//作成日時の取得
					//Debug.Log(fi.CreationTime);
					//更新日時の取得
					if (!(Timing.Equals (Timed))) {
						Debug.Log ("変ったよ");
						KDM = Texture2DFromFile (Pathh);
						FJI = SpriteFromTexture2D (KDM);
						MainSpriteRenderer.sprite = FJI;
						Timed = Timing;
					}						
				}	
			}
		}	

		//Texture2D KDM = Texture2DFromFile ("Y:\\01共有フォルダ\\20_各部活動\\ITサイエンス部\\20-Unity班\\新しいフォルダー\\IMGGTK.png");
		//Sprite FJI = SpriteFromTexture2D (KDM);
		//MainSpriteRenderer.sprite = FJI;
		//sleepAsync ();
	}
	public Texture2D Texture2DFromFile(string path)
	{
		Texture2D texture = null;
		if (File.Exists(path))
		{
			//byte取得
			var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			var bin = new BinaryReader(fileStream);
			var readBinary = bin.ReadBytes((int)bin.BaseStream.Length);
			bin.Close();
			fileStream.Dispose();
			fileStream = null;
			if (readBinary != null)
			{
				//横サイズ
				var pos = 16;
				var width = 0;
				for (var i = 0; i < 4; i++)
				{
					width = width * 256 + readBinary[pos++];
				}
				//縦サイズ
				var height = 0;
				for (var i = 0; i < 4; i++)
				{
					height = height * 256 + readBinary[pos++];
				}
				//byteからTexture2D作成
				texture = new Texture2D(width, height);
				texture.LoadImage(readBinary);
			}
			readBinary = null;
		}
		return texture;
	}
	public Sprite SpriteFromTexture2D(Texture2D texture)
	{
		Sprite sprite = null;
		if (texture)
		{
			//Texture2DからSprite作成
			sprite = Sprite.Create(texture, new UnityEngine.Rect(0, 0, texture.width, texture.height), Vector2.zero);
		}
		return sprite;
	}
}