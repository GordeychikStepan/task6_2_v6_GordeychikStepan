using Android.App;
using CollegeApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CollegeApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ComissionPage : CarouselPage
    {
        private readonly CollegeDatabase _database;
        private List<Student> _students;
        private List<Teacher> _teachers;
        private Student _selectedStudent;
        private Teacher _selectedTeacher;

        public ComissionPage()
        {
            InitializeComponent();

            _database = new CollegeDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Database.db3"));

            LoadStudents();
            LoadTeachers();
        }

        private async void LoadStudents()
        {
            _students = await _database.GetStudentsAsync();
            studentsListView.ItemsSource = _students;
            deleteStudentsListView.ItemsSource = _students;

            var groups = await _database.GetGroupsAsync();
            groupPicker.ItemsSource = groups.Select(g => g.Name).ToList();
        }
        private async void LoadTeachers()
        {
            _teachers = await _database.GetTeachersAsync();
            teachersListView.ItemsSource = _teachers;
        }

        private async void AddStudent_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(fullNameEntry.Text) || string.IsNullOrEmpty(courseEntry.Text) || string.IsNullOrEmpty(loginEntry.Text) || string.IsNullOrEmpty(passwordEntry.Text))
            {
                _ = DisplayAlert("Внимание пользователь!", "Заполните все поля для ввода.", "OK");
                return;
            }
            else if (groupPicker.SelectedIndex == -1)
            {
                _ = DisplayAlert("Внимание пользователь!", "Выберите группу.", "OK");
                return;
            }

            var selectedGroupName = groupPicker.SelectedItem as string;
            var selectedGroup = await _database.GetGroupByNameAsync(selectedGroupName);

            var student = new Student
            {
                FullName = fullNameEntry.Text,
                DateOfBirth = dateOfBirthPicker.Date,
                IsBudget = isBudgetSwitch.IsToggled,
                Course = int.Parse(courseEntry.Text),
                GroupId = selectedGroup.GroupId,
                Photo = null,
                Login = loginEntry.Text,
                Password = passwordEntry.Text
            };

            await _database.SaveStudentAsync(student);

            fullNameEntry.Text = string.Empty;
            dateOfBirthPicker.Date = DateTime.Now;
            isBudgetSwitch.IsToggled = false;
            courseEntry.Text = string.Empty;
            loginEntry.Text = string.Empty;
            passwordEntry.Text = string.Empty;

            LoadStudents();
        }

        private void StudentsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _selectedStudent = e.SelectedItem as Student;

            fullNameEditEntry.Text = _selectedStudent?.FullName;
            dateOfBirthEditPicker.Date = _selectedStudent?.DateOfBirth ?? DateTime.Now;
            isBudgetEditSwitch.IsToggled = _selectedStudent?.IsBudget ?? false;
            courseEditEntry.Text = _selectedStudent?.Course.ToString();
            groupIdEditEntry.Text = _selectedStudent?.GroupId.ToString();
            loginEditEntry.Text = _selectedStudent?.Login;
            passwordEditEntry.Text = _selectedStudent?.Password;
        }

        private async void SaveStudent_Clicked(object sender, EventArgs e)
        {
            if (_selectedStudent != null)
            {
                _selectedStudent.FullName = fullNameEditEntry.Text;
                _selectedStudent.DateOfBirth = dateOfBirthEditPicker.Date;
                _selectedStudent.IsBudget = isBudgetEditSwitch.IsToggled;
                _selectedStudent.Course = int.Parse(courseEditEntry.Text);
                _selectedStudent.GroupId = int.Parse(groupIdEditEntry.Text);
                _selectedStudent.Login = loginEditEntry.Text;
                _selectedStudent.Password = passwordEditEntry.Text;

                await _database.SaveStudentAsync(_selectedStudent);

                fullNameEditEntry.Text = string.Empty;
                dateOfBirthEditPicker.Date = DateTime.Now;
                isBudgetEditSwitch.IsToggled = false;
                courseEditEntry.Text = string.Empty;
                groupIdEditEntry.Text = string.Empty;
                loginEditEntry.Text = string.Empty;
                passwordEditEntry.Text = string.Empty;

                LoadStudents();
            }
        }

        private async void DeleteStudent_Clicked(object sender, EventArgs e)
        {
            if (_selectedStudent != null)
            {
                await _database.DeleteStudentAsync(_selectedStudent);

                fullNameEditEntry.Text = string.Empty;
                dateOfBirthEditPicker.Date = DateTime.Now;
                isBudgetEditSwitch.IsToggled = false;
                courseEditEntry.Text = string.Empty;
                groupIdEditEntry.Text = string.Empty;
                loginEditEntry.Text = string.Empty;
                passwordEditEntry.Text = string.Empty;

                LoadStudents();
            }
        }

        private void DeleteStudentsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _selectedStudent = e.SelectedItem as Student;
        }

        private void EditFilterEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = editFilterEntry.Text.ToLowerInvariant();
            var filteredStudents = _students.Where(s => s.FullName.ToLowerInvariant().Contains(filter)).ToList();
            studentsListView.ItemsSource = filteredStudents;
        }

        private void DeleteFilterEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = deleteFilterEntry.Text.ToLowerInvariant();
            var filteredStudents = _students.Where(s => s.FullName.ToLowerInvariant().Contains(filter)).ToList();
            deleteStudentsListView.ItemsSource = filteredStudents;
        }

        private async void SaveTeacher_Clicked(object sender, EventArgs e)
        {
            if (_selectedTeacher != null)
            {
                _selectedTeacher.FullName = fullNameEditTeacherEntry_2.Text;
                _selectedTeacher.Workload = int.Parse(workloadEditEntry.Text);
                _selectedTeacher.Login = loginEditEntry_2.Text;
                _selectedTeacher.Password = passwordEditEntry_2.Text;
                _selectedTeacher.IsAdmission = isAdmissionEditSwitch.IsToggled;

                await _database.SaveTeacherAsync(_selectedTeacher);

                fullNameEditTeacherEntry_2.Text = string.Empty;
                workloadEditEntry.Text = string.Empty;
                loginEditEntry_2.Text = string.Empty;
                passwordEditEntry_2.Text = string.Empty;
                isAdmissionEditSwitch.IsToggled = false;

                LoadTeachers();
            }
        }

        private void TeachersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _selectedTeacher = e.SelectedItem as Teacher;

            fullNameEditTeacherEntry_2.Text = _selectedTeacher?.FullName;
            workloadEditEntry.Text = _selectedTeacher?.Workload.ToString();
            loginEditEntry_2.Text = _selectedTeacher?.Login;
            passwordEditEntry_2.Text = _selectedTeacher?.Password;
            isAdmissionEditSwitch.IsToggled = _selectedTeacher?.IsAdmission ?? false;
        }

        private void EditTeacherFilterEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = editTeacherFilterEntry.Text.ToLowerInvariant();
            var filteredTeachers = _teachers.Where(t => t.FullName.ToLowerInvariant().Contains(filter)).ToList();
            teachersListView.ItemsSource = filteredTeachers;
        }

    }
}