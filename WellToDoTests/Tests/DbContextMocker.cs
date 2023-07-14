using Microsoft.EntityFrameworkCore;
using WellToDo.Models;
using TaskModel = WellToDo.Models.Task;

namespace WellToDoTests.Mocks;

public static class DbContextMocker
{
    public static DatabaseContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var dbContext = new DatabaseContext(options);

        // Seed the database with test data
        Seed(dbContext);

        return dbContext;
    }

    public static void ResetDbContext(DatabaseContext dbContext)
    {
        dbContext.Database.EnsureDeleted();
    }

    private static void Seed(DatabaseContext dbContext)
    {
        // Seed the database with test data
        var users = new List<User>
        {
            new User { Id = 1, Username = "john", Firstname = "John", Lastname = "Doe" }
        };
        dbContext.Users.AddRange(users);

        var groups = new List<Group>
        {
            new Group { Id = 1, Title = "Group 1", Color = "Red", UserId = 1 },
            new Group { Id = 2, Title = "Group 2", Color = "Green", UserId = 1 }
        };
        dbContext.Groups.AddRange(groups);

        var tasks = new List<TaskModel>
        {
            new TaskModel
            {
                Id = 1,
                Title = "Task 1",
                Description = "Description 1",
                Date = DateTime.ParseExact("2023-07-11", "yyyy'-'MM'-'dd", null),
                Priority = TaskPriority.Low,
                Url = "http://example.com",
                IsChecked = false,
                IsFlagged = true,
                GroupId = 1,
                UserId = 1
            },
            new TaskModel
            {
                Id = 2,
                Title = "Task 2",
                Description = "Description 2",
                Date = DateTime.ParseExact("2023-07-12", "yyyy'-'MM'-'dd", null),
                Priority = TaskPriority.High,
                Url = null,
                IsChecked = true,
                IsFlagged = false,
                GroupId = 2,
                UserId = 1
            },
        };
        dbContext.Tasks.AddRange(tasks);

        dbContext.SaveChanges();
    }
}