using UnityEngine;
using NUnit.Framework;
using GEAR.Localization;

public class LanguageManagerTest
{
    private LanguageManager _languageManager;
    private static TextAsset ValidMlg => Resources.Load<TextAsset>("ValidMLG");

    [SetUp]
    public void Setup()
    {
        _languageManager = new GameObject("LanguageManager").AddComponent<LanguageManager>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(_languageManager.gameObject);
    }

    [TestCase("ValidMLG", true)]
    [TestCase("InvalidMLG", false)]
    public void TestLoadingMlgFile(string mlgFile, bool expected)
    { 
        var result =_languageManager.LoadMlgFile(Resources.Load<TextAsset>(mlgFile));
        Assert.AreEqual(expected, result, $"Wrong result for {mlgFile}");
    }

    [Test]
    public void TestOnLanguageChange()
    {
        var result = SystemLanguage.Unknown;
        var triggered = false;

        _languageManager.OnLanguageChanged.AddListener((var) => 
        { 
            triggered = true;
            result = var;
        });

        _languageManager.CurrentLanguage = SystemLanguage.English;

        Assert.True(triggered, "Does not trigger OnLanguageChanged event.");
        Assert.AreEqual(SystemLanguage.English, result, "Unable to set language.");
    }

    [TestCase("English", SystemLanguage.English)]
    [TestCase("Invalid", SystemLanguage.Unknown)]
    public void TestSetLanguage(string languageKey, SystemLanguage expected)
    {
        _languageManager.SetLanguage(languageKey);
        Assert.AreEqual(expected, _languageManager.CurrentLanguage);
    }

    [TestCase("GER", SystemLanguage.English, "German")]
    [TestCase("GER", SystemLanguage.German, "Deutsch")]
    [TestCase("EscapedMicroSign", SystemLanguage.English, "µ", Description = "Tests whether escaped characters are converted.")]
    public void TestLocalizedText(string key, SystemLanguage language, string expected)
    {
        _languageManager.LoadMlgFile(ValidMlg);
        _languageManager.CurrentLanguage = language;

        var localizedText = _languageManager.GetString(key);
        Assert.AreEqual(expected, localizedText);
    }

}
