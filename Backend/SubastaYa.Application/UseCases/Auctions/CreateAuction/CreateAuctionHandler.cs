using SubastaYa.Application.Interfaces;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Auctions;
using SubastaYa.Domain.Constants;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.UseCases.Auctions.CreateAuction
{
    public class CreateAuctionHandler : ICreateAuctionHandler
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAuctionHandler(IAuctionRepository auctionRepository, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IUserRepository userRepository)
        {
            _auctionRepository = auctionRepository;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        public async Task<int> HandleAsync(CreateAuctionCommand command)
        {
            if (command.EndDate <= command.StartDate)
                throw new ValidationException("La fecha de finalización debe ser posterior a la fecha de inicio.");

            if(!await _userRepository.ExistsByIdAsync(command.SellerId))
                throw new NotFoundException("El usuario no se encontro");

            if (!await _categoryRepository.ExistsAsync(command.CategoryId))
                throw new NotFoundException("La categoría especificada no existe");


            var status = command.StartDate <= DateTime.UtcNow ? AuctionConstants.Active : AuctionConstants.Scheduled;

            var auction = new Auction
            {
                SellerId = command.SellerId,
                Title = command.Title,
                Description = command.Description,
                ImageUrl = command.ImageUrl,
                BasePrice = command.BasePrice,
                CurrentBidAmount = command.BasePrice,
                MinimumIncrement = command.MinimumIncrement,
                StartDate = command.StartDate,
                EndDate = command.EndDate,
                CategoryId = command.CategoryId,
                Status = status
            };

            await _auctionRepository.AddAsync(auction);

            await _unitOfWork.SaveChangesAsync();

            return auction.Id;
        }
    }
}
