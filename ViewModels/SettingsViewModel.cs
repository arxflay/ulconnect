using System.Reactive;
using ReactiveUI;
using System.Collections.Generic;
using UlConnect.Logic;
using UlConnect.Services;
namespace UlConnect.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private List<string> fileNames;
        private int selectedIndex;
        private string language;
        private string returnToMenu;
        private string save;
        public string Language {get {return language;} set {this.RaiseAndSetIfChanged(ref language, value);}}
        public string ReturnToMenu{get {return returnToMenu;} set{this.RaiseAndSetIfChanged(ref returnToMenu, value);}}
        public string Save{get {return save;} set{this.RaiseAndSetIfChanged(ref save, value);}}
       
        public SettingsViewModel(LanguageDatabase languageDatabase, SettingsDatabase settingsDatabase, ConnectionInfoDatabase connectionInfoDatabase)
        {
            UpdateVisual(languageDatabase);
            FileNames = FileOperations.GetFileNames("lang");
            if (fileNames.Contains(settingsDatabase.Database["Language"]))
            {
                selectedIndex = FileNames.IndexOf(settingsDatabase.Database["Language"]);
            }
            SaveSettingsCommand = ReactiveCommand.Create(() => {
                settingsDatabase.ChangeParameter("Language", FileNames[selectedIndex]);
                languageDatabase.ImportLanguage(FileNames[selectedIndex]);
                UpdateVisual(languageDatabase);
                languageDatabase.SetLanguageForAllElementsInDatabase(connectionInfoDatabase.Database);
                });
        }
        public void UpdateVisual(LanguageDatabase languageDatabase)
        {
            Language = languageDatabase.Database["Language"];
            ReturnToMenu = languageDatabase.Database["ReturnToMenu"];
            Save = languageDatabase.Database["Save"];
        }
        public int SelectedIndex {get {return selectedIndex;} set {this.RaiseAndSetIfChanged(ref selectedIndex, value);}}
        public ReactiveCommand<Unit, Unit> ReturnToMenuCommand {get; set;}
        public ReactiveCommand<Unit, Unit> SaveSettingsCommand {get; set;}
        public List<string> FileNames {get {return fileNames;} set {this.RaiseAndSetIfChanged(ref fileNames, value);}}
    }
}