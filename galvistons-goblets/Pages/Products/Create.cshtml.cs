using galvistons_goblets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace galvistons_goblets.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _config;
        public CreateModel(IConfiguration config)
        {
            _config = config;
        }

        public ProductInfo productInfo = new ProductInfo();
        public String errorMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            productInfo.name = Request.Form["name"];
            productInfo.description = Request.Form["description"];
            productInfo.SKU = Request.Form["SKU"];

            if (productInfo.name.Length == 0 || productInfo.description.Length== 0
                || productInfo.SKU.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                String connectionString = _config["ConnectionStrings:GalvistonString"];
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO products " +
                        "(Name, Description, SKU) VALUES " +
                        "(@name, @description, @SKU);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
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

            productInfo.name = ""; productInfo.description = ""; productInfo.SKU = "";

            Response.Redirect("/Products/Index");

        }
    }
}
