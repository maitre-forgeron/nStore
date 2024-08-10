// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer;
using Duende.IdentityServer.Test;
using IdentityModel;
using System.Security.Claims;
using System.Text.Json;

namespace NStore.Identity
{
    public static class TestUsers
    {
        public static List<TestUser> Users
        {
            get
            {
                return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                        Username = "testmanager",
                        Password = "password",
                        Claims = new List<Claim>
                        {
                            new Claim(JwtClaimTypes.GivenName, "test manager"),
                            new Claim(JwtClaimTypes.FamilyName, "test manager"),
                            new Claim(JwtClaimTypes.Role, "Manager")
                        },
                    },
                    new TestUser
                    {
                        SubjectId = "d452edce-e6a5-11ed-a05b-0242ac120003",
                        Username = "testbuyer",
                        Password = "password",
                        Claims = new List<Claim>
                        {
                            new Claim(JwtClaimTypes.GivenName, "test buyer"),
                            new Claim(JwtClaimTypes.FamilyName, "test buyer"),
                            new Claim(JwtClaimTypes.Role, "Buyer")
                        }
                    }
                };
            }
        }
    }
}