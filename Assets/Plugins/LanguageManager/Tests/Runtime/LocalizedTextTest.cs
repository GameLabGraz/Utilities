using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using NUnit.Framework;
using GEAR.Localization;

public class LocalizedTextTest 
{
    private LanguageManager _languageManager;
    private LocalizedText _localizedText;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        yield return new EnterPlayMode();

        _languageManager = new GameObject("LanguageManager").AddComponent<LanguageManager>();
        _languageManager.LoadMlgFile(Resources.Load<TextAsset>("ValidMLG"));
        
        _localizedText = new GameObject("LocalizedText").AddComponent<LocalizedText>();
        _localizedText.Key = "GER";
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        Object.Destroy(_languageManager);
        Object.Destroy(_localizedText);

        yield return new ExitPlayMode();
    }

    [UnityTest]
    [TestCase(SystemLanguage.English, "German", ExpectedResult = null)]
    [TestCase(SystemLanguage.German, "Deutsch", ExpectedResult = null)]
    public IEnumerator TestLocalizedText(SystemLanguage language, string expected)
    {
        var textComponent = _localizedText.GetComponent<Text>();
       
        _languageManager.CurrentLanguage = language;
        
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(expected, textComponent.text);
    }

}
