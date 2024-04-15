
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

//

const string STORAGE_ACCOUNT_CONNECTION_STRING = "";

//
Console.WriteLine("Azure Storage Account # Blob Storage: exercise");

//

Console.WriteLine("Beginning");

ProcessAsync(STORAGE_ACCOUNT_CONNECTION_STRING).GetAwaiter().GetResult();

Console.WriteLine("End");

//
static async Task ProcessAsync(string storageAccountConnection)
{
    var storageAccountServiceClient = new BlobServiceClient(storageAccountConnection);
    Console.WriteLine("Azure Storage Account # Blob Storage: Connected!");
    Console.ReadLine();

    //
    var blobContainerClientName =  ("ViCunha" + Guid.NewGuid()).ToString().ToLower().Replace("-","");
    BlobContainerClient blobContainerClient = await storageAccountServiceClient.CreateBlobContainerAsync(blobContainerClientName);
    
    Console.WriteLine("Azure Storage Account # Blob Storage: Blob Storage Container created!");
    Console.ReadLine();

    //
    var blobLocalDirectory ="./data/";
    var blobLocalFilename = ("ViCunha" + Guid.NewGuid() + ".txt").ToString().ToLower().Replace("-","");
    var blobFullPath = Path.Combine(blobLocalDirectory, blobLocalFilename);

    await File.AppendAllTextAsync(blobFullPath, "Hello Ana Paula!");
    Console.WriteLine("Azure Storage Account # Blob Storage: File created!");
    Console.ReadLine();

    BlobClient blobClient = blobContainerClient.GetBlobClient(blobFullPath);
    Console.WriteLine("Azure Storage Account # Blob Storage: Blob created!");
    Console.ReadLine();

    //
    using (FileStream fileStream = File.OpenRead(blobFullPath))
    {
        await blobClient.UploadAsync(fileStream);
        fileStream.Close();
    }
    Console.WriteLine("Azure Storage Account # Blob Storage: Blob uploaded!");
    Console.ReadLine();
    
    //
    Console.WriteLine("Azure Storage Account # Blob Storage: Listing blob Beginning");
    
    await foreach(var blob in blobContainerClient.GetBlobsAsync())
    {
        Console.WriteLine("blob: {0}", blob.Name);
    }

    Console.WriteLine("Azure Storage Account # Blob Storage: Listing blob End");
    Console.ReadLine();

    //
    var blobFullPathDownload = blobFullPath.Replace(".txt", "DOWNLOADED.txt");
    BlobDownloadInfo blobDownloadInfo= await blobClient.DownloadAsync();

    using  (FileStream fileStream = File.OpenWrite(blobFullPathDownload))
    {
        await blobDownloadInfo.Content.CopyToAsync(fileStream);
    }    
    
    Console.WriteLine("Azure Storage Account # Blob Storage: Blob Downloaded");
    Console.ReadLine();

    //
    File.Delete(blobFullPathDownload);
    File.Delete(blobFullPath);

    await blobClient.DeleteAsync();
    await blobContainerClient.DeleteAsync();

    Console.WriteLine("Azure Storage Account # Blob Storage: Clean-Up Done!");
    Console.ReadLine();

}