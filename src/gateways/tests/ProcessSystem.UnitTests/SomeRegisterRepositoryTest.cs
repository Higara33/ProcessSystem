using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Moq;
using Xunit;
using System.Linq;
using ProcessSystem.DB;
using Common.DB;
using Newtonsoft.Json;
using ProcessSystem.Contracts;
using System;
using System.Threading.Tasks;

namespace ProcessSystem.UnitTests
{
   
    public class SomeRegisterRepositoryTest
    {
        public readonly Mock<ProcessContext> _mockContext;
        public readonly RegisterRepository registerRepository;
        
        public SomeRegisterRepositoryTest()
        {
            _mockContext = new Mock<ProcessContext>();
            registerRepository = new RegisterRepository(_mockContext.Object);
        }

        string _token = "fhasdbvdhfb";
        string _url = "www";
        string _name = "user";

        [Fact]
        public void FindByNameAndUrlTests()
        {
            // Arrange
            
            Register register = new Register(_token, _url, _name);

            // Act
            var mohova = registerRepository.FindByNameAndUrl(register);


            // Assert
            Assert.NotNull(mohova);

        }

        [Fact]
        public void FindByTokenAsyncTests()
        {
            // Arrange
            
            

            // Act
            var mohova = registerRepository.FindByTokenAsync("fhasdbvdhfb");


            // Assert
            Assert.NotNull(mohova);

        }

    }
}
