# FolderSynchronizationTask

This is a simple C# console application that synchronizes two folders: **source** and **replica**.  
The synchronization is one-way: after running, the content of the replica folder is an exact copy of the source folder.  
The application performs synchronization periodically and logs all file and folder creation, copying, and removal events to both the console and a log file.

This solution was created as part of a recruitment test task.

## Features

- One-way synchronization from **source** to **replica**
- Periodic synchronization with configurable interval
- Creation, updating, and deletion of files and folders
- Logging to both console and a log file
- Command-line argument validation with custom exceptions
- Unit tests

**Tech used:** .NET 6, xUnit

The application is implemented as a simple C# console program following a modular structure.
The `SyncService` class contains the core synchronization logic: it compares the contents of the source and replica directories, creates any missing folders, copies new or updated files, and removes files or folders not present in the source.
The `Logger` class is responsible for writing log entries both to the console and to a specified log file, ensuring that all synchronization events are recorded.
Command-line arguments are validated at startup, with custom exceptions used to clearly report invalid input.
The program runs in a loop, performing synchronization at regular intervals defined by the user, until it is stopped manually or an exception is thrown.

## How to build and run

### Requirements

- .NET 6 SDK
- Windows, Linux, or macOS

### Command-line arguments

The program requires **4 arguments**:

1. `sourcePath` - Path to the source folder
2. `replicaPath` - Path to the replica folder
3. `intervalSeconds` - Synchronization interval in seconds
4. `logFilePath` - Path to the log file

Example:

```bash
dotnet run "C:\Data\Source" "C:\Data\Replica" 60 "C:\Logs\syncLog.txt"
```

### 1. Clone repository

```bash
git clone https://github.com/P-H-98/FolderSynchronizationTask.git
cd VeeamTest
```

### 2. Build application

```bash
dotnet build
```

### 3. Run application

```bash
dotnet run --project VeeamTest "C:\Source" "C:\Replica" 30 "C:\Logs\sync.log"
```

## Example log output

```
2025-08-12 21:15:55 - Copied/Updated: E:\Data\Origin\TextDocument1.txt
2025-08-12 21:16:55 - Copied/Updated: E:\Data\Origin\TextDocument1.txt
2025-08-12 21:16:55 - Copied/Updated: E:\Data\Origin\SubfolderC\Document2.docx
2025-08-12 21:16:55 - Created directory: E:\Data\Replica\SubfolderD
2025-08-12 21:16:55 - Deleted: E:\Data\Replica\Document2.docx
```

## Running unit tests

Unit tests are located in the **VeeamTest.Tests** project and use **xUnit**.

```bash
cd VeeamTest.Tests
dotnet test
```

### Test Scenarios

Not all tests were automated, table below shows test scenarios that were designed to verify the correctness of the folder synchronization process under different conditions.

| **ID** | **Scenario**                                            | **Preconditions**                                                    | **Steps**                                             | **Expected Result**                                      | **Automated?**                                                                    |
| ------ | ------------------------------------------------------- | -------------------------------------------------------------------- | ----------------------------------------------------- | -------------------------------------------------------- | --------------------------------------------------------------------------------- |
| TC-01  | Synchronization copies new file from source to replica  | Source and replica folders exist; file present only in source        | 1. Place file in source.<br>2. Run synchronization.   | File appears in replica with identical content.          | ✅ (SyncServiceTests.Synchronize_CopiesFilesFromSourceToReplica)                  |
| TC-02  | Synchronization removes extra file in replica           | Source and replica exist; file present only in replica               | 1. Place file in replica.<br>2. Run synchronization.  | File in replica is deleted.                              | ✅ (SyncServiceTests.Synchronize_RemovesExtraFilesInReplica)                      |
| TC-03  | Synchronization fails when source folder does not exist | Source folder path is invalid; replica exists                        | 1. Run synchronization.                               | `SourceDirectoryNotFoundException` is thrown and logged. | ✅ (SyncServiceTests.Synchronize_ThrowsException_WhenSourceDirectoryDoesNotExist) |
| TC-04  | Program rejects too few command-line arguments          | Run program with only one argument                                   | 1. Execute program with invalid args.                 | Error message printed; program exits.                    | ✅ (ProgramTests.ValidateArguments_ThrowsException_WhenInvalidNumberOfArguments)  |
| TC-05  | Program rejects non-integer interval argument           | Provide non-numeric interval value                                   | 1. Execute program with invalid interval.             | Error message printed; program exits.                    | ✅ (ProgramTests.ValidateArguments_ThrowsException_WhenIntervalIsInvalid)         |
| TC-06  | Log file is created successfully if not existing        | Provide valid log file path                                          | 1. Run program.                                       | Log file is created and contains initialization entry.   | ✅ (LoggerTests.Log_CreatesLogFileIfNotExists)                                    |
| TC-07  | Synchronization overwrites changed file in replica      | File with same name exists in both folders but has different content | 1. Modify file in replica.<br>2. Run synchronization. | Replica file content matches source file.                | ⬜ Manual                                                                         |

## Lessons Learned:

While solving this task, I improved my understanding of C# syntax and object-oriented programming patterns by comparing them to my previous experience with C++ and Java.
I learned how to work with file system operations in .NET, including directory traversing, file copying, and deletion.
The task also gave me practical experience with creating custom exceptions and validating user input from command-line arguments.
I reinforced the importance of separating concerns in code - keeping synchronization logic, logging, and argument validation in separate classes makes the application easier to maintain and test.
Finally, writing unit tests showed me how automated testing can quickly reveal problems, especially when working with file operations.

## Author

Created by **Patryk Hotlos** as part of a recruitment process.
FolderSynchronizationTask
