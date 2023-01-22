using AutoFixture;
using AutoMapper;
using BL.Services;
using DataAccess.Repositories;
using Entities.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using proj.Controllers;
using proj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.ControllerTests
{
    public class OperationControllerTests
    {
        [Test]
        public async Task GetAllAsync_200StatusPositive_CallOperationService()
        {
            // Arrange
            var operationService = new Mock<IOperationService>();
            var cardService = new Mock<ICardService>();
            var mapper = new Mock<IMapper>();
            var operationController = new OperationController( operationService.Object, cardService.Object, mapper.Object);
            cardService.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Card>());

            // Act
            var data = await operationController.Get();
            var okObject = data as OkObjectResult;

            //Assert
            operationService.Verify(x => x.GetAllAsync(), Times.Once());
            okObject.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test]
        public async Task GetById_200StatusPositive_CallOperationService()
        {
            // Arrange
            var operationService = new Mock<IOperationService>();
            var cardService = new Mock<ICardService>();
            var operationModel = new OperationModel();
            var operation = new Operation
            {
                Name = "buy short",
                Sum = 800,
                Type = OperationType.WritingOff,
                Card = new Card
                {
                    NumberCard = "4149",
                    CardAmount = 10000,
                    User = new User
                    {
                        Id = 1,
                        FirstName = "Dima",
                        LastName = "Last"
                    }
                }
            };
            operationService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(operation);
            var mapper = new Mock<IMapper>();
            var operationController = new OperationController(operationService.Object, cardService.Object, mapper.Object);
            operationService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(operation);
            mapper.Setup(x => x.Map<OperationModel>(operation)).Returns(operationModel);

            // Act
            var result = await operationController.GetById(1);
            var okObject = result as OkObjectResult;

            //Assert
            okObject.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okObject.Value.Should().BeEquivalentTo(operationModel);
        }

        [Test]
        public async Task Create_200StatusPositive_CallOperationService()
        {
            // Arrange
            var serviceCard = new Mock<ICardService>();
            var operationService = new Mock<IOperationService>();
            var mapper = new Mock<IMapper>();
            var fix = new Fixture();
            fix.Behaviors.Remove(new ThrowingRecursionBehavior());
            fix.Behaviors.Add(new OmitOnRecursionBehavior());
            var operationModel = fix.Create<OperationModel>();
            var operation = fix.Create<Operation>();
            var card = fix.Create<Card>();
            var operationController = new OperationController(operationService.Object, serviceCard.Object, mapper.Object);
            serviceCard.Setup(x => x.GetByIdAsync(operationModel.CardId)).ReturnsAsync(card);
            mapper.Setup(x => x.Map<Card>(operationModel)).Returns(card);

            // Act
            var result = await operationController.Create(operationModel);
            var okObject = result as StatusCodeResult;

            //Assert
            okObject.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
        [Test]
        public async Task Delete_True_CallOperationService()
        {
            // Arrange
            var serviceCard = new Mock<ICardService>();
            var serviceOperation = new Mock<IOperationService>();
            var fix = new Fixture();
            fix.Behaviors.Remove(new ThrowingRecursionBehavior());
            fix.Behaviors.Add(new OmitOnRecursionBehavior());
            var operation = fix.Create<Operation>();
            var mapper = new Mock<IMapper>();
            var operationController = new OperationController (serviceOperation.Object, serviceCard.Object, mapper.Object);
            serviceOperation.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(operation);

            // Act
            await operationController.DeleteAsync(1);

            //Assert
            serviceOperation.Verify(x => x.DeleteAsync(It.IsAny<Operation>()), Times.Once());
        }
    }
}
