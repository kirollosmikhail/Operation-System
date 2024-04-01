using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Presentation_Layer.Helpers
{
    public static class DocumentSettings
    {

        //Upload
        public static string UploadFile(IFormFile file, string FolderName)
        {
            // 1. Get Located Folder Path
            // K:\devcreed\DevCreed project\ROUTE STUDY 3-3-2024\Mvc Project\Operation System\Presentation Layer\wwwroot\Files\Images\
            // Directory.GetCurrentDirectory()+"\\wwwroot\\Files\\"+FolderName;
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);
            // 2. Get FileName and make it unique
            string FileName = $"{Guid.NewGuid()} {file.FileName}";

            // 3. get file path[folder path+filename]
            string FilePath = Path.Combine(FolderPath, FileName);

            // 4. save file as streams
            using var Fs = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(Fs);

            // 5. return file name
            return FileName;
        }

        //Delete
        public static void DeleteFile(string FileName,string FolderName)
        {
            // 1. Get File Path
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\Files",FolderName,FileName);

            // 2. Check if file exists or not if exists remove it
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            
        }





    }
}
