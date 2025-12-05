using System.Collections.ObjectModel;
using BitFlare.Model;

namespace BitFlare.ViewModel;

public class MainWindowViewModel
{
    internal class MainWindowModel
    {
        public ObservableCollection<Item> Items { get; set;}
        
        public MainWindowModel()
        {
            
        }
    }
}