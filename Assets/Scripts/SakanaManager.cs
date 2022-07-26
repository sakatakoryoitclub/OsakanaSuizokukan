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
	/// 初期化処理をまとめてする
	/// </summary>
	private void Start()
	{
		//ウィンドウサイズを変更
		Screen.SetResolution( SettingData.WidthResolution, SettingData.HeightResolution ,SettingData.IsFullScreen, 60);
		
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
			string imgPath = SettingData.GetFullPath(n);

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
			yield return new WaitForSeconds(SettingData.CoolTime);

			//画像ファイルの更新じくが変化するまで待機
			yield return new WaitWhile(() => lastTime.Equals(new FileInfo(imgPath).LastWriteTime));

			if (SettingData.UseKeyReload)
			{
				yield return new WaitUntil(() => Input.GetKeyDown(SettingData.ReloadKeyCode));
			}
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
			sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
		}

		return sprite;
	}
}