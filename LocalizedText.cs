using UnityEngine;
using UnityEngine.UI;

//이 스크립트를 현지화할 UI Text, 또는 Text Mesh가 존재하는 오브젝트에 달아주세요
public class LocalizedText: MonoBehaviour {
    public enum LocalizationTag {
        localizedString
		//StringBundle과 같은 숫자만큼 태그 생성, 인스펙터에서 설정하기 위함입니다.
    }
    public LocalizationTag locTag;

    private Text uiText;
    private TextMesh textMesh; 

    private void Awake () {
        uiText = GetComponent<Text> ();
        textMesh = GetComponent<TextMesh> ();
		//Text Mesh와 uGUI Text에 각각 대응. nGUI를 쓰신다면 nGUI용 컴포넌트를 추가해주세요.
		//만일 둘 중 하나만 있어도(심지어 둘 다 없어도!) 에러 없이 진행되니 걱정 안 해도 됩니다.

        SetText ();
    }

    private void OnEnable () { //씬에서 켜질 때마다 TextData의 textsOnScene으로 들어갑니다.
        TextData.GetInstance.textsOnScene.Add (this);
    }

    private void OnDisable () {//씬에서 꺼질 때마다 TextData의 textsOnScene에서 제거됩니다.
        TextData.GetInstance.textsOnScene.Remove (this);
    }

    private string GetText () {
        string result;
        switch (locTag) {
        case LocalizationTag.localizedString:
            result = TextData.GetInstance.currentLanguagePack.localizedString;
            break;
			//StringBundle과 locTag에 맞춰서 작성해 주세요. 하나씩 이름 지정하기가 힘들다면 StringBundle을 string[]으로 만들어도 무방.(대신 인스펙터에서 보기가 힘들어집니다)
        default:
            result = string.Empty;
            break;
        }
        return result;
    }

    public void SetText () {
        if (uiText) { //uGUI Text가 null이 아니면 텍스트를 변경
            uiText.text = GetText ();
        }
        if (textMesh) { //마찬가지로 Text Mesh가 존재하면 텍스트를 변경
            textMesh.text = GetText ();
        }
    }
}