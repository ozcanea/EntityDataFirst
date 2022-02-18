using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20220216_Entity
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DBFirst db = new DBFirst();
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("1. QUERY");
            comboBox1.Items.Add("2. QUERY");
            comboBox1.Items.Add("3. QUERY");
            comboBox1.Items.Add("4. QUERY");
            comboBox1.Items.Add("5. QUERY");
            comboBox1.Items.Add("6. QUERY");
            comboBox1.SelectedIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    //LINQ to Entity
                    //dataGridView1.DataSource = db
                    //                        .Products
                    //                        .Where(x => x.UnitPrice > 20 && x.UnitPrice < 50)
                    //                        .OrderByDescending(x => x.UnitPrice)
                    //                        .Join(db.Categories, x => x.CategoryID, c=>c.CategoryID, (x,c)=> new {Product=x,Category=c} )
                    //                        .Select(x=>new { x.Product.ProductID,x.Product.ProductName,x.Product.UnitPrice,x.Product.UnitsInStock,x.Category.CategoryName }).ToList();
                    //LINQ to SQL
                    var result = from p in db.Products
                                 where p.UnitPrice > 20 && p.UnitPrice < 50
                                 orderby p.UnitPrice descending
                                 select new
                                 {
                                     UrunID = p.ProductID,
                                     UrunAdi = p.ProductName,
                                     Fiyat = p.UnitPrice,
                                     Kategori = p.Category.CategoryName
                                 };
                    dataGridView1.DataSource = result.ToList();
                    break;
                    case 1:
                        dataGridView1.DataSource=db.Orders
                                                  .Select(x=> new
                                                  {EmployeeName=x.Employee.FirstName+" "+x.Employee.LastName,x.Customer.CompanyName,x.OrderID,x.OrderDate}).ToList();
                    //LINQ to SQL
                    //var reslt = from o in db.Orders
                    //            select new
                    //            {
                    //                EmployeeName = o.Employee.FirstName + " " + o.Employee.LastName,
                    //                o.Customer.CompanyName,
                    //                o.OrderID,
                    //                o.OrderDate,
                    //            };
                    break;
                    case 2:
                        //dataGridView1.DataSource=db.Customers
                        //                         .Where(x=>x.CompanyName.Contains("restauran"))
                        //                         .Select(x=> new
                        //                         {x.CompanyName}).ToList();
                        var result2 = from c in db.Customers
                                     where c.CompanyName.Contains("restaurant")
                                     select c;
                    dataGridView1.DataSource = result2.ToList();
                    break;
                    case 3:
                    Product objProduct = new Product();
                    //int categoryID = db.Categories.FirstOrDefault(x => x.CategoryName == "Seafoods").CategoryID;
                    //objProduct.ProductName = "Kola";
                    //objProduct.UnitPrice = 5;
                    //objProduct.UnitsInStock = 500;
                    //objProduct.CategoryID = categoryID;
                    //db.Products.Add(objProduct);
                    //db.SaveChanges();
                    ////////////////************************************************/////////
                    db.Products.Add(new Product
                    {
                        ProductName = "Kola",
                        UnitPrice = 5,
                        UnitsInStock = 500,
                        CategoryID = db.Categories.Single(x => x.CategoryName == "Seafoods").CategoryID
                    });

                    db.SaveChanges();
                    dataGridView1.DataSource = db.Products.ToList();

                    //////3.yol
                    //db.Categories.FirstOrDefault(c => c.CategoryName == "Beverages").Products.Add(new Product
                    //{
                    //    ProductName = "Kola",
                    //    UnitPrice = 5,
                    //    UnitsInStock = 500,
                    //});
                    //db.SaveChanges();

                    //var result3= from c in db.Categories
                    //             where c.CategoryName== "Beverages"
                    //             select c.CategoryID;



                        break;
                    case 4:
                     dataGridView1.DataSource=db.Employees
                                               .Select(x => new {x.FirstName,x.LastName,x.BirthDate,Yas=SqlFunctions.DateDiff("Year",x.BirthDate,DateTime.Now)}).ToList();

                    //////////////////////////////////////////////////
                    //var result4 = from ee in db.Employees
                    //              select new {ee.FirstName,ee.LastName,ee.BirthDate,yas=SqlFunctions.DateDiff("year",ee.BirthDate,DateTime.Now)};
                    //dataGridView1.DataSource = result4.ToList();
                    break;
                    case 5:
                        dataGridView1.DataSource = db.Products.GroupBy(p=>p.Category.CategoryName).Select( a=> new
                        {
                            CategoryName = a.Key,
                            TotalStock= a.Sum(p=>p.UnitsInStock)
                        }).Where(x=>x.CategoryName!=null).ToList();
                        
                    ////////////////////////////////
                    var resultt = from p in db.Products
                                  where p.CategoryID!=null
                                  group p by p.Category.CategoryName into g
                                  select new
                                  {
                                      CategoryName = g.Key,
                                      TotalStock = g.Sum(p => p.UnitsInStock)
                                  };

                    break;
            }
        }
    }
}
