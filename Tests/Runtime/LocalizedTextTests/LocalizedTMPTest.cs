using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using TMPro;
using GEAR.Localization;
using GEAR.Localization.Text;

public class LocalizedTMPTest
{
    private LanguageManager _languageManager;
    private LocalizedTMP _localizedTMP;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        yield return new EnterPlayMode();

        _languageManager = new GameObject("LanguageManager").AddComponent<LanguageManager>();
        _languageManager.LoadMlgFile(Resources.Load<TextAsset>("ValidMLG"));

        var localizedTextObj = new GameObject("LocalizedText");
        localizedTextObj.AddComponent<TextMeshPro>();

        _localizedTMP= localizedTextObj.AddComponent<LocalizedTMP>();
        _localizedTMP.Key = "GER";
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        Object.Destroy(_languageManager.gameObject);
        Object.Destroy(_localizedTMP.gameObject);

        yield return new ExitPlayMode();
    }

    [UnityTest]
    [TestCase(SystemLanguage.English, "German", ExpectedResult = null)]
    [TestCase(SystemLanguage.German, "Deutsch", ExpectedResult = null)]
    public IEnumerator TestLocalizedText(SystemLanguage language, string expected)
    {
        var tmpText = _localizedTMP.GetComponent<TMP_Text>();

        _languageManager.CurrentLanguage = language;

        yield return new WaitForFixedUpdate();

        Assert.AreEqual(expected, tmpText.text);
    }

}
