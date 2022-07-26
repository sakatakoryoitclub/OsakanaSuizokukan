using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koushin : MonoBehaviour {

	string Timed,Timing;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		System.IO.FileInfo fi = new System.IO.FileInfo(@"Y:\01共有フォルダ\20_各部活動\ITサイエンス部\20-Unity班\New Unity Project\Assets\mudai.png");
		Timing = fi.LastWriteTime.ToString();
		if (Timed == null) {
			Timed = Timing;
		}	
		//作成日時の取得
		//Debug.Log(fi.CreationTime);
		//更新日時の取得
		if (!(Timing.Equals(Timed))) {
			Debug.Log ("変ったよ");
			Timed = Timing;
		}	
	}
}
