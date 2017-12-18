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
        public InvoiceDetailDAOImpl() : base()
        {

        }
        public bool check(string column)
        {
            return base.checkColumnExists(column);
        }

        public InvoiceDetail checkExist(InvoiceDetail invoicedetail)
        {
            throw new NotImplementedException();
        }

        public void deleteInvoiceDetail(short InvoiceDetailId)
        {
            base.delete(InvoiceDetailId);
        }

        public void dispose()
        {
            base.Dispose();
        }

        public IEnumerable<InvoiceDetail> getInvoiceDetail()
        {
            return base.get().ToList();
        }

        public InvoiceDetail getInvoiceDetailById(short invoiceDetailId)
        {
            return base.getById(invoiceDetailId);
        }

        public void insertInvoiceDetail(InvoiceDetail invoicedetail)
        {
            base.insert(invoicedetail);
        }

        public PagedResult<InvoiceDetail> PageView(int pageIndex, int pageSize, string columnName)
        {
            throw new NotImplementedException();
        }

        public void updateInvoiceDetail(InvoiceDetail invoicedetail)
        {
            base.update(invoicedetail);
        }

        public IEnumerable<InvoiceDetail> getListDetailByInvoiceId(short invoiceId)
        {
            using (DBContext context = new DBContext())
            {
                return context.invoiceDetails.Where(inv => inv.invoice.Equals(invoiceId)).ToList();
            }     

        }
    }
}