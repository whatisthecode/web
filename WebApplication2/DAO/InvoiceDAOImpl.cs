using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class InvoiceDAOImpl: BaseImpl<Invoice, Int16>, InvoiceDAO, IDisposable
    {
        public InvoiceDAOImpl() : base()
        {

        }

        public bool check(string column)
        {
            return base.checkColumnExists(column);
        }

        public Invoice checkExist(Invoice invoice)
        {
            Invoice invoice1 = base.getContext().invoices.Where(c => c.code == invoice.code).FirstOrDefault();
            if (invoice1 != null)
            {
                return invoice1;
            }
            base.detach(invoice);
            return null;
        }

        public void deleteInvoice(Int16 InvoiceId)
        {
            base.delete(InvoiceId);
        }

        public void dispose()
        {
            base.Dispose();
        }

        public IEnumerable<Invoice> getInvoice()
        {
            return base.get();
        }

        public Invoice getInvoiceById(Int16 invoiceId)
        {
            return base.getById(invoiceId);
        }

        public void insertInvoice(Invoice invoice)
        {
            base.insert(invoice);
        }

        public PagedResult<Invoice> PageView(int pageIndex, int pageSize, string columnName)
        {
            var query = from c in base.getContext().invoices select c;
            switch (columnName)
            {
                case "name":
                    query = query.OrderBy(n => n.code);
                    break;
            }

            PagedResult<Invoice> pv = base.PageView(query, pageIndex, pageSize);
            return pv;
        }

        public void saveInvoice()
        {
            base.save();
        }

        public void updateInvoice(Invoice invoice)
        {
            base.update(invoice);
        }
    }
}