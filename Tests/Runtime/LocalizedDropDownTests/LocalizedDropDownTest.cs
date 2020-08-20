using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using NUnit.Framework;
using GEAR.Localization;
using GEAR.Localization.DropDown;

public class LocalizedDropDownTest
{
    private LanguageManager _languageManager;
    private Dropdown _dropDown;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        yield return new EnterPlayMode();

        _languageManager = new GameObject("LanguageManager").AddComponent<LanguageManager>();
        _languageManager.LoadMlgFile(Resources.Load<TextAsset>("ValidMLG"));

        _dropDown = new GameObject("DropDown").AddComponent<Dropdown>();

        // Add Language Keys
        _dropDown.AddOptions(new List<Dropdown.OptionData>()
        {
            new Dropdown.OptionData("GER"),
            new Dropdown.OptionData("ENG"),
        });
        _dropDown.gameObject.AddComponent<LocalizedDropDown>();
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        Object.Destroy(_languageManager.gameObject);
        Object.Destroy(_dropDown.gameObject);

        yield return new ExitPlayMode();
    }

    [UnityTest]
    [TestCase(SystemLanguage.English, new []{"German", "English"}, ExpectedResult = null)]
    [TestCase(SystemLanguage.German, new []{"Deutsch", "Englisch"}, ExpectedResult = null)]
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
