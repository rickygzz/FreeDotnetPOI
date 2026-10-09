
namespace NPOI.Util
{
    using System;
    using System.IO;

    public class TempFile
    {

        private static string dir;
        /**
         * Creates a temporary file.  Files are collected into one directory and by default are
         * deleted on exit from the VM.  Files can be kept by defining the system property
         * <c>poi.keep.tmp.files</c>.
         * 
         * Dont forget to close all files or it might not be possible to delete them.
         */
        public static FileInfo CreateTempFile(String prefix, String suffix)
        {
            if (string.IsNullOrWhiteSpace(dir))
            {
                string tempDir = Path.Combine(Path.GetTempPath(), "poifiles");
                dir = Directory.CreateDirectory(tempDir).FullName;
            }

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // Generate a unique new filename 
            string file = Path.Combine(dir, prefix + Guid.NewGuid().ToString() + suffix);
            while (File.Exists(file))
            {
                file = Path.Combine(dir, prefix + Guid.NewGuid().ToString() + suffix);
            }

            using (FileStream newFile = new FileStream(file, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite)) { };

            return new FileInfo(file);
        }

        public static string GetTempFilePath(String prefix, String suffix)
        {
            if (string.IsNullOrWhiteSpace(dir))
            {
                string tempDir = Path.Combine(Path.GetTempPath(), "poifiles");
                dir = Directory.CreateDirectory(tempDir).FullName;
            }

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // Use a GUID like CreateTempFile. A Random seeded from the current millisecond gives
            // the same names to processes that call this at the same time.
            string path = Path.Combine(dir, prefix + Guid.NewGuid().ToString() + suffix);
            while(File.Exists(path))
            {
                path = Path.Combine(dir, prefix + Guid.NewGuid().ToString() + suffix);
            }
            return path;
        }
    }
}
