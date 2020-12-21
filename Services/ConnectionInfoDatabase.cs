using ReactiveUI;
using UlConnect.Models;
using UlConnect.Logic;
using System.Collections.ObjectModel;
namespace UlConnect.Services
{
    public class ConnectionInfoDatabase : ReactiveObject
    {
        public ConnectionInfoDatabase()
        {
            IsDatabaseEmpty = true;
        }
        private bool isDatabaseEmpty;
        private string createConnectionText;
        private ObservableCollection<ConnectionInfoItem> database;
        public string CreateConnectionText {get {return createConnectionText;}set {this.RaiseAndSetIfChanged(ref createConnectionText, value);}}
        public ObservableCollection<ConnectionInfoItem> Database {get {return database;} set{this.RaiseAndSetIfChanged(ref database, value);}}
        public bool IsDatabaseEmpty {get {return isDatabaseEmpty;} set { this.RaiseAndSetIfChanged(ref isDatabaseEmpty, value);}}
        public void ImportDatabaseFromFile(string filename)
        {
            this.Database = new ObservableCollection<ConnectionInfoItem>(FileOperations.LoadDataToDatabase(FileOperations.ReadDataFromFile(filename)));
        }
        public void RemoveItem(int index, string filename)
        {
            if (Database.Count != 0)
            {
                this.Database.RemoveAt(index);
                FileOperations.SynchronizeFileWithDatabase(this.Database, filename); 
                if (Database.Count == 0) IsDatabaseEmpty = true;
            }
        }
        public void AddItem(ConnectionInfoItem connectionInfoItem, string filename)
        {
            this.Database.Add(connectionInfoItem);
            FileOperations.SynchronizeFileWithDatabase(this.Database, filename);
            if (IsDatabaseEmpty == true) IsDatabaseEmpty = false;
        }
        public void SaveInfo(int index, string filename)
        {
            var temp = this.Database[index];
            this.Database[index] = temp;
            FileOperations.SynchronizeFileWithDatabase(this.Database, filename);
        }
    }
}