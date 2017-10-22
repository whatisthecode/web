using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LaptopWebsite.Models.Mapping;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class InvoiceDetailDAOImpl : BaseImpl<InvoiceDetail, Int16>, InvoiceDetailDAO, IDisposable
    {
        public bool check(string column)
        {
            throw new NotImplementedException();
        }

        public InvoiceDetail checkExist(InvoiceDetail invoicedetail)
        {
            throw new NotImplementedException();
        }

        public void deleteInvoiceDetail(short InvoiceDetailId)
        {
            throw new NotImplementedException();
        }

        public void dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InvoiceDetail> getInvoiceDetail()
        {
            throw new NotImplementedException();
        }

        public InvoiceDetail getInvoiceDetailById(short invoiceDetailId)
        {
            throw new NotImplementedException();
        }

        public void insertInvoiceDetail(InvoiceDetail invoicedetail)
        {
            throw new NotImplementedException();
        }

        public PagedResult<InvoiceDetail> PageView(int pageIndex, int pageSize, string columnName)
        {
            throw new NotImplementedException();
        }

        public void saveInvoiceDetail()
        {
            throw new NotImplementedException();
        }

        public void updateInvoiceDetail(InvoiceDetail invoicedetail)
        {
            throw new NotImplementedException();
        }
    }
}