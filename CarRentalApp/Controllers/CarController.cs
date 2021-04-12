using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using CarRentalApp.Models;
using Newtonsoft.Json;

namespace CarRentalApp.Controllers
{
    [RoutePrefix("api/values")]
    public class CarController : ApiController
    {


        [HttpGet, HttpPost]
        public DataTable GetCarDetails(Car car)
        {
            string carId = "%";
            if (car != null)
            {
                if (car.carId != null)
                {
                    carId = car.carId;
                }

            }
            DataTable dataTable = new DataTable();
            string response;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
            con.Open();
            SqlCommand sqlCommand = new SqlCommand("Select *,categoryName as category from car_master cm join category_master c on cm.categoryId=c.categoryId where carId like '" + carId + "' and cm.carStatus=1", con);
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            da.Fill(dataTable);
            response = DataTableToJSONWithJSONNet(dataTable);
            return dataTable;
        }

        [HttpGet, HttpPost]
        public DataTable Gettop8(Car car)
        {
            string carId = "%";
            if (car != null)
            {
                if (car.carId != null)
                {
                    carId = car.carId;
                }

            }
            DataTable dataTable = new DataTable();
            string response;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
            con.Open();
            SqlCommand sqlCommand = new SqlCommand("Select top(8) *,categoryName as category,imageName=(select top 1 Imagename from carImages where carid=cm.carId) from car_master cm join category_master c on cm.categoryId=c.categoryId  where cm.carId like '" + carId + "' and cm.carStatus=1", con);
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            da.Fill(dataTable);
            response = DataTableToJSONWithJSONNet(dataTable);
            return dataTable;
        }


        [HttpGet, HttpPost]
        public DataTable GetCarImage(string carId)
        {

            DataTable dataTable = new DataTable();
            string response;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
            con.Open();
            SqlCommand sqlCommand = new SqlCommand("Select ImageName from carImages where carId='" + carId + "'", con);
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            da.Fill(dataTable);
            return dataTable;
        }

        [HttpPost]
        [ActionName("addCar")]
        public string addCar(Car car)
        {
            DataTable dataTable = new DataTable();
            string jsonResult;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
            con.Open();
            SqlCommand sqlCommand = new SqlCommand("insert into car_master (categoryId,carName,carModel,carMake,carDoors,carSeats,carAC,carType,rentalCharge,mileageAllowed,carStatus,dateAdded,addedBy,bufferDays) output INSERTED.carId values ('" + car.categoryId + "','" + car.carName + "','" + car.carModel + "','" + car.carMake + "','" + car.carDoors + "','" + car.carSeats + "','" + car.carAC + "','" + car.carType + "','" + car.rentalCharge + "','" + car.mileageAllowed + "','" + 1 + "','" + System.DateTime.Now.ToString() + "','" + car.addedBy + "','" + car.BufferDays + "')", con);
            int modified = (int)sqlCommand.ExecuteScalar();
            jsonResult = modified.ToString();
            return jsonResult;
        }

        public string addCarImages(List<string> images, string carId)
        {
            DataTable dataTable = new DataTable();
            string jsonResult = "";
            foreach (string x in images)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
                con.Open();
                SqlCommand sqlCommand = new SqlCommand("insert into carImages (carId,ImageName) values ('" + carId + "','" + x + "')", con);
                SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                da.Fill(dataTable);
                con.Close();
                con.Dispose();
            }
            return jsonResult;
        }


        [HttpPost]
        [ActionName("deleteCar")]
        public HttpResponseMessage deleteCar(string id)
        {
            string jsonResult = "";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
            con.Open();
            SqlCommand sqlCommand = new SqlCommand("delete from car_master where carId='" + id + "')", con);
            int i = sqlCommand.ExecuteNonQuery();

            return new HttpResponseMessage()
            {
                Content = new StringContent(jsonResult, Encoding.UTF8, "text/plain")
            };
        }

        [HttpPost]
        [ActionName("editCar")]
        public HttpResponseMessage editCar(Car car)
        {
            string jsonResult = "";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
            con.Open();
            SqlCommand sqlCommand = new SqlCommand("Update car_master set categoryId='" + car.categoryId + "',carName='" + car.carName + "',carMake='" + car.carMake + "',carDoors='" + car.carDoors + "',carSeats='" + car.carSeats + "',carAC='" + car.carAC + "',carType='" + car.carType + "',rentalCharge='" + car.rentalCharge + "',mileageAllowed='" + car.mileageAllowed + "',modifiedOn='" + System.DateTime.Now.ToString() + "',modifiedBy='" + car.modifiedBy + "',bufferDays='" + car.BufferDays + "' where carId='" + car.carId + "'", con);
            int i = sqlCommand.ExecuteNonQuery();
            return new HttpResponseMessage()
            {
                Content = new StringContent(jsonResult, Encoding.UTF8, "text/plain")
            };
        }

