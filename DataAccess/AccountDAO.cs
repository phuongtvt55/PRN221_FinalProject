using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AccountDAO
    {
        private static AccountDAO instance = null;
        private static readonly object instanceLock = new object();
        public static AccountDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new AccountDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Account> GetAccountList()
        {
            var account = new List<Account>();
            try
            {
                using var context = new ShoesStoreContext();
                account = context.Accounts.ToList();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            return account;
        }

        public Account GetAccountByEmail(string email)
        {
            Account account = null;
            try
            {
                using var context = new ShoesStoreContext();
                account = context.Accounts.SingleOrDefault(c => c.Email == email);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return account;
        }
    }
}
