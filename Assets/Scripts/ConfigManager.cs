using System;
using System.Linq;
using SFB;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// ConfigSceneでの設定を行うクラス
/// </summary>
public class ConfigManager : MonoBehaviour
{
	/// <summary>
	/// 次のシーンに行くボタン
	/// </summary>
	[SerializeField] private Button startButton;

	/// <summary>
	/// 次のシーンの名前
	/// </summary>
	[SerializeField] private string nextSceneName;

	# region Path周り

	/// <summary>
	/// ディレクトリを入力するInputField
	/// </summary>
	[SerializeField] private TMP_InputField pathInput;

	/// <summary>
	/// エクスプローラーから検索するボタン
	/// </summary>
	[SerializeField] private Button pathChoiceButton;

	/// <summary>
	/// Prefix用のInputField
	/// </summary>
	[SerializeField] private TMP_InputField prefixInput;

	/// <summary>
	/// 拡張子を指定するドロップダウン
	/// </summary>
	[SerializeField] private TMP_Dropdown extentionDropdown;

	# endregion

	# region クールタイム周り

	/// <summary>
	/// クールタイムを設定するスライダー
	/// </summary>
	[SerializeField] private Slider coolTimeSlider;

	/// <summary>
	/// クールタイムを設定するInputField
	/// </summary>
	[SerializeField] private TMP_InputField coolTimeInput;

	# endregion

	# region 解像度周り

	/// <summary>
	/// 横幅解像度を設定するInputField
	/// </summary>
	[SerializeField] private TMP_InputField widthInput;
	
	/// <summary>
	/// 縦幅解像度を設定するInputField
	/// </summary>
	[SerializeField] private TMP_InputField heightInput;
	
	/// <summary>
	/// フルスクリーンかどうかを設定するトグル
	/// </summary>
	[SerializeField] private Toggle fullScreenToggle;

	# endregion

	# region キーリロード周り

	/// <summary>
	/// キーリロードするか、否かを設定するトグル
	/// </summary>
	[SerializeField] private Toggle keyReloadToggle;
	
	/// <summary>
	/// キーリロードのトリガーとなるキーを設定するドロップダウン
	/// </summary>
	[SerializeField] private TMP_Dropdown reloadKeyDropdown;

	# endregion

	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Start()
	{
		//解像度をHDに設定 コンテンツ上映時の解像度だと設定画面が使いづらいであろうため
		Screen.SetResolution(1280, 720, false, 60);

		//ボタンがクリックされたときに実行される処理を設定
		startButton.onClick.AddListener(
			() => //以下の　() => {} はラムダ式という
		{
			//Config画面のデータを登録
			SubmitSetting();
			
			//次のシーンへ以降
			SceneManager.LoadScene(nextSceneName);
		});
		
		//Path周りのGUIの初期設定
		PathInit();
		
		//クールタイム周りのGUIの初期設定
		CoolTimeInit();
		
		//キーリロード周りのGUIの初期設定
		KeyReloadInit();
	}

	/// <summary>
	/// Path周りのGUIの初期化
	/// </summary>
	private void PathInit()
	{
		//Choice ボタンが押されたときの処理
		pathChoiceButton.onClick.AddListener(() =>
		{
			//エクスプローラーのダイアログを開いて取得
			string path = StandaloneFileBrowser.OpenFolderPanel("Choice directory", "C://", false)
				.FirstOrDefault(); // OpenFolderPanelは長さが1の配列で帰ってくるため、配列の先頭を取得 (Linqで実装)
			
			//pathが空のとき
			if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
			{
				//早期リターン 
				return;
			}

			//PathのInputFieldに代入
			pathInput.text = path;
		});

		//拡張子のドロップダウンの設定
		extentionDropdown.options =
			Enum.GetValues(typeof(Extentions)).Cast<Extentions>() //拡張子のEnumをまとめて取得 (この時点では拡張子単体のコレクション)
			.Select(ex => new TMP_Dropdown.OptionData(ex + "")).ToList();//Linqを使ってドロップダウンのオプションとして設定できるリストに変換
	}

	/// <summary>
	/// クールタイム周りのGUIの初期設定
	/// </summary>
	private void CoolTimeInit()
	{
		//スライダーが変わったとき、InputFieldも同じ値に
		coolTimeSlider.onValueChanged.AddListener(value => coolTimeInput.text = $"{value}");
		
		//InputFieldが変わったとき、Sliderも同じ値に
		coolTimeInput.onValueChanged.AddListener(value => coolTimeSlider.value = float.Parse(value));
		
		//初期値を設定
		coolTimeSlider.value = 30f;
	}

	/// <summary>
	/// キーリロード周りのGUIの初期設定
	/// </summary>
	private void KeyReloadInit()
	{
		//キーリロードを使わないときはKeyCodeのドロップダウンを非表示
		keyReloadToggle.onValueChanged.AddListener(active => reloadKeyDropdown.gameObject.SetActive(active));
		
		//トグルに初期値を設定
		keyReloadToggle.isOn = false;

		//キーリロードの選択肢の設定
		reloadKeyDropdown.options = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>()
			.Select(key => new TMP_Dropdown.OptionData(key + "")).ToList();

		//ドロップダウンに初期値を設定
		reloadKeyDropdown.value = (int)KeyCode.F5;
	}

	/// <summary>
	/// グローバルな設定インスタンスに値を設定
	/// </summary>
	private void SubmitSetting()
	{
		//代入しているだけなので多分分かるはず　説明省略！
		
		# region Pathの設定
		
		SettingData.FolderPath = pathInput.text;
		SettingData.Prefix = prefixInput.text;
		SettingData.Extention = (Extentions) extentionDropdown.value;

		# endregion

		#region クールタイム周り

		SettingData.CoolTime = coolTimeSlider.value;

		#endregion

		#region 解像度周り

		SettingData.WidthResolution = int.Parse(widthInput.text);
		SettingData.HeightResolution = int.Parse(heightInput.text);
		SettingData.IsFullScreen = fullScreenToggle.isOn;

		#endregion

		#region キーリロード周り

		SettingData.UseKeyReload = keyReloadToggle.isOn;
		SettingData.ReloadKeyCode = (KeyCode) reloadKeyDropdown.value;

		#endregion
	}
}