using System.Net.Mail;
using System.Net;
using APIzza.DAO;
using APIzza.DTO;

namespace APIzza.Models
{
    public class EmailConfirmation
    {
        private IOrderDAO orderDAO;

        public EmailConfirmation(IOrderDAO _orderDAO)
        {
            this.orderDAO = _orderDAO;
        }

        public static void EmailNotifications(CartDto cart)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("APIzzabusiness@gmail.com");
            mail.To.Add(cart.CustomerOrder.Email);
            mail.Subject = $"Your {cart.CustomerOrder.OrderType}, confirmation status";
            mail.Body = $"<html><head><title>Order Confirmation</title>\r\n</head>\r\n<body style=\"margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;\">\r\n<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"400\" style=\"margin-top: 20px; background-color: #ffffff;\">\r\n<tr>\r\n<td align=\"center\" valign=\"top\" style=\"padding: 20px;\">\r\n<img src=\"https://i.imgur.com/6C86pTU.png\" alt=\"Pizza Hut\" style=\"width: 150px; margin-bottom: 20px;\">\r\n<h1 style=\"font-size: 20px; color: #333333; margin: 0;\">Order Confirmation</h1>\r\n<p style=\"font-size: 16px; color: #666666; margin: 10px 0;\">Thank you for placing an order with APIzza!</p>\r\n<p style=\"font-size: 16px; color: #666666; margin: 10px 0;\">Your order is confirmed.</p>\r\n<p style=\"font-size: 16px; color: #666666; margin: 10px 0;\">Order Number: #{cart.CustomerOrder.OrderId}</p>\r\n<p style=\"font-size: 16px; color: #666666; margin: 10px 0;\">Order Date: {DateTime.Now}</p>\r\n<p style=\"font-size: 16px; color: #666666; margin: 10px 0;\">Email: APIzzabusiness@gmail.com</p>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td align=\"center\" valign=\"top\" style=\"padding: 20px;\">\r\n<p style=\"font-size: 16px; color: #666666; margin: 10px 0;\">Thank you for choosing APIzza. We look forward to serving you!</p>\r\n</td>\r\n</tr>\r\n</table>\r\n</body>\r\n</html>\r\n";
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("APIzzabusiness@gmail.com", "uatatiidrpsldcrm");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        public void OrderStatus(int id)
        {
            //send order status through email
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("APIzzabusiness@gmail.com");
            mail.To.Add(orderDAO.GetEmail(id));
            mail.Subject = $"Your order status";
            mail.Body = $"Your order is: {orderDAO.GetOrderStatus(id)}\n";
            mail.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("APIzzabusiness@gmail.com", "uatatiidrpsldcrm");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}
