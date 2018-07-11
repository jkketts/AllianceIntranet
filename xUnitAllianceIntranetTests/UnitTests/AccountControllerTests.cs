using AllianceIntranet.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using AllianceIntranet.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace xUnitAllianceIntranetTests.UnitTests
{
    public class AccountControllerTests
    {
        [Theory]
        [InlineData(1)]
        public async Task Edit_WithNonexistingId(int id)
        {
            //Arrange
            var mockRepo = new Mock<UserManager<AppUser>>();
            var role = mockRepo.
            
            //Act
            


        }

    }
}
