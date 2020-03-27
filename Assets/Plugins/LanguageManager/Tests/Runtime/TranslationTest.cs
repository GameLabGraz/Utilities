using UnityEngine;
using NUnit.Framework;
using GEAR.Localization;

public class TranslationTest
{
    private Translation _translation;

    [SetUp]
    public void Setup()
    {
        _translation = new Translation("GER");
        _translation.AddTranslation(SystemLanguage.English, "German");
        _translation.AddTranslation(SystemLanguage.German, "Deutsch");
    }

    [TearDown]
    public void Teardown()
    {
        _translation = null;
    }

    [TestCase(SystemLanguage.English, "German")]
    [TestCase(SystemLanguage.German, "Deutsch")]
    [TestCase(SystemLanguage.Unknown, "German")]
    public void TestGetValue(SystemLanguage language, string expected)
    {
        var result = _translation.GetValue(language);
        Assert.AreEqual(expected, result);
    }
}
