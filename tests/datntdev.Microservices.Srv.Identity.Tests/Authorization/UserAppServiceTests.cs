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
    public class UserAppServiceTests : SrvIdentityTestBase
    {
        public const string UsernamePrefix = "testuser_";
        public const string BaseUrl = "/api/users";

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
        public async Task Create_WithValidData_ReturnsNewUser()
        {
            // Arrange
            var createDto = new UserCreateDto
            {
                Username = $"{UsernamePrefix}{Guid.NewGuid():N}",
                EmailAddress = $"{UsernamePrefix}{Guid.NewGuid():N}@test.com",
                Password = "Test@123456",
                FirstName = "Test",
                LastName = "User",
                Permissions = [AppPermission.Users_Read],
                RoleIds = [_adminRole.Id]
            };

            // Act
            var response = await _client.PostAsJsonAsync(BaseUrl, createDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(createDto.Username, result.Username);
            Assert.AreEqual(createDto.EmailAddress, result.EmailAddress);
            Assert.AreEqual(createDto.FirstName, result.FirstName);
            Assert.AreEqual(createDto.LastName, result.LastName);
            Assert.IsNotNull(result.CreatedAt);

            var createdUser = GetUserByName(createDto.Username);
            Assert.AreEqual(createDto.Username, createdUser.Username);
            Assert.AreEqual(createDto.EmailAddress, createdUser.EmailAddress);
            Assert.AreEqual(createDto.FirstName, createdUser.FirstName);
            Assert.AreEqual(createDto.LastName, createdUser.LastName);
            CollectionAssert.AreEquivalent(createDto.Permissions, createdUser.Permissions);
            CollectionAssert.AreEquivalent(createDto.RoleIds, createdUser.Roles.Select(r => r.Id).ToArray());
        }

        [TestMethod]
        public async Task Create_WithInvalidEmail_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new UserCreateDto
            {
                Username = $"{UsernamePrefix}{Guid.NewGuid():N}",
                EmailAddress = "invalid-email-format", // Invalid email format
                Password = "Test@123456",
                FirstName = "Test",
                LastName = "User",
                Permissions = [],
                RoleIds = []
            };

            // Act
            var response = await _client.PostAsJsonAsync(BaseUrl, createDto);

            // Assert
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Create_WithDuplicateEmail_ReturnBadRequest()
        {
            // Arrange - Create first user
            var firstUsername = $"{UsernamePrefix}{Guid.NewGuid():N}";
            var duplicateEmail = $"{UsernamePrefix}{Guid.NewGuid():N}@test.com";
            
            var firstUserDto = new UserCreateDto
            {
                Username = firstUsername,
                EmailAddress = duplicateEmail,
                Password = "Test@123456",
                FirstName = "First",
                LastName = "User",
                Permissions = [],
                RoleIds = []
            };

            var firstResponse = await _client.PostAsJsonAsync(BaseUrl, firstUserDto);
            firstResponse.EnsureSuccessStatusCode();

            // Arrange - Create second user with duplicate email
            var secondUserDto = new UserCreateDto
            {
                Username = $"{UsernamePrefix}{Guid.NewGuid():N}",
                EmailAddress = duplicateEmail, // Duplicate email
                Password = "Test@123456",
                FirstName = "Second",
                LastName = "User",
                Permissions = [],
                RoleIds = []
            };

            // Act
            var response = await _client.PostAsJsonAsync(BaseUrl, secondUserDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Create_WithInvalidPermission_ReturnBadRequest()
        {
            // Arrange
            var createDto = new UserCreateDto
            {
                Username = $"{UsernamePrefix}{Guid.NewGuid():N}",
                EmailAddress = $"{UsernamePrefix}{Guid.NewGuid():N}@test.com",
                Password = "Test@123456",
                FirstName = "Test",
                LastName = "User",
                Permissions = [(AppPermission)99999], // Invalid permission value
                RoleIds = []
            };

            // Act
            var response = await _client.PostAsJsonAsync(BaseUrl, createDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region GetAsync & GetAllAsync

        [TestMethod]
        public async Task Get_WithExistingUserId_ReturnsUser()
        {
            // Arrange - Create a user
            var createDto = new UserCreateDto
            {
                Username = $"{UsernamePrefix}{Guid.NewGuid():N}",
                EmailAddress = $"{UsernamePrefix}{Guid.NewGuid():N}@test.com",
                Password = "Test@123456",
                FirstName = "Test",
                LastName = "User",
                Permissions = [AppPermission.Users_Read],
                RoleIds = [_adminRole.Id]
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();
            Assert.IsNotNull(createdUser);

            // Act
            var response = await _client.GetAsync($"{BaseUrl}/{createdUser.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.IsNotNull(result);
            Assert.AreEqual(createdUser.Id, result.Id);
            Assert.AreEqual(createDto.Username, result.Username);
            Assert.AreEqual(createDto.EmailAddress, result.EmailAddress);
            Assert.AreEqual(createDto.FirstName, result.FirstName);
            Assert.AreEqual(createDto.LastName, result.LastName);
        }

        [TestMethod]
        public async Task Get_WithNonExistingUserId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingUserId = 999999L;

            // Act
            var response = await _client.GetAsync($"{BaseUrl}/{nonExistingUserId}");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task GetAll_WithPagination_ReturnsPaginatedResult()
        {
            // Arrange - Create multiple test users
            var usersToCreate = 3;
            var createdUserIds = new List<long>();

            for (int i = 0; i < usersToCreate; i++)
            {
                var createDto = new UserCreateDto
                {
                    Username = $"{UsernamePrefix}{Guid.NewGuid():N}",
                    EmailAddress = $"{UsernamePrefix}{Guid.NewGuid():N}@test.com",
                    Password = "Test@123456",
                    FirstName = $"Test{i}",
                    LastName = $"User{i}",
                    Permissions = [],
                    RoleIds = []
                };

                var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
                createResponse.EnsureSuccessStatusCode();
                var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();
                Assert.IsNotNull(createdUser);
                createdUserIds.Add(createdUser.Id);
            }

            // Act
            var offset = 0;
            var limit = 10;
            var response = await _client.GetAsync($"{BaseUrl}?Offset={offset}&Limit={limit}");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<PaginatedResult<UserListDto>>();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
            Assert.IsTrue(result.Total >= usersToCreate); // At least the users we created (plus admin user)
            Assert.AreEqual(limit, result.Limit);
            Assert.AreEqual(offset, result.Offset);
            
            // Verify our created users are in the result
            var resultUserIds = result.Items.Select(u => u.Id).ToList();
            foreach (var createdUserId in createdUserIds)
            {
                Assert.IsTrue(resultUserIds.Contains(createdUserId), 
                    $"Created user with ID {createdUserId} should be in the paginated result");
            }
        }

        #endregion

        #region GetRolesAsync

        [TestMethod]
        public async Task GetRoles_WithExistingUserId_ReturnsRoles()
        {
            // Arrange - Create a user with roles
            var createDto = new UserCreateDto
            {
                Username = $"{UsernamePrefix}{Guid.NewGuid():N}",
                EmailAddress = $"{UsernamePrefix}{Guid.NewGuid():N}@test.com",
                Password = "Test@123456",
                FirstName = "Test",
                LastName = "User",
                Permissions = [],
                RoleIds = [_adminRole.Id]
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();
            Assert.IsNotNull(createdUser);

            // Act
            var offset = 0;
            var limit = 10;
            var response = await _client.GetAsync($"{BaseUrl}/{createdUser.Id}/roles?Offset={offset}&Limit={limit}");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<PaginatedResult<RoleListDto>>();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(createDto.RoleIds.Length, result.Total);
            Assert.AreEqual(limit, result.Limit);
            Assert.AreEqual(offset, result.Offset);
            
            // Verify the roles match
            var resultRoleIds = result.Items.Select(r => r.Id).ToList();
            CollectionAssert.AreEquivalent(createDto.RoleIds, resultRoleIds);
        }

        [TestMethod]
        public async Task GetRoles_WithNonExistingUserId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingUserId = 999999L;

            // Act
            var response = await _client.GetAsync($"{BaseUrl}/{nonExistingUserId}/roles?Offset=0&Limit=10");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion

        #region UpdateAsync

        [TestMethod]
        public async Task Update_WithValidData_UpdatesUser()
        {
            // Arrange - Create a user
            var createDto = new UserCreateDto
            {
                Username = $"{UsernamePrefix}{Guid.NewGuid():N}",
                EmailAddress = $"{UsernamePrefix}{Guid.NewGuid():N}@test.com",
                Password = "Test@123456",
                FirstName = "Test",
                LastName = "User",
                Permissions = [],
                RoleIds = []
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();
            Assert.IsNotNull(createdUser);

            // Arrange - Prepare update data
            var updateDto = new UserUpdateDto
            {
                Username = $"{UsernamePrefix}{Guid.NewGuid():N}_updated",
                EmailAddress = $"{UsernamePrefix}{Guid.NewGuid():N}_updated@test.com",
                FirstName = "Updated",
                LastName = "UserName",
                AppendPermissions = [],
                RemovePermissions = [],
                AppendRoleIds = [],
                RemoveRoleIds = []
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{createdUser.Id}", updateDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.IsNotNull(result);
            Assert.AreEqual(createdUser.Id, result.Id);
            Assert.AreEqual(updateDto.EmailAddress, result.EmailAddress);
            Assert.AreEqual(updateDto.FirstName, result.FirstName);
            Assert.AreEqual(updateDto.LastName, result.LastName);
            
            // Verify in database
            var updatedUser = GetUserByName(updateDto.Username);
            Assert.AreEqual(updateDto.EmailAddress, updatedUser.EmailAddress);
            Assert.AreEqual(updateDto.FirstName, updatedUser.FirstName);
            Assert.AreEqual(updateDto.LastName, updatedUser.LastName);
        }

        [TestMethod]
        public async Task Update_WithValidPermissions_UpdatesUser()
        {
            // Arrange - Create a user with initial permissions
            var createDto = new UserCreateDto
            {
                Username = $"{UsernamePrefix}{Guid.NewGuid():N}",
                EmailAddress = $"{UsernamePrefix}{Guid.NewGuid():N}@test.com",
                Password = "Test@123456",
                FirstName = "Test",
                LastName = "User",
                Permissions = [AppPermission.Users_Read],
                RoleIds = []
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();
            Assert.IsNotNull(createdUser);

            // Arrange - Prepare update with different permissions (add new permissions)
            var updateDto = new UserUpdateDto
            {
                Username = createdUser.Username,
                EmailAddress = createdUser.EmailAddress,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                AppendPermissions = [AppPermission.Roles_Read, AppPermission.Roles_Write],
                RemovePermissions = [],
                AppendRoleIds = [],
                RemoveRoleIds = []
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{createdUser.Id}", updateDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.IsNotNull(result);
            
            // Verify permissions in database - should have initial permission + appended permissions
            var updatedUser = GetUserByName(createdUser.Username);
            var expectedPermissions = new[] { AppPermission.Users_Read, AppPermission.Roles_Read, AppPermission.Roles_Write };
            CollectionAssert.AreEquivalent(expectedPermissions, updatedUser.Permissions);
        }

        [TestMethod]
        public async Task Update_WithValidRoles_UpdatesUser()
        {
            // Arrange - Create a user with initial roles
            var createDto = new UserCreateDto
            {
                Username = $"{UsernamePrefix}{Guid.NewGuid():N}",
                EmailAddress = $"{UsernamePrefix}{Guid.NewGuid():N}@test.com",
                Password = "Test@123456",
                FirstName = "Test",
                LastName = "User",
                Permissions = [],
                RoleIds = [_adminRole.Id]
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();
            Assert.IsNotNull(createdUser);

            // Arrange - Prepare update to remove the admin role
            var updateDto = new UserUpdateDto
            {
                Username = createdUser.Username,
                EmailAddress = createdUser.EmailAddress,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                AppendPermissions = [],
                RemovePermissions = [],
                AppendRoleIds = [],
                RemoveRoleIds = [_adminRole.Id]
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{createdUser.Id}", updateDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.IsNotNull(result);
            
            // Verify roles in database - should have no roles after removal
            var updatedUser = GetUserByName(createdUser.Username);
            Assert.AreEqual(0, updatedUser.Roles.Count);
        }

        [TestMethod]
        public async Task Update_WithNonExistingUserId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingUserId = 999999L;
            var updateDto = new UserUpdateDto
            {
                Username = "nonexistinguser",
                EmailAddress = $"{UsernamePrefix}{Guid.NewGuid():N}@test.com",
                FirstName = "Test",
                LastName = "User",
                AppendPermissions = [],
                RemovePermissions = [],
                AppendRoleIds = [],
                RemoveRoleIds = []
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{nonExistingUserId}", updateDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WithInvalidEmail_ReturnsBadRequest()
        {
            // Arrange - Create a user
            var createDto = new UserCreateDto
            {
                Username = $"{UsernamePrefix}{Guid.NewGuid():N}",
                EmailAddress = $"{UsernamePrefix}{Guid.NewGuid():N}@test.com",
                Password = "Test@123456",
                FirstName = "Test",
                LastName = "User",
                Permissions = [],
                RoleIds = []
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();
            Assert.IsNotNull(createdUser);

            // Arrange - Prepare update with invalid email
            var updateDto = new UserUpdateDto
            {
                Username = createdUser.Username,
                EmailAddress = "invalid-email-format", // Invalid email format
                FirstName = "Test",
                LastName = "User",
                AppendPermissions = [],
                RemovePermissions = [],
                AppendRoleIds = [],
                RemoveRoleIds = []
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{createdUser.Id}", updateDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WithDuplicateEmail_ReturnsBadRequest()
        {
            // Arrange - Create first user
            var firstUserDto = new UserCreateDto
            {
                Username = $"{UsernamePrefix}{Guid.NewGuid():N}",
                EmailAddress = $"{UsernamePrefix}{Guid.NewGuid():N}@test.com",
                Password = "Test@123456",
                FirstName = "First",
                LastName = "User",
                Permissions = [],
                RoleIds = []
            };

            var firstResponse = await _client.PostAsJsonAsync(BaseUrl, firstUserDto);
            firstResponse.EnsureSuccessStatusCode();
            var firstUser = await firstResponse.Content.ReadFromJsonAsync<UserDto>();
            Assert.IsNotNull(firstUser);

            // Arrange - Create second user
            var secondUserDto = new UserCreateDto
            {
                Username = $"{UsernamePrefix}{Guid.NewGuid():N}",
                EmailAddress = $"{UsernamePrefix}{Guid.NewGuid():N}@test.com",
                Password = "Test@123456",
                FirstName = "Second",
                LastName = "User",
                Permissions = [],
                RoleIds = []
            };

            var secondResponse = await _client.PostAsJsonAsync(BaseUrl, secondUserDto);
            secondResponse.EnsureSuccessStatusCode();
            var secondUser = await secondResponse.Content.ReadFromJsonAsync<UserDto>();
            Assert.IsNotNull(secondUser);

            // Arrange - Try to update second user with first user's email
            var updateDto = new UserUpdateDto
            {
                Username = firstUserDto.Username,
                EmailAddress = firstUser.EmailAddress, // Duplicate email
                FirstName = "Second",
                LastName = "User",
                AppendPermissions = [],
                RemovePermissions = [],
                AppendRoleIds = [],
                RemoveRoleIds = []
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{secondUser.Id}", updateDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WithDefaultAdminUser_ReturnConflict()
        {
            // Arrange
            var updateDto = new UserUpdateDto
            {
                Username = _adminUser.Username,
                EmailAddress = $"{UsernamePrefix}{Guid.NewGuid():N}@test.com",
                FirstName = "Updated",
                LastName = "Admin",
                AppendPermissions = [],
                RemovePermissions = [],
                AppendRoleIds = [],
                RemoveRoleIds = []
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{_adminUser.Id}", updateDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.Conflict, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WithInvalidPermission_ReturnsBadRequest()
        {
            // Arrange - Create a user
            var createDto = new UserCreateDto
            {
                Username = $"{UsernamePrefix}{Guid.NewGuid():N}",
                EmailAddress = $"{UsernamePrefix}{Guid.NewGuid():N}@test.com",
                Password = "Test@123456",
                FirstName = "Test",
                LastName = "User",
                Permissions = [],
                RoleIds = []
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();
            Assert.IsNotNull(createdUser);

            // Arrange - Prepare update with invalid permission
            var updateDto = new UserUpdateDto
            {
                Username = createdUser.Username,
                EmailAddress = createdUser.EmailAddress,
                FirstName = "Test",
                LastName = "User",
                AppendPermissions = [(AppPermission)99999], // Invalid permission value
                RemovePermissions = [],
                AppendRoleIds = [],
                RemoveRoleIds = []
            };

            // Act
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{createdUser.Id}", updateDto);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region DeleteAsync

        [TestMethod]
        public async Task Delete_WithExistingUserId_DeletesUser()
        {
            // Arrange - Create a user to delete
            var createDto = new UserCreateDto
            {
                Username = $"{UsernamePrefix}{Guid.NewGuid():N}",
                EmailAddress = $"{UsernamePrefix}{Guid.NewGuid():N}@test.com",
                Password = "Test@123456",
                FirstName = "Test",
                LastName = "User",
                Permissions = [],
                RoleIds = []
            };

            var createResponse = await _client.PostAsJsonAsync(BaseUrl, createDto);
            createResponse.EnsureSuccessStatusCode();
            var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();
            Assert.IsNotNull(createdUser);

            // Act
            var response = await _client.DeleteAsync($"{BaseUrl}/{createdUser.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            
            // Verify user is soft deleted in database
            var dbContext = GetDbContext();
            var deletedUser = await dbContext.AppUsers.IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == createdUser.Id);
            Assert.IsNotNull(deletedUser);
            Assert.IsTrue(deletedUser.IsDeleted);
        }

        [TestMethod]
        public async Task Delete_WithNonExistingUserId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingUserId = 999999L;

            // Act
            var response = await _client.DeleteAsync($"{BaseUrl}/{nonExistingUserId}");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_WithDefaultAdminUser_ReturnConflict()
        {
            // Act
            var response = await _client.DeleteAsync($"{BaseUrl}/{_adminUser.Id}");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.Conflict, response.StatusCode);
        }

        #endregion
    }
}
