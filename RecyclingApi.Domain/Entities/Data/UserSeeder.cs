using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RecyclingApi.Domain.Entities.User;

namespace RecyclingApi.Domain.Entities.Data
{
    public class UserSeeder
    {
        public static async Task SeedUsersAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // 1. 确保角色存在
            var roles = new[] { "Admin", "Staff" , "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. 创建管理员用户
            var adminUser = new ApplicationUser
            {                
                UserName = "admin",
                Email = "admin@example.com",
                RealName = "管理员",
                UserType = UserType.Admin,
                Department = "管理部",
                Position = "系统管理员",
                IsActive = true,
                EmailConfirmed = true
            };
            if (await userManager.FindByNameAsync(adminUser.UserName) == null)
            {

                var user  = await userManager.CreateAsync(adminUser, "leng5201314@"); // 设置初始密码
                if (user != null) { }
                var existingUser = await userManager.FindByNameAsync(adminUser.UserName);
                if (existingUser == null)
                {
                    throw new Exception("用户不存在");
                }

                // 分配角色
                var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                if (!roleResult.Succeeded)
                {
                    throw new Exception("角色分配失败");
                }
            }

            // 3. 创建员工用户
            var staffUser = new ApplicationUser
            {
                UserName = "staff",
                Email = "staff@example.com",
                RealName = "客服小张",
                UserType = UserType.Staff,
                Department = "客服部",
                Position = "客服专员",
                IsActive = true,
                EmailConfirmed = true
            };
            if (await userManager.FindByNameAsync(staffUser.UserName) == null)
            {
                await userManager.CreateAsync(staffUser, "leng5201314@"); // 设置初始密码
                await userManager.AddToRoleAsync(staffUser, "Staff");
            }
        }
    }
}
