using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FileUploader
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void UploadBtn_Click(object sender, EventArgs e)
        {
            if (FileUpLoad1.HasFile)
            {

                FileUpLoad1.SaveAs(@"C:\Upload\" + FileUpLoad1.FileName);
                Label1.Text = "File Uploaded: " + FileUpLoad1.FileName;
            }
            else
            {
                Label1.Text = "No File Uploaded.";
            }
        }
    }
}