using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Moq;
using Xunit;
using System.Linq;

namespace ProcessSystem.Contracts
{
    public class RegisterRequestTests
    {
        [Fact]
        public void RegisterRequestNameTests()
        {
            // Arrange
            RegisterRequest request = new RegisterRequest
            {
                Name = "  ",
                Url = null,
                ProcessTypesList = null,               
            };

            // Act
            var mohova = request.Validate(null).ToList();
            

            // Assert
            Assert.Equal("Имя витрины должно быть задано", mohova.First().ErrorMessage);
            
        }

        [Fact]
        public void RegisterRequestUrlTests()
        {
            // Arrange
            RegisterRequest request = new RegisterRequest
            {
                Name = "  ",
                Url = null,
                ProcessTypesList = null,
            };

            // Act
            var mohova = request.Validate(null).ToList();

            // Assert
            Assert.Equal("Url для ответа пустой", mohova[1].ErrorMessage);
        }

        [Fact]
        public void RegisterRequestProcessTypelistTests()
            {
                // Arrange
                RegisterRequest request = new RegisterRequest
                {
                    Name = "  ",
                    Url = null,
                    ProcessTypesList = null,
                };

                // Act
                var mohova = request.Validate(null).ToList();

                // Assert
                Assert.Equal("Список событий подписки пустой", mohova[2].ErrorMessage);
            }
    }
}