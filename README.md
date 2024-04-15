### Overview
---
This exercise uses the Azure Blob storage client library to show you how to perform the following actions on Azure Blob storage in a console app:

- Create a container
- Upload blobs to a container
- List the blobs in a container
- Download blobs
- Delete a container

### Key Aspects
---
- To be defined 

### Environment
---
Microsoft Azure Portal
- Valid Subscription
- Azure Cloud Shell

Integrated Development Environment (IDE)
- Visual Studio Community 2022 or Visual Studio Code (with C# extension)

Framework 
- .NET 6 or greater

### Actions
---
Prepare the environment

- Create the project folder

- Open the Visual Studio Code using the project folder as the base

- Open Azure Cloud Shell (https://portal.azure.com/#cloudshell/)

- Create a resource group for the resources needed for this exercise
```
az account list-locations --output table
DEFAULT_REGION="westeurope"
DEFAULT_RESOURCEGROUP="MyResourceGroup"
az group create --location ${DEFAULT_REGION} --name ${DEFAULT_RESOURCEGROUP}
```

- Create a storage account
```
DEFAULT_STORAGEACCOUNT="mystorage20240415175100"
az storage account create \
--resource-group ${DEFAULT_RESOURCEGROUP} \
--location ${DEFAULT_REGION} \
--name ${DEFAULT_STORAGEACCOUNT} \
--sku Standard_LRS
```

- Get credentials for the storage account (Connection string value under key1)
```
XYZ
```

Implement the code in a .NET project
- Create a new project
```
dotnet new console -n az204-blob
cd az204-blob
dotnet build
```

- Build and Run
```
cd az204-blob
dotnet build
dotnet run
```

- Create a new repository to store the blobs
```
md data
```

- Add the .NET package library needed to manage blob objects
```
dotnet add package Azure.Storage.Blobs
```

- Implement the code on the Program.cs
```
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

//

const string STORAGE_ACCOUNT_CONNECTION_STRING = "STORAGE-ACCOUNT-KEY-ACCESS";

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
```

- Build and Run
```
dotnet build
dotnet run
```

### Media
---
![image](https://github.com/ViCunha/Lab-Azure-AzureStorageAccount-BlobStorage-ClientLibraryConsoleApp/assets/65992033/11d59490-cf77-4cd1-8bd7-a6faf3f2473d)
---
![image](https://github.com/ViCunha/Lab-Azure-AzureStorageAccount-BlobStorage-ClientLibraryConsoleApp/assets/65992033/8536725c-3598-49e2-b791-cccedfd3849f)

### References
---
- https://learn.microsoft.com/en-us/training/modules/work-azure-blob-storage/4-develop-blob-storage-dotnet
