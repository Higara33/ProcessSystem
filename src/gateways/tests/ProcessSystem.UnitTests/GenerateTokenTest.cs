using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Moq;
using Xunit;
using System.Linq;
using ProcessSystem.Token;

namespace ProcessSystem.UnitTests
{
    public class GenerateTokenTest
    {
    

        [Fact]
        public void GenerateTokenMethodTest()
        {

            // Arrange
            TokenImpl token = new TokenImpl();
            

            // Act
            var mohova = token.GenerateToken();


            // Assert
            Assert.NotNull(mohova);
            Assert.Equal("mysupersecret_secretkey!123", mohova);
        }

     
    }
}
