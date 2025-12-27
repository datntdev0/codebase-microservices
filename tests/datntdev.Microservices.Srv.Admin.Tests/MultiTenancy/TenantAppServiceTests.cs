using System.Net.Http.Json;
using datntdev.Microservices.Common;
using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Srv.Admin.Contract.MultiTenancy.Dto;
using datntdev.Microservices.Srv.Admin.Web.App.MultiTenancy.Models;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Srv.Admin.MultiTenancy
{
    [TestClass]
    public class TenantAppServiceTests : SrvAdminTestBase
    {
        public const string TenantNamePrefix = "testtenant_";
        public const string BaseUrl = "/api/tenants";

        private static AppTenantEntity _defaultTenant = default!;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _defaultTenant = GetTenantById(Constants.MultiTenancy.DefaultTenantId);
        }

        #region CreateAsync

        [TestMethod]
        public async Task Create_WithValidData_ReturnsNewTenant()
        {
            // Arrange
            var createDto = new TenantCreateDto
            {
                TenantName = $"{TenantNamePrefix}{Guid.NewGuid():N}"
            };

            // Act
            var response = await _client.PostAsJsonAsync(BaseUrl, createDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TenantDto>();
            Assert.IsNotNull(result);
            Assert.AreEqual(createDto.TenantName, result.TenantName);
            Assert.IsNotNull(result.CreatedAt);

            var createdTenant = GetTenantByName(createDto.TenantName);
            Assert.AreEqual(createDto.TenantName, createdTenant.TenantName);
        }

        [TestMethod]
        public async Task Create_WithEmptyName_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new TenantCreateDto
            {
                TenantName = "" // Empty name
            };

            // Act
            var response = await _client.PostAsJsonAsync(BaseUrl, createDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Create_WithDuplicateName_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new TenantCreateDto
            {
                TenantName = _defaultTenant.TenantName
            };

            // Act
            var response = await _client.PostAsJsonAsync(BaseUrl, createDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region GetAsync & GetAllAsync

        [TestMethod]
        public async Task Get_WithExistingTenantId_ReturnsTenant()
        {
            // Act
            var response = await _client.GetAsync($"{BaseUrl}/{_defaultTenant.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TenantDto>();
            Assert.IsNotNull(result);
            Assert.AreEqual(_defaultTenant.Id, result.Id);
            Assert.AreEqual(_defaultTenant.TenantName, result.TenantName);
        }

        [TestMethod]
        public async Task Get_WithNonExistingTenantId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingTenantId = 999999;

            // Act
            var response = await _client.GetAsync($"{BaseUrl}/{nonExistingTenantId}");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task GetAll_WithPagination_ReturnsPaginatedResult()
        {
            // Arrange - Create multiple test tenants
            var tenantsToCreate = 3;
            var createdTenantIds = new List<int>();

            for (int i = 0; i < tenantsToCreate; i++)
            {
                var createDto = new TenantCreateDto
                {
                    TenantName = $"{TenantNamePrefix}{Guid.NewGuid():N}"
                };

                var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
                createResponse.EnsureSuccessStatusCode();
                var createdTenant = await createResponse.Content.ReadFromJsonAsync<TenantDto>();
                Assert.IsNotNull(createdTenant);
                createdTenantIds.Add(createdTenant.Id);
            }

            // Act
            var offset = 0;
            var limit = 10;
            var response = await _client.GetAsync($"{BaseUrl}?Offset={offset}&Limit={limit}");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<PaginatedResult<TenantListDto>>();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
            Assert.IsTrue(result.Total >= tenantsToCreate); // At least the tenants we created (plus default tenant)
            Assert.AreEqual(limit, result.Limit);
            Assert.AreEqual(offset, result.Offset);
            
            // Verify our created tenants are in the result
            var resultTenantIds = result.Items.Select(t => t.Id).ToList();
            foreach (var createdTenantId in createdTenantIds)
            {
                Assert.IsTrue(resultTenantIds.Contains(createdTenantId), 
                    $"Created tenant with ID {createdTenantId} should be in the paginated result");
            }
        }

        [TestMethod]
        public async Task GetAll_WithDifferentPagination_ReturnsCorrectPage()
        {
            // Arrange - Create multiple test tenants
            var tenantsToCreate = 5;
            for (int i = 0; i < tenantsToCreate; i++)
            {
                var createDto = new TenantCreateDto
                {
                    TenantName = $"{TenantNamePrefix}{Guid.NewGuid():N}"
                };
                await _client.PostAsJsonAsync(BaseUrl, createDto);
            }

            // Act - Get first page
            var limit = 2;
            var firstPageResponse = await _client.GetAsync($"{BaseUrl}?Offset=0&Limit={limit}");
            var firstPage = await firstPageResponse.Content.ReadFromJsonAsync<PaginatedResult<TenantListDto>>();

            // Act - Get second page
            var secondPageResponse = await _client.GetAsync($"{BaseUrl}?Offset={limit}&Limit={limit}");
            var secondPage = await secondPageResponse.Content.ReadFromJsonAsync<PaginatedResult<TenantListDto>>();

            // Assert
            Assert.IsNotNull(firstPage);
            Assert.IsNotNull(secondPage);
            Assert.AreEqual(limit, firstPage.Items.Count());
            Assert.AreEqual(limit, secondPage.Items.Count());
            Assert.AreEqual(firstPage.Total, secondPage.Total);
            
            // Verify pages don't contain the same tenants
            var firstPageIds = firstPage.Items.Select(t => t.Id).ToList();
            var secondPageIds = secondPage.Items.Select(t => t.Id).ToList();
            Assert.IsFalse(firstPageIds.Intersect(secondPageIds).Any(), "Pages should not contain the same tenants");
        }

        #endregion

        #region UpdateAsync

        [TestMethod]
        public async Task Update_WithValidData_UpdatesTenant()
        {
            // Arrange - Create a tenant
            var createDto = new TenantCreateDto
            {
                TenantName = $"{TenantNamePrefix}{Guid.NewGuid():N}"
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdTenant = await createResponse.Content.ReadFromJsonAsync<TenantDto>();
            Assert.IsNotNull(createdTenant);

            // Arrange - Prepare update data
            var updateDto = new TenantUpdateDto
            {
                TenantName = $"{TenantNamePrefix}{Guid.NewGuid():N}_updated"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{createdTenant.Id}", updateDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TenantDto>();
            Assert.IsNotNull(result);
            Assert.AreEqual(createdTenant.Id, result.Id);
            Assert.AreEqual(updateDto.TenantName, result.TenantName);
            Assert.IsNotNull(result.UpdatedAt);
            
            // Verify in database
            var updatedTenant = GetTenantByName(updateDto.TenantName);
            Assert.AreEqual(updateDto.TenantName, updatedTenant.TenantName);
        }

        [TestMethod]
        public async Task Update_WithNonExistingTenantId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingTenantId = 999999;
            var updateDto = new TenantUpdateDto
            {
                TenantName = "nonexistingtenant"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{nonExistingTenantId}", updateDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WithEmptyName_ReturnsBadRequest()
        {
            // Arrange - Create a tenant
            var createDto = new TenantCreateDto
            {
                TenantName = $"{TenantNamePrefix}{Guid.NewGuid():N}"
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdTenant = await createResponse.Content.ReadFromJsonAsync<TenantDto>();
            Assert.IsNotNull(createdTenant);

            // Arrange - Prepare update with empty name
            var updateDto = new TenantUpdateDto
            {
                TenantName = "" // Empty name
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{createdTenant.Id}", updateDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WithDuplicateName_ReturnsBadRequest()
        {
            // Arrange - Create a tenant
            var tenantDto = new TenantCreateDto
            {
                TenantName = $"{TenantNamePrefix}{Guid.NewGuid():N}"
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, tenantDto);
            createResponse.EnsureSuccessStatusCode();
            var createdTenant = await createResponse.Content.ReadFromJsonAsync<TenantDto>();
            Assert.IsNotNull(createdTenant);

            // Arrange - Get the default tenant
            var defaultTenant = GetTenantById(Constants.MultiTenancy.DefaultTenantId);
            Assert.IsNotNull(defaultTenant);

            // Arrange - Try to update created tenant with default tenant's name
            var updateDto = new TenantUpdateDto
            {
                TenantName = defaultTenant.TenantName // Duplicate name
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{createdTenant.Id}", updateDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WithDefaultTenant_ReturnsConflict()
        {
            // Arrange
            var updateDto = new TenantUpdateDto
            {
                TenantName = $"{TenantNamePrefix}modified_default"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{_defaultTenant.Id}", updateDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.Conflict, response.StatusCode);
        }

        #endregion

        #region DeleteAsync

        [TestMethod]
        public async Task Delete_WithExistingTenantId_DeletesTenant()
        {
            // Arrange - Create a tenant to delete
            var createDto = new TenantCreateDto
            {
                TenantName = $"{TenantNamePrefix}{Guid.NewGuid():N}"
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdTenant = await createResponse.Content.ReadFromJsonAsync<TenantDto>();
            Assert.IsNotNull(createdTenant);

            // Act
            var response = await _client.DeleteAsync($"{BaseUrl}/{createdTenant.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            
            // Verify tenant is deleted in database
            var dbContext = GetDbContext();
            var deletedTenant = await dbContext.AppTenants
                .FirstOrDefaultAsync(t => t.Id == createdTenant.Id);
            Assert.IsNull(deletedTenant);
        }

        [TestMethod]
        public async Task Delete_WithNonExistingTenantId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingTenantId = 999999;

            // Act
            var response = await _client.DeleteAsync($"{BaseUrl}/{nonExistingTenantId}");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_WithDefaultTenant_ReturnsConflict()
        {
            // Act
            var response = await _client.DeleteAsync($"{BaseUrl}/{_defaultTenant.Id}");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.Conflict, response.StatusCode);
        }

        #endregion
    }
}
