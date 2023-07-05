using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAccounts();

        Account GetAccountByEmail(string email);
    }
}
