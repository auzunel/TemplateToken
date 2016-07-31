﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using TemplateToken.EFRepository;
using DevOne.Security.Cryptography.BCrypt;
using TemplateToken.Business.Security;

namespace TemplateToken.Business.UserManagement
{
    public class UserManagement
    {
        public static CustomIdentity Login(string email, string password)
        {
            using (var lifetimescope = IoCContainer.BeginLifetimeScope())
            using (var repository = lifetimescope.Resolve<IRepository<User>>())
            {
                try
                {
                    var existingUser = repository.GetSingle<User>(x =>
                        x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase) &&
                        x.UserMemberships.Any());

                    if (existingUser != null)
                    {
                        var correctPassword = BCryptHelper.CheckPassword(password, existingUser.UserMemberships.FirstOrDefault().PasswordHash);
                        if (correctPassword)
                            return new CustomIdentity(true, existingUser.Email)
                            {
                                Roles = existingUser.UserRoles.Select(x => x.Role.RoleName).ToList(),
                                IsActive = existingUser.IsActive,
                                Email = existingUser.Email,
                                UserId = existingUser.Id
                            };
                        else
                            return new CustomIdentity(false, String.Empty);
                    }
                    else
                    {
                        //user does not exist 
                        return new CustomIdentity(false, String.Empty);
                    }
                }
                catch (Exception e)
                {
                    return new CustomIdentity(false, String.Empty);
                }
            }
        }

        public static bool Register(string email, string password)
        {
            using (var lifetimescope = IoCContainer.BeginLifetimeScope())
            using (var repository = lifetimescope.Resolve<IRepository<User>>())
            {
                try
                {
                    var user = new TemplateToken.EFRepository.User
                    {
                        Email = "auzunel@hotmail.com",
                        FirstName = "Ali",
                        LastName = "Uzunel",
                        IsActive = true,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = Guid.NewGuid()
                    };

                    var salt = BCryptHelper.GenerateSalt();
                    user.UserMemberships.Add(new TemplateToken.EFRepository.UserMembership
                    {
                        PasswordSalt = salt,
                        PasswordHash = BCryptHelper.HashPassword(password, salt),
                        IsLocked = false,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = Guid.NewGuid()
                    });

                    user.UserRoles.Add(new UserRole
                    {
                        RoleId = 1
                    });

                    repository.Add(user);
                }
                catch (Exception e)
                {
                    throw;
                }
                //catch (DbEntityValidationException e)
                //{
                //    foreach (var eve in e.EntityValidationErrors)
                //    {
                //        foreach (var ve in eve.ValidationErrors)
                //        {
                //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //                ve.PropertyName, ve.ErrorMessage);
                //        }
                //    }
                //    throw;
                //}
            }
            return true;
        }

        public static CustomIdentity Load(string email)
        {
            using (var lifetimescope = IoCContainer.BeginLifetimeScope())
            using (var repository = lifetimescope.Resolve<IRepository<User>>())
            {
                try
                {
                    var existingUser = repository.GetSingle<User>(x =>
                        x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase) &&
                        x.UserMemberships.Any());

                    if (existingUser != null)
                    {
                        return new CustomIdentity(true, existingUser.Email)
                        {
                            Roles = existingUser.UserRoles.Select(x => x.Role.RoleName).ToList(),
                            IsActive = existingUser.IsActive
                        };
                    }
                    else
                    {
                        //user does not exist 
                        return new CustomIdentity(false, String.Empty);
                    }
                }
                catch (Exception e)
                {
                    return new CustomIdentity(false, String.Empty);
                }
            }
        }
    }
}