        public DataTable getCategory(Category category)
        {
            string categoryId = "%";
            if (category != null)
            {
                if (category.categoryId != null)
                {
                    categoryId = category.categoryId;
                }

            }
            DataTable dataTable = new DataTable();
            string response;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
            con.Open();
            SqlCommand sqlCommand = new SqlCommand("Select categoryId,categoryName from category_master  where categoryId like '" + categoryId + "'", con);
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            da.Fill(dataTable);
            response = DataTableToJSONWithJSONNet(dataTable);
            return dataTable;
        }

        public DataTable getAvailableCars(Booking book)
        {
            string categoryId = "%";
            if (book.categoryId != null)
            {
               
                    categoryId = book.categoryId;
               

            }
            DataTable dataTable = new DataTable();
            string response;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
            con.Open();
            SqlCommand sqlCommand = new SqlCommand("select *,imageName=(select top 1 Imagename from carImages where carid=cm.carId),categoryName=(select categoryName from category_master where categoryId=cm.categoryId) from car_master cm where carStatus=1 and categoryId='" + categoryId+"' and carId not in (select carid from Booking where pickUpdate between '"+book.pickUpDate+"' and '"+book.returnDate+ "' and Status='Booked')", con);
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            da.Fill(dataTable);
            response = DataTableToJSONWithJSONNet(dataTable);
            return dataTable;
        }

        [HttpPost]
        [ActionName("addCategory")]
        public HttpResponseMessage addCarCategory(Category category)
        {
            DataTable dataTable = new DataTable();
            string jsonResult;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
            con.Open();
            SqlCommand sqlCommand = new SqlCommand("insert into category_master (categoryName,dateAdded,addedBy) values ('" + category.CategoryName + "','" + System.DateTime.Now.ToString() + "','" + category.addedBy + "')", con);
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            da.Fill(dataTable);
            jsonResult = DataTableToJSONWithJSONNet(dataTable);
            return new HttpResponseMessage()
            {
                Content = new StringContent(jsonResult, Encoding.UTF8, "text/plain")
            };
        }

        [HttpPost]
        [ActionName("addUsers")]
        public HttpResponseMessage addUsers(Users user)
        {
            DataTable dataTable = new DataTable();
            string jsonResult;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
            con.Open();
            SqlCommand sqlCommand = new SqlCommand("insert into user_master (userName,password,phone,emailid,address,dateAdded,addedBy) values ('" + user.userName + "','" + user.password + "','" + user.phone + "','" + user.emailId + "','" + user.address + "','" + user.dateAdded + "','" + user.addedBy + "')", con);
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            da.Fill(dataTable);
            jsonResult = DataTableToJSONWithJSONNet(dataTable);
            return new HttpResponseMessage()
            {
                Content = new StringContent(jsonResult, Encoding.UTF8, "text/plain")
            };
        }

        [HttpPost]
        [ActionName("addBooking")]
        public HttpResponseMessage addBooking(Booking book,Customer customer)
        {
            DataTable dataTable = new DataTable();
            string jsonResult;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
            con.Open();
            SqlCommand sqlCommand = new SqlCommand("insert into Booking (carId,pickUpDate,returnDate,location) output INSERTED.bookingID values ('" + book.carId + "','" + book.pickUpDate + "','" + book.returnDate + "','" + book.location + "')", con);
            int bookingId = (int)sqlCommand.ExecuteScalar();
            con.Close();
            con.Open();
            sqlCommand = new SqlCommand("insert into customerinfo (firstName,lastName,emailid,phone,carId,bookingId,rentTotal,fees_taxes) values ('" + customer.firstName + "','" + customer.lastName + "','" + customer.emailId + "','" + customer.phone + "','" + customer.carId + "','" + bookingId + "','" + customer.rentTotal + "','" + customer.feesTaxes + "')", con);
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            da.Fill(dataTable);
            jsonResult = DataTableToJSONWithJSONNet(dataTable);
            return new HttpResponseMessage()
            {
                Content = new StringContent(jsonResult, Encoding.UTF8, "text/plain")
            };
        }

        [HttpPost]
        [ActionName("addRentalStatus")]
        public HttpResponseMessage addRentalStatus(Rental rent)
        {
            DataTable dataTable = new DataTable();
            string jsonResult;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
            con.Open();
            SqlCommand sqlCommand = new SqlCommand("insert into rental_status (carId,customerId,status,dateAdded,addedBy) values ('" + rent.carId + "','" + rent.customerId + "','" + rent.status + "','" + rent.dateAdded + "','" + rent.addedBy + "')", con);
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            da.Fill(dataTable);
            jsonResult = DataTableToJSONWithJSONNet(dataTable);
            return new HttpResponseMessage()
            {
                Content = new StringContent(jsonResult, Encoding.UTF8, "text/plain")
            };
        }

        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }



        //// GET: api/Car/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Car
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Car/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Car/5
        //public void Delete(int id)
        //{
        //}
    }
}
