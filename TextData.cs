using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class StringBundle
{
    public string localizingString;
    //필요한 만큼 추가
}

public class TextData : Singleton<TextData>
{
    Dictionary<string, Hashtable> languagePacks = new Dictionary<string, Hashtable>(); //언어 변경 가능시 유지, 언어 자동 설정시 필요 없음
    public StringBundle currentLanguagePack; //현재 적용중인 언어팩
    public List<LocalizedText> textsOnScene = new List<LocalizedText>(); //언어 변경 가능시 유지, 자동 설정시 필요 없음

    private void Awake()
    {
        if (isInstanceNull())
        { //싱글톤
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        GetLanguagePacks(); //현지화 데이터 로드
                            //언어 변경 기능 삽입시 이 부분을 Application.systemLanguage가 아닌 세이브한 데이터에서 불러오면 됩니다.
        currentLanguagePack = SetLanguage(Application.systemLanguage);

        ApplyLanguage();
    }

    /// <summary>
    /// 사실 그다지 효율적이지만은 않으므로 xml을 로드하는 다른 방식을 선호한다면 주저 없이 바꾸면 됩니다.
	/// json같은 다른 데이터 형식으로도 물론 가능해요.
    /// </summary>
    private void GetLanguagePacks()
    {
        TextAsset xmlData = (TextAsset)Resources.Load("Text_Data"); //xml 파일 이름
        XmlDocument document = new XmlDocument();
        document.LoadXml(xmlData.text);

        XmlNodeList nodes = document.SelectSingleNode("Languages").ChildNodes; //xml파일의 노드명과 같아야 합니다

        foreach (XmlNode node in nodes)
        {
            Hashtable languagePack = new Hashtable();

            foreach (XmlElement element in node.ChildNodes)
            {
                languagePack.Add(element.GetAttribute("name"), element.InnerText);
            }
            languagePacks.Add(node.Name, languagePack);
        }
    }

    /// <summary>
    /// SystemLanguage가 아닌 string형으로 비교해도 무방. 존재하는 모든 언어코드를 넣는 게 아니라면 default 설정은 빼먹지 맙시다.
    /// </summary>
    private StringBundle SetLanguage(SystemLanguage language)
    {
        StringBundle result = new StringBundle();
        string languageString;

        switch (language)
        {
            case SystemLanguage.English:
                languageString = "English"; //xml파일의 Languages노드 내의 언어명들과 같게 입력해주세요.
                break;
            case SystemLanguage.Portuguese:
                languageString = "Portuguese";
                break;
            case SystemLanguage.ChineseSimplified:
                languageString = "ChineseS";
                break;
            case SystemLanguage.ChineseTraditional:
                languageString = "ChineseT";
                break;
            case SystemLanguage.Japanese:
                languageString = "Japanese";
                break;
            case SystemLanguage.Korean:
                languageString = "Korean";
                break;
            default:
                languageString = "English";
                break;
        }
        result.localizingString = GetString("localizedString", languageString); 
StringBundle의 각 멤버마다 하나씩 추가해주면 됩니다.

        return result;
    }

    /// <summary>
    /// xml 형식을 괴상하게 짜놔서 추가한 메소드. xml 로드 부분을 최적화한다면 이걸 뺄 수도 있겠네요.
    /// </summary>	
    private string GetString(string name, string language)
    {
        try
        {
            return (string)languagePacks[language][name];
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// 현재 씬에 있는 모든 LocalizedText에 현지화 텍스트 적용.
    /// </summary>
    private void ApplyLanguage()
    {
        for (int i = 0; i < textsOnScene.Count; i++)
        {
            textsOnScene[i].SetText();
        }
    }
}