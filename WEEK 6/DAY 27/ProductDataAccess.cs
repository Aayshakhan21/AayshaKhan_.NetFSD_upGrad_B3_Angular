using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.IO;

namespace ConsoleApp2
{
    public class ProductDataAccess
    {
        private readonly string connstr;

public ProductDataAccess()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            connstr = config.GetConnectionString("DefaultConnection");
        }

        // get 
        public DataTable GetProducts()
        {
            using (SqlConnection con = new SqlConnection(connstr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM _Products", con);

                DataTable dt = new DataTable();
                da.Fill(dt); 

                return dt;
            }
        }

        public DataRow GetProductById(int id)
        {
            using (SqlConnection con = new SqlConnection(connstr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM _Products", con);

                DataTable dt = new DataTable();
                da.Fill(dt); // disconnected

                foreach (DataRow row in dt.Rows)
                {
                    if ((int)row["ProductId"] == id)
                    {
                        return row; // return matching row
                    }
                }

                return null; // not found
            }

}


        // insert product
        public void InsertProduct(Product p)
        {
            using (SqlConnection con = new SqlConnection(connstr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM _Products", con);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);

                DataTable dt = new DataTable();
                da.Fill(dt);

                DataRow row = dt.NewRow();
                row["ProductName"] = p.ProductName;
                row["Category"] = p.Category;
                row["Price"] = p.Price;

                dt.Rows.Add(row);

                da.Update(dt); // changes to DB
            }
        }

        // update product
        public void UpdateProduct(Product p)
        {
            using (SqlConnection con = new SqlConnection(connstr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM _Products", con);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);

                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    if ((int)row["ProductId"] == p.ProductId)
                    {
                        row["ProductName"] = p.ProductName;
                        row["Category"] = p.Category;
                        row["Price"] = p.Price;
                    }
                }

                da.Update(dt);
            }
        }

        // delete product
        public void DeleteProduct(int id)
        {
            using (SqlConnection con = new SqlConnection(connstr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM _Products", con);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);

                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    if ((int)row["ProductId"] == id)
                    {
                        row.Delete();
                        break;
                    }
                }

                da.Update(dt);
            }
        }

}

}
