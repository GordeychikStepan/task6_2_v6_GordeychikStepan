using CollegeApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeApp
{
    public class CollegeDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public CollegeDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Student>().Wait();
            _database.CreateTableAsync<Group>().Wait();
            _database.CreateTableAsync<Specialty>().Wait();
            _database.CreateTableAsync<Teacher>().Wait();
            _database.CreateTableAsync<StudentSpecialty>().Wait();
            _database.CreateTableAsync<TeacherSpecialty>().Wait();

            if (_database.Table<Student>().CountAsync().Result == 0)
            {
                var specialtyes = new List<Specialty>
                {
                    new Specialty { Name = "Информационные системы и программирование" },
                    new Specialty { Name = "Технология аналитического контроля химических соединений" },
                };
                _database.InsertAllAsync(specialtyes).Wait();

                var groups = new List<Group>
                {
                    new Group { Name = "ПР-21", SpecialtyId = 1 },
                    new Group { Name = "ХТ-11", SpecialtyId = 1 },
                };
                _database.InsertAllAsync(groups).Wait();

                var teachers = new List<Teacher>
                {
                    new Teacher { FullName = "Сидоров Сидор Сидорович", Workload = 100, Login = "Sidorov", IsAdmission = false, Password = "123456" },
                    new Teacher { FullName = "Козлова Анна Ивановна", Workload = 200, Login = "Kozlova", IsAdmission = true, Password = "654321" },
                };
                _database.InsertAllAsync(teachers).Wait();

                var students = new List<Student>
                {
                    new Student { FullName = "Гордейчик Степан Алексеевич", DateOfBirth = new DateTime(2005, 3, 31), IsBudget = true, Course = 2, GroupId = 1, Login = "Stepan", Password = "123456789", Photo = null },
                    new Student { FullName = "Иванов Иван Иванович", DateOfBirth = new DateTime(2004, 12, 1), IsBudget = true, Course = 1, GroupId = 2, Login = "Ivan", Password = "4321", Photo = "Ivan.JPG" },
                };
                _database.InsertAllAsync(students).Wait();

                var studentSpecialties = new List<StudentSpecialty>
                {
                    new StudentSpecialty { StudentId = 1, SpecialtyId = 1 },
                    new StudentSpecialty { StudentId = 2, SpecialtyId = 1 },
                    new StudentSpecialty { StudentId = 2, SpecialtyId = 2 },
                };
                _database.InsertAllAsync(studentSpecialties).Wait();

                var teacherSpecialties = new List<TeacherSpecialty>
                {
                    new TeacherSpecialty { TeacherId = 1, SpecialtyId = 1 },
                    new TeacherSpecialty { TeacherId = 2, SpecialtyId = 1 },
                };
                _database.InsertAllAsync(teacherSpecialties).Wait();
            }
        }

        // методы для поиска и группировки данных
        public async Task<Student> FindStudentByLoginAndPasswordAsync(string login, string password)
        {
            return await _database.Table<Student>()
                .Where(s => s.Login == login && s.Password == password)
                .FirstOrDefaultAsync();
        }
        public async Task<Teacher> FindTeacherByLoginAndPasswordAsync(string login, string password)
        {
            return await _database.Table<Teacher>()
                .Where(t => t.Login == login && t.Password == password)
                .FirstOrDefaultAsync();
        }
        public Task<List<Specialty>> GetStudentSpecialtiesAsync(int studentId)
        {
            return _database.QueryAsync<Specialty>(
                "SELECT Specialty.* FROM Specialty " +
                "JOIN StudentSpecialty ON Specialty.SpecialtyId = StudentSpecialty.SpecialtyId " +
                "WHERE StudentSpecialty.StudentId = ?", studentId);
        }
        public Task<string> GetGroupNameAsync(int groupId)
        {
            return _database.ExecuteScalarAsync<string>(
                "SELECT Name FROM [Group] WHERE GroupId = ?", groupId);
        }
        public async Task<List<string>> GetAllGroupNamesAsync()
        {
            var groupNames = await _database.Table<Group>().ToListAsync();
            return groupNames.Select(group => group.Name).ToList();
        }
        public async Task<List<Student>> GetStudentsByGroupNameAsync(string groupName)
        {
            var group = await _database.Table<Group>().FirstOrDefaultAsync(g => g.Name == groupName);
            if (group != null)
            {
                var students = await _database.Table<Student>().Where(s => s.GroupId == group.GroupId).ToListAsync();
                return students;
            }
            return new List<Student>();
        }
        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _database.Table<Student>().ToListAsync();
        }
        public async Task SaveStudentAsync(Student student)
        {
            if (student.StudentId != 0)
            {
                await _database.UpdateAsync(student);
            }
            else
            {
                await _database.InsertAsync(student);
            }
        }
        public async Task DeleteStudentAsync(Student student)
        {
            if (student != null)
            {
                await _database.DeleteAsync(student);
            }
        }

        public Task<int> SaveTeacherAsync(Teacher teacher)
        {
            if (teacher.TeacherId != 0)
            {
                // Update an existing teacher
                return _database.UpdateAsync(teacher);
            }
            else
            {
                // Insert a new teacher
                return _database.InsertAsync(teacher);
            }
        }

        public Task<List<Teacher>> GetTeachersAsync()
        {
            return _database.Table<Teacher>().ToListAsync();
        }

        public async Task<List<Group>> GetGroupsAsync()
        {
            return await _database.Table<Group>().ToListAsync();
        }

        public async Task<Group> GetGroupByNameAsync(string groupName)
        {
            return await _database.Table<Group>().FirstOrDefaultAsync(g => g.Name == groupName);
        }


    }
}
