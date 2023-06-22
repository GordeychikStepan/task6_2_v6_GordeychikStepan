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
    public partial class StudentsPage : ContentPage
    {
        private string selectedGroupName;
        private CollegeDatabase database;
        public ObservableCollection<Student> Students { get; set; }

        public StudentsPage(string selectedGroupName)
        {
            InitializeComponent();

            Title = $"Студенты группы {selectedGroupName}";
            this.selectedGroupName = selectedGroupName;
            database = new CollegeDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Database.db3"));

            Students = new ObservableCollection<Student>();
            StudentListView.ItemsSource = Students;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadStudents();
        }
        private async Task LoadStudents()
        {
            var students = await database.GetStudentsByGroupNameAsync(selectedGroupName);
            Students.Clear();
            foreach (var student in students)
            {
                Students.Add(student);
            }
        }
        async void Handle_StudentSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            var selectedStudent = (Student)e.SelectedItem;
            await Navigation.PushModalAsync(new StudentPage(selectedStudent));
            ((ListView)sender).SelectedItem = null;
        }
    }
}
