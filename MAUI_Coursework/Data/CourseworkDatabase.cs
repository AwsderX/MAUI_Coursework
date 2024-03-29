﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAUI_Coursework.Models;

namespace MAUI_Coursework.Data
{
    public class CourseworkDatebase
    {
        SQLiteAsyncConnection Database;

        //public event PropertyChangedEventHandler PropertyChanged;
        //private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        public CourseworkDatebase()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await Database.CreateTableAsync<TodoItem>();
            await Database.CreateTableAsync<Users>();
            await Database.CreateTableAsync<Teachers>();
            await Database.CreateTableAsync<Students>();
            await Database.CreateTableAsync<Lessons>();
            await Database.CreateTableAsync<Grades>();
            await Database.CreateTableAsync<Attendance>();
        }
        public async Task<int> SaveUserAsync(Users item)
        {
            await Init();
                await Database.InsertAsync(item);
            return item.ID; 
        }
        public async Task<Users> GetUserAsync(string login)
        {
            await Init();
                return await Database.Table<Users>().Where(i => i.Login == login).FirstOrDefaultAsync();
        }
        public async Task<Users> GetUserAsync(int id)
        {
            await Init();
            return await Database.Table<Users>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }
        //public async Task<T> GetSTAsync<T>(int id) where T : new()
        //{
        //    await Init();
        //    return await Database.Table<T>().Where(i => Convert.ToInt32(typeof(T).GetProperty(nameof(Students.ID_user)).GetValue(i)) == id).FirstOrDefaultAsync();     
        //}
        public async Task<Students> GetStudentAsync(int id)
        {
            await Init();
            return await Database.Table<Students>().Where(i => i.ID_user == id).FirstOrDefaultAsync();
        }
        public async Task<string> GetStudentGroupAsync(int id)
        {
            await Init();
            string result = await Database.ExecuteScalarAsync<string>($"SELECT [Group] FROM [Students] WHERE [ID_user] = '{id}'");
            return result;
        }
        public async Task<List<GradesStud>> GetGradesListAsync(string group, DateTime dt)
        {
            await Init();
            List<GradesStud> result = await Database.QueryAsync<GradesStud>($"SELECT Students.Name, Students.Surname, Students.Patronymic, Grades.Est, Grades.Att, Students.ID_user, Grades.ID FROM Students LEFT JOIN Grades ON Students.ID_user = Grades.ID_student AND Grades.[Date_lesson] ='{dt.Ticks}' WHERE Students.[Group] = '{group}' ORDER BY Students.Name");
            return result;
        }
        public async Task<List<LessonsInfoGrades>> GetLessonsInfoGradesAsync(int id)
        {
            await Init();
            List<LessonsInfoGrades> result = await Database.QueryAsync<LessonsInfoGrades>($"SELECT Lessons.Name, Grades.Date_lesson, Grades.Est, Grades.Att FROM Grades JOIN Lessons ON Grades.ID_lesson = Lessons.ID WHERE Grades.ID_student='{id}'");
            return result;
        }
        public async Task<Teachers> GetTeacherAsync(int id)
        {
            await Init();
            return await Database.Table<Teachers>().Where(i => i.ID_user == id).FirstOrDefaultAsync();
        }
        public async Task<List<Lessons>> GetLessonsAsync(int id)
        {
            await Init();
            return await Database.Table<Lessons>().Where(i => i.ID_teacher == id).ToListAsync();
        }
        public async Task<List<Users>> GetUsersAsync()
        {
            await Init();
                return await Database.Table<Users>().ToListAsync();
        }
        public async Task<int> SaveStudentAsync(Students item)
        {
            await Init();
                return await Database.InsertAsync(item);
        }
        public async Task<int> SaveTeacherAsync(Teachers item)
        {
            await Init();
                return await Database.InsertAsync(item);
        }
        public async Task<List<Students>> GetStudentsAsync()
        {
            await Init();
            return await Database.Table<Students>().ToListAsync();
                // Оповещаем подписчиков об изменении данных
                //NotifyPropertyChanged(nameof(Students));
               // return a;
        }
        public async Task<List<Students>> GetStudentsAsync(string group)
        {
            await Init();
            return await Database.Table<Students>().Where(i => i.Group == group).ToListAsync();
            // Оповещаем подписчиков об изменении данных
            //NotifyPropertyChanged(nameof(Students));
            // return a;
        }
        public async Task<List<Teachers>> GetTeachersAsync()
        {
            await Init();
            return await Database.Table<Teachers>().ToListAsync();
        }
        public async Task<int> UpdateUser(Users user)
        {
            await Init();
            var a = await Database.UpdateAsync(user);
            // Оповещаем подписчиков об изменении данных
            //-NotifyPropertyChanged(nameof(Students));
            return a;
        }
        public async Task<int> UpdateStudent(Students student)
        {
            await Init();
                var a = await Database.UpdateAsync(student);
                // Оповещаем подписчиков об изменении данных
                //-NotifyPropertyChanged(nameof(Students));
                return a;
        }
        public async Task<int> UpdateTeacher(Teachers teacher)
        {
            await Init();
                return await Database.UpdateAsync(teacher);
        }
        public async Task<int> SaveLessonAsync(Lessons item)
        {
            await Init();
            return await Database.InsertAsync(item);
        }
        public async Task<int> DeleteLessonAsync(Lessons item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
        public async Task<List<Lessons>> GetLessonsScheduleAsync(int id, string weekday)
        {
            await Init();
            return await Database.Table<Lessons>().Where(i => i.ID_teacher == id).Where(i => i.Weekday == weekday).OrderBy(i => i.TimeL).ToListAsync();
        }
        public async Task<List<Lessons>> GetLessonsScheduleAsync(string group, string weekday)
        {
            await Init();
            return await Database.Table<Lessons>().Where(i => i.Group == group).Where(i => i.Weekday == weekday).OrderBy(i => i.TimeL).ToListAsync();
        }
        public async Task<int> SaveGradesAsync(Grades item)
        {
            await Init();
            return await Database.InsertAsync(item);
        }
        public async Task<int> UpdateGradesEstAsync(int gradeId, string Est)
        {
            await Init();
            int result = 0;
            var grade = await Database.Table<Grades>().Where(g => g.ID == gradeId).FirstOrDefaultAsync();
            if (grade != null)
            {
                grade.Est = Est;
                result = await Database.UpdateAsync(grade);
            }
            return result;
        }
        public async Task<int> UpdateGradesAttAsync(int gradeId, string Att)
        {
            await Init();
            int result = 0;
            var grade = await Database.Table<Grades>().Where(g => g.ID == gradeId).FirstOrDefaultAsync();
            if (grade != null)
            {
                grade.Att = Att;
                result = await Database.UpdateAsync(grade);
            }
            return result;
        }



        //public async Task<int> GetId(string login)
        //{
        //    await Init();
        //    var result = await Database.QueryAsync($"SELECT Id FROM [User] WHERE [Login] = '{login}'");
        //    return (int)result.First()[0];
        //}

    }
}
