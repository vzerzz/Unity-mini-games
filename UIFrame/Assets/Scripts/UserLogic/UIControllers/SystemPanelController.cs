using UnityEditor;
using UnityEngine;
using UIFrame;
using Localization;

public class SystemPanelController : UIControllerBase
    {
    protected override void ControllerStart()
    {
        base.ControllerStart();
        BindEvent();
    }

    private void BindEvent()
    {
        crtModule.FindCurrentModuleWidget("RebackButton_F").AddOnClickListener(() =>
        {
            UIManager.Instance.PopUI();
        });
        crtModule.FindCurrentModuleWidget("LanguageEnglishButton_F").AddOnClickListener(() =>
        {
            LocalizationManager.Instance.ChangeLanguage(SupportLanguage.English);
        });
        crtModule.FindCurrentModuleWidget("LanguageChineseButton_F").AddOnClickListener(() =>
        {
            LocalizationManager.Instance.ChangeLanguage(SupportLanguage.SimpleChinese);
        });
    }
}
