using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Infrastructure.Helpers
{
    public static class EmailBuilder
    {
        public static (string subject, string textBody, string htmlBody) Build(bool isNew, User vendor, User user)
        {
            var subject = isNew ? "New customer assigned" : "Customer updated";
            var textBody = isNew ? $"You have a new customer" : $"Customer {user.Id} has been updated";

            var htmlBody = @$"
                <p>Hello {vendor.Name},</p>

                <p>{textBody}</p>


                <table style='border-collapse: collapse;'>
                    <tr>
                        <td><b>Name</b></td>
                        <td>{user.Name}</td>
                    </tr>
                    <tr>
                        <td><b>Title</b></td>
                        <td>{user.Title}</td>
                    </tr>
                    <tr>
                        <td><b>Email</b></td>
                        <td>{user.Email}</td>
                    </tr>
                    <tr>
                        <td><b>Phone</b></td>
                        <td>{user.PhoneNumber}</td>
                    </tr>
                </table>

                <table style='border-collapse: collapse;'>
                    <caption style='text-align: left;'><b>Address:</b></caption>
                    <tr>
                        <td><b>Street</b></td>
                        <td>{user.Address.Street}</td>
                    </tr>
                    <tr>
                        <td><b>City</b></td>
                        <td>{user.Address.City}</td>
                    </tr>
                    <tr>
                        <td><b>Postal code</b></td>
                        <td>{user.Address.PostalCode}</td>
                    </tr>
                    <tr>
                        <td><b>Country</b></td>
                        <td>{user.Address.Country}</td>
                    </tr>
                </table>
                ";

            return (subject, textBody, htmlBody);
        }
    }
}