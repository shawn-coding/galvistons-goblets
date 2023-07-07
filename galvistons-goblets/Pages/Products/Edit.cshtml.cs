using galvistons_goblets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace galvistons_goblets.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _config;
        public EditModel(IConfiguration config)
        {
            _config = config;
        }

        public ProductInfo productInfo = new ProductInfo();

        public String errorMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = _config["ConnectionStrings:GalvistonString"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM products WHERE ProductID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                productInfo.id = "" + reader.GetInt32(0);
                                productInfo.name = reader.GetString(1);
                                productInfo.description = reader.GetString(2);
                                productInfo.SKU = reader.GetString(3);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
        public void OnPost()
        {
            productInfo.id = Request.Form["id"];
            productInfo.name = Request.Form["name"];
            productInfo.description = Request.Form["description"];
            productInfo.SKU = Request.Form["SKU"];

            if (productInfo.name.Length == 0 || productInfo.description.Length == 0
                || productInfo.SKU.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                String connectionString = _config["ConnectionStrings:GalvistonString"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE products " +
                        "SET Name=@name, Description=@description, SKU=@SKU " +
                        "WHERE ProductID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", productInfo.id);
                        command.Parameters.AddWithValue("@name", productInfo.name);
                        command.Parameters.AddWithValue("@description", productInfo.description);
                        command.Parameters.AddWithValue("@SKU", productInfo.SKU);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Products/Index");
        }
    }
}
