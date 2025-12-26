using System.Net.Http.Json;
using datntdev.Microservices.Common;
using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Permissions.Dto;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles.Dto;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Users.Dto;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles.Models;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models;
using Microsoft.EntityFrameworkCore;
using static datntdev.Microservices.Common.Constants.Enum;

namespace datntdev.Microservices.Srv.Identity.Authorization
{
    [TestClass]
    public class RoleAppServiceTests : SrvIdentityTestBase
    {
        public const string RoleNamePrefix = "testrole_";
        public const string BaseUrl = "/api/roles";

        private static AppUserEntity _adminUser = default!;
        private static AppRoleEntity _adminRole = default!;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _adminUser = GetAdminUser();
            _adminRole = GetRoleByName(Constants.Authorization.DefaultAdminRole);
        }

        #region CreateAsync

        [TestMethod]
        public async Task Create_WithValidData_ReturnsNewRole()
        {
            // Arrange
            var createDto = new RoleCreateDto
            {
                TenantId = null,
                Name = $"{RoleNamePrefix}{Guid.NewGuid():N}",
                Description = "Test Role Description",
                Permissions = [AppPermission.Users_Read],
                UserIds = [_adminUser.Id]
            };

            // Act
            var response = await _client.PostAsJsonAsync(BaseUrl, createDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<RoleDto>();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(createDto.Name, result.Name);
            Assert.AreEqual(createDto.Description, result.Description);
            Assert.AreEqual(createDto.TenantId, result.TenantId);
            Assert.IsNotNull(result.CreatedAt);

            var createdRole = GetRoleByName(createDto.Name);
            Assert.AreEqual(createDto.Name, createdRole.Name);
            Assert.AreEqual(createDto.Description, createdRole.Description);
            Assert.AreEqual(createDto.TenantId, createdRole.TenantId);
            CollectionAssert.AreEquivalent(createDto.Permissions, createdRole.Permissions);
            CollectionAssert.AreEquivalent(createDto.UserIds, createdRole.Users.Select(u => u.Id).ToArray());
        }

        [TestMethod]
        public async Task Create_WithEmptyName_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new RoleCreateDto
            {
                TenantId = null,
                Name = "", // Empty name
                Description = "Test Role Description",
                Permissions = [],
                UserIds = []
            };

            // Act
            var response = await _client.PostAsJsonAsync(BaseUrl, createDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Create_WithDuplicateName_ReturnBadRequest()
        {
            // Arrange - Create first role
            var firstRoleName = $"{RoleNamePrefix}{Guid.NewGuid():N}";
            
            var firstRoleDto = new RoleCreateDto
            {
                TenantId = null,
                Name = firstRoleName,
                Description = "First Test Role",
                Permissions = [],
                UserIds = []
            };

            var firstResponse = await _client.PostAsJsonAsync(BaseUrl, firstRoleDto);
            firstResponse.EnsureSuccessStatusCode();

            // Arrange - Create second role with duplicate name
            var secondRoleDto = new RoleCreateDto
            {
                TenantId = null,
                Name = firstRoleName, // Duplicate name
                Description = "Second Test Role",
                Permissions = [],
                UserIds = []
            };

            // Act
            var response = await _client.PostAsJsonAsync(BaseUrl, secondRoleDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Create_WithInvalidPermission_ReturnBadRequest()
        {
            // Arrange
            var createDto = new RoleCreateDto
            {
                TenantId = null,
                Name = $"{RoleNamePrefix}{Guid.NewGuid():N}",
                Description = "Test Role Description",
                Permissions = [(AppPermission)99999], // Invalid permission value
                UserIds = []
            };

            // Act
            var response = await _client.PostAsJsonAsync(BaseUrl, createDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region GetAsync & GetAllAsync

        [TestMethod]
        public async Task Get_WithExistingRoleId_ReturnsRole()
        {
            // Arrange - Create a role
            var createDto = new RoleCreateDto
            {
                TenantId = null,
                Name = $"{RoleNamePrefix}{Guid.NewGuid():N}",
                Description = "Test Role Description",
                Permissions = [AppPermission.Users_Read],
                UserIds = [_adminUser.Id]
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdRole = await createResponse.Content.ReadFromJsonAsync<RoleDto>();
            Assert.IsNotNull(createdRole);

            // Act
            var response = await _client.GetAsync($"{BaseUrl}/{createdRole.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<RoleDto>();
            Assert.IsNotNull(result);
            Assert.AreEqual(createdRole.Id, result.Id);
            Assert.AreEqual(createDto.Name, result.Name);
            Assert.AreEqual(createDto.Description, result.Description);
            Assert.AreEqual(createDto.TenantId, result.TenantId);
        }

        [TestMethod]
        public async Task Get_WithNonExistingRoleId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingRoleId = 999999;

            // Act
            var response = await _client.GetAsync($"{BaseUrl}/{nonExistingRoleId}");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task GetAll_WithPagination_ReturnsPaginatedResult()
        {
            // Arrange - Create multiple test roles
            var rolesToCreate = 3;
            var createdRoleIds = new List<int>();

            for (int i = 0; i < rolesToCreate; i++)
            {
                var createDto = new RoleCreateDto
                {
                    TenantId = null,
                    Name = $"{RoleNamePrefix}{Guid.NewGuid():N}",
                    Description = $"Test Role {i} Description",
                    Permissions = [],
                    UserIds = []
                };

                var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
                createResponse.EnsureSuccessStatusCode();
                var createdRole = await createResponse.Content.ReadFromJsonAsync<RoleDto>();
                Assert.IsNotNull(createdRole);
                createdRoleIds.Add(createdRole.Id);
            }

            // Act
            var offset = 0;
            var limit = 10;
            var response = await _client.GetAsync($"{BaseUrl}?Offset={offset}&Limit={limit}");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<PaginatedResult<RoleListDto>>();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
            Assert.IsTrue(result.Total >= rolesToCreate); // At least the roles we created (plus admin role)
            Assert.AreEqual(limit, result.Limit);
            Assert.AreEqual(offset, result.Offset);
            
            // Verify our created roles are in the result
            var resultRoleIds = result.Items.Select(r => r.Id).ToList();
            foreach (var createdRoleId in createdRoleIds)
            {
                Assert.IsTrue(resultRoleIds.Contains(createdRoleId), 
                    $"Created role with ID {createdRoleId} should be in the paginated result");
            }
        }

        #endregion

        #region GetPermissionsAsync & GetUsersAsync

        [TestMethod]
        public async Task GetPermissions_WithExistingRoleId_ReturnsPermissions()
        {
            // Arrange - Create a role with permissions
            var createDto = new RoleCreateDto
            {
                TenantId = null,
                Name = $"{RoleNamePrefix}{Guid.NewGuid():N}",
                Description = "Test Role Description",
                Permissions = [AppPermission.Users_Read, AppPermission.Roles_Read],
                UserIds = []
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdRole = await createResponse.Content.ReadFromJsonAsync<RoleDto>();
            Assert.IsNotNull(createdRole);

            // Act
            var offset = 0;
            var limit = 100;
            var response = await _client.GetAsync($"{BaseUrl}/{createdRole.Id}/permissions?Offset={offset}&Limit={limit}");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<PaginatedResult<PermissionDto>>();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(limit, result.Limit);
            Assert.AreEqual(offset, result.Offset);
            
            // Verify the permissions match
            var fullPermissions = GetPermissions();
            var resultPermissions = result.Items.ToDictionary(x => x.Permission);
            Assert.AreEqual(fullPermissions.Length, result.Items.Count());
            Assert.IsTrue(createDto.Permissions.All(x => resultPermissions[x].IsGranted));
        }

        [TestMethod]
        public async Task GetPermissions_WithNonExistingRoleId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingRoleId = 999999;

            // Act
            var response = await _client.GetAsync($"{BaseUrl}/{nonExistingRoleId}/permissions?Offset=0&Limit=10");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task GetUsers_WithExistingRoleId_ReturnsUsers()
        {
            // Arrange - Create a role with users
            var createDto = new RoleCreateDto
            {
                TenantId = null,
                Name = $"{RoleNamePrefix}{Guid.NewGuid():N}",
                Description = "Test Role Description",
                Permissions = [],
                UserIds = [_adminUser.Id]
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdRole = await createResponse.Content.ReadFromJsonAsync<RoleDto>();
            Assert.IsNotNull(createdRole);

            // Act
            var offset = 0;
            var limit = 10;
            var response = await _client.GetAsync($"{BaseUrl}/{createdRole.Id}/users?Offset={offset}&Limit={limit}");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<PaginatedResult<UserListDto>>();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(createDto.UserIds.Length, result.Total);
            Assert.AreEqual(limit, result.Limit);
            Assert.AreEqual(offset, result.Offset);
            
            // Verify the users match
            var resultUserIds = result.Items.Select(u => u.Id).ToList();
            CollectionAssert.AreEquivalent(createDto.UserIds, resultUserIds);
        }

        [TestMethod]
        public async Task GetUsers_WithNonExistingRoleId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingRoleId = 999999;

            // Act
            var response = await _client.GetAsync($"{BaseUrl}/{nonExistingRoleId}/users?Offset=0&Limit=10");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion

        #region UpdateAsync

        [TestMethod]
        public async Task Update_WithValidData_UpdatesRole()
        {
            // Arrange - Create a role
            var createDto = new RoleCreateDto
            {
                TenantId = null,
                Name = $"{RoleNamePrefix}{Guid.NewGuid():N}",
                Description = "Test Role Description",
                Permissions = [],
                UserIds = []
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdRole = await createResponse.Content.ReadFromJsonAsync<RoleDto>();
            Assert.IsNotNull(createdRole);

            // Arrange - Prepare update data
            var updateDto = new RoleUpdateDto
            {
                Name = $"{RoleNamePrefix}{Guid.NewGuid():N}_updated",
                Description = "Updated Role Description",
                AppendPermissions = [],
                RemovePermissions = [],
                AppendUserIds = [],
                RemoveUserIds = []
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{createdRole.Id}", updateDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<RoleDto>();
            Assert.IsNotNull(result);
            Assert.AreEqual(createdRole.Id, result.Id);
            Assert.AreEqual(updateDto.Name, result.Name);
            Assert.AreEqual(updateDto.Description, result.Description);
            
            // Verify in database
            var updatedRole = GetRoleByName(updateDto.Name);
            Assert.AreEqual(updateDto.Name, updatedRole.Name);
            Assert.AreEqual(updateDto.Description, updatedRole.Description);
        }

        [TestMethod]
        public async Task Update_WithValidPermissions_UpdatesRole()
        {
            // Arrange - Create a role with initial permissions
            var createDto = new RoleCreateDto
            {
                TenantId = null,
                Name = $"{RoleNamePrefix}{Guid.NewGuid():N}",
                Description = "Test Role Description",
                Permissions = [AppPermission.Users_Read],
                UserIds = []
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdRole = await createResponse.Content.ReadFromJsonAsync<RoleDto>();
            Assert.IsNotNull(createdRole);

            // Arrange - Prepare update with different permissions (add new permissions)
            var updateDto = new RoleUpdateDto
            {
                Name = createdRole.Name,
                Description = createdRole.Description,
                AppendPermissions = [AppPermission.Roles_Read, AppPermission.Roles_Write],
                RemovePermissions = [],
                AppendUserIds = [],
                RemoveUserIds = []
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{createdRole.Id}", updateDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<RoleDto>();
            Assert.IsNotNull(result);
            
            // Verify permissions in database - should have initial permission + appended permissions
            var updatedRole = GetRoleByName(createdRole.Name);
            var expectedPermissions = new[] { AppPermission.Users_Read, AppPermission.Roles_Read, AppPermission.Roles_Write };
            CollectionAssert.AreEquivalent(expectedPermissions, updatedRole.Permissions);
        }

        [TestMethod]
        public async Task Update_WithValidUsers_UpdatesRole()
        {
            // Arrange - Create a role with initial users
            var createDto = new RoleCreateDto
            {
                TenantId = null,
                Name = $"{RoleNamePrefix}{Guid.NewGuid():N}",
                Description = "Test Role Description",
                Permissions = [],
                UserIds = [_adminUser.Id]
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdRole = await createResponse.Content.ReadFromJsonAsync<RoleDto>();
            Assert.IsNotNull(createdRole);

            // Arrange - Prepare update to remove the admin user
            var updateDto = new RoleUpdateDto
            {
                Name = createdRole.Name,
                Description = createdRole.Description,
                AppendPermissions = [],
                RemovePermissions = [],
                AppendUserIds = [],
                RemoveUserIds = [_adminUser.Id]
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{createdRole.Id}", updateDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<RoleDto>();
            Assert.IsNotNull(result);
            
            // Verify users in database - should have no users after removal
            var updatedRole = GetRoleByName(createdRole.Name);
            Assert.AreEqual(0, updatedRole.Users.Count);
        }

        [TestMethod]
        public async Task Update_WithNonExistingRoleId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingRoleId = 999999;
            var updateDto = new RoleUpdateDto
            {
                Name = "nonexistingrole",
                Description = "Test Role Description",
                AppendPermissions = [],
                RemovePermissions = [],
                AppendUserIds = [],
                RemoveUserIds = []
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{nonExistingRoleId}", updateDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WithEmptyName_ReturnsBadRequest()
        {
            // Arrange - Create a role
            var createDto = new RoleCreateDto
            {
                TenantId = null,
                Name = $"{RoleNamePrefix}{Guid.NewGuid():N}",
                Description = "Test Role Description",
                Permissions = [],
                UserIds = []
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdRole = await createResponse.Content.ReadFromJsonAsync<RoleDto>();
            Assert.IsNotNull(createdRole);

            // Arrange - Prepare update with empty name
            var updateDto = new RoleUpdateDto
            {
                Name = "", // Empty name
                Description = "Test Role Description",
                AppendPermissions = [],
                RemovePermissions = [],
                AppendUserIds = [],
                RemoveUserIds = []
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{createdRole.Id}", updateDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WithDuplicateName_ReturnsBadRequest()
        {
            // Arrange - Create first role
            var firstRoleDto = new RoleCreateDto
            {
                TenantId = null,
                Name = $"{RoleNamePrefix}{Guid.NewGuid():N}",
                Description = "First Test Role",
                Permissions = [],
                UserIds = []
            };

            var firstResponse = await _client.PostAsJsonAsync(BaseUrl, firstRoleDto);
            firstResponse.EnsureSuccessStatusCode();
            var firstRole = await firstResponse.Content.ReadFromJsonAsync<RoleDto>();
            Assert.IsNotNull(firstRole);

            // Arrange - Create second role
            var secondRoleDto = new RoleCreateDto
            {
                TenantId = null,
                Name = $"{RoleNamePrefix}{Guid.NewGuid():N}",
                Description = "Second Test Role",
                Permissions = [],
                UserIds = []
            };

            var secondResponse = await _client.PostAsJsonAsync(BaseUrl, secondRoleDto);
            secondResponse.EnsureSuccessStatusCode();
            var secondRole = await secondResponse.Content.ReadFromJsonAsync<RoleDto>();
            Assert.IsNotNull(secondRole);

            // Arrange - Try to update second role with first role's name
            var updateDto = new RoleUpdateDto
            {
                Name = firstRole.Name, // Duplicate name
                Description = "Second Test Role",
                AppendPermissions = [],
                RemovePermissions = [],
                AppendUserIds = [],
                RemoveUserIds = []
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{secondRole.Id}", updateDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WithDefaultAdminRole_ReturnConflict()
        {
            // Arrange
            var updateDto = new RoleUpdateDto
            {
                Name = _adminRole.Name,
                Description = "Updated Admin Role",
                AppendPermissions = [],
                RemovePermissions = [],
                AppendUserIds = [],
                RemoveUserIds = []
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{_adminRole.Id}", updateDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.Conflict, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WithInvalidPermission_ReturnsBadRequest()
        {
            // Arrange - Create a role
            var createDto = new RoleCreateDto
            {
                TenantId = null,
                Name = $"{RoleNamePrefix}{Guid.NewGuid():N}",
                Description = "Test Role Description",
                Permissions = [],
                UserIds = []
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdRole = await createResponse.Content.ReadFromJsonAsync<RoleDto>();
            Assert.IsNotNull(createdRole);

            // Arrange - Prepare update with invalid permission
            var updateDto = new RoleUpdateDto
            {
                Name = createdRole.Name,
                Description = createdRole.Description,
                AppendPermissions = [(AppPermission)99999], // Invalid permission value
                RemovePermissions = [],
                AppendUserIds = [],
                RemoveUserIds = []
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{createdRole.Id}", updateDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region DeleteAsync

        [TestMethod]
        public async Task Delete_WithExistingRoleId_DeletesRole()
        {
            // Arrange - Create a role to delete
            var createDto = new RoleCreateDto
            {
                TenantId = null,
                Name = $"{RoleNamePrefix}{Guid.NewGuid():N}",
                Description = "Test Role Description",
                Permissions = [],
                UserIds = []
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdRole = await createResponse.Content.ReadFromJsonAsync<RoleDto>();
            Assert.IsNotNull(createdRole);

            // Act
            var response = await _client.DeleteAsync($"{BaseUrl}/{createdRole.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            
            // Verify role is soft deleted in database
            var dbContext = GetDbContext();
            var deletedRole = await dbContext.AppRoles.IgnoreQueryFilters()
                .FirstOrDefaultAsync(r => r.Id == createdRole.Id);
            Assert.IsNotNull(deletedRole);
            Assert.IsTrue(deletedRole.IsDeleted);
        }

        [TestMethod]
        public async Task Delete_WithNonExistingRoleId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingRoleId = 999999;

            // Act
            var response = await _client.DeleteAsync($"{BaseUrl}/{nonExistingRoleId}");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_WithDefaultAdminRole_ReturnConflict()
        {
            // Act
            var response = await _client.DeleteAsync($"{BaseUrl}/{_adminRole.Id}");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.Conflict, response.StatusCode);
        }

        #endregion
    }
}
