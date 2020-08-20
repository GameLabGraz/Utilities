using System.Collections;
using System.Collections.Generic;
using GEAR.Localization;
using GEAR.Localization.DropDown;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

public class LocalizedDropDownTMPTest
{
    private LanguageManager _languageManager;
    private TMP_Dropdown _dropDown;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        yield return new EnterPlayMode();

        _languageManager = new GameObject("LanguageManager").AddComponent<LanguageManager>();
        _languageManager.LoadMlgFile(Resources.Load<TextAsset>("ValidMLG"));

        _dropDown = new GameObject("DropDown").AddComponent<TMP_Dropdown>();

        // Add Language Keys
        _dropDown.AddOptions(new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData("GER"),
            new TMP_Dropdown.OptionData("ENG"),
        });
        _dropDown.gameObject.AddComponent<LocalizedDropDownTMP>();
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        Object.Destroy(_languageManager.gameObject);
        Object.Destroy(_dropDown.gameObject);

        yield return new ExitPlayMode();
    }

    [UnityTest]
    [TestCase(SystemLanguage.English, new[] { "German", "English" }, ExpectedResult = null)]
    [TestCase(SystemLanguage.German, new[] { "Deutsch", "Englisch" }, ExpectedResult = null)]
    public IEnumerator TestLocalizedDropDown(SystemLanguage language, string[] expectedOptions)
    {
        _languageManager.CurrentLanguage = language;

        yield return new WaitForFixedUpdate();

        Assert.AreEqual(expectedOptions.Length, _dropDown.options.Count);

        for (var i = 0; i < _dropDown.options.Count; i++)
        {
            Assert.AreEqual(expectedOptions[i], _dropDown.options[i].text);
        }
    }
}
