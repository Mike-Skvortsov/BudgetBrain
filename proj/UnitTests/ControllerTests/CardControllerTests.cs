using AutoFixture;
using AutoMapper;
using BL.Services;
using Entities.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using proj.Controllers;
using proj.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace UnitTests.ControllerTests
{
    public class CardControllerTests
    {
        [Test]
        public async Task GetAllAsync_200StatusPositive_CallUserService()
        {
            // Arrange
            var userService = new Mock<IUserService>();
            var cardService = new Mock<ICardService>();
            var mapper = new Mock<IMapper>();
            var cardController = new CardController(cardService.Object, userService.Object, mapper.Object);
            cardService.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Card>());

            // Act
            var data = await cardController.Get();
            var okObject = data as OkObjectResult;

            //Assert
            cardService.Verify(x => x.GetAllAsync(), Times.Once());
            okObject.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test]
        public async Task GetById_200StatusPositive_CallUserService()
        {
            // Arrange
            var userService = new Mock<IUserService>();
            var cardService = new Mock<ICardService>();
            var cardModel = new CardModel();
            var card = new Card
            {
                NumberCard = "4149",
                CardAmount = 10000,
                User = new User
                {
                    Id = 1,
                    FirstName = "Dima",
                    LastName = "Last"
                }
            };
            cardService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(card);
            var mapper = new Mock<IMapper>();
            var cardController = new CardController(cardService.Object, userService.Object, mapper.Object);
            cardService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(card);
            mapper.Setup(x => x.Map<CardModel>(card)).Returns(cardModel);

            // Act
            var result = await cardController.GetById(1);
            var okObject = result as OkObjectResult;

            //Assert
            okObject.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okObject.Value.Should().BeEquivalentTo(cardModel);
        }

        [Test]
        public async Task Create_200StatusPositive_CallCardService()
        {
            // Arrange
            var serviceCard = new Mock<ICardService>();
            var serviceUser = new Mock<IUserService>();
            var mapper = new Mock<IMapper>();
            var fix = new Fixture();
            fix.Behaviors.Remove(new ThrowingRecursionBehavior());
            fix.Behaviors.Add(new OmitOnRecursionBehavior());
            var cardModel = fix.Create<CardModel>();
            var user = fix.Create<User>();
            var card = fix.Create<Card>();
            var cardController = new CardController(serviceCard.Object, serviceUser.Object, mapper.Object);
            serviceUser.Setup(x => x.GetByIdAsync(cardModel.UserId)).ReturnsAsync(user);
            mapper.Setup(x => x.Map<Card>(cardModel)).Returns(card);

            // Act
            var result = await cardController.Create(cardModel);
            var okObject = result as StatusCodeResult;

            //Assert
            okObject.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
        [Test]
        public async Task Delete_True_CallCardService()
        {
            // Arrange
            var serviceCard = new Mock<ICardService>();
            var serviceUser = new Mock<IUserService>();
            var fix = new Fixture();
            fix.Behaviors.Remove(new ThrowingRecursionBehavior());
            fix.Behaviors.Add(new OmitOnRecursionBehavior());
            var card = fix.Create<Card>();
            var mapper = new Mock<IMapper>();
            var cardController = new CardController(serviceCard.Object, serviceUser.Object, mapper.Object);
            serviceCard.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(card);

            // Act
            await cardController.DeleteAsync(1);

            //Assert
            serviceCard.Verify(x => x.DeleteAsync(It.IsAny<Card>()), Times.Once());
        }
    }
}
