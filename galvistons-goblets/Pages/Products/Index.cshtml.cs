using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using galvistons_goblets.Models;
using Microsoft.VisualBasic;

namespace galvistons_goblets.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;

        public IndexModel(IConfiguration config)
        {
            _config = config;
        }

        public List<ProductInfo> listProducts = new List<ProductInfo>();

        
        public void OnGet()
        {
            try
            {

                String connectionString = _config["ConnectionStrings:GalvistonString"];
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM products";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                ProductInfo info = new ProductInfo();
                                info.id = "" + reader.GetInt32(0);
                                info.name = reader.GetString(1);
                                info.description = reader.GetString(2);
                                info.SKU = reader.GetString(3);

                                listProducts.Add(info);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
    }



}
