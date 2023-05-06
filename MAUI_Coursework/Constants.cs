using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Coursework
{
    public static class Constants
    {
        public const string DatabaseFilename = "CourseWorkSQLite.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache |

            SQLite.SQLiteOpenFlags.NoMutex;
#pragma warning disable CS0117
        public static string DatabasePath =>
            //Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
            //Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath, DatabaseFilename);
            Path.Combine(Path.GetFullPath("//sdcard//Documents//"), DatabaseFilename);
            //Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath, DatabaseFilename);
            //Path.Combine(Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath, DatabaseFilename);
#pragma warning restore CS0117
    }
}
