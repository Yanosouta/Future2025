using UnityEngine;
using System.Linq;

public class KapiDicData : MonoBehaviour
{
    static int Kapis = 24;  //カピの数
    private int[] IsKapi = new int[Kapis]; // 図鑑の発見状況 (0: 未発見, 1: 発見済み)
    private int[] IsNewKapi = new int[Kapis]; // 新規登録状況 (0: 未登録, 1: 登録済み)
    private int[] KapiNum = new int[Kapis]; //カピに割り当てる番号
    private int IsSortAorD = 0;    //セーブ済みデータが存在しているかどうか


    
    //--------------------------------------------------------------------------------------
    //前ソート　配列の情報を保持する関数作成後、KapiDitionaryに受け渡し、そこで画像をソート
    //--------------------------------------------------------------------------------------

    private void Start()
    {
        // データをロードする
        LoadData();
        if(IsSortAorD == 1)
            SortKapi(0);
    }

    // データを保存するメソッド
    public void SaveData()
    {
        // IsDiscovered 配列をカンマ区切りの文字列に変換して保存
        PlayerPrefs.SetString("IsKapi", string.Join(",", IsKapi));

        // IsNewlyRegistered 配列をカンマ区切りの文字列に変換して保存
        PlayerPrefs.SetString("IsNewKapi", string.Join(",", IsNewKapi));

        PlayerPrefs.SetInt("IsSortAorD", IsSortAorD);

        // PlayerPrefsを保存 (データをディスクに書き込む)
        PlayerPrefs.Save();
    }



    // データを読み込むメソッド
    public void LoadData()
    {
        // IsKapiのデータを文字列で取得
        string KapiData = PlayerPrefs.GetString("IsKapi", "");
        if (!string.IsNullOrEmpty(KapiData))
        {
            // コンマ区切りのデータを配列に変換
            IsKapi = KapiData.Split(',').Select(int.Parse).ToArray();
        }

        // IsNewKapiのデータを文字列で取得
        string NewKapiData = PlayerPrefs.GetString("IsNewKapi", "");
        if (!string.IsNullOrEmpty(NewKapiData))
        {
            // カンマ区切りのデータを配列に変換
            IsNewKapi = NewKapiData.Split(',').Select(int.Parse).ToArray();
        }

        //IsSortAorDのデータを文字列で取得
        IsSortAorD = PlayerPrefs.GetInt("IsSortAorD",0);
    }

    public void SaveCharacterName(int characterIndex,string Name)
    {
            PlayerPrefs.SetString($"CharacterName_{characterIndex}", Name);
            PlayerPrefs.Save();
            Debug.Log($"{characterIndex}の名前を保存しました: {Name}");
    }

    public string LoadCharacterName(int characterIndex)
    {
        return PlayerPrefs.GetString($"CharacterName_{characterIndex}", $"名もなきカピ");
    }

    //0:昇順 1:降順 2:登録済カピを前に
    public void SortKapi(int Sort)
    {
        int C = 0;
        switch(Sort)    
        {
            case 0:
                //カピ番号昇順降順(切り替わり)
                for (int i = 0; i < Kapis / 2; i++)
                { 
                    C = IsKapi[i];
                    IsKapi[i]= IsKapi[(Kapis -1) - i];
                    IsKapi[(Kapis - 1) - i] = C;

                }
                if (IsSortAorD == 0)
                    IsSortAorD = 1;
                else
                    IsSortAorD = 0;

                break;
            case 2:
                //登録済カピを前にソート
                int k;
                for (int i = 0; i < IsKapi.Length; i++)
                {
                    if (IsKapi[i] == 0)
                    {
                        for (int j = i +1; j < IsKapi.Length; j++)
                        {
                            if (IsKapi[j] == 1)
                            {
                                IsKapi[i] = 1;
                                IsKapi[j] = 0;
                                break;
                            }
                        }
                    }
                }
                break;
        }
        SaveData();
    }

    // 図鑑のエントリーを発見としてマーク
    public void MarkAsKapi(int entryIndex)
    {
        if (entryIndex >= 0 && entryIndex < IsKapi.Length)
        {
            IsKapi[entryIndex] = 1; // 発見済みとしてマーク
            SaveData();
            Debug.Log("カピ番号" + entryIndex + "を新規登録しました。");

            //for (int i = 0; i < IsKapi.Length; i++)
                //Debug.Log("カピ番号" + i + "登録状況" +IsKapi[i]);
        }
    }

    // 図鑑のエントリーを新規登録としてマーク
    public void MarkAsNewlyKapi(int entryIndex)
    {
        if (entryIndex >= 0 && entryIndex < IsNewKapi.Length)
        {
            IsNewKapi[entryIndex] = 1; // 登録済みとしてマーク
            SaveData();
        }
    }

    public void DestroyKapi()
    {
        //全ての登録済カピを未登録に
        for (int i = 0; i < IsKapi.Length; i++)
        {
            if (IsKapi[i] == 1)
                IsKapi[i] = 0;
        }
        SaveData();

        //PlayerPrefs.DeleteAll();
        Debug.Log("全てのカピを消しました");
    }
    // 発見済みかどうかをチェック
    public bool IsEntryKapi(int entryIndex)
    {
        if (entryIndex >= 0 && entryIndex < IsKapi.Length)
        {
            return IsKapi[entryIndex] == 1;
        }
        return false;
    }

    // 新規登録済みかどうかをチェック
    public bool IsEntryNewKapi(int entryIndex)
    {
        if (entryIndex >= 0 && entryIndex < IsNewKapi.Length)
        {
            return IsNewKapi[entryIndex] == 1;
        }
        return false;
    }
}
