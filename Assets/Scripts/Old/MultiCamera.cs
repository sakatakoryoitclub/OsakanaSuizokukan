using UnityEngine;

public class MultiCameras : MonoBehaviour
{
	[SerializeField, Range( 1, 8 )]
	private int m_useDisplayCount   = 2;

	private void Awake()
	{
		var count   = Mathf.Min( Display.displays.Length, m_useDisplayCount );

		for( var i = 0; i < count; ++i )
		{
			Display.displays[ i ].Activate();
		}
	}

} // class GameController