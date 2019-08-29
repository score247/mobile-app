namespace LiveScore.iOS
{
    using System;
    using Akavache.Sqlite3;

    [Preserve]
    public static class LinkerPreserve
    {
        static LinkerPreserve()
        {
            var persistentName = typeof(SQLitePersistentBlobCache).FullName;
            var encryptedName = typeof(SQLiteEncryptedBlobCache).FullName;
        }
    }

    public class PreserveAttribute : Attribute
    {
    }
}