using UlConnect.Services;
using ReactiveUI;
namespace UlConnect.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {  
        private ViewModelBase content;
        private SettingsDatabase settingsDatabase;
        private LanguageDatabase languageDatabase;
        public MainWindowViewModel()
        {

            languageDatabase = new LanguageDatabase();
            settingsDatabase = new SettingsDatabase();
            //Importing settings   
            settingsDatabase.LoadSettingsFile();    
            //Importing language
            if (!settingsDatabase.Database.ContainsKey("Language"))
            {
                languageDatabase.ImportLanguage("en");
            }
            else
            {
                languageDatabase.ImportLanguage(settingsDatabase.Database["Language"]);
            }
            
            MainMenu = new UlConnectViewModel(this.languageDatabase);
            MainMenu.ConnectionInfoDatabase.CreateConnectionText = languageDatabase.Database["CreateConnectionText"];
            Content = MainMenu;            
        }
        public void OpenSettingsMenu()
        {
            var sv = new SettingsViewModel(languageDatabase, settingsDatabase, MainMenu.ConnectionInfoDatabase);
            sv.ReturnToMenuCommand = ReactiveCommand.Create(() => {Content = MainMenu;});
            Content = sv;
        }
        
        public ViewModelBase Content {get {return content;} set{this.RaiseAndSetIfChanged(ref content, value);}}
        public UlConnectViewModel MainMenu {get;}
    }
}
