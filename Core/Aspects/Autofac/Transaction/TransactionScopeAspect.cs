using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Core.Aspects.Autofac.Transaction
{
    //Yapılan işlem başarsızın olduğunda ona bağlı işlemleri geri almaya yarar.
    //örn bankadan para atıyorum benden parayı kesti gideceği yere gidemedi bana parayı iade etmeli
    //Kod tekrarından kaçınmak için aspect olarak yazıldı
    //bu sayede unit of work kullanmaya gerek kalmaz
    //nested transactions araştırılacak..
    public class TransactionScopeAspect : MethodInterception
    {
        public override void Intercept(IInvocation invocation)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    invocation.Proceed();
                    transactionScope.Complete();
                }
                catch (System.Exception e)
                {
                    transactionScope.Dispose();
                    throw;
                }
            }
        }
    }
}
