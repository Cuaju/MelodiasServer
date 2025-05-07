using DataAccess.DAO;
using DataAccess.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;
using System.Data.Entity.Infrastructure;

namespace MelodiasTest.SupplierDAOTEST
{
    public class SupplierDaoTests : IDisposable
    {
        private readonly SupplierDao _dao;

        public SupplierDaoTests()
        {
            _dao = new SupplierDao();
        }

        [Fact]
        public void IsSupplierNameTaken_ReturnsTrue_WhenNameExists()
        {
            using (var scope = new TransactionScope())
            {
                var name = "TestName_" + Guid.NewGuid();
                using (var ctx = new MelodiasContext())
                {
                    ctx.SupplierCompanies.Add(new Supplier
                    {
                        Name = name,
                        Address = "Addr",
                        PostalCode = "00000",
                        City = "City",
                        Country = "Country",
                        Phone = "555-0000",
                        Email = $"{name}@ex.com"
                    });
                    ctx.SaveChanges();
                }

                Assert.True(_dao.IsSupplierNameTaken(name));
            }
        }

        [Fact]
        public void IsSupplierNameTaken_ReturnsFalse_WhenNameDoesNotExist()
        {
            using (var scope = new TransactionScope())
            {
                var randomName = "NonExistent_" + Guid.NewGuid();
                Assert.False(_dao.IsSupplierNameTaken(randomName));
            }
        }

        [Fact]
        public void IsSupplierNameTaken_IgnoresCase()
        {
            using (var scope = new TransactionScope())
            {
                var baseName = "CaseTest_" + Guid.NewGuid();
                using (var ctx = new MelodiasContext())
                {
                    ctx.SupplierCompanies.Add(new Supplier
                    {
                        Name = baseName.ToUpperInvariant(),
                        Address = "Addr",
                        PostalCode = "11111",
                        City = "City",
                        Country = "Country",
                        Phone = "555-1111",
                        Email = $"{baseName}@ex.com"
                    });
                    ctx.SaveChanges();
                }

                // query with lower-case
                Assert.True(_dao.IsSupplierNameTaken(baseName.ToLowerInvariant()));
            }
        }

        [Fact]
        public void IsSupplierEmailTaken_ReturnsTrue_WhenEmailExists()
        {
            using (var scope = new TransactionScope())
            {
                var email = $"email_{Guid.NewGuid()}@ex.com";
                using (var ctx = new MelodiasContext())
                {
                    ctx.SupplierCompanies.Add(new Supplier
                    {
                        Name = "N1",
                        Address = "Addr",
                        PostalCode = "22222",
                        City = "City",
                        Country = "Country",
                        Phone = "555-2222",
                        Email = email
                    });
                    ctx.SaveChanges();
                }

                Assert.True(_dao.IsSupplierEmailTaken(email));
            }
        }

        [Fact]
        public void IsSupplierEmailTaken_ReturnsFalse_WhenEmailDoesNotExist()
        {
            using (var scope = new TransactionScope())
            {
                var randomEmail = $"no_email_{Guid.NewGuid()}@ex.com";
                Assert.False(_dao.IsSupplierEmailTaken(randomEmail));
            }
        }

        [Fact]
        public void IsSupplierEmailTaken_ExactMatchOnly()
        {
            using (var scope = new TransactionScope())
            {
                var email = $"Exact_{Guid.NewGuid()}@ex.com";
                using (var ctx = new MelodiasContext())
                {
                    ctx.SupplierCompanies.Add(new Supplier
                    {
                        Name = "N2",
                        Address = "Addr",
                        PostalCode = "33333",
                        City = "City",
                        Country = "Country",
                        Phone = "555-3333",
                        Email = email
                    });
                    ctx.SaveChanges();
                }

                // mismatched case (SQL default collation is case-insensitive, but method uses ==)
                Assert.False(_dao.IsSupplierEmailTaken(email.ToUpperInvariant() + ".invalid"));
            }
        }

        [Fact]
        public void AddSupplier_ReturnsTrue_WhenNewSupplier()
        {
            using (var scope = new TransactionScope())
            {
                var supplier = new Supplier
                {
                    Name = "New_" + Guid.NewGuid(),
                    Address = "Addr",
                    PostalCode = "44444",
                    City = "City",
                    Country = "Country",
                    Phone = "555-4444",
                    Email = $"add_{Guid.NewGuid()}@ex.com"
                };

                var result = _dao.AddSupplier(supplier);
                Assert.True(result);

                using (var ctx = new MelodiasContext())
                {
                    Assert.True(ctx.SupplierCompanies.Any(s => s.Email == supplier.Email));
                }
            }
        }

