using Microsoft.EntityFrameworkCore;
using SubastaYa.Application.Interfaces.Security;
using SubastaYa.Domain.Constants;
using SubastaYa.Domain.Entities;
using SubastaYa.Infrastructure.Persistence;

namespace SubastaYa.API
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<AppDbContext>();
            var passwordHasher = services.GetRequiredService<IPasswordHasher>();

            await context.Database.MigrateAsync();

            if (context.Users.Any())
                return;

            var userOne = new User
            {
                Name = "VendedorTest",
                Email = "vendedor@test.com",
                PasswordHash = passwordHasher.Hash("contra123")
            };

            var walletOne = new Wallet
            {
                User = userOne,
                TotalBalance = 0,
                HeldBalance = 0
            };

            var userTwo = new User
            {
                Name = "Comprador1Test",
                Email = "comprador1@test.com",
                PasswordHash = passwordHasher.Hash("contra123")
            };

            var walletTwo = new Wallet
            {
                User = userTwo,
                TotalBalance = 150000,
                HeldBalance = 45000
            };

            var userThree = new User
            {
                Name = "Comprador2Test",
                Email = "comprador2@test.com",
                PasswordHash = passwordHasher.Hash("contra123")
            };

            var walletThree = new Wallet
            {
                User = userThree,
                TotalBalance = 200000,
                HeldBalance = 50000
            };

            var userFour = new User
            {
                Name = "sinFondos",
                Email = "sinfondos@test.com",
                PasswordHash = passwordHasher.Hash("contra123")
            };

            var walletFour = new Wallet
            {
                User = userFour,
                TotalBalance = 500,
                HeldBalance = 0
            };

            context.Users.AddRange(userOne, userTwo, userThree, userFour);
            context.Wallets.AddRange(walletOne, walletTwo, walletThree, walletFour);

            await context.SaveChangesAsync();

            var categoryTech = new Category { Name = "Tecnología", IconUrl = "https://ejemplo.com/icono.png" };
            var categoryCollectibles = new Category { Name = "Coleccionables", IconUrl = "https://ejemplo.com/icono.png" };
            var categoryClothing = new Category { Name = "Indumentaria", IconUrl = "https://ejemplo.com/icono.png" };
            var categoryVehicles = new Category { Name = "Vehículos", IconUrl = "https://ejemplo.com/icono.png" };

            context.Categories.AddRange(categoryTech, categoryCollectibles, categoryClothing, categoryVehicles);
            await context.SaveChangesAsync();

            var auctionStandard = new Auction
            {
                Seller = userOne,
                CategoryId = categoryCollectibles.Id,
                Title = "Reloj antiguo coleccionable",
                Description = "Reloj de colección en buen estado, funcionando",
                ImageUrl = "https://ejemplo.com/reloj.jpg",
                BasePrice = 40000,
                CurrentBidAmount = 45000,
                MinimumIncrement = 1000,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMinutes(25),
                Status = AuctionConstants.Active
            };

            var auctionCritic = new Auction
            {
                Seller = userOne,
                CategoryId = categoryVehicles.Id,
                Title = "Casco de moto",
                Description = "Casco integral, talle M",
                ImageUrl = "https://ejemplo.com/casco.jpg",
                BasePrice = 5000,
                CurrentBidAmount = 5000,
                MinimumIncrement = 500,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMinutes(2),
                Status = AuctionConstants.Active
            };

            var auctionUpcoming = new Auction
            {
                Seller = userOne,
                CategoryId = categoryTech.Id,
                Title = "Notebook gamer",
                Description = "Notebook nueva, sin uso",
                ImageUrl = "https://ejemplo.com/notebook.jpg",
                BasePrice = 80000,
                CurrentBidAmount = 80000,
                MinimumIncrement = 20000,
                StartDate = DateTime.UtcNow.AddHours(24),
                EndDate = DateTime.UtcNow.AddHours(48),
                Status = AuctionConstants.Scheduled
            };

            var auctionExpiredWithWinner = new Auction
            {
                Seller = userOne,
                CategoryId = categoryVehicles.Id,
                Title = "Bicicleta de montaña",
                Description = "Bicicleta rodado 29, poco uso",
                ImageUrl = "https://ejemplo.com/bici.jpg",
                BasePrice = 10000,
                CurrentBidAmount = 50000,
                MinimumIncrement = 5000,
                StartDate = DateTime.UtcNow.AddHours(-2),
                EndDate = DateTime.UtcNow.AddMinutes(-5),
                Status = AuctionConstants.Active
            };

            var auctionExpiredDeserted = new Auction
            {
                Seller = userOne,
                CategoryId = categoryClothing.Id,
                Title = "Campera de cuero",
                Description = "Campera talle L, nunca usada",
                ImageUrl = "https://ejemplo.com/campera.jpg",
                BasePrice = 30000,
                CurrentBidAmount = 30000,
                MinimumIncrement = 2000,
                StartDate = DateTime.UtcNow.AddHours(-2),
                EndDate = DateTime.UtcNow.AddMinutes(-5),
                Status = AuctionConstants.Active
            };

            var bidStandardOne = new Bid
            {
                Auction = auctionStandard,
                BuyerId = userThree.Id,
                Amount = 30000,
                CreatedAt = DateTime.UtcNow,
            };

            var bidStandardTwo = new Bid
            {
                Auction = auctionStandard,
                BuyerId = userTwo.Id,
                Amount = 45000,
                CreatedAt = DateTime.UtcNow.AddMinutes(1),
            };

            var bidWinner = new Bid
            {
                Auction = auctionExpiredWithWinner,
                BuyerId = userThree.Id,
                Amount = 50000,
                CreatedAt = DateTime.UtcNow,
            };

            var ledgerRetentionThreeStandard = new TransactionLedger
            {
                Wallet = walletThree,
                Type = TransactionConstants.Retention,
                Amount = 30000,
                CreatedAt = DateTime.UtcNow.AddMinutes(-1),
                Auction = auctionStandard
            };

            var ledgerReleaseThreeStandard = new TransactionLedger
            {
                Wallet = walletThree,
                Type = TransactionConstants.Release,
                Amount = 30000,
                CreatedAt = DateTime.UtcNow.AddMinutes(1),
                Auction = auctionStandard
            };

            var ledgerDepositTwo = new TransactionLedger
            {
                Wallet = walletTwo,
                Type = TransactionConstants.Deposit,
                Amount = 150000,
                CreatedAt = DateTime.UtcNow.AddMinutes(-10)
            };

            var ledgerRetentionTwo = new TransactionLedger
            {
                Wallet = walletTwo,
                Type = TransactionConstants.Retention,
                Amount = 45000,
                CreatedAt = DateTime.UtcNow.AddMinutes(1),
                Auction = auctionStandard
            };

            var ledgerDepositThree = new TransactionLedger
            {
                Wallet = walletThree,
                Type = TransactionConstants.Deposit,
                Amount = 200000,
                CreatedAt = DateTime.UtcNow.AddHours(-3)
            };

            var ledgerRetentionThree = new TransactionLedger
            {
                Wallet = walletThree,
                Type = TransactionConstants.Retention,
                Amount = 50000,
                CreatedAt = DateTime.UtcNow,
                Auction = auctionExpiredWithWinner
            };

            context.Bids.AddRange(bidStandardOne, bidStandardTwo, bidWinner);
            context.Auctions.AddRange(auctionStandard, auctionCritic, auctionUpcoming, auctionExpiredWithWinner, auctionExpiredDeserted);
            context.TransactionLedgers.AddRange(ledgerDepositTwo, ledgerRetentionTwo, ledgerDepositThree, ledgerRetentionThree, ledgerRetentionThreeStandard, ledgerReleaseThreeStandard);
            await context.SaveChangesAsync();
        }
    }
}
