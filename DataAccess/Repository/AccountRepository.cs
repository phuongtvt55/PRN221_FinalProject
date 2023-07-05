using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AccountRepository: IAccountRepository
    {
        public IEnumerable<Account> GetAccounts()
        {
            return AccountDAO.Instance.GetAccountList();
        }

        public Account GetAccountByEmail(string email)
        {
            return AccountDAO.Instance.GetAccountByEmail(email);
        }
    }
}
