using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class SakanaManager : MonoBehaviour
{
	[SerializeField] private SpriteRenderer[] sakanaImages;
	[SerializeField] private string imagePath;
	[SerializeField] private Extentions extention;
	[SerializeField] private KeyCode reloadKey;
	[SerializeField] private float coolDownTime = 30f;

	private void Start()
	{
		if (sakanaImages.Count(img => img != null) == 0)
		{
			sakanaImages = GetComponentsInChildren<SpriteRenderer>();
		}

		for (int i = 0; i < sakanaImages.Length; i++)
		{
			StartCoroutine(ImageProcess(sakanaImages[i], i + 1));
		}
	}

	private IEnumerator ImageProcess(SpriteRenderer sakana, int n)
	{
		while (true)
		{
			string imgPath = imagePath + n + "." + extention.ToString();
			var lastTime = new FileInfo(imgPath).LastWriteTime;
			var request = UnityWebRequestTexture.GetTexture("file://" + imgPath);
			yield return request.SendWebRequest();

			if (request.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError($"Error is {n}");
				continue;
			}

			Texture2D tex = ((DownloadHandlerTexture) request.downloadHandler).texture;
			sakana.sprite = SpriteFromTexture2D(tex);
			
			yield return new WaitForSeconds(coolDownTime);

			yield return new WaitWhile(() => lastTime.Equals(new FileInfo(imgPath).LastWriteTime));

			//yield return new WaitUntil(() => Input.GetKeyDown(reloadKey));
		}
	}

	private static Sprite SpriteFromTexture2D(Texture2D texture)
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