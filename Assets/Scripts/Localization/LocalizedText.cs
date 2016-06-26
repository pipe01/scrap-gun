using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour {

    public string textKey;

	// Use this for initialization
	void Awake () {

        GetComponent<Text>().text = SmartLocalization.LanguageManager.Instance.GetTextValue(textKey);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
