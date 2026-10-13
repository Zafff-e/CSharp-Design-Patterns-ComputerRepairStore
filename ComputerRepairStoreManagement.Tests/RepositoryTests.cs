using ComputerRepairStoreManagement.Entities;
using ComputerRepairStoreManagement.Repositories;
using System.Linq;
using Xunit;

namespace ComputerRepairStoreManagement.Tests
{
    public class RepositoryTests
    {
        [Fact]
        public void RepairRepository_InitializesWithThreeSeedRecords()
        {
            var repo = new RepairRepository();

            var list = repo.GetAllRequestsList().ToList();

            Assert.NotNull(list);
            Assert.Equal(3, list.Count);
            Assert.Equal(1, list[0].RequestId);
            Assert.Equal(2, list[1].RequestId);
            Assert.Equal(3, list[2].RequestId);
        }

        [Fact]
        public void GetRequestById_ReturnsCorrectRecord_WhenIdExists()
        {
            var repo = new RepairRepository();

            var item = repo.GetRequestById(2);

            Assert.NotNull(item);
            Assert.Equal("Nusrat Jahan", item.CustomerName);
            Assert.Equal(RepairCategory.Software, item.Category);
        }

        [Fact]
        public void GetRequestById_ReturnsNull_WhenIdDoesNotExist()
        {
            var repo = new RepairRepository();

            var item = repo.GetRequestById(999);

            Assert.Null(item);
        }

        [Fact]
        public void SaveRequest_AutoIncrementsId_AndPersists()
        {
            var repo = new RepairRepository();
            var newTicket = new RepairRequest(0, "Farhan Kabir", "Laptop", "Keyboard not responding", RepairCategory.Hardware)
            {
                TotalEstimate = 2500,
                AdvancePaid = 500
            };

            repo.SaveRequest(newTicket);

            Assert.Equal(4, newTicket.RequestId);
            Assert.Equal(4, repo.GetAllRequestsList().Count());
            Assert.NotNull(repo.GetRequestById(4));
        }

        [Fact]
        public void UpdateRequest_ModifiesExistingRecord()
        {
            var repo = new RepairRepository();
            var existing = repo.GetRequestById(1);
            Assert.NotNull(existing);

            existing.CustomerName = "Tanvir Hossain Updated";
            existing.Status = RepairStatus.Completed;
            existing.AdvancePaid = 7665;

            repo.UpdateRequest(existing);

            var refreshed = repo.GetRequestById(1);
            Assert.Equal("Tanvir Hossain Updated", refreshed.CustomerName);
            Assert.Equal(RepairStatus.Completed, refreshed.Status);
            Assert.Equal(7665, refreshed.AdvancePaid);
        }

        [Fact]
        public void DeleteRequest_RemovesRecordFromRepository()
        {
            var repo = new RepairRepository();

            repo.DeleteRequest(1);

            Assert.Equal(2, repo.GetAllRequestsList().Count());
            Assert.Null(repo.GetRequestById(1));
        }
    }
}
