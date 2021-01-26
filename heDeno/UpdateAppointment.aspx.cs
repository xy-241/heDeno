﻿using heDeno.MyDenoDBServiceReference;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace heDeno
{
    public partial class UpdateAppointment : System.Web.UI.Page
    {
        int appointment_id = 0;
        string clinicId, clinicType, doctorId;
        string startDateTime, endDateTime, date;

        string DBConnect = Environment.GetEnvironmentVariable("MyDenoDB").ToString();
        MySqlConnection myConn = null;

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            DateTime u_date = DateTime.ParseExact(select_date.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            var u_startTime = DateTime.ParseExact(select_start_time.Text, "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            var u_endTime = DateTime.ParseExact(select_end_time.Text, "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            DateTime u_startDateTime = new DateTime(u_date.Year, u_date.Month, u_date.Day, u_startTime.Hour, u_startTime.Minute, u_startTime.Second);
            DateTime u_endDateTime = new DateTime(u_date.Year, u_date.Month, u_date.Day, u_endTime.Hour, u_endTime.Minute, u_endTime.Second);

            MyDenoDBServiceReference.Service1Client client = new MyDenoDBServiceReference.Service1Client();
            int update = client.UpdateAppointment(appointment_id, u_startDateTime, u_endDateTime, int.Parse(select_doctor.SelectedValue), 1);

            if (update == 1)
            {
                Response.Redirect("AppointmentSuccess");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var s_aid = (string)Session["Appointment_id"];
            if (s_aid != null)
            {
                appointment_id = int.Parse(Session["Appointment_id"].ToString());
            }

            retreiveData();

            select_date.Attributes["min"] = DateTime.Today.AddDays(5).ToString("yyyy-MM-dd");


            if (!IsPostBack && appointment_id > 0)
            {
                try
                {
                    List<Specialty> specialtyList = new List<Specialty>();
                    MyDenoDBServiceReference.Service1Client client = new MyDenoDBServiceReference.Service1Client();
                    specialtyList = client.GetAllSpecialty().ToList<Specialty>();

                    select_specialty.DataSource = specialtyList;
                    select_specialty.DataTextField = "specialtyFull";
                    select_specialty.DataValueField = "specialtyName";
                    select_specialty.DataBind();

                    select_specialty.Items.FindByValue(clinicType).Selected = true;
                    

                    /**select_specialty.Items.Insert(0, new ListItem("-- Select Specialty --", ""));
                    select_clinic.Items.Insert(0, new ListItem("-- Select Clinic --", ""));
                    select_doctor.Items.Insert(0, new ListItem("-- Select Preferred Doctor --", ""));**/
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }


                string selected_specialty = select_specialty.SelectedValue;

                try
                {
                    List<Clinic> clinicList = new List<Clinic>();
                    MyDenoDBServiceReference.Service1Client client = new MyDenoDBServiceReference.Service1Client();
                    clinicList = client.GetClinicBySpecialty(selected_specialty).ToList<Clinic>();

                    select_clinic.DataSource = clinicList;
                    select_clinic.DataTextField = "clinicName";
                    select_clinic.DataValueField = "id";
                    select_clinic.DataBind();

                    select_clinic.Items.FindByValue(clinicId).Selected = true;
                    /** select_clinic.Items.Insert(0, new ListItem("-- Select Clinic --", "")); **/
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }

                string selected_clinic = select_clinic.SelectedValue;

                try
                {
                    myConn = new MySqlConnection(DBConnect);
                    string sqlstmt = string.Format("SELECT * FROM doctor INNER JOIN clinic ON doctor.clientId = clinic.id WHERE clinic.id = @clinic_id");
                    MySqlCommand command = new MySqlCommand(sqlstmt, myConn);
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@clinic_id", selected_clinic);

                    MySqlDataAdapter da = new MySqlDataAdapter();
                    da.SelectCommand = command;
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    //Custom text in dropdownlist
                    dt.Columns.Add("doctorFull", typeof(String), "firstName + ' ' + lastName");

                    select_doctor.DataSource = dt;
                    select_doctor.DataTextField = "doctorFull";
                    select_doctor.DataValueField = "id";
                    select_doctor.DataBind();
                    
                    
                    select_doctor.Items.FindByValue(doctorId).Selected = true;
                    /**select_doctor.Items.Insert(0, new ListItem("-- Select Preferred Doctor --", ""));**/
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    myConn.Close();
                }

                select_date.Text = date;
                select_start_time.Text = startDateTime;
                select_end_time.Text = endDateTime;
            }
        }

        protected void select_specialty_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected_specialty = select_specialty.SelectedValue;

            try
            {
                List<Clinic> clinicList = new List<Clinic>();
                MyDenoDBServiceReference.Service1Client client = new MyDenoDBServiceReference.Service1Client();
                clinicList = client.GetClinicBySpecialty(selected_specialty).ToList<Clinic>();

                select_clinic.DataSource = clinicList;
                select_clinic.DataTextField = "clinicName";
                select_clinic.DataValueField = "id";
                select_clinic.DataBind();

                select_clinic.Items.Insert(0, new ListItem("-- Select Clinic --", ""));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        protected void select_clinic_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected_clinic = select_clinic.SelectedValue;

            try
            {
                myConn = new MySqlConnection(DBConnect);
                string sqlstmt = string.Format("SELECT * FROM doctor INNER JOIN clinic ON doctor.clientId = clinic.id WHERE clinic.id = @clinic_id");
                MySqlCommand command = new MySqlCommand(sqlstmt, myConn);
                command.Parameters.AddWithValue("@clinic_id", selected_clinic);

                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();
                da.Fill(dt);

                //Custom text in dropdownlist
                dt.Columns.Add("doctorFull", typeof(String), "firstName + ' ' + lastName");

                select_doctor.DataSource = dt;
                select_doctor.DataTextField = "doctorFull";
                select_doctor.DataValueField = "id";
                select_doctor.DataBind();

                select_doctor.Items.Insert(0, new ListItem("-- Select Preferred Doctor --", ""));            
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            finally
            {
                myConn.Close();
            }

            try
            {
                MyDenoDBServiceReference.Service1Client client = new MyDenoDBServiceReference.Service1Client();
                var clinic = client.GetOneClinic(select_clinic.SelectedItem.Text);
                select_start_time.Attributes["min"] = clinic.StartTime.ToString();
                select_start_time.Attributes["max"] = clinic.EndTime.ToString();
                select_end_time.Attributes["max"] = clinic.EndTime.ToString();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        protected void retreiveData()
        {
            try
            {
                MyDenoDBServiceReference.Service1Client client = new MyDenoDBServiceReference.Service1Client();
                Appointment appointmentObj = client.GetOneAppointment(appointment_id);
                if(appointmentObj !=null) 
                {
                    clinicType = appointmentObj.clinicType;
                    clinicId = appointmentObj.clinicId.ToString();
                    doctorId = appointmentObj.doctorId.ToString();
                    var a = appointmentObj.startDateTime;
                    date = a.ToString("yyyy-MM-dd");
                    startDateTime = a.ToString("HH:mm");
                    var b = appointmentObj.endDateTime;
                    endDateTime = b.ToString("HH:mm");
                } else
                {
                    lbl_msg.ForeColor = System.Drawing.Color.Red;
                    lbl_msg.Text = "NO DATA FOUND";
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}