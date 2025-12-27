using System.Net.Http.Json;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Permissions.Dto;
using static datntdev.Microservices.Common.Constants.Enum;

namespace datntdev.Microservices.Srv.Identity.Authorization
{
    [TestClass]
    public class PermissionAppServiceTests : SrvIdentityTestBase
    {
        public const string BaseUrl = "/api/permissions";

        #region GetAllAsync

        [TestMethod]
        public async Task GetAll_ReturnsAllPermissions()
        {
            // Act
            var response = await _client.GetAsync(BaseUrl);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<PermissionDto>>();
            Assert.IsNotNull(result);
            
            var resultList = result.ToList();
            Assert.IsTrue(resultList.Count > 0, "Should return at least one permission");
            
            // Verify that all permissions have required properties
            foreach (var permission in resultList)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(permission.PermissionName), "Permission name should not be empty");
                Assert.IsTrue(Enum.IsDefined(typeof(AppPermission), permission.Permission), "Permission should be a valid AppPermission enum value");
            }
        }

        [TestMethod]
        public async Task GetAll_ReturnsExpectedPermissionStructure()
        {
            // Act
            var response = await _client.GetAsync(BaseUrl);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<PermissionDto>>();
            Assert.IsNotNull(result);
            
            var resultList = result.ToList();
            
            // Verify some known permissions exist
            var expectedPermissions = new[]
            {
                AppPermission.Users_Read,
                AppPermission.Users_Write,
                AppPermission.Roles_Read,
                AppPermission.Roles_Write
            };
            
            foreach (var expectedPermission in expectedPermissions)
            {
                var found = resultList.Any(p => p.Permission == expectedPermission);
                Assert.IsTrue(found, $"Expected permission {expectedPermission} should be in the result");
            }
        }

        [TestMethod]
        public async Task GetAll_ReturnsPermissionsWithCorrectHierarchy()
        {
            // Act
            var response = await _client.GetAsync(BaseUrl);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<PermissionDto>>();
            Assert.IsNotNull(result);
            
            var resultList = result.ToList();
            
            // Verify that child permissions have correct parent relationships
            // For example, Users_Read should have Users as parent, or None if it's a root permission
            var usersReadPermission = resultList.FirstOrDefault(p => p.Permission == AppPermission.Users_Read);
            if (usersReadPermission != null)
            {
                // The parent should either be None (root) or a valid parent permission
                Assert.IsTrue(
                    usersReadPermission.Parent == AppPermission.None || 
                    Enum.IsDefined(typeof(AppPermission), usersReadPermission.Parent),
                    "Parent permission should be None or a valid AppPermission"
                );
            }
        }

        [TestMethod]
        public async Task GetAll_ReturnsConsistentResults()
        {
            // Act - Call the endpoint twice
            var response1 = await _client.GetAsync(BaseUrl);
            var response2 = await _client.GetAsync(BaseUrl);

            // Assert
            response1.EnsureSuccessStatusCode();
            response2.EnsureSuccessStatusCode();
            
            var result1 = await response1.Content.ReadFromJsonAsync<IEnumerable<PermissionDto>>();
            var result2 = await response2.Content.ReadFromJsonAsync<IEnumerable<PermissionDto>>();
            
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            
            var list1 = result1.ToList();
            var list2 = result2.ToList();
            
            // Both calls should return the same number of permissions
            Assert.AreEqual(list1.Count, list2.Count, "Multiple calls should return the same number of permissions");
            
            // Verify that the same permissions are returned
            var permissions1 = list1.Select(p => p.Permission).OrderBy(p => p).ToList();
            var permissions2 = list2.Select(p => p.Permission).OrderBy(p => p).ToList();
            
            CollectionAssert.AreEqual(permissions1, permissions2, "Multiple calls should return the same permissions");
        }

        [TestMethod]
        public async Task GetAll_ReturnsUniquePermissions()
        {
            // Act
            var response = await _client.GetAsync(BaseUrl);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<PermissionDto>>();
            Assert.IsNotNull(result);
            
            var resultList = result.ToList();
            var permissionValues = resultList.Select(p => p.Permission).ToList();
            
            // Check that there are no duplicate permissions
            var distinctPermissions = permissionValues.Distinct().ToList();
            Assert.AreEqual(distinctPermissions.Count, permissionValues.Count, "All permissions should be unique");
        }

        [TestMethod]
        public async Task GetAll_ReturnsNonNonePermissions()
        {
            // Act
            var response = await _client.GetAsync(BaseUrl);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<PermissionDto>>();
            Assert.IsNotNull(result);
            
            var resultList = result.ToList();
            
            // Verify that AppPermission.None is not in the returned permissions
            // (assuming None is used as a placeholder/default and shouldn't be a real permission)
            var nonePermissions = resultList.Where(p => p.Permission == AppPermission.None).ToList();
            Assert.AreEqual(0, nonePermissions.Count, "AppPermission.None should not be in the list of actual permissions");
        }

        #endregion
    }
}