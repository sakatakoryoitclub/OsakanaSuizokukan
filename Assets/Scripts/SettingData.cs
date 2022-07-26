using UnityEngine;

public class SettingData
{
	# region Path周り
	
	/// <summary>
	/// ディレクトリのPath
	/// </summary>
	public static string FolderPath = "C://SakanaImg";
	
	/// <summary>
	/// 魚のPrefix (ex: C:/ImageData/sakana_1.png なら sakana)
	/// </summary>
	public static string Prefix = "sakana";
	
	/// <summary>
	/// 画像の拡張子
	/// </summary>
	public static Extentions Extention = Extentions.png;


	/// <summary>
	/// 完全なPathを取得する
	/// </summary>
	/// <param name="n">番号</param>
	/// <returns>完全なPath</returns>
	public static string GetFullPath(int n)
	{
		return FolderPath + "//" + Prefix + n + "." + Extention;
	}
	
	# endregion

	#region クールタイム周り

	/// <summary>
	/// リロードまでのクールタイム
	/// </summary>
	public static float CoolTime = 30f;

	#endregion

	# region  解像度周り

	/// <summary>
	/// 横幅解像度
	/// </summary>
	public static int WidthResolution = 3840;
	
	/// <summary>
	/// 縦幅解像度
	/// </summary>
	public static int HeightResolution = 1080;
	
	/// <summary>
	/// フルスクリーンにするか否か
	/// </summary>
	public static bool IsFullScreen;
	
	# endregion
	
	#region キーリロード周り

	/// <summary>
	/// キーリロードするか否か
	/// </summary>
	public static bool UseKeyReload = false;
	
	/// <summary>
	/// リロードするキー
	/// </summary>
	public static KeyCode ReloadKeyCode = KeyCode.F5;

	#endregion

}