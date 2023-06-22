using CollegeApp.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CollegeApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TeacherPage : ContentPage
    {
        private Teacher teacher;
        private CollegeDatabase database;
        public ObservableCollection<string> GroupNames { get; set; }

        public TeacherPage(Teacher teacher)
        {
            InitializeComponent();

            Title = $"Добро пожаловать, {teacher.FullName}!";
            this.teacher = teacher;
            database = new CollegeDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Database.db3"));

            GroupNames = new ObservableCollection<string>();
            GroupListView.ItemsSource = GroupNames;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadGroupNames();
        }

        private async Task LoadGroupNames()
        {
            var groupNames = await database.GetAllGroupNamesAsync();
            GroupNames.Clear();
            foreach (var groupName in groupNames)
            {
                GroupNames.Add(groupName);
            }
        }

        async void Handle_GroupSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var selectedGroupName = (string)e.SelectedItem;
            await Navigation.PushAsync(new StudentsPage(selectedGroupName));
            ((ListView)sender).SelectedItem = null;
        }
    }
}
