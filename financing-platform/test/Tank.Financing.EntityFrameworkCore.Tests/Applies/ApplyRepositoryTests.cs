using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tank.Financing.Applies;
using Tank.Financing.EntityFrameworkCore;
using Xunit;

namespace Tank.Financing.Applies
{
    public class ApplyRepositoryTests : FinancingEntityFrameworkCoreTestBase
    {
        private readonly IApplyRepository _applyRepository;

        public ApplyRepositoryTests()
        {
            _applyRepository = GetRequiredService<IApplyRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _applyRepository.GetListAsync(
                    enterpriseName: "657c6a75d9d340bc93ffb829ae8efcd5d51855f7377149a1ad4f60c6d7648de0489",
                    organization: "e839b8eb5a6441a783f395fbdf7e07a5afbc8fced41c4f939c80b2737078f852f84e44a8f90e46edb36",
                    productName: "f9e90934c9404a0f8ff8d20884fc9c52156166956a5644bea6f6618f180a0e80728a4bca1f3d47de97592f8b9a94f3fedd9",
                    allowance: "5255c48f9754",
                    aPR: "dc7c4d926f4d45bba11bd1717346d67041eb3abb6d85493baaa42f43f29f872b409b668596f841f5a828d143a059a5dc92f",
                    period: "a3b10cf493db4b13bcce",
                    applyStatus: default,
                    guaranteeMethod: default
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("4ded5ed9-024e-4820-b29c-46bea9ffabc9"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _applyRepository.GetCountAsync(
                    enterpriseName: "672537ba89c54b30b66c14343e98de93fc0e87cdc15d4b8aa1c6d20f46321cf54a82a0fe4",
                    organization: "b5d25ea0e713466fb6f9381f0",
                    productName: "062a331ce0c4467796eb57a56e5f556dba06f4fefbb14dbbae2e3bbeb1f6",
                    allowance: "3722e70b0c814c0282d453ba3e5d9d4ff27816b289704574",
                    aPR: "bf6f52992c654",
                    period: "fe4e449b48a546b38c7cabfbe0c41a86be8303e24a4e4114b70f054598bac60d4ca15c73c9f44b97af72b49de8",
                    applyStatus: default,
                    guaranteeMethod: default
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}