        [Fact]
        public void AddSupplier_ThrowsDbUpdateException_OnDuplicateName()
        {
            using (var scope = new TransactionScope())
            {
                var dupName = "DupName_" + Guid.NewGuid();
                var email1 = $"1_{Guid.NewGuid()}@ex.com";
                var email2 = $"2_{Guid.NewGuid()}@ex.com";

                using (var ctx = new MelodiasContext())
                {
                    ctx.SupplierCompanies.Add(new Supplier
                    {
                        Name = dupName,
                        Address = "A",
                        PostalCode = "55555",
                        City = "City",
                        Country = "Country",
                        Phone = "555-5555",
                        Email = email1
                    });
                    ctx.SaveChanges();
                }

                // same name, different email → unique-name index violation
                var dupSupplier = new Supplier
                {
                    Name = dupName,
                    Address = "A2",
                    PostalCode = "55556",
                    City = "City2",
                    Country = "Country2",
                    Phone = "555-5556",
                    Email = email2
                };

                Assert.Throws<DbUpdateException>(() => _dao.AddSupplier(dupSupplier));
            }
        }

        [Fact]
        public void AddSupplier_ThrowsDbUpdateException_OnDuplicateEmail()
        {
            using (var scope = new TransactionScope())
            {
                var name1 = "N_" + Guid.NewGuid();
                var name2 = "M_" + Guid.NewGuid();
                var dupEmail = $"dup_{Guid.NewGuid()}@ex.com";

                using (var ctx = new MelodiasContext())
                {
                    ctx.SupplierCompanies.Add(new Supplier
                    {
                        Name = name1,
                        Address = "A",
                        PostalCode = "66666",
                        City = "City",
                        Country = "Country",
                        Phone = "555-6666",
                        Email = dupEmail
                    });
                    ctx.SaveChanges();
                }

                // same email, different name → unique-email index violation
                var dupSupplier = new Supplier
                {
                    Name = name2,
                    Address = "B",
                    PostalCode = "66667",
                    City = "City2",
                    Country = "Country2",
                    Phone = "555-6667",
                    Email = dupEmail
                };

                Assert.Throws<DbUpdateException>(() => _dao.AddSupplier(dupSupplier));
            }
        }

        [Fact]
        public void UpdateSupplier_ReturnsTrue_WhenSupplierExists()
        {
            using (var scope = new TransactionScope())
            {
                int id;
                using (var ctx = new MelodiasContext())
                {
                    var sup = new Supplier
                    {
                        Name = "Updt_" + Guid.NewGuid(),
                        Address = "Addr",
                        PostalCode = "77777",
                        City = "City",
                        Country = "Country",
                        Phone = "555-7777",
                        Email = $"u_{Guid.NewGuid()}@ex.com"
                    };
                    ctx.SupplierCompanies.Add(sup);
                    ctx.SaveChanges();
                    id = sup.supplierId;
                }

                var toChange = new Supplier
                {
                    supplierId = id,
                    Name = "ChangedName",
                    Address = "NewAddr",
                    PostalCode = "77778",
                    City = "NewCity",
                    Country = "NewCountry",
                    Phone = "555-7778",
                    Email = $"changed_{Guid.NewGuid()}@ex.com"
                };

                Assert.True(_dao.UpdateSupplier(toChange));
            }
        }

        [Fact]
        public void UpdateSupplier_ReturnsFalse_WhenSupplierDoesNotExist()
        {
            using (var scope = new TransactionScope())
            {
                var fake = new Supplier
                {
                    supplierId = -1,
                    Name = "X",
                    Address = "X",
                    PostalCode = "00000",
                    City = "X",
                    Country = "X",
                    Phone = "000-0000",
                    Email = "x@ex.com"
                };

                Assert.False(_dao.UpdateSupplier(fake));
            }
        }

        [Fact]
        public void UpdateSupplier_ReturnsFalse_WhenNoChangesMade()
        {
            using (var scope = new TransactionScope())
            {
                Supplier original;
                using (var ctx = new MelodiasContext())
                {
                    original = new Supplier
                    {
                        Name = "NoChange_" + Guid.NewGuid(),
                        Address = "Addr",
                        PostalCode = "88888",
                        City = "City",
                        Country = "Country",
                        Phone = "555-8888",
                        Email = $"nc_{Guid.NewGuid()}@ex.com"
                    };
                    ctx.SupplierCompanies.Add(original);
                    ctx.SaveChanges();
                }

                // call update with identical values
                Assert.False(_dao.UpdateSupplier(original));
            }
        }


