using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 魚の処理をまとめて管理するManager
/// </summary>
public class SakanaManager : MonoBehaviour
{
	/// <summary>
	/// 魚のSpriteRendererをまとめる配列 設定しない場合はManagerの子オブジェクトから取得する
	/// </summary>
	[SerializeField] private SpriteRenderer[] sakanaImages;
	
	/// <summary>
	/// 画像のPath(Ex: C://SakanaImg/sakana10.png の場合は C://SakanaImg/sakana まで入力
	/// </summary>
	[SerializeField] private string imagePath;
	
	/// <summary>
	/// 拡張子
	/// </summary>
	[SerializeField] private Extentions extention;
	
	/// <summary>
	/// リロードに使用するキー (オプション) 
	/// </summary>
	[SerializeField] private KeyCode reloadKey;
	
	/// <summary>
	/// 画像の更新のクールダウンタイム
	/// </summary>
	[SerializeField] private float coolDownTime = 30f;

	
	/// <summary>
	/// 初期化処理をまとめてする
	/// </summary>
	private void Start()
	{
		//sakanaImagesの中にちゃんと値が入っているか？
		if (sakanaImages.Count(img => img != null) == 0)
		{
			//なかったらSakanaManagerの子オブジェクトをまとめて取得
			sakanaImages = GetComponentsInChildren<SpriteRenderer>();
		}

		//魚の数だけループ
		for (int i = 0; i < sakanaImages.Length; i++)
		{
			//それぞれの魚で非同期処理を実行
			StartCoroutine(ImageProcess(sakanaImages[i], i + 1));
		}
	}

	/// <summary>
	/// 魚の更新処理を非同期で行うコルーチン
	/// </summary>
	/// <param name="sakana">魚のSpriteRenderer</param>
	/// <param name="n">魚の番号 (1～)</param>
	/// <returns></returns>
	private IEnumerator ImageProcess(SpriteRenderer sakana, int n)
	{
		//実行中はずっと以下の処理を実行
		while (Application.isPlaying)
		{
			//それぞれの画像Path
			string imgPath = imagePath + n + "." + extention.ToString();
			
			//画像の最終更新時間を取得
			var lastTime = new FileInfo(imgPath).LastWriteTime;
			
			//Http通信で画像を取得するやつ (Texture限定 Audio用とかもある)
			var request = UnityWebRequestTexture.GetTexture("file://" + imgPath);
			
			//Http通信が終わるまで待つ
			yield return request.SendWebRequest();

			//通信結果が成功しなてなければ (エラーが出たら)
			if (request.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError($"Error is {n}");
				//Whileからやり直し
				continue;
			}
			
			//通信結果からテクスチャを取得
			Texture2D tex = ((DownloadHandlerTexture) request.downloadHandler).texture;
			
			//Texture2DからSpriteに変換して、Sakanaのオブジェクトに代入
			sakana.sprite = SpriteFromTexture2D(tex);
			
			//クールタイムが終わるまで待機
			yield return new WaitForSeconds(coolDownTime);

			//画像ファイルの更新じくが変化するまで待機
			yield return new WaitWhile(() => lastTime.Equals(new FileInfo(imgPath).LastWriteTime));

			//yield return new WaitUntil(() => Input.GetKeyDown(reloadKey));
		}
	}

	/// <summary>
	/// Texture2DからSpriteに変換
	/// </summary>
	/// <param name="texture">変換対象のTexture</param>
	/// <returns>Textureから変換されたSprite</returns>
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