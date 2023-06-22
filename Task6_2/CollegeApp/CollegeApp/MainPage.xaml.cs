using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using System.IO;
using CollegeApp.Models;

namespace CollegeApp
{

    public partial class MainPage : ContentPage
    {
        private CollegeDatabase _database;

        public MainPage ()
        {
            InitializeComponent();

            _database = new CollegeDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Database.db3"));
        }

        private async void loginButton_Clicked (object sender, EventArgs e)
        {
            if (myPicker.SelectedIndex == -1)
            {
                _ = DisplayAlert("Внимание пользователь!", "Выберите режим работы.", "OK");
                return;
            } 
            else if (string.IsNullOrEmpty(userNameEntry.Text) || string.IsNullOrEmpty(passwordEntry.Text))
            {
                _ = DisplayAlert("Внимание пользователь!", "Заполните логин и пароль.", "OK");
                return;
            }

            string login = userNameEntry.Text;
            string password = passwordEntry.Text;

            string selectedMode = myPicker.SelectedItem.ToString();

            if (selectedMode == "Студент")
            {
                Student student = await _database.FindStudentByLoginAndPasswordAsync(login, password);

                if (student != null) { await Navigation.PushAsync(new StudentPage(student)); }
                else { await DisplayAlert("Ошибка", "Неверный логин или пароль.", "OK"); }
            }
            else if (selectedMode == "Преподователь" || selectedMode == "Приемная комиссия")
            {
                Teacher teacher = await _database.FindTeacherByLoginAndPasswordAsync(login, password);

                if (teacher != null)
                {
                    if (selectedMode == "Приемная комиссия" && !teacher.IsAdmission)
                    {
                        await DisplayAlert("Ошибка", "У вас нет доступа к приемной комиссии.", "OK");
                        return;
                    }

                    if (selectedMode == "Преподователь")
                    {
                        await Navigation.PushAsync(new TeacherPage(teacher));
                    }
                    else if (selectedMode == "Приемная комиссия")
                    {
                        await Navigation.PushAsync(new ComissionPage());
                    }
                }
                else { await DisplayAlert("Ошибка", "Неверный логин или пароль.", "OK");}
            }


        }
    }
}