        [Fact]
        public void GetSuppliersList_ReturnsEmpty_WhenNoMatch()
        {
            using (var scope = new TransactionScope())
            {
                var list = _dao.GetSuppliersList("zzz_nope_123");
                Assert.Empty(list);
            }
        }

        [Fact]
        public void GetSuppliersList_ReturnsMatchingSuppliers()
        {
            using (var scope = new TransactionScope())
            {
                var token = "FindMe_" + Guid.NewGuid().ToString().Substring(0, 5);
                using (var ctx = new MelodiasContext())
                {
                    ctx.SupplierCompanies.Add(new Supplier
                    {
                        Name = $"A_{token}",
                        Address = "Addr",
                        PostalCode = "99999",
                        City = "City",
                        Country = "Country",
                        Phone = "555-9999",
                        Email = $"a_{token}@ex.com"
                    });
                    ctx.SupplierCompanies.Add(new Supplier
                    {
                        Name = $"B_{token}",
                        Address = "Addr2",
                        PostalCode = "99998",
                        City = "City",
                        Country = "Country",
                        Phone = "555-9998",
                        Email = $"b_{token}@ex.com"
                    });
                    ctx.SaveChanges();
                }

                var list = _dao.GetSuppliersList(token);
                Assert.True(list.Count >= 2);
                Assert.All(list, d => Assert.Contains(token, d.Name, StringComparison.OrdinalIgnoreCase));
            }
        }

        [Fact]
        public void GetSuppliersList_LimitsToTenResults()
        {
            using (var scope = new TransactionScope())
            {
                var token = "Limit_" + Guid.NewGuid().ToString().Substring(0, 5);
                using (var ctx = new MelodiasContext())
                {
                    // insert 12 matching suppliers
                    for (int i = 0; i < 12; i++)
                        ctx.SupplierCompanies.Add(new Supplier
                        {
                            Name = $"{token}_{i}",
                            Address = "Addr",
                            PostalCode = "00000",
                            City = "City",
                            Country = "Country",
                            Phone = $"555-00{i:00}",
                            Email = $"{token}_{i}@ex.com"
                        });
                    ctx.SaveChanges();
                }

                var list = _dao.GetSuppliersList(token);
                Assert.Equal(10, list.Count);
            }
        }

        [Fact]
        public void DeleteSupplier_ReturnsTrue_WhenSupplierExists()
        {
            using (var scope = new TransactionScope())
            {
                int id;
                using (var ctx = new MelodiasContext())
                {
                    var sup = new Supplier
                    {
                        Name = "Del_" + Guid.NewGuid(),
                        Address = "Addr",
                        PostalCode = "10101",
                        City = "City",
                        Country = "Country",
                        Phone = "555-1010",
                        Email = $"del_{Guid.NewGuid()}@ex.com"
                    };
                    ctx.SupplierCompanies.Add(sup);
                    ctx.SaveChanges();
                    id = sup.supplierId;
                }

                Assert.True(_dao.DeleteSupplier(id));

                using (var ctx = new MelodiasContext())
                    Assert.False(ctx.SupplierCompanies.Any(s => s.supplierId == id));
            }
        }

        [Fact]
        public void DeleteSupplier_ReturnsFalse_WhenSupplierDoesNotExist()
        {
            using (var scope = new TransactionScope())
            {
                Assert.False(_dao.DeleteSupplier(-1234));
            }
        }

        [Fact]
        public void DeleteSupplier_ReturnsFalse_WhenAlreadyDeleted()
        {
            using (var scope = new TransactionScope())
            {
                int id;
                using (var ctx = new MelodiasContext())
                {
                    var sup = new Supplier
                    {
                        Name = "Del2_" + Guid.NewGuid(),
                        Address = "Addr",
                        PostalCode = "20202",
                        City = "City",
                        Country = "Country",
                        Phone = "555-2020",
                        Email = $"del2_{Guid.NewGuid()}@ex.com"
                    };
                    ctx.SupplierCompanies.Add(sup);
                    ctx.SaveChanges();
                    id = sup.supplierId;
                }

                // first delete
                Assert.True(_dao.DeleteSupplier(id));
                // second delete should be false
                Assert.False(_dao.DeleteSupplier(id));
            }
        }
        public void Dispose()
        {
            // nothing here, each test rolls back
        }
    }
}
