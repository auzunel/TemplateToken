﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TemplateToken.Business.Security
{
    public class CustomIdentity : IIdentity
    {
        private readonly bool _isAuthenticated;
        private readonly string _name;

        public CustomIdentity(bool isAuthenticated, string name)
        {
            _isAuthenticated = isAuthenticated;
            _name = name;
        }

        public DateTime? LastPasswordChange { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsLockedOut { get; set; }
        public List<string> Roles { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public bool RememberMe { get; set; }
        public bool IsActive { get; set; }

        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
