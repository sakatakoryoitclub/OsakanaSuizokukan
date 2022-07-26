using UnityEngine;

public class MouseVector: MonoBehaviour{

	void Updata(){
		
	}

	void OnMouseDrag(){
		var objectPointInScreen
		= Camera.main.WorldToScreenPoint(this.transform.position);

		var mousePointInScreen
		= new Vector3(Input.mousePosition.x,
			Input.mousePosition.y,
			objectPointInScreen.z);

		var mousePointInWorld = Camera.main.ScreenToWorldPoint(mousePointInScreen);
		mousePointInWorld.z = this.transform.position.z;
		this.transform.position = mousePointInWorld;
	}
}