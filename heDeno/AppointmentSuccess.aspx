﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AppointmentSuccess.aspx.cs" Inherits="heDeno.AppointmentSuccess" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <section class="vertical-middle-landing">
        <div class="box-style box-desktop-900 item-center">
            <h4 class="montserrat grey-blue-second medium-font" style="margin-bottom:20px;">Appointment Successfully Booked! Please check you email regarding your appointment status</h4>
            <img src="https://res.cloudinary.com/xenonic/image/upload/v1611055985/hedeno/check-circle_wgy7n7.svg" alt="Success Logo" />
            <br />
            <asp:Button runat="server" Text="Back to Appointments" cssClass="montserrat rounded-full standard-btn btn-standard-width y-gap" ID="btn_backHome" OnClick="btn_backHome_Click"/>
        </div>
    </section>
</asp:Content>
